$(document).ready(function () {

    $('#myTable').DataTable({
        "scrollY": "450px",
        "scrollCollapse": true,
        "paging": true,
        "searching": true, // Enable searching (this is default, but just to ensure)
        "info": true,      // Display info (optional)
        "lengthChange": true, // Enable changing page length
        "pageLength": 10 
    });
});
