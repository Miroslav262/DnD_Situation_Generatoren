const one_roll = function (size) {
    return Math.floor(Math.random() * size) + 1;
};

const roll = function (count, size) {
    let result = 0;
    for (let i = 0; i < count; i++) {
        result += one_roll(size);
    }
    return result;
};

document.addEventListener("DOMContentLoaded", () => {

    document.querySelectorAll(".roll").forEach(el => {
        el.addEventListener("click", (e) => {
            e.preventDefault();

            const bonus = parseInt(el.dataset.bonus);
            const r = one_roll(20);
            const total = r + bonus;

            el.classList.remove("crit", "fail");

            if (r === 20) {
                el.classList.add("crit");
            }

            if (r === 1) {
                el.classList.add("fail");
            }

            el.innerHTML = `${bonus} → ( = ${total})`;
        });
    });


    document.querySelectorAll(".hp_roll").forEach(el => {
        el.addEventListener("click", (e) => {
            e.preventDefault();

            const hp = el.dataset.hp_roll;

            if (!hp || !hp.includes("d")) {
                el.innerHTML = `${hp}`;
                return;
            }

            const [countPart, rest] = hp.split("d");
            const [sizePart, bonusPart] = rest.split("+");

            const count = parseInt(countPart);
            const size = parseInt(sizePart);
            const bonus = parseInt(bonusPart);

            const total = roll(count, size) + bonus;

            el.innerHTML = `(${hp}) → ${total}`;
        });
    });

});
