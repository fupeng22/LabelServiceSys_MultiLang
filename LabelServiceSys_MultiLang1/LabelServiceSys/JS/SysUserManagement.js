$(function () {
    var _$_datagrid = $("#DG_UserMaintain");
    var _$_cmbUnitCode = $("#cmbUnitCode");
    var QueryURL = "/" + pub_WhichLang + "/SysUserManagement/GetData";
    var CreateURL = "/" + pub_WhichLang + "/SysUserManagement/AddUsers?UserName=";
    var UpdateURL = "/" + pub_WhichLang + "/SysUserManagement/UpdateUsers?UserId=";
    var DeleteURL = "/" + pub_WhichLang + "/SysUserManagement/Delete?ids=";
    var TestExist_UserName = "/" + pub_WhichLang + "/SysUserManagement/ExistUserNum?strUserNum=";
    var TestExist_UserName_Update = "/" + pub_WhichLang + "/SysUserManagement/ExistUserNum_Update?strUserNum=";

    var LoadUnitCodeURL = "/" + pub_WhichLang + "/Default/LoadUnitCodeWithId";

    var CreateDlg = null;
    var CreateDlgForm = null;

    var PrintURL = "";

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'auNum',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'auID',
        columns: [[
                        { field: 'cb', width: 120, checkbox: true },
    					{ field: 'auNum', title: '用户号', width: 120, sortable: true,
    					    sorter: function (a, b) {
    					        return (a > b ? 1 : -1);
    					    }
    					},
                        { field: 'auUnitCodeIDs_Des', title: '口岸', width: 400, sortable: true,
                            sorter: function (a, b) {
                                return (a > b ? 1 : -1);
                            }
                        },
    				]],
        pagination: true,
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        toolbar: [{
            id: 'btnQuery',
            text: '查询',
            iconCls: 'icon-search',
            handler: function () {
                _$_datagrid.datagrid("reload");
                _$_datagrid.datagrid("unselectAll");
            }
        }, '-', {
            id: 'btnAdd',
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                Add();
            }
        }, '-', {
            id: 'btnUpdate',
            text: '修改',
            iconCls: 'icon-edit',
            handler: function () {
                Update();
            }
        }, '-', {
            id: 'btnDelete',
            text: '删除',
            disabled: false,
            iconCls: 'icon-remove',
            handler: function () {
                Delete();
            }
        }, '-', {
            id: 'btnSeleAll',
            text: '全选',
            disabled: false,
            iconCls: 'icon-seleall',
            handler: function () {
                SeleAll();
            }
        }, '-', {
            id: 'btnInverseSele',
            text: '反选',
            disabled: false,
            iconCls: 'icon-inversesele',
            handler: function () {
                InverseSele();
            }
        }, '-', {
            id: 'btnPrint',
            text: '打印',
            disabled: false,
            iconCls: 'icon-print',
            handler: function () {
                Print();
            }
        }, '-', {
            id: 'btnExcel',
            text: '导出',
            disabled: false,
            iconCls: 'icon-excel',
            handler: function () {
                Excel();
            }
        }],
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html("查询").appendTo(cmenu);
            $('<div  id="mnuAdd" iconCls="icon-add"/>').html("添加").appendTo(cmenu);
            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html("修改").appendTo(cmenu);
            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html("删除").appendTo(cmenu);
            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html("全选").appendTo(cmenu);
            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html("反选").appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html("打印").appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html("导出").appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnuadd":
                            Add();
                            break;
                        case "mnuupdate":
                            Update();
                            break;
                        case "mnudelete":
                            Delete();
                            break;
                        case "mnuseleall":
                            SeleAll();
                            break;
                        case "mnuinversesele":
                            InverseSele();
                            break;
                        case "mnuprint":
                            Print();
                            break;
                        case "mnuexcel":
                            Excel();
                            break;
                    }
                }
            });

            $('#cmenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onHeaderContextMenu: function (e, field) {
            e.preventDefault();
            if (!$('#tmenu').length) {
                createColumnMenu();
            }
            $('#tmenu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onSortColumn: function (sort, order) {
            // _$_datagrid.datagrid("reload");
        },
        onDblClickRow: function (rowIndex, rowData) {
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);
            Update(rowData.cId);
        }
    });

    function Print() {
        PrintURL = "/" + pub_WhichLang + "/SysUserManagement/Print?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
            div_PrintDlg.show();
            var PrintDlg = null;
            div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL);
            PrintDlg = div_PrintDlg.window({
                title: '打印',
                href: "",
                modal: true,
                resizable: true,
                minimizable: false,
                collapsible: false,
                cache: false,
                closed: true,
                width: 900,
                height: 500
            });
            div_PrintDlg.window("open");

        } else {
            reWriteMessagerAlert("提示", "没有数据，不可打印", "error");
            //reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForPrint, "error");
            return false;
        }
    }

    function Excel() {
        var browserType = "";
        if ($.browser.msie) {
            browserType = "msie";
        }
        else if ($.browser.safari) {
            browserType = "safari";
        }
        else if ($.browser.mozilla) {
            browserType = "mozilla";
        }
        else if ($.browser.opera) {
            browserType = "opera";
        }
        else {
            browserType = "unknown";
        }

        PrintURL = "/" + pub_WhichLang + "/SysUserManagement/Excel?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            //reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function Add() {
        $("#txtUserName").val("");
        $("#hd_UserId").val("");
        $("#txtPassword").val("");
        $("#txtRePassword").val("");

        _$_cmbUnitCode.combobox({
            url: LoadUnitCodeURL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            multiple: true,
            multiline: true,
            onLoadSuccess: function () {
                _$_cmbUnitCode.combobox("setValue", "-99");
            }
        });

        CreateDlg = $('#dlg_Create_User').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var _$_txtUserName = $("#txtUserName").val();
                    var _$_txtPassword = $("#txtPassword").val();
                    var _$_txtRePassword = $("#txtRePassword").val();
                    var _$_UnitCode = _$_cmbUnitCode.combobox("getValues").join(",");
                    if (_$_UnitCode == "" || _$_UnitCode == "-99" || _$_txtUserName == "" || _$_txtPassword == "" || _$_txtRePassword == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(用户名、密码、确认密码、口岸)', "error");
                        return false;
                    }

                    if (_$_txtPassword != _$_txtRePassword) {
                        reWriteMessagerAlert('操作提示', '密码与确认密码不一致', "error");
                        return false;
                    }

                    var bExist = false;
                    $.ajax({
                        type: "GET",
                        url: TestExist_UserName + encodeURI(_$_txtUserName),
                        data: "",
                        async: false,
                        cache: false,
                        beforeSend: function (XMLHttpRequest) {

                        },
                        success: function (msg) {
                            var JSONMsg = eval("(" + msg + ")");
                            if (JSONMsg.result.toLowerCase() == 'error') {
                                bExist = true;
                                //reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
                                return false;
                            } else {

                            }
                        },
                        complete: function (XMLHttpRequest, textStatus) {

                        },
                        error: function () {

                        }
                    });

                    if (!bExist) {
                        var bOK = false;
                        $.ajax({
                            type: "POST",
                            url: CreateURL + encodeURI(_$_txtUserName) + "&Password=" + encodeURI(_$_txtPassword) + "&auUnitCodeIDs=" + encodeURI(_$_UnitCode.replace("-99,", "")),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                var JSONMsg = eval("(" + msg + ")");
                                if (JSONMsg.result.toLowerCase() == 'ok') {
                                    bOK = true;
                                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                } else {
                                    reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });
                        if (bOK) {
                            CreateDlg.dialog('close');
                            _$_datagrid.datagrid("reload");
                            _$_datagrid.datagrid("unselectAll");
                        }
                    }
                }
            }, {
                text: '关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateDlg.dialog('close');
                }
            }],
            title: '添加用户',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 300
        });

        $('#dlg_Create_User').dialog("open");
    }

    function Update() {
        var selects = _$_datagrid.datagrid("getSelections");
        if (selects.length != 1) {
            reWriteMessagerAlert("提示", "<center>请选择数据进行修改(<font style='color:red'>只可选择一行</font>)</center>", "error");
            return false;
        } else {
            $("#hd_UserId").val(selects[0].auID);
            $("#txtUserName").val(selects[0].auNum);
            var auUnitCodeIDs = selects[0].auUnitCodeIDs;
            $("#txtPassword").val("");
            $("#txtRePassword").val("");
            _$_cmbUnitCode.combobox({
                url: LoadUnitCodeURL,
                valueField: 'id',
                textField: 'text',
                editable: false,
                panelHeight: null,
                multiple: true,
                multiline: true,
                onLoadSuccess: function () {
                    _$_cmbUnitCode.combobox("setValues", auUnitCodeIDs.split(","));
                }
            });

            CreateDlg = $('#dlg_Create_User').dialog({
                buttons: [{
                    text: '保 存',
                    iconCls: 'icon-ok',
                    handler: function () {
                        var _$_hd_UserId = $("#hd_UserId").val();
                        var _$_txtUserName = $("#txtUserName").val();
                        var _$_txtPassword = $("#txtPassword").val();
                        var _$_txtRePassword = $("#txtRePassword").val();
                        var _$_UnitCode = _$_cmbUnitCode.combobox("getValues").join(",");

                        if (_$_UnitCode == "" || _$_UnitCode == "-99" || _$_hd_UserId == "" || _$_txtUserName == "" || _$_txtPassword == "" || _$_txtRePassword == "") {
                            reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(用户名、密码、确认密码、口岸)', "error");
                            return false;
                        }

                        if (_$_txtPassword != _$_txtRePassword) {
                            reWriteMessagerAlert('操作提示', '密码与确认密码不一致', "error");
                            return false;
                        }

                        //验证此用户名是否已使用
                        var bExist = false;
                        $.ajax({
                            type: "GET",
                            url: TestExist_UserName_Update + encodeURI(_$_hd_UserId) + "&strUserNum=" + encodeURI(_$_txtUserName),
                            data: "",
                            async: false,
                            cache: false,
                            beforeSend: function (XMLHttpRequest) {

                            },
                            success: function (msg) {
                                if (msg.toLowerCase() == 'true') {
                                    reWriteMessagerAlert('操作提示', '此用户名已经使用!', 'error');
                                    bExist = true;
                                }
                            },
                            complete: function (XMLHttpRequest, textStatus) {

                            },
                            error: function () {

                            }
                        });

                        if (!bExist) {
                            var bOK = false;
                            $.ajax({
                                type: "POST",
                                url: UpdateURL + encodeURI(_$_hd_UserId) + "&UserName=" + encodeURI(_$_txtUserName) + "&Password=" + encodeURI(_$_txtPassword) + "&auUnitCodeIDs=" + encodeURI(_$_UnitCode.replace("-99,", "")),
                                data: "",
                                async: false,
                                cache: false,
                                beforeSend: function (XMLHttpRequest) {

                                },
                                success: function (msg) {
                                    var JSONMsg = eval("(" + msg + ")");
                                    if (JSONMsg.result.toLowerCase() == 'ok') {
                                        bOK = true;
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'info');
                                    } else {
                                        reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
                                    }
                                },
                                complete: function (XMLHttpRequest, textStatus) {

                                },
                                error: function () {

                                }
                            });
                            if (bOK) {
                                CreateDlg.dialog('close');
                                _$_datagrid.datagrid("reload");
                                _$_datagrid.datagrid("unselectAll");
                            }
                        }
                    }
                }, {
                    text: '关 闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        CreateDlg.dialog('close');
                    }
                }],
                title: '修改系统用户信息',
                modal: true,
                resizable: true,
                cache: false,
                left: 50,
                top: 30,
                width: 400,
                height: 300,
                closed: true
            });
            _$_datagrid.datagrid("unselectAll");
        }
        $('#dlg_Create_User').dialog("open");


        //        if (id) {
        //            EditDlg = $('#dlg_Update').dialog({
        //                buttons: [{
        //                    text: Public_Dialog_ButtonOfSave, //'保 存',
        //                    iconCls: 'icon-ok',
        //                    handler: function () {
        //                        var txturNum = EditDlg.find('#txturNum').val();
        //                        var txturPSW = EditDlg.find('#txturPSW').val();
        //                        var txtReurPSW = EditDlg.find('#txtReurPSW').val();
        //                        var txturUnitCode = EditDlg.find('#txturUnitCode').val();

        //                        if (txturNum == "" || txturPSW == "" || txtReurPSW == "" || txturUnitCode == "-99") {
        //                            //reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(用户号、密码、确认密码、口岸)', "error");
        //                            reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage1, "error");
        //                            return false;
        //                        }

        //                        if (txturPSW != txtReurPSW) {
        //                            //reWriteMessagerAlert('操作提示', '密码与确认密码不一致', "error");
        //                            reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage2, "error");
        //                            return false;
        //                        }

        //                        if (txturNum.length > 10) {
        //                            // reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
        //                            reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
        //                            $("#txturNum").focus();
        //                            return false;
        //                        }

        //                        if (!(/[\W_]/.test(txturNum) || !txturNum ? false : true)) {
        //                            // reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
        //                            reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
        //                            $("#txturNum").focus();
        //                            return false;
        //                        }

        //                        $("#txturMemo").val($("#areaurMemo").text());

        //                        //验证此用户名是否已使用
        //                        var bExist = false;
        //                        $.ajax({
        //                            type: "GET",
        //                            url: TestExist_UserName_Update + encodeURI(txturNum) + "&id=" + id,
        //                            data: "",
        //                            async: false,
        //                            cache: false,
        //                            beforeSend: function (XMLHttpRequest) {

        //                            },
        //                            success: function (msg) {
        //                                var JSONMsg = eval("(" + msg + ")");
        //                                if (JSONMsg.result.toLowerCase() == 'error') {
        //                                    bExist = true;
        //                                    //reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
        //                                    reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
        //                                    return false;
        //                                } else {

        //                                }
        //                            },
        //                            complete: function (XMLHttpRequest, textStatus) {

        //                            },
        //                            error: function () {

        //                            }
        //                        });

        //                        if (!bExist) {
        //                            EditDlgForm = EditDlg.find('form');
        //                            EditDlgForm.form('submit', {
        //                                url: EditDlgForm.url,
        //                                onSubmit: function () {
        //                                    //return $(this).form('validate');
        //                                },
        //                                success: function () {
        //                                    // reWriteMessagerAlert('提示', '成功', "info");
        //                                    reWriteMessagerAlert(Public_Dialog_Title, Public_Message_OK, "info");
        //                                    EditDlg.dialog('close');
        //                                    _$_datagrid.datagrid("reload");
        //                                    _$_datagrid.datagrid("unselectAll");
        //                                }
        //                            });
        //                        }


        //                    }
        //                }, {
        //                    text: Public_Dialog_ButtonOfClose, // '关 闭',
        //                    iconCls: 'icon-cancel',
        //                    handler: function () {
        //                        EditDlg.dialog('close');
        //                    }
        //                }],
        //                title: User_Dialog_UpdateUser_Title, // '修改系统用户信息',
        //                href: UpdateURL + id + "?d=" + Date(),
        //                modal: true,
        //                resizable: true,
        //                cache: false,
        //                left: 50,
        //                top: 30,
        //                width: 800,
        //                height: 320,
        //                closed: true
        //            });
        //            _$_datagrid.datagrid("unselectAll");
        //        } else {
        //            var selects = _$_datagrid.datagrid("getSelections");
        //            if (selects.length != 1) {
        //                //reWriteMessagerAlert("提示", "<center>请选择数据进行修改(<font style='color:red'>只可选择一行</font>)</center>", "error");
        //                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage4, "error");
        //                return false;
        //            } else {
        //                id = selects[0].urID;
        //                EditDlg = $('#dlg_Update').dialog({
        //                    buttons: [{
        //                        text: Public_Dialog_ButtonOfSave, // '保 存',
        //                        iconCls: 'icon-ok',
        //                        handler: function () {
        //                            var txturNum = EditDlg.find('#txturNum').val();
        //                            var txturPSW = EditDlg.find('#txturPSW').val();
        //                            var txtReurPSW = EditDlg.find('#txtReurPSW').val();
        //                            var txturUnitCode = EditDlg.find('#txturUnitCode').val();

        //                            if (txturNum == "" || txturPSW == "" || txtReurPSW == "" || txturUnitCode == "-99") {
        //                                //reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(用户号、密码、确认密码、口岸)', "error");
        //                                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage1, "error");
        //                                return false;
        //                            }

        //                            if (txturPSW != txtReurPSW) {
        //                                //reWriteMessagerAlert('操作提示', '密码与确认密码不一致', "error");
        //                                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage2, "error");
        //                                return false;
        //                            }

        //                            if (txturNum.length > 10) {
        //                                // reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
        //                                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
        //                                $("#txturNum").focus();
        //                                return false;
        //                            }

        //                            if (!(/[\W_]/.test(txturNum) || !txturNum ? false : true)) {
        //                                //reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
        //                                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
        //                                $("#txturNum").focus();
        //                                return false;
        //                            }

        //                            $("#txturMemo").val($("#areaurMemo").text());

        //                            //验证此用户名是否已使用
        //                            var bExist = false;
        //                            $.ajax({
        //                                type: "GET",
        //                                url: TestExist_UserName_Update + encodeURI(txturNum) + "&id=" + id,
        //                                data: "",
        //                                async: false,
        //                                cache: false,
        //                                beforeSend: function (XMLHttpRequest) {

        //                                },
        //                                success: function (msg) {
        //                                    var JSONMsg = eval("(" + msg + ")");
        //                                    if (JSONMsg.result.toLowerCase() == 'error') {
        //                                        bExist = true;
        //                                        //reWriteMessagerAlert('操作提示', JSONMsg.message, 'error');
        //                                        reWriteMessagerAlert(Public_Dialog_Title, JSONMsg.message, 'error');
        //                                        return false;
        //                                    } else {

        //                                    }
        //                                },
        //                                complete: function (XMLHttpRequest, textStatus) {

        //                                },
        //                                error: function () {

        //                                }
        //                            });

        //                            if (!bExist) {
        //                                EditDlgForm = EditDlg.find('form');
        //                                EditDlgForm.form('submit', {
        //                                    url: EditDlgForm.url,
        //                                    onSubmit: function () {
        //                                        //return $(this).form('validate');
        //                                    },
        //                                    success: function () {
        //                                        //reWriteMessagerAlert('提示', '成功', "info");
        //                                        reWriteMessagerAlert(Public_Dialog_Title, Public_Message_OK, "info");
        //                                        EditDlg.dialog('close');
        //                                        _$_datagrid.datagrid("reload");
        //                                        _$_datagrid.datagrid("unselectAll");
        //                                    }
        //                                });
        //                            }
        //                        }
        //                    }, {
        //                        text: Public_Dialog_ButtonOfClose, // '关 闭',
        //                        iconCls: 'icon-cancel',
        //                        handler: function () {
        //                            EditDlg.dialog('close');
        //                        }
        //                    }],
        //                    title: User_Dialog_UpdateUser_Title, // '修改系统用户信息',
        //                    href: UpdateURL + id + "?d=" + Date(),
        //                    modal: true,
        //                    resizable: true,
        //                    cache: false,
        //                    left: 50,
        //                    top: 30,
        //                    width: 800,
        //                    height: 320,
        //                    closed: true
        //                });
        //                _$_datagrid.datagrid("unselectAll");
        //            }
        //        }

        //        $('#dlg_Update').dialog("open");
    }

    function Delete() {
        reWriteMessagerConfirm("提示", "您确定需要删除所选的系统用户信息吗？",
        //reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].auID);
                            }
                            if (selects.length == 0) {
                                $.messager.alert("提示", "<center>请选择需要删除的数据</center>", "error");
                                //$.messager.alert(Public_Dialog_Title, User_JS_ErrorMessage6, "error");
                                return false;
                            }

                            $.ajax({
                                url: DeleteURL + '?ids=' + ids.join(','),
                                type: "POST",
                                cache: false,
                                async: false,
                                success: function (msg) {
                                    var JSONMsg = eval("(" + msg + ")");
                                    if (JSONMsg.result.toLowerCase() == 'ok') {
                                        // $.messager.alert('操作提示', JSONMsg.message, 'info');
                                        $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                                        _$_datagrid.datagrid("reload");
                                        _$_datagrid.datagrid("unselectAll");
                                    } else {
                                        // $.messager.alert('操作提示', JSONMsg.message, 'error');
                                        $.messager.alert(Public_Dialog_Title, JSONMsg.message, 'error');
                                        return false;
                                    }
                                }
                            });

                        } else {

                        }
                    }
                );
    }

    function SeleAll() {
        var rows = _$_datagrid.datagrid("getRows");
        for (var i = 0; i < rows.length; i++) {
            var m = _$_datagrid.datagrid("getRowIndex", rows[i]);
            _$_datagrid.datagrid("selectRow", m)
        }
    }

    function InverseSele() {
        var rows = _$_datagrid.datagrid("getRows");
        var selects = _$_datagrid.datagrid("getSelections");
        for (var i = 0; i < rows.length; i++) {
            var bSele = false;
            var m = _$_datagrid.datagrid("getRowIndex", rows[i]);
            for (var j = 0; j < selects.length; j++) {
                var n = _$_datagrid.datagrid("getRowIndex", selects[j]);
                if (m == n) {
                    bSele = true;
                }
            }
            if (bSele) {
                _$_datagrid.datagrid("unselectRow", m)
            } else {
                _$_datagrid.datagrid("selectRow", m)
            }
        }
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
        var fields = _$_datagrid.datagrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.datagrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "cb":
                    break;
                case "urNum":
                    break;

                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "userID") {

                } else {
                    if (item.iconCls == 'icon-ok') {
                        _$_datagrid.datagrid('hideColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-empty'
                        });
                    } else {
                        _$_datagrid.datagrid('showColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-ok'
                        });
                    }
                }
            }
        });
    }
});
