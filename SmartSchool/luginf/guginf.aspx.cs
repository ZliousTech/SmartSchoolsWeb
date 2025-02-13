using Common.Base;
using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SmartSchool.luginf
{
    public partial class guginf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    string UserID = SystemBase.GetCookie((int)clsenumration.UserData.UserID);
                    LblUName.Text = SystemBase.GetUserName(UserID);

                    hdSchoolID.Value = SystemBase.GetCookie((int)clsenumration.UserData.SchoolID);

                    SetCulture();
                    SetLookups();
                    GetStudentsSchoolList();
                }
                else
                {
                    SystemBase.Logout();
                }
            }

            SetCulture();
            SetASPElementsTxt();
        }

        private void SetCulture()
        {
            string CultureName = SystemBase.GetCurrentCultureName("_culture");
            if (String.IsNullOrEmpty(CultureName)) CultureName = "ar";
            SystemBase.SetCulture("_culture", CultureName);
            hdclutrueName.Value = CultureName;
        }

        private void SetLookups()
        {

        }

        private void SetASPElementsTxt()
        {
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

        private void GetStudentsSchoolList()
        {
            string StudentName = hdclutrueName.Value.Contains("en") ? "StudentEnglishName" : "StudentArabicName";
            ReptStudentsSchoolList.DataSource =
                SystemBase.GetDataTble($"SELECT S.StudentID, StudentName = S.{StudentName}, S.AddressID " +
                                       $"FROM Student S " +
                                       $"INNER JOIN StudentSchoolDetails SSD ON " +
                                       $"(SSD.StudentID = S.StudentID) " +
                                       $"WHERE SSD.SchoolID = {hdSchoolID.Value}");
            ReptStudentsSchoolList.DataBind();
        }

        protected void OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string StudentID = (e.Item.DataItem as DataRowView)["StudentID"].ToString();
                string AddressID = (e.Item.DataItem as DataRowView)["AddressID"].ToString();

                HtmlTableCell CellLinkItem = e.Item.FindControl("CellLinkItem") as HtmlTableCell;

                //CellLinkItem.Attributes.Add("class", "text-danger");
                CellLinkItem.InnerText = $"https://smartschool.zlioustech.com/LinksForUpdateStudentsLocation/UpdateStudentLocation.aspx?sid=" +
                                         $"{SystemBase.EncryptF(StudentID)}" +
                                         $"&aid={SystemBase.EncryptF(AddressID)}";
            }
        }
    }
}