$(document).ready(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
    $('#loader').modal('hide');
    $('.select2').select2();
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
        searchValue: $("#txtEVSearch").val()
    };
}



$('#CountryId').each(function () {
    $(this).change(function () {
        var selectedValue = $(this).val();
        var selectedText = $(this).find('option:selected').text();
        var selectedKey = $(this).find('option:selected').data('key');

        console.log("Value: " + selectedValue);
        console.log("Text: " + selectedText);
        console.log("Key: " + selectedKey);
        $("#RegionName").val(selectedKey);
        // Add your logic here (like updating another field)
    });
});

//$('#CountryId').change(function () {
//    // Selected option
//    var selectedText = $(this).find('option:selected').text(); // Name
//    var selectedValue = $(this).val(); // Value
//    console.log("Selected Text: " + selectedText);
//    console.log("Selected Value: " + selectedValue);

//    // If you want Key specifically from Model, use a mapping or data-attribute
//    var selectedKey = $(this).find('option:selected').data('key');
//    console.log("Selected Key: " + selectedKey);
//});
