$(function () {
//    $("#bindTitle").html("Bind infomation");
    var isDVIR = $("#txtIsDVIR").val();
    var LoginOutURL = "/" + pub_WhichLang + "/LoginCenter/LoginOut?strUserNum=";
//    switch (isDVIR) {
//        case "1":
//            $("#chkIsDVIR").attr("checked", "checked");
//            break;
//        case "0":
//            $("#chkIsDVIR").removeAttr("checked");
//            break;
//        default:
//            $("#chkIsDVIR").removeAttr("checked");
//            break;
//    }

//    $("#chkIsDVIR").click(function () {
//        return false;
//    });

    $("#btnLoginOut").click(function () {
        $.ajax({
            url: LoginOutURL + encodeURI($("#txtUserNumber").val()),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    DisplayMsg(0, JSONMsg.message);
                    //alert(JSONMsg.message);
                    //$.messager.alert('操作提示', JSONMsg.message, 'info', function () {
                    window.location = "/" + pub_WhichLang + "/LoginCenter/Login?userNumber=" + encodeURI($("#txtUserNumber").val()) + "&isDVIR=" + encodeURI($("#txtIsDVIR").val()) + "&WorkstationId=" + encodeURI($("#WorkstationId").val()) + "&getWorkstationId=" + encodeURI($("#hd_getWorkstationId").val());
                    //});

                } else {
                    DisplayMsg(1, JSONMsg.message);
                    //alert(JSONMsg.message);
                    //$.messager.alert('操作提示', JSONMsg.message, 'error');
                    return false;
                }
            }
        });
    });

    function DisplayMsg(type, msg) {
        switch (type) {
            case 0:
                $("#lblErrorTip").css("background-color", "blue");
                $("#lblErrorTip").html(msg);
                break;
            case 1:
                $("#lblErrorTip").css("background-color", "red");
                $("#lblErrorTip").html(msg);
                break;
            default:
                break;
        }
    }

    $("#btnLoginOut").blur();
    $("#btnLoginOut").focus();
});