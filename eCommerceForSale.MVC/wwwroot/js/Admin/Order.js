var OrderTable;
$(document).ready(function () {
    LoadOrderDatatable();
})

function LoadOrderDatatable() {
    OrderTable = $("#OrderTable").DataTable({
        "order": [],
        "ajax": {
            "url": "/Admin/Order/GetAll",
            "async": true,
            "dataSrc": ""
        },
        "responsive": true,
        "columns": [
            {
                "data": "id"
            },
            {
                "data": "applicationUser.fullName"
            },
            {
                "data": "address.mobileNumber"
            },
            {
                "data": "applicationUser.email"
            },
            {
                "data": "orderStatus"
            }, {
                "data": "orderTotal"
            },
            {
                "orderable": false,
                "data": "id",
                "width": "15%",
                "render": function (data) {
                    return `<a role="button" title="Mark as complete" class="pointer" reasonly href="#"><i class="fas fa-check text-success"></i></a>`
                }
                //href="/Admin/Order/MarkComplete/${data}
            },
            {
                "orderable": false,
                "data": "id",
                "width": "15%",
                "render": function (data) {
                    return `<a role="button" class="pointer" href="Order/EditOrder/${data}" title="Manage order"><i class="fas fa-edit text-info"></i></a>`
                }
            }
        ]
    });
}

function DeleteConfirm(url, type = 'soft') {
    swal({
        title: 'Are you sure?',
        text: type === "hard" ? "The category will be deleted permanently" : "The category will be deactivated temporarily",
        icon: 'warning',
        buttons: true,
        dangerMode: true,
    }).then((IsConfirm) => {
        if (IsConfirm) {
            $.ajax({
                type: "Delete",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        ProductTable.ajax.reload();
                        if (type === "soft") {
                            var id = url.split('/')[4];
                            window.location = "/Admin/Category/AddOrModifyProduct/" + id
                        }
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}