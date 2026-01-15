$(document).ready(function () {
    setAccept();
    GetTxtFile();

    $(function () {
        const maxSizeMB = document.getElementById("maxSizeMB").textContent; //GlobalCode.ImageSize;
        const maxSizeBytes = maxSizeMB * 1024 * 1024;
        const allowedAudioExtensions = document.getElementById("AllowAudioFileExt").textContent; // Html.Raw(Json.Serialize(GlobalCode.AllowAudioFileExt.Select(ext => ext.ToLower()).Distinct()));
        const allowedVideoExtensions = document.getElementById("AllowVideoFileExt").textContent; // Html.Raw(Json.Serialize(GlobalCode.AllowVideoFileExt.Select(ext => ext.ToLower()).Distinct()));
        const allowedImageExtensions = document.getElementById("AllowImgFileExt").textContent; // Html.Raw(Json.Serialize(GlobalCode.AllowImgFileExt.Select(ext => ext.ToLower()).Distinct()));
        const allowedTextExtensions = document.getElementById("AllowTextFileExt").textContent; // Html.Raw(Json.Serialize(GlobalCode.AllowTextFileExt.Select(ext => ext.ToLower()).Distinct()));
        debugger;

        $("#MediaFile").on("change", function (e) {
            /*this = e;*/
            const file = this.files[0];
            $("#MediaFile-error").html("");

            if (!file) {
                $("#MediaFile-preview").hide();
                return;
            }

            const fileExtension = file.name.split('.').pop().toLowerCase();
            const mType = document.getElementById("TypeId").value;
            if (mType == "2") //For Audio
            {
                if (!allowedAudioExtensions.includes(fileExtension)) {
                    $("#MediaFile-error").html("Only Audio with .mp3, .wav, .aiff, .flac, .aac, .wma  & .m4a extensions are allowed.");
                    $(this).val("");
                    $("#MediaFile-preview").hide();
                    return;
                }
            } else if (mType == "3") //For Video
            {
                if (!allowedVideoExtensions.includes(fileExtension)) {
                    $("#MediaFile-error").html("Only Audio with .mp4, .avi, .mkv, .mov, .wmv, .flv, .webm, .3gp, .mpeg, .m4v  & .m4a extensions are allowed.");
                    $(this).val("");
                    $("#MediaFile-preview").hide();
                    return;
                }
            } else if (mType == "4") //For Image
            {
                if (!allowedImageExtensions.includes(fileExtension)) {
                    $("#MediaFile-error").html("Only images with .jpg, .jpeg, .png & .bmp extensions are allowed.");
                    $(this).val("");
                    $("#MediaFile-preview").hide();
                    return;
                }
            } else if (mType == "5") //For Text
            {
                if (!allowedTextExtensions.includes(fileExtension)) {
                    $("#MediaFile-error").html("Only Text with  .txt, .csv, .log, .json, .xml, .md, .ini &  .yml extensions are allowed.");
                    $(this).val("");
                    $("#MediaFile-preview").hide();
                    return;
                }
            }
            if (file.size > maxSizeBytes) {
                $("#MediaFile-error").html("File must be under " + maxSizeMB + " MB.");
                $("#MediaFile-preview").hide();
                return;
            }
            const reader = new FileReader();
            reader.onload = function (evt) {
                $("#MediaFile-preview-img").attr("src", evt.target.result);
                $("#MediaFile-preview").show();
            };
            reader.readAsDataURL(file);
        });
        $("#remove-file").on("click", function () {

            var imagePath = $("#StrMediaFile").val();
            if (imagePath.length > 1) {
                deletePrice();
            }
            else {
                const fileInput = $('input[type="file"][asp-for="MediaFile"]');
                const previewContainer = $('#MediaFile-preview');
                const previewImg = $('#MediaFile-preview-img');
                $("#MediaFile").val('');
                previewImg.attr('src', '#');
                previewContainer.hide();
            }
            //$("#MediaFile").val("");
            //$("#MediaFile-preview-img").attr("src", "#");
            //$("#MediaFile-preview").hide();
        });

        const fileInput = $('input[type="file"][asp-for="MediaFile"]');
        const previewContainer = $('#MediaFile-preview');
        const previewImg = $('#MediaFile-preview-img');
        const removeBtn = $('#remove-image');

        fileInput.on('change', function (e) {
            const file = this.files[0];

            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    previewImg.attr('src', e.target.result);
                    previewContainer.show();
                };
                reader.readAsDataURL(file);
            } else {
                previewContainer.hide();
                previewImg.attr('src', '#');
            }
        });

        removeBtn.on('click', function () {
            fileInput.val('');
            previewImg.attr('src', '#');
            previewContainer.hide();
        });
    })

    // Change file type validation based on selected post type
    $("#PostType").on("change", function () {
        var selected = $(this).val();
        var upload = $("#MediaFile").data("kendoUpload");

        if (selected === "Audio") {
            upload.options.validation.allowedExtensions = [".mp3", ".wav"];
        } else if (selected === "Video") {
            upload.options.validation.allowedExtensions = [".mp4", ".avi"];
        } else if (selected === "Image") {
            upload.options.validation.allowedExtensions = [".jpg", ".jpeg", ".png"];
        } else {
            upload.options.validation.allowedExtensions = [".txt", ".pdf"];
        }
    });

});

function setAccept() {
    const mediaType = parseInt(document.getElementById("TypeId").value, 10);
    const fileInput = document.querySelector("input[name='MediaFile']");
    switch (mediaType) {
        case 2:
            fileInput.accept = ".mp3,.wav,.aiff,.flac,.aac,.wma,.m4a";
            break;
        case 3:
            fileInput.accept = "video/*";
            break;
        case 4:
            fileInput.accept = "image/*";
            break;
        case 5:
            fileInput.accept = ".txt,.doc,.docx,.pdf";
            break;
        default:
            fileInput.accept = "";
    }
}
function GetTxtFile() {
    const mediaType = parseInt(document.getElementById("TypeId").value, 10);
    if (mediaType == 5) {
        const txtUrl = document.getElementById("StrMediaFileFortxt").textContent;
        fetch(txtUrl).then(response => response.text()).then(data => {
            document.getElementById('preview').textContent = data;
        }).catch(error => {
            document.getElementById('preview').textContent = "Failed to load text: " + error;
        });
    }
}


function deletePrice() {
    const Id = $("#Id").val();
    const CreatorId = $("#CreatorId").val();
    showConfirmation("Do you want to delete this Media?", 'Yes').then((result) => {
        if (result.value) {
            $.ajax({
                url: deleteFileUrl,
                type: 'Get',
                data: { Id: Id, creatorId: CreatorId },
                success: function (response) {
                    if (response.success == true) {
                        toastMsg(response.msg, true);
                        window.location.reload();
                    }
                    else {
                        toastMsg(response.msg, false);
                    }
                }
            })
            return true;
        }
        else {
            return false;
        }
    })
}





$(document).ready(function () {

    hideAll();

    $('#ContentTypeId').on('change', function () {
        handleContentType($(this).val());
    });

    // 🔹 Edit page support
    const selectedContentType = $('#ContentTypeId').val();
    if (selectedContentType) {
        handleContentType(selectedContentType);
    }

    function handleContentType(contentTypeId) {

        hideAll();

        switch (parseInt(contentTypeId)) {

            case 33: // Music
                $('#divCountry').show();
                $('#divGenre').show();
                break;

            case 34: // Podcast
                $('#divCategory').show();
                loadPodcastCategory();
                break;

            case 35: // Viewercast
                // No dropdown
                break;

            case 36: // Social Media Influencer
                $('#divCategory').show();
                loadSocialCategory();
                break;

            case 37: // Music Producer
                $('#divType').show();
                $('#divGenre').show();
                loadMusicProducerType();
                break;
        }
    }

    function hideAll() {
        $('#divCountry,#divGenre,#divType,#divCategory').hide();
    }

    // 🔹 CATEGORY LOADERS
    function loadPodcastCategory() {
        $.get('/PostManagement/GetPodcastCategories', function (data) {
            bindDropdown('#CategoryId', data, '- Select Podcast Category -');
        });
    }

    function loadSocialCategory() {
        $.get('/PostManagement/GetSocialCategories', function (data) {
            bindDropdown('#CategoryId', data, '- Select Social Category -');
        });
    }

    // 🔹 TYPE LOADER (Music Producer)
    function loadMusicProducerType() {
        $.get('/PostManagement/GetMusicProducerTypes', function (data) {
            bindDropdown('#TypeId', data, '- Select Music Producer Type -');
        });
    }
});

// 🔹 COMMON BIND FUNCTION
function bindDropdown(selector, data, placeholder) {
    let ddl = $(selector);
    ddl.empty().append(`<option value="">${placeholder}</option>`);
    $.each(data, function (i, item) {
        ddl.append(`<option value="${item.id}">${item.EnumValue}</option>`);
    });
}



