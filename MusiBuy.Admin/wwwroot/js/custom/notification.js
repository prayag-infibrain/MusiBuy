$(document).ready(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
});
$(function () { $("#Email").focus(); })

function Search() {
    $('#grid').data('kendoGrid').dataSource.page(1);
    $('#grid').data('kendoGrid').dataSource.read();
    $('#grid').data('kendoGrid').refresh();
}

function FilterData() {
    return {
        notificationValue: $("#txtSearch").val()
    };
}
function onGridDataBoundSubGrid(n) {
    this.expandRow(this.tbody.find("tr.k-master-row").first());
    if (!n.sender.dataSource.view().length) {
        var t = n.sender.thead.find("th:visible").length, i = '<tr><td colspan="' + t + '" align="center">No records found.<\/td><\/tr>'; n.sender.tbody.parent().width(n.sender.thead.width()).end().html(i)
    }
}
$(document).ready(function () {
    var lastSubjectSelected = '';
    var currentSelection = '';

    $('#frmNotification .select-frontuser').multiselect({
        selectAll: true,
        columns: 1,
        search: true,
        values: 2,
        includeSelectAllOption: true,
        maxHeight: 250,
        width: '100%',
        texts: {
            placeholder: $("#frmNotification .select-frontuser").attr("data-placeholder"),
        },
        onControlClose: function (element, option) {
            $("#StrFrontUserId").val($("#SelectedFrontUserIds").val().join(","));
            if ($("#SelectedFrontUserIds").val().length > 0) {
                $("#SelectedFrontUserIds").parent().find(".field-validation-valid").html("");
            }
        },
        onOptionClick: function (element, option) {
            $("#StrFrontUserId").val($("#SelectedFrontUserIds").val().join(","));
            if ($("#SelectedFrontUserIds").val().length > 0) {
                $("#SelectedFrontUserIds").parent().find(".field-validation-valid").html("");
            }
            var maxSelect = 100;
            if ($(element).val().length > maxSelect) {
                if ($(option).is(':checked')) {
                    var thisVals = $(element).val();

                    thisVals.splice(
                        thisVals.indexOf($(option).val()), 1
                    );

                    $(element).val(thisVals);

                    $(option).prop('checked', false).closest('li')
                        .toggleClass('selected');
                }
            }
            else if ($(element).val().length == maxSelect) {
                $(element).next('.ms-options-wrap')
                    .find('li:not(.selected)').addClass('disabled')
                    .find('input[type="checkbox"]')
                    .attr('disabled', 'disabled');
            }
            else {
                $(element).next('.ms-options-wrap')
                    .find('li.disabled').removeClass('disabled')
                    .find('input[type="checkbox"]')
                    .removeAttr('disabled');
            }

            currentSelection = $("#frmNotification .select-frontuser").val().join(",");
        },
        searchOptions: {
            delay: 0,
            onSearch: function (element, option) {
                setTimeout(function () {
                    var opt_count = $(element).parent().find(".ms-options-wrap").find(".ms-options ul li:not(.ms-hidden)").length;;
                    if (opt_count == 0) {
                        if ($(element).parent().find(".ms-options-wrap").find(".ms-options ul #not_found_li").length < 1) {
                            var not_found_element = $("<li id='not_found_li'><label>Not found</label></li>")
                            $(element).parent().find(".ms-options-wrap").find(".ms-options ul").append(not_found_element);
                        }
                        else {
                            $(element).parent().find(".ms-options-wrap").find(".ms-options ul #not_found_li").removeClass("ms-hidden");
                        }
                    }
                    else {
                        $(element).parent().find(".ms-options-wrap").find(".ms-options ul #not_found_li").remove();
                    }
                }, 10);
            }
        }
    });
});
$("#btnSubmit").on("click", function () {
    var isValid = $("#frmNotification").valid();
    var title = $("#Title").val().trim();
    var description = $("#Message").val().trim();
    var nIds = $("#SelectedFrontUserIds").val();
    console.log(nIds.length);

    if (title == "") {
        $("#titleError").html("Title is required");
    }
    if (description == "") {
        $("#descriptionError").html("Message is required");
        isValid = false;
    }
    if (nIds.length == 0) {
        $("#userError").html("Client is required");
    }
    if (title != "" && description != "" && nIds.length > 0) {
        $("#frmNotification").submit();
    }
});

