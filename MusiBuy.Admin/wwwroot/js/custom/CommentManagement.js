$(document).ready(function () {

    $("#grid table").addClass("table-responsive").addClass("tablestyle");
    var n = $("input[name=chkDelete]");
    n.prop("checked", false);
    $("#allCheckBox").prop("checked", false);

    $(document).on("click", ".dots-btn", function (e) {
        e.stopPropagation();
        $(".dropdown-content").hide(); // close other dropdowns
        $(this).siblings(".dropdown-content").toggle(); // show clicked one
    });

    $(document).on("click", function () {
        $(".dropdown-content").hide(); // close when clicking elsewhere
    });

    onCreatorFilterChange();
});

function onCreatorFilterChange() {
    $("#grid").data("kendoGrid").dataSource.read();
    var creatorId = $("#CreatorId").val();
    var selectedPostId = $("#hdnPostId").val(); 
    $("#PostId").empty().append('<option value="">- Select Post -</option>');

    if (creatorId) {
        $.get('/CommentManagement/BindPostDropdown', { creatorId: creatorId }, function (data) {
            $.each(data, function (i, post) {
                var isSelected = post.value == selectedPostId ? 'selected' : '';
                var optionHtml = `<option value="${post.value}" ${isSelected}>${post.name}</option>`;
                $("#PostId").append(optionHtml);
            });
        });
    }
}

function onPostFilterChange() {
    $("#grid").data("kendoGrid").dataSource.read();
}

$("#txtSearch").on("keyup", function () {
    $("#grid").data("kendoGrid").dataSource.read();
});

function updateStatus(Id, newStatusId) {
    debugger;
    $.ajax({
        url: '/CommentManagement/UpdateStatus',
        type: 'POST',
        data: { CommentId: Id, statusId: newStatusId },
        success: function (response) {
            if (response.success) {
                $("#grid").data("kendoGrid").dataSource.read();
            } else {
                alert("Failed to update status.");
            }
        },
        error: function () {
            alert("Error occurred while updating status.");
        }
    });
}