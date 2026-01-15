$(document).ready(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
    $('#loader').modal('hide');
});

function OnUserBound(n) {
    if (!n.sender.dataSource.view().length) { var t = n.sender.thead.find("th:visible").length, i = '<tr><td colspan="' + t + '" align="center">No records found.<\/td><\/tr>'; n.sender.tbody.parent().width(n.sender.thead.width()).end().html(i) }
}

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

async function showConfirmation(message, buttonText) {
    return Swal.fire(
        {
            title: "Are you sure?",
            text: message,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: buttonText
        });
}


function buyNow(planId) {
    debugger;

    $.ajax({
        url: SiteUrl + "Payment/CreateCheckoutSession",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({ "planId": planId, "userId": 1 }),
        success: function (data) {
            var stripe = Stripe(window.stripePublishableKey);
            console.log(data);
            window.location.href = data.id;
            /*stripe.redirectToCheckout({ sessionId: data.id });*/
        },
        error: function () {
            alert('Error starting payment session.');
        }
    });
}


