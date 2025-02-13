async function initMap() {
    let plat = 30.056034, plng = 31.241401;
    $("#hdlat").val(plat);
    $("#hdlng").val(plng);

    // Request needed libraries.
    const { Map, InfoWindow } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    const map = new Map(document.getElementById("map"), {
        center: { lat: plat, lng: plng },
        zoom: 14,
        mapId: "DEMO_MAP_ID",
        mapTypeId: 'roadmap',
    });

    infoWindow = new google.maps.InfoWindow();

    const draggableMarker = new AdvancedMarkerElement({
        map,
        position: { lat: plat, lng: plng },
        gmpDraggable: true
    });

    draggableMarker.addListener("dragend", (event) => {
        const position = draggableMarker.position;

        $("#hdlat").val(position.lat);
        $("#hdlng").val(position.lng);

        infoWindow.close();
        infoWindow.setContent(`Lat: ${position.lat}, Lng: ${position.lng}`);
        infoWindow.open(draggableMarker.map, draggableMarker);
    });
}

initMap();