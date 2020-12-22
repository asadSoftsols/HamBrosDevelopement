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
    public partial class rptsale_prowise : System.Web.UI.Page
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

                get_custsal(custid);
            
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

        private void get_custsal(string Custid)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    if (custid != null && fdat != null && ldat != null && monid != null && yrid != null && usrid != null && areaid != null && proid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where CustomerID = '" + Custid + "' and Month = '" + monid + "' and CreatedAt between '" + fdat + "' and '" + ldat + "'  and year(CreatedAt) = '" + yrid + "' and createdby= '" + usrid + "' and areaid = '" + areaid + "' and ProductID ='" + proid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    else if (custid != null && monid != null && yrid != null && usrid != null && areaid != null && proid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where CustomerID = '" + Custid + "' and Month = '" + monid + "' and year(CreatedAt) = '" + yrid + "' and createdby= '" + usrid + "' and areaid = '" + areaid + "' and ProductID ='" + proid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    else if (custid != null && usrid != null && areaid != null && proid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where CustomerID = '" + Custid + "' and createdby= '" + usrid + "' and areaid = '" + areaid + "' and ProductID ='" + proid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";

                    }
                    else if (custid != null && areaid != null && proid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where CustomerID = '" + Custid + "' and areaid = '" + areaid + "' and ProductID ='" + proid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    else if (custid != null && proid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where CustomerID = '" + Custid + "' and ProductID ='" + proid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    else if (usrid != null && areaid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where createdby= '" + usrid + "' and areaid = '" + areaid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";

                    }
                    else if (custid != null && usrid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where CustomerID = '" + Custid + "' and createdby = '" + usrid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    else if (usrid != null && proid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where createdby = '" + usrid + "' and ProductID ='" + proid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    else if (usrid != null && fdat != null && ldat != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where createdby= '" + usrid + "' and CreatedAt between '" + fdat + "' and '" + ldat + "'and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    else if (custid != null)
                    {
                        cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from v_rptSal where CustomerID = '" + Custid + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                    }
                    cmd.Connection = con;
                    con.Open();

                    DataTable dtcustsal = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtcustsal);
                    if (dtcustsal.Rows.Count > 0)
                    {
                        lbl_cust.Text = dtcustsal.Rows[0]["CustomerName"].ToString();
                        lbl_add.Text = dtcustsal.Rows[0]["Address"].ToString();
                        lbl_phno.Text = dtcustsal.Rows[0]["PhoneNo"].ToString();

                        GV_CustSale.DataSource = dtcustsal;
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

    }
}