$(document).ready(function () {
    $(".product-name").click(function () {
        var selectedProduct = $(this).text()
        var productDetails = selectedProduct.split('|')
        console.log(productDetails)
        /*$('#productModal').modal('show')
        $('#modalProdName').text(selectedProductName)*/

    });
});