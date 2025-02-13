$(document).ready(function () {
    var ParentsStepOneInfonextBtn = $('.ParentsStepOneInfonextBtn');
    var ParentsStepTwonextBtn = $('.ParentsStepTwonextBtn');
    var StudentInfonextBtn = $('.StudentInfonextBtn');
    var SchoolInfonextBtn = $('.SchoolInfonextBtn');
    var AddressInfonextBtn = $('.AddressInfonextBtn');
    var SocialnextBtn = $('.SocialnextBtn');
    var HealthInfonextBtn = $('.HealthInfonextBtn');
    var QuestionInfonextBtn = $('.QuestionInfonextBtn');

    //step 1 
    ParentsStepOneInfonextBtn.click(function () {
        debugger;
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-control-wrapper").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-control-wrapper").addClass("has-error");
            }
        }
        if (isValid) {
            debugger;
            var dataObject = JSON.stringify({
                'Guardian.GuardianArabicName': $("#Guardian_GuardianArabicName").val(),
                'Guardian.GuardianEnglishName': $("#Guardian_GuardianEnglishName").val(),
                'Guardian.NationalNumber': $("#Guardian_NationalNumber").val(),
                'Guardian.Gender': $("input[name='Guardian.Gender']:checked").val(),
                'Guardian.Nationality': $("#Guardian_Nationality option:selected").val(),
                'Guardian.Religion': $("input[name='Guardian.Religion']:checked").val(),
                'Guardian.ResidencyNumber': $("#Guardian_ResidencyNumber").val(),
                'Guardian.PassportNumber': $("#Guardian_PassportNumber").val(),
                'Guardian.MobileNumber': $("#Guardian_MobileNumber").val(),
            });
            $.ajax({
                type: "POST",
                url: "ParentsInfoStepOne",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                        $("#GurdianID").val(response.GurdianID);
                        swal("تمت عملية الحفظ بنجاح", "", "success");
                        nextStepWizard.removeAttr('disabled').trigger('click');
                    } else {
                        swal("حدث خطا ما", "", "error");
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
    // end of step 1

    //step 2
    ParentsStepTwonextBtn.click(function () {
        debugger;
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-control-wrapper").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-control-wrapper").addClass("has-error");
            }
        }

        if (isValid) {
            debugger;
            var dataObject = JSON.stringify({
                'student.GuardianRelationship': $("#student_GuardianRelationship").val(),
                'GurdianID': $("#GurdianID").val(),
                'GuardianMobileNumber': $("#GuardianMobileNumber").val(),
                'smsNumber': $("#smsNumber").val(),
                'FatherName': $("#FatherName").val(),
                'MotherName': $("#MotherName").val(),
                'FatherQualification': $("#FatherQualification").val(),
                'MotherQualification': $("#MotherQualification").val(),
                'FatherSpecialization': $("#FatherSpecialization").val(),
                'MotherSpecialization': $("#MotherSpecialization").val(),
                'FatherJob': $("#FatherJob").val(),
                'MotherJob': $("#MotherJob").val(),
                'FatherMobile': $("#FatherMobile").val(),
                'MotherMobile': $("#MotherMobile").val(),
                'FatherWorkPhone': $("#FatherWorkPhone").val(),
                'MotherWorkPhone': $("#MotherWorkPhone").val(),
                'FatherEmail': $("#FatherEmail").val(),
                'MotherEmail': $("#MotherEmail").val(),
                'mailBox': $("#mailBox").val(),
                'PostalCode': $("#PostalCode").val(),
                'GuardianName': $("#GuardianName").val(),
                'StudentID': $("#StudentID").val()
            });

            $.ajax({
                type: "POST",
                url: "ParentsInfoStepTwo",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {

                    alert(response.Success);

                    if (response.Success) {
                        swal("تمت عملية الحفظ بنجاح", "", "success");

                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                        swal("حدث خطا ما", "", "error");
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
    //end of step 2


    //step 3
    StudentInfonextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }
        if ($("#profileImage").attr('src').includes('camera2.png') && $("#fileuploader").get(0).files.length === 0) {
            swal("الرجاء تحميل الصورة الشخصية", "", "error");
            isValid = false;

        }

        if (isValid) {
            var formData = new FormData();
            var files = $("#fileuploader").get(0).files;
            var FirstArabicName = $("#FirstArabicName").val();
            var SecondArabicName = $("#SecondArabicName").val();
            var ThirdArabicName = $("#ThirdArabicName").val();
            var FourthArabicName = $("#FourthArabicName").val();
            var Gender = $("input[name='student.Gender']:checked").val();
            var NationalNumber = $("#student_NationalNumber").val();
            var ResidencyNumber = $("#student_ResidencyNumber").val();
            var DateofBirth = $("#DateofBirth").val();
            var Nationality = $("#student_Nationality option:selected").val();
            var BirthPlace = $("#BirthPlace").val();
            var BirthCertificateNumber = $("#BirthCertificateNumber").val();
            formData.append('file', files[0]);
            formData.append('FirstArabicName', FirstArabicName);
            formData.append('SecondArabicName', SecondArabicName);
            formData.append('ThirdArabicName', ThirdArabicName);
            formData.append('FourthArabicName', FourthArabicName);
            formData.append('FourthArabicName', FourthArabicName);
            formData.append('student.Gender', Gender);
            formData.append('student.NationalNumber', NationalNumber);
            formData.append('student.ResidencyNumber', ResidencyNumber);
            formData.append('DateofBirth', DateofBirth);
            formData.append('student.Nationality', Nationality);
            formData.append('BirthPlace', BirthPlace);
            formData.append('BirthCertificateNumber', BirthCertificateNumber);
            formData.append('GurdianID', $("#GurdianID").val());

            $.ajax({
                type: "POST",
                url: "StudentInfoStepThree",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response.Success) {
                        debugger;
                        $("#StudentID").val(response.StudentID);
                        swal("تمت عملية الحفظ بنجاح", "", "success");
                        nextStepWizard.removeAttr('disabled').trigger('click');
                    } else {
                        swal("حدث خطا ما", "", "error");

                    }
                }

            });
        }
    });
    //end of step3

    //step 4 
    SchoolInfonextBtn.click(function () {
        debugger;
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid) {
            debugger;
            var dataObject = JSON.stringify({
                'SchoolDetail.ClassID': $("#SchoolClassID").val(),
                'SchoolDetail.EductionalYear': $("#SchoolDetail_EductionalYear").val(),
                'SchoolDetail.CountryID': $("select[name='SchoolDetail_CountryID'] option:selected").val(),
                'SchoolDetail.CompanyID': $("#CompanyID").val(),
                'SchoolDetail.SchoolID': $("#CompanySchoolID").val(),
                'SchoolDetail.LastSchoolName': $("#SchoolDetail_LastSchoolName").val(),
                'SectionID': $("#sectionId").val(),
                'StudentID': $("#StudentID").val()

            });
            $.ajax({
                type: "POST",
                url: "SchoolInfoStepFour",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                        swal("تمت عملية الحفظ بنجاح", "", "success");

                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                        swal("حدث خطا ما", "", "error");
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
    //end of step 4

    //step 5
    AddressInfonextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid) {

            var dataObject = JSON.stringify({
                'StudentAddress.CountryID': $("#StudentAddress_CountryID").val(),
                'StudentAddress.City': $("#StudentAddress_City").val(),
                'StudentAddress.Street': $("#StudentAddress_Street").val(),
                'StudentAddress.Longitude': $("#StudentAddress_Longitude").val(),
                'StudentAddress.Latitude': $("#StudentAddress_Latitude").val(),
                'StudentAddress.Building': $("#StudentAddress_Building").val(),
                'GurdianID': $("#GurdianID").val(),
                'StudentID': $("#StudentID").val()
            });
            $.ajax({
                type: "POST",
                url: "AddressInfoStepFive",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                      
                        $.ajax({
                            type: "GET",
                            url: "GetDiscounts",
                            cache: false,
                            data: { 'StudentID': $('#StudentID').val() }
                        }).done(function (result) {
                            debugger;
                            swal("تمت عملية الحفظ بنجاح", "", "success");
                            $('#DivStudentQuestion').html(result);
                            nextStepWizard.removeAttr('disabled').trigger('click');

                        });
                    }
                    else {
                        swal("حدث خطا ما", "", "error");
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
    //end of step 5

    //setp 6 
    QuestionInfonextBtn.click(function () {
        debugger;
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='radio']"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }
        if (isValid) {
            if ($("#DivStudentQuestion").find('input').length <= 1) {
                nextStepWizard.removeAttr('disabled').trigger('click');
                return;
            }
            var CheckedDiscounts = [];
            $("#DivStudentQuestion input[type='radio']").each(function () {
                if ($(this).is(":checked")) {
                    CheckedDiscounts.push($(this).attr('id'));
                }
            });
            if (CheckedDiscounts.length === 0 && $("#DivStudentQuestion").find('input').length > 1) {
             
                swal("الرجاء الاجابة", "", "error");

            }
            else {
                $.ajax({
                    type: "POST",
                    url: "AddUpdateStudentDiscountsInternal",
                    data: JSON.stringify({ 'CheckedDiscounts': CheckedDiscounts, 'StudentID': $("#StudentID").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    async: false,
                    success: function (response) {
                        debugger;
                        if (response.Success) {
                            swal("تمت عملية الحفظ بنجاح", "", "success");
                            nextStepWizard.removeAttr('disabled').trigger('click');
                        }
                        else {
                            swal("حدث خطا ما", "", "error");
                        }
                    }
                    //traditional: true
                });
            }
        }
    });
    // end of step 6

    //step 7 

    SocialnextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid) {

            var dataObject = JSON.stringify({
                'OtherStudentDetail.LivesWith': $("#OtherStudentDetail_LivesWith").val(),
                'OtherStudentDetail.NumberofBrothers': $("#OtherStudentDetail_NumberofBrothers").val(),
                'OtherStudentDetail.NumberofSisters': $("#OtherStudentDetail_NumberofSisters").val(),
                'OtherStudentDetail.FamilyOrder': $("#OtherStudentDetail_FamilyOrder").val(),
                'OtherStudentDetail.FamilyTotalMonthlyIncome': $("#OtherStudentDetail_FamilyTotalMonthlyIncome").val(),
                'OtherStudentDetail.SpecialResidenceConditions': $("#OtherStudentDetail_SpecialResidenceConditions").val(),
                'StudentID': $("#StudentID").val()
            });

            $.ajax({
                type: "POST",
                url: "SocialInfoStepSeven",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.Success) {

                        swal("تمت عملية الحفظ بنجاح", "", "success");
                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                        swal("حدث خطا ما", "", "error");
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
    //end of step 7 

    //step 8 
    HealthInfonextBtn.click(function () {
        var curStep = $(this).closest(".setup-content"),
            curStepBtn = curStep.attr("id"),
            nextStepWizard = $('div.setup-panel div a[href="#' + curStepBtn + '"]').parent().next().children("a"),
            curInputs = curStep.find("input[type='text'],input[type='url'],select"),
            isValid = true;

        $(".form-group").removeClass("has-error");
        for (var i = 0; i < curInputs.length; i++) {
            if (!curInputs[i].validity.valid) {
                isValid = false;
                $(curInputs[i]).closest(".form-group").addClass("has-error");
            }
        }

        if (isValid) {
            var SpecialCare = 0;
            var HasChronicDisease = 0;
            if ($("#SpecialCare").prop("checked") === true) {
                SpecialCare = -1;
            }

            if ($("#HasChronicDisease").prop("checked") === true) {
                HasChronicDisease = -1;
            }
            var dataObject = JSON.stringify({
                'OtherStudentDetail.BloodType': $("#OtherStudentDetail_BloodType").val(),
                'OtherStudentDetail.PhysicalStatus': $("#OtherStudentDetail_PhysicalStatus").val(),
                'OtherStudentDetail.SpecialCare': SpecialCare,
                'OtherStudentDetail.HasChronicDisease': HasChronicDisease,
                'OtherStudentDetail.DiseaseType': $("#OtherStudentDetail_DiseaseType").val(),
                'StudentDiseas.Mumps': $("#StudentDiseas_Mumps_Value").is(":checked"),
                'StudentDiseas.Chickenpox': $("#StudentDiseas_Chickenpox_Value").is(":checked"),
                'StudentDiseas.rubella': $("#StudentDiseas_rubella_Value").is(":checked"),
                'StudentDiseas.Rheumaticfever': $("#StudentDiseas_Rheumaticfever_Value").is(":checked"),
                'StudentDiseas.Epilepsy': $("#StudentDiseas_Epilepsy_Value").is(":checked"),
                'StudentDiseas.Hepatitis': $("#StudentDiseas_Hepatitis_Value").is(":checked"),
                'StudentDiseas.Shakika': $("#StudentDiseas_Shakika_Value").is(":checked"),
                'StudentDiseas.Fainting': $("#StudentDiseas_Fainting_Value").is(":checked"),
                'StudentDiseas.Kidneydisease': $("#StudentDiseas_Kidneydisease_Value").is(":checked"),
                'StudentDiseas.Surgery': $("#StudentDiseas_Surgery_Value").is(":checked"),
                'StudentDiseas.Urinarysystemdiseases': $("#StudentDiseas_Urinarysystemdiseases_Value").is(":checked"),
                'StudentDiseas.Diabetes': $("#StudentDiseas_Diabetes_Value").is(":checked"),
                'StudentDiseas.Heartdiseases': $("#StudentDiseas_Heartdiseases_Value").is(":checked"),
                'StudentDiseas.Pissingoff': $("#StudentDiseas_Pissingoff_Value").is(":checked"),
                'StudentDiseas.Jointbonediseases': $("#StudentDiseas_Jointbonediseases_Value").is(":checked"),
                'StudentDiseas.sprayer': $("#StudentDiseas_sprayer_Value").is(":checked"),
                'StudentDiseas.Hearingimpairment': $("#StudentDiseas_Hearingimpairment_Value").is(":checked"),
                'StudentDiseas.Visualimpairment': $("#StudentDiseas_Visualimpairment_Value").is(":checked"),
                'StudentDiseas.Speechimpairment': $("#StudentDiseas_Speechimpairment_Value").is(":checked"),
                'StudentDiseas.Bladderdiseases': $("#StudentDiseas_Bladderdiseases_Value").is(":checked"),
                'StudentID': $("#StudentID").val()
            });

            $.ajax({
                type: "POST",
                url: "HealthInfoStepEight",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.Success) {
                        swal("تمت عملية الحفظ بنجاح", "", "success");

                    } else {
                        swal("حدث خطا ما", "", "error");
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
    // end of step8
});
