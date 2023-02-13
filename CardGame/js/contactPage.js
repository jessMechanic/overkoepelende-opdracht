
function modifyText(event) {
   let textbox = event.target;
   let charCount = textbox.value.length;
    let charMax =  textbox.maxLength;
    textbox.nextElementSibling.innerHTML = charCount + "/" + charMax;

    if (charCount > (charMax * .95)){
        textbox.nextElementSibling.style.color = "red"
    }else  if(charCount > (charMax * .8)){
        textbox.nextElementSibling.style.color = "orange"
    }else {
        textbox.nextElementSibling.style.color = "black"
    }

}

const MessageBoxes = document.getElementsByClassName("textinput-box");
    for (let messagebox of MessageBoxes) {
         messagebox.firstElementChild.addEventListener("input",modifyText)
    }