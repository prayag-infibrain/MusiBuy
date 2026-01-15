$(document).ready(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
    var n = $("input[name=chkDelete]");
    n.prop("checked", false);
    $("#allCheckBox").prop("checked", false);
});
$(function () {

    $("form input[data-val-remote-url]").on({
        focus: function () {
            $(this).closest('form').validate().settings.onkeyup = false;
        },
        blur: function () {
            $(this).closest('form').validate().settings.onkeyup = $.validator.defaults.onkeyup;
        }
    });
})
function Search() {
    $('#grid').data('kendoGrid').dataSource.page(1);
    $('#grid').data('kendoGrid').dataSource.read();
    $('#grid').data('kendoGrid').refresh();
}

function FilterData() {
    return {
        searchValue: $("#txtSearch").val()
    };
}

$("#frmUser").on("submit", function () {
    var langList = $("#LanguageLst").val();
    var errorCount = 0;
    if (langList != null && langList != undefined && langList != "") {
        var langArray = langList.split(",");
        $.each(langArray, function (index, value) {
            var errorId = "#error_" + value;
            var descError = "#errorDesc_" + value;
            var nId = "#VideoTitle_" + value;
            var descId = "#VideoDescription_" + value;
            var value = $(nId).val().trim();
            var descValue = $(descId).val().trim();
            if (value == null || value == undefined || value == "") {
                errorCount = errorCount + 1;
                $(errorId).removeClass('d-none');
            }
            else {
                $(errorId).addClass('d-none');
            }
            /*if (descValue == null || descValue == undefined || descValue == "") {
                errorCount = errorCount + 1;
                $(descError).removeClass('d-none');
            }
            else {
                $(descError).addClass('d-none');
            }*/

        });
    }
    if (errorCount == 0) {
        return true;
    }
    else {
        return false;
    }
})