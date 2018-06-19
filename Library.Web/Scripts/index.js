var token = window.localStorage["accessToken"];
var headers = {};
if (token) {
    headers.Authorization = 'Bearer ' + token;
}
function loadAuthData() {
    $.ajax({
        type: "GET",
        url: "/api/books",
        headers:headers,
        success: function (data) {
            createAuthTable(data);
        },
        error: function () {
            alert("Kitaplar getirilirken bir hata oluştu!");
        }
    });
}
function loadData() {
    $.ajax({
        type: "GET",
        url: "/api/books",
        success: function (data) {
            createTable(data);
        },
        error: function() {
            alert("Kitaplar getirilirken bir hata oluştu!");
        }
    });
}
function createTable(data) {
    var x = "<tbody id='bookValues'>";

    for (var i = 0; i < data.length; i++) {
        x += "<tr>";
        x += "<th>"+ data[i].Isbn + "</th>";
        x += "<th>" + data[i].BookTitle + "</th>";
        x += "<th>";
        for (var j = 0; j < data[i].Authors.length; j++) {
            x += data[i].Authors[j].AuthorName + ", ";
        }
        x += "</th>";
        x += "<th>" + data[i].PublishYear + "</th>";
        x += "<th>" + data[i].Count + "</th>";
        x += "</tr>";
    }
    x += "</tbody>";
    $("#bookData").append(x);
}
function createAuthTable(data) {
    var x = "<tbody id='bookValues'>";

    for (var i = 0; i < data.length; i++) {
        x += "<tr>";
        x += "<th>" + data[i].Isbn + "</th>";
        x += "<th>" + data[i].BookTitle + "</th>";
        x += "<th>";
        for (var j = 0; j < data[i].Authors.length; j++) {
            x += data[i].Authors[j].AuthorName + ", ";
        }
        x += "</th>";
        x += "<th>" + data[i].PublishYear + "</th>";
        x += "<th>" + data[i].Count + "</th>";
        x += "<th><button type='button' onclick='reserve(this)' data-id='"+data[i].Isbn+"' class='btn btn-primary'>Kirala</button></th>";
        x += "</tr>";
    }
    x += "</tbody>";
    $("#bookData").append(x);
}
function reserve(data) {
    var self = $(data);
    console.log(self);
    var isbn = self.attr("data-id");
    $.ajax({
        type: "POST",
        url: "/api/books/reserve/"+isbn,
        headers: headers,
        success: function (data) {
           if(alert("Kiralama başarılı"));
            {
                location.reload();
            }
        },
        statusCode:{
            403: function(data) {
                alert(data.responseJSON.message);
            }
        }
    });
}
$(function () {

});