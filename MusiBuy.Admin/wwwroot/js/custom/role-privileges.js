function UnSelectHeaderCheckbox(n, t) { var i = !1; $(".grid-tab tr").find("input:checkbox").each(function () { $(this).parent().attr("class") == "grid-text " + t && (i = this.checked); $(this).parent().attr("class") == "grid-header " + n && (i ? $(this).prop("checked", !0) : $(this).prop("checked", !1)) }) } function UncheckParentCheckbox(n, t, i) { var u = document.getElementById(n), r = document.getElementById(t); i.checked && (r.checked = !0) } function UncheckMainParentCheckbox(n, t) { if (!t.checked) { var i = document.getElementById(n); i != null && (i.checked = !1) } } function CheckUncheckParentCheckBox(n) { n != "" && $(".grid-tab tr").find("input:checkbox").each(function () { $(this).attr("class") == n && $(this).prop("checked", !0) }) } function CheckUncheckChildCheckBox(n, t, i) { var r = !1, u = "chkView" + i, f = "chkAdd" + i, e = "chkEdit" + i, o = "chkDelete" + i, s = "chkDetail" + i, h = "chkAll" + i; t != "" && $(".grid-tab tr").find("input:checkbox").each(function () { $(this).attr("class") == t && (r = this.checked); ($(this).attr("class") == u || $(this).attr("class") == f || $(this).attr("class") == e || $(this).attr("class") == o || $(this).attr("class") == s || $(this).attr("class") == h) && $(this).prop("checked", r) }) } function CheckUncheckChildCheckBoxNew(n, t, i) { n != "" && $(".grid-tab tr").find("input:checkbox").each(function () { var u = $(this), r; u[0].id != null && (r = u[0].id.split("_"), r.length > 1 && r[1] == n && r[0] == t && $(this).prop("checked", i.checked)) }) } function getPrivileges() { $("#frmRolePrivileges").submit() } function SelectViewDetail(n, t) { t.checked && $("input[value=v" + n + "]").prop("checked", !0) } $(document).ready(function () { $("#chkViewAll").click(function () { $(this).parents("table").find("INPUT[name='chkView']").prop("checked", $(this).is(":checked")) }) }); $(document).ready(function () { $("#chkAddAll").click(function () { $(this).parents("table").find("INPUT[name='chkAdd']").prop("checked", $(this).is(":checked")); $(this).parents("table").find("INPUT[name='chkView']").prop("checked", !0); $("#chkViewAll").prop("checked", !0) }) }); $(document).ready(function () { $("#chkEditAll").click(function () { $(this).parents("table").find("INPUT[name='chkEdit']").prop("checked", $(this).is(":checked")); $(this).parents("table").find("INPUT[name='chkView']").prop("checked", !0); $("#chkViewAll").prop("checked", !0) }) }); $(document).ready(function () { $("#chkDeleteAll").click(function () { $(this).parents("table").find("INPUT[name='chkDelete']").prop("checked", $(this).is(":checked")); $(this).parents("table").find("INPUT[name='chkView']").prop("checked", !0); $("#chkViewAll").prop("checked", !0) }) }); $(document).ready(function () { $("#chkDetailAll").click(function () { $(this).parents("table").find("INPUT[name='chkDetail']").prop("checked", $(this).is(":checked")); $(this).parents("table").find("INPUT[name='chkView']").prop("checked", !0); $("#chkViewAll").prop("checked", !0) }) })
function CheckUncheckAll(value, value2, parantId) {
    if ($("#Add_" + value2).is(":checked") || $("#Edit_" + value2).is(":checked") || $("#Delete_" + value2).is(":checked") || $("#Detail_" + value2).is(":checked")) {
        $("#View_" + value2).prop("checked", true);
    }

    if ($("#View_" + value2).is(":checked")) {
        $("#" + parantId).prop("checked", true);
    }
    if ($("#View_" + value2).is(":unchecked")) {

        $("#" + parantId).prop("checked", false);
        $("#Add_" + value2).prop("checked", false);
        $("#Edit_" + value2).prop("checked", false);
        $("#Delete_" + value2).prop("checked", false);
        $("#Detail_" + value2).prop("checked", false);
    }
    if ($("#Add_" + value2).is(":unchecked")) {
        $("#Edit_" + value2).prop("checked", false);
        $("#Delete_" + value2).prop("checked", false);
    }
}