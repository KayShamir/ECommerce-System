$(document).ready(function () {
    $('#btn_submit').click(function (event) {
        event.preventDefault(); // Prevent default form submission
        var data = new FormData();
        data.append('cus_email', $('#cus_email').val());
        data.append('cus_pass', $('#cus_pass').val());

        $.ajax({
            url: '../Home/Login',
            type: 'POST',
            data: data,
            processData: false,
            contentType: false,
            success: function (response) {
                if (response.admin) {
                    window.location.href = '../Home/Dashboard_Admin';
                } else if (response.success) {
                    window.location.href = '../Home/User_Dashboard';
                } else {
                    alert('Login failed: ' + response.message);
                }
            },
            error: function () {
                alert('Error processing your request.');
            }
        });
    });

    $('#btnCreate').click(function () {
        window.location.href = '../Home/Customer';
    });
});