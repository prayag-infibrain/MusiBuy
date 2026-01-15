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
        searchValue : $("#txtSearch").val()
    };
}



CKEDITOR.replace('Criteria', {
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
