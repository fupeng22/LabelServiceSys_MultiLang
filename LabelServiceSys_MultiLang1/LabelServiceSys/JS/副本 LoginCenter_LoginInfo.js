$(function () {
    var LoginOutURL = "/LoginCenter/LoginOut?strUserNum=";
    $("#btnLoginOut").click(function () {
        $.ajax({
            url: LoginOutURL + encodeURI($("#txtUserNumber").val()),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    $.messager.alert('操作提示', JSONMsg.message, 'info', function () {
                        window.location = "/LoginCenter/Login?userNumber=" + encodeURI($("#txtUserNumber").val()) + "&isDVIR=" + encodeURI($("#txtIsDVIR").val());
                    });

                } else {
                    $.messager.alert('操作提示', JSONMsg.message, 'error');
                    return false;
                }
            }
        });
    });
});