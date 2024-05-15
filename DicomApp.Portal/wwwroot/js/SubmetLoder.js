
function GetPacking(id) {
    $.get("/Packing/GetPackingList", { ID: id }, function (data) {
        $.each(data, function (index, Packing) {
            $("#Packingdiv").append('<label for="' + Packing.id + '" class="radio-card"><input type="radio" value="' + Packing.id + '" name="PackingId" id="' + Packing.id + '" /><div class="card-content-wrapper"><span class="check-icon"></span><div class="card-content"><img src="/dist/images/' + Packing.imgUrl + '" alt=""/><h4 class="text-center">' + Packing.nameEn + '</h4><h4 style="float: right;">' + Packing.size + '</h4> <h4 class="float-end">Size</h4><h4 style="float: right;">' + Packing.price + '</h4><h4>Price<h4/></div></div></label>')
        });
    });
    $("#Packingdiv").empty();
}

$(document).ready(function () {
    $(".needs-validation").submit(function () {
        if ($(".needs-validation").valid()) {
            $(".se-pre-con").css("display", "block");
        }
    });
    $("#PackingSelect").change(function () {
        $("#Packingdiv").fadeOut("fast");
        var ID = $("#PackingSelect").val();
        GetPacking(ID);
        $("#Packingdiv").slideDown(1000);
    });
    var ID = $("#PackingSelect").val();
    GetPacking(ID);
    $('.date-popup').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        todayHighlight: true
    });

});

