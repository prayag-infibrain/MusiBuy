$('#Phone').mask('999 999 9999?9', {
    translation: {
        '0': { pattern: /[0-9]/ }
    }
});
function funCofirmPassword() {
    document.getElementById("Password").value.length > 0 && document.getElementById("ConfirmPassword").value.length > 0 && document.getElementById("Password").value != document.getElementById("ConfirmPassword").value ? (document.getElementById("spanConfirm").innerHTML = "Passwords do not match.", document.getElementById("spanConfirm").style.color = "red") : document.getElementById("Password").value.length > 0 && document.getElementById("ConfirmPassword").value.length == 0 ? (document.getElementById("spanConfirm").innerHTML = "Not Confirmed", document.getElementById("spanConfirm").style.color = "red") : (document.getElementById("spanConfirm").innerHTML = "Confirmed", document.getElementById("spanConfirm").style.color = "green")
}
jQuery(document).ready(function () {
    $("#Password").keyup(function () {
        $("#result").html(passwordStrengthForResetPassword($("#Password").val()));
        document.getElementById("result").style.color = $("#result").html() == "  Too short" || $("#result").html() == "  Average" ? "red" : "green"
    })
});
$(function () {
    $("#Password").change(function () {
        return document.getElementById("Password").value.length > 0 ? (document.getElementById("spanConfirm").innerHTML = "Not Confirmed", document.getElementById("spanConfirm").style.color = "red") : document.getElementById("spanConfirm").innerHTML = "", !1
    })
})