<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormsSettings.aspx.cs" Inherits="SmartSchool.questionsweb.FormsSettings" %>

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

    <style>
        * {
            box-sizing: border-box;
        }

        #dataTableInput {
            background-image: url('/css/searchicon.png');
            background-position: 10px 10px;
            background-repeat: no-repeat;
            width: 100%;
            font-size: 16px;
            padding: 12px 20px 12px 40px;
            border: 1px solid #ddd;
            margin-bottom: 12px;
        }

        #dataTable1 {
            border-collapse: collapse;
            width: 100%;
            border: 1px solid #ddd;
            font-size: 18px;
        }

        #dataTable1 th, #dataTable1 td {
        text-align: left;
        padding: 12px;
        }

        #dataTable1 tr {
        border-bottom: 1px solid #ddd;
        }

        #dataTable1 tr.header, #dataTable1 tr:hover {
            background-color: #f1f1f1;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdclutrueName" runat="server" />
        <asp:HiddenField ID="Hdqbgid" runat="server" />
        <asp:HiddenField ID="hdSchoolID" runat="server" />
        <asp:HiddenField ID="hdStaffID" runat="server" />

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

                <div class="head-body-col">
                    <span>
                        <!-- back  icon-->
                        <a href="quesbank.aspx" style="color: #fff"><i class="fa fa-arrow-left" aria-hidden="true"></i></a>
                        <%--<a onclick="goBack()" style="color: #fff"><i class="fa fa-arrow-left" aria-hidden="true"></i></a>--%>
                    </span>
                    <h2><%=Resources.Resource.FormsSettings %></h2>
                </div>

                <div class="light-component inner">
<%--                    <h5>
                        <span class="text-info" style="text-decoration: underline;">
                            <%=Resources.Resource.AddedQuestions %>
                        </span>
                    </h5>--%>
                    <div class="col-md-12">
                        <div class="table-responsive">
                            <table class="table" id="exportTable" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th><%=Resources.Resource.CreatedBy %></th>
                                        <th><%=Resources.Resource.QuestionsFormTitle %></th>
                                        <th><%=Resources.Resource.Description %></th>
                                        <th><%=Resources.Resource.QuestionsFormType %></th>
                                        <th><%=Resources.Resource.Curriculum %></th>
                                        <th><%=Resources.Resource.Class %></th>
                                        <th><%=Resources.Resource.Subject %></th>
                                        <th><%=Resources.Resource.Semester %></th>
                                        <th><%=Resources.Resource.Actions %></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="ReptFormsQuestionsList" runat="server">
                                        <ItemTemplate>
                                            <tr scope="row">
                                                <td><%# Eval("StaffName") %></td>
                                                <td><%# Eval("QuestionsFormTitle") %></td>
                                                <td><%# Eval("QuestionsFormDescription") %></td>
                                                <td><%# Eval("QuestionsFormTypeName") %></td>
                                                <td><%# Eval("CurriculumName") %></td>
                                                <td><%# Eval("SchoolClassName") %></td>
                                                <td><%# Eval("SubjectName") %></td>
                                                <td><%# Eval("SemesterName") %></td>
                                                <td class="center">
                                                    
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <a class="btn btn-success btn-xs" href="AddSettings.aspx?FGID=<%# Eval("FormGuidID") %>"><%=Resources.Resource.Add%> / <%=Resources.Resource.Edit%></a>
                                                            </td>
                                                            <td>
                                                                <a href="javascript:void(0)" onclick="getFormSettingsInfo('<%# Eval("FormGuidID") %>')" <%# Eval("IsThereQuestionFormSettings").ToString() == "0" ? "class='btn btn-warning btn-xs'" : "class='btn btn-info btn-xs'" %> data-toggle="modal" data-target="#FormSettingsInfoModal"><%=Resources.Resource.Details%></a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </>
                    </div>
                </div>
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

        <!-- Modals -->
        <div class="modal fade" id="FormSettingsInfoModal" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="myModalLabel"><%=Resources.Resource.FormSettingsDetails %></h4>
                    </div>
                    <div class="modal-body">

                        <main class="body-container">
                            <div class="inner-row">
                                <div class="light-component inner">
                                    <h5>
                                        <span class="text-info" style="text-decoration: underline;">
                                            <%=Resources.Resource.QuestionsForm%>: <asp:Label ID="LblQuestionsFormTitle" runat="server" ForeColor="OrangeRed"></asp:Label>
                                        </span>
                                    </h5>

                                    <div class="col-md-12">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><%=Resources.Resource.StartingDate %></label>: 
                                                <asp:Label ID="LblDate" runat="server" ForeColor="OrangeRed"></asp:Label>
                                                <%--W3Schools Library--%>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label class="control-label"><%=Resources.Resource.StartingTime %></label>: 
                                                <asp:Label ID="LblTime" runat="server" ForeColor="OrangeRed"></asp:Label>
                                                <%--W3Schools Library--%>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><%=Resources.Resource.EndingTime %></label>: 
                                                <asp:Label ID="LblEndTime" runat="server" ForeColor="OrangeRed"></asp:Label>
                                                <%--W3Schools Library--%>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <label class="control-label"><%=Resources.Resource.IsActive %></label>: 
                                                <asp:Label ID="LblIsActive" runat="server" ForeColor="OrangeRed"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="light-component inner">
                                    <h5>
                                        <span class="text-success" style="text-decoration: underline;">
                                           <%=Resources.Resource.IncludedStudents %>
                                        </span>
                                    </h5>

                                    <div class="col-md-12">
                                        <div class="table-scrollable">
                                            <input type="text" id="dataTableInput" onkeyup="dataTableFunction()" placeholder="<%=Resources.Resource.Search %>"/>
                                            <table class="table table-hover" id="dataTable1">
                                                <thead>
                                                    <tr style="background-color: floralwhite;">
                                                        <td style="text-align: center;"><span style="font-size: small; font-weight: bold;"><%=Resources.Resource.StudentNo %></span></td>
                                                        <td style="text-align: center;"><span style="font-size: small; font-weight: bold;"><%=Resources.Resource.StudentName %></span></td>
                                                    </tr>
                                                </thead>
                                                <tbody id="studentsData">
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </main>

                        <div class="modal-footer">
                            <div class="col-md-12">
                                <button type="button" class="btn btn-primary" data-dismiss="modal"><%=Resources.Resource.Close %></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script>
            function getFormSettingsInfo(FGID) {
                var Lang = $("#hdclutrueName").val();
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "../ssappws.asmx/GetFormSettingsInfo",
                    data: "{'FGID':'" + FGID + "'}",
                    contentType: "application/json",
                    datatype: "json",
                    success: function (responseFromServer) {
                        ResponsRes = responseFromServer.d;
                        var Res = ResponsRes.split('|');
                        $("#LblQuestionsFormTitle").text(Res[0]);
                        $("#LblDate").text(Res[1]);
                        $("#LblTime").text(Res[2]);
                        $("#LblEndTime").text(Res[3]);
                        $("#LblIsActive").text(Res[4]);

                        var Table = document.getElementById("studentsData");
                        Table.innerHTML = "";
                        var Tit = "تنبيه!";
                        var Msge = "لم يتم ضبط أي إعدادات لهذا النموذج من الأسئلة";
                        if (Lang === "en") {
                            Tit = "Warning!";
                            Msge = "There is no seetings set to this questions form";
                        }
                        if (Res[0] === " ") {
                            Msg(Tit, Msge, "warning");
                            return;
                        }
                        getIncludedStudentsInfo(FGID);
                    },
                    error: function () {
                        alert('Something went worng!');
                    }
                });
            }

            function getIncludedStudentsInfo(FGID) {
                var Lang = $("#hdclutrueName").val();
                $.ajax({
                    async: false,
                    type: "POST",
                    url: "../ssappws.asmx/GetIncludedStudentsInfo",
                    data: "{'FGID':'" + FGID + "', 'Lang':'" + Lang + "'}",
                    contentType: "application/json",
                    datatype: "json",
                    success: function (responseFromServer) {
                        ResponsRes = responseFromServer.d;

                        for (let i = 0; i < ResponsRes.length; i++) {
                            var stdData = ResponsRes[i].split('|');

                            var table = document.getElementById("studentsData");
                            var row = table.insertRow(i);
                            var cell1 = row.insertCell(0);
                            var cell2 = row.insertCell(1);
                            cell1.style.textAlign = "center";
                            cell1.style.color = "orangered";
                            cell1.style.fontSize = "small";
                            cell2.style.textAlign = "center";
                            cell2.style.color = "orangered";
                            cell2.style.fontSize = "small";
                            cell1.innerHTML = stdData[0];
                            cell2.innerHTML = stdData[1];
                        }
                    },
                    error: function () {
                        alert('Something went worng!');
                    }
                });
            }
        </script>
        <!-- eof Modals -->

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
        
        <!-- JavaScript Functions -->
        <%--<script src="https://cdnjs.cloudflare.com/ajax/libs/sweetalert/1.1.3/sweetalert.min.js"></script>--%>
        <script>
            function Msg(title, msg, type) {
                swal(title, msg, type);
            }
        </script>
        <!-- eof JavaScript Functions -->

        <!-- Search in table -->
        <script>
            function dataTableFunction() {
                var input, filter, table, tr, td, i, txtValue;
                input = document.getElementById("dataTableInput");
                filter = input.value.toUpperCase();
                table = document.getElementById("dataTable1");
                tr = table.getElementsByTagName("tr");
                for (i = 0; i < tr.length; i++) {
                    for (j = 0; j < 15; j++) { //Max Number of td is 15, you can add more
                        td = tr[i].getElementsByTagName("td")[j]; 
                        if (td) {
                            txtValue = td.textContent || td.innerText;
                            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                                tr[i].style.display = "";
                            } else {
                                tr[i].style.display = "none";
                            }
                        }
                    }
                }
            }
        </script>
        <!-- eof Search in table -->
    </form>
</body>
</html>

