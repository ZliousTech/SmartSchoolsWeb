<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormView.aspx.cs" Inherits="SmartSchool.questionsweb.FormView" %>

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
        <asp:HiddenField ID="hdfgid" runat="server" />
        <asp:HiddenField ID="hdformstylid" runat="server" />
        <asp:HiddenField ID="hdformdurationmin" runat="server" />
        <asp:HiddenField ID="hdQuestionsGuidIDs" runat="server" />
        <asp:HiddenField ID="hdTotalNumberOfQuestion" runat="server" />
        <asp:HiddenField ID ="hdCurrQuesToShow" runat="server" />

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
<%--                <div class="head-body-col">
                    <span>
                        <!-- back  icon-->
                        <a href="DemonstrationForms.aspx" style="color: #fff"><i class="fa fa-arrow-left" aria-hidden="true"></i></a>
                    </span>
                    <h4><%=Resources.Resource.DemonstrateTheQuestionsForm %></h4>
                </div>--%>
                
                <div class="light-component inner" id="DivQuestionsDemonstration" runat="server">
                    <h4>
                        <span class="text-info" style="text-decoration: underline;">
                            <%--<%=Resources.Resource.DemonstrateTheQuestionsFormFor%>: --%>
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

                    <div class="col-md-12">
                        <div class="panel-group" id="DivPanelGroup" runat="server">
                        </div>
                    </div>

                    <div class="col-md-12">
                        <hr />
                        <div class="form-group">
                            <asp:Button ID="BtnDone" runat="server" OnClick="BtnDone_Click" class="btn btn-warning" />
                        </div>
                    </div>
                </div>

                <table style="width: 100%;">
                    <tr>
                        <td style="text-align: left;">

                        </td>
                    </tr>
                </table>

<%--                <div class="light-component inner">
                    <div class="col-md-12">
                        <div class="panel-group" id="Div1" runat="server">
                            <asp:Panel ID="Pnl" runat="server" style="display: block;">
                                <div class="panel panel-primary">
                                    <div class="panel-heading">Question # 0: Question text</div>
                                    <div class="panel-body">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButton ID="radtrue" runat="server" Checked="false" Text="True" GroupName="xxx" /></td>
                                                <td style="width: 30px;"></td>
                                                <td>
                                                    <asp:RadioButton ID="radfalse" runat="server" Checked="false" Text="False" GroupName="xxx" /></td>
                                            </tr>
                                        </table>

                                        <label style="font-size: 10px;"><%=Resources.Resource.Title %></label>
                                        <asp:TextBox ID="TxtFormTitle" runat="server" class="form-control summernote"></asp:TextBox>
                                    </div>
                                    <div class="panel-footer">Panel Footer</div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>--%>

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

                $("#BtnDone").on("click", function () {
                    var selLang = document.getElementById('hdclutrueName').val();
                    var msgTit = "الرجاء الانتظار";
                    var msgBdy = "جاري اعادة توجيهك";
                    if (selLang === "en") {
                        msgTit = "Please wait";
                        msgBdy = "You are being redirected";
                    }

                    swal({
                        title: msgTit,
                        text: msgBdy,
                        timer: 600000,
                        showConfirmButton: false
                    });
                });
            });
        </script>

        <script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>
        <script>
            function Msg(title, msg, type) {
                swal(title, msg, type);
            }
        </script>
    </form>
</body>
</html>


