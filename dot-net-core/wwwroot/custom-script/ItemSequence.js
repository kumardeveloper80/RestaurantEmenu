var routeURL = location.protocol + '//' + location.host;
const urlParams = new URLSearchParams(window.location.search);
debugger
var menuId = urlParams.get('menuId');
var categoryId = urlParams.get('categoryId');
var menuItemTable;

$(document).ready(function () {
    getMenuItem();
});

function getMenuItem() {
    menuItemTable = $("#menuItemDetails").DataTable({
        "searching": false,
        "paging": false,
        "info": false,
        "ordering": false,
        "lengthChange": false,
        "order": [],
        "ajax": {
            "url": "/Menu/GetMenuItem",
            "type": "POST",
            "datatype": "json",
            "data": {
                "menuId": menuId,
                "categoryId": categoryId
            },
        },
        "columns": [
            { "data": "itemSequence", "name": "ItemSequence", "width": "5%", className: 'reorder' },
            { "data": "menuItemName", "name": "menuItemName", "width": "95%", className: 'text-left' },
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false },
        ],
        rowReorder: {
            dataSrc: 'menuItemId',
            "selector": "td:nth-child(1)",
            update: false,
        },
        "language": {
            "emptyTable": "No data found",
        }
    });
    var data = [];
    menuItemTable.on('row-reorder', function (e, diff, edit) {
        for (var i = 0, ien = diff.length; i < ien; i++) {
            var rowData = menuItemTable.row(diff[i].node).data();
            var obj = {};
            obj.MenuItemId = rowData.menuItemId;
            obj.ItemSequence = diff[i].newPosition + 1;
            obj.MenuId = rowData.menuId;
            data.push(obj);
        }
        if (data.length > 0) {
            updateMenuItemSequence(data);
        }
    });
}


function updateMenuItemSequence(data) {
    $.ajax({
        url: "/Menu/ManageMenuItemSequence",
        type: 'POST',
        data: JSON.stringify(data),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.status === 1) {
                menuItemTable.ajax.reload();
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