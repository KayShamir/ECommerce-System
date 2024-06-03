$(document).ready(function () {
    $('.btncheckout').on('click', function () {
        var allValid = true;
        var cartItems = $('.items');

        cartItems.each(function () {
            var stock = parseInt($(this).data('stock')); // Assuming a fixed stock, replace with actual stock data if available
            var quantity = parseInt($(this).find('.quantity').val());

            if (quantity <= 0 || quantity > stock) {
                allValid = false;
                alert('Quantity for ' + $(this).find('.prod-id').text() + ' is invalid. Please ensure it is greater than 0 and less than or equal to stock.');
                return false; // Break out of the loop
            }
        });

        if (allValid) {
            var userId = $(this).data('user-id');

            if (confirm("Are you sure you want to checkout?")) {
                $.ajax({
                    type: "POST",
                    url: "/Home/Checkout",
                    data: { userId: userId },
                    success: function (response) {
                        if (response.success) {
                            toastr.success("Checkout successful!");
                            setTimeout(function () {
                                window.location.reload();
                            }, 1000);
                        } else {
                            alert(response.error);
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error(error);
                        alert("Error during checkout.");
                    }
                });
            }
        } else {
            console.log('Invalid quantities found. Checkout aborted.');
        }
    });

    // Other code...
});
