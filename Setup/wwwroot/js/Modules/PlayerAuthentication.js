export { getPlayerId, setPlayerId, ApiPath };

 const ApiPath = "https://localhost:7188"

function getPlayerId() {
    let logedIn = document.querySelector("#LogedIn").value
    if (logedIn) {
        return document.querySelector('#AuthToken').value
    } else {
        return null
    }
  
}

function setPlayerId(value) {
    sessionStorage.setItem("PlayerId", value);
}


window.getPlayerId = getPlayerId;