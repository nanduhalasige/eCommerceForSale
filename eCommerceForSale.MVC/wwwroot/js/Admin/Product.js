var ProductTable;
$(document).ready(function () {
    LoadProductDatatable();
})

function LoadProductDatatable() {
    ProductTable = $("#ProductTable").DataTable({
        "order": [],
        "ajax": {
            "url": "/Admin/Products/GetAllProduct",
            "async": true,
            "dataSrc": ""
        },
        "responsive": true,
        "columns": [
            {
                "orderable": false,
                "data": function (row, type, val, meta) {
                    return `<a role="button" class="pointer" onclick="setdata($(this))" data-toggle="modal" data-row='${JSON.stringify(row)}' data-target="#ProductDetailsModal"><i class="fas fa-expand text-info"></i></a>`;
                }
            },
            {
                "data": "productName"
            },
            {
                "data": "category.categoryName"
            },
            {
                "data": "stock"
            },
            {
                "orderable": false,
                "data": function (row, type, val, meta) {
                    return row.isActive ? '<i class="fas fa-check text-success" title="Active"></i>' : '<i class="far fa-times-circle text-danger" title="Deactivated"></i>';
                }
            },
            {
                "orderable": false,
                "data": "id",
                "width": "15%",
                "render": function (data) {
                    return `<a role="button" title="Edit" class="pointer" href="/Admin/Products/AddOrModifyProduct/${data}"><i class="far fa-edit text-info"></i></a>`
                }
            },
            {
                "orderable": false,
                "data": "id",
                "width": "15%",
                "render": function (data) {
                    return `<a role="button" class="pointer" onclick=DeleteConfirm('/Admin/Products/HardDelete/${data}','hard') title="Hard Delete"><i class="fas fa-trash-alt text-danger"></i></a>`
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

function setdata(id) {
    var ProdDetails = id.data('row');
    var detailsViewImgString = ProdDetails.imagePath.split('~');
    var imageview = "";

    detailsViewImgString.map((img, i) => {
        if (img != "") {
            imageview += `<div class="tab-pane fade show col-lg-12 mb-2 active">
                                                                          <img class="img-fluid" src="${img}" height="200" />
                                                                          </div>`;
        }
    });

    var modelView = `<div class="container">
                        <div class="row mt-4">
                            <div class="col-lg-5 text-center border-right border-secondery" style="max-height:350px;overflow-y:auto;">
                                <div class="tab-content row h-100 d-flex justify-content-center align-items-center" id="myTabContent">
                                    ${imageview}
                                </div>
                            </div>
                            <div class="col-lg-6 ml-2">
                                <h2>
                                    ${ProdDetails.productName}
                                </h2>
                                <h4>
                                    <strong>Stock: </strong> ${ProdDetails.stock}
                                </h4>
                                ${ProdDetails.desciption}
                                <h4>
                                    <strong>Price: </strong> ${ProdDetails.price}
                                </h4>
                                <h5>
                                    <strong>Category: </strong> ${ProdDetails.category.categoryName}
                                </h5>
                                <h5>
                                    <strong>Product is Sold in: </strong> ${ProdDetails.isBoxOrPack ? "Box/Packs" : "Weights"}
                                </h5>
                             </div>
                         </div>
                     </div>`;

    $('.modal-body').html(modelView);
}