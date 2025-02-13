
function CreateCurrentLocationMap() {
    debugger;
    var map, infoWindow, marker;
    map = new google.maps.Map(document.getElementById('myMap'), {
        center: { lat: -34.397, lng: 150.644 },
        zoom: 18
    });

    infoWindow = new google.maps.InfoWindow;

    // Try HTML5 geolocation.

    navigator.geolocation.getCurrentPosition(function (position) {
        var pos = {
            lat: position.coords.latitude,
            lng: position.coords.longitude
        };
        var myLatlng = new google.maps.LatLng(pos.lat, pos.lng);
        marker = new google.maps.Marker({
            map: map,
            position: myLatlng,
            draggable: true,
            icon: '../../Images/standingmanmarker.png'

        });
        $('#latitudemap').val(marker.getPosition().lat());
        $('#longitudemap').val(marker.getPosition().lng());
        infoWindow.setPosition(pos);
        infoWindow.setContent('تم تحديد موقعك الحالي ');
        infoWindow.open(map, marker);
        map.setCenter(pos);
    }, function () {
        handleLocationError(true, infoWindow, map.getCenter());
    });


}


function createMap() {
    var lat = $('#latitudemap').val();
    var lng = $('#longitudemap').val();
    var map;
    var marker;
    var myLatlng = new google.maps.LatLng(lat, lng);
    var geocoder = new google.maps.Geocoder();
    var infowindow = new google.maps.InfoWindow();

    var mapOptions = {
        zoom: 15,
        center: myLatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("myMap"), mapOptions);

    marker = new google.maps.Marker({
        map: map,
        position: myLatlng,
        draggable: true,
        icon: '../../Images/MapMarker.png'

    });

    geocoder.geocode({
        'latLng': myLatlng
    }, function (results, status) {
        if (status === google.maps.GeocoderStatus.OK) {
            if (results[0]) {
                $('#address').val(results[0].formatted_address);
                $('#latitudemap').val(marker.getPosition().lat());
                $('#longitudemap').val(marker.getPosition().lng());
                infowindow.setContent(results[0].formatted_address);
                infowindow.open(map, marker);
            }
        }
    });

    google.maps.event.addListener(map, 'click', function (event) {
        placeMarker(event.latLng);
    });

    function placeMarker(location) {



        if (marker === undefined) {
            marker = new google.maps.Marker({
                position: location,
                map: map,
                animation: google.maps.Animation.DROP
            });
        }
        else {
            marker.setPosition(location);
        }
        map.setCenter(location);
        geocoder.geocode({
            'latLng': marker.getPosition()
        }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    $('#address').val(results[0].formatted_address);
                    $('#latitudemap').val(marker.getPosition().lat());
                    $('#longitudemap').val(marker.getPosition().lng());
                    infowindow.setContent(results[0].formatted_address);
                    infowindow.open(map, marker);
                }
            }
        });
    }

    google.maps.event.addListener(marker, 'dragend', function () {

        geocoder.geocode({
            'latLng': marker.getPosition()
        }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                if (results[0]) {
                    $('#address').val(results[0].formatted_address);
                    $('#latitudemap').val(marker.getPosition().lat());
                    $('#longitudemap').val(marker.getPosition().lng());
                    infowindow.setContent(results[0].formatted_address);
                    infowindow.open(map, marker);
                }
            }
        });
    });

}
