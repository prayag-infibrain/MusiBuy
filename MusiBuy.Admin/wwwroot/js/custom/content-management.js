CKEDITOR.replace('Content', {
    allowedContent: true,
    extraPlugins: 'easyimage',
    cloudServices_uploadUrl: SiteUrl + 'home/UploadImage',
    cloudServices_tokenUrl: SiteUrl + 'home/EditorToken',
    easyimage_styles: {
        gradient1: {
            group: 'easyimage-gradients',
            attributes: {
                'class': 'easyimage-gradient-1'
            },
            label: 'Blue Gradient',
            icon: 'https://ckeditor.com/docs/ckeditor4/4.16.0/examples/assets/easyimage/icons/gradient1.png',
            iconHiDpi: 'https://ckeditor.com/docs/ckeditor4/4.16.0/examples/assets/easyimage/icons/hidpi/gradient1.png'
        },
        gradient2: {
            group: 'easyimage-gradients',
            attributes: {
                'class': 'easyimage-gradient-2'
            },
            label: 'Pink Gradient',
            icon: 'https://ckeditor.com/docs/ckeditor4/4.16.0/examples/assets/easyimage/icons/gradient2.png',
            iconHiDpi: 'https://ckeditor.com/docs/ckeditor4/4.16.0/examples/assets/easyimage/icons/hidpi/gradient2.png'
        },
        noGradient: {
            group: 'easyimage-gradients',
            attributes: {
                'class': 'easyimage-no-gradient'
            },
            label: 'No Gradient',
            icon: 'https://ckeditor.com/docs/ckeditor4/4.16.0/examples/assets/easyimage/icons/nogradient.png',
            iconHiDpi: 'https://ckeditor.com/docs/ckeditor4/4.16.0/examples/assets/easyimage/icons/hidpi/nogradient.png'
        }
    },
    easyimage_toolbar: [
        'EasyImageFull',
        'EasyImageSide',
        'EasyImageGradient1',
        'EasyImageGradient2',
        'EasyImageNoGradient',
        'EasyImageAlt'
    ]
});

var contentTypeID = $('#PageId').val();
if (contentTypeID == "") {
    $("#PageContent1").hide();
    $("#button").hide();
}
$("#PageId").on("change", function () {
    var PageId = $('#PageId').val();
    if (PageId != "") {
        $.post(SiteUrl + 'Content/getPageContent', { pageId: PageId }, function (objContent) {
            $("#PageContent1").show();
            $("#button").show();
            $("#messageTemplate").hide();
            $(".validation-summary-errors").hide();
            $("#Title").val(objContent.Title);
            $("#MetaTagDescription").val(objContent.MetaTagDescription);
            $("#MetaTagKeyword").val(objContent.MetaTagKeyword);
            $("#Id").val(objContent.Id);
            if (objContent.Id == 0) {
                $('input[name=IsActive][value=true]').prop('checked', true);
            }
            else {
                if (objContent.IsActive)
                    $('input[name=IsActive][value=true]').prop('checked', true);
                else
                    $('input[name=IsActive][value=false]').prop('checked', true);
            }
            CKEDITOR.instances['Content'].setData(objContent.Content);
        });
    }
    else {
        $("#PageContent1").hide();
        $("#button").hide();
    }
});

$("#btnSubmit").on("click", function () {
    var data = CKEDITOR.instances['Content'].getData();
    $("#frmContent").valid();
    if (data == '' || data == undefined) {
        $("#Content").parent().find(".field-validation-valid").html($("#Content is required").attr("data-val-required"));
        return false;
    }
    else {
        $("#Content").parent().find(".field-validation-valid").html($("#Content").removeAttr("data-val-required"));
        return true;
    }
});