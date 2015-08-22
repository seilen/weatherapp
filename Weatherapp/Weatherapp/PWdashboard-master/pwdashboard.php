<?php

// Personal Weather Dashboard version 1.0.3
// 2013-2014 - JF Nutbroek <jfnutbroek@gmail.com>

/* 
    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

// Configuration

// $stationids hold the weatherstation name(s) which will be used for the database(s)
// $password is the password to import data

$stationids  = array('WH1080');          // Use the letters a-Z, numbers 0-9, underscore (no spaces!)
$password    = 'Newuser12';                              // Password, same as pweather.py password, same for all your weather stations
$max_data    = 864;                                       // Maximum amount of rows to return. If more measurements exist averages will be returned
$local_db    = false;                                     // True if you run weewx and pwdashboard on the same server - assumes weewx as db name

// End of configuration

// Code starts here

try {

    /**
     * Verify Authorized Access for data import
     */
    if ((isset($_GET['PASSWORD']) && isset($_GET['ID']) && isset($_GET['action'])) && (in_array($_GET['ID'], $stationids) && $_GET['PASSWORD'] == $password)) {
        
        /**
         * Authorized - password is OK
         */
        if (($_GET['action'] == 'updateraw')) {
            if (!PWdashboard_import($_GET['ID'])) {
                header('HTTP/1.1 503 Service Unavailable');
                die('Service Unavailable');                          
            } else {
                header('HTTP/1.1 200 Success');
                die('Success');                        
            }
        } else {
            header('HTTP/1.1 405 Method Not Allowed');
            die('Method Not Allowed');    
        }
        
    } else if (isset($_GET['ID']) && isset($_GET['action']) && isset($_GET['from']) && isset($_GET['to']) && in_array($_GET['ID'], $stationids) && $_GET['action'] == 'export' && is_numeric($_GET['from']) && is_numeric($_GET['to'])) {

        /**
         * Valid export request (no password required)
         */
        $graphdata = PWdashboard_export($_GET['ID'], $_GET['from'], $_GET['to'], $max_data, $local_db);
        echo $graphdata;

    } else {
        
        /**
         * Unauthorized or bad export request
         */
        header('HTTP/1.1 401 Unauthorized');
        die('ERROR Unauthorized');
        
    }
    
} catch (Exception $e) {

        /**
         * Error
         */    
        header('HTTP/1.1 503 Service Unavailable');
        die('Service Unavailable');
    
}

/**
* Import PWS/Ambient formatted imperial data
*
*/	
function PWdashboard_import($database) {
    
    //  Weather data formats
    $formats   = array(
        'dateTime'    => 'dateutc',
        'barometer'   => 'baromin',
        'outTemp'     => 'tempf',
        'outHumidity' => 'humidity',
        'windSpeed'   => 'windspeedmph',
        'windDir'     => 'winddir',
        'windGust'    => 'windgustmph',
        'dewpoint'    => 'dewptf',
        'hourRain'    => 'rainin',
        'dayRain'     => 'dailyrainin',
        'radiation'   => 'solarradiation',
        'UV'          => 'UV'
    );  
    // Open database connection
    $dbhandle = new PDO("sqlite:database/$database.sdb");
    // Create database table if it does not exist
    $dbhandle->exec("CREATE TABLE IF NOT EXISTS $database (dateTime INTEGER NOT NULL UNIQUE PRIMARY KEY, barometer REAL, outTemp REAL, outHumidity REAL, windSpeed REAL, windDir REAL, windGust REAL, dewpoint REAL, rainRate REAL, rain REAL, radiation REAL, UV REAL)");
    // Prepare database query
    $query = "INSERT INTO \"$database\" (\"dateTime\",\"barometer\",\"outTemp\",\"outHumidity\",\"windSpeed\",\"windDir\",\"windGust\",\"dewpoint\",\"rainRate\",\"rain\",\"radiation\",\"UV\") VALUES (";            
    // Retrieve data & validate
    foreach($formats as $field => $postname) {
        if (isset($_GET["$postname"]) && is_numeric($_GET["$postname"])) {
            $query = $query . "'" . $_GET["$postname"] . "',";
        } else {
            $query = $query . "NULL,";
        }
    }
    $query = rtrim($query, ',') . ')';
    $result = $dbhandle->query($query);
    // Close database connection
    $dbhandle = null;    
    return $result;

}

/**
* Export JSON data
*
*/	
function PWdashboard_export($database, $from, $to, $max, $local) {
    
    // Open database connection
    if ($local) {
      $dbhandle = new PDO("sqlite:database/weewx.sdb");
      $database = 'archive';
    } else {
      $dbhandle = new PDO("sqlite:database/$database.sdb");
    }
    if ($to == 0) {
      // Return last measurement
      $query = "SELECT * FROM $database ORDER BY dateTime DESC LIMIT 1";
    } else {
      // Check amount of rows involved
      $query = "SELECT COUNT(*) FROM $database WHERE dateTime >= $from AND dateTime < $to";
      $result = $dbhandle->query($query);
      $rows = $result->fetchColumn();
      // Build average query or full dataset query
      if ($rows > $max) {
        // Return averaged datasets over the requested period
        $div = ($to - $from) / $max;
        $query = "SELECT (dateTime/$div) AS timeslice,"
            . 'ROUND(AVG(dateTime),0) AS dateTime,'
            . 'ROUND(AVG(barometer),2) AS barometer,'
            . 'ROUND(AVG(outTemp),2) AS outTemp,'
            . 'ROUND(AVG(outHumidity),0) AS outHumidity,'
            . 'ROUND(AVG(windSpeed),2) AS windSpeed,'
            . 'ROUND(AVG(windDir),0) AS windDir,'
            . 'ROUND(AVG(windGust),2) AS windGust,'
            . 'ROUND(AVG(dewpoint),2) AS dewpoint,'
            . 'ROUND(AVG(rainRate),2) AS rainRate,'
            . 'ROUND(AVG(rain),2) AS rain,'
            . 'ROUND(AVG(radiation),2) AS radiation,'
            . "ROUND(AVG(UV),0) AS UV FROM $database WHERE dateTime >= $from AND dateTime < $to GROUP BY timeslice";
      } else {
        // Return full dataset over request period
        $query = "SELECT * FROM $database WHERE dateTime >= $from AND dateTime < $to";        
      }
    }
    // Query database
    $result = $dbhandle->query($query);
    // Prepare the DataTable columns
    $json_data = '{"cols": [{"type": "datetime", "label": "dateTime"},'
                . '{"type": "number","label": "barometer"},'
                . '{"type": "number","label": "outTemp"},'
                . '{"type": "number","label": "outHumidity"},'        
                . '{"type": "number","label": "windSpeed"},'
                . '{"type": "number","label": "windDir"},'
                . '{"type": "number","label": "windGust"},'
                . '{"type": "number","label": "dewpoint"},'
                . '{"type": "number","label": "hourRain"},'
                . '{"type": "number","label": "dayRain"},'
                . '{"type": "number","label": "radiation"},'
                . '{"type": "number","label": "UV"}],"rows": [';
    // Add the rows with data -Timestamps are in UTC
    foreach ($result as $row) {
            $json_data .= '{"c": [{"v": "Date(' . number_format($row['dateTime']*1000, 0, '.', '') . ')"},'
                .'{"v": "' . $row['barometer'] . '"},'
                . '{"v": "' . $row['outTemp'] . '"},'
                . '{"v": "' . $row['outHumidity'] . '"},'
                . '{"v": "' . $row['windSpeed'] . '"},'
                . '{"v": "' . $row['windDir'] . '"},'
                . '{"v": "' . $row['windGust'] . '"},'
                . '{"v": "' . $row['dewpoint'] . '"},'
                . '{"v": "' . $row['rainRate'] . '"},'
                . '{"v": "' . $row['rain'] . '"},'
                . '{"v": "' . $row['radiation'] . '"},'
                . '{"v": "' . $row['UV'] . '"}]},';
    }
    $json_data = rtrim($json_data, ',') . ']}';
    // Close database connection
    $dbhandle = null;
    return $json_data;

}

?>