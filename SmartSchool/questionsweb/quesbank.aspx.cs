using Common.Base;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SmartSchool.questionsweb
{
    public partial class quesbank : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.AuthenticationType == "Forms" && Context.User.Identity.IsAuthenticated)
                {
                    string UserID = SystemBase.GetCookie((int)clsenumration.UserData.UserID);
                    LblUName.Text = SystemBase.GetUserName(UserID);
                    SetCulture();
                }
                else
                {
                    SystemBase.Logout();
                }
            }

            SetCulture();
        }

        private void SetCulture()
        {
            string CultureName = SystemBase.GetCurrentCultureName("_culture");
            if (String.IsNullOrEmpty(CultureName)) CultureName = "ar";
            SystemBase.SetCulture("_culture", CultureName);
            hdclutrueName.Value = CultureName;
        }
    }
}