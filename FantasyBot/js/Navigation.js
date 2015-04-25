function GetDirections() {
    var frameDocument = $("frame[name='loc']", top.document)[0].contentDocument;
    var frame = $(frameDocument).find("frame[name='no_combat']")[0].contentDocument;
    console.log("Frame: " + $(frame).contents().html());
}

GetDirections();