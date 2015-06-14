
function GetDataString(time, interval, indoorTemp, indoorHumidity, outdoorTemp, outdoorHumidity, relativePressure, absolutePressure, windSpeed, gust, windDriection, dewPoint, windChill, hourRainfall, dayRainfall, weekRainfall, totalRainfall){
	var rawWeatherData = {
		'Time':"2015-06-08T21:00:00.00",
		Interval:50.0,
		IndoorTemp:0.0,
		IndoorHumidity:0.0,
		OutdoorTemp:10.0,
		OutdoorHumidity:50.0,
		RelativePressure:0.0,
		AbsolutePressure:0.0,
		WindSpeed:0.0,
		Gust:0.0,
		WindDirection:"N",
		DewPoint:0.0,
		WindChill:0.0,
		HourRainfall:0.0,
		DayRainfall:0.0,
		WeekRainfall:0.0,
		TotalRainfall:0.0
	};

	var dataString = JSON.stringify(rawWeatherData);
	console.log(dataString);
	return dataString;
}

var dataString = GetDataString("2015-06-08T21:00:00.00", 40.0, 0.0, 0.0, 10.0, 50.0, 0.0, 0.0, 0.0, 0.0, "W", 0.0, 0.0, 0.0, 0.0, 0.0, 0.0);

function post(weatherDataString){
	var http = require('http');
	
	var headers = {
		'Content-Type': 'application/json',
		'Content-Length': weatherDataString.length
	};
	
	var options = {
	  host: 'karolinesweather.azurewebsites.net',
	  port: 80,
	  path: '/api/weather',
	  method: 'POST',
	  headers: headers
	};
	
	var req = http.request(options, function(res) {
  		res.setEncoding('utf-8');
	
	  	var responseString = '';
	
		res.on('data', function(data) {
			responseString += data;
		});
	
		res.on('end', function() {
			console.log('end', JSON.parse(responseString));
			
			setTimeout(post, 15000);
		});
	});
	
	req.on('error', function(e) {
  		console.log('error', e);
	});
	
	req.write(weatherDataString);
	req.end();
}

post(dataString);


var inputFile = "WeatherDecember2014.csv";

function GetWeatherFromFile(path){
	var fs = require('fs');
	var csv = require('fast-csv');
	var encoding = require('encoding');
	
	var stream = fs.createReadStream(path);
	
	var csvStream = csv
		.fromStream(stream, {headers: true, quoteHeaders: true, ignoreEmpty: true, discardUnmappedColumns: true, delimiter: '\t'})
		.on("data", function(data){
			// \u0000 maa fjernes, dette er encoding uft-8?
			console.log({row: data});
		})
		.on("end", function(){
			console.log("done");
		});
	stream.pipe(csvStream);
}

GetWeatherFromFile(inputFile);