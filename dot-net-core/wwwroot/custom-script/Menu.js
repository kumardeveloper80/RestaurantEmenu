var menuId = 0;
var selectedStatus;
var refDataTable;
var selectedConceptId = 0;
var selectedMenuIds = null;

$(document).ready(function () {
    selectedStatus = $('#status-select').val();
    selectedConceptId = $('#ConceptId').val();
    getMenuList();
    $('#status-select').change(function () {
        selectedStatus = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.status = selectedStatus
        });
        refDataTable.draw();
    });

    $('#ConceptId').change(function () {
        getMenu();
        selectedConceptId = $(this).val();
        selectedMenuIds = null;
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.conceptId = selectedConceptId
        });
        refDataTable.draw();
    });

    $('#MenuIds').change(function () {
        selectedMenuIds = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.menuIds = selectedMenuIds
        });
        refDataTable.draw();
    });
});

function getMenuList() {
    refDataTable = $("#menuList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/Menu/GetList",
            "type": "POST",
            "datatype": "json",
            "data": {
                "status": selectedStatus,
                "conceptId": selectedConceptId,
                "menuIds": selectedMenuIds,
            }
        },
        "columns": [
            { "data": "code", "name": "Code", "width": "20%" },
            { "data": "name", "name": "Name", "width": "40%" },
            { "data": "conceptName", "name": "ConceptName", "width": "20%" },
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
                    var linkManageSequence = '<a href=CategorySequence/' + row.id + ' title="Manage Sequence"><i class="fas fa-tasks"></i></a>';
                    return '<div style="text-align:center;">' + linkEdit + " &nbsp; " + linkDelete + " &nbsp; " + linkManageSequence +'</div>';
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
    menuId = id
    $('#modal-delete-menu').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/Menu/Delete" + '?' + $.param({ "Id": menuId }),
        type: 'DELETE',
        success: function (result) {
            menuId = 0;
            if (result.status === 1) {
                $("#menuList").dataTable().fnDestroy();
                getMenuList();
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
    menuId = 0;
}

function getMenu() {
    var conceptId = $("#ConceptId").val();
    var url = "/Menu/GetMenu";
    $.getJSON(url, { conceptId: conceptId }, function (data) {
        var item = '<option value="">-Select Menu-</option>';
        $("#MenuIds").empty();
        $.each(data, function (i, menu) {
            item += '<option value="' + menu.value + '">' + menu.text + '</option>'
        });
        $("#MenuIds").html(item);
    });
}