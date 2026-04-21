// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

function showAlert(message, duration = 3000) {
    var alertBox = document.getElementById('custom-alert');
    var textSpan = document.getElementById('alert-message');
    if (alertBox && textSpan) {
        textSpan.innerText = message;
        alertBox.style.display = 'block';
        setTimeout(function () {
            alertBox.style.display = 'none';
        }, duration);
    }
}