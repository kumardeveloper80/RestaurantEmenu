var categoryId = 0;
var selectedStatus;
var refDataTable;
var selectedConceptId = 0;
var selectedNameIds = null;
var selectedLabelENIds = null;
var selectedLabelARIds = null;

$(document).ready(function () {
    selectedStatus = $('#status-select').val();
    selectedConceptId = $('#ConceptId').val();
    getCategoryList();
    $('#status-select').change(function () {
        selectedStatus = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.status = selectedStatus
        });
        refDataTable.draw();
    });

    $('#ConceptId').change(function () {
        getCategory();
        selectedNameIds = null;
        selectedLabelENIds = null;
        selectedLabelARIds = null;
        selectedConceptId = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.conceptId = selectedConceptId
        });
        refDataTable.draw();
    });

    $('#NameIds').change(function () {
        selectedNameIds = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.nameIds = selectedNameIds
        });
        refDataTable.draw();
    });

    $('#LabelENIds').change(function () {
        selectedLabelENIds = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.labelENIds = selectedLabelENIds
        });
        refDataTable.draw();
    });

    $('#LabelARIds').change(function () {
        selectedLabelARIds = $(this).val();
        refDataTable.on('preXhr.dt', function (e, settings, data) {
            data.labelARIds = selectedLabelARIds
        });
        refDataTable.draw();
    });
});

function getCategoryList() {
    refDataTable = $("#categoryList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/Category/GetList",
            "type": "POST",
            "datatype": "json",
            "data": {
                "status": selectedStatus,
                "conceptId": selectedConceptId,
                "nameIds": selectedNameIds,
                "labelENIds": selectedLabelENIds,
                "labelARIds": selectedLabelARIds,
            }
        },
        "columns": [
            { "data": "code", "name": "code", "width": "5%" },
            { "data": "name", "name": "Name", "width": "25%" },
            { "data": "conceptName", "name": "ConceptName", "width": "16%" },
            { "data": "labelEN", "name": "LabelEN", "width": "17%" },
            { "data": "labelAR", "name": "LabelAR", "width": "17%" },
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
    categoryId = id
    $('#modal-delete-category').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/Category/Delete" + '?' + $.param({ "Id": categoryId }),
        type: 'DELETE',
        success: function (result) {
            categoryId = 0;
            if (result.status === 1) {
                $("#categoryList").dataTable().fnDestroy();
                getCategoryList();
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
    categoryId = 0;
}

function getCategory() {
    var conceptId = $("#ConceptId").val();
    var url = "/Category/GetCategory";
    $.getJSON(url, { conceptId: conceptId }, function (data) {
        var name = '<option value="">-Select Name-</option>';
        var lableEN = '<option value="">-Select Label(EN)-</option>';
        var labelAR = '<option value="">-Select Label(AR)-</option>';

        $("#NameIds").empty();
        $("#LabelENIds").empty();
        $("#LabelARIds").empty();
        $.each(data, function (i, itemTag) {
            name += '<option value="' + itemTag.id + '">' + itemTag.name + '</option>'
            lableEN += '<option value="' + itemTag.id + '">' + itemTag.labelEN + '</option>'
            labelAR += '<option value="' + itemTag.id + '">' + itemTag.labelAR + '</option>'
        });
        $("#NameIds").html(name);
        $("#LabelENIds").html(lableEN);
        $("#LabelARIds").html(labelAR);
    });
}