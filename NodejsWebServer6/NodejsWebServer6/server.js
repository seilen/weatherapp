var http = require('http');
var uwp = require('uwp');
uwp.projectNamespace('Windows');
var calendar = new Windows.Globalization.Calendar();

/*var util = require('util');
var events = require('events');
var HID = require('HID');
var Logme = require('logme').Logme;
var log = new Logme({
    level: 'info',
    theme: 'clean'
});

var inbuf = new Buffer(20);
var inbufIndex = 0;

function datalogger() {
    var self = this;
    var devices = HID.devices(0x0FDE, 0xCA01);
    
    // TODO: when a better HID manager is available, wait for device do be plugged in
    // and don't assume it already is.
    if (!devices.length) {
        throw new Error("No valid weather station could be found");
    }
    
    console.log(HID.devices());
    log.info('Device found. Initializing');

    events.EventEmitter.call(self);

    // Set up the logger(s)

    if (nconf.get('pusher')) {
        var PusherLogger = require('./pusher_logger');
        self.pusherLogger = new PusherLogger(nconf.get('pusher'));
    }

    self.hid = new HID.HID(devices[0].path);

    // // Initialization sequence for HID based weather stations from Oregon Scientific
    self.hid.write([0x20, 0x00, 0x08, 0x01, 0x00, 0x00, 0x00, 0x00]);

    // // Bind callback function to HID library
    self.hid.read(self.handleHIDReport.bind(self));
}
*/
http.createServer(function (req, res) {
    res.writeHead(200, { 'Content-Type': 'text/plain' });
    var date = calendar.getDateTime();
    res.end(String(date));
}).listen(1337);