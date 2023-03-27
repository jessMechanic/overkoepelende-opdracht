class CardElement extends HTMLElement {

    constructor() {
        super();
        this.classList.add("card-element");
        let title = this.hasAttribute("title") ? this.getAttribute("title") : 'unnamed';
        let image = this.hasAttribute("image-src") ? this.getAttribute("image-src") : '';
        let pColor = this.hasAttribute("pColor") ? this.getAttribute("pColor") : 'black';
        let description = this.hasAttribute("description") ? this.getAttribute("description").replaceAll('\n',"<br>") : 'dev forgot this one';
        let Size = this.hasAttribute("size") ? this.getAttribute("size") : 1;
        this.style.setProperty('--Size',Size * 0.5);
        this.innerHTML = `
        <section class="card-wrapper" style="--color : ${pColor}; ">
            <div class="element-container">
            <div class="title-cont">
                <h1>${title}</h1></div>
                <img src="${image}">
                 <div class="title-cont">
                <h1>${title}</h1></div>
                <p>${description}</p>
            </div>
        </section>
        `;
    }

}
class Profile_Box extends HTMLElement {

    constructor() {
        super();
        this.classList.add("player_info");
        let name = this.hasAttribute("name") ? this.getAttribute("name") : 'unnamed';
        let image = this.hasAttribute("image") ? this.getAttribute("image") : '';
        this.innerHTML = `
        <section class="player_info">
            <p class="player_info-name">${name}</p>
            <img src="${image}">
        </section>
        `;
    }

}



customElements.define('card-element', CardElement);
customElements.define('player-info',Profile_Box)