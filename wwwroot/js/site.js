// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Hämta födelseåret från HTML
const birthYear = parseInt(document.getElementById('birth-year').textContent, 10);
// Hämta det aktuella året
const currentYear = new Date().getFullYear();
// Beräkna åldern
const age = currentYear - birthYear;
// Sätt åldern i elementet med ID "age"
document.getElementById('age').textContent = age;
