export { getPlayerId, setPlayerId, ApiPath };

 const ApiPath = "https://localhost:7188"

function getPlayerId() {
   return sessionStorage.getItem("PlayerId")
}

function setPlayerId(value) {
    sessionStorage.setItem("PlayerId", value);
}
