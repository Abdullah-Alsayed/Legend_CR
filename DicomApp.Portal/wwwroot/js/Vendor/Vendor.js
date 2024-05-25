
//// Url
////var Path = window.location.pathname.split("/");
////var ControlerName = Path[1];
////var ActionName = Path[2];


//// Change Active Status And Filter By Status Ajax
//let PageIndex = 0;
//function FilterByStatus(ID, Filter) {
//    PageIndex = 0;
//    $("#Shipment-Filter-Ul li").removeClass("Active");
//    $(`#${ID}`).addClass(("Active"));
//    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
//    $("#Ajaxloader").removeClass("d-none");
//    $.ajax({
//        url: `/Vendor/OrderReports?ActionType=Table&Status=${Filter}`,
//        type: "GET",
//        success: function (result) {
//            $("tbody").html(result);
//            $("#Ajaxloader").fadeOut(1000);
//        },
//        error: function () {

//        }
//    });
//}

//// Sort
//function Sort(ControlerName, ActionName)
//{
//    PageIndex = 0;
//    var SortBy = $("#Sort-Btn").data('isdesc');
//    var Status = $(".Active").data('status');
//    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
//    $("#Ajaxloader").removeClass("d-none");
//    $("td").css("color", "#000000");
//    if (SortBy) {
//        $("#Sort-Btn").data('isdesc', false);
//        $("#Sort-Btn").css("background", "#FFFFFF");
//    }
//    else {
//        $("#Sort-Btn").data('isdesc', true);
//        $("#Sort-Btn").css("background", "#ff000030");
//    }
//    $.ajax({
//        url: `/${ControlerName}/${ActionName}?ActionType=Table&IsDesc=${SortBy}&Status=${Status}`,
//        type: "GET",
//        success: function (result) {
//            $("tbody").html(result);
//            $("#Ajaxloader").fadeOut(1000);
//        },
//        error: function (Error) {
//            alert('Error');
//        }
//    });
//}
//// Sort By Column
//function SortColumn(Td, SortByCoulmn, ControlerName, ActionName) {
//    PageIndex = 0;
//    var SortBy = $(`#${Td}`).data('isdesc');
//    var Status = $(".Active").data('status');
//    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
//    $("#Ajaxloader").removeClass("d-none");
//    $("td").css("color", "#000000");
//    $("#Sort-Btn").css("background", "#FFFFFF");
//    if (SortBy) {
//        $(`#${Td}`).data('isdesc', false);
//        $(`#${Td}`).css("color", "#000000");
//    }
//    else {
//        $(`#${Td}`).data('isdesc', true);
//        $(`#${Td}`).css("color", "#4173ea");
//    }
//    $.ajax({
//        url: `/${ControlerName}/${ActionName}?ActionType=Table&IsDesc=${SortBy}&Status=${Status}&OrderByColumn=${SortByCoulmn}`,
//        type: "GET",
//        success: function (result) {
//            $("tbody").html(result);
//            $("#Ajaxloader").fadeOut(1000);
//        },
//        error: function (Error) {
//            alert('Error');
//        }
//    });
//}

//// Search
//function Search(ControlerName, ActionName) {
//    PageIndex = 0;
//    var Key = event.which;
//    if (Key == 13) {
//        var Value = $("#Search-Input").val();
//       /* if (Value != "" && Value != undefined && Value.length > 0) {*/
//            var Status = $(".Active").data('status');
//            $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
//            $("#Ajaxloader").removeClass("d-none");
//            $.ajax({
//                url: `/${ControlerName}/${ActionName}?ActionType=Table&Status=${Status}&Search=${Value}`,
//                type: "GET",
//                success: function (result) {
//                    $("tbody").html(result);
//                    $("#Ajaxloader").fadeOut(1000);
//                },
//                error: function () {

//                }
//            });
//        //} else {
//        //    Swal.fire({
//        //        position: 'center',
//        //        icon: 'error',
//        //        title: 'Search Is Empty',
//        //        showConfirmButton: false,
//        //        timer: 2000
//        //    });
//        //}
//    }
//}

//function ShipmentDetails(ShipmentID) {
//    $.ajax({
//        url: `/Vendor/ShipmentDetails/${ShipmentID}`,
//        type: "GET",
//        success: function (result) {
//            $("#ShipmentDetails-Body").html(result);
//            $('#ShipmentDetails').modal('show');
//        },
//        error: function () {

//        }
//    })
//}

//function GetGame(id) {
//    $.get("/Game/GetGameList", { ID: id }, function (data) {
//        $.each(data, function (index, Game) {
//            $("#Gamediv").append('<div><label for="' + Game.id + '" class="radio-card"><input type="radio" value="' + Game.id + '" name="GameId" id="' + Game.id + '" /><div class="card-content-wrapper"><span class="check-icon"></span><div class="card-content"><img src="/dist/images/' + Game.imgUrl + '" alt=""  width="100" /><p class="Game-Price">' + Game.price +' EGP</p><p class="float-end Game-Name">' + Game.nameEn + '</p><p class="Game-Size">' + Game.size + '<h4/></div></div></label></div>');
//        });
//    });
//    $("#Gamediv").empty();
//}
//function Category(ID)
//{
//    $("#Gamediv").fadeOut(900);
//    GetGame(ID);
//    $("#Gamediv").slideDown(1000);
//}
//function GetTotalPrice() {
//    var Tax;
//    var Amount = $("#SubTotal").val();
//    var AreaID = $("#AreaId").val();
//    var Game = $("input[type='radio']:checked").val();
//    var Weight = $("#Weight").val();
//    $.get(`/vendor/GetTotalPrice/?AreaID=${AreaID}&GameID=${Game}`, function (data) {
//        Tax = data.tax;
//        $("#taxtxt").val(Tax);
//        $("#Zone-name").text(data.zone);
//        var Total = Number(Tax) + Number(Amount) + Number(data.Game);
//        if (Weight > 3) {
//            Total += (Weight - 3) * 5;
//        }
//        if ($('#Partialdelivery').is(':checked'))
//        {
//            Total += 10;
//        }
//        $(".COD").text(Total + " EGP");

//    })
//}

//function SelectAllCheckbox()
//{
//    var checked = $('#Allcheckbox').prop('checked');
//    if (checked) {
//        $("input[type=checkbox]").prop('checked', 'checked');
//    }
//    else {
//        $("input[type=checkbox]").prop('checked', false);

//    }
//}

//function PickupRequestToggel(fadeOut, fadeIn )
//{
//    var ShipmentiDs = $("input:checkbox:checked").map(function () {
//        return $(this).val();
//    }).get()
//    if (ShipmentiDs.length == 0)
//    {
//        Swal.fire({
//            position: 'center',
//            icon: 'error',
//            title: 'Please Select Pickup',
//            showConfirmButton: false,
//            timer: 2000
//        });
//        return false;
//    }
//    $(`#${fadeOut}`).fadeOut(700);
//    $(`#${fadeIn}`).fadeIn(700);
//}
//function PickupRequest() {
//    var ShipmentiDs = $("input:checkbox:checked").map(function () {
//        return $(this).val();
//    }).get()
//    var VendorAddress = $("#VendorAddress").val();
//    var OrderDescription = $("#OrderDescription").val();
//    var AreaId = $("#AreaId").val();
//    var PickupDate = $("#PickupDate").val();
//    var ReadyTime = $("#ReadyTime").val();
//    var LastTimeAvailable = $("#LastTimeAvailable").val();
//    if (AreaId == "")
//    {
//        Swal.fire({
//            position: 'center',
//            icon: 'error',
//            title: 'Please Select Area',
//            showConfirmButton: false,
//            timer: 3000
//        });
//        return false;
//    }
//    if (PickupDate == "") {
//        Swal.fire({
//            position: 'center',
//            icon: 'error',
//            title: 'Please Enter a Date',
//            showConfirmButton: false,
//            timer: 3000
//        });
//        return false;
//    }
//    if (ReadyTime == "") {
//        Swal.fire({
//            position: 'center',
//            icon: 'error',
//            title: 'Please Enter Ready Time',
//            showConfirmButton: false,
//            timer: 3000
//        });
//        return false;
//    }
//    if (LastTimeAvailable == "") {
//        Swal.fire({
//            position: 'center',
//            icon: 'error',
//            title: 'Please Enter Last Time Available',
//            showConfirmButton: false,
//            timer: 3000
//        });
//        return false;
//    }
//    $("#PickupRequestBtn").prop('disabled', true);
//    $.ajax({
//        url: `/Vendor/PickupRequest`,
//        type: "POST",
//        data: {
//            ShipmentIDs: ShipmentiDs,
//            VendorAddress: VendorAddress,
//            OrderDescription: OrderDescription,
//            AreaId: AreaId,
//            PickupDate: PickupDate,
//            ReadyTime: ReadyTime,
//            LastTimeAvailable: LastTimeAvailable
//        },
//        success: function (result) {
//            if (result == "PickupRequest Successfuly")
//            {
//                Swal.fire({
//                    position: 'center',
//                    icon: 'success',
//                    title: result,
//                    showConfirmButton: false,
//                    timer: 3000
//                });
//                for (var i = 0; i < ShipmentiDs.length; i++) {
//                    $(`#${ShipmentiDs[i]}`).fadeOut();
//                }
//                setTimeout(function () {
//                    for (var i = 0; i < ShipmentiDs.length; i++) {
//                        $(`#${ShipmentiDs[i]}`).remove();
//                    }
//                }, 1000);
//                $("#PickupRequestBtn").prop('disabled', false);
//            }
//            else
//            {
//                Swal.fire({
//                    position: 'center',
//                    icon: 'error',
//                    title: 'PickupRequest Failed',
//                    showConfirmButton: false,
//                    timer: 6000
//                });
//                console.log(result);
//            }
//            PickupRequestToggel('PickupRequest-Div', 'PickupRequest-List');
//        },
//        error: function () {
//            alert("Error Faild");
//        }
//    })
//}
//function Changepassword() {
//    if ($("#ChangePassword-Form").valid()) {
//        var Password = $("#Password").val();
//        var NewPassword = $("#NewPassword").val();
//        var ConfirmPassword = $("#ConfirmPassword").val();
//        $.ajax({
//            url: `/Vendor/ChangePassword?ConfirmPassword=${ConfirmPassword}&NewPassword=${NewPassword}&Password=${Password}`,
//            type: "POST",
//            success: function (result) {
//                switch (result) {
//                    case "Password has been changed successfully":
//                        Swal.fire({
//                            position: 'center',
//                            icon: 'success',
//                            title: result,
//                            showConfirmButton: false,
//                            timer: 3000
//                        });
//                       $("#Password").val("");
//                       $("#NewPassword").val("");
//                       $("#ConfirmPassword").val("");
//                        break;
//                    case "New password does not match":
//                        Swal.fire({
//                            position: 'center',
//                            icon: 'error',
//                            title: result,
//                            showConfirmButton: false,
//                            timer: 3000
//                        });
//                        break;
//                    case "Old password is incorrect":
//                        Swal.fire({
//                            position: 'center',
//                            icon: 'error',
//                            title: result,
//                            showConfirmButton: false,
//                            timer: 3000
//                        });
//                        break;
//                    default:
//                }
//            },
//            error: function () {
//                alert("Error")
//            }
//        })
//    } else
//    {
//        $('#ChangePassword-Form').submit();
//    }
//}
//function EditUser() {
//    var Data = $("#EditUser-Form").serialize();
//    $.ajax({
//        url: `/vendor/EditUser`,
//        type: "POST",
//        data: Data,
//        success: function (result)
//        {
//            if (result == "Invalid")
//            {
//                Swal.fire({
//                    position: 'center',
//                    icon: 'error',
//                    title: result,
//                    showConfirmButton: false,
//                    timer: 3000
//                });
//            }
//            else
//            {
//                Swal.fire({
//                    position: 'center',
//                    icon: 'success',
//                    title: result,
//                    showConfirmButton: false,
//                    timer: 3000
//                });
//            }
//        },
//        error: function (result) {
//            Swal.fire({
//                position: 'center',
//                icon: 'error',
//                title: result,
//                showConfirmButton: false,
//                timer: 3000
//            });
//        }
//    })
//}
//function MinimumItemNo(id) {
//    var Value = $(`#${id}`).val();
//    if (Value <= 0) {
//        Swal.fire({
//            position: 'center',
//            icon: 'error',
//            title: "Should Be Number Of Item Bigger Than 0 ",
//            showConfirmButton: false,
//            timer: 4000
//        });
//    }
//}

//function NextPage(ControlerName,ActionName)
//{
//    let Page = ++PageIndex;
//    var Value = $("#Search-Input").val();
//    var Status = $(".Active").data('status');
//    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
//    $("#Ajaxloader").removeClass("d-none");
//    $.ajax({
//        url: `/${ControlerName}/${ActionName}?ActionType=Table&Status=${Status}&Search=${Value}&PageIndex=${Page}`,
//        type: "GET",
//        success: function (result) {
//            $("tbody").html(result);
//            $("#Ajaxloader").fadeOut(1000);
//        },
//        error: function () {

//        }
//    });
//}
//function PreviousPage(ControlerName, ActionName)
//{
//    let Page = 0;
//    if (PageIndex != 0) {
//        Page = --PageIndex;
//    }
//    else
//    {
//        Swal.fire({
//            position: 'center',
//            icon: 'error',
//            title: "This Is The First Page",
//            showConfirmButton: false,
//            timer: 4000
//        });
//        return false;
//    }
//    var Value = $("#Search-Input").val();
//    var Status = $(".Active").data('status');
//    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
//    $("#Ajaxloader").removeClass("d-none");
//    $.ajax({
//        url: `/${ControlerName}/${ActionName}?ActionType=Table&Status=${Status}&Search=${Value}&PageIndex=${Page}`,
//        type: "GET",
//        success: function (result) {
//            $("tbody").html(result);
//            $("#Ajaxloader").fadeOut(1000);
//        },
//        error: function () {

//        }
//    });
//}
//function ShippingCalculator()
//{
//    $("#Calc-Spinner").removeClass("d-none");
//    var ShipmentServiceId = $("#ShipmentServiceId").val();
//    var PackageSize = $("#PackageSize").val();
//    var PackageWeight = $("#PackageWeight").val();
//    var DestinationArea = $("#DestinationArea").val();
//    var SourceArea = $("#SourceArea").val();
//    if ($("#CalcFees-Form").valid()) {
//        $.ajax({
//            url: `/Vendor/ShippingCalculator?ShipmentServiceId=${ShipmentServiceId}&PackageSize=${PackageSize}&PackageWeight=${PackageWeight}&DestinationArea=${DestinationArea}&SourceArea=${SourceArea}`,
//            type: "POST",
//            success: function (result) {
//                $('#Calc-Fees').text(`Shipping fees ${result} EGP`)
//                $("#Calc-Spinner").addClass("d-none");
//            },
//            error: function () {

//            }
//        });
//    } else
//    {
//        $("#CalcFees-Form").submit();
//        $("#Calc-Spinner").addClass("d-none");

//    }
//}


