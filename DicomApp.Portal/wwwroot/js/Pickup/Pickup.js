
function GetVendorDetails() {
    var VendorID = $(`#vendorId`).val();
    $.ajax({
        type: 'GET',
        url: `/User/GetUserData?Id=${VendorID}`,
        async: false,
        success: function (response) {
            if (response == false) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'User Not Found',
                    showConfirmButton: false,
                    timer: 4000
                });
            }
            else {
                $(`#vendorId`).val(response.id);
                $(`#VendorName`).val(response.name);
                $(`#FullName`).val(response.fullName);
                $(`#VendorAddress`).val(response.address);
                //$(`#AreaId`).val(response.areaId);
                //$("#select2-AreaId-container").text(response.areaId);
                //$(`#ZoneId`).val(response.zoneId);
                //$("#select2-ZoneId-container").text(response.zoneName);


                $(`#VendorPhone`).val(response.phoneNumber);
                $(`#Email`).val(response.email);
                $(`#VendorLocation`).val(response.locationUrl);
                $(`#VendorLandmark`).val(response.landmark);
                $(`#VendorFloor`).val(response.floor);
                $(`#VendorApartment`).val(response.apartment);

                GetStockItemsForPickup();
            }
        },
        error: function () { alert("error"); }
    });
}

function GetStockItemsForPickup() {
    var VendorID = $(`#vendorId`).val();
    $.ajax({
        type: "GET",
        url: `/PickUpRequest/GetProductList`,
        contentType: 'application/html; charset=utf-8',
        data: { VendorID: VendorID },
        dataType: 'html',
        success: function (result) {
            $('#StockModalBody').html(result);
        },
        error: function (Error) {
            alert(Error)
        }
    });
}

function AddStockPickup(FormID) {
    var IncludeReturns = $('#cbxIncludeReturns').is(':checked');
    var ProductSelect = $("input.ProductSelect:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    var ProductItemCount = $("input.ProductSelect:checkbox:checked").map(function () {
        return $(this).closest("tr").find('.ProductItemCount').val();
    }).get();
    var ProductsPrice = $("input.ProductSelect:checkbox:checked").map(function () {
        return $(this).closest("tr").find('.ProductPrice').val();
    }).get();

    let TimeFrom = $("#TimeFrom").val();
    let TimeTo = $("#TimeTo").val();
    if (`${TimeFrom}:00` > `${TimeTo}:00`) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Last Time Available Less Than Ready Time',
            showConfirmButton: false,
            timer: 5000
        });
        return false;
    }

    let DataForm = $(`#${FormID}`).serialize();
    DataForm = DataForm + "&ProductIDs=" + ProductSelect.toString();
    DataForm = DataForm + "&ProductsQuantity=" + ProductItemCount.toString();
    DataForm = DataForm + "&ProductsPrice=" + ProductsPrice.toString();
    DataForm = DataForm + "&IncludeReturns=" + IncludeReturns;

    //let form = $(`#${FormID}`).serializeArray();
    //let formData = new FormData();

    //for (let data of form)
    //    formData.append(data.name, data.value);
    //formData.append(`ProductIDs`, ProductIDs);
    //formData.append(`Quantity`, Quantity);
    //formData.append(`Price`, Price);

    $(".invalid-feedback").removeClass("d-none");
    $(".form-control , .select2-selection").css({ "border-color": "#D6E4EC" });
    if ($(`#${FormID}`).valid()) {
        if (ProductSelect.length > 0 && ProductItemCount.length > 0) {
            $("#BtnSend").prop('disabled', true);
            $(`.Spinner`).removeClass(`d-none`);
            $.ajax({
                url: `/PickUpRequest/AddStockPickup`,
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                type: 'POST',
                data: DataForm,
                dataType: 'html',
                success: function (response) {
                    $("#BtnSend").prop('disabled', false);
                    $(`.Spinner`).addClass(`d-none`);

                    alertSuccess(response);
                    MenuNavigation(event, 'All', 'PickUpRequest');
                },
                error: function (response) {
                    alertError(response);
                    MenuNavigation(event, 'All', 'PickUpRequest');
                }
            });
        } else {
            alertError(`Select Products`, 6000);
        }
    }
    else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
    }
}

function PickupRequestToggel(fadeOut, fadeIn) {
    let AdvertisementIds = $("tbody td input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    var ValidFlag = ValidateShipmentPickupRequest(AdvertisementIds);
    if (AdvertisementIds.length == 0) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please select shipment(s)',
            showConfirmButton: false,
            timer: 2000
        });
        return false;
    }
    else if (ValidFlag == false) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'You must choose shipment(s) from one Vendor',
            showConfirmButton: false,
            timer: 2000
        });
        return false;
    }
    ToggleFade(fadeOut, fadeIn)
    var Vendor = $(`#${AdvertisementIds[0]}`).data('vendor');
    GetVendorInfo(Vendor);

    $.ajax({
        url: `/PickUpRequest/GetReadyForReturn?VendorID=${Vendor}`,
        type: "GET",
        success: function (result) {
            $("#ReturnedShipments-TableSection").html(result);
        },
        error: function () { }
    });
}

function AddDeliveryPickup(FormID) {
    var IncludeReturns = $('#cbxIncludeReturns').is(':checked');
    let AdvertisementIds = $("tbody td input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    if ($(`#${FormID}`).valid()) {
        let TimeFrom = $("#TimeFrom").val();
        let TimeTo = $("#TimeTo").val();
        if (`${TimeFrom}:00` > `${TimeTo}:00`) {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'Last Time Available Less Than Ready Time',
                showConfirmButton: false,
                timer: 5000
            });
            return false;
        }

        //let DataForm = formSerialize(FormID, true);
        let DataForm = $(`#${FormID}`).serialize();
        DataForm = DataForm + "&AdvertisementIds=" + AdvertisementIds.toString();
        DataForm = DataForm + "&IncludeReturns=" + IncludeReturns;

        $("#PickupRequestBtn").prop('disabled', true);
        $(`.Spinner`).removeClass(`d-none`);
        $.ajax({
            url: `/PickUpRequest/AddDeliveryPickup`,
            type: 'POST',
            data: DataForm,
            success: function (response) {
                PickupRequestToggel('PickupRequest-Div', 'PickupRequest-List');
                $("#PickupRequestBtn").prop('disabled', false);
                $(`.Spinner`).addClass(`d-none`);

                alertSuccess(response);
                MenuNavigation(event, 'All', 'PickUpRequest');
            },
            error: function (response) {
                PickupRequestToggel('PickupRequest-Div', 'PickupRequest-List');
                $("#PickupRequestBtn").prop('disabled', false);
                $(`.Spinner`).addClass(`d-none`);

                alertError(response);
                MenuNavigation(event, 'All', 'PickUpRequest');
            }
        });
    }
    else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
    }
}

function ToggleFade(fadeOut, fadeIn) {
    $(`#${fadeOut}`).fadeOut(700);
    $(`#${fadeIn}`).fadeIn(700);
}

function ShowModal(Id) {
    $(`#${Id}`).modal("show");
}

function HideModal(Id) {
    $(`#${Id}`).modal("hide");
}

function CancelPickup(pickupID, TableRow) {
    Swal.fire({
        title: 'Are you sure you want to cancel pickup ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#dc3545',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: `/PickUpRequest/Cancel?pickupID=${pickupID}`,
                async: true,
                success: function (response) {
                    $(`#${TableRow}`).fadeOut(1000);
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: response.message,
                        showConfirmButton: false,
                        timer: 2000
                    });
                    setTimeout(function () { $(`#${TableRow}`).remove(); }, 3000);
                },
                error: function () { alert("error"); }
            });
        }
    })
}

function GetReadyForReturn() {
    var VendorID = $(`#vendorId`).val();
    $.ajax({
        url: `/PickUpRequest/GetReadyForReturn?VendorID=${VendorID}`,
        type: "GET",
        success: function (result) {
            $("#ReturnedShipments-TableSection").html(result);
        },
        error: function () { }
    });
}

function UnassignedPickup(PickupID) {
    Swal.fire({
        title: "Are you sure you Unassigned the PickUp Request ?!",
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#d33',
        confirmButtonText: "Confirm",
    }).then((result) => {
        if (result.isConfirmed) {
            url = `/PickUpRequest/Unassigned?PickupID=${PickupID}`;
            $.ajax({
                type: 'POST',
                url: url,
                success: function (response) {
                    if (response.success == false) {
                        Swal.fire({
                            title: response.message,
                            icon: 'error',
                            showConfirmButton: false
                        });
                    }
                    else {
                        $(`#TableRow_${PickupID} .status-Table`).text('Ready For Pickup | جاهز للإستلام').removeClass('Blue').addClass('Yellow');
                        $(`#Unassigned_${PickupID}`).fadeOut();
                        Swal.fire({
                            title: response.message,
                            icon: 'success',
                            showConfirmButton: false
                        });
                    }
                },
                error: function () { alert("error"); }
            });
        }
    })
}