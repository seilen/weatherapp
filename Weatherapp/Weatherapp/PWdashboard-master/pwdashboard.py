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
import weewx.units
import weewx.archive
import weeutil.weeutil

#
#    Copyright (c) 2009, 2010, 2011, 2012 Tom Keffer <tkeffer@gmail.com>
#
#    See the file LICENSE.txt for your full rights.
#
#    $Revision: 772 $
#    $Author: tkeffer $
#    $Date: 2012-12-31 18:04:28 -0800 (Mon, 31 Dec 2012) $
#
"""Classes and functions for interfacing with a weewx archive."""
from __future__ import with_statement
import math
import syslog

from weewx.units import ValueTuple
import weewx.units
import weeutil.weeutil
import weedb
import user.schemas

#===============================================================================
#                         class Archive
#===============================================================================

class Archive(object):
    """Manages a weewx archive file. Offers a number of convenient member functions
    for managing the archive file. These functions encapsulate whatever sql statements
    are needed.
    
    ATTRIBUTES
    
    sqlkeys: A list of the SQL keys that the database supports.
    
    std_unit_system: The unit system used by the database."""
    
    def __init__(self, connection, table='archive'):
        """Initialize an object of type weewx.Archive. 
        
        If the database is uninitialized, an exception of type weewx.UninitializedDatabase
        will be raised. 
        
        connection: A weedb connection to the archive database.
        
        table: The name of the table to be used within the database
        """
        self.connection = connection
        self.table = table
        try:
            self.sqlkeys = self._getTypes()
        except weedb.OperationalError, e:
            self.close()
            raise weewx.UninitializedDatabase(e)
        
        # Fetch the first row in the database to determine the unit system in
        # use. If the database has never been used, then the unit system is
        # still indeterminate --- set it to 'None'.
        _row = self.getSql("SELECT usUnits FROM %s LIMIT 1;" % self.table)
        self.std_unit_system = _row[0] if _row is not None else None

    @staticmethod
    def open(archive_db_dict, table='archive'):
        """Open an Archive database.
        
        An exception of type weedb.OperationalError will be raised if the
        database does not exist.
        
        An exception of type StandardError will be raised if the database
        exists, but has not been initialized.
        
        Returns:
        An instance of Archive."""
        
        _connect = weedb.connect(archive_db_dict)
        return Archive(_connect, table)
    
    @staticmethod
    def open_with_create(archive_db_dict, archiveSchema, table='archive'):
        """Open an Archive database, initializing it if necessary.
        
        archive_db_dict: A database dictionary holding the information necessary
        to open the database.
        
        archiveSchema: The schema to be used
        
        table: The name of the table to be used within the database.
        
        Returns: 
        An instance of Archive""" 
    
        try:
            archive = Archive.open(archive_db_dict, table)
            # The database exists and has been initialized. Return it.
            return archive
        except (weedb.OperationalError, weewx.UninitializedDatabase):
            pass

        _connect = Archive._create_table(archive_db_dict, archiveSchema, table)        
        return Archive(_connect, table)
    
    @property
    def database(self):
        return self.connection.database
    
    def close(self):
        self.connection.close()

    def __enter__(self):
        return self
    
    def __exit__(self, etyp, einst, etb):
        self.close()    
    
    def lastGoodStamp(self):
        """Retrieves the epoch time of the last good archive record.
        
        returns: Time of the last good archive record as an epoch time, or
        None if there are no records."""
        _row = self.getSql("SELECT MAX(dateTime) FROM %s" % self.table)
        return _row[0] if _row else None
    
    def firstGoodStamp(self):
        """Retrieves earliest timestamp in the archive.
        
        returns: Time of the first good archive record as an epoch time, or
        None if there are no records."""
        _row = self.getSql("SELECT MIN(dateTime) FROM %s" % self.table)
        return _row[0] if _row else None

    def addRecord(self, record_obj, log_level=syslog.LOG_NOTICE):
        """Commit a single record or a collection of records to the archive.
        
        record_obj: Either a data record, or an iterable that can return data
        records. Each data record must look like a dictionary, where the keys
        are the SQL types and the values are the values to be stored in the
        database."""
        
        # Determine if record_obj is just a single dictionary instance (in which
        # case it will have method 'keys'). If so, wrap it in something iterable
        # (a list):
        record_list = [record_obj] if hasattr(record_obj, 'keys') else record_obj

        with weedb.Transaction(self.connection) as cursor:

            for record in record_list:
    
                if record['dateTime'] is None:
                    syslog.syslog(syslog.LOG_ERR, "Archive: archive record with null time encountered.")
                    raise weewx.ViolatedPrecondition("Archive record with null time encountered.")

                # Check to make sure the incoming record is in the same unit system as the
                # records already in the database:
                if self.std_unit_system:
                    if record['usUnits'] != self.std_unit_system:
                        raise ValueError("Unit system of incoming record (0x%x) "\
                                         "differs from the archive database (0x%x)" % (record['usUnits'], self.std_unit_system))
                else:
                    # This is the first record. Remember the unit system to check
                    # against subsequent records:
                    self.std_unit_system = record['usUnits']

                # Only data types that appear in the database schema can be inserted.
                # To find them, form the intersection between the set of all record
                # keys and the set of all sql keys
                record_key_set = set(record.keys())
                insert_key_set = record_key_set.intersection(self.sqlkeys)
                # Convert to an ordered list:
                key_list = list(insert_key_set)
                # Get the values in the same order:
                value_list = [record[k] for k in key_list]
                
                # This will a string of sql types, separated by commas. Because
                # some of the weewx sql keys (notably 'interval') are reserved
                # words in MySQL, put them in backquotes.
                k_str = ','.join(["`%s`" % k for k in key_list])
                # This will be a string with the correct number of placeholder question marks:
                q_str = ','.join('?' * len(key_list))
                # Form the SQL insert statement:
                sql_insert_stmt = "INSERT INTO %s (%s) VALUES (%s)" % (self.table, k_str, q_str) 
                try:
                    cursor.execute(sql_insert_stmt, value_list)
                    syslog.syslog(log_level, "Archive: added %s record %s" % (self.table, weeutil.weeutil.timestamp_to_string(record['dateTime'])))
                except Exception, e:
                    syslog.syslog(syslog.LOG_ERR, "Archive: unable to add archive record %s" % weeutil.weeutil.timestamp_to_string(record['dateTime']))
                    syslog.syslog(syslog.LOG_ERR, " ****    Reason: %s" % e)

    def genBatchRecords(self, startstamp=None, stopstamp=None):
        """Generator function that yields records with timestamps within an interval.
        
        startstamp: Exclusive start of the interval in epoch time. If 'None', then
        start at earliest archive record.
        
        stopstamp: Inclusive end of the interval in epoch time. If 'None', then
        end at last archive record.
        
        yields: A dictionary record for each database record within the time interval """
        
        try:
            _cursor = self.connection.cursor()
            if startstamp is None:
                if stopstamp is None:
                    _gen = _cursor.execute("SELECT * FROM %s" % (self.table,))
                else:
                    _gen = _cursor.execute("SELECT * FROM %s WHERE dateTime <= ?" % (self.table,), (stopstamp,))
            else:
                if stopstamp is None:
                    _gen = _cursor.execute("SELECT * FROM %s WHERE dateTime > ?" % (self.table,), (startstamp,))
                else:
                    _gen = _cursor.execute("SELECT * FROM %s WHERE dateTime > ? AND dateTime <= ?" % (self.table,), (startstamp, stopstamp))
            
            for _row in _gen :
                yield dict(zip(self.sqlkeys, _row)) if _row else None
        finally:
            _cursor.close()
        
    def getRecord(self, timestamp):
        """Get a single archive record with a given epoch time stamp.
        
        timestamp: The epoch time of the desired record.
        
        returns: a record dictionary or None if the record does not exist."""

        _cursor = self.connection.cursor()
        try:
            _cursor.execute("SELECT * FROM %s WHERE dateTime=?" % (self.table,), (timestamp,))
            _row = _cursor.fetchone()
            return dict(zip(self.sqlkeys, _row)) if _row else None
        finally:
            _cursor.close()

    def getSql(self, sql, sqlargs=()):
        """Executes an arbitrary SQL statement on the database.
        
        sql: The SQL statement
        
        sqlargs: A tuple containing the arguments for the SQL statement
        
        returns: a tuple containing the results
        """
        _cursor = self.connection.cursor()
        try:
            _cursor.execute(sql, sqlargs)
            return _cursor.fetchone()
        finally:
            _cursor.close()

    def genSql(self, sql, sqlargs=()):
        """Generator function that executes an arbitrary SQL statement on the database."""
        
        try:
            _cursor = self.connection.cursor()
            
            for _row in _cursor.execute(sql, sqlargs):
                yield _row
        finally:
            _cursor.close()
            
    def getSqlVectors(self, sql_type, startstamp, stopstamp,
                      aggregate_interval=None, 
                      aggregate_type=None):
        """Get time and (possibly aggregated) data vectors within a time interval. 
        
        The return value is a 2-way tuple. The first member is a vector of time
        values, the second member an instance of weewx.std_unit_system.Value with a
        value of a vector of data values, and a unit_type given by sql_type. 
        
        An example of a returned value is: (time_vec, Value(outTempVec, 'outTemp')). 
        
        If aggregation is desired (aggregate_interval is not None), then each element represents
        a time interval exclusive on the left, inclusive on the right. The time
        elements will all fall on the same local time boundary as startstamp. 
        For example, if startstamp is 8-Mar-2009 18:00
        and aggregate_interval is 10800 (3 hours), then the returned time vector will be
        (shown in local times):
        
        8-Mar-2009 21:00
        9-Mar-2009 00:00
        9-Mar-2009 03:00
        9-Mar-2009 06:00 etc.
        
        Note that DST happens at 02:00 on 9-Mar, so the actual time deltas between the
        elements is 3 hours between times #1 and #2, but only 2 hours between #2 and #3.
        
        NB: there is an algorithmic assumption here that the archive time interval
        is a constant.
        
        There is another assumption that the unit type does not change within a time interval.
        
        sql_type: The SQL type to be retrieved (e.g., 'outTemp') 
        
        startstamp: If aggregation_interval is None, then data with timestamps greater
        than or equal to this value will be returned. If aggregation_interval is not
        None, then the start of the first interval will be greater than (exclusive of) this
        value. 
        
        stopstamp: Records with time stamp less than or equal to this will be retrieved.
        If interval is not None, then the last interval will include this value.
        
        aggregate_interval: None if no aggregation is desired, otherwise
        this is the time interval over which a result will be aggregated.
        Default: None (no aggregation)
        
        aggregate_type: None if no aggregation is desired, otherwise the type of
        aggregation (e.g., 'sum', 'avg', etc.)  Required if aggregate_interval
        is non-None. Default: None (no aggregation)
        returns: a 2-way tuple of value tuples: 
          (time_vec_value_tuple, data_vec_value_tuple)
        The first element holds the time value tuple, the second the data value tuple.
        See the file weewx.units for the definition of a value tuple.
        """
        # There is an assumption here that the unit type does not change in the
        # middle of the time interval.

        time_vec = list()
        data_vec = list()
        std_unit_system = None
        try:
            _cursor=self.connection.cursor()
    
            if aggregate_interval :
                if not aggregate_type:
                    raise weewx.ViolatedPrecondition, "Aggregation type missing"
                sql_str = 'SELECT MAX(dateTime), %s(%s), usUnits FROM %s WHERE dateTime > ? AND dateTime <= ?' % (aggregate_type, sql_type, self.table)
                for stamp in weeutil.weeutil.intervalgen(startstamp, stopstamp, aggregate_interval):
                    _cursor.execute(sql_str, stamp)
                    _rec = _cursor.fetchone()
                    # Don't accumulate any results where there wasn't a record
                    # (signified by a null result)
                    if _rec:
                        # Also, there may be records, but there may not be any non-Null results.
                        if _rec[0] is not None:
                            if std_unit_system:
                                if std_unit_system != _rec[2]:
                                    raise weewx.UnsupportedFeature, "Unit type cannot change within a time interval."
                            else:
                                std_unit_system = _rec[2]
                            time_vec.append(_rec[0])
                            data_vec.append(_rec[1])
            else:
                sql_str = 'SELECT dateTime, %s, usUnits FROM %s WHERE dateTime >= ? AND dateTime <= ?' % (sql_type, self.table)
                for _rec in _cursor.execute(sql_str, (startstamp, stopstamp)):
                    time_vec.append(_rec[0])
                    data_vec.append(_rec[1])
                    if std_unit_system:
                        if std_unit_system != _rec[2]:
                            raise weewx.UnsupportedFeature, "Unit type cannot change within a time interval."
                    else:
                        std_unit_system = _rec[2]
        finally:
            _cursor.close()

        (time_type, time_group) = weewx.units.getStandardUnitType(std_unit_system, 'dateTime')
        (data_type, data_group) = weewx.units.getStandardUnitType(std_unit_system, sql_type, aggregate_type)
        return (ValueTuple(time_vec, time_type, time_group), ValueTuple(data_vec, data_type, data_group))

    def getSqlVectorsExtended(self, ext_type, startstamp, stopstamp, 
                              aggregate_interval = None, 
                              aggregate_type = None):
        """Get time and (possibly aggregated) data vectors within a time interval.
        
        This function is very similar to getSqlVectors, except that for special types
        'windvec' and 'windgustvec', it returns wind data broken down into 
        its x- and y-components.
        
        sql_type: The SQL type to be retrieved (e.g., 'outTemp', or 'windvec'). 
        If this type is the special types 'windvec', or 'windgustvec', then what
        will be returned is a vector of complex numbers. 
        
        startstamp: If aggregation_interval is None, then data with timestamps greater
        than or equal to this value will be returned. If aggregation_interval is not
        None, then the start of the first interval will be greater than (exclusive of) this
        value. 
        
        stopstamp: Records with time stamp less than or equal to this will be retrieved.
        If interval is not None, then the last interval will include this value.
        
        aggregate_interval: None if no aggregation is desired, otherwise
        this is the time interval over which a result will be aggregated.
        Default: None (no aggregation)
        
        aggregate_type: None if no aggregation is desired, otherwise the type of
        aggregation (e.g., 'sum', 'avg', etc.)  Required if aggregate_interval
        is non-None. Default: None (no aggregation)
        
        returns: a 2-way tuple of value tuples: 
          (time_vec_value_tuple, data_vec_value_tuple)
        The first element holds the time value tuple, the second the data value tuple.
        See the file weewx.units for the definition of a value tuple.
        If sql_type is 'windvec' or 'windgustvec', the data
        vector will be a vector of types complex. The real part is the x-component
        of the wind, the imaginary part the y-component. 
        """

        windvec_types = {'windvec'     : ('windSpeed, windDir'),
                         'windgustvec' : ('windGust,  windGustDir')}
        
        # Check to see if the requested type is not 'windvec' or 'windgustvec'
        if ext_type not in windvec_types:
            # The type is not one of the extended wind types. Use the regular version:
            return self.getSqlVectors(ext_type, startstamp, stopstamp, aggregate_interval, aggregate_type)

        # It is an extended wind type. Prepare the lists that will hold the final results.
        time_vec = list()
        data_vec = list()
        std_unit_system = None
        
        try:
            _cursor=self.connection.cursor()
    
            # Is aggregation requested?
            if aggregate_interval :
                # Aggregation is requested.
                # The aggregation should happen over the x- and y-components. Because they do
                # not appear in the database (only the magnitude and direction do) we cannot
                # do the aggregation in the SQL statement. We'll have to do it in Python.
                # Do we know how to do it?
                if aggregate_type not in ('sum', 'count', 'avg', 'max', 'min'):
                    raise weewx.ViolatedPrecondition, "Aggregation type missing or unknown"
                
                # This SQL select string will select the proper wind types
                sql_str = 'SELECT dateTime, %s, usUnits FROM %s WHERE dateTime > ? AND dateTime <= ?' % (windvec_types[ext_type], self.table)
                # Go through each aggregation interval, calculating the aggregation.
                for stamp in weeutil.weeutil.intervalgen(startstamp, stopstamp, aggregate_interval):
    
                    _mag_extreme = _dir_at_extreme = None
                    _xsum = _ysum = 0.0
                    _count = 0
                    _last_time = None
    
                    for _rec in _cursor.execute(sql_str, stamp):
                        (_mag, _dir) = _rec[1:3]
    
                        if _mag is None:
                            continue
    
                        # A good direction is necessary unless the mag is zero:
                        if _mag == 0.0  or _dir is not None:
                            _count += 1
                            _last_time  = _rec[0]
                            if std_unit_system:
                                if std_unit_system != _rec[3]:
                                    raise weewx.UnsupportedFeature, "Unit type cannot change within a time interval."
                            else:
                                std_unit_system = _rec[3]
                            
                            # Pick the kind of aggregation:
                            if aggregate_type == 'min':
                                if _mag_extreme is None or _mag < _mag_extreme:
                                    _mag_extreme = _mag
                                    _dir_at_extreme = _dir
                            elif aggregate_type == 'max':
                                if _mag_extreme is None or _mag > _mag_extreme:
                                    _mag_extreme = _mag
                                    _dir_at_extreme = _dir
                            else:
                                # No need to do the arithmetic if mag is zero.
                                # We also need a good direction
                                if _mag > 0.0 and _dir is not None:
                                    _xsum += _mag * math.cos(math.radians(90.0 - _dir))
                                    _ysum += _mag * math.sin(math.radians(90.0 - _dir))
                    # We've gone through the whole interval. Was their any good data?
                    if _count:
                        # Record the time of the last good data point:
                        time_vec.append(_last_time)
                        # Form the requested aggregation:
                        if aggregate_type in ('min', 'max'):
                            if _dir_at_extreme is None:
                                # The only way direction can be zero with a non-zero count
                                # is if all wind velocities were zero
                                if weewx.debug:
                                    assert(_mag_extreme <= 1.0e-6)
                                x_extreme = y_extreme = 0.0
                            else:
                                x_extreme = _mag_extreme * math.cos(math.radians(90.0 - _dir_at_extreme))
                                y_extreme = _mag_extreme * math.sin(math.radians(90.0 - _dir_at_extreme))
                            data_vec.append(complex(x_extreme, y_extreme))
                        elif aggregate_type == 'sum':
                            data_vec.append(complex(_xsum, _ysum))
                        elif aggregate_type == 'count':
                            data_vec.append(_count)
                        else:
                            # Must be 'avg'
                            data_vec.append(complex(_xsum/_count, _ysum/_count))
            else:
                # No aggregation desired. It's a lot simpler. Go get the
                # data in the requested time period
                # This SQL select string will select the proper wind types
                sql_str = 'SELECT dateTime, %s, usUnits FROM %s WHERE dateTime >= ? AND dateTime <= ?' % (windvec_types[ext_type], self.table)
                
                for _rec in _cursor.execute(sql_str, (startstamp, stopstamp)):
                    # Record the time:
                    time_vec.append(_rec[0])
                    if std_unit_system:
                        if std_unit_system != _rec[3]:
                            raise weewx.UnsupportedFeature, "Unit type cannot change within a time interval."
                    else:
                        std_unit_system = _rec[3]
                    # Break the mag and dir down into x- and y-components.
                    (_mag, _dir) = _rec[1:3]
                    if _mag is None or _dir is None:
                        data_vec.append(None)
                    else:
                        x = _mag * math.cos(math.radians(90.0 - _dir))
                        y = _mag * math.sin(math.radians(90.0 - _dir))
                        if weewx.debug:
                            # There seem to be some little rounding errors that are driving
                            # my debugging crazy. Zero them out
                            if abs(x) < 1.0e-6 : x = 0.0
                            if abs(y) < 1.0e-6 : y = 0.0
                        data_vec.append(complex(x,y))
        finally:
            _cursor.close()

        (time_type, time_group) = weewx.units.getStandardUnitType(std_unit_system, 'dateTime')
        (data_type, data_group) = weewx.units.getStandardUnitType(std_unit_system, ext_type, aggregate_type)
        return (weewx.units.ValueTuple(time_vec, time_type, time_group), weewx.units.ValueTuple(data_vec, data_type, data_group))

    @staticmethod
    def _create_table(archive_db_dict, archiveSchema, table):
        """Create a SQL table using a given archive schema.
        
        archive_db_dict: A database dictionary holding the information necessary
        to open the database.
        
        archiveSchema: The schema to be used
        
        table: The name of the table to be used within the database.
        
        Returns: 
        A connection""" 
    
        # First try to create the database. If it already exists, an exception will
        # be thrown.
        try:
            weedb.create(archive_db_dict)
        except weedb.DatabaseExists:
            pass
    
        # List comprehension of the types, joined together with commas. Put
        # the SQL type in backquotes, because at least one of them ('interval')
        # is a MySQL reserved word
        _sqltypestr = ', '.join(["`%s` %s" % _type for _type in archiveSchema])

        _connect = weedb.connect(archive_db_dict)
        try:
            with weedb.Transaction(_connect) as _cursor:
                _cursor.execute("CREATE TABLE %s (%s);" % (table, _sqltypestr))
                
        except Exception, e:
            _connect.close()
            syslog.syslog(syslog.LOG_ERR, "archive: Unable to create database table '%s'." % table)
            syslog.syslog(syslog.LOG_ERR, "****     %s" % (e,))
            raise
    
        syslog.syslog(syslog.LOG_NOTICE, "archive: Created and initialized database table '%s'." % table)

        return _connect
    
    def _getTypes(self):
        """Returns the types appearing in an archive database.
        
        Raises exception of type weedb.OperationalError if the 
        database has not been initialized."""
        
        # Get the columns in the table
        column_list = self.connection.columnsOf(self.table)
        return column_list

def reconfig(old_db_dict, new_db_dict, new_unit_system=None, new_schema=user.schemas.defaultArchiveSchema):
    """Copy over an old archive to a new one, using a provided schema."""
    
    with Archive.open(old_db_dict) as old_archive:
        with Archive.open_with_create(new_db_dict, new_schema) as new_archive:

            # Wrap the input generator in a unit converter.
            record_generator = weewx.units.GenWithConvert(old_archive.genBatchRecords(), new_unit_system)
        
            # This is very fast because it is done in a single transaction context:
            new_archive.addRecord(record_generator)

#===============================================================================
#                          Abstract base class REST
#===============================================================================

class REST(object):
    """Abstract base class for RESTful protocols."""
    
    # The types to be retrieved from the arhive database:
    archive_types = ['dateTime', 'usUnits', 'barometer', 'outTemp', 'outHumidity', 
                     'windSpeed', 'windDir', 'windGust', 'windGustDir', 'dewpoint', 'radiation', 'UV']
    # A SQL statement to do the retrieval:
    sql_select = "SELECT " + ", ".join(archive_types) + " FROM archive WHERE dateTime=?"  

    def extractRecordFrom(self, archive, time_ts):
        """Get a record from the archive database. 
        
        This is a general version that works for:
          - WeatherUnderground
          - PWSweather
          - CWOP
        It can be overridden and specialized for additional protocols.
        
        archive: An instance of weewx.archive.Archive
        
        time_ts: The record desired as a unix epoch time.
        
        returns: A dictionary of weather values"""
        
        sod_ts = weeutil.weeutil.startOfDay(time_ts)
        
        # Get the data record off the archive database:
        sqlrec = archive.getSql(REST.sql_select, (time_ts,))
        # There is no reason why the record would not be in the database,
        # but check just in case:
        if sqlrec is None:
            raise SkippedPost("Non existent record %s" % (weeutil.weeutil.timestamp_to_string(time_ts),))

        # Make a dictionary out of the types:
        datadict = dict(zip(REST.archive_types, sqlrec))
        
        # CWOP says rain should be "rain that fell in the past hour".  WU says
        # it should be "the accumulated rainfall in the past 60 min".
        # Presumably, this is exclusive of the archive record 60 minutes before,
        # so the SQL statement is exclusive on the left, inclusive on the right.
        datadict['rain'] = archive.getSql("SELECT SUM(rain) FROM archive WHERE dateTime>? AND dateTime<=?",
                                         (time_ts - 3600.0, time_ts))[0]

        # Similar issue, except for last 24 hours:
        datadict['rain24'] = archive.getSql("SELECT SUM(rain) FROM archive WHERE dateTime>? AND dateTime<=?",
                                            (time_ts - 24*3600.0, time_ts))[0]

        # NB: The WU considers the archive with time stamp 00:00 (midnight) as
        # (wrongly) belonging to the current day (instead of the previous
        # day). But, it's their site, so we'll do it their way.  That means the
        # SELECT statement is inclusive on both time ends:
        datadict['dayRain'] = archive.getSql("SELECT SUM(rain) FROM archive WHERE dateTime>=? AND dateTime<=?", 
                                              (sod_ts, time_ts))[0]

        # All these online weather sites require US units. 
        if datadict['usUnits'] == weewx.US:
            # It's already in US units.
            return datadict
        else:
            # It's in something else. Perform the conversion
            datadict_us = weewx.units.StdUnitConverters[weewx.US].convertDict(datadict)
            # Add the new unit system
            datadict_us['usUnits'] = weewx.US
            return datadict_us
        

#===============================================================================
#                       class Personal Weather using Ambient
#===============================================================================

class PWdashboard(REST):
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
    