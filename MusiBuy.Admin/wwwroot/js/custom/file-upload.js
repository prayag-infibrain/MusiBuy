



$(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");

})
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
// Configure Toastr to display notifications at the bottom-right
toastr.options = {
    "positionClass": "toast-bottom-right", // Positions the toastr at the bottom-right
    "closeButton": true,                   // Adds a close button to the toast
    "progressBar": true,                   // Displays a progress bar
    "timeOut": "5000",                     // Time in milliseconds before the toast disappears
    "extendedTimeOut": "1000",             // Time in milliseconds for the extended timeout
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};

function SaveCount() {
    const countValue = document.getElementById('txtCount').value;

    if (countValue === '') {
        toastr.error('Please enter a count.');
        return;
    }

    $.ajax({
        type: 'POST',
        url: '/ProteinSummary/SaveCount',
        data: { Vewcount: countValue },
        success: function (response) {
            if (response.success) {
                toastr.success('Count saved successfully!');
            } else {
                toastr.error('Error: ' + response.message);
            }
        },
        error: function (xhr, status, error) {
            toastr.error('An error occurred: ' + error);
        }
    });
}

function displayItemDescriptionforProtien(Id) {
    if (Id > 0) {
        $.ajax({
            url: SiteUrl + "ProteinsData/GetDescription",
            type: 'GET',
            data: { Id: Id },
            success: function (data) {
                if (data.success == true && data.itemData != null) {
                    // Clear the text area and show the modal
                    $("#descriptionTxt").val('');
                    $('#showDescriptionModalLabel').text('Description');
                    $('#showDescriptionModal').modal('show');
                    // Populate the text area with the item description
                    $("#descriptionTxt").val(data.itemData);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred while fetching the description: ' + error);
            }
        });
    } else {
        return false;
    }
}


function closeModal() {
    $('#showDescriptionModal').modal('hide');
}
function displayItemDescriptionClinicalData(Id, e) {

    var grid = $("#grid").data("kendoGrid");
    var index = $(this).index();
    var column = grid.columns[index];


    if (Id > 0) {
        $.ajax({
            url: SiteUrl + "ClinicalData/GetDescription",
            type: 'GET',
            data: { Id: Id, column: e },
            success: function (data) {
                if (data.success == true && data.itemData != null) {

                    $("#descriptionTxt").val('');
                    if (e == 1) {
                        $('#showDescriptionModalLabel').text('Ocular Diseases');
                    }
                    else if (e == 2) {
                        $('#showDescriptionModalLabel').text('Comments');
                    }
                    else if (e ==3) {
                        $('#showDescriptionModalLabel').text('Other Diseases');
                    }
                    else if (e == 4) {
                        $('#showDescriptionModalLabel').text('Ocular Medications');
                    }
                    $('#showDescriptionModal').modal('show');

                    $("#descriptionTxt").val(data.itemData);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred while fetching the description: ' + error);
            }
        });
    } else {
        return false;
    }
}

function displayItemDescriptionSummary(Id,e) {

    var grid = $("#grid").data("kendoGrid");
    var index = $(this).index();
    var column = grid.columns[index];

    if (Id > 0) {
        $.ajax({
            url: SiteUrl + "ProteinSummary/GetDescription",
            type: 'GET',
            data: { Id: Id, column: e },
            success: function (data) {
                if (data.success == true && data.itemData != null) {
                    $("#descriptionTxt").val('');
                    if (e == 1) {
                        $('#showDescriptionModalLabel').text('Description');
                    }
                    else if (e == 2) {
                        $('#showDescriptionModalLabel').text('GeneNames');
                    }
                    $('#showDescriptionModal').modal('show');

                    $("#descriptionTxt").val(data.itemData);
                }
            },
            error: function (xhr, status, error) {
                alert('An error occurred while fetching the description: ' + error);
            }
        });
    } else {
        return false;
    }
}


