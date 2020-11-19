var menuScheduleId = 0;
var selectedStatus;
var refDataTable;
var selectedConceptId = 0;
var selectedStoreId = 0;
var selectedMenuId = 0;

$(document).ready(function () {
    selectedStatus = $('#status-select').val();
    selectedConceptId = $('#ConceptId').val();
    getMenuScheduleList();
    $('#status-select').change(function () {
        selectedStatus = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.status = selectedStatus
        });
        refDataTable.draw();
    });

    $('#ConceptId').change(function () {
        getMenu();
        selectedMenuId = 0;
        selectedConceptId = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.conceptId = selectedConceptId
        });
        refDataTable.draw();
    });

    $('#Store_ID').change(function () {
        selectedStoreId = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.storeId = selectedStoreId
        });
        refDataTable.draw();
    });

    $('#MenuId').change(function () {
        selectedMenuId = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.menuId = selectedMenuId
        });
        refDataTable.draw();
    });
});

function getMenuScheduleList() {
    refDataTable = $("#menuScheduleList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "aaSorting": [[4, 'desc']],
        "ajax": {
            "url": "/MenuSchedule/GetList",
            "type": "POST",
            "datatype": "json",
            "data": {
                "status": selectedStatus,
                "conceptId": selectedConceptId,
                "storeId": selectedStoreId,
                "menuId": selectedMenuId
            }
        },
        "columns": [
            { "data": "code", "name": "Code", "width": "10%" },
            { "data": "menuName", "name": "MenuName", "width": "15%" },
            { "data": "conceptName", "name": "ConceptName", "width": "10%" },
            { "data": "storeName", "name": "StoreName", "width": "15%" },
            { "data": "startDate", "name": "StartDate", "width": "15%" },
            { "data": "endDate", "name": "EndDate", "width": "15%" },
            {
                "data": "status", "name": "Status", "width": "5%",
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
                    var preview = '<a href=' + row.url + ' target="_blank"><i class="fas fa-eye"></i></a>'
                    return '<div style="text-align:center;">' + linkEdit + " &nbsp; " + linkDelete + " &nbsp; " + preview + '</div>';
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
    menuScheduleId = id
    $('#modal-delete-menu-schedule').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/MenuSchedule/Delete" + '?' + $.param({ "Id": menuScheduleId }),
        type: 'DELETE',
        success: function (result) {
            menuScheduleId = 0;
            if (result.status === 1) {
                $("#menuScheduleList").dataTable().fnDestroy();
                getMenuScheduleList();
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
    menuScheduleId = 0;
}

function getMenu() {
    var conceptId = $("#ConceptId").val();
    var url = "/MenuSchedule/GetMenuList";
    $.getJSON(url, { conceptId: conceptId }, function (data) {
        var item = '<option value="">-Select Menu-</option>';
        $("#MenuId").empty();
        $.each(data, function (i, menu) {
            item += '<option value="' + menu.value + '">' + menu.text + '</option>'
        });

        $("#MenuId").html(item);
    });
}