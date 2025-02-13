$(document).ready(function () {
    var StudentInfonextBtn = $('.StudentInfonextBtn');
    var SchoolInfonextBtn = $('.SchoolInfonextBtn');
    var ParentsInfonextBtn = $('.ParentsInfonextBtn');
    var AddressInfonextBtn = $('.AddressInfonextBtn');
    var AcademicnextBtn = $('.AcademicnextBtn');
    var SocialnextBtn = $('.SocialnextBtn');
    var HealthInfonextBtn = $('.HealthInfonextBtn');
    var QuestionInfonextBtn = $('.QuestionInfonextBtn');
    StudentInfonextBtn.click(function () {

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
        if ($("#profileImage").attr('src').includes('camera2.png') && $("#fileuploader").get(0).files.length === 0) {
            $.toast({
                heading: 'الرجاء تحميل الصورة الشخصية',
                text: '',
                position: 'top-center',
                loaderBg: '#ff6849',
                icon: 'error',
                hideAfter: 4000,
                stack: 6
            });
            isValid = false;
        }
        if ($("#BCPImage").attr('src').includes('BrithCertificate.jpeg') && $("#BCPfileuploader").get(0).files.length === 0) {
            $.toast({
                heading: 'الرجاء تحميل صورة شهادة الميلاد',
                text: '',
                position: 'top-center',
                loaderBg: '#ff6849',
                icon: 'error',
                hideAfter: 4000,
                stack: 6
            });
            isValid = false;
        }
        if ($("#FBPImage").attr('src').includes('FamilyBook.jpeg') && $("#FBPfileuploader").get(0).files.length === 0) {
            $.toast({
                heading: 'الرجاء تحميل صورة دفتر العائلة',
                text: '',
                position: 'top-center',
                loaderBg: '#ff6849',
                icon: 'error',
                hideAfter: 4000,
                stack: 6
            });
            isValid = false;
        }
        if ($("#LYCImage").attr('src').includes('LastYearCertificate.jpeg') && $("#LYCfileuploader").get(0).files.length === 0) {
            $.toast({
                heading: 'الرجاء تحميل صورة شهادة اخر سنه دراسية',
                text: '',
                position: 'top-center',
                loaderBg: '#ff6849',
                icon: 'error',
                hideAfter: 4000,
                stack: 6
            });
            isValid = false;
        }

        if (isValid) {
            var formData = new FormData();
            var files = $("#fileuploader").get(0).files;
            var BCPfiles = $("#BCPfileuploader").get(0).files;
            var FBPfiles = $("#FBPfileuploader").get(0).files;
            var LYCfiles = $("#LYCfileuploader").get(0).files;

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
            formData.append('StudentIDNumber', $("#StudentIDNumber").val());
            formData.append('GurdianID', $("#GurdianID").val());
            //formData.append('BirthCertificatePhoto', BCPfiles[0]);
            //formData.append('FamilyBookPhoto', FBPfiles[0]);
            //formData.append('LastYearCertificate', LYCfiles[0]);

            $.ajax({
                type: "POST",
                url: "StudentInfoStepOne",
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                success: function (response) {
                    if (response != null) {
                        $.toast({
                            text: 'تمت عملية الحفظ بنجاح',
                            position: 'top-center',
                            loaderBg: '#ff6849',
                            icon: 'success',
                            hideAfter: 1500,
                            stack: 6
                        });

                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                    }
                }

            });
        }
    });


    SchoolInfonextBtn.click(function () {
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
                'StudentIDNumber': $("#StudentIDNumber").val()


            });

            $.ajax({
                type: "POST",
                url: "SchoolInfoStepTwo",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        $.toast({
                            text: 'تمت عملية الحفظ بنجاح',
                            position: 'top-center',
                            loaderBg: '#ff6849',
                            icon: 'success',
                            hideAfter: 1500,
                            stack: 6
                        });
                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                        alert("Something went wrong");
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


    ParentsInfonextBtn.click(function () {
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
                'StudentIDNumber': $("#StudentIDNumber").val()

            });

            $.ajax({
                type: "POST",
                url: "ParentsInfoStepThree",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        $.toast({
                            text: 'تمت عملية الحفظ بنجاح',
                            position: 'top-center',
                            loaderBg: '#ff6849',
                            icon: 'success',
                            hideAfter: 1500,
                            stack: 6
                        });
                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                        alert("Something went wrong");
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
                'StudentAddress.Latitude': $("#StudentAddress_Latitude").val(),
                'StudentAddress.Building': $("#StudentAddress_Building").val(),
                'StudentAddress.TransportDirectionID': $("input[name='UseBus']:checked").val(),
                'StudentIDNumber': $("#StudentIDNumber").val()
            });

            $.ajax({
                type: "POST",
                url: "AddressInfoStepFour",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        $.ajax({
                            type: "GET",
                            url: "GetDiscountsStep",
                            cache: false,
                            data: { 'SchoolID': response.SchoolID, 'StudentID': response.StudentID }


                        }).done(function (result) {
                            debugger;
                            $('#DivStudentQuestion').html(result);
                            $.toast({
                                text: 'تمت عملية الحفظ بنجاح',
                                position: 'top-center',
                                loaderBg: '#ff6849',
                                icon: 'success',
                                hideAfter: 1500,
                                stack: 6
                            });
                            nextStepWizard.removeAttr('disabled').trigger('click');
                        });


                    } else {
                        alert("Something went wrong");
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
                $.toast({
                    heading: 'الرجاء الاجابة',
                    text: '',
                    position: 'top-center',
                    loaderBg: '#ff6849',
                    icon: 'error',
                    hideAfter: 3000,
                    stack: 6
                });
            }
            else {
                $.ajax({
                    type: "POST",
                    url: "AddUpdateStudentDiscounts",
                    data: JSON.stringify({ 'CheckedDiscounts': CheckedDiscounts, 'StudentID': $("#StudentID").val() }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    cache: false,
                    async: false,
                    success: function (response) {
                        debugger;
                        if (response != null) {
                            $.toast({
                                text: 'تمت عملية الحفظ بنجاح',
                                position: 'top-center',
                                loaderBg: '#ff6849',
                                icon: 'success',
                                hideAfter: 1500,
                                stack: 6
                            });
                            nextStepWizard.removeAttr('disabled').trigger('click');


                        }
                        else {
                            alert("error");
                        }
                    }
                    //traditional: true
                });
            }
        }
    });

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
                'StudentIDNumber': $("#StudentIDNumber").val()
            });

            $.ajax({
                type: "POST",
                url: "SocialInfoStepSex",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        $.toast({
                            text: 'تمت عملية الحفظ بنجاح',
                            position: 'top-center',
                            loaderBg: '#ff6849',
                            icon: 'success',
                            hideAfter: 1500,
                            stack: 6
                        });
                        nextStepWizard.removeAttr('disabled').trigger('click');

                    } else {
                        alert("Something went wrong");
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
            if ($("#SpecialCare").prop("checked") == true) {
                SpecialCare = -1;
            }

            if ($("#HasChronicDisease").prop("checked") == true) {
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
                'StudentIDNumber': $("#StudentIDNumber").val()
            });

            $.ajax({
                type: "POST",
                url: "HealthInfoStepSeven",
                data: dataObject,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response != null) {
                        Swal.fire(
                            'تم تقديم طلب التسجيل بنجاح',
                            ' سوف نقوم بإرسال رسالة نصية عند الموافقة على الطلب لإستكمال إجراءات التسجيل من بوابة ولي الأمر',
                            'success'
                        ).then(function () {
                            window.location.href = 'ParentRegistrationRequests';
                        });

                    } else {
                        alert("Something went wrong");
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


});
