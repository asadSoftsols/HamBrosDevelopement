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

using Foods;
using DataAccess;

namespace Foods
{
    public partial class rpt_stk : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string src, comp, add, num;
        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {

                //if (!String.IsNullOrEmpty(Session["CompanyImg"].ToString()) && !String.IsNullOrEmpty(Session["CompanyName"].ToString()))
                //{

                //    //Read values from session
                //    src = Session["CompanyImg"].ToString();
                //    comp = Session["CompanyName"].ToString();
                //    //add = Session["CompanyAddress"].ToString();
                //    //num = Session["Companyph"].ToString();
                //}

                int procatid = Convert.ToInt32(Request.QueryString["Protyp"]);
                string proid = Request.QueryString["ProID"];
                string dat = Request.QueryString["dat"];
                //string comp = Request.QueryString["compnam"];
                //string src = Request.QueryString["imglogo_"];
                //string add = Request.QueryString["add"];
                //string num = Request.QueryString["no"];

               
                imglogo.Src = "img/";
                imglogo.Alt = "";
                lbl_compadd.Text = "";
                lbl_no.Text = "";

                if (procatid != 0)
                {
                    FillGrid(procatid, dat);
                    //calttl(procatid, dat);
                }
                else if (proid != null)
                {
                    FillGrid(proid, dat);
                    //calttl(proid, dat);
                }
                else
                {
                    FillGrid(dat);
                    //calttl(dat);
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
                string FileName = "StockList.xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                
                GVStk.GridLines = GridLines.Both;
                GVStk.HeaderStyle.Font.Bold = true;

                GVStk.RenderControl(htmltextwrtter);

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
        public void FillGrid(string dat)
        {
            try
            {
                string query = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from  v_stk  where  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
               
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(query);


                GVStk.DataSource = dt_;
                GVStk.DataBind();


                float GTotal = 0;
                for (int j = 0; j < GVStk.Rows.Count; j++)
                {
                    Label total = (Label)GVStk.Rows[j].FindControl("lbl_stkval");

                    GTotal += Convert.ToSingle(total.Text);
                }
                lbl_ttlStkVal.Text = GTotal.ToString();
                              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void FillGrid(string proid, string dat)
        {
            try
            {

                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from  v_stk  where ProductID ='" + proid + "' and  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

                GVStk.DataSource = dt_;
                GVStk.DataBind();


                float GTotal = 0;
                for (int j = 0; j < GVStk.Rows.Count; j++)
                {
                    Label total = (Label)GVStk.Rows[j].FindControl("lbl_stkval");

                    GTotal += Convert.ToSingle(total.Text);
                }
                lbl_ttlStkVal.Text = GTotal.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void FillGrid(int protypid, string dat)
        {
            try
            {

                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from  v_stk  where ProductTypeID ='" + protypid + "' and  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

                GVStk.DataSource = dt_;
                GVStk.DataBind();

                float GTotal = 0;
                for (int j = 0; j < GVStk.Rows.Count; j++)
                {
                    Label total = (Label)GVStk.Rows[j].FindControl("lbl_stkval");

                    GTotal += Convert.ToSingle(total.Text);
                }
                lbl_ttlStkVal.Text = GTotal.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void calttl(int protypid, string dat)
        {
            try
            {
                string quer = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,sum(Dstk_purrat) as ttl_purrat, sum(Dstk_salrat)  as ttl_salrat,  " +
                    " sum(Dstk_salrat - Dstk_purrat ) as [Prft], " +
                    " sum(Dstk_ItmQty * Dstk_purrat) as [ttlStkVal] from tbl_Dstk " +
                    " inner join Products on tbl_Dstk.ProductID = Products.ProductID " +
                    " inner join tbl_producttype on Products.ProductTypeID = tbl_producttype.ProductTypeID " +
                    " where  Products.ProductTypeID = '" + protypid + "' and tbl_Dstk.CompanyId = '" + Session["CompanyID"] + "' and tbl_Dstk.BranchId= '" + Session["BranchID"] + "'";
                dt_ = new DataTable();

                dt_ = DBConnection.GetQueryData(quer);

                if (dt_.Rows.Count > 0)
                {
                    //lbl_purrat.Text = dt_.Rows[0]["ttl_purrat"].ToString();
                    //lbl_salrat.Text = dt_.Rows[0]["ttl_salrat"].ToString();
                    lbl_ttlStkVal.Text = dt_.Rows[0]["ttlStkVal"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


        private void calttl(string dat)
        { 
            try
            {
                string query = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, sum(Dstk_purrat) as ttl_purrat, sum(Dstk_salrat)  as ttl_salrat, " +
                     " sum(Dstk_salrat - Dstk_purrat ) as [Prft], " +
                     " sum(Dstk_ItmQty * Dstk_purrat) as [ttlStkVal] from tbl_Dstk where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(query);

                if (dt_.Rows.Count > 0)
                {
                    //lbl_purrat.Text = dt_.Rows[0]["ttl_purrat"].ToString();
                    //lbl_salrat.Text = dt_.Rows[0]["ttl_salrat"].ToString();
                    lbl_ttlStkVal.Text = dt_.Rows[0]["ttlStkVal"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void calttl(string proid, string dat)
        {
            try
            {
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData("  SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, sum(Dstk_purrat) as ttl_purrat, sum(Dstk_salrat)  as ttl_salrat,  " +
                    " sum(Dstk_salrat - Dstk_purrat ) as [Prft], " +
                    " sum(Dstk_ItmQty * Dstk_purrat) as [ttlStkVal] from tbl_Dstk " +
                    " inner join Products on tbl_Dstk.ProductID = Products.ProductID " +
                    " inner join tbl_producttype on Products.ProductTypeID = tbl_producttype.ProductTypeID " +
                    " where  tbl_Dstk.ProductID = '" + proid + "' and tbl_Dstk.CompanyId = '" + Session["CompanyID"] + "' and tbl_Dstk.BranchId= '" + Session["BranchID"] + "'");

                if (dt_.Rows.Count > 0)
                {
                    //lbl_purrat.Text = dt_.Rows[0]["ttl_purrat"].ToString();
                    //lbl_salrat.Text = dt_.Rows[0]["ttl_salrat"].ToString();
                    lbl_ttlStkVal.Text = dt_.Rows[0]["ttlStkVal"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //private void calttl(int procatid)
        //{
        //    try
        //    {
        //        dt_ = new DataTable();
        //        dt_ = DBConnection.GetQueryData(" select sum(Dstk_purrat) as ttl_purrat, sum(Dstk_salrat)  as ttl_salrat, " +
        //            " sum(Dstk_salrat - Dstk_purrat ) as [Prft], " +
        //            " sum(Dstk_ItmQty * Dstk_purrat) as [ttlStkVal] from tbl_Dstk where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

        //        if (dt_.Rows.Count > 0)
        //        {
        //            lbl_purrat.Text = dt_.Rows[0]["ttl_purrat"].ToString();
        //            lbl_salrat.Text = dt_.Rows[0]["ttl_salrat"].ToString();
        //            lbl_ttlStkVal.Text = dt_.Rows[0]["ttlStkVal"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

    }    
}