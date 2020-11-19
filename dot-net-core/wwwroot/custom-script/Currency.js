var currencyId = 0;
$(document).ready(function () {
    getCurrencyList();
});

function getCurrencyList() {
    $("#currencyList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/Currency/GetList",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "currency", "name": "Currency", "width": "70%" },
            { "data": "symbol", "name": "Symbol", "width": "10%" },
            { "data": "symbolAR", "name": "SymbolAR", "width": "10%" },
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
    currencyId = id
    $('#modal-delete-currency').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/Currency/Delete" + '?' + $.param({ "Id": currencyId }),
        type: 'DELETE',
        success: function (result) {
            currencyId = 0;
            if (result.status === 1) {
                $("#currencyList").dataTable().fnDestroy();
                getCurrencyList();
                toastr.success(result.message)
            } else {
                toastr.error(result.message)
            }
        },
        error: function (request, msg, error) {
            toastr.error(msg)
        },
    });
}

function onDeleteNo() {
    currencyId = 0;
}