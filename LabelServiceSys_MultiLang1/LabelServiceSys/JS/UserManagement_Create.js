$(function () {
    var QueryGateWayURL = "/" + pub_WhichLang + "/Default/LoadUnitCode";
    var QuerySexURL = "/" + pub_WhichLang + "/Default/LoadSexJSON";
    var _$_ddlurUnitCode = $('#ddlurUnitCode');
    var _$_ddurSex = $('#ddurSex');
    _$_ddlurUnitCode.combobox({
        url: QueryGateWayURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onLoadSuccess: function () {
            _$_ddlurUnitCode.combobox("setValue", "-99");
        },
        onSelect: function (row) {
            $("#txturUnitCode").val(row.id);
        }
    });
    _$_ddurSex.combobox({
        url: QuerySexURL,
        valueField: 'id',
        textField: 'text',
        editable: false,
        panelHeight: null,
        onLoadSuccess: function () {
            _$_ddurSex.combobox("setValue", "-99");
        },
        onSelect: function (row) {
            $("#txturSex").val(row.id);
        }
    });
});