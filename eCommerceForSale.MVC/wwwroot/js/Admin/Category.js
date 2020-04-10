var datatable;
$(document).ready(function () {
    LoadCategoryDatatable();
})

function LoadCategoryDatatable() {
    datatable = $("#CategoryTable").DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAllCategory",
            "async": true,
            "cache": true,
            "dataSrc": ""
        },
        "responsive": true,
        "columns": [
            {
                "data": "categoryName",
                "width": "60%"
            },
            {
                "orderable": false,
                "data": function (row, type, val, meta) {
                    return row.isActive ? '<i class="fas fa-check text-success" title="Active"></i>' : '<i class="far fa-times-circle text-danger" title="Deactivated"></i>';
                }
            },
            //{ "data": "isActive", "width": "15%" },
            {
                "orderable": false,
                "data": "id",
                "width": "15%",
                "render": function (data) {
                    return `<a role="button" title="Edit" class="pointer" href="/Admin/Category/AddOrModifyCategory/${data}"><i class="far fa-edit text-info"></i></a>`
                }
            },
            {
                "orderable": false,
                "data": "id",
                "width": "15%",
                "render": function (data) {
                    return `<a role="button" class="pointer" onclick=DeleteConfirm('/Admin/Category/HardDelete/${data}','hard') title="Hard Delete"><i class="fas fa-trash-alt text-danger"></i></a>`
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
                        datatable.ajax.reload();
                        if (type === "soft") {
                            debugger;
                            var id = url.split('/')[4];
                            window.location = "/Admin/Category/AddOrModifyCategory/" + id
                        }
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}