
$(document).ready(function () {

    var table = $('#exportTable').DataTable({
        lengthChange: true,
        dom: 'Bflrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        responsive: true
    });
    table.buttons().container().appendTo('#example_wrapper .col-md-6:eq(0)');

});


//$(document).ready(function () {

//    var table = $('#exportTable').DataTable({
//        lengthChange: true,
//        dom: 'Bflrtip',
//        buttons: [
//            'copy', 'csv', 'excel', 'pdf', 'print'
//        ],
//        responsive: true,
//        'columnDefs': [{
//            'targets': [6], /* column index */
//            'orderable': false, /* true or false */
//        }],
//        'lengthMenu': [[10, 25, 50, -1], [10, 25, 50, "All"]]

//    });
//    table.buttons().container().appendTo('#example_wrapper .col-md-6:eq(0)');

//});