$(function () {
    var isDVIR = $("#txtIsDVIR").val();
    var hd_getWorkstationId = $("#hd_getWorkstationId").val();

    var LoadJobNameL1URL = "/" + pub_WhichLang + "/Default/LoadJobNameIdLv1ByUsernumber?userNumber=";
    var LoadJobNameL2URL = "/" + pub_WhichLang + "/Default/LoadJobNameIdLv2ByJobNameL1Code?JobNameL1Code=";
    var LoginURL = "/" + pub_WhichLang + "/LoginCenter/Login_Post?strUserNum=";
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

    //    $("#bindTitle").html("Bind");

    //    $("#chkIsDVIR").click(function () {
    //        return false;
    //    });

    $("#ddlWorkProcess_L1_Code").html("");
    $.ajax({
        url: LoadJobNameL1URL + encodeURI($("#txtUserNumber").val()),
        type: "POST",
        cache: false,
        async: false,
        success: function (msg) {
            if (hd_getWorkstationId == "1") {
                $("#ddlWorkProcess_L1_Code").append("<option value=\"" + $("#hd_WorkprocessL1_Code").val() + "\">" + $("#hd_WorkprocessL1_Text").val() + "</option>");
                ReloadWorkProcess_L2_Code("-99");
            } else {
                var JSONMsg = eval("(" + msg + ")");
                for (var i = 0; i < JSONMsg.length; i++) {
                    $("#ddlWorkProcess_L1_Code").append("<option value=\"" + JSONMsg[i].id + "\">" + JSONMsg[i].text + "</option>");
                }
                ReloadWorkProcess_L2_Code("-99");
            }
        }
    });

    $("#ddlWorkProcess_L1_Code").change(function (data) {
        ReloadWorkProcess_L2_Code($("#ddlWorkProcess_L1_Code").val());
    });

    function ReloadWorkProcess_L2_Code(JobNameL1Id) {
        $("#ddlWorkProcess_L2_Code").html("");
        $.ajax({
            url: LoadJobNameL2URL + encodeURI(JobNameL1Id),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                if (hd_getWorkstationId == "1") {
                    $("#ddlWorkProcess_L2_Code").append("<option value=\"" + $("#hd_WorkprocessL2_Code").val() + "\">" + $("#hd_WorkprocessL2_Text").val() + "</option>");
                } else {
                    var JSONMsg = eval("(" + msg + ")");
                    for (var i = 0; i < JSONMsg.length; i++) {
                        $("#ddlWorkProcess_L2_Code").append("<option value=\"" + JSONMsg[i].id + "\">" + JSONMsg[i].text + "</option>");
                    }
                }
            }
        });
    }

    $("#btnLogin").click(function () {
        var JobNameL1Code = $("#ddlWorkProcess_L1_Code  option:selected").html();
        var JobNameL2Code = $("#ddlWorkProcess_L2_Code  option:selected").html();
        var JobNameL1Id = $("#ddlWorkProcess_L1_Code  option:selected").val();
        var JobNameL2Id = $("#ddlWorkProcess_L2_Code  option:selected").val();

        var userNumber = $("#txtUserNumber").val();
        var isDVIR = $("#txtIsDVIR").val();

        if (JobNameL1Id == "-99") {
            //DisplayMsg(1, "请选择一级工作内容");
            DisplayMsg(1, LoginCenter_JS_Login_ErrorMessage1);
            return false;
        }

        if (JobNameL2Id == "-99") {
            //DisplayMsg(1, "请选择二级工作内容");
            DisplayMsg(1, LoginCenter_JS_Login_ErrorMessage2);
            return false;
        }

        $.ajax({
            url: LoginURL + encodeURI(userNumber) + "&JobNameL1Code=" + encodeURI(JobNameL1Id) + "&JobNameL2Code=" + encodeURI(JobNameL2Id) + "&IsDVIR=" + encodeURI(isDVIR),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    DisplayMsg(0, JSONMsg.message);
                    //alert(JSONMsg.message);
                    //$.messager.alert('操作提示', JSONMsg.message, 'info', function () {
                    //console.info( "/" + pub_WhichLang + "/LoginCenter/Index?usernumber=" + encodeURI(userNumber) + "&isdvir=" + encodeURI(isDVIR) + "&devicetype=0&getWorkstation=" + encodeURI($("#hd_getWorkstationId").val()) + "&workstation=" + encodeURI($("#hd_WorkstationId").val()));
                    window.location = "/" + pub_WhichLang + "/LoginCenter/Index?usernumber=" + encodeURI(userNumber) + "&isdvir=" + encodeURI(isDVIR) + "&devicetype=0&getWorkstation=" + encodeURI($("#hd_getWorkstationId").val()) + "&workstation=" + encodeURI($("#hd_WorkstationId").val());
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
                $("#lblErrorTip").css("display", "block");
                $("#lblErrorTip").css("background-color", "blue");
                $("#lblErrorTip").html(msg);
                setTimeout(function () {
                    $("#lblErrorTip").css("display", "none");
                }, 5000);
                break;
            case 1:
                $("#lblErrorTip").css("display", "block");
                $("#lblErrorTip").css("background-color", "red");
                $("#lblErrorTip").html(msg);
                setTimeout(function () {
                    $("#lblErrorTip").css("display", "none");
                }, 5000);
                break;
            default:
                break;
        }
    }

    $("#btnLogin").blur();
    $("#btnLogin").focus();
});