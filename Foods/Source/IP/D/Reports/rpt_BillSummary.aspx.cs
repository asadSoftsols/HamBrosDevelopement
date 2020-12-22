using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;

using NHibernate;
using NHibernate.Criterion;
using Foods;
using DataAccess;

namespace Foods
{
    public partial class rpt_BillSummary : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string SalmanID, DAT;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {
                SalmanID = Request.QueryString["SALMAN"];
                DAT = Request.QueryString["DAT"];

                FillGrid(SalmanID,DAT);
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        public void FillGrid(string salman, string dat)
        {
            try
            {
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(" select ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_billsummary where SalMan ='" + salman 
                    + "' and MSal_dat='" + dat + "'");

                if (dt_.Rows.Count > 0)
                {
                    lbl_intro.Text = dt_.Rows[0]["SalMan"].ToString();
                    lbldat.Text = dt_.Rows[0]["dsrdat"].ToString();

                    GVCashMemo.DataSource = dt_;
                    GVCashMemo.DataBind();
                }

                decimal GTotal = 0;
                // Total
                for (int j = 0; j < GVCashMemo.Rows.Count; j++)
                {
                    Label total = (Label)GVCashMemo.Rows[j].FindControl("lbl_Amt");
                    GTotal += Convert.ToDecimal(total.Text);
                }

                lbl_ttl.Text = GTotal.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}