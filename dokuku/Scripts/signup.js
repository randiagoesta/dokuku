$(document).ready(function () {
    $('.Capcha').QapTcha({ disabledSubmit: true, autoRevert: true });
    addLoadEvent(prepareInputsForHints);
    $("input#Username").blur(ValidateEmail);
    $.fn.GetErrorMessage = ErrorMessage.getByUrl;
    $.fn.ErrorMessage = ErrorMessage.get;
    $("#error").GetErrorMessage();
    $("#register").submit(Validate);
    $("#register").keypress(ClearErrorMessage);
});

function ValidateEmail() {
    var username = $(this).val();
    $.ajax({
        type: 'GET',
        url: '/signup/validate/' + username,
        async: false,
        dataType: 'json',
        success: ValidateEmailCallBack
    });
}
function ValidateEmailCallBack(data) {
    if (data.Error != true) {
        if (data.Registered) {
            $(".validateEmail").remove();
            $('<div>', { 'class': 'validateEmail', text: 'Sudah Terdaftar' }).insertAfter('#Username');
        }
        else {
            $(".validateEmail").remove();
        }
    }
    else {
        $("#error").ErrorMessage({ message: data.Message, error: false });
    }
}
function Validate(e) {
    var  
        form = $('form#register'),
        username = $('input#Username').val(),
        password = $('input#Password').val(),
        rePassword = $('input#RePassword').val(),
        agree = $("#Agree").attr('checked');
    if (password !== rePassword)
    {
        e.preventDefault();
        $("#error").ErrorMessage({ message: 'Kata Sandi yang dimasukkan tidak cocok', error: false });
        return;
    }
    if(agree == undefined) {
        e.preventDefault();
        $("#error").ErrorMessage({ message: 'Anda belum mensetujui persyaratan untuk mendaftar ke Dokuku', error: false });
        return;
    }
    form.submit();
}

ErrorMessage = {
    getByUrl: function () {
        var defaults = {
            error: false,
            message: ''
        };
        var _message, ErrorMessage;
        _message = $.extend(defaults, $.getUrlVars());
        if (_message.error == 'true') {
            $(this).text(getParameterByName('message'));
        }
    },
    get: function (options) {
        var defaults = {
            message: ''
        };
        var _message, ErrorMessage;
        _message = $.extend(defaults, options);
        $(this).text(_message.message);
    }
};

function ClearErrorMessage() {
    $("#error").empty();
}