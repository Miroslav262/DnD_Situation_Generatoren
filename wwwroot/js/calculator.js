
    document.addEventListener("DOMContentLoaded", function () {

        function validateForm(event) {

            const crInput = document.querySelector("input[name='HeroCRInput']");
            const kHeroInput = document.querySelector("input[name='HeroKInput']");

            const crValue = crInput.value.trim();
            const kHeroValue = kHeroInput.value.trim();

            if (!crValue || !kHeroValue) {
                alert("Введите уровни героев и их количество.");
                event.preventDefault();
                return false;
            }

            const crParts = crValue.split(",").map(x => x.trim());
            const kHeroParts = kHeroValue.split(",").map(x => x.trim());

            if (crParts.length !== kHeroParts.length) {
                alert("Количество уровней и количество героев должно совпадать.");
                event.preventDefault();
                return false;
            }

            for (let part of crParts) {
                if (isNaN(part)) {
                    alert("Уровни героев должны быть числами.");
                    event.preventDefault();
                    return false;
                }
            }

            for (let part of kHeroParts) {
                if (!/^\d+$/.test(part)) {
                    alert("Количество героев должно быть целыми числами.");
                    event.preventDefault();
                    return false;
                }
            }

            const kInput = document.querySelector("input[name='KInput']");
            const kValue = kInput.value.trim();

            if (!kValue) {
                alert("Введите количество существ.");
                event.preventDefault();
                return false;
            }

            const kParts = kValue.split(",").map(x => x.trim());
            for (let part of kParts) {
                if (!/^\d+$/.test(part)) {
                    alert("Количество существ должно быть целыми числами.");
                    event.preventDefault();
                    return false;
                }
            }
        }

        document.querySelector("form").addEventListener("submit", validateForm);
    });
