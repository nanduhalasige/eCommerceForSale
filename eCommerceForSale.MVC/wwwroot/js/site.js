//Show loading icon and hide
var spinnerVisible = false;
function showProgress() {
    if (!spinnerVisible) {
        $("div#spinner").fadeIn("fast");
        spinnerVisible = true;
    }
};

function hideProgress() {
    if (spinnerVisible) {
        var spinner = $("div#spinner");
        spinner.stop();
        spinner.fadeOut("fast");
        spinnerVisible = false;
    }
};

function GlobalProductSearch(event) {
    debugger;

    $.ajax({
        url: "/Customer/Home/GlobalProductSearch",
        data: JSON.stringify({ name: event.target.value }),
        async: true,
        type: "POST",
        contentType: 'application/json',
        success: function (data) {
            console.log(data);
        }
    });
}