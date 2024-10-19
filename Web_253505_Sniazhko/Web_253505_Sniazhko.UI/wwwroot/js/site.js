// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $(".ajax-link").on("click", function (event) {
        event.preventDefault();
        var url = $(this).attr("href");
        $.ajax({
            url: url,
            type: 'GET',
            success: function (data) {
                $('#dishList').html(data);
            },
            error: function () {
                alert("Произошла ошибка при загрузке данных.");
            }
        });
    })

})