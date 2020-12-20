// запрет нажатия ENTER в поле ввода
$('.input-note').on('keydown', function (e) {
    if (e.keyCode === 13) {
        e.preventDefault();
        whenEnterPressed();
    }
});