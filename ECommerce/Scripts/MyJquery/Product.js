$().ready(function () {
    $('#btn_submit').click(function () {
        var data = new FormData();
        data.append('prod_name', $('#prod_name').val());
        data.append('prod_desc', $('#prod_desc').val());
        data.append('prod_price', $('#prod_price').val());
        data.append('prod_gender', $('#prod_gender').val());
        data.append('prod_color', $('#prod_color').val());
        data.append('prod_size', $('#prod_size').val());
        data.append('prod_material', $('#prod_material').val());
        data.append('prod_category', $('#prod_category').val());
        data.append('prod_stock', $('#prod_stock').val());
        data.append('prod_file', $('#prod_file')[0].files[0]);

        $.ajax({
            url: '../Home/postProduct',
            type: 'POST',
            data: data,
            processData: false, 
            contentType: false, 
            success: function (data) {
                console.log(data);
                toastr.success('Product added successfully.');
                $('#addModal').modal('hide');
                setTimeout(function () {
                    location.reload();
                }, 3000);
            },
            error: function () {
                alert('Error uploading file.');
            }
        });

    })
    $('#btn_cancel').click(function () {
        $('#addModal').hide();
    });
})