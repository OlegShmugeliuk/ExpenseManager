function togglePopup() {
  var popup = document.getElementById("popup");
  var overlay = document.querySelector(".overlay");
  if (popup.style.display === "block") {
    popup.style.display = "none";
    overlay.style.display = "none";
  } else {
    popup.style.display = "block";
    overlay.style.display = "block";
  }
}

function closePopup() {
  var popup = document.getElementById("popup");
  var overlay = document.querySelector(".overlay");
  popup.style.display = "none";
  overlay.style.display = "none";
}