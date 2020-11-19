var storeGuid;
$(document).ready(function () {
    storeGuid = $("#StoreId").val();
    getSurveyList();

    $("#StoreId").on('change', function () {
        storeGuid = $(this).val();
        $("#surveyList").dataTable().fnDestroy();
        getSurveyList();
    });
});


function getSurveyList() {
    $("#surveyList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/Survey/GetList/",
            "type": "POST",
            "datatype": "json",
            "data": {
                "storeGuid": storeGuid,
            }
        },
        "columns": [
            { "data": "firstName", "name": "FirstName", "width": "25%" },
            { "data": "lastName", "name": "LastName", "width": "25%" },
            { "data": "gender", "name": "Gender", "width": "10%" },
            { "data": "email", "name": "Email", "width": "20%" },
            { "data": "mobile", "name": "Mobile", "width": "15%" },
            {
                "mRender": function (data, type, row) {
                    var linkDetails = '<a href=Index/?storeid=' + storeId + '&&id=' + row.ccR_ID + '><i class="fas fa-eye"></i></a>';
                    return '<div style="text-align:center;">' + linkDetails + '</div>';
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