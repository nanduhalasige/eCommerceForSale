function LaunchAddressModal() {
    var url = "/Customer/ShoppingCart/AddOrModifyAddress";
    $.ajax({
        url: url,
        async: true,
        type: "GET",
        contentType: 'application/json',
        success: function (data) {
            $('.address-modal-body').html(data);
        },
        error: function (error) {
            toastr.error("Something went wrong");
        }
    });
}

function submitForm(id, event) {
    event.preventDefault();
    event.stopPropagation();

    var form = id.parents('.modal').find('form');
    var actionUrl = form.attr('action');
    var dataToSend = form.serialize();

    $.post(actionUrl, dataToSend).done(function (data) {
        $('.address-modal-body').html('').html(data);

        var isValid = $('.address-modal-body').find('[name="IsValid"]').val() == 'True';
        if (isValid) {
            $('#AddessModal').modal('hide');
            window.location.href = "/Customer/ShoppingCart/CartSummary"
        }
    });
}

$('#AddessModal').on('hidden.bs.modal', function (e) {
    $('.address-modal-body').html('');
})

function ClearAddressForm(event) {
    event.preventDefault();
    $(this).closest('form').find("input[type=text]").val("");
}