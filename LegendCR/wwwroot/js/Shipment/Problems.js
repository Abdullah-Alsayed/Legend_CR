
function AddProblem(shipID, note, TableRow, ProblemTypeID) {
    if (ProblemTypeID == '' || ProblemTypeID == 0) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please select problem type',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    else {
        $.ajax({
            type: 'GET',
            url: `/Shipment/AddProblem?shipID=${shipID}&Note=${note}&ProblemTypeID=${ProblemTypeID}`,
            async: false,
            success: function (response) {

                if (response.success == false) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: response.message,
                        showConfirmButton: true
                    });
                }
                else {
                    $(`#${TableRow}`).fadeOut(1000);
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: response.message,
                        showConfirmButton: false,
                        timer: 2000
                    });
                    setTimeout(function () { $(`#${TableRow}`).remove(); }, 3000);
                }

            },
            error: function () { alert("error"); }
        });
    }
}

function SolveProblem(shipID, Solution) {
    $.ajax({
        type: 'GET',
        url: `/Shipment/SolveProblem?shipID=${shipID}&Solution=${Solution}`,
        async: false,
        success: function (response) {
            $(`#Actions_${shipID}`).removeClass(`d-none`);
            $(`#ProblemDev_${shipID}`).removeClass(`d-flex`);
            $(`#ProblemDev_${shipID}`).addClass(`d-none`);
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: response,
                showConfirmButton: false,
                timer: 2000
            });
        },
        error: function () { alert("error"); }
    });
}



function DeleteProblem(shipID) {
    Swal.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#dc3545',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'Get',
                url: `/Shipment/DeleteProblem?shipID=${shipID}`,
                async: true,
                success: function (response) {
                    $(`#Actions_${shipID}`).removeClass(`d-none`);
                    $(`#ProblemDev_${shipID}`).removeClass(`d-flex`);
                    $(`#ProblemDev_${shipID}`).addClass(`d-none`);
                    Swal.fire({
                        title: response,
                        icon: 'success',
                        showConfirmButton: false,
                    })
                },
                error: function () { alert("error"); }
            });
        }
    })
}

function ReportToVendor(shipID, Note, ProblemTypeID, IsCoruierProblem) {
    if (ProblemTypeID == '' || ProblemTypeID == 0) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please select problem type',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    else {
        $.ajax({
            type: 'GET',
            url: `/Shipment/ReportToVendor?shipID=${shipID}&Note=${Note}&ProblemTypeID=${ProblemTypeID}&IsCoruierProblem=${IsCoruierProblem}`,
            async: false,
            success: function (response) {
                var VendorProblemNote = $(`#VendorProblemNote_${shipID}`).val();
                var VendorReason = $(`#ddlVendorReason_${shipID} :selected`).text();
                $(`#ReportToVendor_${shipID}`).html(`<b>${VendorReason}</b>  ${VendorProblemNote}`);

                if (response.success == false) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: response.message,
                        showConfirmButton: true
                    });
                }
                else {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: response.message,
                        showConfirmButton: false,
                        timer: 2000
                    });
                }
            },
            error: function () { alert("error"); }
        });
    }
}

function SolveCourierProblem(shipID, Solution) {
    $.ajax({
        type: 'GET',
        url: `/Shipment/SolveCourierProblem?shipID=${shipID}&Solution=${Solution}`,
        async: false,
        success: function (response) {
            $(`#Actions_${shipID}`).removeClass(`d-none`);
            $(`#CourierProblemDev_${shipID}`).removeClass(`d-flex`);
            $(`#CourierProblemDev_${shipID}`).addClass(`d-none`);
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: response,
                showConfirmButton: false,
                timer: 2000
            });
        },
        error: function () { alert("error"); }
    });
}

function DeleteCourierProblem(shipID) {
    Swal.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#dc3545',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'Get',
                url: `/Shipment/DeleteCourierProblem?shipID=${shipID}`,
                async: true,
                success: function (response) {
                    $(`#Actions_${shipID}`).removeClass(`d-none`);
                    $(`#CourierProblemDev_${shipID}`).removeClass(`d-flex`);
                    $(`#CourierProblemDev_${shipID}`).addClass(`d-none`);
                    Swal.fire({
                        title: response,
                        icon: 'success',
                        showConfirmButton: false,
                    })
                },
                error: function () { alert("error"); }
            });
        }
    })
}

function ReplyToCourier(shipID, Solution) {
    $.ajax({
        type: 'GET',
        url: `/Shipment/ReplyToCourier?shipID=${shipID}&Solution=${Solution}`,
        async: false,
        success: function (response) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: response,
                showConfirmButton: false,
                timer: 2000
            });
        },
        error: function () { alert("error"); }
    });
}