<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="quesbank.aspx.cs" Inherits="SmartSchool.questionsweb.quesbank" %>

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
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdclutrueName" runat="server" />

        <div class="shader">
            <div class="overlay"></div>
            <div class="spanner show">
                <div class="loader show"></div>
                <p>Loading ... </p>
            </div>
        </div>
        <button onclick="topFunction()" id="scroll-btn" title="Go to top">
            <i class="fa fa-chevron-up" aria-hidden="true"></i>
        </button>

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
                
                <div class="head-body-col">
                    <span>
                        <!-- back  icon-->
                        <%--<a onclick="goBack()" style="color: #fff"><i class="fa fa-arrow-left" aria-hidden="true"></i></a>--%>
                        <a href="/Home" style="color: #fff"><i class="fa fa-arrow-left" aria-hidden="true"></i></a>
                    </span>
                    <h2><%=Resources.Resource.QuestionsBank %></h2>
                </div>

                <div class="Dashboard-widget">
                    <a href="AddForms.aspx" class="light-component widget">
                        <div class="overlay-head">
                            <h3><%=Resources.Resource.AddNewQuestionsForm %></h3>
                        </div>

                        <img src="../AppContent/WidgetImages/AddQuesbank.png" alt="" />
                    </a>

                    <a href="QuestionsForms.aspx" class="light-component widget">
                        <div class="overlay-head">
                            <h3><%=Resources.Resource.QuestionsForms %></h3>
                        </div>

                        <img src="../AppContent/WidgetImages/Quesbank.png" alt="" />
                    </a>

                    <a href="FormsView.aspx" class="light-component widget">
                        <div class="overlay-head">
                            <%--<h3><%=Resources.Resource.DemonstrationTheQuestionsForms %></h3>--%>
                            <h3><%=Resources.Resource.FormsView %></h3>
                        </div>

                        <img src="../AppContent/WidgetImages/DemonstrationForms.png" alt="" />
                    </a>

                    <a href="FormsSettings.aspx" class="light-component widget">
                        <div class="overlay-head">
                            <%--<h3><%=Resources.Resource.DemonstrationTheQuestionsForms %></h3>--%>
                            <h3><%=Resources.Resource.FormsSettings %></h3>
                        </div>

                        <img src="../AppContent/WidgetImages/FormsSettings.png" alt="" />
                    </a>

                    <a href="CorrectingAnswers.aspx" class="light-component widget">
                        <div class="overlay-head">
                            <%--<h3><%=Resources.Resource.DemonstrationTheQuestionsForms %></h3>--%>
                            <h3><%=Resources.Resource.CorrectingTheAnswersForm %></h3>
                        </div>

                        <img src="../AppContent/WidgetImages/CorrectingAnswers.png" alt="" />
                    </a>

                    <a href="ViewStudentsGrades.aspx" class="light-component widget">
                        <div class="overlay-head">
                            <%--<h3><%=Resources.Resource.DemonstrationTheQuestionsForms %></h3>--%>
                            <h3><%=Resources.Resource.StudentsGrades %></h3>
                        </div>

                        <img src="../AppContent/WidgetImages/StudentsGrades.png" alt="" />
                    </a>
                </div>

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


            //window.onscroll = function () { scrollFunction() };
            //function scrollFunction() {
            //    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
            //        $('#scroll-btn').fadeIn();
            //    }
            //    else {
            //        $('#scroll-btn').fadeOut();
            //    }
            //}

            //function topFunction() {
            //    document.body.scrollTop = 0;
            //    document.documentElement.scrollTop = 0;
            //}

            function goBack() {
                window.history.back();
            }
        </script>

        <%--@RenderSection("Scripts", required: false)--%>
    </form>
</body>
</html>

