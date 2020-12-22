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
    public partial class rptsal_prowise : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string custid, fdat, ldat, monid, yrid, usrid, areaid, proid;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {
                custid = Request.QueryString["CUSTID"];
                fdat = Request.QueryString["FDAT"];
                ldat = Request.QueryString["LDAT"];
                monid = Request.QueryString["MONID"];
                yrid = Request.QueryString["YRID"];
                usrid = Request.QueryString["USRID"];
                areaid = Request.QueryString["AREAID"];
                proid = Request.QueryString["PROID"];

                if (proid != null && fdat != null && ldat != null)
                {
                    get_ptrosal(proid, fdat, ldat);
                }
                else
                {
                    get_ptrosal(proid);
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        protected void LinkBtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                string FileName = "CustomerSalesList.xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);

                GV_CustSale.GridLines = GridLines.Both;
                GV_CustSale.HeaderStyle.Font.Bold = true;

                GV_CustSale.RenderControl(htmltextwrtter);

                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                throw;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lblalert.Text = ex.Message;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        private void get_ptrosal(string Proid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_rptProwisSal where ProductID = " + Proid + "";
                    cmd.Connection = con;
                    con.Open();

                    DataTable dtprosal = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtprosal);
                    if (dtprosal.Rows.Count > 0)
                    {
                        lbl_pro.Text = dtprosal.Rows[0]["ProductName"].ToString();

                        GV_CustSale.DataSource = dtprosal;
                        GV_CustSale.DataBind();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void get_ptrosal(string Proid, string fdat, string ldat)
        {
            try
            {
                dt_ = new DataTable();
                //string str = "SELECT * FROM v_incomexp where accno ='" + acc + "' and expensesdat between '" + fdat + "' and '" + tdat + "'  order by expenceid asc";


                ////dt_ = DBConnection.GetQueryData(" SELECT * FROM v_profloss where accno = " + acc + " and Date between '" + fdat + "' and '" + tdat + "'");
                //dt_ = DBConnection.GetQueryData(str);


                using (var cmd = new SqlCommand("sp_ProwisSal", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PRODUCTID ", Proid.Trim());
                    cmd.Parameters.AddWithValue("@fdat ", fdat.Trim());
                    cmd.Parameters.AddWithValue("@ldat ", ldat.Trim());
                    da.Fill(dt_);
                }

                if (dt_.Rows.Count > 0)
                {
                    lbl_pro.Text = dt_.Rows[0]["ProductName"].ToString();

                    GV_CustSale.DataSource = dt_;
                    GV_CustSale.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}