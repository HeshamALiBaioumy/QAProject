//      Fetch CR Types
$(function () {
    try {
        $('#CR_projectID').on("change", function () {
            var ProjectItems = $('#CR_projectItemID');
            var parameters = { projectID: $('#CR_projectID').val() };
            AjaxCall('/CR/getProjectItems', JSON.stringify(parameters), 'POST').done(function (response) {
                ProjectItems.empty();
                if (response.lOVProjectItems.length > 0) {
                    $.each(response.lOVProjectItems, function (index, item) {
                        ProjectItems.append($('<option></option>').val(item.id).text(item.value));
                    });
                }

                if (response.mapSelection != null) {
                    updateMapDisplay(response.mapSelection.centerLatitude, response.mapSelection.centerLongitude
                        , parseInt(response.mapSelection.zoomLevel), response.mapSelection.exportJEOJSON
                        , response.mapSelection.turfCoordinates);
                }

                $("#imgloader").hide();
            }).fail(function (error) {
                alert(error.StatusText);
                $("#imgloader").hide();
            });
        });

        $('#CR_CRTypeMCID').on("change", function () {
            var CRTypeGroups = $('#CR_CRTypeGroupID');
            var parameters = { MCID: $('#CR_CRTypeMCID').val() };
            AjaxCall('/CR/getCRMCGroups', JSON.stringify(parameters), 'POST').done(function (response) {
                if (response.length > 0) {
                    CRTypeGroups.empty();
                    $.each(response, function (index, item) {
                        CRTypeGroups.append($('<option></option>').val(item.id).text(item.value));
                    });
                }
                $("#imgloader").hide();
            }).fail(function (error) {
                alert(error.StatusText);
                $("#imgloader").hide();
            });
        });

        $('#CR_CRTypeGroupID').on("change", function () {
            var CRTypes = $('#CR_CRTypeID');
            var parameters = { CRTypeID: $('#CR_CRTypeGroupID').val() };
            AjaxCall('/CR/getCRTypes', JSON.stringify(parameters), 'POST').done(function (response) {
                if (response.length > 0) {
                    CRTypes.empty();
                    $.each(response, function (index, item) {
                        CRTypes.append($('<option></option>').val(item.id).text(item.value));
                    });
                }
                $("#imgloader").hide();
            }).fail(function (error) {
                alert(error.StatusText);
                $("#imgloader").hide();
            });
        });

        $('#CR_CRTypeID').on("change", function () {
            var parameters = { CRTypeID: $('#CR_CRTypeID').val() };
            AjaxCall('/CR/checkCRTypeSampleRequired', JSON.stringify(parameters), 'POST').done(function (response) {
                var isSampleRequired = (response == true);

                $("#CR_sample_sampleMaker").prop('disabled', !(isSampleRequired));
                $("#CR_sample_sampleSize").prop('disabled', !(isSampleRequired));
                $("#CR_sample_sampleLength").prop('disabled', !(isSampleRequired));
                $("#CR_sample_sampleUnitID").prop('disabled', !(isSampleRequired));

                if (isSampleRequired == true) {
                    SampleRequired();
                } else {
                    SampleNotRequired();
                }

                $("#imgloader").hide();
            }).fail(function (error) {
                alert(error.StatusText);
                $("#imgloader").hide();
            });
        });

    } catch (ex) {
        alert(ex);
    }
});

function SampleRequired() {
    $('#CR_sample_sampleMaker').attr('data-rule-required', "true");
    $('#CR_sample_sampleMaker').attr('data-msg-required', "Please provide Sample Maker");

    $('#CR_sample_sampleMaker').attr('data-rule-rangelength', "3, 50");
    $('#CR_sample_sampleMaker').attr('data-msg-rangelength', 'Sample maker field length between 3 - 50 digits');

    //$('#CR_sample_sampleMaker').attr('data-rule-pattern', "^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]*[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ 0-9]*$");
    //$('#CR_sample_sampleMaker').attr('data-msg-pattern', 'Special Characters Not Allowed');

    ////////////////////

    $('#CR_sample_sampleSize').attr('data-rule-required', "true");
    $('#CR_sample_sampleSize').attr('data-msg-required', "Please Provide sample size");

    $('#CR_sample_sampleSize').attr('data-rule-min', "0");
    $('#CR_sample_sampleSize').attr('data-msg-min', 'Please Provide sample size');

    $('#CR_sample_sampleSize').attr('data-rule-rangelength', "1, 10");
    $('#CR_sample_sampleSize').attr('data-msg-rangelength', 'Sample size field length between 1 - 10 digits');

    ////////////////////

    $('#CR_sample_sampleLength').attr('data-rule-required', "true");
    $('#CR_sample_sampleLength').attr('data-msg-required', "Plesae Provide sample length");

    $('#CR_sample_sampleLength').attr('data-rule-min', "0");
    $('#CR_sample_sampleLength').attr('data-msg-min', 'Plesae Provide sample length');

    $('#CR_sample_sampleLength').attr('data-rule-rangelength', "1, 10");
    $('#CR_sample_sampleLength').attr('data-msg-rangelength', 'Sample length field length between 1 - 10 digits');

    ////////////////////

    $('#CR_sample_sampleUnitID').attr('data-rule-min', "0");
    $('#CR_sample_sampleUnitID').attr('data-msg-min', 'Please Select sample unit');
}

function SampleNotRequired() {
    $('#CR_sample_sampleMaker').rules('remove');
    $('#CR_sample_sampleMaker-error').text('');

    $('#CR_sample_sampleSize').rules('remove');
    $('#CR_sample_sampleSize-error').text('');

    $('#CR_sample_sampleLength').rules('remove');
    $('#CR_sample_sampleLength-error').text('');

    $('#CR_sample_sampleUnitID').val(-1);
    $('#CR_sample_sampleUnitID').rules('remove');
    $('#CR_sample_sampleUnitID-error').text('');
}

var TurfProjectPolygon;
function updateMapDisplay(centerLatitude, centerLongitude, zoomLevel, exportJEOJSON, turfCoordinates) {
    //        Map Script
    $("#MapZoomLevel").val(zoomLevel)
    clearMap();

    var polygon = L.polygon(JSON.parse(exportJEOJSON)).addTo(map);
    map.panTo(polygon.getBounds().getCenter(), zoomLevel);
    TurfProjectPolygon = turf.polygon(JSON.parse(turfCoordinates));
    return true;
}

function clearMap() {
    for (i in map._layers) {
        if (map._layers[i]._path != undefined) {
            try {
                map.removeLayer(map._layers[i]);
            }
            catch (e) {
                console.log("problem with " + e + map._layers[i]);
            }
        }
    }
}

var mapProjectShapeAdded = false;

var map = prepareLeafletMap('map', 24.394944, 39.620850, 13);
var featureGroup = L.featureGroup().addTo(map);
var drawControl = prepareLeafletMap_DrawControl(featureGroup, map, varPolyLineAllowed = true);

var CRDrawType;
map.on('draw:created', function (e) {
    mapProjectShapeAdded = false;
    featureGroup.clearLayers();
    // Each time a feaute is created, it's added to the over arching feature group

    CRDrawType = e.layerType;
    featureGroup.addLayer(e.layer);
    mapProjectShapeAdded = true;
});

map.on('draw:deleted', function (evt) {
    mapProjectShapeAdded = false;
    featureGroup.clearLayers();
});

if (isCRUpdate == true) {
    mapProjectShapeAdded = false;

    updateMapDisplay($("#MapCenterLatitude").val(), $("#MapCenterLongitude").val()
        , parseInt($("#MapZoomLevel").val()), $("#MapJEOJSON").val()
        , $("#turfCoordinates").val());
}

function validateNextStep(stepnumber) {
    return $('#CRForm').valid();
}

function customizedFormValidation() {
    var varValidationStatus = $('#CRForm').valid();

    // Validate Map Step
    if (varValidationStatus == true) {
        if (mapProjectShapeAdded == true) {
            var TurfCR;
            if (CRDrawType === 'polygon' || CRDrawType === 'rectangle') {
                TurfCR = turf.polygon(featureGroup.toGeoJSON().features[0].geometry.coordinates);
            } else if (CRDrawType === 'polyline') {
                TurfCR = turf.lineString(featureGroup.toGeoJSON().features[0].geometry.coordinates);
            }

            if (turf.booleanContains(TurfProjectPolygon, TurfCR)) {
                $('#MapJEOJSON').val(JSON.stringify(featureGroup.toGeoJSON()));
            } else {
                varValidationStatus = false;
                localizedAlerts('Map_CR_Validation_Location');
            }
        } else {
            localizedAlerts('Map_Validation_Location');
            varValidationStatus = false;
        }
    }

    return varValidationStatus;
}

function customizedFormSubmit() {
    $('#CRForm').submit();
}