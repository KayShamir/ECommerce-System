$().ready(function () {
    $('#btn_submit').click(function (event) {
        event.preventDefault();

        var data = new FormData();
        data.append('cus_firstname', $('#cus_firstname').val());
        data.append('cus_lastname', $('#cus_lastname').val());
        data.append('cus_birthdate', $('#cus_birthdate').val());
        data.append('cus_address', $('#cus_address').val());
        data.append('cus_number', $('#cus_number').val());
        data.append('cus_email', $('#cus_email').val());
        data.append('cus_pass', $('#cus_pass2').val());
        data.append('cus_file', $('#cus_file')[0].files[0]);

        $.ajax({
            url: '../Home/postCustomer',
            type: 'POST',
            data: data,
            processData: false,
            contentType: false,
            success: function (response) {
                console.log(response);
                if (response.success) {
                    alert('Successfully Created an Account!');
                    window.location.href = '../Home/Login_Page';
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('Error uploading file.');
            }
        });
    });

    $('#btnBack').click(function () {
        window.location.href = '../Home/Login_Page';
    });
});