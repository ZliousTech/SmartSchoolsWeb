<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewStudentForm.aspx.cs" Inherits="SmartSchool.questionsweb.ViewStudentForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <meta name="theme-color" content="#3699ff"/>
    <meta name="description" content="smartschool,smart,school,student,parent,learn" />
    <title>ZLIOUS | SMART-SCHOOL</title>
    <link href="~/fonts/material-design-icons/material-icon.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css"/>

    <link rel="stylesheet" href="~/assets/plugins/material/material.min.css"/>
    <%--<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">--%>

    <link rel="stylesheet" href="~/assets/css/material_style.css"/>
    <link rel="stylesheet" href="~/assets/plugins/material-datetimepicker/bootstrap-material-datetimepicker.css" />
    <link rel="stylesheet" href="~/assets/plugins/jquery-toast/dist/jquery.toast.min.css"/>

    <link href="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/css/select2.min.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="http://www.w3cschool.cc/try/jeasyui/themes/default/easyui.css"/>
    <link rel="stylesheet" type="text/css" href="http://www.w3cschool.cc/try/jeasyui/themes/icon.css"/>
    <link href="~/AppContent/style.css" rel="stylesheet"/>

    <%if (hdclutrueName.Value.Contains("ar")) { %>
        <!--compiled and minified CSS-->
        <link rel="stylesheet" href="https://cdn.rtlcss.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-cSfiDrYfMj9eYCidq//oGXEkMc0vuTxHXizrMOFAaPsLt1zoCUVnSsURN+nef1lj" crossorigin="anonymous" />
        <!-- compiled and minified theme CSS -->
        <link rel="stylesheet"
            href="https://cdn.rtlcss.com/bootstrap/3.3.7/css/bootstrap-theme.min.css"
            integrity="sha384-YNPmfeOM29goUYCxqyaDVPToebWWQrHk0e3QYEs7Ovg6r5hSRKr73uQ69DkzT1LH"
            crossorigin="anonymous" />

        <!-- compiled and minified JavaScript -->
        <script src="https://cdn.rtlcss.com/bootstrap/3.3.7/js/bootstrap.min.js"
            integrity="sha384-B4D+9otHJ5PJZQbqWyDHJc6z6st5fX3r680CYa0Em9AUG6jqu5t473Y+1CTZQWZv"
            crossorigin="anonymous"></script>

        <link href="~/AppContent/rtl.css" rel="stylesheet" />
    <%} %>
    
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.21/css/jquery.dataTables.css"/>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/buttons/1.6.2/css/buttons.dataTables.min.css"/>
    <link rel="shortcut icon" href="~/AppContent/Images/favicon.png" />
    <link href="~/Content/multi-select.css" rel="stylesheet" />

    <link rel="stylesheet" href="../assets/plugins/summernote/summernote.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.css" />


    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.7.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdclutrueName" runat="server" />
        <asp:HiddenField ID="hdStdid" runat ="server" />
        <asp:HiddenField ID="hdfgid" runat="server" />
        <asp:HiddenField ID="hdformstylid" runat="server" />
        <asp:HiddenField ID="hdformdurationmin" runat="server" />
        <asp:HiddenField ID="hdQuestionsGuidIDs" runat="server" />
        <asp:HiddenField ID="hdQuestionsTime" runat="server" />
        <asp:HiddenField ID="hdTotalNumberOfQuestion" runat="server" />
        <asp:HiddenField ID ="hdCurrQuesToShow" runat="server" />
        <asp:HiddenField ID="hdEndsAtExactly" runat="server" />
        <asp:HiddenField ID="hdStdEndsAtExactly" runat="server" />
        <asp:HiddenField ID="hdBtnFinishClicked" runat="server" />
        <asp:HiddenField ID="hdAllControls" runat="server" />

        <div class="shader">
            <div class="overlay"></div>
            <div class="spanner show">
                <div class="loader show"></div>
                <p>Loading ... </p>
            </div>
        </div>
<%--        <button onclick="topFunction()" id="scroll-btn" title="Go to top">
            <i class="fa fa-chevron-up" aria-hidden="true"></i>
        </button>--%>

        <header class="header-container">
            <section class="inner-row">
                <div class="banner light-component">
                    <div class="logo-holder">
                        <a href="/" class="header-logo-anchor">
                            <img src="/AppContent/Images/smartschoollogo.png" alt="smart school logo"/>
                        </a>
                    </div>

                    <nav class="toolbar-nav">
                        <ul>
                            <li>
                                <div href="javascript:void(0)" class="User-info">
                                    <div class="Avatar">
                                        <img id="userProfileImg" src="../../AppContent/Images/avatar.jpeg" alt="avatar"/>
                                    </div>
                                    <span><asp:Label ID="LblUName" runat="server"></asp:Label></span>
                                    <div class="chveron-holder">
                                        <img src="/AppContent/Images/chveron.png" alt="chveron"/>
                                    </div>
                                    <div class="Dropdwon-item-setting">
                                        <ul>
                                            <li>
                                                <span>
                                                    <i class="fa fa-sign-out" aria-hidden="true"></i>
                                                </span>
                                                <a href="/Account/Logout"><%=Resources.Resource.Logout %></a>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </li>

                            <li>
                                <a href="/Common/SwitchLanguage" class="anchor-lang">
                                    <span>
                                        <i class="fa fa-language" aria-hidden="true"></i>
                                    </span>
                                </a>
                            </li>
                        </ul>
                    </nav>
                </div>
            </section>
        </header>

        <main class="body-container">
            <div class="inner-row">
               
                <div class="light-component inner" id="DivQuestionsDemonstration" runat="server">
                    <h4>
                        <span class="text-info" style="text-decoration: underline;">
                            <%=Resources.Resource.QuestionsForm%>: 
                            <asp:Label ID="LblQuestionsFormTitle" runat="server" ForeColor="OrangeRed"></asp:Label>
                        </span>
                    </h4>
                    <h5>
                        <span id="SpanQuestionsFormDurationMinutes" runat="server" class="text-info" style="text-decoration: underline;">
                            <%=Resources.Resource.QuestionsFormDuration%>: 
                            <asp:Label ID="LblQuestionsFormDurationMinutes" runat="server" ForeColor="OrangeRed"></asp:Label>
                            <%=Resources.Resource.Minutes %>, 
                        </span>
                        <span class="text-info" style="text-decoration: underline;">
                            <%=Resources.Resource.NumberOfQuestions %>:
                            <asp:Label ID="LblNumberOfQuestions" runat="server" ForeColor="OrangeRed"></asp:Label> <%=Resources.Resource.Questions %>
                        </span>
                    </h5>
                    <h5>
                        <span id="SpanQuestionsFormStartsEnds" runat="server" class="text-info" style="text-decoration: underline;">
                            <%=Resources.Resource.StartsAtExactly%>: 
                            <asp:Label ID="LblStartsAtExactly" runat="server" ForeColor="OrangeRed"></asp:Label>, 
                            <%=Resources.Resource.EndsAtExactly %>:
                            <asp:Label ID="LblEndsAtExactly" runat="server" ForeColor="OrangeRed"></asp:Label>
                        </span>
                    </h5>
                    <h5>
                        <span id="SpanStudentStartEndFormInfo" runat="server" class="text-info" style="text-decoration: underline;">
                            <%=Resources.Resource.StudentStartsAtExactly%>: 
                            <asp:Label ID="LblStdStartsAtExactly" runat="server" ForeColor="OrangeRed"></asp:Label>, 
                            <%=Resources.Resource.StudentEndsAtExactly %>:
                            <asp:Label ID="LblStdEndsAtExactly" runat="server" ForeColor="OrangeRed"></asp:Label>
                        </span>
                    </h5>

                    <div class="col-md-12">
                        <div class="panel-group" id="DivPanelGroup" runat="server">
                        </div>
                    </div>

                    <div class="light-component inner" style="width: 98%;">
                        <div class="col-md-12">
                            <br />
                            <div class="col-md-4">
                                <div class="form-group">
                                    <span>
                                        <label><%=Resources.Resource.RemainingTime %></label>: 
                                        <asp:Label ID="LblRemainingTime" runat="server" Style="font-size: small; color: orangered;"></asp:Label> 
                                        <label><%=Resources.Resource.Minutes %></label>
                                    </span>
                                </div>
                            </div>

                            <div class="col-md-8">
                                <div class="form-group">
                                    <span>
                                        <label><%=Resources.Resource.Note %></label>: 
                                        <label style="font-size: small; color: orangered;"><%=Resources.Resource.TheQuestionsFormWillBeAutomaticallyTerminated %></label>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12">
                        <hr />
                        <div class="form-group">
                            <%--<asp:Button ID="BtnFinish" runat="server" OnClick="BtnFinish_Click" class="btn btn-warning" />--%>
                            <a href="javascript:void(0)" id="BtnFinish" onclick="btnFinish()" class="btn btn-warning"><%=Resources.Resource.Finish %></a>
                        </div>
                    </div>
                </div>

                <table style="width: 100%;">
                    <tr>
                        <td style="text-align: left;">

                        </td>
                    </tr>
                </table>
            </div>
        </main>

        <footer class="footer-Contianer">
            <section class="inner-row footer">
                <div class="bg-pattern">
                    <div class="shader"></div>
                </div>
                <p>
                    Super-powered by Zlious Tech © 2016 - <%=DateTime.Now.Year.ToString() %>
                </p>
            </section>

        </footer>
        <!-- end footer -->
        <!-- start js include path -->
        
        <script src="~/Scripts/jquery-3.1.1.min.js"></script>
        <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
        <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

        <script type="text/javascript" src="http://www.w3cschool.cc/try/jeasyui/jquery.easyui.min.js"></script>

        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
        <script type="text/javascript" charset="utf8" src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.js"></script>

        <script src="https://cdn.datatables.net/1.10.21/js/jquery.dataTables.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.6.2/js/dataTables.buttons.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.flash.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.html5.min.js"></script>
        <script src="https://cdn.datatables.net/buttons/1.6.2/js/buttons.print.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-beta.1/dist/js/select2.min.js"></script>
        <script src="https://use.fontawesome.com/207b7392fc.js"></script>

        <script src="../assets/plugins/summernote/summernote.js"></script>
        <script src="../assets/js/pages/summernote/summernote-data.js"></script>

        <script src="~/Scripts/jquery.validate.min.js"></script>
        <script src="~/Scripts/jquery.validate.unobtrusive.min.js"></script>
        <script src="../assets/plugins/jquery-toast/dist/jquery.toast.min.js"></script>
        <script src="../assets/plugins/jquery-toast/dist/toast.js"></script>
        <script src="~/assets/plugins/material/material.min.js"></script>
        <script src="~/assets/plugins/material-datetimepicker/moment-with-locales.min.js"></script>
        <script src="~/assets/plugins/material-datetimepicker/bootstrap-material-datetimepicker.js"></script>
        <script src="~/assets/plugins/material-datetimepicker/datetimepicker.js"></script>
        <script src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.quicksearch/2.4.0/jquery.quicksearch.min.js"></script>
        <script src="~/Scripts/jquery.multi-select.js"></script>
        <!-- summernote -->
        <!-- end js include path -->
        <%if (hdclutrueName.Value.Contains("en"))
            {%>
            <script src="../../Content/dtablesen.js"></script>
        <%} %>
        <%else
        {%>
            <script src="../../Content/dtables.js"></script>
        <%} %>

        <script>
            $('.select2').select2();
            var $loading = $('.shader').hide();

            $(document).ajaxStart(function () {
                $loading.show();
            }).ajaxStop(function () {
                $loading.hide();
            });

            function goBack() {
                window.history.back();
            }
        </script>

        <script>
            $(document).ready(function () {
                $('.summernote').summernote();
            });
        </script>

        <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
        <script>
            function Msg(title, msg, type) {
                swal(title, msg, type);
            }

            formstylid = document.getElementById('hdformstylid').value;
            //var dtm = document.getElementById("hdEndsAtExactly").value;
            var dtm = document.getElementById("hdStdEndsAtExactly").value;
            var countDownDate = new Date(dtm).getTime();
            var x = setInterval(function () {
                var btnFinishClicked = document.getElementById("hdBtnFinishClicked").value;
                if (btnFinishClicked === "YES") {
                    clearInterval(x);
                    return;
                }

                var now = new Date().getTime();
                var distance = countDownDate - now;
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);
                document.getElementById("LblRemainingTime").innerHTML = minutes + ":" + seconds;
                if (distance <= 0) {
                    clearInterval(x);
                    document.getElementById("LblRemainingTime").innerHTML = "0:0";
                    //Save and redirect the student.
                    var saveRes = btnFinish();
                    if(saveRes === "success") location.replace("StudentQuestionsForms.aspx");
                }
            }, 1000);

            //Hide all panels but first one.
            var QuestionsID = document.getElementById('hdQuestionsGuidIDs').value;
            var QuestionsIDArr = QuestionsID.split('|');

            if (formstylid !== "1")
            {
                for (i = 1; i < QuestionsIDArr.length - 1; i++)
                {
                    var panelID = "Pnl_" + QuestionsIDArr[parseInt(i)] + "_" + parseInt(i + 1);
                    document.getElementById(panelID).style.display = 'none';
                }
            }

            if (formstylid === "3") StartCountDownQuestionTime(QuestionsIDArr[0], 1);

            var y;
            function BtnNextClicked(btnID) {
                //console.log("btnID: " + btnID);
                clearInterval(y);
                var NumberOfQuestions = document.getElementById('LblNumberOfQuestions').innerText;
                var btnIDArr = btnID.split('_');
                var currQuestionNo = btnIDArr[2];
                var nextQuestionNo = parseInt(currQuestionNo) + 1;
                if (currQuestionNo === NumberOfQuestions) {
                    nextQuestionNo = currQuestionNo;
                    //You have reached the end of the questions.
                    //لقد وصلت إلى نهاية الأسئلة.
                    return;
                }
                var PanleID = "Pnl_" + btnIDArr[1] + "_" + btnIDArr[2];
                document.getElementById(PanleID).style.display = 'none';
                //console.log("PanleID: " + PanleID);

                var QuestionsID = document.getElementById('hdQuestionsGuidIDs').value;
                var QuestionsIDArr = QuestionsID.split('|');
                var nextPanleID = "Pnl_" + QuestionsIDArr[parseInt(nextQuestionNo) - 1] + "_" + nextQuestionNo;
                document.getElementById(nextPanleID).style.display = 'block';
                /*console.log("nextPanleID: " + nextPanleID);*/

                if (formstylid === '3') StartCountDownQuestionTime(QuestionsIDArr[parseInt(nextQuestionNo) - 1], nextQuestionNo);
            }

            function BtnPrevClicked(btnID) {
                var btnIDArr = btnID.split('_');
                var currQuestionNo = btnIDArr[2];
                var prevQuestionNo = parseInt(currQuestionNo) - 1;
                if (prevQuestionNo < 1) {
                    prevQuestionNo = currQuestionNo;
                    //You have reached the beginning of the questions.
                    //لقد وصلت إلى بداية الأسئلة.
                    return;
                }
                var PanleID = "Pnl_" + btnIDArr[1] + "_" + btnIDArr[2];
                document.getElementById(PanleID).style.display = 'none';

                var QuestionsID = document.getElementById('hdQuestionsGuidIDs').value;
                var QuestionsIDArr = QuestionsID.split('|');
                var prevPanleID = "Pnl_" + QuestionsIDArr[parseInt(prevQuestionNo) - 1] + "_" + prevQuestionNo;
                document.getElementById(prevPanleID).style.display = 'block';
            }

            //For form style 3
            function StartCountDownQuestionTime(QuestionID, QuestionNo) {
                var QuestionsTime = document.getElementById('hdQuestionsTime').value;
                var QuestionsTimeArr = QuestionsTime.split('|');
                var LblcurrQuestionRemainingTime = "LblTimeRemaining_" + QuestionID + "_" + QuestionNo;
                var currQuestionTimeInMinutes = QuestionsTimeArr[QuestionNo - 1];
                currQuestionTimeInMinutes = parseInt(currQuestionTimeInMinutes) - 1;
                var Seconds = 60;

                y = setInterval(function () {
                    document.getElementById(LblcurrQuestionRemainingTime).innerHTML = currQuestionTimeInMinutes + ":" + Seconds;
                    if (Seconds === 0) {
                        currQuestionTimeInMinutes = parseInt(currQuestionTimeInMinutes) - 1;
                        Seconds = 60;

                        if (currQuestionTimeInMinutes < 0) {
                            document.getElementById(LblcurrQuestionRemainingTime).innerHTML = "0:0";
                            clearInterval(y);
                            var currBtnID = "BtnNext_" + QuestionID + "_" + QuestionNo;
                            BtnNextClicked(currBtnID);
                            return;
                        }
                    }
                    Seconds = parseInt(Seconds) - 1;
                }, 1000);
            }

            function btnFinish()
            {
                //$("#BtnFinish").on("click", function () {
                //    var selLang = document.getElementById('hdclutrueName').value;
                //    var msgTit = "الرجاء الانتظار";
                //    var msgBdy = "جاري حفظ البيانات";
                //    if (selLang === "en") {
                //        msgTit = "Please wait";
                //        msgBdy = "The data is being saved";
                //    }

                //    swal({
                //        title: msgTit,
                //        text: msgBdy,
                //        timer: 600000,
                //        showConfirmButton: false
                //    });
                //});

                var selLang = document.getElementById('hdclutrueName').value;
                var msgTit = "الرجاء الانتظار";
                var msgBdy = "جاري حفظ البيانات";
                if (selLang === "en") {
                    msgTit = "Please wait";
                    msgBdy = "The data is being saved";
                }

                swal({
                    title: msgTit,
                    text: msgBdy,
                    timer: 600000,
                    showConfirmButton: false
                });

                var AnswersControls = $("#hdAllControls").val();
                var AnswersControlsArr = AnswersControls.split('|');
                const QuestionIDInfo = [];
                const QuestionValueInfo = [];

                for (var ansIndx = 0; ansIndx < AnswersControlsArr.length - 1; ansIndx++) {
                    var SplitControlInfo = AnswersControlsArr[ansIndx];
                    var ControlInfo = SplitControlInfo.split('_');
                    switch (ControlInfo[0]) {
                        case "TxtEssay":
                            //var TxtEssayValue = $($('#Txtxxx').summernote('code')).text(); //Get text
                            //var TxtEssayValue = $('#Txtxxx').summernote('code'); //Get Code
                            //var TxtEssayValue = $($("#" + AnswersControlsArr[ansIndx] + "").summernote("code")).text();
                            var TxtEssayValue = $("#" + AnswersControlsArr[ansIndx] + "").summernote("code");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = TxtEssayValue;
                            //alert(QuestionIDInfo[ansIndx]);
                            //alert(QuestionValueInfo[ansIndx])
                            break;

                        case "TxtShortAnswer":
                            //var TxtShortAnswerValue = $($("#" + AnswersControlsArr[ansIndx] + "").summernote("code")).text();
                            var TxtShortAnswerValue = $("#" + AnswersControlsArr[ansIndx] + "").summernote("code");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = TxtShortAnswerValue;
                            break;

                        case "RadTrue":
                            var RadTrue = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadTrue;
                            break;
                        case "RadFalse":
                            var RadFalse = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadFalse;
                            break;

                        case "RadYes":
                            var RadYes = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadYes;
                            break;

                        case "RadNo":
                            var RadNo = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadNo;
                            break;

                        case "RadChoice01":
                            var RadChoice01 = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadChoice01;
                            break;

                        case "RadChoice02":
                            var RadChoice02 = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadChoice02;
                            break;

                        case "RadChoice03":
                            var RadChoice03 = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadChoice03;
                            break;

                        case "RadChoice04":
                            var RadChoice04 = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadChoice04;
                            break;

                        case "RadChoice05":
                            var RadChoice05 = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadChoice05;
                            break;

                        case "RadChoice06":
                            var RadChoice06 = $("#" + AnswersControlsArr[ansIndx] + "").is(":checked");
                            QuestionIDInfo[ansIndx] = AnswersControlsArr[ansIndx];
                            QuestionValueInfo[ansIndx] = RadChoice06;
                            break;
                    }
                }

                var FormGuidID = document.getElementById("hdfgid").value;
                var StudentID = document.getElementById("hdStdid").value;
                SaveStudentQuestionsFormData(StudentID, FormGuidID, QuestionIDInfo, QuestionValueInfo);

                return "success";
            }

            function SaveStudentQuestionsFormData(StudentID, FormGuidID, QuestionIDInfo, QuestionValueInfo) {
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "../ssappws.asmx/saveStudentQuestionsFormData",
                    data: JSON.stringify({ StudentID: StudentID, FormGuidID: FormGuidID, QuestionIDInfo: QuestionIDInfo, QuestionValueInfo: QuestionValueInfo }),
                    contentType: "application/json",
                    datatype: "json",
                    success: function (responseFromServer) {
                        ResponsRes = responseFromServer.d;
                        if (ResponsRes === "success") {
                            //alert('Done');
                        }

                        location.replace("StudentQuestionsForms.aspx");
                    },
                    error: function () {
                        alert('Something went worng!');
                    }
                });
            }
        </script>
    </form>
</body>
</html>



