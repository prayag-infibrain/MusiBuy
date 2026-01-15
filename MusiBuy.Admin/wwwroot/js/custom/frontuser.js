$(function () {
    $('#Phone').mask('999 999 99?9999', {
        translation: {
            '0': { pattern: /[0-8]/ }
        }
    });
});

$(document).ready(function () {
    $('.select2').select2();
});

$('#Mobile').mask('999 999 99?9999', {
    translation: {
        '0': { pattern: /[0-8]/ }
    }
});
$(document).ready(function () {
    $("#grid table").addClass("table-responsive").addClass("tablestyle");
    var n = $("input[name=chkDelete]");
    n.prop("checked", false);
    $("#allCheckBox").prop("checked", false);
});


function passwordStrength(password, username) { if (password.length < 6) { return "&nbsp;&nbsp;Too short"; } else if (password.length < 10) { return "&nbsp;&nbsp;Average"; } else { return "&nbsp;&nbsp;Strong"; } }
var _NotConfirmed = "Not Confirmed";
var _Confirmed = "Confirmed";
var _PasswordNotMatch = "Password and confirmed password do not match.";
var _PasswordRequired = "Confirm password is required.";

$(document).ready(function () {  $("#Password").keyup(function () {  $("#passComment").html(passwordStrength($("#Password").val(), $("#Username").val())); $("#passComment").css("color", ($("#passComment").html() === "&nbsp;&nbsp;Too short" || $("#passComment").html() === "&nbsp;&nbsp;Average") ? "red" : "green");});$("#Username").focus(); $("#Password").on("change", function () { if (document.getElementById("Password").value.length > 0) { document.getElementById("spanConfirm").innerHTML = _PasswordRequired; document.getElementById("spanConfirm").style.color = "red";} else { document.getElementById("spanConfirm").innerHTML = "";}return false;});});
function funCofirmPassword() { var password = document.getElementById("Password").value; var confirmPassword = document.getElementById("ConfirmPassword").value; var spanConfirm = document.getElementById("spanConfirm"); if (password && !confirmPassword) { spanConfirm.innerHTML = _PasswordRequired; spanConfirm.style.color = "red"; } else if (password && confirmPassword) { if (password !== confirmPassword) { spanConfirm.innerHTML = _PasswordNotMatch; spanConfirm.style.color = "red"; } else { spanConfirm.innerHTML = _Confirmed; spanConfirm.style.color = "green"; } } else { spanConfirm.innerHTML = ""; } }
$("#frmUser").on('submit', function () {
    $("#btnSubmit").attr("disabled", true);
    var valid = $("#frmUser").valid();
    if ($("#frmUser").valid() && !isEmailDup && !isUserDup) {
        if (document.getElementById("Password").value != document.getElementById("ConfirmPassword").value) {
            $("#btnSubmit").attr("disabled", false);
            return false;
        }
        if (isEmailDup || isUserDup) {
            if ($(".validation-summary-errors ul li#user-name-error").length < 1 && isUserDup) {
                var $li = $("<li id='user-name-error'/>").html(userDupMessage);
                $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
                $(".validation-summary-errors ul").append($li);
            }
            if ($(".validation-summary-errors ul li#email-name-error").length < 1 && isEmailDup) {
                var $li = $("<li id='email-name-error'/>").html(emailDupMessage);
                $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
                $(".validation-summary-errors ul").append($li);
            }
            $("#btnSubmit").attr("disabled", false);
            return false;
        }
    }
    else {
        if ($(".validation-summary-errors ul li#user-name-error").length < 1 && isUserDup) {
            var $li = $("<li id='user-name-error'/>").html(userDupMessage);
            $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
            $(".validation-summary-errors ul").append($li);
        }
        if ($(".validation-summary-errors ul li#email-name-error").length < 1 && isEmailDup) {
            var $li = $("<li id='email-name-error'/>").html(emailDupMessage);
            $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
            $(".validation-summary-errors ul").append($li);
        }
        //e.preventdefault()
        $("#btnSubmit").attr("disabled", false);
        return false;
    }
    if (valid) {
        return true;
    }
});

var isUserDup = false;
var isEmailDup = false;
var userDupMessage = "";
var emailDupMessage = "";

function IsUserNameExists() {
    if ($("#Username").val().trim().length > 0) {
        var n = SiteUrl + "User/ValidateDuplicateUser",
            t = { username: $("#Username").val().trim(), Id: id };
        $.post(n, t, function (n) {
            $(".validation-summary-errors ul li#user-name-error").remove();
            if (n.Status) {
                isUserDup = true;
                userDupMessage = n.Message;
                if ($(".validation-summary-errors ul li#user-name-error").length < 1) {
                    var $li = $("<li id='user-name-error'/>").html(n.Message);
                    $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
                    $(".validation-summary-errors ul").append($li);
                }
            }
            else {
                isUserDup = false;
                if ($(".validation-summary-errors ul li:visible").length < 1) {
                    $(".validation-summary-errors").addClass("validation-summary-valid").removeClass("validation-summary-errors");
                }
            }
        }, "json")
    }
}

function IsUserEmailExists() {
    if ($("#Username").val().trim().length > 0) {
        var n = SiteUrl + "User/ValidateDuplicateEmail",
            t = { email: $("#Email").val().trim(), Id: id };
        $.post(n, t, function (n) {
            $(".validation-summary-errors ul li#email-name-error").remove();
            if (n.Status) {
                isEmailDup = true;
                emailDupMessage = n.Message;
                if ($(".validation-summary-errors ul li#email-name-error").length < 1) {
                    var $li = $("<li id='email-name-error'/>").html(n.Message);
                    $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
                    $(".validation-summary-errors ul").append($li);
                }
            }
            else {
                isEmailDup = false;
                if ($(".validation-summary-errors ul li:visible").length < 1) {
                    $(".validation-summary-errors").addClass("validation-summary-valid").removeClass("validation-summary-errors");
                }
            }
        }, "json")
    }
}

$(function () {

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


$("#frmUserEdit").on('submit', function () {
    $("#btnUpdate").attr("disabled", true);
    var valid = $("#frmUserEdit").valid();
    if ($("#frmUserEdit").valid() && !isEmailDup && !isUserDup) {
        if (document.getElementById("Password").value != document.getElementById("ConfirmPassword").value) {
            $("#btnUpdate").attr("disabled", false);
            return false;
        }
        if (isEmailDup || isUserDup) {
            if ($(".validation-summary-errors ul li#user-name-error").length < 1 && isUserDup) {
                var $li = $("<li id='user-name-error'/>").html(userDupMessage);
                $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
                $(".validation-summary-errors ul").append($li);
            }
            if ($(".validation-summary-errors ul li#email-name-error").length < 1 && isEmailDup) {
                var $li = $("<li id='email-name-error'/>").html(emailDupMessage);
                $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
                $(".validation-summary-errors ul").append($li);
            }
            $("#btnUpdate").attr("disabled", false);
            return false;
        }
    }
    else {
        if ($(".validation-summary-errors ul li#user-name-error").length < 1 && isUserDup) {
            var $li = $("<li id='user-name-error'/>").html(userDupMessage);
            $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
            $(".validation-summary-errors ul").append($li);
        }
        if ($(".validation-summary-errors ul li#email-name-error").length < 1 && isEmailDup) {
            var $li = $("<li id='email-name-error'/>").html(emailDupMessage);
            $(".validation-summary-valid").addClass("validation-summary-errors").removeClass("validation-summary-valid");// 
            $(".validation-summary-errors ul").append($li);
        }
        //e.preventdefault()
        $("#btnUpdate").attr("disabled", false);
        return false;
    }
    if (valid) {
        return true;
    }
});





