var isInit = false;
function dataCall(id) {
    var homogeneous = new kendo.data.HierarchicalDataSource({
        transport: {
            read: {
                url: SiteUrl + "Client/GetVideos?clientId="+id,
                dataType: "json"
            }
        },
        schema: {
            model: {
                id: "id",
                text: "text",
                hasChildren: "hasChildren",
                checked: "Checked",
                fields: {
                    checked: { from: "Checked", type: "boolean" },
                    hasChildren: { from: "hasChildren", type: "boolean" }
                }
            }
        }
    });
    return homogeneous
}



$('#Mobile').mask('999 999 9999?9', {
    translation: {
        '0': { pattern: /[0-9]/ }
    }
});
$(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
    var n = $("input[name=chkDelete]");
    n.prop("checked", false);
    $("#allCheckBox").prop("checked", false);
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
    $("#ProfileFile").val('');
}
function RemoveProfile() {
    $('#pro_img').attr('src', "").parent().css('display', 'none');
    $('.upload_photo_btn #Image').val('');
    $('#pro_img').parent().find("i").remove();
    $("#upload_pro_img").removeClass("croppie-container");
    $("#upload_pro_img").html('');
    $('#div_pro_view').show().addClass('d-block');
    uploadedProfile = null;
    uploadedCountry = "";
    $("#ProfileFile").val('');
}
$(document).on("change", "#ProfileFile", function () {
    var getImage = $("#ProfileFile").val();
    var getExtension = getImage.split(".").pop();

    var fileUpload = $("#ProfileFile").get(0);
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
    if (getExtension == "jpg" || getExtension == "JPG" || getExtension == "jpeg" || getExtension == "JPEG" || getExtension == "png" || getExtension == "PNG" || getExtension == "bmp" || getExtension == "BMP") {
        $("#imageError").hide();

        var fileUpload = $("#ProfileFile").get(0);
        var files = fileUpload.files;

        var ext = files[0].name.substring(files[0].name.lastIndexOf('.') + 1).toLowerCase();
        if (!SearchExt(ext.toLowerCase(), imageExtensions)) {
            var $li = $("<li class='error'/>").html(uploadValidFileMsg);
            $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
            $(".validation-summary-errors ul").append($li);
            $('html, body').animate({ scrollTop: 0 }, 500);
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
                                aspectRatio: 1 / 1
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
                            uploadedCountry = $("#ProfileFile").val();
                            $resize.croppie("result").then(function (canvas) {
                                var points = $resize.croppie('get');
                                if (points != undefined) {
                                    $("#XCoordinate_pro").val(points.points[0]);
                                    $("#YCoordinate_pro").val(points.points[1]);
                                    $("#WCoordinate_pro").val(points.points[2] - points.points[0]);
                                    $("#HCoordinate_pro").val(points.points[3] - points.points[1]);
                                }
                                $('#pro_img').attr('src', canvas).parent().removeClass("displaynone");
                                $('#pro_img').parent().css('display', 'inline-block');
                                $('#div_pro_view').hide();
                                $('<a href="#" class="r-image-icon-preview"><i class="fa fa-times remove_profile" style="color: white;" onclick="RemoveProfile()"></i></a>').insertAfter('#pro_img');
                                $("#div_Profile").removeClass("displaynone");
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
            //RemoveMsg("#display-messages");
        }
    }
    else {
        $("#imageError").show();
        $("#imageError").parent().find(".field-validation-valid").html(uploadValidFileMsg).attr("data-val-required");
        $("#ProfileFile").val("");
        return false;
    }
});
function DeleteImg(id) {
    showConfirmation("Are you sure? Do you want to delete ?", 'Yes').then((result) => {
        if (result.value) {
            $.post(SiteUrl + 'Client/DeleteImage', { id: id }, function (data) {
                if (data.Success) {
                    $("#div_banner_view").remove();
                    $("#div_pro_view").remove();
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
        else {
            return false;
        }
    })
}

function videoClick(id, fname, lname) {
    if (parseInt(id) > 0) {        
        $("#videoClientId").val(parseInt(id))
        $("#treeview-left").kendoTreeView({
            dataSource: dataCall(id),
            dataTextField: "text",
            /*checkboxes: {
                checkChildren: true,
            },*/
            loadOnDemand: false,
            template: function (dataItem) {
                if (!dataItem.item.hasChildren) {
                    if (dataItem.item.checked) {
                        return "<input type='checkbox' checked value=" + dataItem.item.id + " class='childCheckBox'  /> " + dataItem.item.text;
                    }
                    else {
                        return "<input type='checkbox' value=" + dataItem.item.id + " class='childCheckBox'  /> " + dataItem.item.text;
                    }
                    
                } else {
                    return dataItem.item.text;
                }
            }
        });
        $(".modal").modal('show');
        var title = fname + " " + lname + "'s videos."
        $(".modal .title").html(title);
    }
}

$("#btnsaveVideo").on('click', function () { 
    var checkboxes = document.querySelectorAll('.childCheckBox');
    var selectedValues = [];
    var notChecked = [];
    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            var intVal = parseInt(checkbox.value);
            selectedValues.push(intVal);
        }
        else {
            var intVal = parseInt(checkbox.value);
            notChecked.push(intVal);
        }
    });
   
    if (selectedValues != null && selectedValues.length > 0) {
        var selectedId = $("#videoClientId").val();
        var intSelected = parseInt(selectedId);
        if (intSelected != null && intSelected != undefined && intSelected > 0) {
            var videoIdsString = selectedValues.join(',');
            $.ajax({
                url: SiteUrl + "Client/SaveClientVideos",
                data: { "clientId": intSelected, "videoIds": videoIdsString },
                type: "POST",
                success: function (data) {
                    if (data.Success) {
                        toastMsg(data.message, true);
                    }
                    else {
                        toastMsg(data.message, false);
                    }
                    $(".modal").modal('hide');
                }
            });
        }
    }
    else {
        $(".modal").modal('hide');
    }
})
$(document).on("click", ".closeButton", function () {
    $("#treeview-left").kendoTreeView('destroy').empty();
    $(".modal").modal('hide');
});
$('.modal').on('hidden.bs.modal', function (e) {
    $("#treeview-left").kendoTreeView('destroy').empty();
});