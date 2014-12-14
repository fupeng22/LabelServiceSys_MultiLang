$(function () {
    var LoginURL = "/" + pub_WhichLang + "/Login/Login?strUserName=";
    var DefaultURL = "/" + pub_WhichLang + "/Default/Index";

    $("#btnLogin").click(function () {
        var userName = $("#txtUserName").val();
        var password = $("#txtUserPassword").val();
        var strRetURL = $("#hidRetURL").val();
        if (userName == "" || password == "") {
            $.messager.alert('操作提示', '请填写完整信息<br/>(用户名、密码)', "error");
            return false;
        }
        var bOK = false;
        $.ajax({
            type: "POST",
            url: LoginURL + encodeURI(userName) + "&strUserPwd=" + encodeURI(password),
            data: "",
            async: false,
            cache: false,
            beforeSend: function (XMLHttpRequest) {

            },
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    //reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                    bOK = true;
                } else {
                    $.messager.alert('操作提示', JSONMsg.message, 'error');
                }
            },
            complete: function (XMLHttpRequest, textStatus) {

            },
            error: function () {

            }
        });
        if (bOK) {
            if ($("#hidRetURL").val() != "") {
                strRetURL = $("#hidRetURL").val();
            } else {
                strRetURL = DefaultURL;
            }
            window.location = strRetURL;
        }
    });

    $("#btnClear").click(function () {
        $("#txtUserName").val("");
        $("#txtUserPassword").val("");
    });

    $(document).keydown(function (event) {
        if (event.keyCode == 13) {
            $("#btnLogin").click();
        }
    });
});