﻿
let PageIndex = 0;
function ResetTdSort() {
    $(`.Sort-Column`).css("color", "#EEF0F4");
}

function SortClintSide() {
    $('.Sort-Column').each(function (col) {
        $(this).addClass('asc');

        $(this).click(function () {

            if ($(this).is('.asc')) {
                $(this).removeClass('asc');
                $(this).addClass('desc selected');
                $(this).css("color", "#4173ea");
                sortOrder = -1;
            } else {
                $(this).addClass('asc selected');
                $(this).removeClass('desc');
                $(this).css("color", "#EEF0F4");

                sortOrder = 1;
            }
            $(this).siblings().removeClass('asc selected');
            $(this).siblings().removeClass('desc selected');
            $(this).siblings().css("color", "#EEF0F4");

            var arrData = $('table').find('tbody >tr:has(td)').get();
            arrData.sort(function (a, b) {
                var val1 = $(a).children('td').eq(col).text().toUpperCase();
                var val2 = $(b).children('td').eq(col).text().toUpperCase();
                if ($.isNumeric(val1) && $.isNumeric(val2))
                    return sortOrder == 1 ? val1 - val2 : val2 - val1;
                else
                    return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
            });
            $.each(arrData, function (index, row) {
                $('tbody').append(row);
            });
        });
    });

}

function Toglebutton(shipID) {
    document.querySelector(`#button-list_${shipID}`).classList.toggle('active');
}

function NextPage(ControllerName, ActionName, Filter) {
    let Page = ++PageIndex;
    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
    $("#Ajaxloader").removeClass("d-none");
    $.ajax({
        url: `/${ControllerName}/${ActionName}?ActionType=Table&StatusId=${Filter}&PageIndex=${Page}`,
        type: "GET",
        success: function (result) {
            if (ActionName == "All") {
                $("#Table").html(result);
            }
            else {
                $("tbody").html(result);
            }
            ResetTdSort();
            $("#Ajaxloader").fadeOut(1000);
            let DataCount = $(`#DataCount`).val();
            $(`#DataCount-Span`).text(`Showing : ${DataCount}`);
        },
        error: function () { }
    });
}

function PreviousPage(ControllerName, ActionName, Filter) {
    let Page = 0;
    if (PageIndex != 0) {
        Page = --PageIndex;
    }
    else {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: "This Is The First Page",
            showConfirmButton: false,
            timer: 4000
        });
        return false;
    }
    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
    $("#Ajaxloader").removeClass("d-none");
    $.ajax({
        url: `/${ControllerName}/${ActionName}?ActionType=Table&StatusId=${Filter}&PageIndex=${Page}`,
        type: "GET",
        success: function (result) {
            if (ActionName == "All") {
                $("#Table").html(result);
            }
            else {
                $("tbody").html(result);
            }
            ResetTdSort();
            $("#Ajaxloader").fadeOut(1000);
            var DataCount = $(`#DataCount`).val();
            $(`#DataCount-Span`).text(`Showing : ${DataCount}`);
        },
        error: function () { }
    });
}

function Filter(ActionName, ControllerName) {
    PageIndex = 0;
    var Url = GetPramter(ControllerName, ActionName, ControllerName == "Gamer" && ActionName == "All" ? 'PartialView' :'Table' );
    if (ControllerName != "Gamer" && ActionName != "All")
     {
        $("#Ajaxloader").css("display", "inline-block").fadeIn(500)
        $("#Ajaxloader").removeClass("d-none");
    } else {
        $("#AllAccountsList-Section").fadeOut(500);
        $(".Account-Spinner").css("display", "inline-block").fadeIn(500);
        $(".Account-Spinner").removeClass("d-none");
    }
    $.ajax({
        url: Url,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (ControllerName == "Gamer" && ActionName == "All")
            {
                $("#AllAccountsList-Section").fadeIn(500);
                $("#AllAccountsList-Section").html(result);
            }
            else if  (ActionName == "All") {
                $("#Table").html(result);
            }
            else {
                $("tbody").html(result);
            }
            ResetTdSort();
            $("#Ajaxloader").fadeOut(500);
            $(".Account-Spinner").fadeOut(500);
            var DataCount = $(`#DataCount`).val();
            $(`#DataCount-Span`).text(`Showing : ${DataCount}`);
        },
        error: function () { }
    });
}

function FilterByStatus(ActionName, ControllerName, ID, Filter) {
    PageIndex = 0;
    $("#Shipment-Filter-Ul li").removeClass("Active");
    let Search = $("#Search-Input").val();
    $(`#${ID}`).addClass(("Active"));
    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
    $("#Ajaxloader").removeClass("d-none");

    $.ajax({
        url: `/${ControllerName}/${ActionName}?ActionType=Table&Search=${Search}&StatusType=${Filter}`,
        type: "GET",
        success: function (result) {
            if  (ActionName == "All") {
                $("#Table").html(result);
            }
            else {
                $("tbody").html(result);
            }
            ResetTdSort();
            $("#Ajaxloader").fadeOut(1000);
            let DataCount = $(`#DataCount`).val();
            $(`#DataCount-Span`).text(`Showing : ${DataCount}`);
        },
        error: function () { }
    });
}

function Sort(ControllerName, ActionName) {
    PageIndex = 0;
    var Url = GetPramter(ControllerName, ActionName, 'Table');
    var SortBy = $("#Sort-Btn").data('isdesc');
    Url += `&IsDesc=${SortBy}`;
    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
    $("#Ajaxloader").removeClass("d-none");
    $("td").css("color", "##EEF0F4");
    if (SortBy) {
        $("#Sort-Btn").data('isdesc', false);
        $("#Sort-Btn").css("background", "#FFFFFF");
    }
    else {
        $("#Sort-Btn").data('isdesc', true);
        $("#Sort-Btn").css("background", "#ff000030");
    }
    $.ajax({
        url: Url,
        type: "GET",
        success: function (result) {
            $("tbody").html(result);
            $("#Ajaxloader").fadeOut(1000);
            var DataCount = $(`#DataCount`).val();
            $(`#DataCount-Span`).text(`Showing : ${DataCount}`);
        },
        error: function (Error) {
            alert('Error');
        }
    });
}

function SortColumn(Td, SortByCoulmn, ControllerName, ActionName) {
    PageIndex = 0;
    var SortBy = $(`#${Td}`).data('isdesc');
    var Url = GetPramter(ControllerName, ActionName, 'Table');
    Url += `&IsDesc=${SortBy}&OrderByColumn=${SortByCoulmn}`;
    console.log(Url)
    $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
    $("#Ajaxloader").removeClass("d-none");
    $("td").css("color", "##EEF0F4");
    $("#Sort-Btn").css("background", "#FFFFFF");
    if (SortBy) {
        $(`#${Td}`).data('isdesc', false);
        $(`#${Td}`).css("color", "#4173ea");
    }
    else {
        $(`#${Td}`).data('isdesc', true);
        $(`#${Td}`).css("color", "##EEF0F4");
    }
    $.ajax({
        url: Url,
        type: "GET",
        success: function (result) {
            if (ActionName == "All") {
                $("#Table").html(result);
            }
            else {
                $("tbody").html(result);
            }
            $("#Ajaxloader").fadeOut(1000);
            var DataCount = $(`#DataCount`).val();
            $(`#DataCount-Span`).text(`Showing : ${DataCount}`);
        },
        error: function (Error) {
            alert('Error');
        }
    });
}

function Search(ControllerName, ActionName) {
    var Status = $(".Active").data('status');
    PageIndex = 0;
    var Key = event.which;
    if (Key == 13) {
        var Value = $("#Search-Input").val();
        $("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
        $("#Ajaxloader").removeClass("d-none");
        $.ajax({
            url: `/${ControllerName}/${ActionName}?ActionType=Table&StatusId=${Status}&Search=${Value}`,
            type: "GET",
            success: function (result) {
                if (ActionName == "All") {
                    $("#Table").html(result);
                }
                else {
                    $("tbody").html(result);
                }
                ResetTdSort();
                $("#Ajaxloader").fadeOut(1000);
                var DataCount = $(`#DataCount`).val();
                $(`#DataCount-Span`).text(`Showing : ${DataCount}`);
            },
            error: function () { }
        });
    }
}

var MenuNavigationList = document.querySelectorAll(`.Navigation`);

MenuNavigationList.forEach((item) => {
    item.addEventListener("click", MenuNavigation);
});

function displayPermissions(className) {

    $('.nav-link').removeClass('active');
    $('.todo-item').fadeOut('fast');
    $('.todo-item[data-ClassName="' + className + '"]').fadeIn('fast');
    $('.nav-link[data-todotype="' + className + '"]').addClass('active');
}


function MenuNavigation(event, ActionName, ControllerName, paramters = "") {
    if (ControllerName == "User" && ActionName == "Authorization")
        window.location.href = '/User/Authorization';
    else {

        event.preventDefault();
        if (event.target.tagName === "A") {
            ControllerName = event.target.href.split('/')[3];
            ActionName = event.target.href.split('/')[4];
        }
        else if (event.target.tagName === "IMG") {
            ControllerName = event.target.baseURI.split('/')[3];
            ActionName = event.target.baseURI.split('/')[4];
        }
        if (paramters != "")
            window.history.pushState(null, null, `/${ControllerName}/${ActionName}?${paramters}`);
        else
            window.history.pushState(null, null, `/${ControllerName}/${ActionName}`);

        $('#MainLoder').fadeIn(100);
        $('#Footer').hide();
        $("#MainView").hide();
        $.ajax({
            type: "GET",
            url: `/${ControllerName}/${ActionName}?${paramters}`,
            contentType: 'application/html; charset=utf-8',
            data: { ActionType: "PartialView" },
            dataType: 'html',
            success: function (result) {
                $(".Nested-sidebar-menu").addClass("d-none");
                $('.cursor-pointer').removeClass('active');
                SelectNavActive();
                $('#MainView').html(result);
                $("#MainView").fadeIn(1000);
                $('#MainLoder').fadeOut(1000);
                $(".se-pre-con").css("display", "none")
                ValidationForm();
                SortClintSide();
            },
            error: function (Error) {
                alert(Error)
            }
        });
    }
}

$(document).ready(function () {
    SelectNavActive();
    SortClintSide();

    let NavigationList = document.querySelectorAll(`#NavigationList .cursor-pointer ul`);
    for (var i = 0; i < NavigationList.length; i++) {
        if (NavigationList[i].children.length == 0) {
            NavigationList[i].parentNode.style = "display:none";
        }
    }
})

function SelectNavActive() {
    var Url = location.pathname.split('/');
    if (Url[0] == "" && Url[1] == "") {
        $("#Dashboard-li").addClass("active");
    }
    else {
        var Selector = `[href='/${Url[1]}/${Url[2]}']`;
        $(`${Selector}`).closest('li.cursor-pointer').addClass("active");
    }
}

window.onpopstate = () => {
    var Url = location.pathname.split('/');
    MenuNavigation(event, Url[2], Url[1]);
}

// Change Active Status And Filter By Status Ajax
function showConfirmationDialog(title, msg, onclick_function) {
    $('#modal-dialog-confirmation-messageTitle').text(title);
    $('#modal-dialog-confirmation-messageText').html(msg);
    $('#modal-dialog-confirmation-aConfirm').unbind("click");
    $('#modal-dialog-confirmation-aConfirm').click(function () {
        $('#modal-dialog-confirmation').modal('hide');
        onclick_function();
    });
    $('#modal-dialog-confirmation').modal('show');
}

function FadeOutItemList(ListIds) {
    if (ListIds === undefined) {
        ListIds = $("input:checkbox:checked").map(function () {
            return $(this).val();
        }).get()
    }
    for (let i = 0; i < ListIds.length; i++) {
        $(`#${ListIds[i]}`).fadeOut();
    }
    setTimeout(function () {
        for (let i = 0; i < ListIds.length; i++) {
            $(`#${ListIds[i]}`).remove();
        }
    }, 1000);
}

function NavActive(li) {
    $('.active').removeClass("active");
    $(`#${li}`).addClass("active");
}

function OpenSection(UlName) {
    $(`#${UlName}`).toggleClass("d-none");
    $("li").removeClass("active");
    $(`#${UlName}`).parent().toggleClass("active");
}

$("form , main").keyup(function (e) {
    var key = e.charCode || e.keyCode || 0;
    if (key == 13)
        return false;
});

function GetPramter(ControllerName, ActionName, ActionType) {
    // Collect all input values
    var params = {
        Search: $("#Search-Input").val(),
        From: $("#From").val(),
        To: $("#To").val(),
        AreaId: $("#AreaId").val(),
        areadIDs: $("#areadIDs").val(),
        ZoneId: $("#ZoneId").val(),
        VendorID: $("#VendorID").val(),
        StatusId: $("#StatusId").val(),
        CategoryId: $("#CategoryId").val(),
        Quantity: $("#Quantity").val(),
        Status: $(".Active").data('status'),
        DeliveryManId: $("#DeliveryManId").val(),
        RoleId: $("#RoleId").val(),
        Department: $("#FilterForm [name='Department']").val(),
        EmployeeId: $("#EmployeeId").val(),
        Solved: $("#Solved").val(),
        GameId: $("#GameId").val(),
        GamerId: $("#GamerId").val(),
        LessLevel: $("#LessLevel").val(),
        GreeterLevel: $("#GreeterLevel").val(),
        LessPrice: $("#LessPrice").val(),
        GreeterPrice: $("#GreeterPrice").val(),
        GameServiceType: $("#GameServiceType").val()
    };

    var queryString = $.param(params);
    var url = `/${ControllerName}/${ActionName}?ActionType=${ActionType}&${queryString}`;
    return url;
}

function DashbordFilter(ActionName, ControllerName) {
    var Url = GetPramter(ControllerName, ActionName, 'PartialView');
    $('#MainLoder').fadeIn(100);
    $('#Footer').hide();
    $("#MainView").hide();
    $.ajax({
        url: Url,
        type: "GET",
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            $("#MainView").html(result);
            $("#MainView").fadeIn(1000);
            $('#MainLoder').fadeOut(1000);
        },
        error: function () {

        }
    });
}

function AdvertisementDetails(ControllerName, AdvertisementId) {
    $.ajax({
        url: `/${ControllerName}/AdvertisementDetails/${AdvertisementId}`,
        type: "GET",
        success: function (result) {
            $("#ShipmentDetails-Body").html(result);
            //$('#ShipmentDetails').modal('show');
        },
        error: function () {
        }
    })
}

function GetGame(id) {
    alert(id);
    $.get("/Game/GetGameList", { ID: id }, function (data) {
        $("#Gamediv").append(`<label class="font-w-600">Game Size</label>`)
        $.each(data, function (index, Game) {
            $("#Gamediv").append('<div><label for="' + Game.id + '" class="radio-card"><input type="radio" value="' + Game.id + '" onclick="GetTotalPrice()" name="SettingDTO.GameId" id="' + Game.id + '"  /><div class="card-content-wrapper"><span class="check-icon"></span><div class="card-content"><img src="/dist/images/' + Game.imgUrl + '" alt=""  width="100" /><p class="Game-Price">' + Game.price + ' EGP</p><p class="float-end Game-Name">' + Game.nameEn + '</p><p class="Game-Size">' + Game.size + '<h4/></div></div></label></div>');
        });
    });
    //$("#Gamediv").empty();
}

function Category(ID) {
    //$("#Gamediv").fadeOut(900);
    //GetGame(ID);
    $.get("/Game/GetGameList", { ID: ID }, function (data) {
        $("#Gamediv").append(`<label class="font-w-600">Game Size</label>`)
        $.each(data, function (index, Game) {
            $("#Gamediv").append('<div><label for="' + Game.id + '" class="radio-card"><input type="radio" value="' + Game.id + '" onclick="GetTotalPrice()" name="SettingDTO.GameId" id="' + Game.id + '"  /><div class="card-content-wrapper"><span class="check-icon"></span><div class="card-content"><img src="/dist/images/' + Game.imgUrl + '" alt=""  width="100" /><p class="Game-Price">' + Game.price + ' EGP</p><p class="float-end Game-Name">' + Game.nameEn + '</p><p class="Game-Size">' + Game.size + '<h4/></div></div></label></div>');
        });
    });
    $("#Gamediv").empty();
    //$("#Gamediv").slideDown(1000);
}



function ChangeUserPassword(Id) {
    $(".Spinner").removeClass("d-none");
    var NewPassword = $(`#NewPassword_${Id}`).val();
    var ConfirmNewPassword = $(`#ConfirmPassword_${Id}`).val();
    if (NewPassword != ConfirmNewPassword) {
        alertError("Password didn't match");
    }
    else {
        $.ajax({
            url: `/User/ChangePassword?id=${Id}&newPass=${NewPassword}&confirmPass=${ConfirmNewPassword}`,
            type: "POST",
            success: function (result) {
                $(".Spinner").addClass("d-none");
                $(".modal").hide();
                $(".modal-backdrop").remove();
                if (result.success) 
                alertSuccess(result.message);
                else
                    alertError(result.message);

            },
            error: function () {
                alert("Error")
            }
        });
    }
}


function GamerForm(actionName = "SaveGamer", navigation = true)
{
    if ($('#Gamer-Form').valid()) {
        $("#BtnSend").attr("disabled", true);
        $("#BtnLabel").addClass("d-none"); // Hide label
        $(".loader").removeClass("d-none"); // Show loader

    var id = $('#Id').val();
    let formData = new FormData($(`#Gamer-Form`)[0]);
  
        let fileInput = $(`#Gamer-Form input[type='file']`);
        if (fileInput.length > 0) {
            let files = fileInput[0].files;

            if (files.length > 0) {
                for (let i = 0; i < files.length; i++) {
                    formData.append('files', files[i]);
                }
            }
        }
        $.ajax({
            url: `/User/${actionName}`,
            type: "POST",
            data: formData,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            processData: false, 
            contentType: false, 
            dataType: 'json',
            success: function (result) {
                $(".Spinner").addClass("d-none");
                if (result.success)
                {
                    alertSuccessTop(result.message,4000);
                    $("#UserNamelable").text(result.userDTO.name);
                    $("#UpdateUserData").modal('hide');
                    if (navigation) 
                        MenuNavigation(event, 'ListGamer', 'User');
                }
                else
                    alertError(result.message);
            },
            error: function (err) {
                alert("try again!")
                console.log(err);
            }, complete: function () {
                $(".loader").addClass("d-none"); // Hide loader
                $("#BtnSend").attr("disabled", false); // Enable button back
                // Show button label back
                $("#BtnLabel").removeClass("d-none"); // Show original label
            }
        });   
    }
}




function EditInfo() {
    let Data = $("#EditUser-Form").serialize();
    $.ajax({
        url: `/vendor/EditUser`,
        type: "POST",
        data: Data,
        success: function (result) {
            if (result == "Invalid") {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: result,
                    showConfirmButton: false,
                    timer: 3000
                });
            }
            else {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: result,
                    showConfirmButton: false,
                    timer: 3000
                });
            }
        },
        error: function (result) {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: result,
                showConfirmButton: false,
                timer: 3000
            });
        }
    })
}



function ShippingCalculator() {
    $("#Calc-Spinner").removeClass("d-none");

    //var SourceAreaId = $("#SourceAreaId").val();
    var DestinationAreaId = $("#DestinationAreaId").val();
    var ShipmentServiceId = $("#ShipmentServiceId").val();
    var Size = $("#Size").val();
    var Weight = $("#Weight").val();

    if ($("#CalcFees-Form").valid()) {
        $.ajax({
            url: `/Vendor/ShippingCalculator?ServiceId=${ShipmentServiceId}&Size=${Size}&weight=${Weight}&Destination=${DestinationAreaId}`,
            type: "POST",
            success: function (result) {
                $("#lblSourceShipping").text(result.sourceFees);
                $("#lblDestinationShipping").text(result.destinationFees);
                $("#lblWeight").text(result.weightFees);
                $("#lblTotalFees").text(result.totalFees);

                $("#TotalFeesDiv").removeClass("d-none");
                $("#Calc-Spinner").addClass("d-none");
            },
            error: function (result) {
                alert('error' + result);
            }
        });
    } else {
        $("#CalcFees-Form").submit();
        $("#Calc-Spinner").addClass("d-none");
    }
}

function PDFReport(ControlerName, ActionName) {
    var Url = GetPramter(ControlerName, ActionName, 'Print');
    var SortBy = $("#Sort-Btn").data('isdesc');
    Url += `&IsDesc=${SortBy}`;
    window.open(Url, "_blank");
}

function ComparePassword() {
    var Password = $("#Password").val();
    var ConfirmPassword = $("#ConfirmPassword").val();
    if (ConfirmPassword != "") {
        if (Password != ConfirmPassword) {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'Password Does not match',
                showConfirmButton: false,
                timer: 3000
            });
            return false;
        }
    }
}

function AddEntity(ControllerName, ActionName, FormID, Navigation = true) {
    if ($(`#${FormID}`).valid()) {
        let Count = $(`tbody tr`).length;
        $("#BtnSend").prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        $(".custom-loader").removeClass("d-none");

        let formData = formSerialize(FormID);
        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            processData: false,
            contentType: false,
            type: 'POST',
            data: formData,
            success: function (result) {
                if (result.success == false) {
                    alertError(result.message);
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                    $(".custom-loader").addClass("d-none");
                }
                else {
                    ResetForm(FormID);
                    if ((ControllerName == "GamerService" || ControllerName=="Invoice") && ActionName == "Add")
                    {
                        if (ControllerName == "GamerService") 
                          $('#AddService-Model').modal('hide');
                       else
                        $('#AddInvoice-Model').modal('hide');

                        if (Navigation) {
                        $(".modal-backdrop").remove();
                        MenuNavigation(event, "All", ControllerName);
                        }
                    }
                     else
                        $(`tbody`).prepend(result);

                    $(`#${FormID} img`).attr('src', '');
                    $(`#DataCount-Span`).text(`Showing : ${+(++Count)}`);
                    $(".custom-loader").addClass("d-none");

                    alertSuccess();
                    $(".form-container").fadeOut();
                }
            },
            complete: function () {
            },
            error: function (error) {
                alertError();
                $("#BtnSend").prop('disabled', false);
                $(".Spinner").addClass("d-none");
                $(".custom-loader").addClass("d-none");
            }
        })
    } else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
    }
}

function EditEntity(ControllerName, ActionName, FormID, Tr) {
    if ($(`#${FormID}`).valid()) {
        $(`${FormID} #BtnSend`).prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let formData = $(`#${FormID}`).serializeArray();
        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            type: 'POST',
            data: formData,
            success: function (result) {
                if (result.success == false) {
                    alertError();
                    $(`${FormID} #BtnSend`).prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess(result.message);
                    for (let data of formData)
                        $(`#${Tr} #${data.name}`).text(data.value);

                    $(`${FormID} #BtnSend`).prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }

            },
            error: function (error) {
                alertError();
                $("#BtnSend").prop('disabled', false);
            }
        })
    }
    else {
        $(`#${FormID}`).submit();
    }
}

function DeleteEntity(ControllerName, ActionName, Tr, id = 0) {
    if (id == 0)
        id = $("#EntityId").val();

    $(".Spinner").removeClass("d-none");

    $.ajax({
        type: "PUT",
        url: `/${ControllerName}/${ActionName}/?ID=${id}`,
        success: function (result) {
            if (result.success == false) {
                alertError(result.message);
            }
            else {
                let Count = $(`#DataCount`).val();
                $(".Spinner").addClass("d-none");
                $(`#DataCount-Span`).text(`Showing : ${+(--Count)}`);
                $(`#${Tr}${id}`).fadeOut(800);
                $(`.modal`).hide();
                $('.modal-backdrop').remove();
                alertSuccess(result.message);
            }

        }
    })
}

function UpdateStatus(ControllerName, ActionName, Tr, id = 0, status = 0) {
    if (id == 0)
        id = $("#EntityId").val();

    let price = $(`#Price_${id}`).val();
    $.ajax({
        type: "PUT",
        url: `/${ControllerName}/${ActionName}/?ID=${id}&status=${status}&price=${price}`,
        success: function (result) {
            if (result.success == false) {
                alertError(result.message);
            }
            else {
                let Count = $(`#DataCount`).val();
                $(`#DataCount-Span`).text(`Showing : ${+(--Count)}`);
                $(`#${Tr}${id}`).fadeOut(800);
                alertSuccess(result.message);
            }

        }
    })
}

function EditProblemType(ControllerName, ActionName, FormID, Tr) {
    if ($(`#${FormID}`).valid()) {
        $(`${FormID} #BtnSend`).prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let formData = $(`#${FormID}`).serializeArray();
        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            type: 'POST',
            data: formData,
            success: function (result) {
                if (result.success == false) {
                    alertError();
                    $(`${FormID} #BtnSend`).prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess(result.message);
                    for (let ProblemType of formData)
                        if (ProblemType.name == "Type")
                            $(`#${Tr} #${ProblemType.name}`).text(ProblemType.value == 0 ? 'Problem' : 'Reason');
                        else
                            $(`#${Tr} #${ProblemType.name}`).text(ProblemType.value);

                    $(`${FormID} #BtnSend`).prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }

            },
            error: function (error) {
                alertError();
                $("#BtnSend").prop('disabled', false);
            }
        })
    }
    else {
        $(`#${FormID}`).submit();
    }
}

function UpdateRoleAppService(ControllerName, ActionName, FormID) {
    $(`${FormID} #BtnSend`).prop('disabled', true);
    $(".Spinner").removeClass("d-none");
    let formData = $(`#${FormID}`).serializeArray();
    $.ajax({
        url: `/${ControllerName}/${ActionName}`,
        type: 'POST',
        data: formData,
        success: function (result) {
            $(`${FormID} #BtnSend`).prop('disabled', false);
            $(".Spinner").addClass("d-none");

            if (result.success == false)
                alertError();

            else alertSuccess('Saved Successfully');
           
        },
        error: function (error) {
            alertError();
        }
    })

}

function EditArea(ControllerName, ActionName, FormID, ID) {
    if ($(`#${FormID}`).valid()) {
        $("#BtnSend").prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let DataForm = $(`#${FormID}`).serialize();
        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            type: 'POST',
            data: DataForm,
            success: function (result) {
                $(".Spinner").addClass("d-none");
                $("#BtnSend").prop('disabled', false);
                if (result.success == false) {
                    alertError(result.message);
                }
                else {
                    $(`#CityNameAr_${ID}`).text($(`#CityNameArTxt_${ID}`).val());
                    $(`#CityName_${ID}`).text($(`#CityNameTxt_${ID}`).val());
                    $(`#ZoneNameEn_${ID}`).text($(`#${FormID} .select2-selection__rendered`).text());
                    $(".form-control , .select2-selection").css({ "border-color": "#D6E4EC" });
                    alertSuccess(result);
                }
            },
            error: function () {
                alertError();
                $("#BtnSend").prop('disabled', false);
                $(".Spinner").addClass("d-none");
            }
        })
    } else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
    }
}

function formSerialize(FormID, GetShipments = false) {
    let form = $(`#${FormID}`).serializeArray();
    let formData = new FormData();
    let File = document.querySelector(`#${FormID} #ImgFile`);
    if (File !== null)
        if (File.files.length > 0)
            formData.append("File", $(`#${FormID} #ImgFile`)[0].files[0]);

    if (GetShipments) {
        let AdvertisementIds = $("input:checkbox:checked").map(function () {
            return $(this).val();
        }).get()
        formData.append("AdvertisementIds", AdvertisementIds);
    }
    for (let data of form)
        formData.append(data.name, data.value);

    return formData;
}

function OpenModal(id, ModelId) {
    $(`#${ModelId}`).modal('show');
    $("#EntityId").val(id)
}

function EditUser(ControllerName, ActionName, FormID) {
    if ($(`#${FormID}`).valid()) {
        $(`${FormID} #BtnSend`).prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let formData = formSerialize(FormID);
        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            processData: false,
            contentType: false,
            type: 'POST',
            data: formData,
            success: function (result) {
                if (result.success == false) {
                    alertError();
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess();
                    let RoleID = $("#EditEmployee-Form option:selected").text();
                    let file = document.querySelector(`#${FormID} #ImgFile`);

                    $(`#User_${result.model.id} #Td-Name`).html(result.model.name);
                    $(`#User_${result.model.id} #Td-Email`).html(result.model.email);
                    $(`#User_${result.model.id} #Td-Role`).html(`<b>${RoleID}</b>`);
                    $(`#User_${result.model.id} #Td-Info`).html(`${result.model.phoneNumber}<br/>${result.model.address}<br/>${result.model.email}`);
                    $(`#User_${result.model.id} #Td-VendorInfo`).html(`<a href="/PaymentReceive/AccountStatement?clientid=${result.model.id}" target="_blank"><b>${result.model.name}</b></a><br/>${result.model.address}<br/>${result.model.addressDetails}<br/>${result.model.phoneNumber}<br/>${result.model.email}`);
                    $(`#User_${result.model.id} #Td-ProductType`).html(result.model.productType);
                    $(`#User_${result.model.id} #Td-Password`).html(result.model.password);

                    if (file.files.length > 0)
                        $(`#User_${result.model.id} #Td-Img`).attr('src', URL.createObjectURL(file.files[0]));
                    else if (result.model.imageUrl == null)
                        $(`#User_${result.model.id} #Td-Img`).attr('src', `/dist/images/Fake-Img2.png`);

                    else
                        $(`#User_${result.model.id} #Td-Img`).attr('src', `/dist/images/${result.model.imageUrl}`);

                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
            },
            error: function (error) {
                alertError();
                $("#BtnSend").prop('disabled', false);
            }
        })
    }
    else {
        $(`#${FormID}`).submit();
    }
}

function OpenEditUserModel(id, Modal, Show = true) {
    $.ajax({
        type: "GET",
        url: `/User/GetUserData/${id}`,
        success: function (result) {
            if (result == false) {
                alertError()
            }
            else {
                $(`#${Modal} #Id`).val(result.id);
                $(`#${Modal} .Username`).val(result.email);
                $(`#${Modal} #Name`).val(result.name);
                $(`#${Modal} #RoleID`).val(result.roleID);
                $(`#${Modal} #Address`).val(result.address);
                $(`#${Modal} #PhoneNumber`).val(result.phoneNumber);
                $(`#${Modal} #Email`).val(result.email);
                $(`#${Modal} #ProductType`).val(result.productType);
                $(`#${Modal} #Password`).val(result.password);
                $(`#${Modal} #PageName`).val(result.pageName);
                $(`#${Modal} #LocationUrl`).val(result.locationUrl);
                $(`#${Modal} #Landmark`).val(result.landmark);
                $(`#${Modal} #Floor`).val(result.floor);
                $(`#${Modal} #Apartment`).val(result.apartment);
                $(`#${Modal} #ZoneId`).val(result.zoneId);
                $(`#${Modal} #Bank`).val(result.bank);
                $(`#${Modal} #AccountName`).val(result.accountName);
                $(`#${Modal} #AccountNumber`).val(result.accountNumber);
                $(`#${Modal} #IBANNumber`).val(result.ibanNumber);
                $(`#${Modal} #VodafoneCashNumber`).val(result.vodafoneCashNumber);
                $(`#${Modal} #InstaPayAccountName`).val(result.instaPayAccountName);
                $(`#${Modal} #AreaId`).val(result.areaId);
                $(`#${Modal} #ImgUrl`).val(result.imgUrl);

                if (result.imgUrl !== "")
                    $(`#${Modal} #DisplayImg`).attr('src', `/dist/images/${result.imgUrl}`).removeClass(`d-none`);

                if (Show)
                    $(`#${Modal}`).modal('show');
            }

        }
    })
}

function EditBranch(ControllerName, ActionName, FormID, Tr) {
    if ($(`#${FormID}`).valid()) {
        $(`${FormID} #BtnSend`).prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let formData = $(`#${FormID}`).serializeArray();

        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            type: 'POST',
            data: formData,
            success: function (result) {
                if (result.success == false) {
                    alertError();
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess(result.message);
                    for (let branch of formData)
                        $(`#${Tr} #${branch.name}`).text(branch.value);

                    $(`${FormID} #BtnSend`).prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }

            },
            error: function (error) {
                alertError();
                $("#BtnSend").prop('disabled', false);

            }
        })
    }
    else {
        $(`#${FormID}`).submit();
    }
}

function CreateForm(AccountID, DriverID) {
    window.open(`/User/PickupOrdersAssigned?DriverID=${DriverID}&AccountID=${AccountID}&IsPDF=true`, '_blank');
}

function DeleteOrder(OrderID, TableRow) {
    var url = '/User/DeleteOrder'
    url += "?" + "OrderID=" + OrderID + "&ReturnView=PickupOrdersAssigned";
    $.ajax({
        type: 'GET',
        url: url,
        async: false,
        success: function (response) {
            $(`#${TableRow}`).fadeOut(1000);
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: response,
                showConfirmButton: false,
                timer: 2000
            });
            setTimeout(function () { $(`#${TableRow}`).remove(); }, 3000);
        },
        error: function () { alert("error"); }
    });
}

function EditZone(FormID) {
    if ($(`#${FormID}`).valid()) {
        $(`${FormID} #BtnSend`).prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let DataForm = $(`#${FormID}`).serialize();
        $.ajax({
            url: `/Admin/EditZone`,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            type: 'POST',
            data: DataForm,
            success: function (result) {
                if (result.success == false) {
                    alertError(result.message);
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess(result.message);
                    $(`${FormID} #BtnSend`).prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                    $(`.modal-backdrop`).remove();
                    MenuNavigation(event, 'ZoneList', 'Admin');
                }
            },
            error: function (error) {
                alertError();
                $("#BtnSend").prop('disabled', false);
            }
        })
    }
    else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
    }
}

function AddZone(FormID) {
    if ($(`#${FormID}`).valid()) {
        $(`${FormID} #BtnSend`).prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let DataForm = $(`#${FormID}`).serialize();
        $.ajax({
            url: `/Admin/AddZone`,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            type: 'POST',
            data: DataForm,
            success: function (result) {
                if (result.success == false) {
                    alertError(result.message);
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess(result.message);
                    $(`${FormID} #BtnSend`).prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                    $(`.modal-backdrop`).remove();
                    MenuNavigation(event, 'ZoneList', 'Admin');
                }
            },
            error: function (error) {
                alertError();
                $("#BtnSend").prop('disabled', false);
            }
        })
    }
    else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
    }
}


function GetCustomerFollowup(id) {
    $(`#ShipmentDetails-Body`).html(`<div class="spinner-border text-danger d-block m-auto" role="status">
                                     <span class="sr-only">Loading...</span></div>`);
    $.ajax({
        type: 'Get',
        url: `/User/GetCustomerFollowup/${id}`,
        async: true,
        dataType: 'html',
        success: function (response) {
            $(`#ShipmentDetails-Body`).html(response);
            //$(`#ShipmentDetails`).modal('show');
        },
        error: function () { alert("error"); }
    });
}

//////////////Game///////////

function OpenEditGameModel(id, Modal) {
    $.ajax({
        type: "GET",
        url: `/Game/EditGame/${id}`,
        success: function (result) {
            if (!result) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'failed try again',
                    showConfirmButton: false,
                    timer: 4000
                });
            }
            else {
                $(`#${Modal} #id`).val(result.id);
                $(`#${Modal} #Size`).val(result.size);
                $(`#${Modal} #NameEn`).val(result.nameEn);
                $(`#${Modal} #NameAr`).val(result.nameAr);
                $(`#${Modal} #CategoryId`).val(result.categoryId);
                $(`#${Modal} #Description`).val(result.description);
                $(`#${Modal} #ImgUrl`).val(result.imgUrl);
                $(`#${Modal} #GameImg`).attr("src",`/dist/images/${result.imgUrl}`);

                $(`#${Modal}`).modal('show');
            }

        }
    });
}
function OpenEditGameChargeModel(id, Model) {
    $.ajax({
        type: "GET",
        url: `/GameCharge/EditGameCharge/${id}`,
        success: function (result) {
            if (!result) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'failed try again',
                    showConfirmButton: false,
                    timer: 4000
                });
            }
            else {
                $(`#${Model} #Id`).val(result.id);
                $(`#${Model} #Price`).val(result.price);
                $(`#${Model} #Count`).val(result.count);
                $(`#${Model} #Discount`).val(result.discount);
                $(`#${Model} #GameId`).val(result.gameId);
                $(`#${Model} #Img`).val(result.img);
                $(`#${Model} #EditGameChargeImg`).attr("src", `/dist/images/${result.img}`);
                $(`#${Model}`).modal('show');
            }
        }
    });
}
function EditGame(Modal, FormID) {
    if ($(`#${FormID}`).valid()) {
        $("#BtnSend").prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let GameId = $(`#${Modal} #id`).val();
        let Tr = `Tr_${GameId}`
        let formData = formSerialize(FormID);
        let CategoryName = $(`#${Modal}`).find(":selected").text();
        $.ajax({
            type: "POST",
            url: `/Game/EditGame`,
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (!result.success) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'failed try again',
                        showConfirmButton: false,
                        timer: 4000
                    });
                    $(".Spinner").addClass("d-none");
                }
                else {
                    let file = document.querySelector(`#${FormID} #ImgFile`);

                    $(`#${Tr} #id`).text(result.model.id);
                    $(`#${Tr} #Size`).text(result.model.size);
                    $(`#${Tr} #NameEn`).text(result.model.nameEn);
                    $(`#${Tr} #NameAr`).text(result.model.nameAr);
                    $(`#${Tr} #CategoryName b`).text(CategoryName);
                    $(`#${Tr} #Count`).text(result.model.count);
                    $(`#${Tr} #Price`).text(result.model.price);

                    if (file.files.length > 0)
                        $(`#${Tr} #ImgUrl img`).attr('src', URL.createObjectURL(file.files[0]));

                    else if (result.model.imgUrl == null)
                        $(`#${Tr} #ImgUrl img`).attr('src', `/dist/images/Fake-Img2.png`);
                    else
                        $(`#${Tr} #ImgUrl img`).attr('src', `/dist/images/${result.model.imgUrl}`);

                    $(`#${Modal}`).modal('hide');
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                    alertSuccess();
                }

            }
        })
    }
    else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
        $(".Spinner").addClass("d-none");
    }
}

function EditChargeGame(Modal, FormID) {
    if ($(`#${FormID}`).valid()) {
        $("#BtnSend").prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let GameId = $(`#${Modal} #Id`).val();
        let Tr = `Tr_${GameId}`
        let formData = formSerialize(FormID);
        $.ajax({
            type: "POST",
            url: `/GameCharge/EditGameCharge`,
            data: formData,
            processData: false,
            contentType: false,
            success: function (result) {
                if (!result.success) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'failed try again',
                        showConfirmButton: false,
                        timer: 4000
                    });
                    $(".Spinner").addClass("d-none");
                }
                else {
                    let file = document.querySelector(`#${FormID} #ImgFile`);

                    $(`#${Tr} #Price`).text(result.model.price);
                    $(`#${Tr} #Count`).text(result.model.count);
                    $(`#${Tr} #Discount`).text(result.model.discount);
                    $(`#${Tr} #Game`).html(`<b>${result.model.game.nameEn}</b> <img src="/dist/images/${result.model.game.imgUrl}" width="50px"></img>`);

                    if (file.files.length > 0)
                        $(`#${Tr} #ImgUrl img`).attr('src', URL.createObjectURL(file.files[0]));

                    else if (result.model.img == null)
                        $(`#${Tr} #ImgUrl img`).attr('src', `/dist/images/Fake-Img2.png`);
                    else
                        $(`#${Tr} #ImgUrl img`).attr('src', `/dist/images/${result.model.img}`);

                    $(`#${Modal}`).modal('hide');
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                    alertSuccess();
                }

            }
        })
    }
    else {
        $(`#${FormID}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
        $(".Spinner").addClass("d-none");
    }
}

//////////////End Game///////////

////////////Select Shipments

function SelectAllCheckbox() {
    var checked = $('#Allcheckbox').prop('checked');
    if (checked) {
        $("input[type=checkbox]").prop('checked', 'checked');
    }
    else {
        $("input[type=checkbox]").prop('checked', false);
    }
}

function SelectAllCheckboxInTable(TableID) {
    var checked = $('#Allcheckbox').prop('checked');
    if (checked) {
        $(`#${TableID} input[type=checkbox]`).prop('checked', 'checked');
    }
    else {
        $(`#${TableID} input[type=checkbox]`).prop('checked', false);
    }
}

function SelectShipment(Key) {
    var checked = $(`#SelectArea_${Key}`).prop('checked');
    if (checked) {
        $(`#Table_${Key} input[type=checkbox]`).prop('checked', 'checked');
    }
    else {
        $(`#Table_${Key} input[type=checkbox]`).prop('checked', false);
    }
}

function OpenFileInput(form) {
    $(`#${form} #ImgFile`).trigger('click');
}

function UpdateImgDisplay(event , id) {
     const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = function (e) {
        $(`#${id}`).attr('src', e.target.result);
    }

    if (file) {
        reader.readAsDataURL(file);
    }
}

function DisplayUpdateImg(file) {
    let Img = file.target.previousElementSibling;
    Img.classList.remove('d-none');
    let input = file.target;
    let reader = new FileReader();
    reader.onload = function () {
        var dataURL = reader.result;
        Img.src = dataURL;
    };
    reader.readAsDataURL(input.files[0]);
};

function OpenFilter() {
    $("#Filter-Model").slideToggle(400);
}

function ClearFilter() {
    $("#FilterForm").trigger("reset");
}

function ExcelReport(type, fn, dl) {
    var Table = document.getElementById('Red-Table');
    var wb = XLSX.utils.table_to_book(Table, { sheet: "sheet1" });
    return dl ?
        XLSX.write(wb, { bookType: type, bookSST: true, type: 'base64' }) :
        XLSX.writeFile(wb, fn || ('Red-Sheet.' + (type || 'xlsx')));
}

function PrintTable() {
    window.print();

}

function GameUnselect(e) {
    $('input[name="SettingDTO.GameId"]').prop('checked', false);
    GetTotalPrice();
}

function GetVendorInfo(VendorId) {
    $.ajax({
        type: 'GET',
        url: `/User/GetUserData?Id=${VendorId}`,
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
            }
        },
        error: function () { alert("error"); }
    });
}

////////////////////Pickup Requst Section////////////////

function EditePickupRequst(id) {
    var AreaId = $(`#AreaIdtxt_${id}`).val();
    var Address = $(`#AddressTxt_${id}`).val();
    var PickupDate = $(`#DateTxt_${id}`).val();
    var ReadyTime = $(`#ReadyTimeTxt_${id}`).val();
    var LastTimeAvailable = $(`#LastTimeAvailableTxt_${id}`).val();
    if (AreaId == "") {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please Select Area',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    if (Address == "") {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please Enter Address',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    if (PickupDate == "") {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please Enter a Date',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    if (ReadyTime == "") {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please Enter Ready Time',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    if (LastTimeAvailable == "") {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please Enter Last Time Available',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    if (`${ReadyTime}:00` > `${LastTimeAvailable}:00`) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Last Time Available Less Than Ready Time',
            showConfirmButton: false,
            timer: 5000
        });
        return false;
    }
    $("#BtnSend").prop('disabled', true);
    $(".Spinner").removeClass("d-none");
    $.ajax({
        type: 'POST',
        url: `/PickUpRequest/UpdatePickupRequst`,
        data: { id: id, Address: Address, PickupDate: PickupDate, ReadyTime: ReadyTime, LastTimeAvailable: LastTimeAvailable, AreaId: AreaId },
        success: function (response) {
            $("#BtnSend").prop('disabled', false);
            $(".Spinner").addClass("d-none");
            if (response === true) {
                alertSuccess();
                $(`.fade`).remove();
                $('body').removeClass('modal-open');
                var Url = location.pathname.split('/');
                MenuNavigation(event, Url[2], Url[1]);
            }
            else if (response === false) {
                alertError('Invalid , try again');
            }
        }
    });
}

function ChangeVendorInfo() {
    GetVendorInfo($("#VendorId").val());
}

function GetStockPickups(id) {
    $("#PickupStocktDetails-TableSection").html(`<table id="Red-Table" class="table">
                            <tbody id="tblContent">
                                <tr>
                                    <td colspan="10">
                                        <div class="spinner-border text-danger ml-50" role="status">
                                            <span class="sr-only">Loading...</span>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>`);
    $.ajax({
        url: `/PickUpRequest/GetStockPickups`,
        type: "GET",
        data: { id: id },
        success: function (result) {
            $("#PickupStocktDetails-TableSection").html(result);
        },
        error: function () {

        }
    });
}

////////////////////End////////////////

//////////////////// START : Shipping Section ////////////////

function TrackShipment() {
    let TrackShipment = $(`#TrackShipment`).val();
    if (TrackShipment == "") {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: "Plese Enter Shipment Code",
            showConfirmButton: false,
            timer: 3000
        });
    }
    else {
        $.ajax({
            type: 'Get',
            url: `/Shipment/TrackShipment`,
            data: { TrackShipment: TrackShipment },
            success: function (response) {
                if (response != false) {
                    $("#ShipmentDetails-Body").html(response);
                    $('#ShipmentDetails').modal('show');
                }
                else if (response == false) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'No Shipment Found',
                        showConfirmButton: false,
                        timer: 4000
                    });
                }
            }
        });
    }
}

function GetAreasByZone(event) {
    if (event !== undefined)
        ZoneID = event.target.value;
    else
        ZoneID = document.querySelector(`#ZoneId`).value;

    $(`#AreaId , [name='AreaId']`).empty();
    $(`#AreaId , [name='AreaId']`).append(`<option value>--- Select Area ---</option>`);
    $.get("/Admin/GetAreasByZone", { ID: ZoneID }, function (data) {
        $.each(data, function (key, value) {
            $(`#AreaId , [name='AreaId']`).append(`<option value="${value.id}">${value.cityNameAr}</option>`);
        });
    });
}

function StockItems(ControlerName, AdvertisementId) {
    $(`#ItemsModal-body-bod`).html(`<div class="spinner-border text-danger d-block m-auto" role="status">
                                        <span class="sr-only">Loading...</span>
                                    </div>`);
    $.ajax({
        url: `/Shipment/StockItems/${AdvertisementId}`,
        type: "GET",
        success: function (result) {
            $("#ItemsModal-body").html(result);
            //$("#ItemsModal").modal("show");
        },
        error: function () { }
    })
}

function PartialItems(ControlerName, AdvertisementId) {
    $(`#PartialItemsModal-bod`).html(`<div class="spinner-border text-danger d-block m-auto" role="status">
                                          <span class="sr-only">Loading...</span>
                                      </div>`);
    $.ajax({
        url: `/Shipment/PartialItems/${AdvertisementId}`,
        type: "GET",
        success: function (result) {
            $("#PartialItemsModal-body").html(result);
            //$("#PartialItemsModal").modal("show");
        },
        error: function () { }
    })
}

//////////////////// END : Shipping Section ////////////////


//////////////////// START : WearHouse Section ////////////////
function ReceivePickupsDDLchange() {
    var deliveryManID = $('#CourierChangeVal').val();
    var vendorID = $('#VendorChangeVal').val();
    var url = '/Warehouse/GetPickupCodes'
    url = `${url}?deliveryManID=${deliveryManID}&vendorID=${vendorID}`;
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            $("#dvcodes").html(result);
            $("#Ajaxloader").fadeOut(1000);
        },
        error: function () {
            alert('Error');
        }
    });
}

function ReceiveStockDDLchange() {
    var deliveryManID = $('#CourierChangeVal').val();
    var vendorID = $('#VendorChangeVal').val();
    var url = '/Warehouse/GetStockCodes'
    url = `${url}?deliveryManID=${deliveryManID}&vendorID=${vendorID}`;
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            $("#dvcodes").html(result);
            $("#Ajaxloader").fadeOut(1000);
        },
        error: function (result) {
            alert(result);
        }
    });
}

function ReceiveReturnsChange() {
    var deliveryManID = $('#CourierChangeVal').val();
    var url = '/Warehouse/GetShipmentCodes'
    url = `${url}?deliveryManID=${deliveryManID}`;
    $.ajax({
        type: "GET",
        url: url,
        success: function (result) {
            $("#dvcodes").html(result);
            $('#rdbNotDelivered').prop('checked', true);
            rdbReturnFilterChange('rdbNotDelivered');
            $("#Ajaxloader").fadeOut(1000);
        },
        error: function (result) {
            alert(result);
        }
    });
}

function rdbReturnFilterChange(key) {
    var currentShipments = $(`#dvcodes ul li.${key}`);
    $(`#dvcodes ul li`).addClass('d-none');
    $(`#dvcodes ul li.${key}`).removeClass('d-none');

    if (currentShipments.length == 0) {
        $(`#dvcodes ul`).append(`<li><label>No Data</label></li>`);
    }
}

function ReveivePickup() {
    var checkedCheckboxes = $("#dvcodes ul li input[type=checkbox]").filter(function () {
        return this.checked;
    });
    var checkedCheckboxValues = checkedCheckboxes.map(function () {
        return this.id;
    }).get();

    if (checkedCheckboxValues.length == 0) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please select pickup(s)',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    else {
        Swal.fire({
            title: 'Are you sure you want to receive pickup(s) ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#1ee0ac',
            cancelButtonColor: '#dc3545',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.isConfirmed) {
                $(`.Spinner`).removeClass('d-none');
                $.ajax({
                    url: `/Warehouse/ReveivePickup`,
                    type: "POST",
                    data: { pickUpRequestIDs: checkedCheckboxValues.toString() },
                    success: function (response) {
                        if (response.success == false) {
                            Swal.fire({
                                position: 'center',
                                icon: 'error',
                                title: 'Error',
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
                        $(`.Spinner`).addClass('d-none');
                        // $("#dvcodes").html('');
                        checkedCheckboxValues.map(idx => $(`#checkbox_${idx}`).fadeOut());
                    },
                    error: function () { }
                });
            }
        })
    }
}

function ReceiveReturn() {
    var selectedCheckboxes = $("#dvcodes ul li input[type=checkbox]:checked").map(function () {
        return this.id;
    }).get();

    if (selectedCheckboxes.length == 0) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please select shipment(s)',
            showConfirmButton: false,
            timer: 3000
        });
        return false;
    }
    else {
        Swal.fire({
            title: 'Are you sure you want to receive shipment(s) ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#1ee0ac',
            cancelButtonColor: '#dc3545',
            confirmButtonText: 'Yes'
        }).then((result) => {
            if (result.isConfirmed) {
                $(`.Spinner`).toggleClass('d-none');
                $.ajax({
                    url: `/Warehouse/ReceiveReturn`,
                    type: "POST",
                    data: { AdvertisementIds: selectedCheckboxes.toString() },
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
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: response.message,
                                showConfirmButton: false,
                                timer: 2000
                            });
                        }
                        $(`.Spinner`).addClass('d-none');
                        $("#dvcodes").html('');
                        MenuNavigation('ReceiveReturns', 'Warehouse');
                    },
                    error: function () {
                        Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'Error',
                            showConfirmButton: true
                        });
                        MenuNavigation('ReceiveReturns', 'Warehouse');
                    }
                });
            }
        })
    }
}

function GetGameByCategory(ShipId) {
    let CategoryId = $(`#WarehouseCategoryId_${ShipId}`).val();
    $(`#WarehouseGameId_${ShipId}`).empty();
    $(`#WarehouseGameId_${ShipId}`).append(`<option value="0">--- N/A ---</option>`);
    $.get("/Game/GetGameList", { ID: CategoryId }, function (data) {
        $.each(data, function (key, value) {
            $(`#WarehouseGameId_${ShipId}`).append(`<option value="${value.id}">${value.nameEn}</option>`);
        });
    });
}

function ChangeToReady(shipId) {
    Swal.fire({
        title: "Are you sure you have the shipment ready?!",
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#d33',
        confirmButtonText: "Confirm",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'Get',
                url: `/Warehouse/ChangeToReady?shipId=${shipId}`,
                success: function (response) {
                    if (response.success == true) {
                        $(`#${shipId}`).fadeOut(1000);
                        setTimeout(function () { $(`#${shipId}`).remove(); }, 1000);
                        $(`#Policy-Count`).text(``);
                        $(`#Ready-Count`).text(``);
                        $("input:checkbox:checked").prop('checked', false);
                        Swal.fire({
                            title: response.message,
                            icon: 'success',
                            showConfirmButton: false,
                            timer: 2000
                        });
                    }
                    else {
                        Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: response.message,
                            showConfirmButton: false,
                            timer: 4000
                        });
                    }
                }
            })
        }
    })
}

function ChangeToReadyMany() {
    var shipIDs = $("#PendingShipments-Body input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    $.ajax({
        type: 'Get',
        url: `/Warehouse/ChangeToReadyMany`,
        data: { shipIDs: shipIDs.toString() },
        success: function (response) {
            if (response.success == true) {
                FadeOutItemList(shipIDs);
                $(`#Policy-Count`).text(``);
                $(`#Ready-Count`).text(``);
                $(`#Allcheckbox`).prop('checked', false);
                Swal.fire({
                    title: response.message,
                    icon: 'success',
                    showConfirmButton: false,
                    timer: 2000
                });
            }
            else {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: response.message,
                    showConfirmButton: false,
                    timer: 4000
                });
            }
        }
    });
}

function PrintMany() {
    var AdvertisementIds = $("#PendingShipments-Body input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    var Url = `/Shipment/PrintAll?ShipmentsIds=${AdvertisementIds}`;
    window.open(Url, "_blank");
}

function EditWarehouseGame(AdvertisementId) {
    let DataForm = $(`#VerifyGame-Form_${AdvertisementId}`).serialize();
    $.ajax({
        url: "/Warehouse/EditGame",
        type: "GET",
        data: DataForm,
        success: function (response) {
            if (response.success == true) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: response.message,
                    showConfirmButton: false,
                    timer: 2000
                });
            }
            else {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: response.message,
                    showConfirmButton: false,
                    timer: 2000
                });
            }
        },
        error: function () {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: response.message,
                showConfirmButton: false,
                timer: 2000
            });
        }
    });
}

function BackToWarehouse(shipId) {
    Swal.fire({
        title: "Are you sure you want to return the shipment to pending?!",
        icon: 'error',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#d33',
        confirmButtonText: "Confirm",
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: 'Get',
                url: `/Warehouse/BackToWarehouse?shipId=${shipId}`,
                success: function (response) {
                    if (response.success == true) {
                        $(`#${shipId}`).fadeOut(1000);
                        setTimeout(function () { $(`#${shipId}`).remove(); }, 1000);
                        Swal.fire({
                            title: response.message,
                            icon: 'success',
                            showConfirmButton: false,
                            timer: 2000
                        });
                    }
                    else {
                        Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: response.message,
                            showConfirmButton: false,
                            timer: 2000
                        });
                    }
                }
            })
        }
    })
}

function PendingShipmentsSelect() {
    var AdvertisementIds = $("#PendingShipments-Body input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    $(`#Policy-Count`).text(`(${AdvertisementIds.length})`);
    $(`#Ready-Count`).text(`(${AdvertisementIds.length})`);
}
//////////////////// END : WearHouse Section ////////////////

function GetDeliveryPickups(id) {
    $("#PickupRequstDetails-TableSection").html(`<table id="Red-Table" class="table">
                            <tbody id="tblContent">
                                <tr>
                                    <td colspan="10">
                                        <div class="spinner-border text-danger ml-50" role="status">
                                            <span class="sr-only">Loading...</span>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>`);
    $.ajax({
        url: `/PickUpRequest/GetDeliveryPickups/${id}`,
        type: "GET",
        success: function (result) {
            $("#PickupRequstDetails-TableSection").html(result);
            //$("#PartialItemsModal").modal("show");
        },
        error: function () { }
    });
}

function ValidateShipmentPickupRequest(AdvertisementIds) {
    const Vendor = $(`#${AdvertisementIds[0]}`).data('vendor');
    let VaildFlag = true;
    for (var i = 0; i < AdvertisementIds.length; i++) {
        const VendorLope = $(`#${AdvertisementIds[i]}`).data('vendor');
        if (Vendor != VendorLope) {
            VaildFlag = false;
            break;
        }
    }
    return VaildFlag;
}

function ValidationForm() {
    var forms = document.getElementsByClassName('needs-validation');
    var Btns = document.querySelectorAll('.needs-validation #BtnSend');
    if (Btns.length > 0) {
        // Loop over them and prevent submission
        var validation = Array.prototype.filter.call(forms, function (form) {
            Btns.forEach(Btn => {
                Btn.addEventListener('click', function (event) {
                    if (form.checkValidity() === false) {
                        event.preventDefault();
                        event.stopPropagation();
                    }
                    form.classList.add('was-validated');
                }, false);
            })
        });
    }
}

let ShipmentProductList = [];

//////////////////// START : Complains Section ////////////////
function AddEComplains(ControllerName, ActionName, FormID) {
    if ($(`#${FormID}`).valid()) {
        $("#BtnSend").prop('disabled', true);
        $(".Spinner").removeClass("d-none");
        let DataForm = $(`#${FormID}`).serialize();
        let totalIssues = $(`#TotalIssues`).text();
        let pendingIssues = $(`#PendingIssues`).text();
        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            type: 'POST',
            data: DataForm,
            dataType: 'html',
            success: function (result) {
                if (result.success == false) {
                    alertError();
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess();
                    $(`tbody`).prepend(result);
                    $(`#DataCount-Span`).text(`Showing : ${+(++totalIssues)}`);
                    $(`#TotalIssues`).text(totalIssues);
                    $(`#PendingIssues`).text(+(++pendingIssues));
                    ResetForm(FormID);
                    $(`#select2-VendorId-container`).text("--- Select Vendor ---");
                    $(`#select2-Department-container`).text("--- Select Department ---");
                    $(`#select2-EmployeeId-container`).text("--- Select Employee ---");
                }
            },
            error: function (error) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'Failed , try again',
                    showConfirmButton: false,
                    timer: 3000
                });
                $("#BtnSend").prop('disabled', false);
            }
        })
    } else {
        $(`#${FormID}`).submit();
        $(`.error`).css("display", "none");
        $(`#Description`).css("display", "block");

    }
}

function solveComplains(ControllerName, ActionName, FormID) {
    let complainsId = $("#EntityId").val();
    let pendingIssues = $(`#PendingIssues`).text();
    let solvedIssues = $(`#SolvedIssues`).text();
    let totalIssues = $(`#TotalIssues`).text();
    let Comment = $(`#Comment`).val();
    if ($(`#${FormID}`).valid()) {
        $.ajax({
            url: `/${ControllerName}/${ActionName}?id=${complainsId}&Comment=${Comment}`,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            type: 'Post',
            success: function (result) {
                if (result) {
                    //$(`#PendingIssues`).text(--Number(pendingIssues));
                    //$(`#SolvedIssues`).text(++Number(solvedIssues));
                    $(`#PendingIssues`).text(+(--pendingIssues));
                    $(`#SolvedIssues`).text(+(++solvedIssues));
                    $(`#DataCount-Span`).text(`Showing : ${--totalIssues}`)
                    $(`#Complains_${complainsId}`).fadeOut(1000);
                    setTimeout(() => $(`#Complains_${complainsId}`).remove(), 3000);
                    alertSuccess();
                    $(`#SolveComplains-Model`).modal('hide');
                }
                else alertError();
            }
        });
    }
    else {
        $(`#${FormID}`).submit();
    }

}

function deleteComplains(ControllerName, ActionName, complainsId) {
    Swal.fire({
        title: 'Are you sure you delete Complains?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#1ee0ac',
        cancelButtonColor: '#dc3545',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            let pendingIssues = $(`#PendingIssues`).text();
            let totalIssues = $(`#TotalIssues`).text();
            $.ajax({
                url: `/${ControllerName}/${ActionName}?id=${complainsId}`,
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                type: 'Post',
                success: function (result) {
                    if (result) {
                        //$(`#PendingIssues`).text(--Number(pendingIssues));
                        //$(`#SolvedIssues`).text(++Number(solvedIssues));
                        $(`#PendingIssues`).text(+(--pendingIssues));
                        $(`#TotalIssues`).text(+(--totalIssues));
                        $(`#DataCount-Span`).text(`Showing : ${totalIssues}`)
                        $(`#Complains_${complainsId}`).fadeOut(1000);
                        setTimeout(() => $(`#Complains_${complainsId}`).remove(), 3000);
                        alertSuccess();
                    }
                    else alertError();
                }
            });
        }
    })
}
//////////////////// END : Complains Section ////////////////

//////////////////// START : Product Management Section ////////////////
function DeletePorduct(id) {
    $.ajax({
        url: `/Product/DeletProduct/${id}`,
        contentType: false,
        type: 'POST',
        data: id,
        processData: false,
        dataType: 'json',
        success: function (result) {
            $(`#DeleteModal_${id}`).modal('hide');
            $(`#Product_${id}`).fadeOut(1000);
            if (result != null) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: result,
                    showConfirmButton: false,
                    timer: 3000
                });
            }
            $(`Product_${id}`).remove();
        }
    });
}
function ProdactForm(ActionName, ControllerName, Form) {
    let Count = $(`tbody tr`).length;
    if ($(`#${Form}`).valid()) {
        $(".Spinner").removeClass("d-none");
        $("#BtnSend").prop('disabled', true);
        let formData = formSerialize(Form);
        $.ajax({
            url: `/${ControllerName}/${ActionName}`,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            dataType: 'html',
            success: function (result) {
                if (result == "false") {
                    alertError();
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                }
                else {
                    alertSuccess();
                    $("#BtnSend").prop('disabled', false);
                    $(".Spinner").addClass("d-none");
                    let file = document.querySelector(`#${Form} [data-id="ImgFile"]`);
                    let ImgUrl;
                    if (file.files.length > 0)
                        ImgUrl = URL.createObjectURL(file.files[0]);

                    $(".form-control , .select2-selection").css({ "border-color": "#D6E4EC" });
                    if (ActionName === "AddProduct") {
                        $(`tbody`).prepend(result);
                        let id = document.querySelector(`[data-id="ProductId"]`).textContent;
                        $(`#Product_${id} img`).attr(`src`, `${ImgUrl}`)

                        ResetForm(Form);
                        $(`#select2-AccountId-container`).text("--- Select Account ---");
                        $(`#DataCount-Span`).text(`Showing : ${+(++Count)}`);
                        $("#VendorProductImg").attr("src", "/dist/images/Partial.png");
                    }
                    else {
                        let dataForm = $(`#${Form}`).serializeArray();
                        for (let data of dataForm) {
                            if (data.name === "ImageUrl")
                                continue;
                            else
                                $(`#${data.name}_${result}`).text(data.value);
                        }

                        $(`#AccountName_${result}`).text($('#select2-VendorId-container').text())
                        if (file.files.length > 0)
                            $(`#ImageUrl_${result} img`).attr(`src`, `${ImgUrl}`)
                    }

                }
            },
            error: function (error) {
                alertError()
            }
        })
    } else {
        $(`#${Form}`).submit();
        $("label:contains('This field is required.')").css("display", "none");
    }
}

function GetProduct(ProductId, FormID) {
    $.ajax({
        url: `/Product/GetProduct?ProductId=${ProductId}`,
        contentType: false,
        type: 'POST',
        processData: false,
        dataType: 'json',
        success: function (result) {
            if (result != null) {
                $(`#${FormID} #Name`).val(result.name);
                $(`#${FormID} #OriginalPrice`).val(result.originalPrice);
                $(`#${FormID} #VendorId`).val(result.vendorId);
                $(`#${FormID} #Size`).val(result.size);
                $(`#${FormID} #Description`).val(result.description);
                $(`#${FormID} #VendorProductImg`).attr("src", `/dist/images/${result.imageUrl}`);
                $(`#${FormID} #ImageUrl`).val(result.imageUrl)
                $(`#${FormID} #VendorProductId`).val(result.vendorProductId);
                $(`#${FormID} #select2-VendorId-container`).text(result.vendorName);
            }
            else alertError();
        }
    })
}




//////////////////// END : Product Management Section ////////////////


function VendorReply(OrderID, Solution) {
    let url = "/Vendor/VendorReply";
    url = `${url}?OrderID=${OrderID}&Solution=${Solution}`;
    $.ajax({
        type: 'GET',
        url: url,
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


function GetAreasList(id) {
    $.ajax({
        url: `/Admin/GetAreas?ID=${id}`,
        type: "GET",
        success: function (result) {
            $("#Areas-Modal").html(result);

        },
        error: function (error) {
            alert(error)
        }
    });
    $(`#AssignZone-Input`).val(id);
}

function ChangeInvoiceType(event) {
    $.ajax({
        url: `/invoice/GetItemIds`,
        type: "GET",
        data: { invoiceType: event.target.value },
        success: function (result) {
            $(`#ItemId`).removeAttr("disabled");
            if (result) {
                // Clear the existing options
                $(`#ItemId`).html('<option value="">--- Select ---</option>');
                result.forEach(function (item) {
                    const option = `<option value="${item.value}">${item.key}</option>`;
                    $(`#ItemId`).append(option);
                });
            }
            else {
                $(`#ItemId`).html('<option value="">--- Select ---</option>');
            }
        },
        error: function () {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'error',
                showConfirmButton: false,
                timer: 5000
            });
        }
    });
}

function AssignArea() {
    var Description = "";
    let AreaList = $("#Areas-Modal input:checkbox:checked").map(function () {
        return $(this).val();
    }).get();
    let ZoneId = $(`#AssignZone-Input`).val();
    for (var i = 0; i < AreaList.length; i++) {
        Description += $(`#Area_${AreaList[i]}`).data('name') + " , ";
    }
    $.ajax({
        url: `/Admin/AssignAreas`,
        type: "GET",
        data: { ZoneId: ZoneId, AreasId: AreaList.toString() },
        success: function (result) {
            if (result) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Confirm',
                    showConfirmButton: false,
                    timer: 5000
                });
                $("#Areas-Modal input:checkbox").prop('checked', false);
                $(`#Td_${ZoneId}`).text(Description);
            }
            else {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'error',
                    showConfirmButton: false,
                    timer: 5000
                });
            }
        },
        error: function () {
            Swal.fire({
                position: 'center',
                icon: 'error',
                title: 'error',
                showConfirmButton: false,
                timer: 5000
            });
        }
    });
}

function ResetForm(FormID) {
    $(`#${FormID}`).trigger("reset");
    $("#BtnSend").prop('disabled', false);
    $(".Spinner").addClass("d-none");
    $(`.select2-selection__rendered`).text('--- Select ---');
    $(".form-control , .select2-selection").css({ "border-color": "#D6E4EC" });
    $(`.invalid-feedback , .field-validation-error`).addClass("d-none");
    $(`#ProductImg , #DisplayImg`).addClass(`d-none`);
}

// Sweet Alert PopUp Messege
function alertSuccess(title = 'Success', timer = 3000 ) {
    Swal.fire({
        position: 'center',
        icon: 'success',
        title: title,
        showConfirmButton: false,
        timer: timer
    });
}
function alertSuccessTop(title = 'Success', timer = 3000) {
    Swal.fire({
        position: 'top',
        icon: 'success',
        title: title,
        showConfirmButton: false,
        timer: timer
    });
}
function alertError(title = 'Failed, try again', timer = 3000) {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: title,
        showConfirmButton: false,
        timer: timer
    });
}
function ToastSuccess(title = 'Success', timer = 3000)
{
    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: timer,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: "success",
        title: title
    });
}
function ToastError(title = 'Error', timer = 3000) {
    const Toast = Swal.mixin({
        toast: true,
        position: "top-end",
        showConfirmButton: false,
        timer: timer,
        timerProgressBar: true,
        didOpen: (toast) => {
            toast.onmouseenter = Swal.stopTimer;
            toast.onmouseleave = Swal.resumeTimer;
        }
    });
    Toast.fire({
        icon: "error",
        title: title
    });
}

function authenticatedCheck(isAuthenticated, modal = null) {
    if (isAuthenticated == 'false') {
        $('#Login-PupUp-Modal').modal('show');
        if (modal != null) {
        $(`#${modal}`).modal('hide');
        }
        return false;
    } else return true;
}


//function showConfirmationDialog(n, t, i) { $("#modal-dialog-confirmation-messageTitle").text(n); $("#modal-dialog-confirmation-messageText").html(t); $("#modal-dialog-confirmation-aConfirm").unbind("click"); $("#modal-dialog-confirmation-aConfirm").click(function () { $("#modal-dialog-confirmation").modal("hide"); i() }); $("#modal-dialog-confirmation").modal("show") } $(document).ready(function () { $(".date-popup").datepicker({ keyboardNavigation: !1, forceParse: !1, todayHighlight: !0 }) });