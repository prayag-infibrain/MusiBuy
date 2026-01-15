$(document).ready(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
    $('#loader').modal('hide');
});

function OnUserBound(n) {
    if (!n.sender.dataSource.view().length) { var t = n.sender.thead.find("th:visible").length, i = '<tr><td colspan="' + t + '" align="center">No records found.<\/td><\/tr>'; n.sender.tbody.parent().width(n.sender.thead.width()).end().html(i) }
}

function Search() {
    $('#grid').data('kendoGrid').dataSource.page(1);
    $('#grid').data('kendoGrid').dataSource.read();
    $('#grid').data('kendoGrid').refresh();
}

function FilterData() {
    return {
        enumValue: $("#txtEVSearch").val()
    };
}

function validateEnum() {
    var errorMessage = '<ul>';
    var isvalid = true;
    if ($("#EnumTypeId").val() == 0) {
        errorMessage += '<li>Please select enum type.</li>';
        isvalid = false;
    }
    if (($("#EnumTypeId").val() != 0 && $("#ParentTypeId").val() != 0)) {
        if ($("#EnumTypeId").val() == $("#ParentTypeId").val()) {
            errorMessage += '<li>Enum type and parent type can not be same.</li>';
            isvalid = false;
        }
    }
    if ($("#EnumValue").val().trim() == 0) {
        errorMessage += '<li>Please enter value.</li>';
        isvalid = false;
    }
    if ($("#ParentTypeId").val() == "") {
        errorMessage += '<li>Please select parent type.</li>';
        isvalid = false;
    }
    if ($("#ParentTypeId").val() > 0) {
        if ($("#ParentId").val() == 0) {
            errorMessage += '<li>Please select parent.</li>';
            $("#ParentId").parent().find(".field-validation-valid").html("Please select parent.").attr("data-val-required");
            isvalid = false;
        }
        else {
            errorMessage += '<li>Please select parent.</li>';
            $("#ParentId").parent().find(".field-validation-valid").html("Please select parent.").attr("data-val-required");
            isvalid = false;
        }
    }
    if (isvalid == false) {
        $("#messageEnum").removeClass("griddisplaynone");
        $("#messageEnum").show();
        $("#messageEnum").html(errorMessage) + '</ul>';
        window.scrollTo(-300, -300);
        return false;
    }
    else {
        return true;
    }
}

$(function () {
    if ($("#ParentTypeId").val() == 0) {
        $("#compulsoryParent").hide();
        $("#ParentId").attr("disabled", "disabled");
    }
    if ($("#ParentId").val() > 0) {
        $("#spanTag").removeClass("d-none");
    }
   
    $("#EnumTypeId").focus();
    $("#ParentTypeId").on("change",function () {
        $("#ParentId").attr("innerHTML", "");
        $("#ParentId").html('');
        if ($("#ParentTypeId").val() > 0) {
            $.post(SiteUrl + "Enum/GetParent", { ParentTypeID: $("#ParentTypeId").val(), isFromCreate: false }, function (data) {
                $("#ParentId").removeAttr("disabled");
                $("#spanTag").removeClass("d-none");
                $("#compulsoryParent").removeClass("griddisplaynone");
                $("#compulsoryParent").show();
                if (data != undefined && data != null) {
                    $("#ParentId").append("<option value='" + 0 + "'>- Select Parent -</option>");
                    $.each(data, function (index, list) {
                        $("#ParentId").append("<option value='" + list.value + "'>" + list.name + " </option>");
                    });
                }
            });
        }
        else {
            $("#compulsoryParent").hide();
            $("#ParentId").html('');
            var x = $("#ParentId");
            var opt = document.createElement("OPTION");
            opt.innerHTML = "- Select Parent -";
            opt.value = '0';
            x.append(opt);
            $("#ParentId").attr("disabled", "disabled");
            $("#ParentId-error").text("");
            $("#spanTag").addClass("d-none");
        }
    });

    $("#EnumTypeId").change(function () {
        if ($("#EnumTypeId").val() != "" && $('#EnumValue').val() != "") {
            var ids = $("#Id").val();
            console.log(ids);
            var data = { Id: $("#Id").val(), EnumTypeID: $("#EnumTypeId").val(), EnumValue: $('#EnumValue').val(), AttributeDetailId: $('#AttributeDetailId').val() };

            $.post(SiteUrl + "Enum/ValidateDuplicateEnum", data, function (lstData) {
                if (lstData.status == "0") {
                    $("#duplicateRecord").removeClass("displaynone");
                    $("#duplicateRecord").show();
                    $("#btnSubmit").attr("disabled", "disabled");
                    toastMsg(lstData.msg, false);
                }
                else {
                    $("#duplicateRecord").hide();
                    $("#btnSubmit").removeAttr("disabled");
                }
            }, "json");
            return false;
        }
    });
    $("#EnumValue").change(function () {
        if ($("#EnumTypeId").val() != "" && $('#EnumValue').val() != "") {
            var data = { Id: $("#Id").val(), EnumTypeID: $("#EnumTypeId").val(), EnumValue: $('#EnumValue').val(), AttributeDetailId: $('#AttributeDetailId').val() };

            $.post(SiteUrl + "Enum/ValidateDuplicateEnum", data, function (lstData) {
                if (lstData.status == "0") {
                    $("#duplicateRecord").removeClass("displaynone");
                    $("#duplicateRecord").show();
                    $("#btnSubmit").attr("disabled", "disabled");
                    toastMsg(lstData.msg, false);
                }
                else {
                    $("#duplicateRecord").hide();
                    $("#btnSubmit").removeAttr("disabled");
                }
            }, "json");
            return false;
        }
    });

    $("#ParentId").change(function () {
        if ($("#ParentTypeId").val() != "" && $('#ParentId').val() != "" && $("#ParentTypeId").val() > 0 && $("#ParentId").val() > 0) {
            var data = { EnumTypeID: $("#EnumTypeId").val(), EnumValue: $('#EnumValue').val(), ParentID: $('#ParentId').val(), AttributeDetailId: $('#AttributeDetailId').val() };

            $.post(SiteUrl + "Enum/ValidateDuplicateEnumByParentID", data, function (lstData) {
                if (lstData.status == "0") {
                    $("#duplicateRecord").removeClass("displaynone");
                    $("#duplicateRecord").show();
                    $("#btnSubmit").attr("disabled", "disabled");
                    toastMsg(lstData.msg, false);
                }
                else {
                    $("#duplicateRecord").hide();
                    $("#btnSubmit").removeAttr("disabled");
                }
            }, "json");
            return false;
        }
  
    });
});




$('#frmClinicalData').on('submit', function () {

    var isValid = $('#frmClinicalData').valid();
    if (isValid) {
        $('#loader').modal('show');
    }
})
$('#frmClinicalData1').on('submit', function () {

    var isValid = $('#frmClinicalData1').valid();
    if (isValid) {
        $('#loader').modal('show');
    }
})


$('#btnImportOrganization').on('click', function () {

    var isValid = $('#frmClinicalData').valid();
    if (isValid) {
        var fileName = $('#ImportedFile').val().split('\\').pop();
        
        showConfirmation("Are you sure? Do you want to upload " + fileName+" ?", 'Yes').then((result) => {
            if (result.value) {
                $('#btnImportOrganizationSubmit').trigger('click');
            }
            else {
                $('#ImportedFile').val('');
                return false;
            }
        })
    }
})


async function showConfirmation(message, buttonText) {
    return Swal.fire(
        {
            title: "Are you sure?",
            text: message,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: buttonText
        });
}