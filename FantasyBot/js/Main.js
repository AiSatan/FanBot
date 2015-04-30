
function InjectJquery(urlJQuery) {
	if (typeof jQuery != "undefined")
		return;
	var scriptTag = document.createElement("script");
	scriptTag.setAttribute("type", "text/javascript");
	scriptTag.setAttribute("src", urlJQuery);
	scriptTag.onload = scriptTag.onreadystatechange = function() {
		console.log("Injected jQuery: " + $.fn.jquery + "!");
	};
	document.head.appendChild(scriptTag);
}

function GetDirections() {
    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
    var frame = $(frameDocument).find("frame[name='no_combat']")[0].contentDocument;
    console.log("Frame: " + $(frame).contents().html());
}

function MoveTo(direction) {
	var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
	var frame = $(frameDocument).find("frame[name='game_ref']")[0].contentDocument;
	frame.location.href = "cgi/maze_move.php?dir=" + direction;
	var sleepTime = 1000;
	var hru = $(frame).contents().find("#hru").html();
	if(hru !== "бодрость: 100%") {
		sleepTime = 5000;
	}
	setTimeout(function() {
		console.log("Run: ");
	}, sleepTime);
}

function Logon() {
    $("input[name='login']").prop("value", "AiTest");
    $("input[name='password']").prop("value", "adminRam");
    //domen method
    Login();
}