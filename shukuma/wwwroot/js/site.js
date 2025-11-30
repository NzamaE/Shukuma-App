//documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification

document.getElementById("closeAlert").onclick = closeAlert;

function closeAlert(event) {
    document.getElementById("closeAlert").remove();
    document.getElementById("alert").remove();
}