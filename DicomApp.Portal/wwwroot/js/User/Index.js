
function DeleteUserCheck(url, title, msg1, msg2, userID) {
    var msg = msg1 + "<br/>" + msg2;
    showConfirmationDialog(title, msg, function () { DeleteUser(url, userID) });
}

function DeleteUser(url, userID, Row) {
    Swal.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                data: {
                    userID: userID
                },
                success: function (response) {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: response,
                        showConfirmButton: false,
                        timer: 3000
                    });
                    $(`#${Row}`).fadeOut(1000);
                    setTimeout(function () { $(`#${Row}`).remove() }, 2000);
                }
            });
        }
    })

}


function loadUsers(url, pageIndex) {
    var searchVal = $("#inpt-srch").val();
    var orderByColumn = $("#ddl-order").val();
    $.ajax({
        url: url,
        type: "get",
        data: {
            orderByColumn: orderByColumn,
            searchVal: searchVal,
            pageIndex: pageIndex
        },
        success: function (res) {
            $("#div-users").html(res);
        }
    });
}

function Enter(event, action) {
     $(".Spinner").removeClass("d-none");
    if ($(`#${action}-form`).valid()) {

        let formData = $(`#${action}-form`).serialize();
        $.ajax({
            url: `/User/${action}`,
            type: "POST",
            data: formData,
            success: function (result) {
                $(".Spinner").addClass("d-none");
                if (!result.success)
                    $(`#ErrorMessage`).text(result.message);
                else {
                    location.href = "/Gamer/Main";
                $(`#ErrorMessage`).text('');
                }

            },
            error: function (err) {
                $(".Spinner").addClass("d-none");
                alert("try again!")
                console.log(err);
            }
        });
    } else {
        $(".Spinner").addClass("d-none");
        $(`#${action}-form`).submit();
    }
}

