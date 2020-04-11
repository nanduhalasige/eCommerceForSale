var UsersTable;
$(document).ready(function () {
    LoadUserDatatable();
})

function LoadUserDatatable() {
    UsersTable = $("#UsersTable").DataTable({
        "ajax": {
            url: "/Admin/Users/GetAll",
            dataSrc: ""
        },
        "responsive": true,
        "columns": [
            { "data": "fullName" },
            { "data": "email" },
            { "data": "phoneNumber" },
            { "data": "role" },
            {
                "orderable": false,
                "data": { id: "id", lockoutEnd: "lockoutEnd" },
                "width": "15%",
                "render": function (data) {
                    var dateToday = new Date().getDate();
                    var dateLockout = new Date(data.lockoutEnd).getDate();
                    if (dateLockout > dateToday) {
                        return `<a role="button" title="Lock user" class="btn btn-outline-success" onclick=LockUnlockUser('${data.id}')><i class="fas fa-user-lock"></i> Lock</a>`
                    }
                    else {
                        return `<a role="button" title="Unlock" class="btn btn-outline-danger" onclick=LockUnlockUser('${data.id}')><i class="fas fa-unlock-alt"></i> Unlock</a>`
                    }
                }
            }
        ]
    });
}

function LockUnlockUser(id) {
    $.ajax({
        url: "/Admin/Users/LockUnlockUser",
        type: "POST",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                UsersTable.ajax.reload();
            } else {
                toastr.error(data.message);
            }
        }
    })
}