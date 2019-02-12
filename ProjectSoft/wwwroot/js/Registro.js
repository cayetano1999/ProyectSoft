var button = document.getElementById("btnregistrar");
var load = document.getElementById("load");
load.style.display= 'none';

button.addEventListener("click", function btn() {

    button.value = "";
    load.style.display= 'block';


});


