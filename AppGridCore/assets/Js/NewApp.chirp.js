/// <reference path="jquery-1.6.1.min.js" />

$(function () {
    $("#existpanel").hide();
    $("#appname").keyup(function () {
        $.getJSON("../CheckAppName/" + $("#appname").val() + "/", {},
            function (data) {
                http: //cot-cdc-st1813:5500/
                $("#existpanel").removeClass().addClass(data.cssclass);
                $("#message").html(data.message);
                if (data.exist) {
                    $("#save").attr("disabled", "disabled");
                }
                else {
                    $("#save").removeAttr("disabled");
                }
                $("#existpanel").show();
            }
        );
    });

        $("#newapp").bind("submit", function (e) {
            e.preventDefault();
            $(this).ajaxSubmit({
               success:showInfo,
               iframe:true
            });
    });
});

function showInfo(responseText,status)
{
    alert('status: ' + status + '\n\nresponseText: \n' + responseText + 
        '\n\nThe output div should have already been updated with the responseText.'); 
}