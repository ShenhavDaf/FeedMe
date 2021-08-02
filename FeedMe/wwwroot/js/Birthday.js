const COLORS = [
    /*'#00ffff',*//*תכלת*//*
    '#ff00ff',*//*ורוד*//*
    '#707dfa',*/
    '#FF3333',
    '#FF9933',
    '#FFFF33',
    '#33FF33',
    '#3399FF'
];

class Dot {
    constructor(container) {
        this.container = container;
        this.element = document.createElement('div');
        this.element.classList.add('dot');
        const colorIndex = Math.floor(Math.random() * (COLORS.length));
        this.element.style.backgroundColor = COLORS[colorIndex];
        this.xDelta = 0;
        this.yDelta = 0;
        this._shouldFall = false;

        this.xVelocity = this.getRandomNumber(7, 10);
        this.xOffset = this.getRandomNumber(50, 275);
        this.parabolaArch = this.getRandomNumber(0.005, 0.0005);
        this.stoppingHeight = this.getRandomNumber(20, 200);
        this.stoppingWidth = this.getRandomNumber(1, 5) * 100;
        this.yOffset = -this.parabolaArch * (this.xOffset * this.xOffset) + 20;
        this.wavyOffset = this.getRandomNumber(2, 5);
        this._isDone = false;
        this._isFalling = false;
    }

    reset() {
        this.element.remove();
    }

    isDone() {
        return this._isDone;
    }

    getRandomNumber(min, max) {
        return Math.random() * (max - min) + min;
    }

    attach() {
        this.container.append(this.element);
    }

    move(t) {
        if (this._isFalling) {
            if (t - this.startedFalling < 100) {
                return;
            }
            if (this.yDelta > 225) {
                this._isDone = true;
                return;
            }
            this.yDelta += 5;
            const xVal = this.xDelta + Math.sin(t / this.wavyOffset) * 3;
            this.element.style.transform = `translate(${xVal}px, ${this.yDelta}px)`;
            return;
        }

        if (this.xDelta - this.xOffset > 0 && this.yDelta > this.stoppingHeight ||
            this.xDelta > this.stoppingWidth) {
            this._isFalling = true;
            this.startedFalling = t;
            return;
        }
        this.xDelta += this.xVelocity;
        const xVal = this.xDelta - this.xOffset;
        this.yDelta = this.parabolaArch * (xVal * xVal) + this.yOffset;
        this.element.style.transform = `translate(${this.xDelta}px, ${this.yDelta}px)`;
    }
}

class Popper {
    constructor(onAnimationComplete) {
        this.onAnimationComplete = onAnimationComplete;
        this._onClick = this._onClick.bind(this);
        this._onWindupOver = this._onWindupOver.bind(this);
        this.popper = document.getElementById('popper');
        this.popperContainer = document.getElementById('popper-container');

        this.dots = [];
        this.reset();
    }

    _onClick() {
        this.popper.classList.add('windup');
        this.popperContainer.classList.remove('unclicked');
        this.popper.addEventListener('animationend', this._onWindupOver, { once: true });
    }

    reset() {
        this.popperContainer.classList.add('unclicked');
        this.popper.classList.remove('windup');
        this.popper.classList.remove('popped');
        this.popper.addEventListener('click', this._onClick, { once: true });
        this.dots.forEach(d => d.reset());
        this.dots = [];
    }

    _onWindupOver() {
        popper.classList.remove('windup');
        popper.classList.add('popped');
        /*popper.style.visibility = "hidden"; */
        this.onAnimationComplete();
        this._drawConfetti();
    }

    _drawConfetti() {
        for (let i = 0; i < 65; i++) {
            const dot = new Dot(this.popperContainer);
            dot.attach();
            this.dots.push(dot);
        }

        let frameCount = 0;

        const drawFrame = () => {
            const doneDots = this.dots.map(dot => dot.isDone() ? 1 : 0);
            const numberDone = doneDots.reduce((sum, val) => sum + val);
            if (this.dots.length === numberDone) {
                return;
            }
            this.dots.forEach(dot => dot.move(frameCount));
            frameCount++;
            requestAnimationFrame(drawFrame);
        };

        drawFrame();
    }
}

class Banner {
    constructor(message) {
        this.container = document.getElementById('banner');
        this.message = message;
    }

    render() {
        const letters = this.message.split('');
        letters.forEach(letter => {
            const letterDom = document.createElement('div');
            letterDom.textContent = letter;
            this.container.append(letterDom);
        });
        const message = document.getElementById('message');
        message.style.opacity = 1;
    }

    reset() {
        this.container.innerHTML = '';
        const message = document.getElementById('message');
        requestAnimationFrame(() => {
            message.style.transition = '0s';
            message.style.opacity = 0;
            requestAnimationFrame(() => {
                message.style.transition = '1s';
            });
        });
    }
}

class Card {
    constructor() {
        this._showBanner = this._showBanner.bind(this);
        this.banner = new Banner('HAPPY BDAY!');
        const popper = new Popper(this._showBanner);

        const resetButton = document.getElementById('reset-button');
        resetButton.addEventListener('click', () => {
            popper.reset();
            this.banner.reset();
            resetButton.style.visibility = 'hidden';
        });
    }

    _showBanner() {
        this.banner.render();
        const resetButton = document.getElementById('reset-button');
        resetButton.style.visibility = 'visible';
    }
}

new Card();
