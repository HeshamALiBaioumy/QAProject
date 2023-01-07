//        Date Script
$(document).ready(function () {
    $("#project_startDate").datepicker({
        dateFormat: "dd-mm-yy",
        changeMonth: true,
        changeYear: true
    });

    $("#project_endDate").datepicker({
        dateFormat: "dd-mm-yy",
        changeMonth: true,
        changeYear: true
    });

    $("#project_registerDate").datepicker({
        dateFormat: "dd-mm-yy",
        changeMonth: true,
        changeYear: true
    });
});

//             Users Profile Selection Script
$(function () {
    try {
        $('#project_projectOwnerID').on("change", function () {
            var varSupervisorEngineers = $('#project_supervisorEngID');
            var varDepartments = $('#project_departmentID');
            var varDepartmentSections = $('#project_departmentSectionID');
            var parameters = { projectOwnerID: $('#project_projectOwnerID').val() };
            //alert(parameters.projectOwnerID);
            AjaxCall('/Project/getProjectOwner_Related', JSON.stringify(parameters), 'POST').done(function (response) {
                //alert('Inside AJAX ');
                $("#imgloader").hide();
                if (response.lOVSupervisorEngineers.length > 0) {
                    varSupervisorEngineers.empty();
                    $.each(response.lOVSupervisorEngineers, function (index, item) {
                        varSupervisorEngineers.append($('<option></option>').val(item.id).text(item.value));
                    });
                }
                if (response.lOVDepartments.length > 0) {
                    varDepartments.empty();
                    $.each(response.lOVDepartments, function (index, item) {
                        varDepartments.append($('<option></option>').val(item.id).text(item.value));
                    });

                    varDepartmentSections.empty();
                    $.each(response.lOVSections, function (index, item) {
                        varDepartmentSections.append($('<option></option>').val(item.id).text(item.value));
                    });
                }
            }).fail(function (error) {
                $("#imgloader").hide();
                alert(error.StatusText);
            });
        });

        $('#project_departmentID').on("change", function () {
            var varDepartmentSections = $('#project_departmentSectionID');
            var parameters = { departmentID: $('#project_departmentID').val() };
            AjaxCall('/Project/getDepartmentSections', JSON.stringify(parameters), 'POST').done(function (response) {
                $("#imgloader").hide();
                if (response.length > 0) {
                    varDepartmentSections.empty();
                    $.each(response, function (index, item) {
                        varDepartmentSections.append($('<option></option>').val(item.id).text(item.value));
                    });
                }
            }).fail(function (error) {
                $("#imgloader").hide();
                alert(error.StatusText);
            });
        });

        $('#project_consultantID').on("change", function () {
            var varConsultantAssistant = $('#project_consultantAssistantID');
            var parameters = { consultantID: $('#project_consultantID').val() };
            AjaxCall('/Project/getConsultant_Assistants', JSON.stringify(parameters), 'POST').done(function (response) {
                $("#imgloader").hide();
                if (response.length > 0) {
                    varConsultantAssistant.empty();
                    $.each(response, function (index, item) {
                        varConsultantAssistant.append($('<option></option>').val(item.id).text(item.value));
                    });
                }
            }).fail(function (error) {
                $("#imgloader").hide();
                alert(error.StatusText);
            });
        });

        $('#project_contractorID').on("change", function () {
            var varContractorAssistant = $('#project_contractorAssistantID');
            var parameters = { contractorID: $('#project_contractorID').val() };
            AjaxCall('/Project/getContractor_Assistants', JSON.stringify(parameters), 'POST').done(function (response) {
                $("#imgloader").hide();
                if (response.length > 0) {
                    varContractorAssistant.empty();
                    $.each(response, function (index, item) {
                        varContractorAssistant.append($('<option></option>').val(item.id).text(item.value));
                    });
                }
            }).fail(function (error) {
                $("#imgloader").hide();
                alert(error.StatusText);
            });
        });
    } catch (e) {
        alert('JS Error Occured: ' + e);
    }
});

//            Handle Project Milestones
$("body").on("click", "#btnAdd", function () {
    var txtProjMlstone_MileStone = $("#txtProjMlstone_MileStone");
    var txtProjMlstone_Description = $("#txtProjMlstone_Description");
    var txtProjMlstone_Amount = $("#txtProjMlstone_Amount");
    //var txtProjMlstone_Percentage = $("#txtProjMlstone_Percentage");
    var fieldsValidation = validateAddMilestoneFields();

    if (fieldsValidation == true) {
        //Get the reference of the Table's TBODY element.
        var tBody = $("#tblProjectMilestones > TBODY")[0];

        //Add Row.
        var row = tBody.insertRow(-1);

        //Add Milestone cell.
        var cell = $(row.insertCell(-1));
        cell.html(txtProjMlstone_MileStone.val());

        //Add Description cell.
        cell = $(row.insertCell(-1));
        cell.html(txtProjMlstone_Description.val());

        //Add Amount cell.
        cell = $(row.insertCell(-1));
        cell.html(txtProjMlstone_Amount.val());

        //Add Amount Unit cell.
        cell = $(row.insertCell(-1));
        cell.html('<input type="hidden" value="' + $("#ddlProjMlstone_Amt_Unit").val() + '">'
            + $("#ddlProjMlstone_Amt_Unit option:selected").text());

        //Add Percentage cell.
        //cell = $(row.insertCell(-1));

        //cell.html(txtProjMlstone_Percentage.val() + ' %');

        //Add Button cell.
        cell = $(row.insertCell(-1));
        var btnRemove = $("<button />");
        btnRemove.attr("type", "button");
        btnRemove.attr("onclick", "Remove(this);");
        btnRemove.attr("class", "close");
        btnRemove.attr("aria-label", "Close");
        btnRemove.html("<span aria-hidden='true' style='font - size: 20px; color: red'>&times;</span>");
        cell.append(btnRemove);

        //Clear the TextBoxes.
        clearInputFields();
    }
});

function validateAddMilestoneFields() {
    var fieldsValidation = true;

    hideErrorMessage();

    var varTxtMinLength = 5;
    var varTxtMaxLength = 50;

    var txtProjMlstone_MileStone = $("#txtProjMlstone_MileStone");
    if (txtProjMlstone_MileStone.val() == "") {
        fieldsValidation = false;
        showErrorMessage("MileStone", tbl_ProjMlstone_Val_Required);
    } else {
        if (txtProjMlstone_MileStone.val().length < varTxtMinLength || txtProjMlstone_MileStone.val().length > varTxtMaxLength) {
            fieldsValidation = false;
            showErrorMessage("MileStone", tbl_ProjMlstone_Val_Length);
        } else {
            if (!vartextRegex.test(txtProjMlstone_MileStone.val())) {
                fieldsValidation = false;
                showErrorMessage("MileStone", tbl_ProjMlstone_Val_SpecialCharacters);
            }
        }
    }

    var txtProjMlstone_Description = $("#txtProjMlstone_Description");
    if (txtProjMlstone_Description.val() == "") {
        fieldsValidation = false;
        showErrorMessage("Description", tbl_ProjMlstone_Val_Required);
    } else {
        if (txtProjMlstone_Description.val().length < varTxtMinLength || txtProjMlstone_Description.val().length > varTxtMaxLength) {
            fieldsValidation = false;
            showErrorMessage("Description", tbl_ProjMlstone_Val_Length);
        } else {
            if (!vartextRegex.test(txtProjMlstone_Description.val())) {
                fieldsValidation = false;
                showErrorMessage("Description", tbl_ProjMlstone_Val_SpecialCharacters);
            }
        }
    }

    var txtProjMlstone_Amount = $("#txtProjMlstone_Amount");
    if (txtProjMlstone_Amount.val() == "") {
        fieldsValidation = false;
        showErrorMessage("Amount", tbl_ProjMlstone_Val_Required);
    } else {
        if (!varNumericRegex.test(txtProjMlstone_Amount.val())) {
            fieldsValidation = false;
            showErrorMessage("Amount", tbl_ProjMlstone_Val_NumericField);
        }
    }


    var txtProjMlstone_Amt_Unit = $('#ddlProjMlstone_Amt_Unit').val();
    if (txtProjMlstone_Amt_Unit == -1) {
        fieldsValidation = false;
        showErrorMessage("Amt_Unit", tbl_ProjMlstone_Val_Required);
    }

    //var txtProjMlstone_Percentage = $("#txtProjMlstone_Percentage");
    //if (txtProjMlstone_Percentage.val() == "0") {
    //    fieldsValidation = false;
    //    showErrorMessage("Percentage", tbl_ProjMlstone_Val_Required);
    //}

    return fieldsValidation;
}

function showErrorMessage(varlalbelName, varErrorMessage) {
    $('#lblProjMlstone_' + varlalbelName).html(varErrorMessage);
    $('#lblProjMlstone_' + varlalbelName).show();
}

function hideErrorMessage() {
    $("#lblProjMlstone_MileStone").html("");
    $("#lblProjMlstone_Description").html("");
    $("#lblProjMlstone_Amount").html("");
    $("#lblProjMlstone_Amt_Unit").html("");
    //$("#lblProjMlstone_Percentage").html("");

    $("#lblProjMlstone_MileStone").hide();
    $("#lblProjMlstone_Description").hide();
    $("#lblProjMlstone_Amount").hide();
    $("#lblProjMlstone_Amt_Unit").hide();
    //$("#lblProjMlstone_Percentage").hide();
}

function clearInputFields() {
    $("#txtProjMlstone_MileStone").val("");
    $("#txtProjMlstone_Description").val("");
    $("#txtProjMlstone_Amount").val("");
    $("#ddlProjMlstone_Amt_Unit").val(-1)
    //$("#txtProjMlstone_Percentage").val("25");
}

function Remove(button) {
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    if (confirm("Do you want to delete: " + name)) {
        var table = $("#tblProjectMilestones")[0];
        table.deleteRow(row[0].rowIndex);
    }
};

function validateNextStep(stepnumber) {
    var isStepValid = true;
    if (stepnumber < 0) {
    } else if (stepnumber == 3) {
        // Validate Project Milestone Step
        isStepValid = validateProjectMilestones();
    } else {
        isStepValid = $('#ProjectForm').valid();
    }

    return isStepValid;
}

function validateProjectMilestones() {
    var isStepValid = true;

    if ($('#tblProjectMilestones TBODY TR').length <= 0) {
        localizedAlerts('MileStones_Validation');
        isStepValid = false;
    } else {
        //var totalMilestonesPercentage = 0;
        //$("#tblProjectMilestones TBODY TR").each(function () {
        //    totalMilestonesPercentage += parseInt($(this).find("TD").eq(4).html().substring(0
        //        , $(this).find("TD").eq(4).html().length - 1));
        //});

        //if (totalMilestonesPercentage != 100) {
        //    localizedAlerts('tbl_ProjMlstone_Percentage_Validation');
        //    isStepValid = false;
        //}
    }

    return isStepValid;
}

//        Handle Map Script
var mapProjectShapeAdded = 'false';
var displayOnUserLocationVar = $('#MapDisplayOnUserLocation').val();
var centerLatVar = $('#MapCenterLatitude').val();
var centerLngVar = $('#MapCenterLongitude').val();
var zoomVar = $('#MapZoomLevel').val();
var mapJEOJSON = $('#MapJEOJSON').val();

var map = prepareLeafletMap('map', centerLatVar, centerLngVar, zoomVar);
var featureGroup = L.featureGroup().addTo(map);
var drawControl = prepareLeafletMap_DrawControl(featureGroup, map, varPolyLineAllowed = false);

map.on('draw:created', function (e) {
    mapProjectShapeAdded = 'false';
    featureGroup.clearLayers();
    // Each time a feaute is created, it's added to the over arching feature group
    featureGroup.addLayer(e.layer);
    mapProjectShapeAdded = 'true';
});

map.on('draw:deleted', function (evt) {
    mapProjectShapeAdded = 'false';
    featureGroup.clearLayers();
});

if (isProjectUpdate == 'True') {
    // Draw Already Exist Map
    var mapJEOJSONParse = JSON.parse(mapJEOJSON);
    L.geoJson(mapJEOJSONParse, { onEachFeature: onEachFeature });
    mapProjectShapeAdded = 'true';

    //Take advantage of the onEachFeature callback to initialize drawnItems
    function onEachFeature(feature, layer) {
        featureGroup.addLayer(layer);
    }
} else {
    if (displayOnUserLocationVar == 'True') {
        navigator.geolocation.getCurrentPosition(function (location) {
            var currentUserCenter = new L.LatLng(location.coords.latitude, location.coords.longitude);
            map.setView(currentUserCenter, zoomVar);
        });
    }
}

function customizedFormValidation() {
    var varValidationStatus = true;

    // Validate and Submit Project milestones
    varValidationStatus = validateProjectMilestones();
    if (varValidationStatus == true) {
        var projectMilestones = new Array();
        $("#tblProjectMilestones TBODY TR").each(function () {
            var row = $(this);
            var projectMilestone = {};
            projectMilestone.name = row.find("TD").eq(0).html();
            projectMilestone.description = row.find("TD").eq(1).html();
            projectMilestone.amount = row.find("TD").eq(2).html();
            projectMilestone.amountUnitID = row.find("TD").eq(3).find('input[type=hidden]').val();
            //projectMilestone.percentage = parseInt(row.find("TD").eq(4).html().substring(0
            //    , row.find("TD").eq(4).html().length - 1));
            projectMilestones.push(projectMilestone);
        });

        var parameters = { projectMilestones: projectMilestones };
        AjaxCallAsync('/Project/setProjectMilestones', JSON.stringify(parameters), 'POST')
            .done(function (response) {
                varValidationStatus = true;
                $("#imgloader").hide();
            })
            .fail(function (error) {
                varValidationStatus = false;
                $("#imgloader").hide();
                alert(error.StatusText);
            });
    }

    // Validate Map Step
    if (varValidationStatus == true) {
        if (mapProjectShapeAdded == 'true') {
            $('#MapCenterLatitude').val(map.getCenter().lat);
            $('#MapCenterLongitude').val(map.getCenter().lng);
            $('#MapZoomLevel').val(map.getZoom());
            $('#MapJEOJSON').val(JSON.stringify(featureGroup.toGeoJSON()));
        } else {
            localizedAlerts('Map_Validation_ProjectLocation');
            varValidationStatus = false;
        }
    }

    return varValidationStatus;
}

function customizedFormSubmit() {
    $('#ProjectForm').submit();
}