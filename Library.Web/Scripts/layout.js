var token =  window.localStorage["accessToken"];
var headers = {};
if (token) {
    headers.Authorization = 'Bearer ' + token;
}
$(function () {
   
    $("#logout").on("click",
       function (e) {
           e.preventDefault();
           $.ajax({
               type: "POST",
               url: "api/account/logout",
               headers:headers,
               success: function () {
                   sessionStorage.clear();
                   localStorage.clear();
                   window.location.href = "/";
               },
               error: function (error) {
                   alert(error);
               }
           });
       });
    $("#changePasswordForm").submit(function (e) {
        e.preventDefault();
        var self = $(this);
        var url = self.attr("action");
        var data = self.serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            headers: headers,
            success: function (response) {
                alert("Password change successful");
                window.location.href = "/profile";
            },
            statusCode: {
                400: function () {
                    $.each(error.responseJSON.ModelState,
                        function (error, errorText) {
                            switch (error) {
                                case "model.OldPassword":
                                    showError($("#oldPasswordError"), errorText[0]);
                                    break;
                                case "model.Password":
                                    showError($("#passwordError"), errorText[0]);
                                    break;
                                case "model.ConfirmPassword":
                                    showError($("#confirmPasswordError"), errorText[0]);
                                    break;
                            }
                        });
                }
            }
        });
    });
    $("#createBook").submit(function(e) {
        e.preventDefault();
        var authors = $("#author").val();
        authors = authors.split(",");
        var authorList = [];
        for (var i = 0; i < authors.length; i++) {
            var tempObj = { "AuthorName": authors[i] };
            authorList.push(tempObj);
        }
        var bookTitle = $("#bookTitle").val();
        var isbn = $("#isbn").val();
        var publishYear = $("#publishYear").val();
        var count = $("#count").val();
        var postData = {
            BookTitle : bookTitle,Isbn:isbn,PublishYear : publishYear,Count:count,Author:authorList
        };
        $.ajax({
            type: "POST",
            url: "/api/books",
            data: postData,
            headers: headers,
            success:function(response) {
                alert("kitap başarıyla eklendi");
            },
            statusCode : {
                400 : function() {

                }
            }
        });
    });

    $("body").on("click","#deleteBook",function (e) {
        e.preventDefault();
        var self = $(this);
            $.ajax({
                type: "DELETE",
                url: "/api/books?isbn=" + self.attr("data-id"),
                headers: headers,
                success: function() {
                    alert("kitap başarıyla silindi");
                    $("#bookValues").remove();
                    loadData();
                },
                statusCode: {
                404: function() {
                    alert("kitap silinirken bir hata ile karşılaşıldı.");
                }
            }
    });
   });


});
function showError(obj, error) {
    obj.removeClass("passive-error");
    obj.addClass("active-error");
    obj.text(error);
}

function getApiValues() {
    var token = sessionStorage.getItem("accessToken");
    var headers = {};
    if (token) {
        headers.Authorization = 'Bearer ' + token;
    }
    $.ajax({
        type: "GET",
        url: "/api/values",
        headers: headers
    }).done(function (data) {

    }).fail(function (data) {

    });
}
function loadData() {
    $.ajax({
        type: "GET",
        url: "/api/books",
        headers: headers,
        success: function (data) {
            createTable(data);
        }
    });
}

function createTable(data) {
    var x = "<tbody id='bookValues'>";
     
    for (var i = 0; i < data.length; i++) {
        x += "<tr>";
        x += "<th><a href='/bookoperations/bookindex/" + data[i].Isbn + "'>"+data[i].Isbn+"</a></th>";
        x += "<th>" + data[i].BookTitle + "</th>";
        x += "<th>";
        for (var j = 0; j < data[i].Authors.length; j++) {
            x += data[i].Authors[j].AuthorName + ", ";
        }
        x += "</th>";
        x += "<th>" + data[i].PublishYear + "</th>";
        x += "<th>" + data[i].Count + "</th>";
        x += "<th><a class='badge badge-primary' href=/bookoperations/edit/" + data[i].Isbn + ">Düzenle</a></th>";
        x += "<th><a class='badge badge-primary' id='deleteBook' data-id='" + data[i].Isbn + "' href=/bookoperations/delete/" + data[i].Isbn + ">Sil</a></th>";
        x += "</tr>";
    }
    x += "</tbody>";
    $("#bookData").append(x);
}
function loadReserveData(isbn) {
    $.ajax({
        type: "GET",
        url: "/api/books?isbn=" + isbn,
        headers: headers,
        success: function (data) {
            createReserveTable(data);
        }
    });
}

function createReserveTable(data) {
    var x = "<tbody id='bookValues'>";
   
    for (var i = 0; i < data.Reserves.length; i++) {
        var reserveState = data.Reserves[i].ReserveState;
        if (reserveState==0) {
            x += "<tr class='table-primary'>";
        } else if (reserveState == 1) {
            x += "<tr class=\"table-success\">";
        } else {
            x += "<tr class=\"table-danger\"> ";
        }
        x += "<th>" + data.Isbn + "</th>";
        x += "<th>" + data.BookTitle + "</th>";
        x += "<th>";
        for (var j = 0; j < data.Authors.length; j++) {
            x += data.Authors[j].AuthorName + ", ";
        }
        x += "</th>";
        x += "<th>" + data.Count + "</th>";
        x += "<th>" + data.Reserves[i].ReserveDate + "</th>";
        x += "<th>" + data.Reserves[i].ReturnDate + "</th>";
        x += "<th>" + data.Reserves[i].UserReturnedDate + "</th>";
        x += "</tr>";
    }
    x += "</tbody>";
    $("#bookReserveData").append(x);
}
function updateBook(url){
    var authors = $("#author").val();
    authors = authors.split(",");
    var authorList = [];
    for (var i = 0; i < authors.length; i++) {
        var tempObj = { "AuthorName": authors[i] };
        authorList.push(tempObj);
    }
    var bookTitle = $("#bookTitle").val();
    var isbn = $("#isbn").val();
    var publishYear = $("#publishYear").val();
    var count = $("#count").val();
    var postData = {
        BookTitle: bookTitle, Isbn: isbn, PublishYear: publishYear, Count: count, Author: authorList
    };
    $.ajax({
        type: "PUT",
        url: "/api/books?isbn="+url,
        data: postData,
        headers: headers,

        success: function (response) {
            alert("Kitap başarıyla güncellendi.");
        },
        statusCode: {
            400: function () {

            }
        }
    });
};
function loadEditBook(url) {
    var authors = "";
    $.ajax({
        type: "GET",
        url: "/api/books?isbn=" + url,
        headers: headers,
        success: function (data) {
            $("#bookTitle").val(data.BookTitle);
            $("#isbn").val(data.Isbn);
            $("#publishYear").val(data.PublishYear);
            $("#count").val(data.Count);
            for (var i = 0; i < data.Authors.length; i++) {
                authors += data.Authors[i].AuthorName + ",";
            }
            $("#author").text(authors);
        },
        statusCode: {
            404: function () {
                alert("Sorgulanan kitap veri tabanında bulunamadı");
            }
        }
    });
}