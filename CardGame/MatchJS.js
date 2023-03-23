let ConnectionJS = () =>{
let connection = new signalR.HubConnectionBuilder().withUrl("/matchHub").build();

document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    let li = document.createElement("li");
    let messageObj;
    try {
        messageObj = JSON.parse(message);
    } catch (e) {

    }



});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

 function send(message) {
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
}




}