const mods = require('./mods');
const users = require('./users');
const express = require('express');
const app = express();
const server = require('http').createServer(app);
const WebSocket = require('ws');

const wsserver = new WebSocket.Server({ server });

let connections = 0;
wsserver.on("connection", (ws) => {
    console.log('connection made ' + (connections++));

    ws.send(JSON.stringify({type: 1, mods})); // mod list
    ws.send(JSON.stringify({type: 4, user: users[0]})); // user
    ws.send(JSON.stringify({type: 3, members: users.slice(1)})); // party members

    ws.on('message', (message) => handleMessage(ws, message));
});

function handleMessage(ws, message) {
    console.log('got message: ', message);
    let msg = JSON.parse(message);

    switch(msg.type) {
        case 1: // apply
        case 2: // activate
            let mod = mods.filter(mod => mod.uniqueId == msg.modId)[0];
            mod.active = msg.value;
            ws.send(JSON.stringify({type: 2, mod}));
            break;
        case 3: // join
        case 4: // create invite
        case 5: // update user
    }
}

server.listen(5000, console.log('listening'));

