using Business.Base;
using Common.Helpers;
using CrystalDecisions.CrystalReports.Engine;
using DataAccess;
using Newtonsoft.Json.Linq;
using SmartSchool.Models.CrslRpts;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace SmartSchool.Controllers
{
    [Authorize]
    public class CrslRptsController : BaseController
    {

        BusinessComponentsFactory factory = new BusinessComponentsFactory();

        // GET: CrslRpts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RptGuardianContract()
        {
            RptGuardianContractModel model = new RptGuardianContractModel();

            model.Ext_GuardianID = 5;

            var ExternalGuardiansManager = factory.CreateExternalGuardiansManager();
            var ExternalGuardiansResult = ExternalGuardiansManager.Find(a => a.UserID == model.Ext_GuardianID).FirstOrDefault();

            if (ExternalGuardiansResult != null)
            {
                model.Ext_GuardianID = ExternalGuardiansResult.UserID;
                model.Ext_GuardianArabicName = ExternalGuardiansResult.ArabicName.Replace('-', ' ');
                model.Ext_GuardainEnglishName = ExternalGuardiansResult.EnglishName.Replace('-', ' ');
                model.Ext_GuardainGender = ExternalGuardiansResult.Gender == 0 ? "ذكر" : "أنثى";
            }
            else
            {
                model.Ext_GuardianID = 0;
                model.Ext_GuardianArabicName = "";
                model.Ext_GuardainEnglishName = "";
                model.Ext_GuardainGender = "";
            }

            return View(model);
        }

        public ActionResult RptGuardianContract_Show(int EXTGID) // EXTGID short for ExternalGuardianID
        {
            RptGuardianContractModel model = new RptGuardianContractModel();
            model.Ext_GuardianID = EXTGID;
            if (EXTGID == 0) { return View(model); }

            DataTable DataTbl = GetExtStdReqInfo(EXTGID);
            if (DataTbl == null || DataTbl.Rows.Count <= 0) { DataTbl.Dispose(); return View(model); }

            string AcademicYear = GetCurrentAcademicYear();
            DateTime InvDate = DateTime.Today;
            string ContractNo = "";
            double FeeAmount_TransGO = 0;
            double FeeAmount_TransReturn = 0;
            double FeeAmount_TransGOAndReturn = 0;
            DataTable StdDiscounts;
            DataTable StdSpicDiscounts;

            ////Check if the Invoice is exist.
            //if (!InvoiceExist(ContractNo, AcademicYear))
            //{
            //    //Insert data to GuardianContractInvoice
            //    PushDataToInvoiceTbl(DataTbl, ContractNo, InvDate, AcademicYear);

            //    //To get transport coast and update the fees table.
            //    int StudentID = 0;
            //    foreach (DataRow row in DataTbl.Rows)
            //    {
            //        FeeAmount_TransGO = 0;
            //        FeeAmount_TransReturn = 0;
            //        FeeAmount_TransGOAndReturn = 0;

            //        if (StudentID != row.Field<int>(1))
            //        {
            //            StudentID = row.Field<int>(1);

            //            switch (row.Field<int>(10)) //Direction 
            //            {
            //                case 0:
            //                    //Direction, SchoolID, School location, Student location
            //                    FeeAmount_TransGO = GetDircTransCoast(row.Field<int>(10),
            //                                                          row.Field<int>(2),
            //                                                          row.Field<double>(21).ToString() + "," + row.Field<double>(22).ToString(),
            //                                                          row.Field<double>(23).ToString() + "," + row.Field<double>(24).ToString()
            //                                                          );
            //                    PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                                              row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                                              FeeAmount_TransGO, "مواصلات ذهاب", "Go tansportation", "Transportation");
            //                    break;
            //                case 1:
            //                    //Direction, SchoolID, School location, Student location
            //                    FeeAmount_TransReturn = GetDircTransCoast(row.Field<int>(10),
            //                                                              row.Field<int>(2),
            //                                                              row.Field<double>(21).ToString() + "," + row.Field<double>(22).ToString(),
            //                                                              row.Field<double>(23).ToString() + "," + row.Field<double>(24).ToString()
            //                                                              );
            //                    //--------> Insert data to GuardianContractInvoice table.
            //                    PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                                              row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                                              FeeAmount_TransReturn, "مواصلات اياب", "Home tansportation", "Transportation");
            //                    break;
            //                case 2:
            //                    //Direction, SchoolID, School location, Student location
            //                    FeeAmount_TransGOAndReturn = GetDircTransCoast(row.Field<int>(10),
            //                                                                   row.Field<int>(2),
            //                                                                   row.Field<double>(21).ToString() + "," + row.Field<double>(22).ToString(),
            //                                                                   row.Field<double>(23).ToString() + "," + row.Field<double>(24).ToString()
            //                                                                   );
            //                    //--------> Insert data to GuardianContractInvoice table.
            //                    PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                                              row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                                              FeeAmount_TransGOAndReturn, "مواصلات ذهاب واياب", "Home and go tansportation", "Transportation");
            //                    break;
            //                case 3:
            //                    //FeeAmount_TransGO = 0;
            //                    //FeeAmount_TransReturn = 0;
            //                    //FeeAmount_TransGOAndReturn = 0;
            //                    //PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                    //                          row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                    //                          FeeAmount_TransGOAndReturn, "لا يوجد اشتراك", "No tansportation registered", "+");
            //                    break;
            //            }
            //        }
            //    }

            //    StudentID = 0;
            //    foreach (DataRow row in DataTbl.Rows)
            //    {
            //        if (StudentID != row.Field<int>(1))
            //        {
            //            StudentID = row.Field<int>(1);
            //            StdDiscounts = GetStudentsDiscounts(row.Field<int>(2), StudentID);

            //            if (StdDiscounts.Rows.Count > 0)
            //            {
            //                foreach (DataRow StdDiscRow in StdDiscounts.Rows)
            //                {
            //                    if (StdDiscRow.Field<bool>(2))
            //                    {
            //                        string FeeOperator;
            //                        double FeeAmountSummation;
            //                        switch (StdDiscRow.Field<int>(6))
            //                        {
            //                            case 1:
            //                                FeeOperator = "Registration";
            //                                FeeAmountSummation = GetFeeAmountSummation(EXTGID, StudentID, ContractNo, AcademicYear, FeeOperator);
            //                                FeeAmountSummation = FeeAmountSummation * StdDiscRow.Field<double>(5) * -0.01;
            //                                PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                                                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                                                           FeeAmountSummation, "% خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3) + ": " + StdDiscRow.Field<double>(5), "Not avaliable in english lang", "RegistrationDiscount");
            //                                //PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                                //                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                                //                           StdDiscRow.Field<double>(5), "خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3), "Not avaliable in english lang", "RegistrationDiscount");
            //                                break;

            //                            case 2:
            //                                FeeOperator = "Transportation";
            //                                FeeAmountSummation = GetFeeAmountSummation(EXTGID, StudentID, ContractNo, AcademicYear, FeeOperator);
            //                                FeeAmountSummation = FeeAmountSummation * StdDiscRow.Field<double>(5) * -0.01;
            //                                PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                                                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                                                           FeeAmountSummation, "% خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3) + ": " + StdDiscRow.Field<double>(5), "Not avaliable in english lang", "TransportationDiscount");
            //                                //PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                                //                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
            //                                //                           StdDiscRow.Field<double>(5), "خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3), "Not avaliable in english lang", "TransportationDiscount");
            //                                break;
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                //PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
            //                //                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3), row.Field<int>(10),
            //                //                           0, 0, " ", " ");
            //            }

            //        }
            //    }
            //}


            //Delete GuardianContractInvoice If its exist, Insert data to GuardianContractInvoice
            PushDataToInvoiceTbl(DataTbl, InvDate, AcademicYear);

            //To get transport coast and update the fees table.
            int StudentID = 0;
            foreach (DataRow row in DataTbl.Rows)
            {
                ContractNo = row.Field<int>(0).ToString() + row.Field<int>(1).ToString() + DateTime.Today.Year.ToString();
                FeeAmount_TransGO = 0;
                FeeAmount_TransReturn = 0;
                FeeAmount_TransGOAndReturn = 0;

                if (StudentID != row.Field<int>(1))
                {
                    StudentID = row.Field<int>(1);

                    switch (row.Field<int>(10)) //Direction 
                    {
                        case 0:
                            //Direction, SchoolID, School location, Student location
                            FeeAmount_TransGO = GetDircTransCoast(row.Field<int>(10),
                                                                  row.Field<int>(2),
                                                                  row.Field<double>(21).ToString() + "," + row.Field<double>(22).ToString(),
                                                                  row.Field<double>(23).ToString() + "," + row.Field<double>(24).ToString()
                                                                  );
                            PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                                                      row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                                                      FeeAmount_TransGO, "مواصلات ذهاب", "Go tansportation", "Transportation");
                            break;
                        case 1:
                            //Direction, SchoolID, School location, Student location
                            FeeAmount_TransReturn = GetDircTransCoast(row.Field<int>(10),
                                                                      row.Field<int>(2),
                                                                      row.Field<double>(21).ToString() + "," + row.Field<double>(22).ToString(),
                                                                      row.Field<double>(23).ToString() + "," + row.Field<double>(24).ToString()
                                                                      );
                            //--------> Insert data to GuardianContractInvoice table.
                            PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                                                      row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                                                      FeeAmount_TransReturn, "مواصلات اياب", "Home tansportation", "Transportation");
                            break;
                        case 2:
                            //Direction, SchoolID, School location, Student location
                            FeeAmount_TransGOAndReturn = GetDircTransCoast(row.Field<int>(10),
                                                                           row.Field<int>(2),
                                                                           row.Field<double>(21).ToString() + "," + row.Field<double>(22).ToString(),
                                                                           row.Field<double>(23).ToString() + "," + row.Field<double>(24).ToString()
                                                                           );
                            //--------> Insert data to GuardianContractInvoice table.
                            PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                                                      row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                                                      FeeAmount_TransGOAndReturn, "مواصلات ذهاب واياب", "Home and go tansportation", "Transportation");
                            break;
                        case 3:
                            //FeeAmount_TransGO = 0;
                            //FeeAmount_TransReturn = 0;
                            //FeeAmount_TransGOAndReturn = 0;
                            //PushTransDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                            //                          row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                            //                          FeeAmount_TransGOAndReturn, "لا يوجد اشتراك", "No tansportation registered", "+");
                            break;
                    }
                }
            }

            StudentID = 0;
            foreach (DataRow row in DataTbl.Rows)
            {
                if (StudentID != row.Field<int>(1))
                {
                    StudentID = row.Field<int>(1);
                    ContractNo = row.Field<int>(0).ToString() + row.Field<int>(1).ToString() + DateTime.Today.Year.ToString();
                    StdDiscounts = GetStudentsDiscounts(row.Field<int>(2), StudentID);
                    StdSpicDiscounts = GetStudentSpecialDiscount(row.Field<int>(2), StudentID);

                    if (StdDiscounts.Rows.Count > 0)
                    {
                        foreach (DataRow StdDiscRow in StdDiscounts.Rows)
                        {
                            if (StdDiscRow.Field<bool>(2))
                            {
                                string FeeOperator;
                                double FeeAmountSummation;
                                switch (StdDiscRow.Field<int>(6))
                                {
                                    case 1:
                                        FeeOperator = "Registration";
                                        FeeAmountSummation = GetFeeAmountSummation(EXTGID, StudentID, ContractNo, AcademicYear, FeeOperator);
                                        FeeAmountSummation = FeeAmountSummation * StdDiscRow.Field<double>(5) * -0.01;
                                        PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                                                                   row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                                                                   FeeAmountSummation, "% خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3) + ": " + StdDiscRow.Field<double>(5), "Not avaliable in english lang", "RegistrationDiscount");
                                        //PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                                        //                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                                        //                           StdDiscRow.Field<double>(5), "خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3), "Not avaliable in english lang", "RegistrationDiscount");
                                        break;

                                    case 2:
                                        FeeOperator = "Transportation";
                                        FeeAmountSummation = GetFeeAmountSummation(EXTGID, StudentID, ContractNo, AcademicYear, FeeOperator);
                                        FeeAmountSummation = FeeAmountSummation * StdDiscRow.Field<double>(5) * -0.01;
                                        PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                                                                   row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                                                                   FeeAmountSummation, "% خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3) + ": " + StdDiscRow.Field<double>(5), "Not avaliable in english lang", "TransportationDiscount");
                                        //PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                                        //                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                                        //                           StdDiscRow.Field<double>(5), "خصم " + StdDiscRow.Field<string>(7) + ": " + StdDiscRow.Field<string>(3), "Not avaliable in english lang", "TransportationDiscount");
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        //PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                        //                           row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3), row.Field<int>(10),
                        //                           0, 0, " ", " ");
                    }

                    if (StdSpicDiscounts != null)
                    {
                        foreach (DataRow StdSpicDiscRow in StdSpicDiscounts.Rows)
                        {
                            PushDiscntDataToInvoiceTbl(ContractNo, InvDate, AcademicYear,
                            row.Field<int>(0), row.Field<int>(1), row.Field<int>(2), row.Field<int>(4), row.Field<int>(3),
                            (Convert.ToDouble(StdSpicDiscRow.Field<decimal>(3)) * -1), "خصم خاص", "Special Discount", "RegistrationDiscount");
                        }
                    }

                }
            }

            DataTbl.Dispose();

            DataTable GuardianInvData = GetGuardianInvData(EXTGID, AcademicYear);

            ReportDocument RptDoc = new ReportDocument();
            RptDoc.Load(Path.Combine(Server.MapPath("~/CrslRpt"), "GuardianContract.rpt"));
            //RptDoc.SetDatabaseLogon("SMARTSCHOOL", "V1M7yNQ2RQ6NPJ5B", "LOCALHOST", "SmartSchools", false);
            RptDoc.SetDataSource(GuardianInvData);

            GuardianInvData.Dispose();

            RptDoc.Refresh();
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            RptDoc.PrintOptions.PaperOrientation = CrystalDecisions.Shared.PaperOrientation.Portrait;
            RptDoc.PrintOptions.PaperSize = CrystalDecisions.Shared.PaperSize.PaperA4;
            //RptDoc.PrintOptions.ApplyPageMargins(new CrystalDecisions.Shared.PageMargins(1, 1, 1, 1));

            Stream stream = RptDoc.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            RptDoc.Close();
            RptDoc.Dispose();

            return File(stream, "application/pdf", "GuardianContract.pdf");
        }

        private string GetCurrentAcademicYear()
        {
            SmartSchoolsEntities context = new SmartSchoolsEntities();
            string CurrAcademicYear = context.SystemSettings.Select(c => c.CurrentAcademicYear).SingleOrDefault();
            context.Dispose();
            return CurrAcademicYear;
        }

        private DataTable GetExtStdReqInfo(int ExternalGuardianID)
        {
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("SELECT ExternalGuardians.UserID, ExternalStudent.StudentID, ExternalStudentSchoolDetails.SchoolID, " +
                                            "       SchoolClasses.SchoolClassID, SchoolClasses.CurriculumID, SchoolClasses.ClassID, " +
                                            "       ArabicName = REPLACE(ExternalGuardians.ArabicName, '-', ' '), EnglishName = REPLACE(ExternalGuardians.EnglishName, '-', ' '), " +
                                            "       StudentArabicName = REPLACE(ExternalStudent.StudentArabicName, '-', ' '), StudentEnglishName = REPLACE(ExternalStudent.StudentEnglishName, '-', ' '), " +
                                            "       ExternalStudent.TransportDirectionID, " +
                                            "       SchoolBranches.SchoolArabicName, SchoolBranches.SchoolEnglishName, " +
                                            "       Class.ClassArabicName, Class.ClassEnglishName, " +
                                            "       Curriculums.CurriculumArabicName, Curriculums.CurriculumEnglishName, " +
                                            "       Fees.FeeID, Fees.FeeArabicName, Fees.FeeEnglishName, Fees.FeeAmount, " +
                                            "       SchoolBranches.Latitude, SchoolBranches.Longitude, " +
                                            "       ExternalStudentAdresses.Latitude, ExternalStudentAdresses.Longitude " +
                                            "FROM ExternalStudent ExternalStudent " +
                                            "INNER JOIN ExternalGuardians ExternalGuardians ON " +
                                            "((ExternalStudent.GuardianID = ExternalGuardians.UserID) AND (ExternalGuardians.UserID = " + ExternalGuardianID + ")) " +
                                            "INNER JOIN ExternalGuardStudentsRequests ExternalGuardStudentsRequests ON " +
                                            "((ExternalGuardStudentsRequests.GuardianID = ExternalGuardians.UserID) AND (ExternalGuardStudentsRequests.StudentID = ExternalStudent.StudentID) AND (ExternalGuardStudentsRequests.RequestStatus = 2)) " +
                                            "INNER JOIN ExternalStudentSchoolDetails ExternalStudentSchoolDetails ON " +
                                            "(ExternalStudentSchoolDetails.StudentID = ExternalStudent.StudentID) " +
                                            "INNER JOIN ExternalStudentAdresses ExternalStudentAdresses ON " +
                                            "(ExternalStudentAdresses.GuardianID = ExternalGuardians.UserID) " +
                                            "INNER JOIN SchoolClasses SchoolClasses ON " +
                                            "((SchoolClasses.SchoolID = ExternalStudentSchoolDetails.SchoolID) AND (SchoolClasses.SchoolClassID = ExternalStudentSchoolDetails.ClassID)) " +
                                            "INNER JOIN Fees Fees ON " +
                                            "((Fees.SchoolID = ExternalStudentSchoolDetails.SchoolID) AND (Fees.ClassID = SchoolClasses.ClassID) AND ((Fees.CurriculumID = SchoolClasses.CurriculumID) OR (Fees.CurriculumID = 0))) " +
                                            "INNER JOIN Curriculums Curriculums ON " +
                                            "(Curriculums.CurriculumID = SchoolClasses.CurriculumID) " +
                                            "INNER JOIN SchoolBranches SchoolBranches ON " +
                                            "(SchoolBranches.SchoolID = ExternalStudentSchoolDetails.SchoolID) " +
                                            "INNER JOIN Class Class ON " +
                                            "(Class.ClassID = SchoolClasses.ClassID) " +
                                            "ORDER BY SchoolClasses.CurriculumID ASC"
                                            , CONN);
            SqlDataAdapter DataAdptr = new SqlDataAdapter(CMD);
            DataTable DataTbl = new DataTable("tbl");
            DataAdptr.Fill(DataTbl);

            if (CONN.State == ConnectionState.Open)
            {
                DataAdptr.Dispose();
                CONN.Close();
                CONN.Dispose();
            }
            return DataTbl;
        }

        private bool InvoiceExist(string ContractNo, string AcademicYear)
        {
            bool invExist = true;
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("SELECT COUNT(ContractInvNo) FROM GuardianContractInvoice WHERE ContractInvNo = '" + ContractNo + "' AND ContractInvAcademicYear = " + AcademicYear, CONN);
            SqlDataReader DataReader = CMD.ExecuteReader();
            if (DataReader.Read())
            {
                if (DataReader.GetInt32(0) == 0) { invExist = false; }
            }

            CMD.Cancel();
            if (!DataReader.IsClosed) DataReader.Close();

            if (CONN.State == ConnectionState.Open)
            {
                CONN.Close();
                CONN.Dispose();
            }
            return invExist;
        }

        private void DeleteExistInvoice(string ContractNo, string AcademicYear, int GuardianID, int StudentID)
        {
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("DELETE FROM GuardianContractInvoice WHERE " +
                "ContractInvNo = " + ContractNo + " AND ContractInvAcademicYear = " + AcademicYear +
                " AND GuardianID = " + GuardianID + " AND StudentID = " + StudentID, CONN);
            CMD.ExecuteNonQuery();

            if (CONN.State == ConnectionState.Open)
            {
                CONN.Close();
                CONN.Dispose();
            }
        }


        private void PushDataToInvoiceTbl(DataTable DataTbl, DateTime InvDate, string AcademicYear)
        {
            int ParamN = 0;
            string ContractNo = "";
            foreach (DataRow row in DataTbl.Rows)
            {
                ContractNo = row.Field<int>(0).ToString() + row.Field<int>(1).ToString() + DateTime.Today.Year.ToString();
                DeleteExistInvoice(ContractNo, AcademicYear, row.Field<int>(0), row.Field<int>(1));
            }
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand();
            foreach (DataRow row in DataTbl.Rows)
            {
                ParamN++;
                ContractNo = row.Field<int>(0).ToString() + row.Field<int>(1).ToString() + DateTime.Today.Year.ToString();
                CMD.CommandText = "INSERT INTO GuardianContractInvoice (" +
                                    "ContractInvNo, ContractInvDate, ContractInvAcademicYear, GuardianID, StudentID, SchoolID, CurriculumID, SchoolClassID, " +
                                    "FeeAmount, FeeDescArabic, FeeDiscEnglish, FeeOperater " +
                                    ") VALUES (" +
                                    "@ContractInvNo" + ParamN + ", @ContractInvDate" + ParamN + ", @ContractInvAcademicYear" + ParamN + ", @GuardianID" + ParamN + ", @StudentID" + ParamN + ", @SchoolID" + ParamN + ", @CurriculumID" + ParamN + ", @SchoolClassID" + ParamN + ", " +
                                    "@FeeAmount" + ParamN + ", @FeeDescArabic" + ParamN + ", @FeeDiscEnglish" + ParamN + ", @FeeOperater" + ParamN + ")";
                CMD.Parameters.AddWithValue("@ContractInvNo" + ParamN, ContractNo);
                CMD.Parameters.AddWithValue("@ContractInvDate" + ParamN, InvDate);
                CMD.Parameters.AddWithValue("@ContractInvAcademicYear" + ParamN, AcademicYear);
                CMD.Parameters.AddWithValue("@GuardianID" + ParamN, row.Field<int>(0));
                CMD.Parameters.AddWithValue("@StudentID" + ParamN, row.Field<int>(1));
                CMD.Parameters.AddWithValue("@SchoolID" + ParamN, row.Field<int>(2));
                CMD.Parameters.AddWithValue("@CurriculumID" + ParamN, row.Field<int>(4));
                CMD.Parameters.AddWithValue("@SchoolClassID" + ParamN, row.Field<int>(3));
                CMD.Parameters.AddWithValue("@FeeAmount" + ParamN, row.Field<double>(20));
                CMD.Parameters.AddWithValue("@FeeDescArabic" + ParamN, row.Field<string>(18));
                CMD.Parameters.AddWithValue("@FeeDiscEnglish" + ParamN, row.Field<string>(19));
                CMD.Parameters.AddWithValue("@FeeOperater" + ParamN, "Registration");

                CMD.Connection = CONN;
                CMD.ExecuteNonQuery();
            }

            CMD.Cancel();
            if (CONN.State == ConnectionState.Open)
            {
                DataTbl.Dispose();
                CONN.Close();
                CONN.Dispose();
            }
        }

        private double GetDircTransCoast(int DirectionType, int SchoolID, string OriginPoint, string DistenationPoint)
        {
            double TransCoast = 0;
            double StdDistanceInMeter = 0;

            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("SELECT TransportTypeID, DistanceInMeters, TransportCategoryCostGo, TransportCategoryCostReturn, TransportCategoryCostTwoWay " +
                                            "FROM TransportCategories " +
                                            "WHERE SchoolID = " + SchoolID + " " +
                                            "ORDER BY DistanceInMeters ASC", CONN);
            SqlDataAdapter DataAdptr = new SqlDataAdapter(CMD);
            DataTable DataTbl = new DataTable("tbl");
            DataAdptr.Fill(DataTbl);

            if (DataTbl == null || DataTbl.Rows.Count <= 0)
            {
                if (CONN.State == ConnectionState.Open)
                {
                    DataAdptr.Dispose();
                    DataTbl.Dispose();
                    CMD.Cancel();
                    CONN.Close();
                    CONN.Dispose();
                }
                return TransCoast;
            }
            else
            {
                if (CONN.State == ConnectionState.Open)
                {
                    CMD.Cancel();
                    CONN.Close();
                    CONN.Dispose();
                }
            }

            switch (DirectionType)
            {
                case 0: //GO COAST
                    switch (DataTbl.Rows[0].Field<int>(0)) // Real distance OR Air distance
                    {
                        case 1: //Real distance
                            StdDistanceInMeter = GetStdRealDistance(OriginPoint, DistenationPoint);
                            StdDistanceInMeter = GetStdAirDistance(OriginPoint, DistenationPoint);
                            foreach (DataRow row in DataTbl.Rows)
                            {
                                if (StdDistanceInMeter <= row.Field<double>(1))
                                {
                                    TransCoast = row.Field<double>(2);
                                    break;
                                }
                                else
                                {
                                    TransCoast = row.Field<double>(2);
                                }
                            }
                            break;

                        case 2: //Air distance
                            StdDistanceInMeter = GetStdAirDistance(OriginPoint, DistenationPoint);
                            foreach (DataRow row in DataTbl.Rows)
                            {
                                if (StdDistanceInMeter <= row.Field<double>(1))
                                {
                                    TransCoast = row.Field<double>(2);
                                    break;
                                }
                                else
                                {
                                    TransCoast = row.Field<double>(2);
                                }
                            }
                            break;
                    }
                    break;

                case 1: //RETURN COAST
                    switch (DataTbl.Rows[0].Field<int>(0)) // Real distance OR Air distance
                    {
                        case 1: //Real distance
                            StdDistanceInMeter = GetStdRealDistance(OriginPoint, DistenationPoint);
                            foreach (DataRow row in DataTbl.Rows)
                            {
                                if (StdDistanceInMeter <= row.Field<double>(1))
                                {
                                    TransCoast = row.Field<double>(3);
                                    break;
                                }
                                else
                                {
                                    TransCoast = row.Field<double>(3);
                                }
                            }
                            break;

                        case 2: //Air distance
                            StdDistanceInMeter = GetStdAirDistance(OriginPoint, DistenationPoint);
                            foreach (DataRow row in DataTbl.Rows)
                            {
                                if (StdDistanceInMeter <= row.Field<double>(1))
                                {
                                    TransCoast = row.Field<double>(3);
                                    break;
                                }
                                else
                                {
                                    TransCoast = row.Field<double>(3);
                                }
                            }
                            break;
                    }
                    break;

                case 2: //GO AND RETURN COAST
                    switch (DataTbl.Rows[0].Field<int>(0)) // Real distance OR Air distance
                    {
                        case 1: //Real distance
                            StdDistanceInMeter = GetStdRealDistance(OriginPoint, DistenationPoint);
                            foreach (DataRow row in DataTbl.Rows)
                            {
                                if (StdDistanceInMeter <= row.Field<double>(1))
                                {
                                    TransCoast = row.Field<double>(4);
                                    break;
                                }
                                else
                                {
                                    TransCoast = row.Field<double>(4);
                                }
                            }
                            break;

                        case 2: //Air distance
                            StdDistanceInMeter = GetStdAirDistance(OriginPoint, DistenationPoint);
                            foreach (DataRow row in DataTbl.Rows)
                            {
                                if (StdDistanceInMeter <= row.Field<double>(1))
                                {
                                    TransCoast = row.Field<double>(4);
                                    break;
                                }
                                else
                                {
                                    TransCoast = row.Field<double>(4);
                                }
                            }
                            break;
                    }
                    break;

                case 3: //COAST = 0
                    return 0;
            }

            return TransCoast;
        }

        private double GetStdAirDistance(string OriginPoint, string DistenationPoint)
        {
            double Distance = 0.0;
            const double R = 6371.0; //Kilo meter (3960 in Mile)
            double φ1;
            double φ2;
            double Δλ;

            try
            {
                string[] OriginPnt = OriginPoint.Split(',');
                string[] DistenationPnt = DistenationPoint.Split(',');
                double OriginLat = double.Parse(OriginPnt[0]);
                double OriginLng = double.Parse(OriginPnt[1]);
                double DistenationLat = double.Parse(DistenationPnt[0]);
                double DistenationLng = double.Parse(DistenationPnt[1]);

                φ1 = (Math.PI / 180) * OriginLat;
                φ2 = (Math.PI / 180) * DistenationLat;
                Δλ = (Math.PI / 180) * (OriginLng - DistenationLng);

                Distance = Math.Round(Math.Acos(Math.Sin(φ1) * Math.Sin(φ2) + Math.Cos(φ1) * Math.Cos(φ2) * Math.Cos(Δλ)) * R, 1);
            }
            catch
            {
                Distance = 0;
            }

            return Distance * 1000; //Convert Kilo meter to meter
        }

        private double GetStdRealDistance(string OriginPoint, string DistenationPoint)
        {
            double Distance = 0.0;

            try
            {
                try
                {
                    string jsonData = DistanceMatrixRequest(OriginPoint, DistenationPoint, "Driving", "AIzaSyAC4eYLfDTVH4rIJsY4ZnwZpNBnWugR4wg");
                    JObject o = JObject.Parse(jsonData);
                    Distance = Math.Round((double)o.SelectToken("rows[0].elements[0].distance.value"), 1);
                }
                catch
                {
                    Distance = 0;
                }
            }
            catch
            {
                Distance = 0;
            }

            return Distance;
        }

        public string DistanceMatrixRequest(string source, string Destination, string travelMode, string keyString)
        {
            try
            {
                string urlRequest = "";
                urlRequest = @"https://maps.googleapis.com/maps/api/distancematrix/json?origins=" + source + "&destinations=" + Destination + "&mode=" + travelMode + "&key=" + keyString + "&sensor=false";

                WebRequest request = WebRequest.Create(urlRequest);
                request.Method = "POST";
                string postData = "This is a test that posts this string to a Web server.";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);
                string resp = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                return resp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void PushTransDataToInvoiceTbl(string ContractNo, DateTime InvDate, string AcademicYear,
                                               int GuardianID, int StudentID, int SchoolID, int CurriculumID, int SchoolClassID,
                                               double FeeAmount, string FeeDescArabic, string FeeDiscEnglish, string FeeOperater)
        {
            int ParamN = 0;
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand();

            ParamN++;
            CMD.CommandText = "INSERT INTO GuardianContractInvoice (" +
                                "ContractInvNo, ContractInvDate, ContractInvAcademicYear, GuardianID, StudentID, SchoolID, CurriculumID, SchoolClassID, " +
                                "FeeAmount, FeeDescArabic, FeeDiscEnglish, FeeOperater " +
                                ") VALUES (" +
                                "@ContractInvNo" + ParamN + ", @ContractInvDate" + ParamN + ", @ContractInvAcademicYear" + ParamN + ", @GuardianID" + ParamN + ", @StudentID" + ParamN + ", @SchoolID" + ParamN + ", @CurriculumID" + ParamN + ", @SchoolClassID" + ParamN + ", " +
                                "@FeeAmount" + ParamN + ", @FeeDescArabic" + ParamN + ", @FeeDiscEnglish" + ParamN + ", @FeeOperater" + ParamN + ")";
            CMD.Parameters.AddWithValue("@ContractInvNo" + ParamN, ContractNo);
            CMD.Parameters.AddWithValue("@ContractInvDate" + ParamN, InvDate);
            CMD.Parameters.AddWithValue("@ContractInvAcademicYear" + ParamN, AcademicYear);
            CMD.Parameters.AddWithValue("@GuardianID" + ParamN, GuardianID);
            CMD.Parameters.AddWithValue("@StudentID" + ParamN, StudentID);
            CMD.Parameters.AddWithValue("@SchoolID" + ParamN, SchoolID);
            CMD.Parameters.AddWithValue("@CurriculumID" + ParamN, CurriculumID);
            CMD.Parameters.AddWithValue("@SchoolClassID" + ParamN, SchoolClassID);
            CMD.Parameters.AddWithValue("@FeeAmount" + ParamN, FeeAmount);
            CMD.Parameters.AddWithValue("@FeeDescArabic" + ParamN, FeeDescArabic);
            CMD.Parameters.AddWithValue("@FeeDiscEnglish" + ParamN, FeeDiscEnglish);
            CMD.Parameters.AddWithValue("@FeeOperater" + ParamN, FeeOperater);

            CMD.Connection = CONN;
            CMD.ExecuteNonQuery();

            CMD.Cancel();
            if (CONN.State == ConnectionState.Open)
            {
                CONN.Close();
                CONN.Dispose();
            }
        }

        private DataTable GetStudentsDiscounts(int SchoolID, int StudentID)
        {
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("SELECT DiscountStudent.StudentID, DiscountStudent.DiscountID, DiscountStudent.IsYes, " +
                                            "       Discounts.DiscountDescription, Discounts.SchoolID, Discounts.DiscountPercentage, Discounts.DiscountTypeID, " +
                                            "       DiscountTypes.DiscountTypeArabic " +
                                            "FROM DiscountStudent DiscountStudent " +
                                            "INNER JOIN Discounts Discounts ON " +
                                            "((Discounts.DiscountID = DiscountStudent.DiscountID) AND (DiscountStudent.StudentID = " + StudentID + ") AND (Discounts.SchoolID = " + SchoolID + ")) " +
                                            "INNER JOIN DiscountTypes DiscountTypes ON " +
                                            "(DiscountTypes.DiscountTypeID = Discounts.DiscountTypeID) " +
                                            "ORDER BY DiscountStudent.StudentID ASC", CONN);
            SqlDataAdapter DataAdptr = new SqlDataAdapter(CMD);
            DataTable DataTbl = new DataTable("StdDistbl");
            DataAdptr.Fill(DataTbl);

            if (CONN.State == ConnectionState.Open)
            {
                DataAdptr.Dispose();
                CONN.Close();
                CONN.Dispose();
            }
            return DataTbl;
        }

        private DataTable GetStudentSpecialDiscount(int SchoolID, int StudentID)
        {
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("SELECT * FROM SpecialDiscount WHERE StudentID = " + StudentID + " AND SchoolID = " + SchoolID, CONN);
            SqlDataAdapter DataAdptr = new SqlDataAdapter(CMD);
            DataTable DataTbl = new DataTable("StdSpicDistbl");
            DataAdptr.Fill(DataTbl);

            if (CONN.State == ConnectionState.Open)
            {
                DataAdptr.Dispose();
                CONN.Close();
                CONN.Dispose();
            }
            return DataTbl;
        }

        private void PushDiscntDataToInvoiceTbl(string ContractNo, DateTime InvDate, string AcademicYear,
                                                int GuardianID, int StudentID, int SchoolID, int CurriculumID, int SchoolClassID,
                                                double FeeAmount, string FeeDescArabic, string FeeDiscEnglish, string FeeOperater)
        {
            int ParamN = 0;
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand();

            ParamN++;
            CMD.CommandText = "INSERT INTO GuardianContractInvoice (" +
                                "ContractInvNo, ContractInvDate, ContractInvAcademicYear, GuardianID, StudentID, SchoolID, CurriculumID, SchoolClassID, " +
                                "FeeAmount, FeeDescArabic, FeeDiscEnglish, FeeOperater " +
                                ") VALUES (" +
                                "@ContractInvNo" + ParamN + ", @ContractInvDate" + ParamN + ", @ContractInvAcademicYear" + ParamN + ", @GuardianID" + ParamN + ", @StudentID" + ParamN + ", @SchoolID" + ParamN + ", @CurriculumID" + ParamN + ", @SchoolClassID" + ParamN + ", " +
                                "@FeeAmount" + ParamN + ", @FeeDescArabic" + ParamN + ", @FeeDiscEnglish" + ParamN + ", @FeeOperater" + ParamN + ")";
            CMD.Parameters.AddWithValue("@ContractInvNo" + ParamN, ContractNo);
            CMD.Parameters.AddWithValue("@ContractInvDate" + ParamN, InvDate);
            CMD.Parameters.AddWithValue("@ContractInvAcademicYear" + ParamN, AcademicYear);
            CMD.Parameters.AddWithValue("@GuardianID" + ParamN, GuardianID);
            CMD.Parameters.AddWithValue("@StudentID" + ParamN, StudentID);
            CMD.Parameters.AddWithValue("@SchoolID" + ParamN, SchoolID);
            CMD.Parameters.AddWithValue("@CurriculumID" + ParamN, CurriculumID);
            CMD.Parameters.AddWithValue("@SchoolClassID" + ParamN, SchoolClassID);
            CMD.Parameters.AddWithValue("@FeeAmount" + ParamN, FeeAmount);
            CMD.Parameters.AddWithValue("@FeeDescArabic" + ParamN, FeeDescArabic);
            CMD.Parameters.AddWithValue("@FeeDiscEnglish" + ParamN, FeeDiscEnglish);
            CMD.Parameters.AddWithValue("@FeeOperater" + ParamN, FeeOperater);

            CMD.Connection = CONN;
            CMD.ExecuteNonQuery();

            CMD.Cancel();
            if (CONN.State == ConnectionState.Open)
            {
                CONN.Close();
                CONN.Dispose();
            }
        }

        private DataTable GetGuardianInvData(int GuardianID, string CurrentAcademicYear)
        {
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("SELECT GuardianContractInvoice.ContractInvNo, GuardianContractInvoice.ContractInvDate, GuardianContractInvoice.GuardianID, GuardianContractInvoice.StudentID, " +
                                            "       GuardianContractInvoice.SchoolID, GuardianContractInvoice.FeeAmount, GuardianContractInvoice.FeeDescArabic, GuardianContractInvoice.FeeDiscEnglish, GuardianContractInvoice.FeeOperater, " +
                                            "       ArabicName = REPLACE(ExternalGuardians.ArabicName, '-', ' '), EnglishName = REPLACE(ExternalGuardians.EnglishName, '-', ' '), " +
                                            "       SchoolBranches.SchoolArabicName, SchoolBranches.SchoolEnglishName, " +
                                            "       StudentArabicName = REPLACE(ExternalStudent.StudentArabicName, '-', ' '), StudentEnglishName = REPLACE(ExternalStudent.StudentEnglishName, '-', ' '), " +
                                            "       Curriculums.CurriculumArabicName, Curriculums.CurriculumEnglishName, " +
                                            "       Class.ClassArabicName, Class.ClassEnglishName " +
                                            "FROM GuardianContractInvoice GuardianContractInvoice " +
                                            "INNER JOIN ExternalGuardians ExternalGuardians ON " +
                                            "(ExternalGuardians.UserID = GuardianContractInvoice.GuardianID) " +
                                            "INNER JOIN SchoolBranches SchoolBranches ON " +
                                            "(SchoolBranches.SchoolID = GuardianContractInvoice.SchoolID) " +
                                            "INNER JOIN ExternalStudent ExternalStudent ON " +
                                            "(ExternalStudent.StudentID = GuardianContractInvoice.StudentID) " +
                                            "INNER JOIN Curriculums Curriculums ON " +
                                            "(Curriculums.CurriculumID = GuardianContractInvoice.CurriculumID) " +
                                            "INNER JOIN SchoolClasses SchoolClasses ON " +
                                            "(SchoolClasses.SchoolClassID = GuardianContractInvoice.SchoolClassID) " +
                                            "INNER JOIN Class Class ON " +
                                            "(Class.ClassID = SchoolClasses.ClassID) " +
                                            "WHERE GuardianContractInvoice.GuardianID = " + GuardianID + " AND GuardianContractInvoice.ContractInvAcademicYear = '" + CurrentAcademicYear + "'", CONN);
            SqlDataAdapter DataAdptr = new SqlDataAdapter(CMD);
            DataTable DataTbl = new DataTable("GInvDtbl");
            DataAdptr.Fill(DataTbl);

            if (CONN.State == ConnectionState.Open)
            {
                DataAdptr.Dispose();
                CONN.Close();
                CONN.Dispose();
            }
            return DataTbl;
        }

        private double GetFeeAmountSummation(int GuardianID, int StudentID, string ContractNo, string AcademicYear, string FeeOperator)
        {
            double FeesSummation;
            SqlConnection CONN = new SqlConnection(ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString);
            if (CONN.State == ConnectionState.Closed) CONN.Open();
            SqlCommand CMD = new SqlCommand("SELECT DISTINCT GuardianID, ContractInvNo, ContractInvAcademicYear, " +
                                            "       FeesSummation = (SELECT SUM(FeeAmount) FROM GuardianContractInvoice WHERE FeeOperater = '" + FeeOperator + "' AND GuardianID = " + GuardianID + " AND StudentID = " + StudentID + ") " +
                                            "FROM GuardianContractInvoice " +
                                            "WHERE GuardianID = " + GuardianID + " AND ContractInvNo = '" + ContractNo + "' AND ContractInvAcademicYear = '" + AcademicYear + "'", CONN);

            SqlDataReader DataReader = CMD.ExecuteReader();
            if (DataReader.Read())
            {
                FeesSummation = double.Parse(DataReader["FeesSummation"].ToString());
            }
            else FeesSummation = 0;

            CMD.Cancel();
            if (!DataReader.IsClosed) DataReader.Close();

            if (CONN.State == ConnectionState.Open)
            {
                CONN.Close();
                CONN.Dispose();
            }

            return FeesSummation;
        }
    }
}