
var PartialItemsList = [];

function AddShipmentProduct() {
    if ($("#ShipmentProduct-Form").valid()) {
        var ShipmentProduct = {};
        $(`#ShipmentProduct-Form`).serializeArray().map(function (i) { ShipmentProduct[i.name] = i.value; });
        delete ShipmentProduct.__RequestVerificationToken;
        //Object.assign(ShipmentProduct, { file: ShipmentProductFile });
        //var img = document.getElementById("ShipmentProductImg");
        //img.src = "/dist/images/Partial.png";
        PartialItemsList.push(ShipmentProduct);
        $(`#ShipmentProduct-Form`).trigger("reset");
        $("#Partial-Items").append(`<span class="Partial-Shipment mr-2">${ShipmentProduct.Name}</span>`);

        var Price = parseInt(ShipmentProduct.Price);
        var Quantity = parseInt(ShipmentProduct.Quantity);
        var Total = Price * Quantity;

        var OldTotal = $(`#FeesDetailsDTO_Total`).val();
        var NewTotal = Number(OldTotal) + Number(Total);
        $(`#FeesDetailsDTO_Total`).val(NewTotal);

        GetTotalPrice();
    }
    else {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please Enter Item Info',
            showConfirmButton: false,
            timer: 5000
        });
    }
}

function CalcStockItemsPrice() {
    var ProductPrice = document.querySelectorAll(`.ProductPrice`);
    var ProductCount = document.querySelectorAll(`.ProductItemCount`);
    var IsChecked = document.querySelectorAll(`.ProductSelect`);
    var Total = 0;
    var Price, Count;
    var HasChecked = false;
    for (var i = 0; i < IsChecked.length; i++) {
        if (IsChecked[i].checked) {
            HasChecked = true;
            break;
        }
    }
    if (!HasChecked) {
        Swal.fire({
            position: 'center',
            icon: 'error',
            title: 'Please Select Product',
            showConfirmButton: false,
            timer: 4000
        });
        return false;
    }
    for (let i = 0; i < ProductPrice.length; i++) {
        if (IsChecked[i].checked) {
            if (ProductCount[i].value <= 0) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'Min Quantity is 1',
                    showConfirmButton: false,
                    timer: 4000
                });
                return false;
            }
            else {
                Price = parseInt(ProductPrice[i].value);
                Count = parseInt(ProductCount[i].value);
                Total += Price * Count;
            }
        }
    }
    $(`#FeesDetailsDTO_Total`).val(Total);
    $("#STOCKModel").modal('hide');
    GetTotalPrice();
}

function GetTotalPrice() {
    var shippingFees;
    var COD = $("#FeesDetailsDTO_Total").val();
    var AreaID = $("#AreaId").val();
    var GameID = $("input[name='SettingDTO.GameId']:checked").val();
    var Weight = $("#SettingDTO_Weight").val();
    $.get(`/Shipment/GetTotalPrice/?AreaID=${AreaID}&GameID=${GameID}`, function (data) {
        shippingFees = data.shippingFees;
        $(`#lblShippingFees`).val(data.shippingFees);
        $("#Zone-name").text(data.zoneName);
        var REDFees = Number(shippingFees) + Number(data.GameFees);
        if (Weight > 3) {
            REDFees += (Weight - 3) * 5;
        }
        if ($('#SettingDTO_IsPartialDelivery').is(':checked')) {
            REDFees += 10;
        }
        $("#txtREDFees").val(REDFees);
        $(".VendorCash").text((Number(COD) - Number(REDFees)) + " EGP");
        OrderSummary();
    })

    ShowHidePartialDelivery();
}

function OrderSummary() {
    let Summary = "";
    let Weight = $(`#SettingDTO_Weight`).val();
    let GameID = $("input[name='SettingDTO.GameId']:checked").val();
    let GameFess = $(`label[for='${GameID}'] .Game-Price`).text();
    let ShippingFees = $(`#lblShippingFees`).val();
    let REDFees = $(`#txtREDFees`).val();
    let VendorCash = $(".VendorCash").text();
    if (Weight > 3) {
        Summary += `<tr><td>Weight Fees</td><td>${(Weight - 3) * 5} EGP</td></tr>`;
    }
    if (GameID > 0) {
        Summary += `<tr><td>Game Fees</td><td>${GameFess}</td></tr>`;
    }
    if (ShippingFees > 0) {
        Summary += `<tr><td>Shipping Fees</td><td>${ShippingFees} EGP</td></tr>`;
    }
    if ($('#SettingDTO_IsPartialDelivery').is(':checked')) {
        Summary += `<tr><td>Partial Delivery Fees</td><td>${10} EGP</td></tr>`;
    }
    Summary += `<tr class="Red-Tr"><td>RED Fees</td><td>${REDFees} EGP</td></tr>`;
    Summary += `<tr class="Vendor-Tr"><td>Vendor Cash</td><td>${VendorCash.split("EGP")[0]} EGP</td></tr>`;
    $('#Order-Summary-tr').html(Summary);
    // $(`#Order-Summary tfoot`).removeClass(`d-none`);
    // $(`#Order-Summary-Total`).text(`${Total} EGP`);
}

function GetAreaByZoneID() {
    var ZoneID = $('#ZoneID').val();
    $(`#AreaId , [name='AreaId']`).empty();
    $(`#AreaId , [name='AreaId']`).append(`<option value>--- Select Area ---</option>`);
    $.get("/Admin/GetAreasByZone", { ID: ZoneID }, function (data) {
        $.each(data, function (key, value) {
            $(`#AreaId , [name='AreaId']`).append(`<option value="${value.id}">${value.cityNameAr}</option>`);
        });
    });
}

function OpenStockPopup(Id) {
    var Stock = $(`#SettingDTO_IsStock`).prop('checked');
    if (Stock) {
        GetStockPopup(Id);
        $(`.Edit-Stock-Items`).removeClass("d-none");
        //$("#Total").attr("readonly", "readonly");
    }
    else {
        $(`.Edit-Stock-Items`).addClass("d-none");
        //$("#Total").removeAttr("readonly");
    }
    ShowHidePartialDelivery();
}

function GetStockPopup(Id) {
    var VendorID = $(`#VendorDetailsDTO_VendorId`).val();
    $.ajax({
        type: "GET",
        url: `/Product/GetProductList`,
        contentType: 'application/html; charset=utf-8',
        data: { VendorID: VendorID },
        dataType: 'html',
        success: function (result) {
            $('#StockModalBody').html(result);
        },
        error: function (Error) {
            alert(Error)
        }
    })
    $(`#${Id}`).modal("show");
}

function ShowHidePartialDelivery() {
    var partialChecked = $('#SettingDTO_IsPartialDelivery').is(':checked');
    var stockChecked = $(`#SettingDTO_IsStock`).is(':checked');

    if (partialChecked) {
        //$("#Total").attr("readonly", "readonly");
        if (stockChecked) {
            $(`.Edit-Partial-Delivery`).addClass("d-none");
        }
        else {
            $(`.Edit-Partial-Delivery`).removeClass("d-none");
        }
    }
    else {
        $(`.Edit-Partial-Delivery`).addClass("d-none");
        if (stockChecked) {
            //$("#Total").attr("readonly", "readonly");
        }
        else {
            //$("#Total").removeAttr("readonly");
        }
    }
}

function ServiceTypeChange() {
    //let Service = $("#ShipmentServiceId").val();
    //if (Service === "3") {
    //    $("#DivWeight,#DivSize,#DivFreight,#DivOrderDescription").addClass("d-none");
    //    $("#Weight,#Size,#Freight").val(0);
    //} else
    //    $("#DivWeight,#DivSize,#DivFreight,#DivOrderDescription").removeClass("d-none");
}

function AddAdvertisement(ActionName, ControllerName, FormName) {
    $(".invalid-feedback").removeClass("d-none");
    if ($(`#${FormName}`).valid()) {

            $("#BtnSend").prop('disabled', true);
            $('#MainLoder').fadeIn(100);
            $("#MainView").hide();

            // Create FormData object
            let formData = new FormData($(`#${FormName}`)[0]);

            // Append files to formData
            let files = $(`#${FormName} input[type='file']`)[0].files;
            for (let i = 0; i < files.length; i++) {
                formData.append('files', files[i]);
            }

            $.ajax({
                url: `/${ControllerName}/${ActionName}`,
                contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                type: 'POST',
                data: formData,
                processData: false, 
                contentType: false, 
                dataType: 'html',
                success: function (response) {
                    if (response.includes('successfully')) {
                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: response,
                            showConfirmButton: false,
                            timer: 5000
                        });
                    }
                    else {
                        Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: response,
                            showConfirmButton: false,
                            timer: 5000
                        });
                    }

                    $("#MainView").fadeIn(1000);
                    $('#MainLoder').fadeOut(1000);
                    $(".se-pre-con").css("display", "none");

                    $(`#${FormName}`).trigger("reset");
                    deleteAllFiles();
                    $("#BtnSend").prop('disabled', false);
                },
                complete: function () { },
                error: function (response) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: response.Message,
                        showConfirmButton: false,
                        timer: 5000
                    });
                    $('#MainLoder').fadeOut(1000);
                    $(".se-pre-con").css("display", "none");
                    $("#MainView").show();
                    $("#BtnSend").prop('disabled', false);
                }
            })
        }
}

function EditAdvertisement(ActionName, ControllerName, FormName) {
    if ($(`#${FormName}`).valid())
    {
        $("#BtnSend").prop('disabled', true);
        $('#MainLoder').fadeIn(100);
        $("#MainView").hide();
        let DataForm = $(`#${FormName}`).serialize();
        let PartialItems = JSON.stringify(PartialItemsList);
        let VendorId = $('#VendorId').val();
        let Area = $('#AreaId').val();
        $.ajax({
            url: `/${ControllerName}/${ActionName}?PartialItems=${PartialItems}`,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            type: 'POST',
            data: DataForm,
            dataType: 'html',
            success: function (result) {
                $("#MainView").fadeIn(1000);
                $('#MainLoder').fadeOut(1000);
                $(".se-pre-con").css("display", "none");
                if (result == "false") {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'SubTotal Is Negative',
                        showConfirmButton: false,
                        timer: 4000
                    });
                }
                else {
                    $(`#${FormName}`).trigger("reset");
                    $(`#VendorId option[selected=selected]`).attr('selected', false);
                    $(`#VendorId option[value=${VendorId}]`).attr('selected', 'selected');
                    $(`#AreaId option[value=0]`).attr('selected', 'selected');
                    $(`#select2-AreaId-container`).text("--- Select Area ---");
                    $(`#select2-ZoneId-container`).text("--- Select Government ---");
                    $(`#select2-ShipmentServiceId-container`).text("--- Select Service ---");
                    $("#lblShippingFees").val("0");
                    $(".COD ,.VendorCash ,#Zone-name").text("");
                    $(".invalid-feedback").addClass("d-none");
                    $(".form-control , .select2-selection").css({ "border-color": "#D6E4EC" });
                    $(`.Edit-Stock-Items`).addClass("d-none");
                    $("#DivWeight,#DivSize,#DivFreight,#DivOrderDescription").removeClass("d-none");
                    $("#Partial-Items").empty();
                    //$("#Total").removeAttr("readonly");
                    PartialItemsList = [];
                    var StatusSelect = $('#select2-StatusId-container').text();
                    $('.StatusName').text(StatusSelect);
                    if (result != "false") {
                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: result,
                            showConfirmButton: false,
                            timer: 5000
                        });
                    }
                }
                $(`#Order-Summary-tr`).empty();
                $(`#Order-Summary-Total`).text(`0 EGP`);
                $(`#lblShippingFees`).val('');
                $("#BtnSend").prop('disabled', false);
            },
            complete: function () {
            },
            error: function (error) {
                Swal.fire({
                    position: 'center',
                    icon: 'error',
                    title: 'Failed Edit Order , try again',
                    showConfirmButton: false,
                    timer: 3000
                });
                $('#MainLoder').fadeOut(1000);
                $(".se-pre-con").css("display", "none");
                $("#MainView").show();
                $("#BtnSend").prop('disabled', false);
            }
        })
        
    }
    else {
        $("label:contains('This field is required.')").fadeOut();
    }
}

function updateAdvertisementFiles() {
    var files = $("#Advertisement-files")[0].files;
    var filesArray = Array.from(files);
    var validFiles = [];
    var imageCount = $('#Advertisement-Imgs img').length;

    filesArray.forEach(function (file) {
        var fileType = file.type.split('/')[0];
        if (fileType === 'image' && file.size <= 2 * 1024 * 1024) {
            if (imageCount < 5) {
                validFiles.push(file);
                var reader = new FileReader();
                reader.onload = function (e) {
                    var img = $('<img>', {
                        src: e.target.result,
                        class: 'uploaded-img',
                    });
                    $('#Advertisement-Imgs').append(img);
                };
                reader.readAsDataURL(file);
                imageCount++;
            }
        }
    });

    $('#Advertisement-files-Lable').text(imageCount + ' file(s) selected'); // Update count of total images

    if (validFiles.length < filesArray.length) {
        alert('Some files were not added. Ensure each file is an image, less than 2MB, and you select a maximum of 5 files.');
    }
}
function deleteAllFiles() {
    $('#Advertisement-Imgs').empty();
    $('#Advertisement-files-Lable').text('Not selected file');
}
function GetFeesSummary() {
    var RefundCash = $("#RefundCash").val();
    $("#lblRefundCash").html(RefundCash + " EGP");
    var AreaID = $("#AreaId").val();
    $.get(`/Shipment/GetAreaFees/?AreaID=${AreaID}`, function (data) {
        $(`#lblShippingFees`).html(data.shippingFees + " EGP");
        $("#Zone-name").text(data.zoneName);
        
        $("#lblTotalFees").html((Number(RefundCash) + Number(data.shippingFees)) + " EGP");
        $("#spanTotalFees").html((Number(RefundCash) + Number(data.shippingFees)) + " EGP");
    })
}