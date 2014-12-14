$(function () {
    var _$_datagrid = $("#DG_Log_HU");
    var QueryURL = "/" + pub_WhichLang + "/OperationLog_HU/GetData";
    var DeleteURL = "/" + pub_WhichLang + "/OperationLog_HU/Delete";

    var PrintURL = "";
    QueryURL = "/" + pub_WhichLang + "/OperationLog_HU/GetData?dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&urName=" + encodeURI($("#txturName").val());

    $("#btnQuery").click(function () {
        QueryURL = "/" + pub_WhichLang + "/OperationLog_HU/GetData?dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&urName=" + encodeURI($("#txturName").val());
        window.setTimeout(function () {
            $.extend(_$_datagrid.datagrid("options"), {
                url: QueryURL
            });
            _$_datagrid.datagrid("reload");
        }, 10); //延迟100毫秒执行，时间可以更短
    });

//    _$_datagrid.datagrid({
//        iconCls: 'icon-save',
//        nowrap: true,
//        autoRowHeight: false,
//        autoRowWidth: false,
//        striped: true,
//        collapsible: true,
//        url: QueryURL,
//        sortName: 'huopDateTime',
//        sortOrder: 'desc',
//        remoteSort: true,
//        border: false,
//        idField: 'huopID',
//        columns: [[
//                    { field: 'cb', width: 120, checkbox: true },
//					{ field: 'urNum', title: '用户号', width: 100, sortable: true,
//					    sorter: function (a, b) {
//					        return (a > b ? 1 : -1);
//					    }
//					},
//					{ field: 'urName', title: '用户姓名', width: 100, sortable: true,
//					    sorter: function (a, b) {
//					        return (a > b ? 1 : -1);
//					    }
//					},
//                    { field: 'huopJobNameIdLv1', title: '一级工作流程', width: 80, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                     { field: 'huopJobNameIdLv2', title: '二级工作流程', width: 80, sortable: true,
//                         sorter: function (a, b) {
//                             return (a > b ? 1 : -1);
//                         }
//                     },
//                    { field: 'urUnitCode', title: '口岸代码', width: 80, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'huopDateTime', title: '操作时间', width: 130, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    },
//                    { field: 'huopContent', title: '操作内容', width: 350, sortable: true,
//                        sorter: function (a, b) {
//                            return (a > b ? 1 : -1);
//                        }
//                    }
//				]],
//        pagination: true,
//        pageSize: 15,
//        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
//        toolbar: "#toolBar",
//        onRowContextMenu: function (e, rowIndex, rowData) {
//            e.preventDefault();
//            _$_datagrid.datagrid("unselectAll");
//            _$_datagrid.datagrid("selectRow", rowIndex);

//            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
//            $('<div  id="mnuQuery" iconCls="icon-search"/>').html("查询").appendTo(cmenu);
//            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html("删除").appendTo(cmenu);
//            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html("全选").appendTo(cmenu);
//            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html("反选").appendTo(cmenu);
//            $('<div  id="mnuPrint" iconCls="icon-print"/>').html("打印").appendTo(cmenu);
//            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html("导出").appendTo(cmenu);
//            cmenu.menu({
//                onClick: function (item) {
//                    cmenu.remove();
//                    switch (item.id.toLowerCase()) {
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
        sortName: 'huopDateTime',
        sortOrder: 'desc',
        remoteSort: true,
        border: false,
        idField: 'huopID',
        columns: [[
                    { field: 'cb', width: 120, checkbox: true },
					{ field: 'urNum', title: OperationLog_HU_JS_Field1, width: 100, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
					{ field: 'urName', title: OperationLog_HU_JS_Field2, width: 100, sortable: true,
					    sorter: function (a, b) {
					        return (a > b ? 1 : -1);
					    }
					},
                    { field: 'vchar_WorkProcess_L1_Text', title: OperationLog_HU_JS_Field3, width: 150, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                     { field: 'vchar_WorkProcess_L2_Text', title: OperationLog_HU_JS_Field4, width: 150, sortable: true,
                         sorter: function (a, b) {
                             return (a > b ? 1 : -1);
                         }
                     },
                    { field: 'urUnitCode', title: OperationLog_HU_JS_Field5, width: 80, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'huopDateTime', title: OperationLog_HU_JS_Field6, width: 130, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'CCTV_workstation_id', title: "工作岗位", width: 130, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    },
                    { field: 'huopContent', title: OperationLog_HU_JS_Field7, width: 310, sortable: true,
                        sorter: function (a, b) {
                            return (a > b ? 1 : -1);
                        }
                    }
				]],
        pagination: true,
        pageSize: 15,
        pageList: [15, 20, 25, 30, 35, 40, 45, 50],
        toolbar: "#toolBar",
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            _$_datagrid.datagrid("unselectAll");
            _$_datagrid.datagrid("selectRow", rowIndex);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuQuery" iconCls="icon-search"/>').html(OperationLog_HU_Html_btnQuery).appendTo(cmenu);
            $('<div  id="mnuDelete" iconCls="icon-remove"/>').html(OperationLog_HU_Html_btnDele).appendTo(cmenu);
            $('<div  id="mnuSeleAll" iconCls="icon-seleall"/>').html(OperationLog_HU_Html_btnSeleAll).appendTo(cmenu);
            $('<div  id="mnuInverseSele" iconCls="icon-inversesele"/>').html(OperationLog_HU_Html_btnInverseSele).appendTo(cmenu);
            $('<div  id="mnuPrint" iconCls="icon-print"/>').html(OperationLog_HU_Html_btnPrint).appendTo(cmenu);
            $('<div  id="mnuExcel" iconCls="icon-excel"/>').html(OperationLog_HU_Html_btnExcel).appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
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
        }
    });

    $("#btnDele").click(function () {
        Delete();
    });

    $("#btnSeleAll").click(function () {
        SeleAll();
    });

    $("#btnInverseSele").click(function () {
        InverseSele();
    });

    $("#btnPrint").click(function () {
        Print();
    });

    $("#btnExcel").click(function () {
        Excel();
    });

    function Print() {
        PrintURL = "/" + pub_WhichLang + "/OperationLog_HU/Print?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&urName=" + encodeURI($("#txturName").val()) + "&page=1&rows=10000000";
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            var div_PrintDlg = self.parent.$("#dlg_GlobalPrint");
            div_PrintDlg.show();
            var PrintDlg = null;
            div_PrintDlg.find("#frmPrintURL").attr("src", PrintURL);
            PrintDlg = div_PrintDlg.window({
                title: Public_Dialog_ButtonOfPrint,//'打印',
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

        PrintURL = "/" + pub_WhichLang + "/OperationLog_HU/Excel?order=" + _$_datagrid.datagrid("options").sortOrder + "&sort=" + _$_datagrid.datagrid("options").sortName + "&dBegin=" + encodeURI($("#txtBeginD").val()) + "&dEnd=" + encodeURI($("#txtEndD").val()) + "&urName=" + encodeURI($("#txturName").val()) + "&page=1&rows=10000000&browserType=" + browserType;
        if (_$_datagrid.datagrid("getData").rows.length > 0) {
            window.open(PrintURL);

        } else {
            //reWriteMessagerAlert("提示", "没有数据，不可导出", "error");
            reWriteMessagerAlert(Public_Dialog_Title, Public_Dialog_NoDataForExcel, "error");
            return false;
        }
    }

    function Delete() {
       // reWriteMessagerConfirm("提示", "您确定需要删除所选的操作日志信息吗？",
        reWriteMessagerConfirm(Public_Dialog_Title, OperationLog_HU_JS_ErrorMessage1,
                    function (ok) {
                        if (ok) {
                            var selects = _$_datagrid.datagrid("getSelections");
                            var ids = [];
                            for (var i = 0; i < selects.length; i++) {
                                ids.push(selects[i].huopID);
                            }
                            if (selects.length == 0) {
                                //$.messager.alert("提示", "<center>请选择需要删除的数据</center>", "error");
                                $.messager.alert(Public_Dialog_Title, OperationLog_HU_JS_ErrorMessage2, "error");
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
                if ($(item.text).attr("id") == "urNum") {

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
