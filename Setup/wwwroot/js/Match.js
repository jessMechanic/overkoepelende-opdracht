"use strict";

let cardId = -1;
let matchId = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
var connection = new signalR.HubConnectionBuilder().withUrl("/matchHub").build();
connection.on("ReceiveMessage", function (user, message) {});
connection.on("joined", function (user, room) { console.log(`${user} joined room ${room}`);})
connection.on("RoomConnectResult", function (result) { console.log(result ? "succes" : "failed"); })
connection.on("DefineCard", function(result) {DefineCardHand(result);});
connection.on("DefineCardRoomWide", function (result) {DefineCardPlayingField(result);});
connection.on("Reset",function (){reset()})
connection.start().then(function () {
    //checking if the match exists before joining it
    connection.invoke('JoinRoom', matchId,getPlayerId())

}
);
function AnnounceWinner(player) {
    let winMessage = document.querySelector("#win-message")
    if (player == getPlayerId()) {
        winMessage.querySelector(".title").innerHTML = "you won :D"
    } else {
        winMessage.querySelector(".title").innerHTML = "you lost ._."
    }
}

//<card-element size="1" pcolor="#591A12" title="working tite" image-src="/Assets/dwagon.webp"
//                           description="card 1" class="card-element" style="--Size:0.5;float: left"></card-element>
function DefineCardHand(Card){
    let hand = document.querySelector('.card-holder');
     hand.innerHTML += `<card-element size="1" pcolor="#591A12" title="${Card.name}" image-src="/Assets/dwagon.webp"
                          description="health : ${Card.health} \n damage : ${Card.damage}" 
                          CardId="${Card.index}" class="card-element" style="--Size:0.5;float: left"
                           onclick="playCard(event)"></card-element>`;
}

//{"RoomId":"eeb5ff3c-578e-4865-9d67-de733cfd4b2d","Message":"DefineCardRoomWide","IsGlobal":true,"CardDef":{"Name":"smallwall","Description":null,"Health":92,"Damage":0,"Effects":0,"Type":1,"Index":5},"Position":4,"PlayerSide":"640bc774-921f-490c-b65f-b23ab77bb9c7"}

function ready(){
     connection.invoke('Ready', `{"RoomId":"${matchId}","PlayerId":"${getPlayerId()}"}`);
}
function DefineCardPlayingField(json){
    let mes = JSON.parse(json);
    let yourCard  = (mes.PlayerSide == getPlayerId());
    let cardWraper = document.querySelector(`.${yourCard? "player_cards" : "opponent_cards"}`);
    let Card = mes.CardDef;
    console.log(mes.CardDef)
    cardWraper.innerHTML+= `<card-element  size="1" pcolor="#591A12" title="${Card.Name}" image-src="/Assets/dwagon.webp"
                          description="health : ${Card.Health} \n damage : ${Card.Damage}" 
                          class="card-element" style="--Size:0.5;float: ${yourCard? "left" : "right"}"
                          ></card-element>`;

    console.log(json);
}
async function DrawCard(){
    cardId = await connection.invoke('DrawCard', `{"RoomId":"${matchId}","PlayerId":"${getPlayerId()}"}`)
}
function reset(){
    document.querySelector(`.player_cards`).innerHTML = "";
    document.querySelector(`.opponent_cards`).innerHTML = "";
}
async function playCard(e){
    console.log(e);



    let el = e.currentTarget;
    let id =  el.getAttribute('CardId');
   let succes = await connection.invoke('PlayCard', `{"RoomId":"${matchId}","PlayerId":"${getPlayerId()}","Card":${id}}`);
    console.log(id)
        if(succes){
            el.remove();
        }else{
            console.log("not played")
        }


}

