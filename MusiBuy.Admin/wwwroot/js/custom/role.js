$(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
});

function Search() {
    $('#grid').data('kendoGrid').dataSource.page(1);
    $('#grid').data('kendoGrid').dataSource.read();
    $('#grid').data('kendoGrid').refresh();
}

function FilterData() {
    return {
        roleValue: $("#txtSearch").val()
    };
}

$("#roleFrm").on('submit', function () {
    $("#roleBtn").attr('disabled', true);
    var valid = $("#roleFrm").valid();
    if (valid) {
        return true;
    }
    else {
        $("#roleBtn").attr('disabled', false);
        return false;
    }
})
