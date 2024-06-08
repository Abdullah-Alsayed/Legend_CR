
var FilterTimeMultiSelectFormCount = 0, SearchIndex = 0;

function moreData() {
    var Search = $("#Search").val();
    if (Search != "") {
        FilterTimeMultiSelectFormCount = 0, SearchIndex++;
        $('#FilterTimeMultiSelectForm').attr('action', `/User/CustomerFollowupPartial?Search=${Search}&PageIndex=${SearchIndex}`);
        $('#FilterTimeMultiSelectForm').submit();
    }
    else {
        FilterTimeMultiSelectFormCount++, SearchIndex = 0;
        $('#FilterTimeMultiSelectForm').attr("action", `/User/CustomerFollowupPartial?PageIndex=${FilterTimeMultiSelectFormCount}`);
        $('#FilterTimeMultiSelectForm').submit();
    }
}
$('#areadIDs').change(function () {
    FilterTimeMultiSelectFormCount = 0;
    $('#FilterTimeMultiSelectForm').attr("action", `/User/CustomerFollowupPartial?PageIndex=${FilterTimeMultiSelectFormCount}`);

})
$('#btnFilter').click(function () {
    FilterTimeMultiSelectFormCount = 0;
    $('#FilterTimeMultiSelectForm').attr("action", `/User/CustomerFollowupPartial?PageIndex=${FilterTimeMultiSelectFormCount}`);
});

function Postpone(shipID, Date, From, To, Note, IsConfirmed, Model, TableTD, TableRow) {
    //$(`#confirmDate_${shipID}`).datepicker();
    url = `/Shipment/Postpone?shipID=${shipID}&Date=${Date}&From=${From}&To=${To}&Note=${Note}&IsConfirmed=${IsConfirmed}`;
    $.ajax({
        type: 'GET',
        url: url,
        async: false,
        success: function (response) {
            $(".form-group :input").val("");
            // $(`#Li_${shipID}`).data("original", `Call History ${response.count}`);
            $(`#${Model}`).on('hidden.bs.modal', function (e) {
                if (IsConfirmed == true) {
                    $(`#${TableTD}`).html('<span class="status-Table Green float-right mr-3">Confirmed</span>');
                }
            })
            if (response.count > 1) {
                $(`#UlComment_${shipID}`).append(`<li class="list-group-item"><span class="text-wrap"> ${Note} </span></li>`);
            }
            else {
                $(`#CommentTd_${shipID}`).html(`<ul id="UlComment_${shipID}" class="list-group list-group-flush"><li class="list-group-item"><span class="text-wrap"> ${Note} </span></li></ul>`)
            }
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: response.messege,
                showConfirmButton: false,
                timer: 2000
            });
        },
        error: function () { alert("error"); }
    });
}

function EditAddress(shipID) {
    let Address = $("#CustomerAddressTxt_" + shipID).val();
    let Location = $("#LocationTxt_" + shipID).val();
    let Landmark = $("#LandmarkTxt_" + shipID).val();
    let Floor = $("#FloorTxt_" + shipID).val();
    let Apartment = $("#ApartmentTxt_" + shipID).val();
    url = `/Shipment/EditAddress?shipID=${shipID}&Address=${Address}&Location=${Location}&Landmark=${Landmark}&Floor=${Floor}&Apartment=${Apartment}`;
    $.ajax({
        type: 'POST',
        url: url,
        async: true,
        success: function (response) {
            $('#divEditAddress_' + shipID).modal('hide');
            $("#CustomerRefAdress_" + shipID).text(response);
            Swal.fire({
                title: response.message,
                icon: 'success',
                showConfirmButton: false
            });
        },
        error: function () { alert("error"); }
    });
}

function CancelOrder(shipID, Comment, TableRow) {
    Swal.fire({
        title: 'Are you sure you want to cancel shipment ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#dc3545',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            let ReasonType = $(`#Reason_${shipID}`).val();
            if (ReasonType == '' || ReasonType == 0) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'Please select cancel reason',
                    showConfirmButton: false,
                    timer: 3000
                });
                return false;
            }
            else {
                let IsTripCompleted = $("#cbxIsTripCompleted_" + shipID).is(":checked");
                $.ajax({
                    type: 'POST',
                    url: `/Shipment/Cancel?shipID=${shipID}&Comment=${Comment}&IsTripCompleted=${IsTripCompleted}&ReasonType=${ReasonType}`,
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
        }
    })
}



function CallHisory(shipID) {
    let CallAnswer = $(`#CallAnswer_${shipID}`).val();
    let CallNotAnswer = $(`#CallNotAnswer_${shipID}`).val();
    $.ajax({
        url: `/Shipment/CallHisory?shipID=${shipID}&CallAnswer=${CallAnswer}&CallNotAnswer=${CallNotAnswer}`,
        type: "GET",
        success: function (response) {
            if (response.success == true) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: response.message,
                    showConfirmButton: false,
                    timer: 3000
                });
            } else {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: response.message,
                    showConfirmButton: false,
                    timer: 3000
                });
            }
        },
        error: function () {

        }
    });
}

function ReturnToVendor(shipID, TableRow) {
    Swal.fire({
        title: 'Are you sure you want to return shipment ?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#dc3545',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'POST',
                url: `/Shipment/ReturnToVendor?shipID=${shipID}`,
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

function AllowTax(selected) {
    var selectedValue = selected.value;
    var selectedId = selected.id;
    var divId = "divTaxCancel_" + selectedId.substring(16);
    if (selectedValue == 2) {
        document.getElementById(divId).hidden = false;
    }
    else {
        document.getElementById(divId).hidden = true;
    }
}

function AllowReturn(selected) {
    var selectedValue = selected.checked;
    var selectedId = selected.id;
    var divId = "divReturn_" + selectedId.substring(10);
    if (selectedValue == true) {
        document.getElementById(divId).hidden = false;
    }
    else {
        document.getElementById(divId).hidden = true;
    }
}

function UnassignedShipmnet(shipID) {
    Swal.fire({
        title: "Are you sure you want to unassign shipment ?!",
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#d33',
        confirmButtonText: "Confirm",
    }).then((result) => {
        if (result.isConfirmed) {
            url = `/Shipment/Unassigned?AdvertisementId=${shipID}`;
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
                        $(`#TableRow_${shipID} .status-Table`).text(`Ready For Delivery | جاهز للتسليم`).removeClass('Blue').addClass('Yellow');
                        $(`#Unassigned_${shipID}`).fadeOut();
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