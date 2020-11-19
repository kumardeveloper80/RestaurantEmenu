var menuItemId = 0;
var selectedStatus;
var refDataTable;
var selectedConceptId = 0;
var selectedCategoryId = 0;
var selectedMenuItemId = 0;

$(document).ready(function () {
    selectedStatus = $('#status-select').val();
    selectedConceptId = $('#ConceptId').val();
    getMenuItemList();
    $('#status-select').change(function () {
        selectedStatus = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.status = selectedStatus
        });
        refDataTable.draw();
    });

    $('#ConceptId').change(function () {
        getCategory();
        selectedCategoryId = 0;
        selectedConceptId = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.conceptId = selectedConceptId
        });
        refDataTable.draw();
    });

    $('#CategoryId').change(function () {
        getMenuItems();
        selectedCategoryId = $(this).val();
        selectedMenuItemId = 0;
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.categoryId = selectedCategoryId
        });
        refDataTable.draw();
    });

    $('#MenuItemId').change(function () {
        selectedMenuItemId = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.menuItemId = selectedMenuItemId
        });
        refDataTable.draw();
    });
});

function getMenuItemList() {
    refDataTable = $("#menuItemList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/MenuItem/GetList",
            "type": "POST",
            "datatype": "json",
            "data": {
                "status": selectedStatus,
                "conceptId": selectedConceptId,
                "categoryId": selectedCategoryId,
                "menuItemId": selectedMenuItemId
            }
        },
        "columns": [
            { "data": "plu", "name": "PLU", "width": "5%" },
            { "data": "name", "name": "Name", "width": "15%" },
            { "data": "conceptName", "name": "ConceptName", "width": "15%" },
            { "data": "categoryName", "name": "CategoryName", "width": "15%" },
            { "data": "currency", "name": "Currency", "width": "3%" },
            { "data": "price", "name": "Price", "width": "9%" },
            { "data": "labelEN", "name": "LabelEN", "width": "12%" },
            { "data": "labelAR", "name": "LabelAR", "width": "12%" },
            {
                "data": "status", "name": "Status", "width": "9%",
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
    menuItemId = id
    $('#modal-delete-menu-item').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/MenuItem/Delete" + '?' + $.param({ "Id": menuItemId }),
        type: 'DELETE',
        success: function (result) {
            menuItemId = 0;
            if (result.status === 1) {
                $("#menuItemList").dataTable().fnDestroy();
                getMenuItemList();
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
    menuItemId = 0;
}

function getCategory() {
    var conceptId = $("#ConceptId").val();
    var url = "/MenuItem/GetCategory";
    $.getJSON(url, { conceptId: conceptId }, function (data) {
        var item = '<option value="">-Select Category-</option>';
        $("#CategoryId").empty();
        $.each(data.categories, function (i, category) {
            item += '<option value="' + category.value + '">' + category.text + '</option>'
        });

        $("#CategoryId").html(item);

        $("#MenuItemId").empty();
        item = '<option value="">-Select Menu Item--</option>';
        //$.each(data.menuItems, function (i, menuItem) {
        //    item += '<option value="' + menuItem.value + '">' + menuItem.text + '</option>'
        //});

        $("#MenuItemId").html(item);
    });
}

function getMenuItems() {
    var categoryId = $("#CategoryId").val();
    var url = "/MenuItem/GetMenuItems";
    $.getJSON(url, { categoryId: categoryId }, function (data) {
        $("#MenuItemId").empty();
        var item = '<option value="">-Select Menu Item--</option>';
        $.each(data.menuItems, function (i, menuItem) {
            item += '<option value="' + menuItem.value + '">' + menuItem.text + '</option>'
        });
        $("#MenuItemId").html(item);
    });
}