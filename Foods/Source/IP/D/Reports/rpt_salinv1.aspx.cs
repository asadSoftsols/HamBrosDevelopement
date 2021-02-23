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
    public partial class rpt_salinv1 : System.Web.UI.Page
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
                lbl_dat.Text = DateTime.Now.ToShortDateString();
                lbl_tim.Text = DateTime.Now.ToShortTimeString();
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
                dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* FROM [v_Dsalrecipt1] where MSal_id='" + SalID + "'");

                
                lbl_intro.Text = dt_.Rows[0]["Customer"].ToString();
                //lb_preout.Text = dt_.Rows[0]["Outstanding"].ToString();
                lblbilno.Text = dt_.Rows[0]["Bill"].ToString();
                lblsaldat.Text = dt_.Rows[0]["SalDat"].ToString();
                lblbooker.Text = dt_.Rows[0]["Booker"].ToString();
                lbladd.Text = dt_.Rows[0]["Address"].ToString();

                string custid = dt_.Rows[0]["CustomerID"].ToString();
                

                //For Details

                DataTable dtdetail_ = new DataTable();
                dtdetail_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * FROM [v_saldrecipt1] where MSal_id='" + SalID + "'");
                if (dtdetail_.Rows.Count > 0)
                {
                    GVSal.DataSource = dtdetail_;
                    GVSal.DataBind();

                    decimal GTotal = 0;
                    decimal QTotal = 0;
                    decimal Amt = 0;

                    for (int j = 0; j < GVSal.Rows.Count; j++)
                    {
                        Label total = (Label)GVSal.Rows[j].FindControl("lbl_ttl");
                        GTotal += Convert.ToDecimal(total.Text);
                    }
                    lbl_totl.Text = GTotal.ToString();

                    for (int j = 0; j < GVSal.Rows.Count; j++)
                    {
                        Label total = (Label)GVSal.Rows[j].FindControl("lbl_qty");
                        QTotal += Convert.ToDecimal(total.Text);
                    }

                    lbl_ttlqty.Text = QTotal.ToString();

                    for (int j = 0; j < GVSal.Rows.Count; j++)
                    {
                        Label lbl_amt = (Label)GVSal.Rows[j].FindControl("lbl_amt");
                        Amt += Convert.ToDecimal(lbl_amt.Text);
                    }
                    lbl_ttlgross.Text = Amt.ToString();


                    lbl_dis.Text = dtdetail_.Rows[0]["dis"].ToString();
                    lbl_disamt.Text = (Convert.ToDecimal(dtdetail_.Rows[0]["dis"]) / 100 * Convert.ToDecimal(lbl_ttlgross.Text)).ToString();//dtdetail_.Rows[0]["DisAmt"].ToString();
                    
                    DataTable dtsalcre = new DataTable();

                    dtsalcre = DBConnection.GetQueryData("select CredAmt from tbl_Salcredit where CustomerID='" + custid.Trim() + "'");


                    if (dtsalcre.Rows.Count > 0)
                    {
                        //double recv = Convert.ToDecimal(lblOutstan) - Convert.ToDecimal(TBRecy);
                        lbl_outstan.Text = dtsalcre.Rows[0]["CredAmt"].ToString();
                        //lbl_outstan.Text = dtdetail_.Rows[0]["Outstanding"].ToString();
                    }

                    //lbl_othtax.Text = dt_.Rows[0]["othtax"].ToString();

                   
                    //lblgrssamt.Text = GTotal.ToString();

                    //lbldisper.Text = dtdetail_.Rows[0]["Dis"].ToString();
                    //lbldiscamt.Text = (Convert.ToDecimal(lblgrssamt.Text) * (Convert.ToDecimal(lbldisper.Text) / 100)).ToString();//dtdetail_.Rows[0]["DisAmt"].ToString();
                    //lb_currnetpay.Text = (Convert.ToDecimal(lblgrssamt.Text) - Convert.ToDecimal(lbldiscamt.Text)).ToString();

                    //if (lbl_gst.Text != "" || lbl_gst.Text != "0")
                    //{
                    //    string gst = (Convert.ToDecimal(lb_currnetpay.Text.Trim()) * Convert.ToDecimal(lbl_gst.Text.Trim()) / 100).ToString();
                    //    lb_currnetpay.Text = (Convert.ToDecimal(gst.Trim()) + Convert.ToDecimal(lb_currnetpay.Text.Trim())).ToString();
                    //}

                    //if (lbl_othtax.Text != "" || lbl_othtax.Text != "0")
                    //{
                    //    string othtax = (Convert.ToDecimal(lb_currnetpay.Text.Trim()) * Convert.ToDecimal(lbl_othtax.Text.Trim()) / 100).ToString();
                    //    lb_currnetpay.Text = (Convert.ToDecimal(othtax.Trim()) + Convert.ToDecimal(lb_currnetpay.Text.Trim())).ToString();
                    //}

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}