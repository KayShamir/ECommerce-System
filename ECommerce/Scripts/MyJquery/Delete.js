$().ready(function () {
    $(".btnDelete").click(function () {
        var row = $(this).closest('tr');
        var id = row.find('.prod_id').text();
        var stock = parseInt(row.find('.prod_stock').text().trim());

        $('#btnYes').off('click').on('click', function () {
            $.ajax({
                url: '../Home/DeleteProduct',
                type: 'POST',
                data: { prod_id: id },
                success: function (response) {
                    if (response[0].success) {
                        toastr.success(response[0].message);
                        row.remove();
                        setTimeout(function () {
                            location.reload();
                        }, 1000);
                    } else {
                        toastr.error(response[0].message);
                    }
                },
                error: function (error) {
                    alert('Error deleting product: ' + error.responseText);
                }
            });
        });
    });
    $('#btnNo').click(function () {
        $('#deleteModal').hide();
    });
})