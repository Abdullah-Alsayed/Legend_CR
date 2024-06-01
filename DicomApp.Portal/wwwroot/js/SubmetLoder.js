
function GetGame(id) {
    $.get("/Game/GetGameList", { ID: id }, function (data) {
        $.each(data, function (index, Game) {
            $("#Gamediv").append('<label for="' + Game.id + '" class="radio-card"><input type="radio" value="' + Game.id + '" name="GameId" id="' + Game.id + '" /><div class="card-content-wrapper"><span class="check-icon"></span><div class="card-content"><img src="/dist/images/' + Game.imgUrl + '" alt=""/><h4 class="text-center">' + Game.nameEn + '</h4><h4 style="float: right;">' + Game.size + '</h4> <h4 class="float-end">Size</h4><h4 style="float: right;">' + Game.price + '</h4><h4>Price<h4/></div></div></label>')
        });
    });
    $("#Gamediv").empty();
}

$(document).ready(function () {
    $(".needs-validation").submit(function () {
        if ($(".needs-validation").valid()) {
            $(".se-pre-con").css("display", "block");
        }
    });
    $("#GameSelect").change(function () {
        $("#Gamediv").fadeOut("fast");
        var ID = $("#GameSelect").val();
        GetGame(ID);
        $("#Gamediv").slideDown(1000);
    });
    var ID = $("#GameSelect").val();
    GetGame(ID);
    $('.date-popup').datepicker({
        keyboardNavigation: false,
        forceParse: false,
        todayHighlight: true
    });

});

