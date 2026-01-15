$(function () {
    // Initialize the date range picker
    $('#daterange').daterangepicker({
        timePicker: true,
        timePicker24Hour: true,
        timePickerSeconds: false,
        autoUpdateInput: true,
        locale: {
            format: 'YYYY-MM-DD HH:mm:ss'
        }
    }, updateDuration); // Callback on selection

    // Function to update the duration
    function updateDuration(start, end) {
        let diff = moment.duration(end.diff(start));

        let totalDays = Math.floor(diff.asDays());
        let totalHours = Math.floor(diff.asHours());
        let hours = totalHours - (totalDays * 24);
        let minutes = diff.minutes();

        let result = [];
        if (totalDays > 0) result.push(totalDays + ' day' + (totalDays > 1 ? 's' : ''));
        if (hours > 0) result.push(hours + ' hour' + (hours > 1 ? 's' : ''));
        if (minutes > 0) result.push(minutes + ' minute' + (minutes > 1 ? 's' : ''));

        if (result.length === 0) result.push('Less than a minute');

        $('#Duration').val(result.join(' '));



        //let diff = moment.duration(end.diff(start));

        //let days = diff.days();
        //let hours = diff.hours();
        //let minutes = diff.minutes();

        //let result = [];
        //if (days > 0) result.push(days + ' day' + (days > 1 ? 's' : ''));
        //if (hours > 0) result.push(hours + ' hour' + (hours > 1 ? 's' : ''));
        //if (minutes > 0) result.push(minutes + ' minute' + (minutes > 1 ? 's' : ''));

        //if (result.length === 0) result.push('Less than a minute');

        //$('#Duration').val(result.join(' '));
    }

    // 🔁 Trigger duration calculation on page load (if input has value)
    let val = $('#daterange').val();
    if (val.includes(' - ')) {
        let parts = val.split(' - ');
        let start = moment(parts[0], 'YYYY-MM-DD HH:mm:ss');
        let end = moment(parts[1], 'YYYY-MM-DD HH:mm:ss');
        if (start.isValid() && end.isValid()) {
            updateDuration(start, end);
        }
    }
});







