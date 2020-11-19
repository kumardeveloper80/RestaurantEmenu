var routeURL = location.protocol + '//' + location.host;
var url = window.location.pathname;
var menuId = url.substring(url.lastIndexOf('/') + 1);
var categoryTable;

$(document).ready(function () {
    getMenuCategory();
});

function getMenuCategory() {
    categoryTable = $("#categoryDetails").DataTable({
        "searching": false,
        "paging": false,
        "info": false,
        "ordering": false,
        "lengthChange": false,
        "order": [],
        "ajax": {
            "url": "/Menu/GetMenuCategory",
            "type": "POST",
            "datatype": "json",
            "data": {
                "menuId": menuId
            }
        },
        "columns": [
            { "data": "categorySequence", "name": "CategorySequence", "width": "5%", className: 'reorder' },
            { "data": "categoryName", "name": "CategoryName", "width": "70%", className: 'text-left' },
            {
                "mRender": function (data, type, row) {
                    var linkManageItem = '<a href=' + routeURL + '/Menu/ItemSequence?menuId=' + menuId + '&categoryId=' + row.categoryId +'>Manage Menu Item Sequence</a>';
                    return '<div style="text-align:center;">' + linkManageItem + '</div>';
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false },
        ],
        rowReorder: {
            dataSrc: 'categoryId',
            "selector": "td:nth-child(1)",
            update: false,
        },
        "language": {
            "emptyTable": "No data found",
        }
    });
    var data = [];
    categoryTable.on('row-reorder', function (e, diff, edit) {
        for (var i = 0, ien = diff.length; i < ien; i++) {
            var rowData = categoryTable.row(diff[i].node).data();
            var obj = {};
            obj.CategoryId = rowData.categoryId;
            obj.CategorySequence = diff[i].newPosition + 1;
            obj.MenuId = rowData.menuId;
            data.push(obj);
        }
        if (data.length > 0) {
            updateCategorySequence(data);
        }
    });
}

function updateCategorySequence(data) {
    $.ajax({
        url: "/Menu/ManageCategorySequence",
        type: 'POST',
        data: JSON.stringify(data),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.status === 1) {
                categoryTable.ajax.reload();
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
