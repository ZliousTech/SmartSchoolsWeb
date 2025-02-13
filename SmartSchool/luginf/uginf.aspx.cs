using Common.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace SmartSchool.luginf
{
    public partial class uginf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    if (String.IsNullOrEmpty(Request.QueryString["sid"]))
            //    {
            //        ShowMsg("هناك قيم غير موجودة او غير معرفة", "error");
            //        Response.AddHeader("REFRESH", "4;URL=https://smartschool.zlioustech.com");
            //        return;
            //    }

            //    if (String.IsNullOrEmpty(Request.QueryString["aid"]))
            //    {
            //        ShowMsg("هناك قيم غير موجودة او غير معرفة", "error");
            //        Response.AddHeader("REFRESH", "4;URL=https://smartschool.zlioustech.com");
            //        return;
            //    }

            //    string StudentID = Request.QueryString["sid"];
            //    hdsid.Value = StudentID;
            //    string AddressID = Request.QueryString["aid"];
            //    hdaid.Value = AddressID;

            //    GetStudentInfo(SystemBase.DecryptF(hdsid.Value));
            //}
            //catch (Exception exp)
            //{
            //    string Excep = Convert.ToString(exp);
            //    ShowMsg("هناك قيم غير موجودة او غير معرفة", "error");
            //    Response.AddHeader("REFRESH", "4;URL=https://smartschool.zlioustech.com");
            //    return;
            //}

            SetCulture();
            SetASPElementsTxt();

            //ShowMsg(hdclutrueName.Value.Contains("en")
            //    ?
            //    "Updating information through the web app is temporarily suspended, please update information through the mobile app."
            //    :
            //    "تم إيقاف تعديل المعلومات عبر تطبيق الويب مؤقتًا، يرجى تعديل المعلومات عبر تطبيق الهاتف المحمول.", "info");
            //Response.AddHeader("REFRESH", "6;URL=https://smartschool.zlioustech.com");
        }

        private void SetCulture()
        {
            string CultureName = SystemBase.GetCurrentCultureName("_culture");
            if (String.IsNullOrEmpty(CultureName)) CultureName = "ar";
            SystemBase.SetCulture("_culture", CultureName);
            hdclutrueName.Value = CultureName;
        }

        private void SetASPElementsTxt()
        {
            BtnUpdate.Text = Resources.Resource.Update;
        }

        private void ShowMsg(string Msg, string MsgType)
        {
            switch (MsgType)
            {
                case "success":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.WellDone + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;

                case "error":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.SorrySomethingWentWrong + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;

                case "info":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.MsgInfoType + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;

                case "warning":
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>Msg('" + Resources.Resource.MsgWarningType + "', '" + Msg + "', '" + MsgType + "');</script>", false);
                    break;
            }
        }

        private void GetStudentInfo(string StudentID)
        {
            DataTable StudentInfo =
                SystemBase.GetDataTble($"SELECT StudentArabicName " +
                                       $"FROM Student " +
                                       $"WHERE StudentID = '{StudentID}'");
            if (StudentInfo != null)
            {
                if (StudentInfo.Rows.Count > 0)
                {
                    LblStdName.Text = StudentInfo.Rows[0]["StudentArabicName"].ToString();
                }
                else
                {
                    ShowMsg("هناك قيم غير موجودة او غير معرفة", "error");
                    Response.AddHeader("REFRESH", "4;URL=https://smartschool.zlioustech.com");
                }
            }
            else
            {
                ShowMsg("هناك قيم غير موجودة او غير معرفة", "error");
                Response.AddHeader("REFRESH", "4;URL=https://smartschool.zlioustech.com");
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            string CheckInputsRes = CheckInputs();
            if (CheckInputsRes != "success")
            {
                ShowMsg(CheckInputsRes, "error");
                return;
            }

            string GuardianID = GetGuardianID(TxtNationalNumber.Text.Trim(), TxtMobileNo.Text.Trim());
            if (GuardianID == "0")
            {
                ShowMsg(hdclutrueName.Value.Contains("en")
                    ?
                    "Your information was not found"
                    :
                    "لم يتم العثور على معلوماتك", "error");
                return;
            }

            try
            {
                string query = "";
                string conStr = ConfigurationManager.ConnectionStrings["SCHOOLCONSTR"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    query = $"UPDATE StudentAdresses SET " +
                            $"       longitude = @longitude, " +
                            $"       latitude = @latitude " +
                            $"WHERE GuardianID = @GuardianID";
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@longitude", hdlng.Value.Trim());
                        comm.Parameters.AddWithValue("@latitude", hdlat.Value.Trim());
                        comm.Parameters.AddWithValue("@GuardianID", GuardianID);
                        comm.ExecuteNonQuery();
                    }

                    query = $"UPDATE Guardians SET " +
                            $"       MobileNumber = @MobileNumber " +
                            $"WHERE GuardianID = @GuardianID";
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@MobileNumber", TxtMobileNo.Text.Trim());
                        comm.Parameters.AddWithValue("@GuardianID", GuardianID);
                        comm.ExecuteNonQuery();
                    }

                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg(hdclutrueName.Value.Contains("en")
                    ?
                    "The update was not saved, please try again"
                    :
                    "لم يتم حفظ التعديل، الرجاء قم بالمحاولة مرة أخرى", "error");
                Console.WriteLine(ex.Message);
                return;
            }

            ShowMsg(hdclutrueName.Value.Contains("")
                ?
                "The update has been saved successfully."
                :
                "تم حفظ التعديل بنجاح", "info");
            //Response.AddHeader("REFRESH", "4;URL=https://smartschool.zlioustech.com");
        }

        private string CheckInputs()
        {
            string Res = "success";

            if (String.IsNullOrEmpty(TxtNationalNumber.Text) || !SystemBase.RegIsMatch(TxtNationalNumber.Text.Trim(), "REGULAR_TEXT"))
            {
                return "يرجى التحقق من صحة الرقم القومي";
            }

            if (String.IsNullOrEmpty(TxtMobileNo.Text) || !SystemBase.RegIsMatch(TxtMobileNo.Text.Trim(), "REGULAR_TEXT"))
            {
                return "يرجى التحقق من صحة رقم الهاتف النقال";
            }

            if (hdlat.Value.Trim() == "" || hdlng.Value.Trim() == "")
            {
                return "يجب تحديد موقع الطالب على الخريطة";
            }

            //if (!SystemBase.RegIsMatch(SystemBase.DecryptF(hdlat.Value), "NUMBER-OR-DECIMAL") || !SystemBase.RegIsMatch(SystemBase.DecryptF(hdlng.Value), "NUMBER-OR-DECIMAL"))
            //{
            //    ShowMsg("لم يتم تحديد الموقع بشكل صحيح", "error");
            //    return;
            //}

            return Res;
        }

        private string GetGuardianID(string NationalNumber, string MobileNo)
        {
            string Res = "0";

            DataTable GuardianInfo =
                SystemBase.GetDataTble($"SELECT G.GuardianID " +
                                       $"FROM Student S " +
                                       $"INNER JOIN StudentSchoolDetails SSD ON " +
                                       $"(SSD.StudentID = S.StudentID) " +
                                       $"INNER JOIN Guardians G ON " +
                                       $"(G.GuardianID = S.GuardianID) " +
                                       $"WHERE G.NationalNumber = '{NationalNumber}'");
            if (GuardianInfo != null)
            {
                if (GuardianInfo.Rows.Count > 0)
                {
                    Res = GuardianInfo.Rows[0]["GuardianID"].ToString();
                }
            }

            return Res;
        }
    }
}