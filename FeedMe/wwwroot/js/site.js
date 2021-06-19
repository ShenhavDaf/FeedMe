// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

////////////////////////////////    BASIC SYNTAX EXAMPLES   ////////////////////////////////
//$(function () {
//    alert('myButton11')

//    $('#myButton1').on('click', function (e) {
//        e.preventDefault();
//        alert('myButton12')
//    })
//});

//$(document).ready(function () {
//    alert('myButton21')

//    $('#myButton2').click(function () {
//        alert('myButton22')
//    });
//});
////////////////////////////////    BASIC SYNTAX EXAMPLES   ////////////////////////////////



//const options = {
//    bottom: '32px', // default: '32px'
//    right: '32px', // default: '32px'
//    left: 'unset', // default: 'unset'
//    time: '0.3s', // default: '0.3s'
//    mixColor: '#fff', // default: '#fff'
//    backgroundColor: '#fff',  // default: '#fff'
//    buttonColorDark: '#100f2c',  // default: '#100f2c'
//    buttonColorLight: '#fff', // default: '#fff'
//    saveInCookies: true, // default: true,
//    label: '🌓', // default: ''
//}


//const darkmode = new Darkmode(options);
//darkmode.showWidget();



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

function GetMap() {
    var map = new Microsoft.Maps.Map('#myMap', {
        credentials: 'AgKM2ujhhQsxdOGDplWe_PcRt59_Y8KhlxcQsY4bWurgt-M8nkXVXWERyEd0z6pf',
        /* No need to set credentials if already passed in URL */
        center: new Microsoft.Maps.Location(31.9700919, 34.77205380048267),
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
        zoom: 8
    });


    let name;
    const bing_key = 'AgKM2ujhhQsxdOGDplWe_PcRt59_Y8KhlxcQsY4bWurgt-M8nkXVXWERyEd0z6pf';
    var pin;
    var pin_location;
    let count = 0;
    $.ajax({
        url: 'Cities/GetCitiesList',
        type: 'GET',
        success: function (data) {
            $.each(data, function (index) {
                setTimeout(() => {
                    name = data[index].name;
                    console.log(name);

                    pin_location = getLatLon(name, bing_key);
                    //console.log(pin_location);

                    pin = new Microsoft.Maps.Pushpin(pin_location);
                    map.entities.push(pin);
                }, index * 200);
            });
            //console.log(data);
            ////////////////////////////////        $.each WITH TIMEOUT      ////////////////////////////////
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getLatLon(query, bing_key) {
    var latlon;
    var mapObject;
    $.ajax({
        method: 'GET',
        url: `https://dev.virtualearth.net/REST/v1/Locations?q=${query}&key=${bing_key}`,
        async: false,
        success: function (data) {
            latlon = data.resourceSets[0].resources[0].point.coordinates;
            mapObject = new Microsoft.Maps.Location(latlon[0], latlon[1])
            //console.log(data);
            //console.log(latlon[0] + ',' + latlon[1]);
            //console.log(mapObject)
            //return mapObject;
        },
        error: function (err) {
            console.log(err);
        }
    });
    return mapObject;
}




// POST TO FACEBOOK AFTER NEW DISH CREATED
$(function () {
    $('#postToFbButton').click(function () {
        alert("JS Clicked");
        e.preventDefault();
        var page_id = '105380358425572';
        var msg = "Test1";
        var page_access_token = 'EAAMUCFVTWL0BAHyugkuovWBw5uXzDkVkWrtxSLSdYXZAb8VhDE0MCzy3Av9JYUwlJJSQ64Cv6C8Qb9cZAjFEqXWDeW7UZATt7t9hWcb3fnim0zokzLxGKB6nvN5mU6bDrZAUFOXDzHhTfqi9fcc8jkyuZAlKOReOYEsoZCoQWN3Hy1Rp0WZCHHIqEc986s591S7AKgD3B6hnHgH960jnA7v';


        postToFacebook(page_id, msg, page_access_token);
        //$.ajax({
        //    method: 'GET',
        //    url: 'https://graph.facebook.com/' + page_id + '?message=' + msg + '&access_token=' + page_access_token
        //}).done(function () {
        //    alert('Succeed Posting a post');
        //}).fail(function () {
        //    alert('Error, Something went wrong');
        //}).always(function () {
        //    alert('AJAX Clicked');
        //});
    });
});

function postToFacebook(page_id, msg, page_access_token) {
    alert('AJAX Clicked');
    $.ajax({
        method: 'POST',
        url: 'https://graph.facebook.com/105380358425572/feed?message=HelloFans1111!&access_token=EAAMUCFVTWL0BAMhxIJkFQRBOZCmyOffnTkAlonCOj8U8ILB2O943aBqpOOMIou6MEduKppMUM9TcO67yPcQaqEchD2pTvC4FPsJwkQ6SIZAzgbhFhIgrFN50w5QofVWQayq4sIf5AVqWg7fCxtxPEHDDZCtyLmBFczn1kqmMIyWZBQQHOTrf'
    }).done(function () {
        alert('Succeed Posting a post');
    }).fail(function () {
        alert('Error, Something went wrong');
    }).always(function () {
        alert('AJAX Clicked');
    });
}




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


var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
    return new bootstrap.Popover(popoverTriggerEl)
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

$(function () {
    $(".button").on("click", function () {
        var $button = $(this);
        var $parent = $button.parent();
        var oldValue = $parent.find('.input').val();

        if ($button.text() == "+") {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 1) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 1;
            }
        }
        $parent.find('a.add-to-cart').attr('data-quantity', newVal);
        $parent.find('.input').val(newVal);
    });
});


var count;

function starmark(item) {
    count = item.id[0];
    sessionStorage.starRating = count;
    var subid = item.id.substring(1);
    for (var i = 0; i < 5; i++) {
        if (i < count) {
            document.getElementById((i + 1) + subid).style.color = "orange";
        }
        else {
            document.getElementById((i + 1) + subid).style.color = "black";
        }


    }

}

function result() {
    //Rating : Count
    alert("Rating : " + count);
}

/*function result() {
    $.ajax({
        method: 'post',
        url: '/Restaurants/EditRate',
        data: { 'Rate': count }
    }).done(function (data) {
        alert("Rating : " + count);
        *//*console.log(data);*//*
    });
}*/