
// Hämta födelseåret från HTML
const birthYear = parseInt(document.getElementById('birth-year').textContent, 10);

// Hämta dagens datum
const todayDate = new Date();

// Beräkna det aktuella året
const currentYear = todayDate.getFullYear();

// Kontrollera om det är före den 1 januari
const hasBirthdayPassed = todayDate.getMonth() > 0 || (todayDate.getMonth() === 0 && todayDate.getDate() >= 1);

// Beräkna åldern
let age = currentYear - birthYear;
if (hasBirthdayPassed) {
    age--; // Justera om personen inte fyllt år ännu
}

// Sätt åldern i elementet med ID "age"
document.getElementById('age').textContent = age;


//Sätter värdet på combobox i profileredigeraren
document.getElementById("public-setting").value = "Offentlig", "Privat";


function searchUsers() {
    const searchInput = document.getElementById("searchInput");
    const searchQuery = searchInput.value.trim(); // Ta bort eventuella mellanslag
    const resultsContainer = document.getElementById("searchResults");

    // Hantera CSS-klassen baserat på om fältet är tomt
    if (searchQuery === "") {
        resultsContainer.classList.remove("searchCss");
        resultsContainer.innerHTML = ""; // Rensa sökresultat
        return; // Avsluta om fältet är tomt
    }

    // Lägg till CSS-klassen om det finns en sökfråga
    resultsContainer.classList.add("searchCss");

    // Hämta sökresultat via API
    fetch(`/User/Search?query=${encodeURIComponent(searchQuery)}`)
        .then(response => response.json())
        .then(data => {
            resultsContainer.innerHTML = ""; // Rensa tidigare resultat

            // Rendera nya resultat
            data.results.forEach(user => {
                const userElement = document.createElement("li");
                userElement.classList.add("searchLi");
                userElement.textContent = user;

                // Lägg till klickhändelse på varje namn
                userElement.addEventListener("click", () => {
                    searchInput.value = user; // Sätt input-fältet till användarens namn
                    resultsContainer.innerHTML = ""; // Rensa sökresultat
                    resultsContainer.classList.remove("searchCss"); // Ta bort CSS-klassen
                });

                resultsContainer.appendChild(userElement);
            });
        })
        .catch(error => {
            console.error("Ett fel uppstod:", error);
        });
}


function searchUsersGlobal() {
    const searchInput = document.getElementById("searchInputUser");
    const searchQuery = searchInput.value.trim(); // Ta bort eventuella mellanslag
    const resultsContainer = document.getElementById("searchResultsUser");
    const searchButton = document.getElementById("searchButton");

    // Dölj knappen som standard
    searchButton.style.display = "none";

    // Hantera CSS-klassen baserat på om fältet är tomt
    if (searchQuery === "") {
        resultsContainer.classList.remove("searchCss");
        resultsContainer.innerHTML = ""; // Rensa sökresultat
        return; // Avsluta om fältet är tomt
    }

    // Lägg till CSS-klassen om det finns en sökfråga
    resultsContainer.classList.add("searchCss");

    // Hämta sökresultat via API
    fetch(`/User/SearchUser?query=${encodeURIComponent(searchQuery)}`)
        .then(response => response.json())
        .then(data => {
            resultsContainer.innerHTML = ""; // Rensa tidigare resultat

            // Rendera nya resultat
            data.results.forEach(user => {
                const userElement = document.createElement("li");
                userElement.classList.add("searchLi");

                userElement.textContent = user;

                // Lägg till klickhändelse på varje namn
                userElement.addEventListener("click", () => {
                    searchInput.value = user; // Sätt input-fältet till användarens namn
                    resultsContainer.innerHTML = ""; // Rensa sökresultat
                    resultsContainer.classList.remove("searchCss"); // Ta bort CSS-klassen
                    searchButton.style.display = "inline-block"; // Visa knappen
                });

                resultsContainer.appendChild(userElement);
            });
        })
        .catch(error => {
            console.error("Ett fel uppstod:", error);
        });
}

//Skrollar till botten i konversation meddelande
document.addEventListener("DOMContentLoaded", function () {
    var container = document.getElementById("messageContainer");
    setTimeout(function () {
        container.scrollTop = container.scrollHeight;
    }, 500); // Kör efter att innehållet är klart.
});


//Sätter medelande notifikation

document.addEventListener("DOMContentLoaded", function () {
    fetch('/Message/GetMessageCount')
        .then(response => response.json())
        .then(data => {
            const count = data.count; // Hämta count från JSON-objektet
            const messageNotification = document.getElementById('messageNotification');

            if (count >= 0) {
                messageNotification.textContent = count;
                messageNotification.style.display = 'inline-block'; // Visa notisen
            } else {
                messageNotification.style.display = 'none'; // Dölj notisen om inga meddelanden
            }
        })
        .catch(error => console.error('Error fetching message count:', error));
});




