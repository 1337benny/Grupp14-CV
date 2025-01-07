
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

//Sökfunktion för användare
function searchUsers() {
    const searchQuery = document.getElementById("searchInput").value;

    fetch(`/User/Search?query=${encodeURIComponent(searchQuery)}`)
        .then(response => response.json())
        .then(data => {
            const resultsContainer = document.getElementById("searchResults");
            resultsContainer.innerHTML = "";

            data.results.forEach(user => {
                const userElement = document.createElement("li");
                userElement.textContent = user;
                resultsContainer.appendChild(userElement);
            });
        })
        .catch(error => {
            console.error("Ett fel uppstod:", error);
        });
}
