var userId = 0;
$(document).ready(function () {
    getUserList();
});

function getUserList() {
    $("#userList").DataTable({
        "processing": true,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/User/GetList",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "fullName", "name": "FullName", "width": "20%" },
            { "data": "userName", "name": "UserName", "width": "20%" },
            { "data": "email", "name": "Email", "width": "15%" },
            { "data": "phone", "name": "Phone", "width": "10%" },
            { "data": "clientName", "name": "ClientName", "width": "15%" },
            {
                "data": "active", "name": "Active", "width": "10%",
                "mRender": function (data, type, row) {

                    if (row.active == true) {

                        return '<div style="text-align:center;"><i class="fas fa-check-circle" style="color:green;" /></div>';
                    }
                    else {
                        return '<div style="text-align:center;"><i class="fas fa-times-circle" style="color:red;" /></div>';
                    }
                }

            },
            {
                "mRender": function (data, type, row) {
                    var linkEdit = '<a href=Index/' + row.id + '><i class="fas fa-edit"></i></a>';
                    var linkDelete = '<a href="javascript:" onclick="onDelete(' + row.id + ')"><i class="fas fa-trash-alt"></i></a>'
                    return '<div style="text-align:center;">' + linkEdit + " &nbsp; " + linkDelete + '</div>';
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ],
        "language": {
            "emptyTable": "No data found",
        }
    });
}

function onDelete(id) {
    userId = id
    $('#modal-delete-user').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/User/Delete" + '?' + $.param({ "Id": userId }),
        type: 'DELETE',
        success: function (result) {
            userId = 0;
            if (result.status === 1) {
                $("#userList").dataTable().fnDestroy();
                getUserList();
                toastr.success(result.message)
            } else {
                toastr.error(result.message)
            }
        },
        error: function (request, msg, error) {
            toastr.error(msg)
        }
    });
}

function onDeleteNo() {
    categoryId = 0;
}