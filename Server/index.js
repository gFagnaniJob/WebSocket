var WebSocketServer = require('websocket').server;
var http = require('http');

var server = http.createServer(function (request, response) {
    //Ci interessano solo le web socket, quindi non implementiamo niente qua dentro
});

server.listen(1337, function () {
    console.log("Server listening on port 1337");
});

wsServer = new WebSocketServer({
    httpServer: server
});

wsServer.on('request', function (request) {
    var connection = request.accept(null, request.origin);
    connection.on('message', function (message) {
        if (message.type == 'utf8') {
            if (message.utf8Data === "Connection UP") {
                console.log('Connected with ' + connection.remoteAddress);
                connection.sendUTF('{"key": "ciao"}');
            } else {
                console.log(message.utf8Data);
                connection.sendUTF('{"key": "hola"}');
            }
        }
        if (message.type == 'binary') {
            var json = JSON.parse(message.binaryData);
            console.log('key = ' + json.key);
            connection.sendBytes(message.binaryData);
        }
    });
    connection.on('close', function (connection) {

    });
});