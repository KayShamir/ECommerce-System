$(document).ready(function () {
    $(".btnEdit").click(function () {
        $.ajax({
            url: '../Home/GetUserDetails',
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    $("#cus_firstname").val(response.data.cus_firstname);
                    $("#cus_lastname").val(response.data.cus_lastname);
                    $("#cus_birthdate").val(response.data.cus_birthdate);
                    $("#cus_address").val(response.data.cus_address);
                    $("#cus_number").val(response.data.cus_phonenumber);
                    $("#cus_email").val(response.data.cus_email);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error("An error occurred while fetching user details.");
            }
        });
    });

    $("#btnSaveChanges").click(function () {
        var userData = {
            cus_firstname: $("#cus_firstname").val(),
            cus_lastname: $("#cus_lastname").val(),
            cus_birthdate: $("#cus_birthdate").val(),
            cus_address: $("#cus_address").val(),
            cus_phonenumber: $("#cus_number").val(),
            cus_email: $("#cus_email").val()
        };

        $.ajax({
            url: '../Home/UpdateUserDetails',
            type: 'POST',
            data: JSON.stringify(userData),
            contentType: 'application/json',
            success: function (response) {
                if (response.success) {
                    console.log('User details updated successfully.'); // Add this line
                    $('#updateModal').modal('hide');
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                } else {
                    toastr.error(response.message);
                }
            },

            error: function () {
                toastr.error("An error occurred while updating user details.");
            }
        });
    });
});
