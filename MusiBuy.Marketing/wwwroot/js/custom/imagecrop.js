$(document).on("change", "#ProfilePicture", function () {
    var getImage = $("#ProfilePicture").val();
    var getExtension = getImage.split(".").pop().toLowerCase();

    var fileUpload = $("#ProfilePicture").get(0);
    var files = fileUpload.files;
    var fileData = new FormData();
    for (var i = 0; i < files.length; i++) {
        var filesizemb = ((files[i].size) / 1024) / 1024;
        if (filesizemb > maxFileSize) {
            $("#imageError").show();
            $("#imageError").parent().find(".field-validation-valid").html(maxFileSizeMsg).attr("data-val-required");
            return false;

        }
        else {
            $("#imageError").hide();

        }
    }
    if (getExtension == "jpg" || getExtension == "JPG" || getExtension == "jpeg" || getExtension == "JPEG" || getExtension == "png" || getExtension == "PNG" || getExtension == "bmp" || getExtension == "BMP" ) {
        $("#imageError").hide();

        var fileUpload = $("#ProfilePicture").get(0);
        var files = fileUpload.files;

        var ext = files[0].name.substring(files[0].name.lastIndexOf('.') + 1).toLowerCase();
        if (!SearchExt(ext.toLowerCase(), imageExtensions)) {
            var $li = $("<li class='error'/>").html(uploadValidFileMsg);
            $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
            $(".validation-summary-errors ul").append($li);
            $('html, body').animate({ scrollTop: 0 }, 2000);
            $('.upload_photo_btn #ProfilePicture').val('');
            e.preventDefault();
        }
        else {
            if ($(".validation-summary-errors ul li:visible").length < 1) {
                $(".validation-summary-errors").addClass("validation-summary-valid").removeClass("validation-summary-errors");
            }

            if (files.length > 0) {
                for (var i = 0; i < files.length; i++) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $("#div_upload_pro").addClass("displaynone");
                        $("#div_upload_pro_img").removeClass("displaynone");
                        $("#div_crop_pro").removeClass("displaynone");
                        $("#has_crop_image_Pro").val("false");
                        try {
                            $('#upload_pro_img').removeClass("croppie-container").html("");
                            var el = document.getElementById('upload_pro_img');
                            $resize = $(el).croppie({
                                showZoomer: true,
                                enableOrientation: true,
                                mouseWheelZoom: 'ctrl',
                                viewport: {
                                    width: imgWidth,
                                    height: imgHeight,
                                    type: 'squre'
                                },
                                enableResize: true,
                                boundary: {
                                    width: cropWidth,
                                    height: cropHeight
                                }
                            });

                            $resize.croppie('bind', {
                                url: e.target.result
                            });
                        } catch (e) {
                            // nothing
                        }
                        $(".btnaction").addClass("disable-click");
                        $(".form-actions").hide();
                        $("#btn_save_pro").bind("click", function (event) {
                            uploadedCountry = $("#ProfilePicture").val();
                            $resize.croppie("result").then(function (canvas) {
                                var points = $resize.croppie('get');
                                if (points != undefined) {
                                    $("#XCoordinate_pro").val(points.points[0]);
                                    $("#YCoordinate_pro").val(points.points[1]);
                                    $("#WCoordinate_pro").val(points.points[2] - points.points[0]);
                                    $("#HCoordinate_pro").val(points.points[3] - points.points[1]);
                                }
                                $('#pro_img').attr('src', canvas).parent().removeClass("displaynone");
                                $('#div_pro_view').hide();
                                $('<i class="r-image-icon fas fa-times" onclick="RemoveProfile()"></i>').insertAfter('#pro_img');
                                $("#div_country").removeClass("displaynone");
                                $("#div_upload_pro").removeClass("displaynone");
                                $("#div_upload_pro_img").addClass("displaynone");
                                $("#div_crop_pro").addClass("displaynone");
                                $("#has_crop_image_Pro").val("true");
                                $(".btnaction").removeClass("disable-click");
                                $(".form-actions").show();
                                $("#btn_save_pro").unbind("click");
                                $('#div_pro_view').hide().removeClass('d-block');
                                $resize = null;
                            });
                            $(this).unbind(event);
                        });
                    };
                }
                reader.readAsDataURL(files[0]);
            }
            if ($("#partial_msg .alret-close").length > 0) {
                closemsg($("#partial_msg .alret-close"), true);
            }
            RemoveMsg("#display-messages");
        }
    }
    else {
        $("#imageError").show();
        $("#imageError").parent().find(".field-validation-valid").html(uploadValidFileMsg).attr("data-val-required");
        return false;
    }
});

function DeleteImg(id) {
    if (confirm("Are you sure? Do you want to delete this image?")) {
        $.post(SiteUrl + 'Tutor/DeleteImage', { id: id }, function (data) {
            if (data.Success) {
                $("#div_Profile_view").remove();
                toastMsg(data.Messages, true);
            }
            else {
                toastMsg(data.Messages, false);
                var $li = $("<li/>").html(data.Messages);
                $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");
                $(".validation-summary-errors ul").append($li);
            }
        });
        return true;
    }
    return false;
}

function RemoveProfile_Cancel() {
    $('#pro_img').attr('src', "").parent().addClass("displaynone");
    $("#div_Profile i").remove();
    $('.upload_photo_btn #Image').val('');
    $("#div_Profile").addClass("displaynone");
    $("#div_upload_pro").removeClass("displaynone");
    $("#div_upload_pro_img").addClass("displaynone");
    $("#div_crop_pro").addClass("displaynone");
    $("#upload_pro_img").removeClass("croppie-container");
    $("#upload_pro_img").html('');
    $(".btnaction").removeClass("disable-click");
    $('#div_pro_view').show().addClass('d-block');
    $(".form-actions").show();
}

function RemoveProfile() {
    $('#pro_img').attr('src', "").parent().addClass("displaynone");
    $('.upload_photo_btn #Image').val('');
    $('#pro_img').parent().find("i").remove();
    $("#upload_pro_img").removeClass("croppie-container");
    $("#upload_pro_img").html('');
    $('#div_pro_view').show().addClass('d-block');
    uploadedProfile = null;
    uploadedCountry = "";
}

function RemoveMsg(divId) {
    $(divId).find(".alret-close").trigger("click");
}