function validateForm() {
    var x = document.forms["Pay"]["email"].value;
    if (x == "") {
        alert("Name must be filled out");
        return false;
    }
}