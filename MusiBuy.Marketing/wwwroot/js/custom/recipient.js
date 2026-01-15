$(document).ready(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
});
var id = $("#Id").val() != undefined ? $("#Id").val() : 0;
function IsRecipientExists() { if ($("#Email").val().trim().length > 0) { var n = SiteUrl + "Recipient/ValidateDuplicateRecipientCombination", t = { templateId: $("#TemplateId").val(), email: $("#Email").val().trim(), recipientId: id }; $.post(n, t, function (n) { n.status == "0" ? ($("#duplicateRecord").show(), $("#btnSubmit").prop("disabled", !0)) : ($("#duplicateRecord").hide(), $("#btnSubmit").prop("disabled", !1)) }, "json") } } $(function () { $("#Value").change(function () { IsRecipientExists() }) })

function Search() {
    var searchValue = $("#txtSearch").val();
    var pageNumber = $(".k-textbox").val();
    var pageDataSize = $(".k-input").text();
    var grid = $("#grid").data("kendoGrid");
    grid.dataSource.transport.options.read.global = false;
    grid.dataSource.query({
        page: pageNumber,
        pageSize: pageDataSize,
        filter: {
            logic: "or",
            filters: [
                { field: "FirstName", operator: "contains", value: searchValue },
                { field: "LastName", operator: "contains", value: searchValue },
                { field: "Email", operator: "contains", value: searchValue },
            ]
        }
    });
}