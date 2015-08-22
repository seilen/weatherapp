# Personal Weather Dashboard version 1.0.3
# 2013-2014 - JF Nutbroek <jfnutbroek@gmail.com>
# Based on Ambient weewx restful.py by Tom Keffer <tkeffer@gmail.com>

"""weewx module that uploads weather data to a personal website

   Copy this file into the bin/weewx directory, then add this to weewx.conf:

    [StdRESTful]
        ...
    [[PWdashboard]]
        station = stationid (e.g., "WMR8800001") - No spaces in name, only a-Z, 0-9!
        password = password (e.g., "ABCdef1234") - No spaces in password, only a-Z, 0-9! 
        server_url = URL to your webservers PHP API (e.g., "http://www.site.com/pwdashboard.php?") 
        driver = weewx.pwdashboard.PWdashboard
        
"""

import httplib
import socket
import syslog
import urllib
import urllib2
import datetime

import weewx
import weewx.restful

#===============================================================================
#                       class Personal Weather using Ambient
#===============================================================================

class PWdashboard(weewx.restful.REST):
    """Upload using the Ambient protocol for a personal weather database. 
    
    For details of the Ambient upload protocol,
    see http://wiki.wunderground.com/index.php/PWS_-_Upload_Protocol
    
    For details on how urllib2 works, see "urllib2 - The Missing Manual"
    at http://www.voidspace.org.uk/python/articles/urllib2.shtml
    """

    # Types and formats of the data to be published:
    _formats = {'dateTime'    : 'dateutc=%i',
                'barometer'   : 'baromin=%.3f',
                'outTemp'     : 'tempf=%.1f',
                'outHumidity' : 'humidity=%03.0f',
                'windSpeed'   : 'windspeedmph=%03.0f',
                'windDir'     : 'winddir=%03.0f',
                'windGust'    : 'windgustmph=%03.0f',
                'dewpoint'    : 'dewptf=%.1f',
                'hourRain'    : 'rainin=%.2f',
                'dayRain'     : 'dailyrainin=%.2f',
                'radiation'   : 'solarradiation=%.2f',
                'UV'          : 'UV=%.2f'}


    def __init__(self, site, **kwargs):
        """Initialize for a given upload site.
        
        site: The upload site ('PWdashboard')
        
        station: The name of the station (e.g., "WMR8800001") as a string [Required]
        
        password: Password for the station [Required]
        
        server_url: The URL of the upload point [Required]
        
        max_tries: Max # of tries before giving up [Optional. Default is 3]"""
        
        self.site        = site
        self.station     = kwargs['station']
        self.password    = kwargs['password']
        self.server_url  = kwargs.get('server_url', 'http://localhost')        
        self.max_tries   = int(kwargs.get('max_tries', 3))

    def postData(self, archive, time_ts):
        """Post using the Ambient HTTP protocol

        archive: An instance of weewx.archive.Archive
        
        time_ts: The record desired as a unix epoch time."""
        
        _url = self.getURL(archive, time_ts)

        # Retry up to max_tries times:
        for _count in range(self.max_tries):
            # Now use an HTTP GET to post the data. Wrap in a try block
            # in case there's a network problem.
            try:
                _response = urllib2.urlopen(_url)
            except (urllib2.URLError, socket.error, httplib.BadStatusLine), e:
                # Unsuccessful. Log it and go around again for another try
                syslog.syslog(syslog.LOG_ERR, "restful: Failed attempt #%d to upload to %s" % (_count+1, self.site))
                syslog.syslog(syslog.LOG_ERR, "   ****  Reason: %s" % (e,))
            else:
                # No exception thrown, but we're still not done.
                # We have to also check for a bad station ID or password.
                # It will have the error encoded in the return message:
                for line in _response:
                    # PWSweather signals with 'ERROR', WU with 'INVALID':
                    if line.startswith('ERROR') or line.startswith('INVALID'):
                        # Bad login. No reason to retry. Log it and raise an exception.
                        syslog.syslog(syslog.LOG_ERR, "restful: %s returns %s. Aborting." % (self.site, line))
                        raise FailedPost, line
                # Does not seem to be an error. We're done.
                return
        else:
            # This is executed only if the loop terminates normally, meaning
            # the upload failed max_tries times. Log it.
            syslog.syslog(syslog.LOG_ERR, "restful: Failed to upload to %s" % self.site)
            raise IOError, "Failed upload to site %s after %d tries" % (self.site, self.max_tries)

    def getURL(self, archive, time_ts):

        """Return an URL for posting using the Ambient protocol.
        
        archive: An instance of weewx.archive.Archive
        
        time_ts: The record desired as a unix epoch time.
        """
    
        record = self.extractRecordFrom(archive, time_ts)
        
        _liststr = ["action=updateraw", "ID=%s" % self.station, "PASSWORD=%s" % self.password ]
        
        # Go through each of the supported types, formatting it, then adding to _liststr:
        for _key in PWdashboard._formats:
            v = record[_key]
            # Check to make sure the type is not null - upload timestamp as UNIX Epoch
            if v is not None :
                # Format the value, and accumulate in _liststr:
                _liststr.append(PWdashboard._formats[_key] % v)
        # Add the software type and version:
        _liststr.append("softwaretype=weewx-%s" % weewx.__version__)
        # Now stick all the little pieces together with an ampersand between them:
        _urlquery='&'.join(_liststr)
        # This will be the complete URL for the HTTP GET:
        _url=self.server_url + _urlquery
        return _url
    