<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagerDash.aspx.cs" Inherits="SmartSchool.ManagerDashaspx.ManagerDash" %>

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

    <link href="css/gmapstyle.css" rel="stylesheet" type="text/css" />
    <script src="js/gmapdashfun1.0.js"></script>

    <style>
        a.specitem:link {color:white; text-decoration: none;}
        a.specitem:visited {color:white; text-decoration: none;}
        a.specitem:hover {color:black; text-decoration: none;}
    </style>
</head>

<body class="page-header-fixed sidemenu-closed-hidelogo page-content-white page-md page-full-width header-white white-sidebar-color logo-indigo">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hduid" runat="server" />

        <%--PreLoad--%>
<%--        <div class="">
            <div class="overlay"></div>
            <div class="spanner show">
                <div class="loader show"></div>
                <p>Loading ... </p>
            </div>
        </div>--%>
        <%--End PreLoad--%>

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
                            <%--Main Items--%>
                            <div class="card card-topline-aqua">
				                <div class="card card-box">
					                <div class="card-head">
						                <header>لوحة القيادة</header>
					                </div>
                                    <div class="card-body ">
                                        <div class="state-overview">
				                            <div class="row">
                                                <div class="col-xl-3 col-md-6 col-12">
                                                    <a class="specitem" href="DashFinance.aspx">
	                                                    <div class="info-box bg-b-green">
		                                                    <span class="info-box-icon push-bottom">
                                                                <i class="material-icons">monetization_on</i>
		                                                    </span>
		                                                    <div class="info-box-content">
			                                                    <span class="info-box-text">الادارة المالية</span>
			                                                    <span class="info-box-number">
                                                                    <span> عدد العقود</span>
                                                                    <asp:Label ID="LblTotalCompanyContracts" runat="server" Text=""></asp:Label>
			                                                    </span>
			                                                    <div class="progress">
				                                                    <div class="progress-bar" id="ProgAcceptedRequestsPercent" runat="server"></div>
			                                                    </div>
			                                                    <span class="progress-description">
                                                                    عقود مقبولة
                                                                    <asp:Label ID="LblAcceptedRequestsPercent" runat="server" Text=""></asp:Label>
				                                                    % 
			                                                    </span>
		                                                    </div>
	                                                    </div>
                                                    </a>
                                                </div>

					                            <div class="col-xl-3 col-md-6 col-12">
                                                    <a class="specitem" href="#">
						                                <div class="info-box bg-b-yellow">
							                                <span class="info-box-icon push-bottom">
                                                                <i class="material-icons">swap_calls</i>
							                                </span>
							                                <div class="info-box-content">
								                                <span class="info-box-text">ادارة الحركة</span>
								                                <span class="info-box-number">155</span>
								                                <div class="progress">
									                                <div class="progress-bar" style="width: 40%"></div>
								                                </div>
								                                <span class="progress-description">
									                                40% حافلات عاملة
								                                </span>
							                                </div>
						                                </div>
                                                    </a>
					                            </div>
							
                                                <div class="col-xl-3 col-md-6 col-12">
                                                    <a class="specitem" href="#">
						                                <div class="info-box bg-b-blue">
							                                <span class="info-box-icon push-bottom">
                                                                <i class="material-icons">group</i>
							                                </span>
							                                <div class="info-box-content">
								                                <span class="info-box-text">شؤون الطلاب</span>
								                                <span class="info-box-number">52</span>
								                                <div class="progress">
									                                <div class="progress-bar" style="width: 85%"></div>
								                                </div>
								                                <span class="progress-description">
									                                85% طلبة ملتزمين
								                                </span>
							                                </div>
						                                </div>
                                                    </a>
					                            </div>

					                            <div class="col-xl-3 col-md-6 col-12">
                                                    <a class="specitem" href="#">
						                                <div class="info-box bg-b-pink">
							                                <span class="info-box-icon push-bottom">
                                                                <i class="material-icons">group</i>
							                                </span>
							                                <div class="info-box-content">
								                                <span class="info-box-text">شؤون الموظفين</span>
								                                <span class="info-box-number">126</span>
								                                <div class="progress">
									                                <div class="progress-bar" style="width: 50%"></div>
								                                </div>
								                                <span class="progress-description">
									                                50% معلمين
								                                </span>
							                                </div>
						                                </div>
                                                    </a>
					                            </div>
				                            </div>
			                            </div>
                                    </div>
				                <%--</div>--%>
                            </div>
                            </div>

                            <br />
                            <%--Schools Table--%>
                            <div class="card card-topline-green">
                                <div class="card card-box">
                                    <div class="card-head">
                                        <header>المدارس</header>
                                    </div>
								    <div class="card-body " id="bar-parent">
									    <table id="exportTable" style="width:100%" class="display nowrap" style="width:100%">
										    <thead>
											    <tr>
												    <%--<th>تسلسل</th>--%>
												    <th>المدرسة</th>
                                                    <th>رقم الهاتف</th>
                                                    <th>خط عرض</th>
                                                    <th>خط طول</th>
												    <th>عرض</th>
											    </tr>
										    </thead>
										    <tbody>
                                                <asp:Repeater ID="ReptCompSchls" runat="server">
                                                    <ItemTemplate>
													    <tr>
														    <%--<td><%# Eval("SchoolID") %></td>--%>
														    <td><%# Eval("SchoolArabicName") %></td>
                                                            <td><%# Eval("SchoolContactNumber") %></td>
                                                            <td><%# Eval("Latitude") %></td>
                                                            <td><%# Eval("Longitude") %></td>
														    <td>
                                                                <a href="SchoolDetails.aspx?id=s_<%# Eval("SchoolID") %>" class="" data-toggle="tooltip" title="عرض">
                                                                    <i class="material-icons">info_outline</i>
                                                                </a>
														    </td>
													    </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
										    </tbody>
									    </table>
                                    </div>
                                </div>
                            </div>

                            <br />
                            <%--Maps--%>
                            <div class="card card-topline-red">
                                <div class="card card-box">
                                    <div class="card-head">
                                        <header>المواقع الجغرافية</header>
                                    </div>
								    <div class="card-body">
                                        <div id="map"></div>
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

    <script src="js/gmap1.0.js"></script>
    <script src="https://maps.googleapis.com/maps/api/js?key=<%=ConfigurationManager.AppSettings["GoogleAPIKey"] %>&callback=initMap&libraries=&v=weekly" defer></script>
</body>
</html>
