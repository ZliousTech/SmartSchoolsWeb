function initMap(StartPointLat, StartPointLng, EndPointLat, EndPointLng, WayPointsLat, WayPointsLng) {

    //alert(StartPoint, EndPoint);
    //for (var i = 0; i < WayPoints.length; i++) {
    //    alert(WayPoints[i]);
    //}

    const directionsService = new google.maps.DirectionsService();
    const directionsRenderer = new google.maps.DirectionsRenderer();
    const map = new google.maps.Map(document.getElementById("map"), {
        zoom: 6,
        center: { lat: StartPointLat, lng: StartPointLng },
    });

    directionsRenderer.setMap(map);
    calculateAndDisplayRoute(directionsService, directionsRenderer,
                             StartPointLat, StartPointLng,
                             EndPointLat, EndPointLng,
                             WayPointsLat, WayPointsLng);
}

function calculateAndDisplayRoute(directionsService, directionsRenderer,
                                  StartPointLat, StartPointLng,
                                  EndPointLat, EndPointLng,
                                  WayPointsLat, WayPointsLng) {

    const waypts = [];

    for (let i = 0; i < WayPointsLat.length; i++) {
        waypts.push({
            location: { lat: WayPointsLat[i], lng: WayPointsLng[i] },
            stopover: true,
        });
    }

    const summaryPanel = document.getElementById("directions-panel");
    directionsService
        .route({
            origin: { lat: StartPointLat, lng: StartPointLng },
            destination: { lat: EndPointLat, lng: EndPointLng },
            waypoints: waypts,
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.DRIVING,
        })
        .then((response) => {
            directionsRenderer.setDirections(response);

            const route = response.routes[0];
            summaryPanel.innerHTML = "";

            var totalDist = 0;
            var totalTime = 0;
            for (let i = 0; i < route.legs.length; i++) {
                totalDist += route.legs[i].distance.value;
                totalTime += route.legs[i].duration.value;
            }
            totalDist = (totalDist / 1000).toFixed(2);
            totalTime = (totalTime / 60).toFixed(2);

            var minutes = totalTime % 60;
            var hours = Math.floor(totalTime / 60);
            var _minutes = padTo2Digits(minutes.toFixed(2));
            var _hours = padTo2Digits(hours);

            summaryPanel.innerHTML += "<h6>Tour distance: " + totalDist + " Kilometer</h6>";
            //summaryPanel.innerHTML += "<h6>Tour time: " + totalTime + " Minutes</h6>";
            summaryPanel.innerHTML += "<h6>Tour time in hours: " + _hours + ":" + _minutes + " Hour,  Tour time in minutes: " + totalTime + "</h6>";
        })
        .catch((e) => summaryPanel.innerHTML = "<h6><span>Directions request failed.</span></h6>");
        //.catch((e) => {
        //    summaryPanel.innerHTML = "<p>Directions request failed.</p>";
        //    window.alert("Directions request failed due to " + status);
        //});
}

function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

window.initMap = initMap;


