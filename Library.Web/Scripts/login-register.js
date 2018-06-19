$(function() {
    $("#registerForm").submit(function (e) {
        removeErrors();
        e.preventDefault();
        
        var self = $(this);
        var url = self.attr("action");
        var data = self.serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                alert("Account created");
                window.location.href = "/home/login";
            },
            statusCode: {
                403: function () {
                    // Only if your server returns a 403 status code can it come in this block. :-)
                    alert("Username already exist");
                    removeErrors();
                },
                400: function (error) {
                    $.each(error.responseJSON.ModelState, function (error, errorText) {
                        switch (error) {
                            case "model.Email":
                                showError($("#emailError"), errorText[0]);
                                break;
                            case "model.Password":
                                showError($("#passwordError"), errorText[0]);
                                break;
                            case "model.ConfirmPassword":
                                showError($("#confirmPasswordError"), errorText[0]);
                                break;
                            default:
                                alert(errorText);
                                break;
                        }
                    });
                }
            }
        });
    });
    $("#loginForm").submit(function (e) {
        e.preventDefault();
        var self = $(this);
        var url = self.attr("action");
        var data = self.serialize();
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                window.localStorage['accessToken'] = response.access_token;
                var returnUrl = getParameterByName("returnUrl");
                if (returnUrl != null) {
                    window.location.href = returnUrl;
                } else {
                    window.location.href = "/";
                }
            },
            statusCode: {
                404: function () {
                    alert("Incorrect email or password");
                },
                400: function () {
                    alert("Incorrect email or password");
                }
            }
        });
    });
    function showError(obj, error) {
        obj.removeClass("passive-error");
        obj.addClass("active-error");
        obj.text(error);
    }
    function removeErrors() {
        $("#emailError").text("");
        $("#passwordError").text("");
        $("#confirmPasswordError").text("");
    }
});
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}