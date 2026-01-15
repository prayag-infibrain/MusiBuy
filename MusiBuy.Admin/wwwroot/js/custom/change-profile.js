$('#Phone').mask('999 999 99?9999', {
    translation: {
        '0': { pattern: /[0-8]/ }
    }
});

$(document).ready(function () {
    $('.select2').select2();
});
