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
    public partial class rpt_dsr : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db = new DBConnection();
        string id, dat, query, USNAM;
        decimal totalSalary = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            var check = Session["user"];
            id = Request.QueryString["DSRID"];
            dat = Request.QueryString["date"];
            USNAM = Request.QueryString["USNAM"];

            if (check != null)
            {
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
               //Credit Sheets

                DataTable dtcredit_ = new DataTable();

                query = "select  distinct(tbl_Mdsr.CustomerID),tbl_Mdsr.dsrid,Salesman, " +
                    " ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, tbl_mdsr.saleper " +
                    " ,CustomerName as [Party Name],[Address],'' as [Chq],tbl_Mdsr.dsrid as [Bill], isnull(furout,0) as [furout] , " +
                   " sum(CredAmt) as [NetValue],convert(date, cast(dsrdat as date) ,103) as [dsrdat] from tbl_Mdsr " +
                    " inner join tbl_ddsr on tbl_Mdsr.dsrid = tbl_ddsr.dsrid " +
                    " inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.cust_acc " +
                    " inner join tbl_Salcredit on tbl_Mdsr.CustomerID = tbl_Salcredit.CustomerID " +
                    " where tbl_Mdsr.dsrdat = '" + dat + "' and tbl_Mdsr.CreateBy='" + USNAM + "'" +
                    " group by tbl_Mdsr.dsrid, tbl_Mdsr.CustomerID,CustomerName,[Address], dsrdat, tbl_mdsr.saleper,furout,Salesman ";

                dtcredit_ = DBConnection.GetQueryData(query);

                if (dtcredit_.Rows.Count > 0)
                {
                    GVCred.DataSource = dtcredit_;
                
                    //Calculate Sum and display in Footer Row
                    decimal totalSalary = 0;
                    foreach (DataRow dr in dtcredit_.Rows)
                    {
                        totalSalary += Convert.ToDecimal(dr["furout"]);
                    }

                    //for credit:
                    decimal cre = 0;
                    foreach (DataRow dr in dtcredit_.Rows)
                    {
                        cre += Convert.ToDecimal(dr["furout"]);
                    }
                    
                    
                    //--- Here 3 is the number of column where you want to show the total.  
                    GVCred.Columns[5].FooterText = "Total";
                    GVCred.Columns[6].FooterText = totalSalary.ToString();

                    //--- Make sure you bind gridview after writing total into footer.
                    GVCred.DataBind();
                }


                //For Recovery Details

               query = " select  distinct(tbl_Mdsr.CustomerID),tbl_Mdsr.dsrid, " +
                   "  ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, tbl_mdsr.saleper " +
                   " ,CustomerName as [Party Name],[Address],'' as [Chq],tbl_Mdsr.dsrid as [Bill], " +
                   " sum(recvry) as [NetValue],convert(date, cast(dsrdat as date) ,103) as [dsrdat] from tbl_Mdsr " +
                   " inner join tbl_ddsr on tbl_Mdsr.dsrid = tbl_ddsr.dsrid " +
                   " inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.cust_acc " +
                   " inner join tbl_Salcredit on tbl_Mdsr.CustomerID = tbl_Salcredit.CustomerID " +
                   " where tbl_Mdsr.dsrdat = '" + dat + "' and tbl_Mdsr.CreateBy='" + USNAM + "'" +
                   " group by tbl_Mdsr.dsrid, tbl_Mdsr.CustomerID, " +
                   " CustomerName,[Address], dsrdat, tbl_mdsr.saleper";

                DataTable dtrecovy_ = new DataTable();

                dtrecovy_ = DBConnection.GetQueryData(query);

                if (dtrecovy_.Rows.Count > 0)
                {
                    GV_Recov.DataSource = dtrecovy_;
                

                    // for pettycash:
                    decimal pettycash = 0;
                    foreach (DataRow dr in dtrecovy_.Rows)
                    {
                        pettycash += Convert.ToDecimal(dr["NetValue"]);
                    }
                    
                    lbl_ptycash.Text = pettycash.ToString();


                    // for recovery:
                    decimal recov = 0;
                    foreach (DataRow dr in dtrecovy_.Rows)
                    {
                        recov += Convert.ToDecimal(dr["NetValue"]);
                    }

                    lbl_recovery.Text = recov.ToString();


                    //Calculate Sum and display in Footer Row
                    decimal totalSalary = 0;
                    foreach (DataRow dr in dtrecovy_.Rows)
                    {
                        totalSalary += Convert.ToDecimal(dr["NetValue"]);
                    }

                    //--- Here 3 is the number of column where you want to show the total.  
                    GV_Recov.Columns[5].FooterText = "Total";
                    GV_Recov.Columns[6].FooterText = totalSalary.ToString();

                    //--- Make sure you bind gridview after writing total into footer.
                    GV_Recov.DataBind();
                }

                //For diccount

                //query = "select sum(tbl_Mdsr.saleper) as [saleper] from  tbl_Mdsr  inner join tbl_ddsr on tbl_Mdsr.dsrid = tbl_ddsr.dsrid  inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.CustomerID where tbl_Mdsr.dsrdat = '" + dat + "'";
                query = "select left(SUM(saleper)/count(saleper),6) as [saleper] from tbl_mdsr where dsrdat= '" + dat + "' and tbl_Mdsr.CreateBy='" + USNAM + "'";

                dt_ = DBConnection.GetQueryData(query);

                // for discount:
                decimal saleper = 0;
                foreach (DataRow dr in dt_.Rows)
                {
                    saleper += Convert.ToDecimal(dr["saleper"]);
                }

                lbl_discount.Text = saleper.ToString();

                //for DSR Sheet
                query = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, * from  v_rpt_dsr where CreateBy='" + USNAM + "' and Mdsr_dat = '" + dat + "' and  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                
                //query = " select * from  v_rpt_dsr where username='" + USNAM + "' and  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                dt_ = DBConnection.GetQueryData(query);

                if (dt_.Rows.Count > 0)
                {
                    lbl_booker.Text = dt_.Rows[0]["CreateBy"].ToString();
                    lbl_Salman.Text = dt_.Rows[0]["Salesman"].ToString();
                    //lbl_area.Text = dt_.Rows[0]["Area"].ToString();
                    lbl_dat.Text = dt_.Rows[0]["Mdsr_dat"].ToString();
                    //totalSalary = Convert.ToDecimal(dt_.Rows[0]["ttlamt"].ToString());
                    GVdsr.DataSource = dt_;
                    GVdsr.DataBind();


                    //foreach (DataRow dr in dt_.Rows)
                    //{
                    //    totalSalary += Convert.ToDecimal(dr["Amount"]);
                    //}

                    

                    lbl_grosssal.Text = totalSalary.ToString();
                    lbl_grsssal.Text = totalSalary.ToString();

                    //net sales

                    string disc = (Convert.ToDecimal(totalSalary.ToString()) * (Convert.ToDecimal(lbl_discount.Text)) / 100).ToString();
                    lbl_netsal.Text = (Convert.ToDecimal(totalSalary.ToString()) - Convert.ToDecimal(disc.Trim())).ToString();

                    //net sales after recovery
                    string aftrecov = (Convert.ToDecimal(lbl_netsal.Text.Trim()) + Convert.ToDecimal(lbl_recovery.Text.Trim())).ToString();
                    lbl_netsalafter.Text = aftrecov;


                    //sale return
                    decimal salrtren = 0;
                    foreach (DataRow dr in dt_.Rows)
                    {
                        salrtren += Convert.ToDecimal(dr["Return"]);
                    }

                    lbl_salreturn.Text = salrtren.ToString();

                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                string FileName = "DSRList.xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);

                GVdsr.GridLines = GridLines.Both;
                GVdsr.HeaderStyle.Font.Bold = true;

                GVdsr.RenderControl(htmltextwrtter);

                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }


        private void calNetttl()
        {
            try
            {
                string quer = " select sum(recvry) as [recvry],sum(outstan) as [outstan], " +
                    " '' as [PettyCash], sum(saleper) as [saleper], " +
                    " sum(salrturn) as [salrturn],'' as [NetCash] from tbl_ddsr " +
                    " inner join tbl_Mdsr on tbl_ddsr.dsrid  = tbl_Mdsr.dsrid " +
                    " inner join Customers_ on  tbl_Mdsr.CustomerID = Customers_.cust_acc" +
                    "  where tbl_Mdsr.dsrdat = '" + dat + "' and  tbl_Mdsr.CompanyId = '" +
                    Session["CompanyID"] + "' and tbl_Mdsr.BranchId= '" + Session["BranchID"] + "' and tbl_Mdsr.CreateBy='" + USNAM + "'"; 


                dt_ = new DataTable();

                dt_ = DBConnection.GetQueryData(quer);

                if (dt_.Rows.Count > 0)
                {
                    //lbl_grsssal.Text = dt_.Rows[0]["ttlamt"].ToString();
                    lbl_dics.Text = dt_.Rows[0]["saleper"].ToString();
                    lbl_discount.Text = dt_.Rows[0]["saleper"].ToString();
                    lbl_recovery.Text = dt_.Rows[0]["recvry"].ToString();
                    if (lbl_discount.Text == "0" || lbl_discount.Text == "0.00")
                    {
                        lbl_grsssal.Text = (Convert.ToDecimal(totalSalary.ToString())).ToString();
                    }
                    else if (lbl_discount.Text != "0" || lbl_discount.Text != "0.00")
                    {
                        lbl_grsssal.Text = (Convert.ToDecimal(totalSalary.ToString()) - Convert.ToDecimal(lbl_grsssal.Text)).ToString();
                    }

                    lbl_netsal.Text = lbl_grsssal.Text;

                    lbl_netsalafter.Text = (Convert.ToDecimal(lbl_netsal.Text) - Convert.ToDecimal(lbl_recovery.Text)).ToString();
                    lbl_cre.Text = dt_.Rows[0]["outstan"].ToString();
                    lbl_ptycash.Text = dt_.Rows[0]["PettyCash"].ToString();
                    lbl_salreturn.Text = dt_.Rows[0]["salrturn"].ToString();
                    lbl_netcash.Text = dt_.Rows[0]["NetCash"].ToString();

                    
                }   
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }



    }
}