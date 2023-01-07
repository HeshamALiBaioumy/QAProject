function AjaxCall(url, data, type) {
    return $.ajax({
        url: url,
        type: type ? type : 'GET',
        //async: false,
        //cache: false,
        //timeout: 30000,
        data: data,
        beforeSend: function () {
            $("#imgloader").show();
        },
        contentType: 'application/json'
    });
}

function AjaxCallAsync(url, data, type) {
    return $.ajax({
        url: url,
        type: type ? type : 'GET',
        async: false,
        cache: false,
        timeout: 30000,
        data: data,
        beforeSend: function () {
            $("#imgloader").show();
        },
        contentType: 'application/json'
    });
}

function prepareLeafletMap(varDivID, varCenterLat, varCenterLng, varZoom) {
    L.mapbox.accessToken = 'pk.eyJ1IjoiZW5nbW9oYW1lZGhlZ2F6eSIsImEiOiJja2txcXRlZzgwcDhjMnVwOGNqbjYzN2J0In0._gKwFux3FZpLya2xT_Z-Xw';
    var map = L.mapbox.map(varDivID).setView([varCenterLat, varCenterLng], varZoom);

    // Add Tile layers to the map
    L.control.layers({
        'Satellite Map': L.tileLayer('https://api.mapbox.com/styles/v1/mapbox/streets-v10/tiles/256/{z}/{x}/{y}?access_token='
            + L.mapbox.accessToken, { accessToken: L.mapbox.accessToken }).addTo(map),
        'OpenStreet Map': L.tileLayer('https://api.mapbox.com/styles/v1/mapbox/emerald-v8/tiles/{z}/{x}/{y}?access_token='
            + L.mapbox.accessToken, { accessToken: L.mapbox.accessToken }),
        'Esri Satellite Map': L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}?access_token='
            + L.mapbox.accessToken, { accessToken: L.mapbox.accessToken }),
    }).addTo(map);

    var WMS_layer = L.tileLayer.wms("http://173.212.219.178:8090/geoserver/amana/wms", {
        layers: 'dig-elec',
        format: 'image/png',
        uppercase: true,
        transparent: true,
        continuousWorld: true,
        tiled: true,
        info_format: 'text/html',
        opacity: 1,
        identify: false
    }).addTo(map);

    return map;
}

function prepareLeafletMap_DrawControl(varFeatureGroup, varMap, varPolyLineAllowed) {
    var drawControl = new L.Control.Draw(
        {
            draw: {
                position: 'topleft',
                polygon: {
                    allowIntersection: false,
                    drawError: {
                        color: '#e1e100',
                        message: 'Intersection Not allowed'
                    },
                    shapeOptions: {
                        color: '#97009c'
                    }
                },
                circle: false,
                polyline: varPolyLineAllowed,
                marker: false
            },
            edit: { featureGroup: varFeatureGroup }
        }
    ).addTo(varMap);

    return drawControl;
}

function prepareLeafletMap_DrawControl_MarkerOnly(varFeatureGroup, varMap, varMarkerAllowed) {
    var drawControl = new L.Control.Draw(
        {
            draw: {
                position: 'topleft',
                polygon: false,
                rectangle: false,
                polyline: false,
                circle: false,
                marker: varMarkerAllowed
            },
            edit: { featureGroup: varFeatureGroup }
        }
    ).addTo(varMap);

    return drawControl;
}

function LeafletMap_Convert_TxtGeoJson_To_Polygon(varTxtexportJEOJSON) {
    var polygon = L.polygon(JSON.parse(varTxtexportJEOJSON));
    return polygon;
}

function LeafletMap_Convert_Coordinates_To_TurfPolygon(varTxtturfCoordinates) {
    var TurfPolygon = turf.polygon(JSON.parse(varTxtturfCoordinates));
    return TurfPolygon;
}

function myFunction() {
    return Math.PI;
}