const CardClass = "card"


function UpdateCard() {

    let Cards = document.getElementsByClassName(CardClass)


    for (let i = 0; i < Cards.length; i++) {
        let Card = Cards[i];
        Card.style.setProperty('--TotalCards', Cards.length.toString())
        Card.style.setProperty('--CardIndex', i.toString())
        Card.style.setProperty('--TopOffset', Math.pow(i - (Cards.length / 2), 2).toString());
        Card.style.zIndex = i.toString();
    }
}

function Hover(event) {
    let index = event.target.style.getPropertyValue("--CardIndex")
    let Cards = document.getElementsByClassName(CardClass)



    for (let i = 0; i < Cards.length; i++) {
        let Card = Cards[i];
        Card.style.setProperty('--LeftOffset', (i == index ? 0 : i > index ? 0.8 : -0.8 ).toString());


    }
}

function HoverReset() {


    let Cards = document.getElementsByClassName(CardClass)


    for (let i = 0; i < Cards.length; i++) {
        let Card = Cards[i];
        Card.style.setProperty('--LeftOffset', "0");


    }

}

function click(){
    console.log("click");
}


function AddCard() {
    const newCard = document.createElement("div");
    newCard.className = CardClass;
    newCard.addEventListener("mouseover", Hover)
    newCard.addEventListener("mouseleave", HoverReset)
    newCard.addEventListener("click", click)

    document.body.appendChild(newCard);

    UpdateCard();
}


for (let i = 0; i < 10; i++) {
    AddCard()

}