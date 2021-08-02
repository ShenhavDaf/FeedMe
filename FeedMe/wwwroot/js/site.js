// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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
        url: 'https://' + new URL(window.location.host) + '/Cities/GetCitiesList',
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
    $('#postToFbButton').click(function (e) {
        e.preventDefault();
        var page_id = 'XXX';
        var msg = "TEXT TEXT TEXT";
        var page_access_token = 'XXX';

        postToFacebook(page_id, msg, page_access_token);
    });
});

function postToFacebook(page_id, msg, page_access_token) {
    $.ajax({
        method: 'POST',
        url: "XXX",
    }).done(function () {
        alert('Done');
    }).fail(function () {
        alert('Error');
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

//go back buttom
function goBack() {
    window.history.back();
}


// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0; // For Safari
    document.documentElement.scrollTop = 0; // For Chrome, Firefox, IE and Opera
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