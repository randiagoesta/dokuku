ErrorMessage = {
    value: function () {
        var defaults = {
            error: false,
            username: ''
        };
        var message,ErrorMessage;
        message = $.extend(defaults, $.getUrlVars());
        if(message.error == 'true')
            ErrorMessage = "Email atau Kata Sandi Salah";
        $(this).text(ErrorMessage);
    }
};$.fn.ErrorMessage = ErrorMessage.value;

$(document).ready(function () {
    $("#error").ErrorMessage();
});