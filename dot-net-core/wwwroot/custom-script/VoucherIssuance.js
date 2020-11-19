var voucherIssuanceId = 0;
var refDataTable;

$(document).ready(function () {
    getVoucherIssuanceList();
});

function getVoucherIssuanceList() {
    refDataTable = $("#voucherIssuanceList").DataTable({
        "processing": false,
        "serverSide": true,
        "lengthMenu": [[10, 20, 50, -1], [10, 20, 50, "All"]],
        "ordering": true,
        "filter": true,
        "orderMulti": false,
        "ajax": {
            "url": "/VoucherIssuance/GetList",
            "type": "POST",
            "datatype": "json",
        },
        "columns": [
            { "data": "voucherName", "name": "voucherName", "width": "30%" },
            { "data": "customerName", "name": "customerName", "width": "15%" },
            { "data": "categoryName", "name": "categoryName", "width": "15%" },
            { "data": "subCategoryName", "name": "subCategoryName", "width": "15%" },
            { "data": "approvedStatus", "name": "approvedStatus", "width": "15%" },
            {
                "mRender": function (data, type, row) {
                    var linkEdit = '';
                    var linkDelete = '';
                    var linkApproval = '';
                    var ouput = '<div style="text-align:center;">';
                    if (row.hasVoucherIssuancePermission) {
                        linkEdit = '<a href=Index/' + row.id + '><i class="fas fa-edit"></i></a>';
                        linkDelete = '<a href="javascript:" onclick="onDelete(' + row.id + ')"><i class="fas fa-trash-alt"></i></a>';
                        ouput += '<span>' + linkEdit + " &nbsp; " + linkDelete + '</span> &nbsp;';
                    }

                    if (row.hasVoucherApprovalPermission) {
                        linkApproval = '<a href="javascript:" onclick="onApproval(' + row.id + ')"><i class="fas fa-cog"></i></a>';
                        ouput += '<span>' + linkApproval + '</span>';
                    }
                    ouput += "</div>";
                    return ouput;
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
    voucherIssuanceId = id
    $('#modal-delete-voucher-issuance').modal('show');
}

function onDeleteConfirm() {
    $.ajax({
        url: "/VoucherIssuance/Delete" + '?' + $.param({ "Id": voucherIssuanceId }),
        type: 'DELETE',
        success: function (result) {
            voucherIssuanceId = 0;
            if (result.status === 1) {
                $("#voucherIssuanceList").dataTable().fnDestroy();
                getVoucherIssuanceList();
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
    voucherIssuanceId = 0;
}

function onApproval(id) {
    $("#voucherApprovalBody").html('')
    voucherIssuanceId = id;

    $.ajax({
        url: "/VoucherIssuance/GetVoucherStatus?id=" + id,
        type: 'GET',
        success: function (result) {
            debugger
            $("#voucherApprovalBody").html(result)
            $("#modal-voucher-approval").modal('show');
        },
        error: function (request, msg, error) {
            toastr.error(msg)
        }
    });
}

function onSubmit() {
    if ($("input:radio[name='IsApproved']").is(":checked")) {
        if ($('input[type=radio][name=IsApproved]:checked').val() == 'true') {
            var isConfirm = confirm("You are going to approve a voucher, this cannot be undone again.");
            if (isConfirm) {
                fnConfrim(true);
            } else {
                $("#modal-voucher-approval").modal('hide');
            }
        }
        else {
            fnConfrim(false);
        }
    } else {
        alert('Please select any voucher approval status')
    }
}

function fnConfrim(isApproved) {
    debugger;
    $("#modal-voucher-approval").modal('hide');
    var requestData = {
        Id: voucherIssuanceId,
        IsApproved: isApproved,
        ReasonDescription: $("#ReasonDescription").val()
    }
    $("#description").val('');
    voucherIssuanceId = 0;
    $.ajax({
        url: "/VoucherIssuance/Approve",
        type: 'POST',
        data: JSON.stringify(requestData),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.status === 1) {
                $("#voucherIssuanceList").dataTable().fnDestroy();
                getVoucherIssuanceList();
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