tinymce.init({
    selector: 'textarea#RCVMissmatch_caseDescription',
    //height: 500,
    //menubar: false,
    plugins: [
        'advlist autolink lists link image charmap print preview anchor',
        'searchreplace visualblocks code fullscreen',
        'insertdatetime media table paste code help wordcount'
    ],
    toolbar: 'undo redo | formatselect | ' +
        'bold italic backcolor | alignleft aligncenter ' +
        'alignright alignjustify | bullist numlist outdent indent | ' +
        'removeformat | help'
});

$(document).ready(function () {
    $('#send').click(function () {

    });
});

var zoomVar = $('#MapZoomLevel').val();
var projectMapJEOJSON = $('#ProjectMapJEOJSON').val();
var CRMapJEOJSON = $('#CRMapJEOJSON').val();
var CRTurfCoordinates = $('#CRTurfCoordinates').val();
var CRMapDrawType = $('#CRMapDrawType').val();
var Sample1MapJEOJSON = $('#Sample1MapJEOJSON').val();
var CRSample1TurfCoordinates = $('#Sample1MapTurfCoordinates').val();
var Sample1MapShapeAdded = false;
var Sample2MapJEOJSON = $('#Sample2MapJEOJSON').val();
var CRSample2TurfCoordinates = $('#Sample2MapTurfCoordinates').val();
var Sample2MapShapeAdded = false;
var TurfCR;
var TurfSample1;
var TurfSample2;

var ProjectPolygon1 = L.polygon(JSON.parse(projectMapJEOJSON));
var ProjectPolygon2 = L.polygon(JSON.parse(projectMapJEOJSON));

var map1 = prepareLeafletMap('map1', ProjectPolygon1.getBounds().getCenter().lat
    , ProjectPolygon1.getBounds().getCenter().lng, zoomVar);
var map2 = prepareLeafletMap('map2', ProjectPolygon2.getBounds().getCenter().lat
    , ProjectPolygon2.getBounds().getCenter().lng, zoomVar);

var featureGroup1 = L.featureGroup().addTo(map1);
var featureGroup2 = L.featureGroup().addTo(map2);

var drawControl1 = prepareLeafletMap_DrawControl_MarkerOnly(featureGroup1, map1, varMarkerAllowed = true);
var drawControl2 = prepareLeafletMap_DrawControl_MarkerOnly(featureGroup2, map2, varMarkerAllowed = true);

ProjectPolygon1.addTo(map1);
ProjectPolygon2.addTo(map2);

if (CRMapDrawType == 'polyline') {
    var polyline1 = L.polyline(JSON.parse(CRMapJEOJSON), { color: "#ff0000", weight: 3, smoothFactor: 1 }).addTo(map1);
    var polyline2 = L.polyline(JSON.parse(CRMapJEOJSON), { color: "#ff0000", weight: 3, smoothFactor: 1 }).addTo(map2);
    TurfCR = turf.lineString(JSON.parse(CRTurfCoordinates));
} else if (CRMapDrawType == 'Polygon') {
    var CRPolygon1 = L.polygon(JSON.parse(CRMapJEOJSON), { color: "#ff0000", weight: 3, smoothFactor: 1 }).addTo(map1);
    var CRPolygon2 = L.polygon(JSON.parse(CRMapJEOJSON), { color: "#ff0000", weight: 3, smoothFactor: 1 }).addTo(map2);
    TurfCR = turf.polygon(JSON.parse(CRTurfCoordinates));
}

if (Sample1MapJEOJSON.length > 0) {
    var marker1 = L.marker(JSON.parse(Sample1MapJEOJSON)).addTo(featureGroup1);
    TurfSample1 = turf.point(JSON.parse(CRSample1TurfCoordinates));
    Sample1MapShapeAdded = true;
}

if (Sample2MapJEOJSON.length > 0) {
    var marker2 = L.marker(JSON.parse(Sample2MapJEOJSON)).addTo(featureGroup2);
    TurfSample2 = turf.point(JSON.parse(CRSample2TurfCoordinates));
    Sample2MapShapeAdded = true;
}

map1.on('draw:created', function (e) {
    Sample1MapShapeAdded = false;
    featureGroup1.clearLayers();
    // Each time a feaute is created, it's added to the over arching feature group
    featureGroup1.addLayer(e.layer);
    Sample1MapShapeAdded = true;
});

map2.on('draw:created', function (e) {
    Sample2MapShapeAdded = false;
    featureGroup2.clearLayers();
    // Each time a feaute is created, it's added to the over arching feature group
    featureGroup2.addLayer(e.layer);
    Sample2MapShapeAdded = true;
});

map1.on('draw:deleted', function (evt) {
    Sample1MapShapeAdded = false;
    featureGroup1.clearLayers();
});

map2.on('draw:deleted', function (evt) {
    Sample2MapShapeAdded = false;
    featureGroup2.clearLayers();
});

$('#CreateRCVMissmatchForm').submit(function () {
    return customizedFormValidation();
});

function customizedFormValidation() {
    var varValidationStatus = true;

    // Validate Sample 1 Details
    if ($('#RCVMissmatch_sample1_sampleMaker').val() != "" || $('#RCVMissmatch_sample1_sampleSize').val() != ""
        || $('#RCVMissmatch_sample1_sampleLength').val() != "" || $('#RCVMissmatch_sample1_sampleUnitID').val() != -1) {
        SampleRequired('sample1');
    } else {
        SampleNotRequired('sample1');
    }

    // Validate Sample 2 Details
    if ($('#RCVMissmatch_sample2_sampleMaker').val() != "" || $('#RCVMissmatch_sample2_sampleSize').val() != ""
        || $('#RCVMissmatch_sample2_sampleLength').val() != "" || $('#RCVMissmatch_sample2_sampleUnitID').val() != -1) {
        SampleRequired('sample2');
    } else {
        SampleNotRequired('sample2');
    }

    // Validate Sample1 Map
    if (Sample1MapShapeAdded == true) {
        TurfSample1 = turf.point(featureGroup1.toGeoJSON().features[0].geometry.coordinates);
        if (turf.booleanContains(TurfCR, TurfSample1)) {
            $('#Sample1MapJEOJSON').val(JSON.stringify(featureGroup1.toGeoJSON()));
        } else {
            varValidationStatus = false;
            localizedAlerts('Map_CRSample1_Validation_Location');
        }
    }

    // Validate Sample2 Map
    if (Sample2MapShapeAdded == true) {
        TurfSample2 = turf.point(featureGroup2.toGeoJSON().features[0].geometry.coordinates);
        if (turf.booleanContains(TurfCR, TurfSample2)) {
            $('#Sample2MapJEOJSON').val(JSON.stringify(featureGroup2.toGeoJSON()));
        } else {
            varValidationStatus = false;
            localizedAlerts('Map_CRSample2_Validation_Location');
        }
    }

    tinyMCE.triggerSave();
    //$('#RCVMissmatch_projectName').val(tinyMCE.get('RCVMissmatch_caseDescription').getContent({ format: 'text' }));
    if (tinyMCE.get('RCVMissmatch_caseDescription').getContent().length <= 0) {
        showRichEditorError(Sample_Field_Required);
        varValidationStatus = false;
    } else {
        var $regexname = "^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z|\r\n|\r|\n|\t]*[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ 0-9|\r\n|\r|\n|\t]*$";
        if (!(tinyMCE.get('RCVMissmatch_caseDescription').getContent({ format: 'text' }).match($regexname))) {
            showRichEditorError(SpecialCharactersNotAllowed);
            varValidationStatus = false;
        } else {
            $(".mce-panel").css("background-color", "#acd2b8");
            $("#infoMessage").hide("fast");
            //varValidationStatus = true;
        }
    }

    return varValidationStatus;
}

function showRichEditorError(ErrorMessage) {
    $(".mce-panel").css("background-color", "#ffe3bb");
    $("#infoMessage").show("fast");
    $("#infoMessage").text(ErrorMessage);

    $("#RCVMissmatch_sample1_sampleMaker").focus().select();
    tinyMCE.get('RCVMissmatch_caseDescription').focus();
}

// Sample Details Mandatory
function SampleRequired(sampleID) {
    $('#RCVMissmatch_' + sampleID + '_sampleMaker').rules('add', { required: true });
    $('#RCVMissmatch_' + sampleID + '_sampleMaker').attr('data-msg-required', Sample_Field_Required);

    $('#RCVMissmatch_' + sampleID + '_sampleMaker').attr('data-rule-rangelength', "3, 50");
    $('#RCVMissmatch_' + sampleID + '_sampleMaker').attr('data-msg-rangelength', Sample_Maker_Length);

    $('#RCVMissmatch_' + sampleID + '_sampleMaker').attr('data-rule-pattern', "^[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z]*[\u0600-\u065F\u066A-\u06EF\u06FA-\u06FFa-zA-Z-_ 0-9]*$");
    $('#RCVMissmatch_' + sampleID + '_sampleMaker').attr('data-msg-pattern', SpecialCharactersNotAllowed);
    ////////////////////

    $('#RCVMissmatch_' + sampleID + '_sampleSize').attr('data-rule-required', "true");
    $('#RCVMissmatch_' + sampleID + '_sampleSize').attr('data-msg-required', Sample_Field_Required);
    $('#RCVMissmatch_' + sampleID + '_sampleSize').rules('add', { required: true });

    $('#RCVMissmatch_' + sampleID + '_sampleSize').attr('data-rule-min', "0");
    $('#RCVMissmatch_' + sampleID + '_sampleSize').attr('data-msg-min', Sample_Field_Required);

    $('#RCVMissmatch_' + sampleID + '_sampleSize').attr('data-rule-rangelength', "1, 10");
    $('#RCVMissmatch_' + sampleID + '_sampleSize').attr('data-msg-rangelength', Sample_SizeLength_Length);
    ////////////////////

    $('#RCVMissmatch_' + sampleID + '_sampleLength').attr('data-rule-required', "true");
    $('#RCVMissmatch_' + sampleID + '_sampleLength').attr('data-msg-required', Sample_Field_Required);
    $('#RCVMissmatch_' + sampleID + '_sampleLength').rules('add', { required: true });

    $('#RCVMissmatch_' + sampleID + '_sampleLength').attr('data-rule-min', "0");
    $('#RCVMissmatch_' + sampleID + '_sampleLength').attr('data-msg-min', Sample_Field_Required);

    $('#RCVMissmatch_' + sampleID + '_sampleLength').attr('data-rule-rangelength', "1, 10");
    $('#RCVMissmatch_' + sampleID + '_sampleLength').attr('data-msg-rangelength', Sample_SizeLength_Length);
    ////////////////////
    $('#RCVMissmatch_' + sampleID + '_sampleUnitID').rules("add", { required: true, number: true, min: 0 });
    $('#RCVMissmatch_' + sampleID + '_sampleUnitID').attr('data-msg-min', Sample_Unit_Required);
}

// Sample Details Not Mandatory
function SampleNotRequired(sampleID) {
    $('#RCVMissmatch_' + sampleID + '_sampleMaker').rules('remove');
    $('#RCVMissmatch_' + sampleID + '_sampleMaker-error').text('');
    $('#RCVMissmatch_' + sampleID + '_sampleMaker').rules('add', { required: false });
    /////////////

    $('#RCVMissmatch_' + sampleID + '_sampleSize').rules('remove');
    $('#RCVMissmatch_' + sampleID + '_sampleSize-error').text('');
    $('#RCVMissmatch_' + sampleID + '_sampleSize').rules('add', { required: false });
    /////////////

    $('#RCVMissmatch_' + sampleID + '_sampleLength').rules('remove');
    $('#RCVMissmatch_' + sampleID + '_sampleLength-error').text('');
    $('#RCVMissmatch_' + sampleID + '_sampleLength').rules('add', { required: false });
    /////////////
    $('#RCVMissmatch_' + sampleID + '_sampleUnitID').val(-1);
    $('#RCVMissmatch_' + sampleID + '_sampleUnitID').rules('remove');
    $('#RCVMissmatch_' + sampleID + '_sampleUnitID-error').text('');
    $('#RCVMissmatch_' + sampleID + '_sampleUnitID').rules("add", { required: true, number: true, min: -1 });
}