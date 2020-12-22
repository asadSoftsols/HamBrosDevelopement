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
    public partial class rpt_salinv : System.Web.UI.Page
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
                //dt_ = DBConnection.GetQueryData(" SELECT * FROM [v_salrecipt] where MSal_id='" + SalID + "'");
                dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* FROM [v_Dsalrecipt] where MSal_id='" + SalID + "'");

                
                lbl_intro.Text = dt_.Rows[0]["Customer"].ToString();
                lb_preout.Text = dt_.Rows[0]["Outstanding"].ToString();
                lblbilno.Text = dt_.Rows[0]["Bill"].ToString();
                lblsaldat.Text = dt_.Rows[0]["SalDat"].ToString();
                lblbooker.Text = dt_.Rows[0]["Booker"].ToString();
                lbladd.Text = dt_.Rows[0]["Address"].ToString();
                lblph.Text = dt_.Rows[0]["PhoneNo"].ToString();  

                //For Details

                DataTable dtdetail_ = new DataTable();
                dtdetail_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * FROM [v_saldrecipt] where MSal_id='" + SalID + "'");
                if (dtdetail_.Rows.Count > 0)
                {
                    GVSal.DataSource = dtdetail_;
                    GVSal.DataBind();

                    decimal GTotal = 0;

                    for (int j = 0; j < GVSal.Rows.Count; j++)
                    {
                        Label total = (Label)GVSal.Rows[j].FindControl("lbl_ttl");
                        GTotal += Convert.ToDecimal(total.Text);
                    }
                    lbl_gst.Text = dt_.Rows[0]["gst"].ToString();
                    lbl_othtax.Text = dt_.Rows[0]["othtax"].ToString();

                   
                    lblgrssamt.Text = GTotal.ToString();

                    lbldisper.Text = dtdetail_.Rows[0]["Dis"].ToString();
                    lbldiscamt.Text = (Convert.ToDecimal(lblgrssamt.Text) * (Convert.ToDecimal(lbldisper.Text) / 100)).ToString();//dtdetail_.Rows[0]["DisAmt"].ToString();
                    lb_currnetpay.Text = (Convert.ToDecimal(lblgrssamt.Text) - Convert.ToDecimal(lbldiscamt.Text)).ToString();

                    if (lbl_gst.Text != "" || lbl_gst.Text != "0")
                    {
                        string gst = (Convert.ToDecimal(lb_currnetpay.Text.Trim()) * Convert.ToDecimal(lbl_gst.Text.Trim()) / 100).ToString();
                        lb_currnetpay.Text = (Convert.ToDecimal(gst.Trim()) + Convert.ToDecimal(lb_currnetpay.Text.Trim())).ToString();
                    }

                    if (lbl_othtax.Text != "" || lbl_othtax.Text != "0")
                    {
                        string othtax = (Convert.ToDecimal(lb_currnetpay.Text.Trim()) * Convert.ToDecimal(lbl_othtax.Text.Trim()) / 100).ToString();
                        lb_currnetpay.Text = (Convert.ToDecimal(othtax.Trim()) + Convert.ToDecimal(lb_currnetpay.Text.Trim())).ToString();
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