"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/matchHub").build();
connection.on("ReceiveMessage", function (user, message) {});
connection.on("joined", function (user, room) { console.log(`${user} joined room ${room}`);})
connection.on("RoomConnectResult", function (result) { console.log(result ? "succes" : "failed"); })
connection.on("DefineCard", function (result) { console.log(result) });
connection.on("DefineCardRoomWide", function (result) { console.log(result) });
connection.start().then(function () {
    let matchId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);

    //checking if the match exists before joining it
    connection.invoke('JoinRoom', matchId, sessionStorage.getItem("PlayerId"));
}
);

