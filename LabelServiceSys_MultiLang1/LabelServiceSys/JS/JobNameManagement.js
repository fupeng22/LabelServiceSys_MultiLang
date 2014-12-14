$(function () {
    var _$_datagrid = $("#DG_JobName");
    var QueryURL = "/" + pub_WhichLang + "/JobNameManagement/GetData";
    var DeleteJobName1URL = "/" + pub_WhichLang + "/JobNameManagement/DeleteJobName1?ids=";
    var DeleteJobName2URL = "/" + pub_WhichLang + "/JobNameManagement/DeleteJobName2?ids=";
    var AddJobName1URL = "/" + pub_WhichLang + "/JobNameManagement/AddJobName1?GateWay=";
    var UpdateJobName1URL = "/" + pub_WhichLang + "/JobNameManagement/UpdateJobName1?id=";
    var AddJobName2URL = "/" + pub_WhichLang + "/JobNameManagement/AddJobName2?JobName1ID=";
    var UpdateJobName2URL = "/" + pub_WhichLang + "/JobNameManagement/UpdateJobName2?Id=";

    var QueryJobName1URL = "/" + pub_WhichLang + "/Default/LoadJobNameIdLv1";
    var QueryGateWayURL = "/" + pub_WhichLang + "/Default/LoadUnitCode";

    var EnableOrDisableJobName1URL = "/" + pub_WhichLang + "/JobNameManagement/EnableOrDisableJobName1?ids=";
    var EnableOrDisableJobName2URL = "/" + pub_WhichLang + "/JobNameManagement/EnableOrDisableJobName2?ids=";

    var TestExist_JobName1Code_Add = "/" + pub_WhichLang + "/JobNameManagement/TestExist_JobName1Code_Add?JobName1Code=";
    var TestExist_JobName1Code_Update = "/" + pub_WhichLang + "/JobNameManagement/TestExist_JobName1Code_Update?JobName1Code=";

    var TestExist_JobName2Code_Add = "/" + pub_WhichLang + "/JobNameManagement/TestExist_JobName2Code_Add?JobName2Code=";
    var TestExist_JobName2Code_Update = "/" + pub_WhichLang + "/JobNameManagement/TestExist_JobName2Code_Update?JobName2Code=";

    var CreateDlg = null;
    var UpdateDlg = null;

    var _$_GateWay = $("#txtGateWay");
    var _$_JobName1_id_Sele = $("#txtJobName1_id_Sele");

    $("#btnRefresh").click(function () {
        _$_datagrid.treegrid("reload");
        _$_datagrid.treegrid("unselectAll");
        //ExpandAllNode();
    });

    $("#btnAddJobName1").click(function () {
        AddJobName1();
    });

    _$_datagrid.treegrid({
        iconCls: 'icon-save',
        nowrap: true,
        autoRowHeight: false,
        autoRowWidth: false,
        striped: true,
        collapsible: true,
        url: QueryURL,
        sortName: 'int_id',
        sortOrder: 'asc',
        remoteSort: true,
        border: false,
        idField: 'ID',
        treeField: 'WorkProcess_Code',
        columns: [[
					{ field: 'WorkProcess_Code', title: '工作流程代码', width: 120
					},
					{ field: 'WorkProcess_Text', title: '工作流程名称', width: 200
					},
                    { field: 'gateway', title: '口岸', width: 150
                    },
                    { field: 'CCTV_workstation_id', title: '工位标签', width: 200
                    },
                    { field: 'bit_IsUse_Des', title: '状态', width: 100,
                        formatter: function (value, rowData, rowIndex) {
                            var strRet = "";
                            if (rowData.ID.indexOf("top_") != -1) {//工作内容1
                                switch (rowData.bit_IsUse.toLowerCase()) {
                                    case "false":
                                        strRet = "<a href='#'  title='点击可启用' class='ToggleEnableJobName1_cls' JobName1Id='" + rowData.ID.replace("top_", "") + "' isUse=1>" + "<span style='color:red'>" + rowData.bit_IsUse_Des + "</span>" + "</a>";
                                        break;
                                    case "true":
                                        strRet = "<a href='#' title='点击可禁用' class='ToggleEnableJobName1_cls' JobName1Id='" + rowData.ID.replace("top_", "") + "' isUse=0>" + "<span style='color:blue'>" + rowData.bit_IsUse_Des + "</span>" + "</a>";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else {//工作内容2
                                switch (rowData.bit_IsUse.toLowerCase()) {
                                    case "false":
                                        strRet = "<a href='#'  title='点击可启用' class='ToggleEnableJobName2_cls' JobName2Id='" + rowData.ID.replace("child_", "") + "' isUse=1>" + "<span style='color:red'>" + rowData.bit_IsUse_Des + "</span>" + "</a>";
                                        break;
                                    case "true":
                                        strRet = "<a href='#' title='点击可禁用' class='ToggleEnableJobName2_cls' JobName2Id='" + rowData.ID.replace("child_", "") + "' isUse=0>" + "<span style='color:blue'>" + rowData.bit_IsUse_Des + "</span>" + "</a>";
                                        break;
                                    default:
                                        break;
                                }
                            }

                            return strRet;
                        }
                    },
                     { field: 'updateJobName', title: '操作', width: 300,
                         formatter: function (value, rowData, rowIndex) {
                             if (rowData.ID.indexOf("top_") != -1) {
                                 if (rowData.hasChild == "1") {
                                     return "<a href='#' class='btnAddJobName2_cls' JobName1Id='" + rowData.ID.replace("top_", "") + "'>添加二级工作流程</a>" + "   " + "<a href='#' class='btnUpdateJobName1_cls' gateWay='" + rowData.gateway + "' JobName1Id='" + rowData.ID.replace("top_", "") + "' JobName1Code='" + rowData.WorkProcess_Code + "' JobName1Text='" + rowData.WorkProcess_Text + "'>修改</a>";
                                 } else {
                                     return "<a href='#' class='btnAddJobName2_cls' JobName1Id='" + rowData.ID.replace("top_", "") + "'>添加二级工作流程</a>" + "   " + "<a href='#' class='btnUpdateJobName1_cls' gateWay='" + rowData.gateway + "' JobName1Id='" + rowData.ID.replace("top_", "") + "' JobName1Code='" + rowData.WorkProcess_Code + "' JobName1Text='" + rowData.WorkProcess_Text + "'>修改</a>"; // +"   " + "<a href='#' class='btnDeleJobName1_cls' ID='" + rowData.ID.replace("top_", "") + "'>删除</a>";
                                     //return "<a href='#' class='btnAddJobName2_cls' JobName1Id='" + rowData.ID.replace("top_", "") + "'>添加二级工作流程</a>" + "   " + "<a href='#' class='btnUpdateJobName1_cls' gateWay='" + rowData.gateway + "' JobName1Id='" + rowData.ID.replace("top_", "") + "' JobName1Code='" + rowData.WorkProcess_Code + "' JobName1Text='" + rowData.WorkProcess_Text + "'>修改</a>" + "   " + "<a href='#' class='btnDeleJobName1_cls' ID='" + rowData.ID.replace("top_", "") + "' onclick='alert(345)' >删除</a>";
                                 }
                             }
                             else {
                                 return "<a href='#' class='btnUpdateJobName2_cls' JobName1Id='" + rowData.parentID.replace("top_", "") + "' JobName2Code='" + rowData.WorkProcess_Code + "' JobName2Text='" + rowData.WorkProcess_Text + "' JobName2Id='" + rowData.ID.replace("child_", "") + "' CCTV_workstation_id='" + rowData.CCTV_workstation_id + "'>修改</a>";// +"   " + "<a href='#' class='btnDeleJobName2_cls' ID='" + rowData.ID.replace("child_", "") + "'>删除</a>";
                             }
                         }
                     }
				]],
        pagination: false,
        pageSize: 200,
        pageList: [50, 60, 70, 80, 90, 100, 110, 120],
        toolbar: "#toolBar",
        onRowContextMenu: function (e, rowIndex, rowData) {
            e.preventDefault();
            _$_datagrid.treegrid("unselectAll");
            _$_datagrid.treegrid("selectRow", rowIndex);

            var cmenu = $('<div id="cmenu" style="width:100px;"></div>').appendTo('body');
            $('<div  id="mnuRefresh" iconCls="icon-reload"/>').html("刷新").appendTo(cmenu);
            cmenu.menu({
                onClick: function (item) {
                    cmenu.remove();
                    switch (item.id.toLowerCase()) {
                        case "mnurefresh":
                            _$_datagrid.treegrid("reload");
                            _$_datagrid.treegrid("unselectAll");
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
            //_$_datagrid.treegrid("reload");
        },
        onClickRow: function (row) {
            //_$_datagrid.treegrid("expand", row.ID);
            //            if (row.isLeaf == "1") {
            //                Update(row.frID, row.CategoryName, row.CategoryValue, row.CategoryUnit, row.mMemo, row.parentID)
            //            } else {
            //                _$_datagrid.treegrid("toggle", row.CategoryID);
            //            }
            _$_datagrid.treegrid("toggle", row.ID);
        },
        onDblClickRow: function (row) {
            //_$_datagrid.treegrid("expand", row.ID);
            //            if (row.isLeaf == "1") {
            //                Update(row.frID, row.CategoryName, row.CategoryValue, row.CategoryUnit, row.mMemo, row.parentID)
            //            }
        },
        onLoadSuccess: function (data) {
            var allDeleJobName1_cls = $(".btnDeleJobName1_cls");
            $.each(allDeleJobName1_cls, function (i, item) {
                var ID = $(item).attr("ID");
                $(item).click(function () {
                    DeleteJobName1(ID);
                });
            });

            var allDeleJobName2_cls = $(".btnDeleJobName2_cls");
            $.each(allDeleJobName2_cls, function (i, item) {
                var ID = $(item).attr("ID");
                $(item).click(function () {
                    DeleteJobName2(ID);
                });
            });

            var allUpdateJobName1_cls = $(".btnUpdateJobName1_cls");
            $.each(allUpdateJobName1_cls, function (i, item) {
                var JobName1Id = $(item).attr("JobName1Id");
                var gateWay = $(item).attr("gateWay");
                var JobName1Code = $(item).attr("JobName1Code");
                var JobName1Text = $(item).attr("JobName1Text");
                $(item).click(function () {
                    UpdateJobName1(gateWay, JobName1Code, JobName1Text, JobName1Id);
                });
            });

            var allUpdateJobName2_cls = $(".btnUpdateJobName2_cls");
            $.each(allUpdateJobName2_cls, function (i, item) {
                var JobName1Id = $(item).attr("JobName1Id");
                var JobName2Id = $(item).attr("JobName2Id");
                var JobName2Code = $(item).attr("JobName2Code");
                var JobName2Text = $(item).attr("JobName2Text");
                var CCTV_workstation_id = $(item).attr("CCTV_workstation_id");
                $(item).click(function () {
                    UpdateJobName2(JobName1Id, JobName2Code, JobName2Text, JobName2Id, CCTV_workstation_id);
                });
            });

            var allAddJobName2_cls = $(".btnAddJobName2_cls");
            $.each(allAddJobName2_cls, function (i, item) {
                var JobName1Id = $(item).attr("JobName1Id");
                $(item).click(function () {
                    AddJobName2(JobName1Id);
                });
            });

            var allToggleEnableJobName1_cls = $(".ToggleEnableJobName1_cls");
            $.each(allToggleEnableJobName1_cls, function (i, item) {
                var JobName1Id = $(item).attr("JobName1Id");
                var isUse = $(item).attr("isUse");
                $(item).click(function () {
                    EnableOrDisableJobName1(JobName1Id, isUse);
                });
            });

            var allToggleEnableJobName2_cls = $(".ToggleEnableJobName2_cls");
            $.each(allToggleEnableJobName2_cls, function (i, item) {
                var JobName2Id = $(item).attr("JobName2Id");
                var isUse = $(item).attr("isUse");
                $(item).click(function () {
                    EnableOrDisableJobName2(JobName2Id, isUse);
                });
            });

            _$_datagrid.treegrid("expandAll");
        }
    });

    //    _$_datagrid.treegrid('getPager').pagination({
    //        onSelectPage: function (pageNumber, pageSize) {
    //            //console.info(_$_datagrid.treegrid('options').queryParams['id']);
    //            delete _$_datagrid.treegrid('options').queryParams['id'];
    //            var QueryURL = "/" + pub_WhichLang + "/JobNameManagement/GetData";
    //            window.setTimeout(function () {
    //                $.extend(_$_datagrid.treegrid("options"), {
    //                    url: QueryURL
    //                });
    //                _$_datagrid.treegrid("reload");
    //            }, 10); //延迟100毫秒执行，时间可以更短
    //        }
    //    });

    function DeleteJobName1(ids) {
        $.ajax({
            url: DeleteJobName1URL + encodeURI(ids),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    // $.messager.alert('操作提示', JSONMsg.message, 'info');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                    _$_datagrid.treegrid("reload");
                    _$_datagrid.treegrid("unselectAll");
                } else {
                    // $.messager.alert('操作提示', JSONMsg.message, 'error');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'error');
                    //_$_datagrid.treegrid("reload");
                    //_$_datagrid.treegrid("unselectAll");
                    return false;
                }
            }
        });
    }

    function EnableOrDisableJobName1(ids, isUse) {
        $.ajax({
            url: EnableOrDisableJobName1URL + encodeURI(ids) + "&iEnable=" + encodeURI(isUse),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    // $.messager.alert('操作提示', JSONMsg.message, 'info');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                    _$_datagrid.treegrid("reload");
                    _$_datagrid.treegrid("unselectAll");
                } else {
                    // $.messager.alert('操作提示', JSONMsg.message, 'error');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'error');
                    //_$_datagrid.treegrid("reload");
                    //_$_datagrid.treegrid("unselectAll");
                    return false;
                }
            }
        });
    }

    function DeleteJobName2(ids) {
        $.ajax({
            url: DeleteJobName2URL + encodeURI(ids),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    // $.messager.alert('操作提示', JSONMsg.message, 'info');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                    _$_datagrid.treegrid("reload");
                    _$_datagrid.treegrid("unselectAll");
                } else {
                    // $.messager.alert('操作提示', JSONMsg.message, 'error');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'error');
                    //_$_datagrid.treegrid("reload");
                    //_$_datagrid.treegrid("unselectAll");
                    return false;
                }
            }
        });
    }

    function EnableOrDisableJobName2(ids, isUse) {
        $.ajax({
            url: EnableOrDisableJobName2URL + encodeURI(ids) + "&iEnable=" + encodeURI(isUse),
            type: "POST",
            cache: false,
            async: false,
            success: function (msg) {
                var JSONMsg = eval("(" + msg + ")");
                if (JSONMsg.result.toLowerCase() == 'ok') {
                    // $.messager.alert('操作提示', JSONMsg.message, 'info');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'info');
                    _$_datagrid.treegrid("reload");
                    _$_datagrid.treegrid("unselectAll");
                } else {
                    // $.messager.alert('操作提示', JSONMsg.message, 'error');
                    //$.messager.alert(Public_Dialog_Title, JSONMsg.message, 'error');
                    //_$_datagrid.treegrid("reload");
                    //_$_datagrid.treegrid("unselectAll");
                    return false;
                }
            }
        });
    }

    function AddJobName1() {
        $("#txtGateWay").val("");
        $("#hd_JobName1_Id").val("");
        $("#txtJobName1Code").val("");
        $("#txtJobName1Text").val("");

        _$_GateWay.combobox({
            url: QueryGateWayURL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_GateWay.combobox("setValue", "-99");
            }
        });

        CreateDlg = $('#dlg_Create_JobName1').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var _$_txtGateWay = _$_GateWay.combobox("getValue");
                    var _$_txtJobName1Code = $("#txtJobName1Code").val();
                    var _$_txtJobName1Text = $("#txtJobName1Text").val();

                    if (_$_txtGateWay == "-99" || _$_txtJobName1Code == "" || _$_txtJobName1Text == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(口岸、工作流程代码、工作流程文本)', "error");
                        return false;
                    }

                    //验证此工作流程1代码是否已使用
                    var bExist = false;
                    $.ajax({
                        type: "POST",
                        url: TestExist_JobName1Code_Add + encodeURI(_$_txtJobName1Code),
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
                            url: AddJobName1URL + encodeURI(_$_txtGateWay) + "&WorkProcess_L1_Code=" + encodeURI(_$_txtJobName1Code) + "&WorkProcess_L1_Text=" + encodeURI(_$_txtJobName1Text),
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
                            $("#btnRefresh").click();
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
            title: '添加一级工作流程',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 200
        });

        $('#dlg_Create_JobName1').dialog("open");
    }

    function UpdateJobName1(gateWay, JobName1Code, JobName1Text, JobName1Id) {
        $("#txtGateWay").val(gateWay);
        $("#hd_JobName1_Id").val(JobName1Id);
        $("#txtJobName1Code").val(JobName1Code);
        $("#txtJobName1Text").val(JobName1Text);

        _$_GateWay.combobox({
            url: QueryGateWayURL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_GateWay.combobox("setValue", gateWay);
            }
        });

        CreateDlg = $('#dlg_Create_JobName1').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var _$_txtGateWay = _$_GateWay.combobox("getValue");
                    var _$_txtJobName1Code = $("#txtJobName1Code").val();
                    var _$_txtJobName1Text = $("#txtJobName1Text").val();
                    var _$_hdJobName1_Id = $("#hd_JobName1_Id").val();

                    if (_$_txtGateWay == "-99" || _$_txtJobName1Code == "" || _$_txtJobName1Text == "" || _$_hdJobName1_Id == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(口岸、工作流程代码、工作流程文本)', "error");
                        return false;
                    }

                    //验证此工作流程1代码是否已使用
                    var bExist = false;
                    $.ajax({
                        type: "POST",
                        url: TestExist_JobName1Code_Update + encodeURI(_$_txtJobName1Code) + "&int_Id=" + encodeURI(_$_hdJobName1_Id),
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
                            url: UpdateJobName1URL + encodeURI(_$_hdJobName1_Id) + "&GateWay=" + encodeURI(_$_txtGateWay) + "&WorkProcess_L1_Code=" + encodeURI(_$_txtJobName1Code) + "&WorkProcess_L1_Text=" + encodeURI(_$_txtJobName1Text),
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
                            $("#btnRefresh").click();
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
            title: '修改一级工作流程',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 200
        });

        $('#dlg_Create_JobName1').dialog("open");
    }

    function AddJobName2(JobName1Id) {
        _$_JobName1_id_Sele.combobox({
            url: QueryJobName1URL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_JobName1_id_Sele.combobox("setValue", JobName1Id);
            }
        });

        $("#hd_JobName2_Id").val("");
        $("#txtJobName2Code").val("");
        $("#txtJobName2Text").val("");
        $("#txtCCTV_workstation_id").val("");

        CreateDlg = $('#dlg_Create_JobName2').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var jobName1Id = _$_JobName1_id_Sele.combobox("getValue");
                    var jobName2Code = $("#txtJobName2Code").val();
                    var JobName2Text = $("#txtJobName2Text").val();
                    var CCTV_workstation_id = $("#txtCCTV_workstation_id").val();

                    if (jobName1Id == "-99" || jobName2Code == "" || JobName2Text == "" || CCTV_workstation_id == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(一级工作流程、二级工作流程代码、二级工作流程文本,工位标签)', "error");
                        return false;
                    }

                    //验证此工作流程2代码是否已使用
                    var bExist = false;
                    $.ajax({
                        type: "POST",
                        url: TestExist_JobName2Code_Add + encodeURI(jobName2Code),
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
                            url: AddJobName2URL + encodeURI(jobName1Id) + "&WorkProcess_L2_Code=" + encodeURI(jobName2Code) + "&WorkProcess_L2_Text=" + encodeURI(JobName2Text) + "&CCTV_workstation_id=" + encodeURI(CCTV_workstation_id),
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
                            //$("#btnRefresh").click();
                            _$_datagrid.treegrid("reload", "top_" + JobName1Id);
                            _$_datagrid.treegrid("unselectAll");
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
            title: '添加二级工作流程',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 250
        });

        $('#dlg_Create_JobName2').dialog("open");
    }

    function UpdateJobName2(JobName1Id, JobName2Code, JobName2Text, JobName2Id, CCTV_workstation_id) {
        _$_JobName1_id_Sele.combobox({
            url: QueryJobName1URL,
            valueField: 'id',
            textField: 'text',
            editable: false,
            panelHeight: null,
            onLoadSuccess: function () {
                _$_JobName1_id_Sele.combobox("setValue", JobName1Id);
            }
        });

        $("#hd_JobName2_Id").val(JobName2Id);
        $("#txtJobName2Code").val(JobName2Code);
        $("#txtJobName2Text").val(JobName2Text);
        $("#txtCCTV_workstation_id").val(CCTV_workstation_id);

        CreateDlg = $('#dlg_Create_JobName2').dialog({
            buttons: [{
                text: '保 存',
                iconCls: 'icon-ok',
                handler: function () {
                    var jobName1Id = _$_JobName1_id_Sele.combobox("getValue");
                    var jobName2Code = $("#txtJobName2Code").val();
                    var JobName2Text = $("#txtJobName2Text").val();
                    var CCTV_workstation_id = $("#txtCCTV_workstation_id").val();

                    if (jobName1Id == "-99" || jobName2Code == "" || JobName2Text == "" || CCTV_workstation_id == "") {
                        reWriteMessagerAlert('操作提示', '请填写完整信息<br/>(一级工作流程、二级工作流程代码、二级工作流程文本,工位标签)', "error");
                        return false;
                    }

                    //验证此工作流程2代码是否已使用
                    var bExist = false;
                    $.ajax({
                        type: "POST",
                        url: TestExist_JobName2Code_Update + encodeURI(jobName2Code) + "&int_Id=" + encodeURI($("#hd_JobName2_Id").val()),
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
                            url: UpdateJobName2URL + encodeURI($("#hd_JobName2_Id").val()) + "&JobName1ID=" + encodeURI(jobName1Id) + "&WorkProcess_L2_Code=" + encodeURI(jobName2Code) + "&WorkProcess_L2_Text=" + encodeURI(JobName2Text) + "&CCTV_workstation_id=" + encodeURI(CCTV_workstation_id),
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
                            //$("#btnRefresh").click();
                            _$_datagrid.treegrid("reload", "top_" + JobName1Id);
                            _$_datagrid.treegrid("unselectAll");
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
            title: '修改二级工作流程',
            modal: true,
            resizable: true,
            cache: false,
            closed: true,
            left: 50,
            top: 30,
            width: 400,
            height: 250
        });

        $('#dlg_Create_JobName2').dialog("open");
    }

    function SeleAll() {
        var rows = _$_datagrid.treegrid("getRows");
        for (var i = 0; i < rows.length; i++) {
            var m = _$_datagrid.treegrid("getRowIndex", rows[i]);
            _$_datagrid.treegrid("selectRow", m)
        }
    }

    function InverseSele() {
        var rows = _$_datagrid.treegrid("getRows");
        var selects = _$_datagrid.treegrid("getSelections");
        for (var i = 0; i < rows.length; i++) {
            var bSele = false;
            var m = _$_datagrid.treegrid("getRowIndex", rows[i]);
            for (var j = 0; j < selects.length; j++) {
                var n = _$_datagrid.treegrid("getRowIndex", selects[j]);
                if (m == n) {
                    bSele = true;
                }
            }
            if (bSele) {
                _$_datagrid.treegrid("unselectRow", m)
            } else {
                _$_datagrid.treegrid("selectRow", m)
            }
        }
    }

    function createColumnMenu() {
        var tmenu = $('<div id="tmenu" style="width:100px;"></div>').appendTo('body');
        var fields = _$_datagrid.treegrid('getColumnFields');

        for (var i = 0; i < fields.length; i++) {
            var title = _$_datagrid.treegrid('getColumnOption', fields[i]).title;
            switch (fields[i].toLowerCase()) {
                case "workprocess_text":
                    break;
                default:
                    $('<div iconCls="icon-ok"/>').html("<span id='" + fields[i] + "'>" + title + "</span>").appendTo(tmenu);
                    break;
            }
        }
        tmenu.menu({
            onClick: function (item) {
                if ($(item.text).attr("id") == "WorkProcess_Text") {

                } else {
                    if (item.iconCls == 'icon-ok') {
                        _$_datagrid.treegrid('hideColumn', $(item.text).attr("id"));
                        tmenu.menu('setIcon', {
                            target: item.target,
                            iconCls: 'icon-empty'
                        });
                    } else {
                        _$_datagrid.treegrid('showColumn', $(item.text).attr("id"));
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
