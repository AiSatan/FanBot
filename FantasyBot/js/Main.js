
function InjectJquery(urlJQuery) {
    if (typeof jQuery != "undefined")
        return;
    var scriptTag = document.createElement("script");
    scriptTag.setAttribute("type", "text/javascript");
    scriptTag.setAttribute("src", urlJQuery);
    scriptTag.onload = scriptTag.onreadystatechange = function () {
        console.log("Injected jQuery: " + $.fn.jquery + "!");
    };
    document.head.appendChild(scriptTag);
}

function GetDirections() {
    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
    var frame = $(frameDocument).find("frame[name='no_combat']")[0].contentDocument;
    /*console.log("Frame: " + $(frame).contents().html());*/
    return $(frame).contents().html();
}

function MoveTo(direction) {
    /*
    //it's working
    var loc = window.parent.frames['0'];
    var noCombat = loc.window.frames['4'];
    $('#d5 img', noCombat).click();
    */

    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
    var frame = $(frameDocument).find("frame[name='game_ref']")[0].contentDocument;
    frame.location.href = "cgi/maze_move.php?dir=" + direction;
}

function GetStatus() {
    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
    var frame = $(frameDocument).find("frame[name='no_combat']")[0].contentDocument;

    var hru = $(frame).contents().find("#hru").html();
    /*console.log("Status: " + hru);*/
    return hru;
}

function Logon(login, pass) {
    $("input[name='login']").prop("value", login);
    $("input[name='password']").prop("value", pass);
    //domen method
    Login();
}

function PickUpItem(id) {
    /*    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
        var frame = $(frameDocument).find("frame[name='no_combat']")[0].contentWindow;
    
        frame.pickUp(id, 0);
    
        var loc = window.parent.frames['loc'];
        var noCombat = loc.window.frames['no_combat'];*/

    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
    var frame = $(frameDocument).find("frame[name='game_ref']")[0].contentDocument;

    var qn = "&qn=" + window.parent.frames[0].window.frames[4].document.getElementById("pickupID" + id).value;
    frame.location.href = "cgi/maze_pickup.php?item_id=" + id + "&moo=" + 0 + qn;
    console.log("PickUpItem: ");
}

function InvokeAction(id) {
    /*    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
        var frame = $(frameDocument).find("frame[name='no_combat']")[0].contentWindow;
    
        frame.doQuestAction(id);
    
        var loc = window.parent.frames['loc'];
        var noCombat = loc.window.frames['no_combat'];*/

    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
    var frame = $(frameDocument).find("frame[name='game_ref']")[0].contentDocument;

    frame.location.href = "cgi/maze_qaction.php?id=" + id;
    console.log("InvokeAction: ");
}