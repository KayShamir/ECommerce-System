$(".btnCart").each(function () {
    $(this).click(function () {
        var id = $(this).closest('.items').find('.studId').html();
        alert(id);
    });
});
