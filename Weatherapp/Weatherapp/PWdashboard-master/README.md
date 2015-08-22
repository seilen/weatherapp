PWdashboard
===========

<strong>Publish your real-time Weewx weather data on your own website!</strong>

Innovative HTML5 based SVG dashboard with dynamic charts, animated gauges, sortable data tables, support for Imperial & Metric views, date picker for the time period to view, data zoom function, a map, and a data filter to eliminate bad measurements.

<img src="http://www.mywebmymail.com/images/newstories/HTML5_Logo_64.png" height="64px" width="64px">

Capture your data with a simple device like a Raspberry Pi and post the measurements to your hosting provider so you can display your data in real-time on your own website.
As from version 1.0.2 it is possible to run PWdashboard from the local device using the original Weewx database. As all graph creation and calculations are done on the fly on the client, the load on the local device is kept to a minimum.

The dashboard is cross-browser & cross-platform compatible. You can easily customize graphs by editing the index.htm file.

<img src="http://www.mywebmymail.com/images/newstories/pwdashboard.png" height="551px" width="625px">
<img src="http://www.mywebmymail.com/images/newstories/pwdashboardchart.png" height="297px" width="625px">

<strong>The package comprises:</strong>

<ul>
<li><strong>pwdashboard.py</strong> - Python RESTful extension for Weewx</li>
<li><strong>pwdashboard.php</strong> - PHP API for importing REST & exporting JSON data</li>
<li><strong>index.htm</strong> - The HTML5 page to display the SVG dashboard</li>
<li>and of course a stylesheet and some eye candy</li>
</ul>

<strong>It uses:</strong>

<ul>
<li>Google Charts</li>
<li>Google Map API</li>
<li>JQuery</li>
<li>JQuery plugins Backstretch & Cookie</li>
<li>PHP & SQLite</li>
</ul>

Check out the live station on: http://www.mywebmymail.com/pwdashboard

<strong>Get started</strong>

Download and unzip the PWdashboard.zip package and edit the <strong>pwdashboard.php</strong> file. You have to provide the ID of your weather station (which will be the database name) and a password which will be used for importing data.

```
$stationids  = array('WMR8800001'); <= your weather station name, no spaces, characters only a-Z,0-9
$password    = 'password'; <= the same password you will use in your weewx.conf configuration
```


Edit the <strong>index.htm</strong> file and provide your weather station information:

```
      // Personal Weather Dashboard ID
      var stationid   = 'WMR8800001';
      var displayname = 'My Local Weather Station';
      
      // Google map: Location, altitude, lat/lon, station type, setup
      var mapinfo = new Array(
	'Entremont, France', 
	'900m (2950 ft)', 
	'45.95207, 6.39798', 
	'WMR88', 
	'Weewx running on a Pi'
      );
```

Create a directory on your webserver (for example 'pwdashboard') and upload all PWdashboard files and the empty directory 'database' (it contains a .htaccess file). You can exclude the pwdashboard.py file as this will be required on your home server which is connected to your weatherstation.

Install Weewx on your home server. Just follow the standard 'out-of-the-box' Weewx installation instructions. Note: PWdashboard expects the 'raw' data which it receives from Weewx to be in the imperial data format (Fahrenheit, inHg, etc.). Once you have Weewx up and running, add the <strong>pwdashboard.py</strong> extension to the weewx/bin directory. Edit the <strong>weewx.conf</strong> file and add the following lines to the StdRESTful section:

```
    [StdRESTful]
        ...
    [[PWdashboard]]
        station = stationid (e.g., "WMR8800001") 
        password = password (e.g., "ABCdef1234") 
        server_url = URL to your webservers PHP API (e.g., "http://www.site.com/pwdashboard.php?") 
        driver = weewx.pwdashboard.PWdashboard
```

Stop Weewx (sh /etc/init.d/weewx stop) and restart (sh /etc/init.d/weewx start) to load the new configuration and RESTful extension. You can watch the syslog to verify if Weewx is running properly and data is posted to your webserver (watch tail /var/log/syslog).


<strong>Weewx and PWdashboard on the same server</strong>

There is no need to export/import data if you run your dashboard on the same server as Weewx. The original Weewx database will be used.

Verify your local device supports SQLite by entering the following at the commandline (control+z to exit SQLite):

```
    sqlite3
```

Should SQLite not be installed, proceed to install it:

```
    sudo apt-get install sqlite3
```

Edit pwdashboard.php and set the local database flag to true:

```
    $local_db = true;
```

Create a symbolic link in the database directory to the weewx.sdb file on your server, as an example (could be different on your installation):

```
    ln -s /home/weewx/archive/weewx.sdb weewx.sdb
```

Finally set the stationid variable in index.htm to 'weewx'.

```
    var stationid   = 'weewx';
```

Note: No changes are needed to Weewx (pwdashboard.py is not used and weewx.conf remains unchanged)

<strong>Final note</strong>

That should be it, now you can watch your weatherstation on your own HTML5/SVG website from anywhere in the world, even on your mobile phone!

PS: Should you run into any problems, make sure that your stationid and password match up in weewx.conf/pwdashboard.php/index.htm, you have read/write access to the files and the symbolic link (for local installations) is working. To verify if the database is accessible, download the package PhpLiteAdmin and add it to the 'database' directory of PWdashboard. Open the PhpLiteAdmin page in your browser to verify you can access the database.





