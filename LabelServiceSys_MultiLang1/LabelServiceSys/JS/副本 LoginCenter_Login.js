$(function () {
    var _$_txtWorkProcess_L1_Code = $("#txtWorkProcess_L1_Code");
    var _$_txtWorkProcess_L2_Code = $("#txtWorkProcess_L2_Code");
    var LoadJobNameL1ByUsernumberURL = "/Default/LoadJobNameIdLv1ByUsernumber?userNumber=";
    var LoadJobNameL2ByJobNameL1IdURL = "/Default/LoadJobNameIdLv2ByJobNameL1Id?JobNameL1Id=";
    var LoginURL = "/LoginCenter/Login?strUserNum=";
    _$_txtWorkProcess_L1_Code.combobox({
        url: LoadJobNameL1ByUsernumberURL + encodeURI($("#txtUserNumber").val()),
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onLoadSuccess: function () {
            _$_txtWorkProcess_L1_Code.combobox("setValue", "-99");

        },
        onChange: function (NewRow, OldRow) {
            _$_txtWorkProcess_L2_Code.combobox({
                url: LoadJobNameL2ByJobNameL1IdURL + encodeURI(NewRow),
                valueField: 'id',
                textField: 'text',
                editable: false,
                panelHeight: null,
                onLoadSuccess: function () {
                    _$_txtWorkProcess_L2_Code.combobox("setValue", "-99");
                }
            });
        }
    });



    $("#btnLogin").click(function () {
        var JobNameL1Code = _$_txtWorkProcess_L1_Code.combobox("getText");
        var JobNameL2Code = _$_txtWorkProcess_L2_Code.combobox("getText");
        var JobNameL1Id = _$_txtWorkProcess_L1_Code.combobox("getValue");
        var JobNameL2Id = _$_txtWorkProcess_L2_Code.combobox("getValue");
        var userNumber = $("#txtUserNumber").val();
        var isDVIR = $("#txtIsDVIR").val();
        //        console.info("JobNameL1Code:" + JobNameL1Code);
        //        console.info("JobNameL2Code:" + JobNameL2Code);
        //        console.info("JobNameL1Id:" + JobNameL1Id);
        //        console.info("JobNameL2Id:" + JobNameL2Id);
        //        return false;
        if (!JobNameL1Id || JobNameL1Id == "-99") {
            $.messager.alert('操作提示', "请选择一级工作内容", 'info');
            return false;
        }
        if (!JobNameL2Id || JobNameL2Id == "-99") {
            $.messager.alert('操作提示', "请选择二级工作内容", 'info');
            return false;
        }

        $.ajax({
            url: LoginURL + encodeURI(userNumber) + "&JobNameL1Code=" + encodeURI(JobNameL1Code) + "&JobNameL2Code=" + encodeURI(JobNameL2Code) + "&IsDVIR=" + encodeURI(isDVIR),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    $.messager.alert('操作提示', JSONMsg.message, 'info', function () {
                        window.location = "/LoginCenter/Index?usernumber=" + encodeURI(userNumber) + "&isdvir=" + encodeURI(isDVIR) + "&devicetype=0";
                    });

                } else {
                    $.messager.alert('操作提示', JSONMsg.message, 'error');
                    return false;
                }
            }
        });
    });
});