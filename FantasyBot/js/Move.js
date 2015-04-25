function MoveTo(direction) {
	var frameDocument = $("frame[name=\"loc\"]", top.document)[0].contentDocument;
	var frame = $(frameDocument).find("frame[name=\"game_ref\"]")[0].contentDocument;
	frame.location.href = "cgi/maze_move.php?dir=" + direction;
	var sleepTime = 1000;
	var hru = $(frame).contents().find("#hru").html();
	if(hru === "бодрость: 100%") {
		sleepTime = 5000;
	}
	setTimeout(function() {
		console.log("Run: ");
	}, sleepTime);
}

/*

7 3 -
1 0 2
- 4 -

*/