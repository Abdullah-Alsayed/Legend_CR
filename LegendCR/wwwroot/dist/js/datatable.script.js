(function ($) {
    "use strict";
    var editor;
    var url = window.location.pathname.split("/");
    var ActionName = url[2];
    if (ActionName.toLowerCase().includes("Report") || ActionName.toLowerCase().includes("List") && !ActionName.toLowerCase().includes("Order") && !ActionName.toLowerCase().includes("Request")) {
        $('.dataTable').DataTable({
            //searching: true,
            ////dom: 'lBfrtip',
            ////buttons: [
            ////    'copy', 'csv', 'excel', 'pdf', 'print'
            ////],
            //responsive: true,
            searching: false,
            paging: false,
            dom: 'lBfrtip',
            buttons: [
                { extend: 'excel', className: 'DarkGreen-Bg' },
                { extend: 'pdf', className: 'DarkRed-Bg' },
                { extend: 'print', className: 'DarkGreen-Bg' }

            ],
            responsive: false,
            lengthMenu: [[-1, 10, 25, 50], ["All", 10, 25, 50]],
            aaSorting: []
        })
    }
    else {
        $('.dataTable').DataTable({
            searching: false,
            ////dom: 'lBfrtip',
            ////buttons: [
            ////    'copy', 'csv', 'excel', 'pdf', 'print'
            ////],
            //responsive: true,
            paging: false,
            dom: 'lBfrtip',
            buttons: [
                { extend: 'excel', className: 'DarkGreen-Bg' },
                { extend: 'pdf', className: 'DarkRed-Bg' },
                { extend: 'print', className: 'DarkGreen-Bg' }
            ],
            responsive: false,
            lengthMenu: [[-1, 10, 25, 50], ["All", 10, 25, 50]],
            aaSorting: []
        })
    }


    //.on('search.dt', function () {
       // var input = $('.dataTables_filter input')[0];
        //$('#Search').attr("action", `/User/OrderListPartial?Search=${input.value}`);
        //$('#Search').submit();
        //$(".spinner-border").css("display", "inline-block");
        //$("#Ajaxloader").css("display", "inline-block").fadeIn(20000)
        //$("#Ajaxloader").removeClass("d-none");
        //$.ajax({
        //    url: "/user/OrderListPartial",
        //    data: { Search:input.value },
        //    Success: function (Resut) {
        //        $(".spinner-border").css("display", "none");
        //        $("#Ajaxloader").fadeOut(1000);
        //        if (ActionName == "PaidReport") {
        //            $('#DataTable').DataTable({
        //                destroy: true,
        //                responsive: true,
        //                searching: true,
        //                "order": [],
        //                dom: 'Bfrtip',
        //                lengthMenu: [
        //                    [10, 25, 50, 100, -1],
        //                    ['10 rows', '25 rows', '50 rows', '100 rows', 'Show all']
        //                ],
        //                buttons: [
        //                    'copy', 'csv', 'excel', 'pdf', 'print', 'pageLength'
        //                ],
        //                columns: [
        //                    { visible: true }, //col 1
        //                    { visible: true }, //col 2
        //                    { visible: true }, //col 3
        //                    { visible: true }, //col 4
        //                    { visible: true }, //col 5
        //                    { visible: true }, //col 6
        //                    { visible: true }, //col 7
        //                    { visible: true }  //col 8
        //                ]
        //            });
        //            $("tfoot").remove();
        //        }
        //        else if (ActionName == "DriverReport") {
        //            $('#DataTable ,#TotalCanceledTbl ,#TotalDeliveredTbl').DataTable({
        //                destroy: true,
        //                responsive: true,
        //                searching: true,
        //                "order": [],
        //                dom: 'Bfrtip',
        //                lengthMenu: [
        //                    [10, 25, 50, 100, -1],
        //                    ['10 rows', '25 rows', '50 rows', '100 rows', 'Show all']
        //                ],
        //                buttons: [
        //                    'copy', 'csv', 'excel', 'pdf', 'print', 'pageLength'
        //                ]
        //            });
        //            $("tfoot").remove();
        //        }
        //        else if (ActionName == "AccountReport") {
        //            $('#DataTable').DataTable({
        //                destroy: true,
        //                responsive: true,
        //                searching: true,
        //                "order": [],
        //                dom: 'Bfrtip',
        //                lengthMenu: [
        //                    [10, 25, 50, 100, -1],
        //                    ['10 rows', '25 rows', '50 rows', '100 rows', 'Show all']
        //                ],
        //                buttons: [
        //                    'copy', 'csv', 'excel', 'pdf', 'print', 'pageLength'
        //                ]
        //            });
        //            $("tfoot").remove();
        //        }
        //        else if (ActionName == "ProblemsReport") {
        //        }
        //        else {
        //            $('#DataTable').DataTable({
        //                destroy: true,
        //                responsive: true,
        //                searching: true,
        //                'order': [[0, 'desc']],
        //                dom: 'Bfrtip',
        //                lengthMenu: [
        //                    [10, 25, 50, 100, -1],
        //                    ['10 rows', '25 rows', '50 rows', '100 rows', 'Show all']
        //                ],
        //                buttons: [
        //                    'copy', 'csv', 'excel', 'pdf', 'print', 'pageLength'
        //                ]
        //            });
        //        }
        //    }
        //})
   // })
})(jQuery);
