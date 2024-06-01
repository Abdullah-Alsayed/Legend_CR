
function ReceiveCashCourierChange() {
    $("#dvcodes").fadeOut('fast');
    $(".spinner-border").toggleClass('d-none');
    $(".spinner-border").toggleClass('d-flex');
    var DeliveryManId = $('#CourierChangeval').val();
    $.ajax({
        url: "/Cash/GetCourierCodes?DeliveryManId=" + DeliveryManId,
        type: "GET",
        success: function (result) {
            $("#dvcodes").html(result);
            $("#dvcodes").fadeIn('fast');
            $(".spinner-border").toggleClass('d-none');
            $(".spinner-border").toggleClass('d-flex');
            $("#Ajaxloader").fadeOut(1000);
        },
        error: function () { }
    });
}

function ReceiveCash() {
    var DeliveryManId = $('#CourierChangeval').val();
    var FinalTotal = $('#FinalTotal').val();
    if (DeliveryManId == '' || DeliveryManId == 0) {
        alertError('Please select courier');
        return false;
    }
    else if (FinalTotal <= 0) {
        alertError('No cash to receive');
        return false;
    }
    else {
        Swal.fire({
            title: 'Are you sure you want to receive ' + FinalTotal + ' EGP from courier ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#1ee0ac',
            cancelButtonColor: '#dc3545',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.isConfirmed) {
                $("#btnReceiveCash").attr("disabled", true);
                $(`.Spinner`).removeClass("d-none");
                $.ajax({
                    url: `/Cash/ReceiveCash`,
                    type: "GET",
                    data: { DeliveryManId: DeliveryManId },
                    success: function (response) {
                        $("#btnReceiveCash").attr("disabled", false);
                        $(`.Spinner`).addClass("d-none");
                        ReceiveCashCourierChange();

                        if (response.success == true) {
                            alertSuccess(response.message);
                        }
                        else {
                            alertError(response.message);
                        }
                        MenuNavigation(event, 'Treasury', 'Cash');
                    },
                    error: function () {
                        alertError(response.message);
                        MenuNavigation(event, 'Treasury', 'Cash');
                    }
                });
            }
        })
    }
}

function CashTransferVendorChange() {
    var VendorId = $('#VendorChangeVal').val();

    //*** Load Vendor Details Data
    $.ajax({
        url: `/Cash/GetAccountDetails`,
        type: "GET",
        data: { Id: VendorId },
        success: function (result) {
            $(`#Name`).val(result.name);
            $(`#Phone`).val(result.phoneNumber);
            $(`#Address`).val(result.address);
            $(`#Email`).val(result.email);
            $(`#Landmark`).val(result.landmark);
            $(`#Floor`).val(result.floor);
            $(`#Apartment`).val(result.apartment);

            //$(`#ZoneId`).val(result.zoneId);
            //GetAreaByZoneID();
            //$(`#AreaId`).val(result.AreaId);
        },
        error: function () { }
    });

    //*** Load Vendor UnPaid Shipments
    $.ajax({
        url: "/Cash/GetVendorShipments?VendorId=" + VendorId,
        type: "GET",
        success: function (result) {
            $("#Table_dvcodes").html(result);
            $("#Table_dvcodes").fadeIn('fast');
        },
        error: function () { }
    });
}

function GetAreaByZoneID() {
    var ZoneID = $('#ZoneId').val();
    $(`#AreaId , [name='AreaId']`).empty();
    $(`#AreaId , [name='AreaId']`).append(`<option value>--- Select Area ---</option>`);
    $.get("/Admin/GetAreasByZone", { ID: ZoneID }, function (data) {
        $.each(data, function (key, value) {
            $(`#AreaId , [name='AreaId']`).append(`<option value="${value.id}">${value.cityNameAr}</option>`);
        });
    });
}

function TransferCash() {
    var VendorId = $('#VendorChangeVal').val();
    var FinalTotal = $('#lblFinalAmount').text();
    if (VendorId == '' || VendorId == 0) {
        alertError('Please select vendor');
        return false;
    }
    if (FinalTotal <= 0) {
        alertError('Transfer amount is less than or equal zero');
        return false;
    }
    var TransIDs = $("#Table_dvcodes input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    if (TransIDs.length == 0) {
        alertError('No shipment(s) found');
        return false;
    }
    if ($("#CashTransferForm").valid()) {
        let DataForm = $(`#CashTransferForm`).serialize();
        DataForm = DataForm + "&TransIDs=" + TransIDs.toString();
        Swal.fire({
            title: 'Are you sure you want to transfer cash to vendor ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#1ee0ac',
            cancelButtonColor: '#dc3545',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.isConfirmed) {
                $("#btnTransferCash").attr("disabled", true);
                $('#MainLoder').fadeIn(100);
                $("#MainView").hide();
                $.ajax({
                    url: `/Cash/TransferCash`,
                    type: 'POST',
                    data: DataForm,
                    success: function (response) {
                        if (response.success == true) {
                            alertSuccess(response.message);
                        }
                        else {
                            alertError(response.message);
                        }
                        $('#MainLoder').fadeOut(1000);
                        $("#MainView").show();
                        MenuNavigation(event, 'Invoices', 'Cash');                    },
                    error: function (response) {
                        alertError(response.message);
                        $('#MainLoder').fadeOut(1000);
                        $("#MainView").show();
                        MenuNavigation(event, 'Invoices', 'Cash');
                    }
                });
            }
        })
    }
    else {
        alertError('Please enter data');
    }
}



function TotalCashTransferInvoice() {
    var Total = 0;
    var CancelFees = 0;
    var InvoiceiDs = $("input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    for (var i = 0; i < InvoiceiDs.length; i++) {
        if ($(`#${InvoiceiDs[i]} .Invoices-Type`).val() == 1 || $(`#${InvoiceiDs[i]} .Invoices-Type`).val() == "1") {
            Total += Number($(`#${InvoiceiDs[i]} .Invoices-TotalPrice`).text());
        }
        else {
            CancelFees += Number($(`#${InvoiceiDs[i]} .Invoices-TotalPrice`).text());
        }
    }
    $(`#CashTransfer-Total`).removeClass(`d-none`);
    $(`#CashTransfer-Cancel`).removeClass(`d-none`);
    $(`#CashTransfer-SubTotal`).removeClass(`d-none`);

    $(`#CashTransfer-Total span`).text(`${Total} EGP`);
    $(`#CashTransfer-Cancel span`).text(`${CancelFees} EGP`);
    var sub = Total - CancelFees;
    $(`#CashTransfer-SubTotal span`).text(`${sub} EGP`);
}