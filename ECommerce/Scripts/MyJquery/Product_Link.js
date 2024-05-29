$(document).ready(function () {
    // Click event for product cards
    $('.card-clickable').click(function () {
        var productData = $(this).find('.product-name').text().split('|');

        $('#modalProdName').text(productData[0].trim());
        $('#modalProdPrice').text(productData[1].trim());
        $('#modalProdDesc').text(productData[2].trim());
        $('#modalProdGender').text(productData[3].trim());
        $('#modalProdColor').text(productData[4].trim());
        $('#modalProdSize').text(productData[5].trim());
        $('#modalProdMaterial').text(productData[6].trim());
        $('#modalProdCategory').text(productData[7].trim());
        $('#modalProdStocks').text(productData[8].trim());

        // Show the modal
        $('#productModal').modal('show');
    });
});
