<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SchoolDetails.aspx.cs" Inherits="SmartSchool.ManagerDashaspx.SchoolDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta content="width=device-width, initial-scale=1" name="viewport" />
    <meta name="theme-color" content="#3699ff"/>
    <meta name="description" content="smartschool,smart,school,student,parent,learn" />
    <title>Smart Schools</title>
    <link href="/AppContent/rtl.css" rel="stylesheet" />

	<link href="https://fonts.googleapis.com/css?family=Poppins:300,400,500,600,700" rel="stylesheet" type="text/css" />
	<link href="fonts/simple-line-icons/simple-line-icons.min.css" rel="stylesheet" type="text/css" />
	<link href="fonts/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
	<link href="fonts/material-design-icons/material-icon.css" rel="stylesheet" type="text/css" />
	<link href="../assets/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
	<link href="../assets/plugins/summernote/summernote.css" rel="stylesheet"/>
	<link rel="stylesheet" href="../assets/plugins/material/material.min.css"/>
	<link rel="stylesheet" href="../assets/css/material_style.css"/>
	<link href="../assets/css/pages/inbox.min.css" rel="stylesheet" type="text/css" />
	<link href="../assets/css/theme/full/theme_style.css" rel="stylesheet" id="rt_style_components" type="text/css" />
	<link href="../assets/css/plugins.min.css" rel="stylesheet" type="text/css" />
	<link href="../assets/css/theme/full/style.css" rel="stylesheet" type="text/css" />
	<link href="../assets/css/responsive.css" rel="stylesheet" type="text/css" />
	<link href="../assets/css/theme/full/theme-color.css" rel="stylesheet" type="text/css" />

	<link href="../assets/plugins/datatables/plugins/bootstrap/dataTables.bootstrap4.min.css" rel="stylesheet" type="text/css" />
	<link href="../assets/plugins/datatables/export/buttons.dataTables.min.css" rel="stylesheet" type="text/css" />

    <link href="../assets/css/pages/formlayout.css" rel="stylesheet" type="text/css" />
</head>

<body class="page-header-fixed sidemenu-closed-hidelogo page-content-white page-md page-full-width header-white white-sidebar-color logo-indigo">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hduid" runat="server" />
        <asp:HiddenField ID="hdschid" runat="server" />

        <%--Header--%>
        <div class="page-wrapper">
            <div class="page-header navbar navbar-fixed-top">
                <div class="page-header-inner">

                    <div class="page-logo">
                        <a href="/" class="header-logo-anchor">
                            <span class="logo-default">
                                <img src="../AppContent/Images/smartschoollogon.png" alt="smart school logo"/>
                            </span>
                        </a>
                    </div>

                    <div class="top-menu">

                        <ul class="nav navbar-nav pull-right">
                            <li class="dropdown dropdown-user">
                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" data-hover="dropdown"
                                    data-close-others="true">
                                    <img alt="" class="img-circle " src="../assets/img/dp.jpg" />
                                    <span class="username username-hide-on-mobile"><asp:Label ID="LblUName" runat="server"></asp:Label></span>
                                    <i class="fa fa-angle-down"></i>
                                </a>
                                <ul class="dropdown-menu dropdown-menu-default">
                                    <li>
                                        <a href="/Account/Logout">
                                            <i class="icon-logout"></i> تسجيل الخروج
                                        </a>
                                    </li>
                                </ul>
                            </li>
                        </ul>

                    </div>
                </div>
            </div>
        </div>
        <%--End Header--%>

        <br />

	    <div class="page-content-wrapper">
		    <div class="page-content">

		        <div class="row">
			        <div class="col-md-12">

                        <%--Page Content--%>
                        <div class="card card-box">
                            <%--Back--%>
                            <div class="card-head">
                                <header>
                                    <button data-toggle="button" class="btn btn-circle btn-dark" onclick="location.href='ManagerDash.aspx';">
                                        <i class="fa fa-location-arrow"></i> 
                                        عودة
                                    </button>
                                </header>
                            </div>

                            <%--School Information--%>
                            <div class="card card-topline-aqua">
                                <div class="card card-box">
                                    <div class="card-head">
                                        <header>مدرسة "<asp:Label ID="LblSchoolName" runat="server" Text=""></asp:Label>"</header>
                                    </div>
                                    <div class="row">
						                <div class="col-md-4 col-sm-6">
							                <div class="card card-box">
								                <div class="card-body ">
									                <div class="form-group has-success">
                                                        <label class="control-label" for="LblTotalEmployees">الموظفين</label>
                                                        <asp:Label ID="LblTotalEmployees" runat="server" Text="20,350" Font-Bold="true" class="form-control"></asp:Label>
                                                        <br />
                                                        <asp:Button ID="BtnTotalEmployees" runat="server" OnClick="BtnTotalEmployees_Click" Text="عرض التفاصيل" class="btn dark btn-outline m-b-10" />
									                </div>
								                </div>
							                </div>
						                </div>

						                <div class="col-md-4 col-sm-6">
							                <div class="card card-box">
								                <div class="card-body ">
									                <div class="form-group has-warning">
                                                        <label class="control-label" for="LblTotalStudents">الطلاب</label>
                                                        <asp:Label ID="LblTotalStudents" runat="server" Text="" Font-Bold="true" class="form-control"></asp:Label>
                                                        <br />
                                                        <asp:Button ID="BtnTotalStudents" runat="server" OnClick="BtnTotalStudents_Click" Text="عرض التفاصيل" class="btn dark btn-outline m-b-10" />
									                </div>
								                </div>
							                </div>
						                </div>

						                <div class="col-md-4 col-sm-6">
							                <div class="card card-box">
								                <div class="card-body ">
									                <div class="form-group has-error">
                                                        <label class="control-label" for="LblTotalBuses">الحافلات</label>
                                                        <asp:Label ID="LblTotalBuses" runat="server" Text="20,350" Font-Bold="true" class="form-control"></asp:Label>
                                                        <br />
                                                        <asp:Button ID="BtnTotalBuses" runat="server" OnClick="BtnTotalBuses_Click" Text="عرض التفاصيل" class="btn dark btn-outline m-b-10" />
									                </div>
								                </div>
							                </div>
						                </div>

                                    </div>
                                </div>
                            </div>

                            <%--Data Table Of Staff--%>
                            <div class="card card-topline-lightblue" id="DivTblStaffData" runat="server">
                                <div class="card card-box">
                                    <div class="card-head">
                                        <header><asp:Label ID="LblTblStaffData" runat="server" Text="" ForeColor="OrangeRed"></asp:Label></header>
                                    </div>
								    <div class="card-body " id="bar-parent">
									    <table id="exportTable" style="width:100%" class="display nowrap" style="width:100%">
										    <thead>
											    <tr>
												    <th>الاسم</th>
                                                    <th>رقم الهاتف النقال</th>
                                                    <th>القسم</th>
                                                    <th>المسمى الوضيفي</th>
                                                    <th>تاريخ الالتحاق</th>
                                                    <th>الجنسية</th>
											    </tr>
										    </thead>
										    <tbody>
                                                <asp:Repeater ID="ReptTblStaffData" runat="server">
                                                    <ItemTemplate>
													    <tr>
														    <td><%# Eval("StaffArabicName") %></td>
                                                            <td><%# Eval("MobileNo") %></td>
                                                            <td><%# Eval("DepartmentArabicName") %></td>
                                                            <td><%# Eval("DesignationArabicText") %></td>
                                                            <td><%# Eval("DateOfJoining") %></td>
                                                            <td><%# Eval("ArabicNationality") %></td>
													    </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
										    </tbody>
									    </table>
                                    </div>
                                </div>
                            </div>

                            <%--Data Table Of Students--%>
                            <div class="card card-topline-lightblue" id="DivTblStudentsData" runat="server">
                                <div class="card card-box">
                                    <div class="card-head">
                                        <header><asp:Label ID="LblTblStudentsData" runat="server" Text="" ForeColor="OrangeRed"></asp:Label></header>
                                    </div>
								    <div class="card-body " id="bar-parent">
									    <table id="exportTable" style="width:100%" class="display nowrap" style="width:100%">
										    <thead>
											    <tr>
												    <th>الاسم</th>
                                                    <th>ولي الأمر</th>
                                                    <th>الصف</th>
                                                    <th>الحالة</th>
											    </tr>
										    </thead>
										    <tbody>
                                                <asp:Repeater ID="ReptTblStudentsData" runat="server">
                                                    <ItemTemplate>
													    <tr>
														    <td><%# Eval("StudentArabicName") %></td>
                                                            <td><%# Eval("GuardianArabicName") %></td>
                                                            <td><%# Eval("ClassArabicName") %></td>
                                                            <td><%# Eval("StatusArabicText") %></td>
													    </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
										    </tbody>
									    </table>
                                    </div>
                                </div>
                            </div>

                            <%--Data Table Of Students--%>
                            <div class="card card-topline-lightblue" id="DivTblBusesData" runat="server">
                                <div class="card card-box">
                                    <div class="card-head">
                                        <header><asp:Label ID="LblTblBusesData" runat="server" Text="" ForeColor="OrangeRed"></asp:Label></header>
                                    </div>
								    <div class="card-body " id="bar-parent">
									    <table id="exportTable" style="width:100%" class="display nowrap" style="width:100%">
										    <thead>
											    <tr>
												    <th>رقم الحافلة</th>
                                                    <th>رقم اللوحة</th>
                                                    <th>المالك</th>
                                                    <th>سائق الحافلة</th>
                                                    <th>عدد المقاعد</th>
											    </tr>
										    </thead>
										    <tbody>
                                                <asp:Repeater ID="ReptTblBusesData" runat="server">
                                                    <ItemTemplate>
													    <tr>
														    <td><%# Eval("BusNo") %></td>
                                                            <td><%# Eval("PlateNumber") %></td>
                                                            <td><%# Eval("BusOwner") %></td>
                                                            <td><%# Eval("StaffArabicName") %></td>
                                                            <td><%# Eval("NumberofSeats") %></td>
													    </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
										    </tbody>
									    </table>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <%--End Page Content--%>
                    
                        <%--Footer--%>
		                <div class="page-footer">
			                <div class="page-footer-inner"> Super-powered by Zlious Tech © 2022-2023
			                </div>
			                <div class="scroll-to-top">
				                <i class="icon-arrow-up"></i>
			                </div>
		                </div>
                        <%--End Footer--%>

                    </div>
                </div>

            </div>
        </div>
    </form>

	<script src="../assets/plugins/jquery/jquery.min.js"></script>
	<script src="../assets/plugins/popper/popper.js"></script>
	<script src="../assets/plugins/jquery-blockui/jquery.blockui.min.js"></script>
	<script src="../assets/plugins/jquery-slimscroll/jquery.slimscroll.js"></script>
	<script src="../assets/plugins/bootstrap/js/bootstrap.min.js"></script>
	<script src="../assets/plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
	<script src="../assets/plugins/sparkline/jquery.sparkline.js"></script>
	<script src="../assets/js/pages/sparkline/sparkline-data.js"></script>
	<script src="../assets/js/app.js"></script>
	<script src="../assets/js/layout.js"></script>
	<script src="../assets/js/theme-color.js"></script>
	<script src="../assets/plugins/material/material.min.js"></script>

	<script src="../assets/plugins/chart-js/Chart.bundle.js"></script>
	<script src="../assets/plugins/chart-js/utils.js"></script>
	<script src="../assets/js/pages/chart/chartjs/home-data.js"></script>
	<script src="../assets/plugins/summernote/summernote.js"></script>
	<script src="../assets/js/pages/summernote/summernote-data.js"></script>

	<script src="../assets/plugins/datatables/jquery.dataTables.min.js"></script>
	<script src="../assets/plugins/datatables/plugins/bootstrap/dataTables.bootstrap4.min.js"></script>
	<script src="../assets/plugins/datatables/export/dataTables.buttons.min.js"></script>
	<script src="../assets/plugins/datatables/export/buttons.flash.min.js"></script>
	<script src="../assets/plugins/datatables/export/jszip.min.js"></script>
	<script src="../assets/plugins/datatables/export/pdfmake.min.js"></script>
	<script src="../assets/plugins/datatables/export/vfs_fonts.js"></script>
	<script src="../assets/plugins/datatables/export/buttons.html5.min.js"></script>
	<script src="../assets/plugins/datatables/export/buttons.print.min.js"></script>
	<script src="../assets/js/pages/table/table_data.js"></script>
</body>
</html>
