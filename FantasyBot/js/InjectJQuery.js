function InjectJquery(urlJQuery) {
	if (typeof jQuery != "undefined")
		return;
	var scriptTag = document.createElement("script");
	scriptTag.setAttribute("type", "text/javascript");
	scriptTag.setAttribute("src", urlJQuery);
	scriptTag.onload = scriptTag.onreadystatechange = function() {
		alert("Injected jQuery " + $.fn.jquery + "!");
	};
	document.head.appendChild(scriptTag);
}

InjectJquery("https://code.jquery.com/jquery-2.1.3.js");