
var http = require('http');

var data = {
  Temperature: 20.0,
  Humidity: 15.0
};

var dataString = JSON.stringify(data);
console.log(dataString);
var headers = {
  'Content-Type': 'application/json',
  'Content-Length': dataString.length
};

var options = {
  host: 'karolinesweather.azurewebsites.net',
  port: 80,
  path: '/api/IndoorTemperatureModels',
  method: 'POST',
  headers: headers
};

function post(){
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
	
	req.write(dataString);
	req.end();
}

post();