$(document).ready(function () {
    $("#account_yes").click(function () {
        $.ajax({
            url: '/Home/DeleteAccount',
            type: 'POST',
            success: function (response) {
                if (response.success) {
                    window.location.href = '/Home/Login_Page';
                } else {
                    alert(response.message);
                }
            },
            error: function () {
                alert('An error occurred while deleting account.');
            }
        });
    });
});
