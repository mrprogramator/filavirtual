$("#menu-toggle").click(function (e) {
    e.preventDefault();
    $("#wrapper").toggleClass("toggled");
});
if ($("#Tipo :selected").text() === "Solucion") {
    $("#PadreContenedor").hide();
} else {
    $("#PadreContenedor").show();

}
