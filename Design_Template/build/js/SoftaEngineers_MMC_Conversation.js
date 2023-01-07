tinymce.init({
    selector: 'textarea',
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

function viewAttachmentsModal(id) {
    $('#divViewAttachmentsModal' + id).modal('toggle');

    responseStatus = false;
    return responseStatus;
}

function viewAddReplyModal() {
    $('#divAddReplyModal').modal('toggle');

    responseStatus = false;
    return responseStatus;
}

function viewCloseCaseModal() {
    $('#divCloseCaseModal').modal('toggle');

    responseStatus = false;
    return responseStatus;
}

function AddReplyValidation() {
    var varValidationStatus = true;

    tinyMCE.triggerSave();
    if (tinyMCE.get('conversation_replyMessage').getContent().length <= 0) {
        showRichEditorError(Sample_Field_RequiAddReplyValidationred);
        varValidationStatus = false;
    } else {
        if (!(tinyMCE.get('conversation_replyMessage').getContent({ format: 'text' }).match(TextArea_Regex))) {
            showRichEditorError(SpecialCharactersNotAllowed);
            varValidationStatus = false;
        } else {
            $(".mce-panel").css("background-color", "#acd2b8");
            $("#infoMessage").hide("fast");
        }
    }

    if (varValidationStatus) {
        $('#divAddReplyModal').modal('toggle');
    }

    return varValidationStatus;
}

function showRichEditorError(ErrorMessage) {
    $(".mce-panel").css("background-color", "#ffe3bb");
    $("#infoMessage").show("fast");
    $("#infoMessage").text(ErrorMessage);

    $("#conversation_replyAttachments").focus().select();
    tinyMCE.get('conversation_replyMessage').focus();
}