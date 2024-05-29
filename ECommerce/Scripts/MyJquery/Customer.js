$().ready(function () {
    $('#btn_submit').click(function () {
        var data = new FormData();
        data.append('cus_firstname', $('#cus_firstname').val());
        data.append('cus_lastname', $('#cus_lastname').val());
        data.append('cus_birthdate', $('#cus_birthdate').val());
        data.append('cus_address', $('#cus_address').val());
        data.append('cus_number', $('#cus_number').val());
        data.append('cus_email', $('#cus_email').val());
        data.append('cus_pass', $('#cus_pass2').val());

        $.ajax({
            url: '../Home/postCustomer',
            type: 'POST',
            data: data,
            processData: false,
            contentType: false,
            success: function (data) {
                console.log(data);
                alert('Successfully Created an Account!');
            },
            error: function () {
                alert('Error uploading file.');
            }
        });

    })
    $('#btn_submit').click(function () {
        window.location.href = '../Home/Login_Page';
    })
    $('#btnBack').click(function () {
        window.location.href = '../Home/Login_Page';
    })
})