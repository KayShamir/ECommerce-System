$().ready(function () {
    $(".btnDelete").click(function () {
        var row = $(this).closest('tr')
        var id = row.find('.prod_id').text()

        $('#btnYes').click(function () {

            $.ajax({
                url: '../Home/DeleteProduct',
                type: 'POST',
                data: { prod_id: id },
                success: function (response) {
                    toastr.success('Product removed successfully.');
                    row.remove();
                    setTimeout(function () {
                        location.reload();
                    }, 3000);
                },

                error: function (error) {
                    alert('Error deleting product: ' + error.responseText);
                }
            })
        })
    })
    $('#btnNo').click(function () {
        $('#deleteModal').hide();
    });
})