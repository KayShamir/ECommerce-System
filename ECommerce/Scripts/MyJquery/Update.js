$().ready(function () {
    $(".btnEdit").click(function () {
        var row = $(this).closest('tr');
        var id = row.find('.prod_id').text();
        $.ajax({
            url: '/Home/GetProduct',
            type: 'GET',
            data: { prod_id: id },
            success: function (data) {
                if (data) {
                    $('#edit_prod_id').val(id);
                    $('#edit_prod_name').val(data.prod_name);
                    $('#edit_prod_desc').val(data.prod_desc);
                    $('#edit_prod_price').val(data.prod_price);
                    $('#edit_prod_gender').val(data.prod_gender);
                    $('#edit_prod_color').val(data.prod_color);
                    $('#edit_prod_size').val(data.prod_size);
                    $('#edit_prod_material').val(data.prod_material);
                    $('#edit_prod_category').val(data.prod_category);
                    $('#edit_prod_stock').val(data.prod_stock);
                    $('#edit_prod_file').val(data.prod_file);
                    // Show the modal if not shown
                    $('#updateModal').modal('show');
                } else {
                    alert('No product found with this ID.');
                }
            },
            error: function (error) {
                alert('Error fetching product: ' + error.responseText);
            }
        });
    });

    $("#btn_savechange").click(function () {
        var data = new FormData();
        var fileInput = $('#edit_prod_file')[0].files[0];
        if (fileInput) {
            data.append('insert_file', fileInput);
        }

        data.append('prod_id', $('#edit_prod_id').val());
        data.append('prod_name', $('#edit_prod_name').val());
        data.append('prod_desc', $('#edit_prod_desc').val());
        data.append('prod_price', $('#edit_prod_price').val());
        data.append('prod_gender', $('#edit_prod_gender').val());
        data.append('prod_color', $('#edit_prod_color').val());
        data.append('prod_size', $('#edit_prod_size').val());
        data.append('prod_material', $('#edit_prod_material').val());
        data.append('prod_category', $('#edit_prod_category').val());
        data.append('prod_stock', $('#edit_prod_stock').val());

        $.ajax({
            url: '/Home/PostProductUpdate',
            type: 'POST',
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                console.log(data);
                toastr.success('Product updated successfully.');
                $('#updateModal').modal('hide');
                setTimeout(function () {
                    location.reload();
                }, 3000);
            },
            error: function () {
                alert('Error saving changes.');
            }
        });
    });
});
