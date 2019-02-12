var divinfo = document.getElementById("divinfo");
var divfotos = document.getElementById("divifotos");
var divarchivo = document.getElementById("divarchivo");


divfotos.style.display = 'none';
divarchivo.style.display = 'none';

divagregarproyecto.style.display = 'none';

alert("OK");

btnproyecto.addEventListener("click", function btnclick() {
    
    tabla.style.display = 'none';
    divagregarproyecto.style.display = 'inline-block';

});


