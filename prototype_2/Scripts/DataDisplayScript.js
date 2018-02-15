$(function () {
    if (!Modernizr.inputtypes.datetime) {
        $("input[type='datetime']").datepicker();
    }
});
