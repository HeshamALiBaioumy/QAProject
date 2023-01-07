var mapProjectShapeAdded = false;
var isLabSampleRequired = $('#isLabRequired').val() == 'True';
var zoomVar = $('#MapZoomLevel').val();
var projectMapJEOJSON = $('#ProjectMapJEOJSON').val();
var CRMapJEOJSON = $('#CRMapJEOJSON').val();
var CRMapDrawType = $('#CRMapDrawType').val();
var CRSampleMapJEOJSON = $('#CRSampleMapJEOJSON').val();
var CRTurfCoordinates = $('#CRTurfCoordinates').val();
var CRSampleTurfCoordinates = $('#CRSampleTurfCoordinates').val();
var TurfCR;
var TurfSample;

var ProjectPolygon = L.polygon(JSON.parse(projectMapJEOJSON));

var map = prepareLeafletMap('map', ProjectPolygon.getBounds().getCenter().lat
    , ProjectPolygon.getBounds().getCenter().lng, zoomVar);
var featureGroup = L.featureGroup().addTo(map);
var drawControl = prepareLeafletMap_DrawControl_MarkerOnly(featureGroup, map, varMarkerAllowed = isLabSampleRequired);
ProjectPolygon.addTo(map);

if (CRMapDrawType == 'polyline') {
    var polyline = L.polyline(JSON.parse(CRMapJEOJSON), { color: "#ff0000", weight: 3, smoothFactor: 1 }).addTo(map);
    TurfCR = turf.lineString(JSON.parse(CRTurfCoordinates));
} else if (CRMapDrawType == 'Polygon') {
    var CRPolygon = L.polygon(JSON.parse(CRMapJEOJSON), { color: "#ff0000", weight: 3, smoothFactor: 1 }).addTo(map);
    TurfCR = turf.polygon(JSON.parse(CRTurfCoordinates));
}

if (CRSampleMapJEOJSON.length > 0) {
    var marker = L.marker(JSON.parse(CRSampleMapJEOJSON)).addTo(map);
    TurfSample = turf.point(JSON.parse(CRSampleTurfCoordinates));
    mapProjectShapeAdded = true;
}

map.on('draw:created', function (e) {
    mapProjectShapeAdded = false;
    featureGroup.clearLayers();
    // Each time a feaute is created, it's added to the over arching feature group
    featureGroup.addLayer(e.layer);
    mapProjectShapeAdded = true;
});

map.on('draw:deleted', function (evt) {
    mapProjectShapeAdded = false;
    featureGroup.clearLayers();
});

//$('#ExecuteCRForm').submit(function () {
//    return customizedFormValidation();
//});

function AcceptCRValidation() {
    // Determine if the customer Accepted the CR or rejecetd it
    var isAcceptCR = true;
    return customizedFormValidation(isAcceptCR);
}

function RejectCRValidation() {
    var responseStatus = false;

    // Determine if the customer Accepted the CR or rejecetd it
    var isAcceptCR = false;
    responseStatus = customizedFormValidation(isAcceptCR);

    if (responseStatus == true) {
        $('#divViewItemModal').modal('toggle');
    }

    responseStatus = false;
    return responseStatus;
}

function RejectCRSubmit() {
    var responseStatus = false;

    // Determine if the customer Accepted the CR or rejecetd it
    var isAcceptCR = false;
    responseStatus = customizedFormValidation(isAcceptCR);

    if (responseStatus == true) {
        var rejectReason = $('#CR_rejectReason').val();
        if (rejectReason == "" || rejectReason.length < 5 || rejectReason.length > 250) {
            localizedAlerts('RejectCR_RejectReason_Validation');
            responseStatus = false;
        } else {
            $('#divViewItemModal').modal('toggle')
            responseStatus = true;
        }
    }

    return responseStatus;
}

function customizedFormValidation(isAcceptCR) {
    var varValidationStatus = true;

    // Validate Map
    if (varValidationStatus == true) {
        if (isLabSampleRequired == true) {
            if (isAcceptCR == true) {
                if (mapProjectShapeAdded == true) {
                    TurfSample = turf.point(featureGroup.toGeoJSON().features[0].geometry.coordinates);
                    $('#CRSampleMapJEOJSON').val(JSON.stringify(featureGroup.toGeoJSON()));
                    if (turf.booleanContains(TurfCR, TurfSample)) {
                        $('#CRSampleMapJEOJSON').val(JSON.stringify(featureGroup.toGeoJSON()));
                    } else {
                        varValidationStatus = false;
                        localizedAlerts('Map_CRSample_Validation_Location');
                    }
                } else {
                    localizedAlerts('Map_Validation_Location');
                    varValidationStatus = false;
                }
            }
        }
    }

    return varValidationStatus;
}