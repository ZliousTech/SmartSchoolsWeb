$(document).ready(function () {
    var SchoolInfonextBtn = $('.SchoolInfonextBtn');
    var DepartmentInfonextBtn = $('.DepartmentInfonextBtn');
    var CurriculumInfonextBtn = $('.CurriculumInfonextBtn');
    var PricesnextBtn = $('.PricesnextBtn');
    var FeesnextBtn = $('.FeesnextBtn');
    var DiscountnextBtn = $('.DiscountnextBtn');
    var TransportInfonextBtn = $('.TransportInfonextBtn');
    var LoginInfonextbtn = $('.LoginInfonextBtn');

    SchoolInfonextBtn.click(function () {
        $(document).off('click.SchoolInfonextBtn');
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid || curInputs[i].value.trim() === "") {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid) {
            //var dataObject = JSON.stringify({
            //    'schoolBranch.SchoolArabicName': $("#schoolBranch_SchoolArabicName").val(),
            //    'schoolBranch.SchoolContactNumber': $("#schoolBranch_SchoolContactNumber").val(),
            //    'schoolBranch.Longitude': $("#schoolBranch_Longitude").val(),
            //    'schoolBranch.Latitude': $("#schoolBranch_Latitude").val(),
            //    'SchoolEmail': $("#SchoolEmail").val(),
            //    'SchoolSettings.NumberofClassRooms': $("#SchoolSettings_NumberofClassRooms").val(),
            //    'SchoolSettings.NumberofChairsPerClass': $("#SchoolSettings_NumberofChairsPerClass").val(),
            //    'schoolBranch.SchoolID': $("#schoolBranch_SchoolID").val(),
            //    'schoolBranch.Country': $("#schoolBranch_Country").val(),
            //});

            debugger;
            var formData = new FormData();
            formData.append('schoolBranch.SchoolArabicName', $("#schoolBranch_SchoolArabicName").val());
            formData.append('schoolBranch.SchoolContactNumber', $("#schoolBranch_SchoolContactNumber").val());
            formData.append('schoolBranch.Longitude', $("#schoolBranch_Longitude").val());
            formData.append('schoolBranch.Latitude', $("#schoolBranch_Latitude").val());
            formData.append('SchoolEmail', $("#SchoolEmail").val());
            formData.append('SchoolSettings.NumberofClassRooms', $("#SchoolSettings_NumberofClassRooms").val());
            formData.append('SchoolSettings.NumberofChairsPerClass', $("#SchoolSettings_NumberofChairsPerClass").val());
            formData.append('schoolBranch.SchoolID', $("#schoolBranch_SchoolID").val());
            formData.append('schoolBranch.Country', $("#schoolBranch_Country").val());
            formData.append('file', $('#fileuploader')[0].files[0]);

            console.log(formData); 

            $.ajax({
                type: "POST",
                url: "SchoolInfoStepOne",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.Success) {
                        Swal.fire({
                            title: '',
                            text: 'تمت عملية الحفظ بنجاح',
                            icon: 'info',
                            timer: 1500,
                            timerProgressBar: true,
                            showConfirmButton: false,
                            position: 'top'
                        });
                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                        Swal.fire({
                            title: 'Error',
                            text: response.Message,
                            icon: 'error',
                            timer: 1500,
                            timerProgressBar: true,
                            showConfirmButton: false,
                            position: 'top'
                        });
                    }
                },
                failure: function (response) {
                    alert(response.responseText);
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });     
           }

           
        
    });


    DepartmentInfonextBtn.click(function () {
        debugger;
        $(document).off('click.DepartmentInfonextBtn');
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a");
        var Curriculumrows = $('#tblCurriculums tr td').length; //should be > 1
        var AcademictableRow = $("#tblDepartments td").filter(function () {
            return $(this).text() === "اكاديمي";
        }).closest("tr");
        if (Curriculumrows === 1 || AcademictableRow.length === 0) {
            Swal.fire({
                title: 'Error',
                text: 'الرجاء التأكد من إدخال الأقسام الأكاديمية والمناهج !',
                icon: 'error',
                timer: 5000,
                timerProgressBar: true,
                showConfirmButton: false,
                position: 'top'
            });
        }
        else {
            $.ajax({
                type: "GET",
                url: "GetCurriculumStep",
                cache: false,
                data: { 'SchoolID': $("#schoolBranch_SchoolID").val() }


            }).done(function (result) {
                debugger;
                $('#DivCurriculumPartial').html(result);
                nextStepWizard.removeAttr('disabled').trigger('click');

            });
        }
        
    });


    CurriculumInfonextBtn.click(function () {
        debugger;
        $(document).off('click.CurriculumInfonextBtn');
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a");
        var CheckedDepartments = [];
        var UncheckedDepartments = [];
        $("input[name='AcdCheckbox']").each(function () {
            if ($(this).is(":checked")) {
                CheckedDepartments.push($(this).attr('id'));
            }
            else {
                UncheckedDepartments.push($(this).attr('id'));

            }
        }); 
        $.ajax({
            type: "POST",
            url: "AddCurriculumDepartments",
            data: JSON.stringify({ 'CheckedDepartments': CheckedDepartments, 'UncheckedDepartments': UncheckedDepartments, 'SchoolID': $("#schoolBranch_SchoolID").val() }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            async: false,
            success: function (response) {
                debugger;
                if (response.NoCheckAcdError === "") {
                    $.ajax({
                        type: "GET",
                        url: "GetClassesStep",
                        cache: false,
                        data: { 'SchoolID': $("#schoolBranch_SchoolID").val() }


                    }).done(function (result) {

                        $('#DivClassesPartial').html(result);
                        nextStepWizard.removeAttr('disabled').trigger('click');

                    });
                }
                else {
                    Swal.fire({
                        title: 'Error',
                        text: 'الرجاء أختيار الاقسام الاكاديمية لكل منهاج  !',
                        icon: 'error',
                        timer: 10000,
                        timerProgressBar: true,
                        showConfirmButton: false,
                        position: 'top'
                    });
                }
            }
            //traditional: true
        });

        
    });
  
    $(document).off('click', '.PricesnextBtn').on('click', '.PricesnextBtn', function () {
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        //for (var i = 0; i < curInputs.length; i++) {
        //    if (!curInputs[i].validity.valid || curInputs[i].value.trim() === "") {
        //        isValid = false;
        //        $(curInputs[i]).closest(".form-group").addClass("has-error");
        //    }
        //}


        if (isValid) {
            var RadioBook = $("input[name='RadioBookPrice']:checked").val();
            var RadioUniform = $("input[name='RadioUniformPrice']:checked").val();

            var AllBookPrice = [];
            var AllUniformPrice = $("#UniformPrice").val();
            var BooksPrice = [];
            var UniformsPrice = [];
            $("#DivBooksPrice >div input").each(function () {
                var item = $(this).attr('id') + "," + $(this).val();
                AllBookPrice.push(item);
                
            });
        
            $("#DivBookPricePerClass >div >div input").each(function () {
                var item = $(this).attr('id') + "," + $(this).val();
                BooksPrice.push(item);
            });
            $("#DivUniformPricePerClass >div >div input").each(function () {
                var item = $(this).attr('id') + "," + $(this).val();
                UniformsPrice.push(item);
            });
    

            $.ajax({
                type: "POST",
                url: "AddUpdateBookUniformPrices",
                data: JSON.stringify({ 'BooksPrice': AllBookPrice, 'UniformsPrice': AllUniformPrice, 'BookPriceList': BooksPrice, 'UniformPriceList': UniformsPrice, 'RadioBook': RadioBook, 'RadioUniform': RadioUniform, 'SchoolID': $("#schoolBranch_SchoolID").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json"

            }).done(function () {
                $.ajax({
                    type: "GET",
                    url: "GetFeesStep",
                    cache: false,
                    data: { 'SchoolID': $("#schoolBranch_SchoolID").val() }

                }).done(function (result) {
                    $('#DivFeesPartial').html(result);
                    nextStepWizard.removeAttr('disabled').trigger('click');
                });
                Swal.fire({
                    title: '',
                    text: 'تمت عملية الحفظ بنجاح',
                    icon: 'info',
                    timer: 1000,
                    timerProgressBar: true,
                    showConfirmButton: false,
                    position: 'top'
                });

            });
        }
    });


    FeesnextBtn.click(function () {
        $(document).off('click.FeesnextBtn');
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        //for (var i = 0; i < curInputs.length; i++) {
        //    if (!curInputs[i].validity.valid || curInputs[i].value.trim() === "") {
        //        isValid = false;
        //        $(curInputs[i]).closest(".form-group").addClass("has-error");
        //    }
        //}


        if (isValid) {
            var RadioFee = $("input[name='RadioFeePrice']:checked").val();

            var AllFeePrice = [];
            var FeesPrice = [];
            $("#DivFeesPrice >div input").each(function () {
                var item = $(this).attr('id') + "," + $(this).val();
                AllFeePrice.push(item);
            });
            $("#DivFeePricePerClass >div >div input").each(function () {
                var item = $(this).attr('id') + "," + $(this).val();
                FeesPrice.push(item);
            });
       


            $.ajax({
                type: "POST",
                url: "AddUpdateSchoolFees",
                data: JSON.stringify({ 'FeesPrice': AllFeePrice, 'FeePriceList': FeesPrice,'RadioFee': RadioFee,'SchoolID': $("#schoolBranch_SchoolID").val() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json"
               

            }).done(function () {
    
                $.ajax({
                    type: "GET",
                    url: "GetDiscountsStep",
                    cache: false,
                    data: { 'SchoolID': $("#schoolBranch_SchoolID").val() }

                }).done(function (result) {
                    $('#DivDiscountsPartial').html(result);
                    nextStepWizard.removeAttr('disabled').trigger('click');
                });
                Swal.fire({
                    title: '',
                    text: 'تمت عملية الحفظ بنجاح',
                    icon: 'info',
                    timer: 1000,
                    timerProgressBar: true,
                    showConfirmButton: false,
                    position: 'top'
                });
            });
        }
    });

    DiscountnextBtn.click(function () {
        $(document).off('click.DiscountnextBtn');
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a");

        nextStepWizard.removeAttr('disabled').trigger('click');

    });

    TransportInfonextBtn.click(function () {
        $(document).off('click.TransportInfonextBtn');
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid || curInputs[i].value.trim() === "") {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }


        if (isValid) {
            var RadioDistanceCategory = $("input[name='DistanceCategory']:checked").val();
            if (RadioDistanceCategory === "2") {
                var Distance1 = $("#Distance_1").val();
                var Distance2 = $("#Distance_2").val();
                var Distance3 = $("#Distance_3").val();
                var Distance4 = $("#Distance_4").val();
                var Distance5 = $("#Distance_5").val();
                var GoCost_1 = $("#GoCost_1").val();
                var GoCost_2 = $("#GoCost_2").val();
                var GoCost_3 = $("#GoCost_3").val();
                var GoCost_4 = $("#GoCost_4").val();
                var GoCost_5 = $("#GoCost_5").val();
                var ReturnCost_1 = $("#ReturnCost_1").val();
                var ReturnCost_2 = $("#ReturnCost_2").val();
                var ReturnCost_3 = $("#ReturnCost_3").val();
                var ReturnCost_4 = $("#ReturnCost_4").val();
                var ReturnCost_5 = $("#ReturnCost_5").val();
                var TwoWay_1 = $("#TwoWay_1").val();
                var TwoWay_2 = $("#TwoWay_2").val();
                var TwoWay_3 = $("#TwoWay_3").val();
                var TwoWay_4 = $("#TwoWay_4").val();
                var TwoWay_5 = $("#TwoWay_5").val();
                var SchoolID = $("#schoolBranch_SchoolID").val();

                var dataObject = JSON.stringify({
                    'Distanceinmeters1': Distance1,
                    'Distanceinmeters2': Distance2,
                    'Distanceinmeters3': Distance3,
                    'Distanceinmeters4': Distance4,
                    'Distanceinmeters5': Distance5,

                    'GoCost_1': GoCost_1,
                    'GoCost_2': GoCost_2,
                    'GoCost_3': GoCost_3,
                    'GoCost_4': GoCost_4,
                    'GoCost_5': GoCost_5,

                    'ReturnCost_1': ReturnCost_1,
                    'ReturnCost_2': ReturnCost_2,
                    'ReturnCost_3': ReturnCost_3,
                    'ReturnCost_4': ReturnCost_4,
                    'ReturnCost_5': ReturnCost_5,

                    'TwoWay_1': TwoWay_1,
                    'TwoWay_2': TwoWay_2,
                    'TwoWay_3': TwoWay_3,
                    'TwoWay_4': TwoWay_4,
                    'TwoWay_5': TwoWay_5,
                    'SchoolID': SchoolID

                });

                $.ajax({
                    type: "POST",
                    url: "AddUpdateTransportCategories",
                    data: dataObject,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response !== null) {
                            nextStepWizard.removeAttr('disabled').trigger('click');

                        } else {
                            alert("Something went wrong");
                        }
                    },
                 
                });
            }
            else {
                nextStepWizard.removeAttr('disabled').trigger('click');
            }
        }
    });

    LoginInfonextbtn.click(function () {
        $(document).off('click.LoginInfonextbtn');
        $(this).closest(".setup-content").unbind("click");
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],input[type='password'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid || curInputs[i].value.trim() === "") {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }


        if (isValid) {
            var UserName = $("#UserName").val();
            var Password = $("#Password").val();
            var ConfirmPassword = $("#Confirm").val();
            var SchoolID= $("#schoolBranch_SchoolID").val();
            if (Password === ConfirmPassword) {
                $.ajax({
                    type: "GET",
                    url: "LoginInfoStep",
                    data: { 'UserName': UserName, 'Password': Password, 'SchoolID': SchoolID },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json"
                

                }).done(function () {
                    Swal.fire(
                        'تم  التسجيل بنجاح',
                        '',
                        'success'
                    ).then(function () {
                        window.location.href = 'RegistrationStepTwo';
                    });
                });

            }
            else {
                $("#ErrorPassword").css("display", "");
            }
        }
    });
});

