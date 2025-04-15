// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('.dataTable').DataTable({
        responsive: true
    });
});

$(document).ready(function () {
    $('.select-2').select2();
});

$('.fast-tooltip').tooltip({ show: { effect: "none", delay: 0 } });