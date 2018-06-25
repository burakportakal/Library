function callApi() {
    $.ajax({
        type: "GET",
        url: "/api/books/reserve",
        headers: headers,
        success: function (data) {
            getReserves(data);
        },
        statusCode: {
            403: function (data) {
                alert(data.responseJSON.message);
            }
        }
    });
}

function getReserves(data) {
    var x = "<tbody id='bookValues'>";
    var y = "<tbody id='returnedBooks'>";
    for (var i = 0; i < data.length; i++) {
        if (data[i].ReserveState == 0 || data[i].ReserveState == 2) {
            x += createTable(data, i,false);
        } else {
            y += createTable(data, i,true);
        }
    }
    y += "</tbody>";
    x += "</tbody>";
    $("#booksReturned").append(y);
    $("#booksReserved").append(x);
}
function createTable(data, i, returned) {
    var x = "";
    x += "<tr>";
    x += "<th>" + data[i].BookTitle + "</th>";
    x += "<th>";
    for (var j = 0; j < data[i].Authors.length; j++) {
        x += data[i].Authors[j].AuthorName + ", ";
    }
    x = x.substring(0, x.length-2);
    x += "</th>";
    var date = new Date(data[i].ReserveDate);
    x += "<th>" + date.format("dd.mm.yyyy") + "</th>";
    if (!returned) {
        date = new Date(data[i].ReturnDate);
        x += "<th>" + date.format("dd.mm.yyyy") + "</th>";
        x += "<th><button type='button' onclick='returnBook(this)' data-id='" +
            data[i].ReserveId +
            "' class='btn btn-primary'>İade Et</button></th>";
    } else {
        date = new Date(data[i].UserReturnedDate);
        x += "<th>" + date.format("dd.mm.yyyy") + "</th>";
    }
    x += "</tr>";
    return x;
}
function returnBook(obj) {
    var reserveId = $(obj).attr("data-id");
    $.ajax({
        type: "PUT",
        url: "/api/books/reserve/"+reserveId,
        headers: headers,
        success: function (data) {
            if(alert("iade başarılı"));
            {
                location.reload();
            }
        },
        statusCode: {
            400: function (data) {
                alert(data.responseJSON);
            }
        }
    });
}
$(function() {
    $("#pills-reserved-tab").on("click",
        function () {
            $("#pills-returned-tab").removeClass("active");
            $("#pills-reserved-tab").removeClass("active");
            $("#pills-reserved-tab").addClass("active");
        });
    $("#pills-returned-tab").on("click",
        function () {
            $("#pills-returned-tab").removeClass("active");
            $("#pills-reserved-tab").removeClass("active");
            $("#pills-returned-tab").addClass("active");
        });
})