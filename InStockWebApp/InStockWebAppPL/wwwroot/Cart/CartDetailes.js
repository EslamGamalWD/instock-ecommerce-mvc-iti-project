$(document).ready(function () {
    $('.decreaseButton').on('click', function () {
        var $row = $(this).closest('tr');
        var quantityInput = parseInt($row.find('.quantityInput').val());
        var itemId = $(this).data('item-id');
        console.log(itemId);

        $.ajax({
            url: '/Cart/DecreaseItemCount',
            type: 'POST',
            cache: false,

            data: { itemId: itemId },
            success: function (response) {
                console.log(response);
                if (response.success) {
                    $('.totalcard').text(response.cardPrice + 'EGP');
                    // console.log(response.cardPrice);
                    $row.find('.quantityInput').val(response.quantity);
                    $('#cartCount').text('(' + response.total + ')');
                    $row.find('.totalPrice').text(response.totalPrice + 'EGP');
                    // console.log(response)
                    if (response.quantity < response.instock) {
                        $row.find('.increaseButton').prop('disabled', false);
                    } else {
                        $row.find('.increaseButton').prop('disabled', true);

                    }
                    if (response.isDeleted) {
                        $row.remove();
                    }

                    if (response.total == 0) {
                        $('.checkout').hide();
                    }
                } else {
                    console.error('Error decreasing item count:', response.error);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error decreasing item count:', error);
            }
        });
    });

    $('.increaseButton').on('click', function () {
        var $row = $(this).closest('tr');
        var itemId = $(this).data('item-id');
        var Index = parseInt($(this).data('index'));
        console.log(Index);
        $('.checkout').show();
        $.ajax({
            url: '/Cart/IncreaseItemCount',
            type: 'POST',
            cache: false,

            data: { itemId: itemId },
            success: function (response) {
                if (response.success) {
                    $row.find('.totalPrice').text(response.totalPrice + 'EGP');
                    $row.find('.quantityInput').val(response.quantity);
                    $('#cartCount').text('(' + response.total + ')');
                    $('.totalcard').text(response.cardPrice + 'EGP');

                    console.log(response)
                    if (response.quantity === response.instock) {
                        $row.find('.increaseButton').prop('disabled', true);
                        Toastify({
                            text: "Quantity cannot exceed available stock",
                            className: "info",
                            style: {
                                background: "linear-gradient(to right, #00b09b, #96c93d)",
                            },
                            preventDuplicates: true
                        }).showToast();
                    } else {
                        $row.find('.increaseButton').prop('disabled', false);
                        // console.log(response)
                    }
                } else {
                    console.error('Error increasing item count:', response.error);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error increasing item count:', error);
            }
        });


    });
    $('.deleteCartItem').on('click', function () {
        var itemId = $(this).data('item-id');
        var $row = $(this).closest('tr');
        console.log(itemId)
        $.ajax({
            type: "POST",
            url: "/Cart/DeleteItem",
            cache: false,

            data: { itemId: itemId },
            success: function (result) {
                console.log(result);

                $row.remove();
                $('#cartCount').text('(' + result.total + ')');
                $('.totalcard').text(result.cardPrice + 'EGP');
                console.log($("#row-" + itemId));
                if (result.cardPrice == 0) {
                    $('.checkout').hide();
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });
});