var conceptsId = 0;
var refDataTable;

$(document).ready(function () {
    getConceptThemeList();
});

function getConceptThemeList() {
    refDataTable = $("#conceptThemeList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/ConceptTheme/GetList",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "conceptName", "name": "ConceptName", "width": "60%" },
            {
                "data": "colorCode", "name": "ColorCode", "width": "10%",
                "mRender": function (data, type, row) {
                    if (row.colorCode === "-") {
                        return '<div class="text-center">-</div>';
                    } else {
                        return '<div class="text-center"><div class="w-h-25" style="background-color:' + row.colorCode+';display: inline-table"></div></div>';
                    }

                }
            },
            {
                "data": "logoName", "name": "LogoName", "width": "10%",
                "mRender": function (data, type, row) {
                    if (row.logoName === "-") {
                        return '<div class="text-center">-</div>';
                    } else {
                        return '<div class="text-center"><img src="' + row.logoName + '" class="w-h-25"/></div>';
                    }

                }
            },
            {
                "data": "feedBackIconName", "name": "FeedBackIconName", "width": "10%",
                "mRender": function (data, type, row) {
                    if (row.feedBackIconName === "-") {
                        return '<div class="text-center">-</div>';
                    } else {
                        return '<div class="text-center"><img src="' + row.feedBackIconName + '" class="w-h-25"/></div>';
                    }

                }
            },
            {
                "mRender": function (data, type, row) {
                    var linkEdit = '<a href=Index/' + row.conceptId + '><i class="fas fa-edit"></i></a>';
                    var linkDelete = '<a href="javascript:" onclick="onDelete(' + row.conceptId + ')"><i class="fas fa-trash-alt"></i></a>'
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
    conceptsId = id
    $('#modal-delete-concept-theme').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/ConceptTheme/Delete" + '?' + $.param({ "Id": conceptsId }),
        type: 'DELETE',
        success: function (result) {
            conceptsId = 0;
            if (result.status === 1) {
                $("#conceptThemeList").dataTable().fnDestroy();
                getConceptThemeList();
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
    conceptsId = 0;
}