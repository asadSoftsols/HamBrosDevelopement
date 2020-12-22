﻿using System;
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
    public partial class rpt_purprowise : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string proid, FDAT, LDAT, MONID, YRID;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            if (check != null)
            {
                proid = Request.QueryString["PROID"];
                FDAT = Request.QueryString["FDAT"];
                LDAT = Request.QueryString["LDAT"];
                MONID = Request.QueryString["MONID"];
                YRID = Request.QueryString["YRID"];

                if (proid != null && FDAT != null && LDAT != null)
                {
                    get_ptropur(proid, FDAT, LDAT);

                }
                else if (proid != null && MONID != null && YRID != null)
                {
                    get_propurmonyr(proid, MONID, YRID);
                }
                else
                {
                    get_ptropur(proid);
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
                string FileName = "SaleProductWiseList.xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);

                GV_Propur.GridLines = GridLines.Both;
                GV_Propur.HeaderStyle.Font.Bold = true;

                GV_Propur.RenderControl(htmltextwrtter);

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

        private void get_ptropur(string Proid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_rptSal where ProductID = " + Proid + " and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    cmd.Connection = con;
                    con.Open();

                    DataTable dtprosal = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtprosal);
                    if (dtprosal.Rows.Count > 0)
                    {
                        lbl_pro.Text = dtprosal.Rows[0]["ProductName"].ToString();
                        GV_Propur.DataSource = dtprosal;
                        GV_Propur.DataBind();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void get_ptropur(string Proid, string fdat, string ldat)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_rptSal where ProductID = " + Proid + " and CreatedAt between '" + fdat + "' and '" + ldat + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_DailyPur where ProductID = " + Proid + " and CreatedAt between '" + fdat + "' and '" + ldat + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    cmd.Connection = con;
                    con.Open();

                    DataTable dtprosal = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtprosal);
                    if (dtprosal.Rows.Count > 0)
                    {
                        lbl_pro.Text = dtprosal.Rows[0]["ProductName"].ToString();
                        GV_Propur.DataSource = dtprosal;
                        GV_Propur.DataBind();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void get_propurmonyr(string Proid, string monid, string yrid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,* from v_DailyPur where ProductID = " + Proid + " and Month = '" + monid + "' and Year = '" + yrid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    cmd.Connection = con;
                    con.Open();

                    DataTable dtprosal = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtprosal);
                    if (dtprosal.Rows.Count > 0)
                    {
                        lbl_pro.Text = dtprosal.Rows[0]["ProductName"].ToString();
                        GV_Propur.DataSource = dtprosal;
                        GV_Propur.DataBind();
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}