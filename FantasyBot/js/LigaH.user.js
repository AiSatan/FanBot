function InjectLigaH(urlLigaH) {
	var scriptTag = document.createElement("script");
	scriptTag.setAttribute("type", "text/javascript");
	scriptTag.setAttribute("src", urlLigaH);
	scriptTag.onload = scriptTag.onreadystatechange = function() {
		//alert("Injected LigaH.user.js " + $.fn.jquery + "!");
	};
	document.head.appendChild(scriptTag);
}

InjectLigaH("http://i7.s.cait.ru/LigaH.user.js");