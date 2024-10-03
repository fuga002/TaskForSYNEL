$(document).ready(function () {

    $('#myTable').DataTable({
        "scrollY": "450px",
        "scrollCollapse": true,
        "paging": true,
        "searching": true, 
        "info": true,      
        "lengthChange": true, 
        "pageLength": 10 
    });
});

