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
    public partial class rpt_NetProfit : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DataTable dtexp;
        DBConnection db = new DBConnection();
        string FrmNetDat, ToNetdat, NetMon, NetYr, query;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {
                FrmNetDat = Request.QueryString["FrmNetDat"];
                ToNetdat = Request.QueryString["ToNetdat"];
            
                NetMon = Request.QueryString["NetMon"];
                NetYr = Request.QueryString["NetYr"];
                pnl_dat.Visible = false;
                pnl_monyr.Visible = false;

                lbl_dat.Text = DateTime.Now.ToShortDateString();

                if (FrmNetDat != null && ToNetdat != null)
                {
                    FillDAT(FrmNetDat, ToNetdat);
                    pnl_dat.Visible = true;
                    pnl_monyr.Visible = false;
                    lbl_netproffdat.Text = FrmNetDat;
                    lbl_netproftdat.Text = ToNetdat;
                }
                else if (NetMon != null && NetYr != null)
                {
                    FillMONYR(NetMon,NetYr);
                    pnl_monyr.Visible = true;
                    pnl_dat.Visible = false;
                    lbl_mon.Text = NetMon;

                    lbl_yr.Text = NetYr;
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }

        }
        public void FillDAT(string fdat, string tdat)
        {
            try
            {

                dtexp = new DataTable();
                query = "select accno,acctitle,typeofpay,billno,Amountpaid  from tbl_expenses inner join SubHeadCategories on tbl_expenses.accno " +
                    " = SubHeadCategories.SubHeadCategoriesGeneratedID " +
                    " where SubHeadCategories.SubHeadGeneratedID in('0023','0024') and  " +
                    " expensesdat between '" + fdat + "' and  '" + tdat + "'";


                //dtexp = DBConnection.GetQueryData(query);
                //if (dtexp.Rows.Count > 0)
                //{
                //    GVExpence.DataSource = dtexp;
                //    GVExpence.DataBind();
                //}

                dt_ = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("sp_GetNetProtdattDat", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Fdat", fdat.Trim());
                cmd.Parameters.AddWithValue("@Ldat", tdat.Trim());

                adapter.SelectCommand = cmd;
                adapter.Fill(dt_);

                if (dt_.Rows.Count > 0)
                {
                    GVProf.DataSource = dt_;
                    GVProf.DataBind();
                }


                //For Details

                decimal diff = 0;

                for (int j = 0; j < GVProf.Rows.Count; j++)
                {
                    Label lbl_netprof = (Label)GVProf.Rows[j].FindControl("lbl_netprof");

                    diff += Convert.ToDecimal(lbl_netprof.Text);

                }

                lbl_proff.Text = diff.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillMONYR(string MON, string YR)
        {
            try
            {


                dtexp = new DataTable();

                query = " select accno,acctitle,typeofpay,billno,Amountpaid from tbl_expenses " +
                    " inner join SubHeadCategories on tbl_expenses.accno " +
                    " = SubHeadCategories.SubHeadCategoriesGeneratedID " +
                    " where SubHeadCategories.SubHeadGeneratedID in('0023','0024') and " +
                    " month(expensesdat)='" + MON + "' and year(expensesdat)='" + YR + "'";

                //dtexp = DBConnection.GetQueryData(query);
                //if (dtexp.Rows.Count > 0)
                //{
                //    GVExpence.DataSource = dtexp;
                //    GVExpence.DataBind();
                //}

                dt_ = new DataTable();

                SqlDataAdapter adapter = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand("sp_GetNetProtmonyr", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@mon", MON.Trim());
                cmd.Parameters.AddWithValue("@yr", YR.Trim());

                adapter.SelectCommand = cmd;
                adapter.Fill(dt_);

                if (dt_.Rows.Count > 0)
                {
                    GVProf.DataSource = dt_;
                    GVProf.DataBind();
                }

                //For Details
                
                decimal diff = 0;

                for (int j = 0; j < GVProf.Rows.Count; j++)
                {
                    Label lbl_netprof = (Label)GVProf.Rows[j].FindControl("lbl_netprof");

                    diff += Convert.ToDecimal(lbl_netprof.Text);

                }

                lbl_proff.Text = diff.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}