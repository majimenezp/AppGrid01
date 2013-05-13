﻿/// <reference path="jquery-1.6.1.min.js" />

$(function () {

    $.getJSON("Apps/List", {},
    function (data) {
        var resultados = "";
        for (var i = 0; i < data.length; i++) {
            resultados += "<tr><td><a href=\"Apps/App/" + data[i].Name + "\">" + data[i].Name + "<a></td><td>" + data[i].Status + "</td><td>" + data[i].NumProc + "</td></tr>";
        }
        $("#applist tbody").html("");
        $("#applist tbody").append(resultados);
    }
);
});