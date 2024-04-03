
    var modal = document.getElementById("depositSuccessModal");

    var span = document.getElementsByClassName("close")[0];

    if (depositResult === false) {
        modal.style.display = "block";
}

    span.onclick = function() {
        modal.style.display = "none";
}
    window.onclick = function(event) {
  if (event.target == modal) {
        modal.style.display = "none";
  }
}
