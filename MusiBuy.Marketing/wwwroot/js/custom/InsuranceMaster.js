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
        value: $("#txtSearch").val()
    };
}
$("#roleFrm").on('submit', function () {
    $("#roleBtn").attr('disabled', true);
    var valid = $("#roleFrm").valid();
    if (valid) {
        return true;
    }
    else {
        $("#roleBtn").attr('disabled', false);
        return false;
    }
})

$("#strRenewalDate").datetimepicker({
    format: 'DD-MM-YYYY',   
    icons: {
        time: 'fa fa-clock-o',
        date: 'fa fa-calendar',
        up: 'fa fa-chevron-up',
        down: 'fa fa-chevron-down',
        previous: 'fa fa-chevron-left',
        next: 'fa fa-chevron-right',
        today: 'fa fa-check',
        clear: 'fa fa-trash',
        close: 'fa fa-times'
    }
});
$("#strStartDate").datetimepicker({
    format: 'DD-MM-YYYY',   
    icons: {
        time: 'fa fa-clock-o',
        date: 'fa fa-calendar',
        up: 'fa fa-chevron-up',
        down: 'fa fa-chevron-down',
        previous: 'fa fa-chevron-left',
        next: 'fa fa-chevron-right',
        today: 'fa fa-check',
        clear: 'fa fa-trash',
        close: 'fa fa-times'
    }
}).on('dp.change', function (e) {
    if (e.date) {
        $('#strRenewalDate').data("DateTimePicker");

        var selectedText = $('#PaymentMode option:selected').text().trim();
        if (selectedText != null && selectedText != undefined && selectedText != '' && selectedText !='- Select Payment Mode -') {
            let strRenewalDate;
            switch (selectedText) {
                case "Monthly":
                    strRenewalDate = e.date.clone().add(1, 'months');
                    break;
                case "Quarterly":
                    strRenewalDate = e.date.clone().add(3, 'months');
                    break;
                case "Half Yearly":
                    strRenewalDate = e.date.clone().add(6, 'months');
                    break;
                case "Yearly":
                    strRenewalDate = e.date.clone().add(1, 'years');
                    break;
            }
            $('#strRenewalDate').data("DateTimePicker").date(strRenewalDate);
        }
    }
});
$("#strBirthDate").datetimepicker({
    maxDate: moment().subtract(1, 'days'),
    format: 'DD-MM-YYYY',  
    icons: {
        time: 'fa fa-clock-o',
        date: 'fa fa-calendar',
        up: 'fa fa-chevron-up',
        down: 'fa fa-chevron-down',
        previous: 'fa fa-chevron-left',
        next: 'fa fa-chevron-right',
        today: 'fa fa-check',
        clear: 'fa fa-trash',
        close: 'fa fa-times'
    }
});

function DeleteDocument(id) {
    showConfirmation("Are you sure? Do you want to delete ?", 'Yes').then((result) => {
        if (result.value) {
            $.post(SiteUrl + 'InsuranceMaster/DeleteDocument', { id: id }, function (data) {
                if (data.Success) {
                    $("#routeFile").remove();
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
};

$(document).ready(function () {
    $('#document').on('change', function () {
        var file = this.files[0];
        var errorMsg = $('#fileimgErrorMsg');

        if (file) {
            if (file.type !== 'application/pdf') {
                errorMsg.text('Only PDF files are allowed.');
                $(this).val(''); 
            } else {
                errorMsg.text('');
            }
        } else {
            errorMsg.text('');
        }
    });
});

