$().ready(function () {
    $('.add_cart').click(function () {
        var productId = $(this).data('product-id'); 
        var quantity = $(this).closest('.card-footer').find('.quantity').val();

        var maxStock = parseInt($(this).closest('.card').find('.card-text').text().split(": ")[1].trim()); 

        if (quantity > 0 && quantity <= maxStock) {
            $.ajax({
                url: '../Home/AddToCart',
                type: 'POST',
                data: { productId: productId, quantity: quantity },
                success: function (data) {
                    toastr.success('Product Added to Cart');
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                },
                error: function () {
                    toastr.error('Error adding to cart');
                }
            });
        } else {
            if (quantity <= 0) {
                toastr.error('Quantity must be greater than 0');
            } else {
                toastr.error('Quantity exceeds available stock');
            }
        }
    });
    $(".btnDelete").click(function () {
        var row = $(this).closest('tr');
        var prodId = row.find('.prod_id').text().trim();
        $('#cart_yes').off('click').on('click', function () {
            $.ajax({
                url: '../Home/DeleteCart',
                type: 'POST',
                data: { prod_id: prodId },
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        row.remove();
                        setTimeout(function () {
                            location.reload();
                        }, 2000);
                    } else {
                        toastr.error(response.message);
                    }
                },
                error: function (error) {
                    toastr.error('Error deleting product: ' + error.responseText);
                }
            });
        });
    });
    $('#btnLogout').click(function () {
        window.location.href = '../Home/Login_Page';
    });
});
function incrementQuantity(prodId) {
    var qtyField = document.getElementById('qty-' + prodId);
    var currentQty = parseInt(qtyField.value);
    qtyField.value = currentQty + 1;
    updateTotalPrice(prodId);
}

function decrementQuantity(prodId) {
    var qtyField = document.getElementById('qty-' + prodId);
    var currentQty = parseInt(qtyField.value);
    if (currentQty > 1) {
        qtyField.value = currentQty - 1;
        updateTotalPrice(prodId);
    }
}

function updateTotalPrice(prodId) {
    var qtyField = document.getElementById('qty-' + prodId);
    var currentQty = parseInt(qtyField.value);
    var unitPrice = parseFloat(document.getElementById('price-' + prodId).getAttribute('data-unit-price'));
    var totalPrice = unitPrice * currentQty;
    document.getElementById('price-' + prodId).innerText = totalPrice.toFixed(2);
}

function incrementQuantity(prodId) {
    var qtyField = document.getElementById('qty-' + prodId);
    var currentQty = parseInt(qtyField.value);
    var newQty = currentQty + 1;
    var stock = parseInt($('tr[data-stock][data-prod-id="' + prodId + '"]').data('stock')); // Get the specific stock for this product

    if (newQty <= stock) {
        $.ajax({
            url: '../Home/UpdateCartQuantity',
            type: 'POST',
            data: { prodId: prodId, newQty: newQty },
            success: function (response) {
                if (response.success) {
                    qtyField.value = newQty;
                    updateTotalPrice(prodId);
                    toastr.success('Quantity updated');
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Error updating quantity');
            }
        });
    } else {
        toastr.error('Quantity exceeds available stock');
    }
}


function decrementQuantity(prodId) {
    var qtyField = document.getElementById('qty-' + prodId);
    var currentQty = parseInt(qtyField.value);
    var newQty = currentQty - 1;

    if (newQty >= 1) {
        $.ajax({
            url: '../Home/UpdateCartQuantity',
            type: 'POST',
            data: { prodId: prodId, newQty: newQty },
            success: function (response) {
                if (response.success) {
                    qtyField.value = newQty;
                    updateTotalPrice(prodId);
                    toastr.success('Quantity updated');
                    setTimeout(function () {
                        location.reload();
                    }, 2000);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Error updating quantity');
            }
        });
    } else {
        toastr.error('Quantity cannot be less than 1');
    }
}


function updateTotalPrice(prodId) {
    var qtyField = document.getElementById('qty-' + prodId);
    var currentQty = parseInt(qtyField.value);
    var unitPrice = parseFloat(document.getElementById('price-' + prodId).getAttribute('data-unit-price'));
    var totalPrice = unitPrice * currentQty;
    document.getElementById('price-' + prodId).innerText = totalPrice.toFixed(2);
}
