/// <reference path="jquery-1.6.1.min.js" />
$(function () {
    $("#numInstances").slider({
        max: 20,
        min: 1,
        value:$("#valNumInst").html(),
        change: function (event, ui) {
            $("#valNumInst").html(ui.value);
            var appname = $("#appname").val();
            $.getJSON("AppsInstance/" + appname + "/" + ui.value, {},
                function (data) {
                    var regs = "";
                    for (var i = 0; i < data.Instances.length; i++) {
                        regs += "<tr><td>" + data.Instances[i].Pid + "</td><td>" + data.Instances[i].Port + "</td><td>" + data.Instances[i].Server + "</td></tr>";
                    }
                    $("#tbInstances").html(regs);
                });
        }
    });
});
