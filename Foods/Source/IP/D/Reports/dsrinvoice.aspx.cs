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
    public partial class dsrinvoice : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string SalID;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {
                SalID = Request.QueryString["SalID"];
                FillGrid();
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

                public void FillGrid()
        {
            try
            {

                dt_ = new DataTable();

                dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,convert(varchar, dsrdat, 23) as [dsrdat],* FROM v_dsrinvoice where dsrid='" + SalID + "'");

                
                lbl_intro.Text = dt_.Rows[0]["Customer"].ToString();
                lb_preout.Text = dt_.Rows[0]["Outstanding"].ToString();
                lblbilno.Text = dt_.Rows[0]["Bill"].ToString();
                lblsaldat.Text = dt_.Rows[0]["dsrdat"].ToString();
                lblbooker.Text = dt_.Rows[0]["CreateBy"].ToString();
                lbl_salman.Text = dt_.Rows[0]["Salesman"].ToString();
                lbladd.Text = dt_.Rows[0]["Address"].ToString();
                lblph.Text = dt_.Rows[0]["PhoneNo"].ToString();
                lbl_disc.Text = dt_.Rows[0]["Dis"].ToString(); 

                if (dt_.Rows.Count > 0)
                {
                    GVSal.DataSource = dt_;
                    GVSal.DataBind();

                    decimal GTotal = 0;

                    for (int j = 0; j < GVSal.Rows.Count; j++)
                    {
                        Label total = (Label)GVSal.Rows[j].FindControl("lbl_GTtl");
                        GTotal += Convert.ToDecimal(total.Text);
                    }

                    lbl_grosamt.Text = GTotal.ToString();// dt_.Rows[0]["GTtl"].ToString();

                    if (lbl_disc.Text != "0")
                    {
                        string disc = (Convert.ToDecimal(GTotal) * (Convert.ToDecimal(lbl_disc.Text) / 100)).ToString();
                        string total = (Convert.ToDecimal(GTotal) - Convert.ToDecimal(disc)).ToString();
                        lbl_net.Text = total;
                        lb_currnetpay.Text = (Convert.ToDecimal(total) - Convert.ToDecimal(lb_preout.Text)).ToString();

                        if (disc != "")
                        {
                            lbl_discamt.Text = disc;
                        }
                        else
                        {
                            lbl_discamt.Text = "0";
                        }
                    }
                    else if (lbl_disc.Text == "0")
                    {
                        lb_currnetpay.Text = GTotal.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}