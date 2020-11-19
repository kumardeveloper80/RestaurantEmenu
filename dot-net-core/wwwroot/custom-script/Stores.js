var storeId = 0;
var selectedStatus;
var refDataTable;

$(document).ready(function () {
    selectedStatus = $('#status-select').val();
    selectedConceptId = $('#ConceptId').val();
    getStoreList();
    $('#status-select').change(function () {
        selectedStatus = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.status = selectedStatus
        });
        refDataTable.draw();
    });

    $('#ConceptId').change(function () {
        selectedConceptId = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.conceptId = selectedConceptId
        });
        refDataTable.draw();
    });
});

function getStoreList() {
    refDataTable = $("#storeList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/Stores/GetList",
            "type": "POST",
            "datatype": "json",
            "data": {
                "status": selectedStatus
            }
        },
        "columns": [
            { "data": "storeCode", "name": "StoreCode", "width": "5%" },
            { "data": "storeName", "name": "StoreName", "width": "20%" },
            { "data": "clientName", "name": "ClientName", "width": "15%" },
            { "data": "conceptName", "name": "ConceptName", "width": "20%" },
            { "data": "regionName", "name": "RegionName", "width": "10%" },
            { "data": "countryName", "name": "CountryName", "width": "10%" },
            {
                "data": "status", "name": "Status", "width": "10%",
                "mRender": function (data, type, row) {
                    if (row.status == true) {
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
    storeId = id
    $('#modal-delete-store').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/Stores/Delete" + '?' + $.param({ "Id": storeId }),
        type: 'DELETE',
        success: function (result) {
            storeId = 0;
            if (result.status === 1) {
                $("#storeList").dataTable().fnDestroy();
                getStoreList();
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
    storeId = 0;
}