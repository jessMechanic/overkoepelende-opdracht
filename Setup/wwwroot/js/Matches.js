
import { getPlayerId, ApiPath } from "/js/Modules/PlayerAuthentication.js";
let send
document.getElementById("CreateNewButton").addEventListener("click", CreateMatch)
document.getElementById("copy").addEventListener("click", copy)
document.getElementById("JoinMatch").addEventListener("click", JoinMatch)
document.getElementById("joinRoomButton").addEventListener("click", JoinMatch)

function JoinMatch(event) {
    event.preventDefault()
    let roomCode = document.getElementById("roomCode").value
    let newRoomCode = document.getElementById("room-code").value
    if (roomCode != "") {
        window.location.href = "match/" + roomCode;
    }
    if (newRoomCode != "") {
        window.location.href = "match/" + newRoomCode;
    } 


}

function copy() {
    let copyText = document.getElementById("room-code");
    navigator.clipboard.writeText(copyText.value);
}

function CreateMatch(event) {
    let responceJson;



    event.preventDefault()

    let playerId = getPlayerId();
    if (playerId == null) {
        //error handling
        return;
    }
    let data = playerId;
    let statusCode;

    postData(data)
        .then(async (data) => {
            responceJson = await data.json();
            statusCode = await data.status
        }).catch(function (error) {
            console.log(error)
            console.log("error")
        }).finally(() => {
            console.log(statusCode);
           
                document.getElementById("join-match-hidden").style.display = "block"
                document.getElementById("join-match").style.display = "none"

                document.getElementById("room-code").value = responceJson

           
        })

    // make the join good





}


let url = ApiPath + '/Match'


async function postData(player1) {
    let request = {
        method: "POST", // *GET, POST, PUT, DELETE, etc.
        mode: "cors", // no-cors, *cors, same-origin

        headers: {
            "Content-Type": "application/json",

            // 'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: JSON.stringify(player1) // body data type must match "Content-Type" header
    }
    console.log(request);
    const response = await fetch(url, request);

    return response;
}
