
function modifyText(event) {
    let textbox = event.target;
    let charCount = textbox.value.length;
    let charMax = textbox.maxLength;
    textbox.nextElementSibling.innerHTML = charCount + "/" + charMax;

    if (charCount > (charMax * .95)) {
        textbox.nextElementSibling.style.color = "red"
    } else if (charCount > (charMax * .8)) {
        textbox.nextElementSibling.style.color = "orange"
    } else {
        textbox.nextElementSibling.style.color = "black"
    }

}

const MessageBoxes = document.getElementsByClassName("textinput-box");
for (let messagebox of MessageBoxes) {
    messagebox.firstElementChild.addEventListener("input", modifyText)
}

function SendMessage() {
    let Title = document.getElementById("title").value
    let Body = document.getElementById("message").value

    let data = {
        id : "515b7e14-b9ea-42c3-9e0a-5a77c0651988", //dummy overwriten in database
        body : Body,
        title : Title
    }
    console.log(JSON.stringify(data))


    postData(data)
        .then((data) => {
            console.log(data); // JSON data parsed by `data.json()` call


        }).catch(function (error) {
            console.log(error)
        });

}


url = 'https://localhost:7188/user/515b7e14-b9ea-42c3-9e0a-5a77c0651988/messages'// dummy till user accounts are fully implemented


async function postData(data) {
    const response = await fetch(url, {
        method: 'POST', 
        
        headers: {
            "Content-type": "application/json;charset=UTF-8",
            "Mode" : "no-cors"
        },
        body: JSON.stringify(data)
    });

    return response;
}
// function to handle success
async function success() {
    const response = await fetch('https://localhost:7188/users', {
        method: "GET",
        mode: 'no-cors',
        headers: {
            "Content-type": "application/json;charset=UTF-8",
            "accept": "text/plain",
        }

    });
    console.log(response)
}
