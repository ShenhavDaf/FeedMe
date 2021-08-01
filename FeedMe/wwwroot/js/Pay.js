function cardnumber1(inputtxt) {
    var cardno = "^[1-9]{1}(?:[0-9]{15})?$";
    if (inputtxt.value.match(cardno)) {

        return true;
    }
    else {
        alert("Not a valid Visa credit card number!");
        return false;
    }
}