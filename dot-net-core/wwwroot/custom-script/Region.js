var regionId = 0;
var selectedStatus;
var refDataTable;

$(document).ready(function () {
    selectedStatus = $('#status-select').val();
    getRegionList();
    $('#status-select').change(function () {
        selectedStatus = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.status = selectedStatus
        });
        refDataTable.draw();
    });
});

function getRegionList() {
    refDataTable = $("#regionList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/Region/GetList",
            "type": "POST",
            "datatype": "json",
            "data": {
                "status": selectedStatus
            }
        },
        "columns": [
            { "data": "region", "name": "region", "width": "80%" },
            {
                "data": "active", "name": "Active", "width": "10%",
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
                    var linkEdit = '<a href=Index/' + row.regionId + '><i class="fas fa-edit"></i></a>';
                    var linkDelete = '<a href="javascript:" onclick="onDelete(' + row.regionId + ')"><i class="fas fa-trash-alt"></i></a>'
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
    regionId = id
    $('#modal-delete-region').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/Region/Delete" + '?' + $.param({ "Id": regionId }),
        type: 'DELETE',
        success: function (result) {
            regionId = 0;
            if (result.status === 1) {
                $("#regionList").dataTable().fnDestroy();
                getRegionList();
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
    regionId = 0;
}