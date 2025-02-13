function GetHeadQuarterInfo(Hduid) {
    var ResponsRes;
    $.ajax({
        async: false,
        type: "POST",
        url: "../gmapdashWS.asmx/GetHeadQuarterInfoWS",
        data: "{'Hduid':'" + Hduid + "'}",
        contentType: "application/json",
        datatype: "json",
        success: function (responseFromServer) {
            ResponsRes = responseFromServer.d;
            //callback.call(ResponsRes);
        },
        error: function () {
            alert('Something goes worng');
        }
    });
    return (ResponsRes);
}

function GetSchoolsInfo(Hduid) {
    var ResponsRes;
    $.ajax({
        async: false,
        type: "POST",
        url: "../gmapdashWS.asmx/GetSchoolsInfoWS",
        data: "{'Hduid':'" + Hduid + "'}",
        contentType: "application/json",
        datatype: "json",
        success: function (responseFromServer) {
            ResponsRes = responseFromServer.d;
            //callback.call(ResponsRes);
        },
        error: function () {
            alert('Something goes worng');
        }
    });
    return (ResponsRes);
}