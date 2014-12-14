$(function () {
    var _$_datagrid = $("#DG_UserMaintain");
    var QueryURL = "/" + pub_WhichLang + "/User/GetData";
    var CreateURL = "/" + pub_WhichLang + "/User/Create";
    var UpdateURL = "/" + pub_WhichLang + "/User/Edit/";
    var DeleteURL = "/" + pub_WhichLang + "/User/Delete";
    var EnableUserURL = "/" + pub_WhichLang + "/User/EnableUser";
    var DisableUserURL = "/" + pub_WhichLang + "/User/DisableUser";
    var TestExist_UserName = "/" + pub_WhichLang + "/User/ExistUserNum?strUserNum=";
    var TestExist_UserName_Update = "/" + pub_WhichLang + "/User/ExistUserNum_Update?strUserNum=";
    var CreateDlg = null;
    var CreateDlgForm = null;
    var EditDlg = null;
    var EditDlgForm = null;

    var PrintURL = "";
    var PrintBarcodeURL = "/" + pub_WhichLang + "/User/PrintBarcode?urIDs=";

    //    _$_datagrid.datagrid({
    //        iconCls: 'icon-save',
    //        nowrap: true,
    //        autoRowHeight: false,
    //        autoRowWidth: false,
    //        striped: true,
    //        collapsible: true,
    //        url: QueryURL,
    //        sortName: 'urNum',
    //        sortOrder: 'asc',
    //        remoteSort: true,
    //        border: false,
    //        idField: 'urID',
    //        columns: [[
    //                    { field: 'cb', width: 120, checkbox: true },
    //					{ field: 'urNum', title: '用户号', width: 120, sortable: true,
    //					    sorter: function (a, b) {
    //					        return (a > b ? 1 : -1);
    //					    }
    //					},
    //					{ field: 'urName', title: '用户姓名', width: 100, sortable: true,
    //					    sorter: function (a, b) {
    //					        return (a > b ? 1 : -1);
    //					    }
    //					},
    //                    { field: 'urSexDesc', title: '性别', width: 50, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                     { field: 'urAge', title: '年龄', width: 40, sortable: true,
    //                         sorter: function (a, b) {
    //                             return (a > b ? 1 : -1);
    //                         }
    //                     },
    //                    { field: 'urStaffNum', title: '员工工号', width: 150, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'urDept', title: '员工部门', width: 220, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'urDuty', title: '员工职位', width: 150, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'urUnitCode', title: '口岸代码', width: 100, sortable: true,
    //                        sorter: function (a, b) {
    //                            return (a > b ? 1 : -1);
    //                        }
    //                    },
    //                    { field: 'ManualHandle', title: '操作', width: 80, sortable: true, align: "center",
    //                        formatter: function (value, rowData, rowIndex) {
    //                            //return "<input type='button' class='handle_ReInStore' value='入库' wbfID='" + rowData.WbfID + "'/>" + "<input type='button' class='handle_ReOutStore' value='出库' wbfID='" + rowData.WbfID + "'/>";
    //                            return "<a href='#' class='ManualHandle_cls' urID='" + rowData.urID + "'>打印条码</a>";
    //                        }
    //                    }
    //				]],
    //        pagination: true,
    //        pageSize: 15,
    //        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
    //        toolbar: [{
    //            id: 'btnQuery',
    //            text: '查询',
    //            iconCls: 'icon-search',
    //            handler: function () {
    //                _$_datagrid.datagrid("reload");
    //                _$_datagrid.datagrid("unselectAll");
    //            }
    //        }, '-', {
    //            id: 'btnAdd',
    //            text: '添加',
    //            iconCls: 'icon-add',
    //            handler: function () {
    //                Add();
    //            }
    //        }, '-', {
    //            id: 'btnUpdate',
    //            text: '修改',
    //            iconCls: 'icon-edit',
    //            handler: function () {
    //                Update();
    //            }
    //        }, '-', {
    //            id: 'btnDelete',
    //            text: '删除',
    //            disabled: false,
    //            iconCls: 'icon-remove',
    //            handler: function () {
    //                Delete();
    //            }
    //        }, '-', {
    //            id: 'btnSeleAll',
    //            text: '全选',
    //            disabled: false,
    //            iconCls: 'icon-seleall',
    //            handler: function () {
    //                SeleAll();
    //            }
    //        }, '-', {
    //            id: 'btnInverseSele',
    //            text: '反选',
    //            disabled: false,
    //            iconCls: 'icon-inversesele',
    //            handler: function () {
    //                InverseSele();
    //            }
    //        }, '-', {
    //            id: 'btnPrint',
    //            text: '打印',
    //            disabled: false,
    //            iconCls: 'icon-print',
    //            handler: function () {
    //                Print();
    //            }
    //        }, '-', {
    //            id: 'btnPrintBarcode',
    //            text: '打印条码',
    //            disabled: false,
    //            iconCls: 'icon-barcode',
    //            handler: function () {
    //                PrintBarcode();
    //            }
    //        }, '-', {
    //            id: 'btnExcel',
    //            text: '导出',
    //            disabled: false,
    //            iconCls: 'icon-excel',
    //            handler: function () {
    //                Excel();
    //            }
    //        }],
    //        onRowContextMenu: function (e, rowIndex, rowData) {
    //            e.preventDefault();
    //            _$_datagrid.datagrid("unselectAll");
    //            _$_datagrid.datagrid("selectRow", rowIndex);

    //            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
    //            $('<div  id="mnuQuery" iconCls="icon-search"/>').html("查询").appendTo(cmenu);
    //            $('<div  id="mnuAdd" iconCls="icon-add"/>').html("添加").appendTo(cmenu);
    //            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html("修改").appendTo(cmenu);
    //            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html("删除").appendTo(cmenu);
    //            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html("全选").appendTo(cmenu);
    //            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html("反选").appendTo(cmenu);
    //            $('<div  id="mnuPrint" iconCls="icon-print"/>').html("打印").appendTo(cmenu);
    //            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html("导出").appendTo(cmenu);
    //            cmenu.menu({
    //                onClick: function (item) {
    //                    cmenu.remove();
    //                    switch (item.id.toLowerCase()) {
    //                        case "mnuadd":
    //                            Add();
    //                            break;
    //                        case "mnuupdate":
    //                            Update();
    //                            break;
    //                        case "mnudelete":
    //                            Delete();
    //                            break;
    //                        case "mnuseleall":
    //                            SeleAll();
    //                            break;
    //                        case "mnuinversesele":
    //                            InverseSele();
    //                            break;
    //                        case "mnuprint":
    //                            Print();
    //                            break;
    //                        case "mnuexcel":
    //                            Excel();
    //                            break;
    //                    }
    //                }
    //            });

    //            $('#cmenu').menu('show', {
    //                left: e.pageX,
    //                top: e.pageY
    //            });
    //        },
    //        onHeaderContextMenu: function (e, field) {
    //            e.preventDefault();
    //            if (!$('#tmenu').length) {
    //                createColumnMenu();
    //            }
    //            $('#tmenu').menu('show', {
    //                left: e.pageX,
    //                top: e.pageY
    //            });
    //        },
    //        onSortColumn: function (sort, order) {
    //            // _$_datagrid.datagrid("reload");
    //        },
    //        onDblClickRow: function (rowIndex, rowData) {
    //            _$_datagrid.datagrid("unselectAll");
    //            _$_datagrid.datagrid("selectRow", rowIndex);
    //            Update(rowData.urID);
    //        },
    //        onLoadSuccess: function (data) {
    //            var allManualHandleBtn = $(".ManualHandle_cls");
    //            $.each(allManualHandleBtn, function (i, item) {
    //                var urID = $(item).attr("urID");
    //                $(item).show();
    //                $(item).click(function () {
    //                    PrintBarcode(urID);
    //                });
    //            });
    //        }
    //    });

    _$_datagrid.datagrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'urNum',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'urID',
        columns: [[
                    { field: 'cb', width: 120, checkbox: true },
					{ field: 'urNum', title: User_JS_DG_Field1, width: 120, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'urName', title: User_JS_DG_Field2, width: 90, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'urSexDesc', title: User_JS_DG_Field3, width: 50, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                     { field: 'urAge', title: User_JS_DG_Field4, width: 40, sortable: true,
                         sorter: function (a, b) {
                             return (a > b ? 1 : -1);
                         }
                     },
                    { field: 'urStaffNum', title: User_JS_DG_Field5, width: 150, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'urDept', title: User_JS_DG_Field6, width: 220, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'urDuty', title: User_JS_DG_Field7, width: 150, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'urUnitCode', title: User_JS_DG_Field8, width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'urDelflag_Des', title: "状态", width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        },
                        formatter: function (value, rowData, rowIndex) {
                            var strRet = "";
                            switch (rowData.urDelflag) {
                                case "0":
                                    strRet = "<a href='#'  title='点击可禁用' class='ToggleEnableUser_cls' urID='" + rowData.urID + "' DelFlag=1>" + "<span style='color:blue'>" + rowData.urDelflag_Des + "</span>" + "</a>";
                                    break;
                                case "1":
                                    strRet = "<a href='#' title='点击可启用' class='ToggleEnableUser_cls' urID='" + rowData.urID + "' DelFlag=0>" + "<span style='color:red'>" + rowData.urDelflag_Des + "</span>" + "</a>";
                                    break;
                                default:
                                    break;
                            }
                            return strRet;
                        }
                    },
                    { field: 'ManualHandle', title: User_JS_DG_Field9, width: 80, sortable: true, align: "center",
                        formatter: function (value, rowData, rowIndex) {
                            //return "<input type='button' class='handle_ReInStore' value='入库' wbfID='" + rowData.WbfID + "'/>" + "<input type='button' class='handle_ReOutStore' value='出库' wbfID='" + rowData.WbfID + "'/>";
                            return "<a href='#' class='ManualHandle_cls' urID='" + rowData.urID + "'>" + User_JS_DG_Tool8 + "</a>";
                        }
                    }
				]],
        pagination: true,
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        toolbar: [{
            id: 'btnQuery',
            text: User_JS_DG_Tool1,
            iconCls: 'icon-search',
            handler: function () {
                _$_datagrid.datagrid("reload");
                _$_datagrid.datagrid("unselectAll");
            }
        }, '-', {
            id: 'btnAdd',
            text: User_JS_DG_Tool2,
            iconCls: 'icon-add',
            handler: function () {
                Add();
            }
        }, '-', {
            id: 'btnUpdate',
            text: User_JS_DG_Tool3,
            iconCls: 'icon-edit',
            handler: function () {
                Update();
            }
        }, '-'
//        , {
//            id: 'btnDelete',
//            text: User_JS_DG_Tool4,
//            disabled: false,
//            iconCls: 'icon-remove',
//            handler: function () {
//                Delete();
//            }
//        }
        , '-', {
            id: 'btnEnableUser',
            text: "启用",
            disabled: false,
            iconCls: 'icon-enable',
            handler: function () {
                EnableUser();
            }
        }, '-', {
            id: 'btnDisbaleUser',
            text: "禁用",
            disabled: false,
            iconCls: 'icon-disable',
            handler: function () {
                DisableUser();
            }
        }, '-', {
            id: 'btnSeleAll',
            text: User_JS_DG_Tool5,
            disabled: false,
            iconCls: 'icon-seleall',
            handler: function () {
                SeleAll();
            }
        }, '-', {
            id: 'btnInverseSele',
            text: User_JS_DG_Tool6,
            disabled: false,
            iconCls: 'icon-inversesele',
            handler: function () {
                InverseSele();
            }
        }, '-', {
            id: 'btnPrint',
            text: User_JS_DG_Tool7,
            disabled: false,
            iconCls: 'icon-print',
            handler: function () {
                Print();
            }
        }, '-', {
            id: 'btnPrintBarcode',
            text: User_JS_DG_Tool8,
            disabled: false,
            iconCls: 'icon-barcode',
            handler: function () {
                PrintBarcode();
            }
        }, '-', {
            id: 'btnExcel',
            text: User_JS_DG_Tool9,
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
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html(User_JS_DG_Tool1).appendTo(cmenu);
            $('<div  id="mnuAdd" iconCls="icon-add"/>').html(User_JS_DG_Tool2).appendTo(cmenu);
            $('<div  id="mnuUpdate" iconCls="icon-edit"/>').html(User_JS_DG_Tool3).appendTo(cmenu);
            //$('<div  id="mnuDelete" iconCls="icon-remove"/>').html(User_JS_DG_Tool4).appendTo(cmenu);
            $('<div  id="mnuEnableUser" iconCls="icon-enable"/>').html("启用").appendTo(cmenu);
            $('<div  id="mnuDisableUser" iconCls="icon-disable"/>').html("禁用").appendTo(cmenu);
            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html(User_JS_DG_Tool5).appendTo(cmenu);
            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html(User_JS_DG_Tool6).appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html(User_JS_DG_Tool7).appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html(User_JS_DG_Tool9).appendTo(cmenu);
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
                        case "mnuenableuser":
                            EnableUser();
                            break;
                        case "mnudisableuser":
                            DisableUser();
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
            Update(rowData.urID);
        },
        onLoadSuccess: function (data) {
            var allManualHandleBtn = $(".ManualHandle_cls");
            var allToggleEnableUserBtn = $(".ToggleEnableUser_cls");
            $.each(allManualHandleBtn, function (i, item) {
                var urID = $(item).attr("urID");
                $(item).show();
                $(item).click(function () {
                    PrintBarcode(urID);
                });
            });

            $.each(allToggleEnableUserBtn, function (i, item) {
                var urID = $(item).attr("urID");
                var DelFlag = $(item).attr("DelFlag");
                $(item).show();
                $(item).click(function () {
                    ToggleEnableUser(urID,DelFlag);
                });
            });
        }
    });

    function Print() {
        PrintURL = "/" + pub_WhichLang + "/User/Print?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000";
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
            div_PrintDlg.show();
            var PrintDlg = null;
            div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL);
            PrintDlg = div_PrintDlg.window({
                title: Public_Dialog_ButtonOfPrint, //'打印',
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
            //reWriteMessagerAlert("提示", "没有数据，不可打印", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForPrint, "error");
            return false;
        }
    }

    function PrintBarcode(ids_Temp) {
        if (ids_Temp) {
            if (_$_datagrid.datagrid("getData").rows.length > 0) {
                PrintBarcodeURL = "/" + pub_WhichLang + "/User/PrintBarcode?urIDs=" + encodeURI(ids_Temp);
                var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
                div_PrintDlg.show();
                var PrintDlg = null;
                div_PrintDlg.find("#frmPrintURL").attr("src", PrintBarcodeURL);
                PrintDlg = div_PrintDlg.window({
                    title: Public_Dialog_ButtonOfPrint, //'打印',
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
                //                reWriteMessagerAlert("提示", "没有数据，不可打印", "error");
                reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForPrint, "error");
                return false;
            }
        } else {
            if (_$_datagrid.datagrid("getData").rows.length > 0) {
                var selects = _$_datagrid.datagrid("getSelections");
                var ids = [];
                for (var i = 0; i < selects.length; i++) {
                    ids.push(selects[i].urID);
                }
                if (selects.length == 0) {
                    //$.messager.alert("提示", "<center>请选择需要打印的数据</center>", "error");
                    $.messager.alert(Public_Dialog_Title, "<center>" + Public_Dialog_NoDataSelectForPrint + "</center>", "error");
                    return false;
                }
                PrintBarcodeURL = "/" + pub_WhichLang + "/User/PrintBarcode?urIDs=" + encodeURI(ids.join(','));
                var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
                div_PrintDlg.show();
                var PrintDlg = null;
                div_PrintDlg.find("#frmPrintURL").attr("src", PrintBarcodeURL);
                PrintDlg = div_PrintDlg.window({
                    //title: '打印',
                    title: Public_Dialog_ButtonOfPrint,
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
                //reWriteMessagerAlert(Public_Dialog_Title, "没有数据，不可打印", "error");
                reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForPrint, "error");
                return false;
            }
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

        PrintURL = "/" + pub_WhichLang + "/User/Excel?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            //reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function Add() {
        _$_datagrid.datagrid("unselectAll");
        CreateDlg = $('#dlg_Create').dialog({
            buttons: [{
                text: Public_Dialog_ButtonOfSave, // '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var txturNum = CreateDlg.find('#txturNum').val();
                    var txturPSW = CreateDlg.find('#txturPSW').val();
                    var txtReurPSW = CreateDlg.find('#txtReurPSW').val();
                    var txturUnitCode = CreateDlg.find('#txturUnitCode').val();

                    if (txturNum == "" || txturPSW == "" || txtReurPSW == "" || txturUnitCode == "-99") {
                        //reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(用户号、密码、确认密码、口岸)', "error");
                        reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage1, "error");
                        return false;
                    }

                    if (txturPSW != txtReurPSW) {
                        reWriteMessagerAlert('操作提示', '密码与确认密码不一致', "error");
                        //reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage2, "error");
                        return false;
                    }

                    if (txturNum.length > 10) {
                        //reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
                        reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage2, "error");
                        $("#txturNum").focus();
                        return false;
                    }

                    if (!(/[\W_]/.test(txturNum) || !txturNum ? false : true)) {
                        //reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
                        reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage2, "error");
                        $("#txturNum").focus();
                        return false;
                    }

                    $("#txturMemo").val($("#areaurMemo").text());

                    //验证此用户名是否已使用
                    var bExist = false;
                    $.ajax({
                        type: "GET",
                        url: TestExist_UserName + encodeURI(txturNum),
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
                        CreateDlgForm = CreateDlg.find('form');
                        CreateDlgForm.form('submit', {
                            url: CreateDlgForm.url,
                            onSubmit: function () {
                                //return $(this).form('validate');
                                //console.info($(this).form('validate'));
                            },
                            success: function () {
                                // reWriteMessagerAlert('提示', '成功', "info");
                                reWriteMessagerAlert(Public_Dialog_Title, Public_Message_OK, "info");
                                CreateDlg.dialog('close');
                                _$_datagrid.datagrid("reload");
                                _$_datagrid.datagrid("unselectAll");
                            }
                        });
                    }

                }
            }, {
                text: Public_Dialog_ButtonOfClose, //'关 闭',
                iconCls: 'icon-cancel',
                handler: function () {
                    CreateDlg.dialog('close');
                }
            }],
            title: User_Dialog_AddUser_Title, // '添加系统用户信息',
            href: CreateURL + "?d=" + Date(),
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 800,
            height: 320
        });

        $('#dlg_Create').dialog("open");
    }

    function Update(id) {
        if (id) {
            EditDlg = $('#dlg_Update').dialog({
                buttons: [{
                    text: Public_Dialog_ButtonOfSave, //'保 存',
                    iconCls: 'icon-ok',
                    handler: function () {
                        var txturNum = EditDlg.find('#txturNum').val();
                        var txturPSW = EditDlg.find('#txturPSW').val();
                        var txtReurPSW = EditDlg.find('#txtReurPSW').val();
                        var txturUnitCode = EditDlg.find('#txturUnitCode').val();

                        if (txturNum == "" || txturPSW == "" || txtReurPSW == "" || txturUnitCode == "-99") {
                            //reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(用户号、密码、确认密码、口岸)', "error");
                            reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage1, "error");
                            return false;
                        }

                        if (txturPSW != txtReurPSW) {
                            reWriteMessagerAlert('操作提示', '密码与确认密码不一致', "error");
                            //reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage2, "error");
                            return false;
                        }

                        if (txturNum.length > 10) {
                            // reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
                            reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
                            $("#txturNum").focus();
                            return false;
                        }

                        if (!(/[\W_]/.test(txturNum) || !txturNum ? false : true)) {
                            // reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
                            reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
                            $("#txturNum").focus();
                            return false;
                        }

                        $("#txturMemo").val($("#areaurMemo").text());

                        //验证此用户名是否已使用
                        var bExist = false;
                        $.ajax({
                            type: "GET",
                            url: TestExist_UserName_Update + encodeURI(txturNum) + "&id=" + id,
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
                            EditDlgForm = EditDlg.find('form');
                            EditDlgForm.form('submit', {
                                url: EditDlgForm.url,
                                onSubmit: function () {
                                    //return $(this).form('validate');
                                },
                                success: function () {
                                    // reWriteMessagerAlert('提示', '成功', "info");
                                    reWriteMessagerAlert(Public_Dialog_Title, Public_Message_OK, "info");
                                    EditDlg.dialog('close');
                                    _$_datagrid.datagrid("reload");
                                    _$_datagrid.datagrid("unselectAll");
                                }
                            });
                        }


                    }
                }, {
                    text: Public_Dialog_ButtonOfClose, // '关 闭',
                    iconCls: 'icon-cancel',
                    handler: function () {
                        EditDlg.dialog('close');
                    }
                }],
                title: User_Dialog_UpdateUser_Title, // '修改系统用户信息',
                href: UpdateURL + id + "?d=" + Date(),
                modal: true,
                resizable: true,
                cache: false,
                left: 50,
                top: 30,
                width: 800,
                height: 320,
                closed: true
            });
            _$_datagrid.datagrid("unselectAll");
        } else {
            var selects = _$_datagrid.datagrid("getSelections");
            if (selects.length != 1) {
                //reWriteMessagerAlert("提示", "<center>请选择数据进行修改(<font style='color:red'>只可选择一行</font>)</center>", "error");
                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage4, "error");
                return false;
            } else {
                id = selects[0].urID;
                EditDlg = $('#dlg_Update').dialog({
                    buttons: [{
                        text: Public_Dialog_ButtonOfSave, // '保 存',
                        iconCls: 'icon-ok',
                        handler: function () {
                            var txturNum = EditDlg.find('#txturNum').val();
                            var txturPSW = EditDlg.find('#txturPSW').val();
                            var txtReurPSW = EditDlg.find('#txtReurPSW').val();
                            var txturUnitCode = EditDlg.find('#txturUnitCode').val();

                            if (txturNum == "" || txturPSW == "" || txtReurPSW == "" || txturUnitCode == "-99") {
                                //reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(用户号、密码、确认密码、口岸)', "error");
                                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage1, "error");
                                return false;
                            }

                            if (txturPSW != txtReurPSW) {
                                reWriteMessagerAlert('操作提示', '密码与确认密码不一致', "error");
                                //reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage2, "error");
                                return false;
                            }

                            if (txturNum.length > 10) {
                                // reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
                                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
                                $("#txturNum").focus();
                                return false;
                            }

                            if (!(/[\W_]/.test(txturNum) || !txturNum ? false : true)) {
                                //reWriteMessagerAlert('操作提示', '用户名最大长度为10,且为字母或数字或其组合', "error");
                                reWriteMessagerAlert(Public_Dialog_Title, User_JS_ErrorMessage3, "error");
                                $("#txturNum").focus();
                                return false;
                            }

                            $("#txturMemo").val($("#areaurMemo").text());

                            //验证此用户名是否已使用
                            var bExist = false;
                            $.ajax({
                                type: "GET",
                                url: TestExist_UserName_Update + encodeURI(txturNum) + "&id=" + id,
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
                                EditDlgForm = EditDlg.find('form');
                                EditDlgForm.form('submit', {
                                    url: EditDlgForm.url,
                                    onSubmit: function () {
                                        //return $(this).form('validate');
                                    },
                                    success: function () {
                                        //reWriteMessagerAlert('提示', '成功', "info");
                                        reWriteMessagerAlert(Public_Dialog_Title, Public_Message_OK, "info");
                                        EditDlg.dialog('close');
                                        _$_datagrid.datagrid("reload");
                                        _$_datagrid.datagrid("unselectAll");
                                    }
                                });
                            }
                        }
                    }, {
                        text: Public_Dialog_ButtonOfClose, // '关 闭',
                        iconCls: 'icon-cancel',
                        handler: function () {
                            EditDlg.dialog('close');
                        }
                    }],
                    title: User_Dialog_UpdateUser_Title, // '修改系统用户信息',
                    href: UpdateURL + id + "?d=" + Date(),
                    modal: true,
                    resizable: true,
                    cache: false,
                    left: 50,
                    top: 30,
                    width: 800,
                    height: 320,
                    closed: true
                });
                _$_datagrid.datagrid("unselectAll");
            }
        }

        $('#dlg_Update').dialog("open");
    }

    function Delete() {
        //reWriteMessagerConfirm("提示", "您确定需要删除所选的系统用户信息吗？",
        reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].urID);
                            }
                            if (selects.length == 0) {
                                // $.messager.alert("提示", "<center>请选择需要删除的数据</center>", "error");
                                $.messager.alert(Public_Dialog_Title, User_JS_ErrorMessage6, "error");
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

    function ToggleEnableUser(urId,delFlag) {
        var PostURL = "";
        switch (delFlag) {
            case "0":
                PostURL = EnableUserURL + '?ids=' + urId;
                break;
            case "1":
                PostURL = DisableUserURL + '?ids=' + urId;
                break;
            default:
                break;
        }
        if (PostURL != "") {
            $.ajax({
                url: PostURL,
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
        }
    }

    function EnableUser() {
        reWriteMessagerConfirm("提示", "您确定需要启用所选的用户吗？",
        //reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].urID);
                            }
                            if (selects.length == 0) {
                                $.messager.alert("提示", "<center>请选择需要启用的用户</center>", "error");
                                //$.messager.alert(Public_Dialog_Title, User_JS_ErrorMessage6, "error");
                                return false;
                            }

                            $.ajax({
                                url: EnableUserURL + '?ids=' + ids.join(','),
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

    function DisableUser() {
        reWriteMessagerConfirm("提示", "您确定需要禁用所选的用户吗？",
        //reWriteMessagerConfirm(Public_Dialog_Title, User_JS_ErrorMessage5,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].urID);
                            }
                            if (selects.length == 0) {
                                $.messager.alert("提示", "<center>请选择需要禁用的用户</center>", "error");
                                //$.messager.alert(Public_Dialog_Title, User_JS_ErrorMessage6, "error");
                                return false;
                            }

                            $.ajax({
                                url: DisableUserURL + '?ids=' + ids.join(','),
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
