class CardElement extends HTMLElement {

    constructor() {
        super();
        this.classList.add("card-element");
        let title = this.hasAttribute("title") ? this.getAttribute("title") : 'unnamed';
        let image = this.hasAttribute("image-src") ? this.getAttribute("image-src") : '';
        let pColor = this.hasAttribute("pColor") ? this.getAttribute("pColor") : 'black';
        let description = this.hasAttribute("description") ? this.getAttribute("description").replaceAll('\n',"<br>") : 'dev forgot this one';
        this.innerHTML = `
        <section class="card-wrapper" style="--color : ${pColor};">
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

customElements.define('card-element', CardElement);