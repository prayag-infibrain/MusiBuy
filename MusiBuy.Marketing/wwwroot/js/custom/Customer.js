$(document).ready(function () {
    $('.select2').select2();
});


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

$(function () {
    //const maxSizeMB = @GlobalCode.ImageSize;
    const maxSizeMB = $("#hdnimagesize").val();
    const maxSizeBytes = maxSizeMB * 1024 * 1024;
    const allowedImageExtensions = document.getElementById("hdnimageext").textContent;
    //const allowedImageExtensions = @Html.Raw(Json.Serialize(GlobalCode.AllowImgFileExt.Select(ext => ext.ToLower()).Distinct()));

    $("#ProfilePicture").on("change", function (e) {
        const file = this.files[0];
        $("#ProfilePicture-error").html("");

        if (!file) {
            $("#ProfilePicture-preview").hide();
            return;
        }

        const fileExtension = file.name.split('.').pop().toLowerCase();

        if (!allowedImageExtensions.includes(fileExtension)) {
            $("#ProfilePicture-error").html("Only images with .jpg, .jpeg, .png & .bmp extensions are allowed.");
            $(this).val("");
            $("#ProfilePicture-preview").hide();
            return;
        }

        if (file.size > maxSizeBytes) {
            $("#ProfilePicture-error").html("Image must be under " + maxSizeMB + " MB.");
            $("#ProfilePicture-preview").hide();
            return;
        }

        const reader = new FileReader();
        reader.onload = function (evt) {
            $("#ProfilePicture-preview-img").attr("src", evt.target.result);
            $("#ProfilePicture-preview").show();
        };
        reader.readAsDataURL(file);
    });

    $("#remove-image").on("click", function () {
        $("#ProfilePicture").val("");
        $("#ProfilePicture-preview-img").attr("src", "#");
        $("#ProfilePicture-preview").hide();
    });
});