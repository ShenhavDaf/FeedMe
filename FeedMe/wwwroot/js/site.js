// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const options = {
    bottom: '32px', // default: '32px'
    right: '32px', // default: '32px'
    left: 'unset', // default: 'unset'
    time: '0.3s', // default: '0.3s'
    mixColor: '#fff', // default: '#fff'
    backgroundColor: '#fff',  // default: '#fff'
    buttonColorDark: '#100f2c',  // default: '#100f2c'
    buttonColorLight: '#fff', // default: '#fff'
    saveInCookies: true, // default: true,
    label: '🌓', // default: ''
}

/*const darkmode = new Darkmode(options);
darkmode.showWidget();*/


var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
    return new bootstrap.Popover(popoverTriggerEl)
})

/* -- quantity box in dish-- */

/*$(document).ready(function () {
    $('.count').prop('disabled', true);
    $(document).on('click', '.plus', function () {
        $('.count').val(parseInt($('.count').val()) + 1);
    });
    $(document).on('click', '.minus', function () {
        $('.count').val(parseInt($('.count').val()) - 1);
        if ($('.count').val() == 0) {
            $('.count').val(1);
        }
    });
});
*/
/*$(function () {
    $('.plus').click(function () {
        $('.count').val(parseInt($('.count').val()) + 1);
    });
});

$(function () {
    $('.minus').click(function () {
        $('.count').val(parseInt($('.count').val()) - 1);
        if ($('.count').val() == 0) {
            $('.count').val(1);
        };
    });
});*/


/*$(document).ready(function () {
    $('#qty_input').prop('disabled', true);
    $('#plus-btn').click(function () {
        $('#qty_input').val(parseInt($('#qty_input').val()) + 1);
    });
    $('#minus-btn').click(function () {
        $('#qty_input').val(parseInt($('#qty_input').val()) - 1);
        if ($('#qty_input').val() == 0) {
            $('#qty_input').val(1);
        }

    });
});*/

/*$(document).ready(function () {
    $('button').click(function (e) {
        var button_classes, value = +$('.counter').val();
        button_classes = $(e.currentTarget).prop('class');
        if (button_classes.indexOf('up_count') !== -1) {
            value = (value) + 1;
        } else {
            value = (value) - 1;
        }
        value = value < 0 ? 0 : value;
        $('.counter').val(value);
    });
    $('.counter').click(function () {
        $(this).focus().select();
    });
});*/
/*
$("input[type='number']").inputSpinner()

var $changedInput = $("#changedInput")
var $valueOnInput = $("#valueOnInput")
var $valueOnChange = $("#valueOnChange")
$changedInput.on("input", function (event) {
    $valueOnInput.html($(event.target).val())
    // or $valueOnInput.html(event.target.value) // in vanilla js
    // or $valueOnInput.html($changedInput.val())
})
$changedInput.on("change", function (event) {
    $valueOnChange.html($(event.target).val())
})
*/






/* -- save dish id value-- */

/*$(function () {
    $('.addQuantity').click(function () {
        addToCart($(this).attr('quantity'));
    });
});*/

$(function () {
    $('.addDish').click(function () {
        addToCart($(this).attr('dish-id'), 1);
    });
});

function addToCart(id, quantity) {    
    $.ajax({
        method: 'post',
        url: '/CartItems/Create',
        data: { 'DishId': id, 'Quantity': quantity }
    }).done(function (data) {
        console.log(data);
    });
}



})

var popover = new bootstrap.Popover(document.querySelector('.popover-dismiss'), {
    trigger: 'focus'
})


function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
}

function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {
        console.log('User signed out.');
    });
}

function testing() {
    alert("TADA")
}