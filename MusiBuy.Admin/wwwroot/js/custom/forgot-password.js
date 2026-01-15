$(function () {
    $('#Mobile').mask('999 999 9999?9', {
        translation: {
            '0': { pattern: /[0-9]/ }
        }
    });
});
