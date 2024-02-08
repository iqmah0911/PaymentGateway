
    var expensetable = $("#offrptTable").DataTable({
       
        dom: 'Bfrtip', 
        //dom: 'Afrtip',
        lengthMenu: [
            [10, 25, 50, -1],
            ['10 rows', '25 rows', '50 rows', 'Show all']
        ], 
        buttons: [
            'pageLength',
            //'copyHtml5',
            //'excelHtml5',
            //'csvHtml5',
            //'pdfHtml5',
            { extend: 'copyHtml5', footer: true },
            { extend: 'excelHtml5', footer: true },
            { extend: 'csvHtml5', footer: true },
            { extend: 'pdfHtml5', footer: true }
        ]
    });
 
    var summed = expensetable.column(5).data().sum();

    var tocur = (summed).toLocaleString('en-Ng', {
        style: 'currency',
        currency: 'NGN',
    });

    $("#amt-summed").text("Total: " + tocur);

//"bFilter": false,
//    "sorting": false,
//        "bInfo": false, //Dont display info e.g. "Showing 1 to 4 of 4 entries"
//            "paging": false,


