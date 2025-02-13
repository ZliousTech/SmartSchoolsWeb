function initMap() {
    var map;
    var MyPolygon = null;
    var Circle = null;
    let Objs_area = [];
    let markers = [];
    const infoWindow = new google.maps.InfoWindow();

    const CenterPoint = { lat: 30.044225, lng: 31.235584 };
    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 7,
        center: CenterPoint,
        mapTypeId: 'roadmap',
        mapTypeControl: true,
        mapTypeControlOptions: {
            position: google.maps.ControlPosition.TOP_RIGHT
        },
        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.DEFAULT,
            position: google.maps.ControlPosition.RIGHT_TOP
        },
        streetViewControl: true,
        streetViewControlOptions: {
            position: google.maps.ControlPosition.RIGHT_TOP
        },
        panControl: true,
        panControlOptions: {
            position: google.maps.ControlPosition.RIGHT_TOP
        },
        scaleControl: false,
        scaleControlOptions: {
            position: google.maps.ControlPosition.BOTTOM_RIGHT
        }
    });

    google.maps.event.addListener(map, 'click', function () {
        infoWindow.close();
    });

    function addMarker(position, InfoContent, micon) {
        const marker = new google.maps.Marker({
            position,
            icon: "icons/" + micon,
            map,
            title: "",
            animation: google.maps.Animation.DROP
        });

        google.maps.event.addListener(marker, 'click', function (event) {
            infoWindow.close();
            infoWindow.setContent(InfoContent);
            infoWindow.open(marker.getMap(), marker);
        });

        markers.push(marker);
    }

    function setMarkersToMap(map) {
        for (let i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    function clearMarkers() {
        for (let i = 0; i < markers.length; i++) {
            markers[i].setMap(null);
        }
        markers = [];
    }

    $(document).ready(function () {
        var CompanyInfo = GetHeadQuarterInfo($("#hduid").val());

        var infoWindowContent =
            '<hr>' +
            '<p style="color:red;">' +
            '<strong>' +
            CompanyInfo[2] +
            '</strong>' +
            '</p>';

        map.panTo(new google.maps.LatLng(parseFloat(CompanyInfo[0]), parseFloat(CompanyInfo[1])));
        clearMarkers();
        addMarker(new google.maps.LatLng(parseFloat(CompanyInfo[0]), parseFloat(CompanyInfo[1])), infoWindowContent, "mHQ.png");
        setMarkersToMap(map);

        var SchoolsInfo = GetSchoolsInfo($("#hduid").val());
        for (let indx = 0; SchoolsInfo[indx] !== null; indx++) {
            if (SchoolsInfo[indx].length > 0) {
                var _SchoolInfo = SchoolsInfo[indx].split('|');
                var _SchoolInfoLatLng = _SchoolInfo[0].split(',');

                infoWindowContent =
                    '<hr>' +
                    '<p style="color:red;">' +
                    '<strong>' +
                    _SchoolInfo[1] +
                    '</strong>' +
                    '</p>';

                addMarker(new google.maps.LatLng(parseFloat(_SchoolInfoLatLng[0]), parseFloat(_SchoolInfoLatLng[1])), infoWindowContent, "mSch.png");
                setMarkersToMap(map);
            }
        }
    }); //document.ready
}