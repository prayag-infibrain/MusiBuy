function validfile(data) {
    $("#lbl_fileName").text("");
  

    var allowed_extensions = new Array($(data).attr("accept"));
    var file_extension = data.value.split('.').pop().toLowerCase();
    var valid = false;
    for (var i = 0; i < allowed_extensions.length; i++) {
        if (allowed_extensions[i].indexOf(file_extension) > -1) {
            valid = true;

            break;
        }
    }
    if (!valid) {
        $("#fileError").text(`Only ${allowed_extensions} format is valid`);
        $(data).val('');
        return false;
    }
    else {
        $("#fileError").text("");
        var filename = $('input[type=file]').val().split('\\').pop();
        $("#lbl_fileName").text(filename);
    }
}