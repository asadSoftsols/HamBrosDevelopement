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
    public partial class frm_VChq : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        string DSR, DAT, Cust, Usr, DSRID;
        DBConnection db = new DBConnection();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {   
                FillGrid();
            }
        }
        public void FillGrid()
        {
            try
            {
                string query = "";
                dt_ = new DataTable();

                query = " select expenceid, acctitle, expencermk, CashBnk_nam,convert(varchar, ChqDat, 23) as [ChqDat],ChqNO,ChqOK from tbl_expenses where ChqOK = '0' and typeofpay='Cheque' and CompanyId = '" + Session["CompanyID"] + "' and BranchId ='" + Session["BranchID"] + "' order by " +
                       " ChqDat desc";


                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                        GVCheque.DataSource = dt_;
                        GVCheque.DataBind();
                        ViewState["vdsr"] = dt_;
                    }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         protected void SeacrhBtn_Click(object sender, EventArgs e)
        {
            if (TBSDSR.Text == "")
            {
                FillGrid();
            }
            else if (TBSDSR.Text != "")
            {
                SearchRecord();
            }
        }

         private void SearchRecord()
         {
             try
             {
                 string query = "";
                 dt_ = new DataTable();

                 //query = " select distinct(convert(date, cast(dsrdat as date) ,103)) as [dat],  convert(varchar, dsrdat, 23) as [dsrdat], username, Isdon " +
                 //       " from tbl_Mdsr where tbl_Mdsr.dsrdat ='" + TBSDSR.Text.Trim() + "' and tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and tbl_Mdsr.BranchId ='" + Session["BranchID"] + "' order by " +
                 //       " dsrdat desc";
                 query = " select expenceid, acctitle, CashBnk_nam,expencermk, convert(varchar, ChqDat, 23) as [ChqDat],ChqNO,ChqOK from tbl_expenses where acctitle='" + TBSDSR.Text.Trim() + "' OR convert(varchar, ChqDat, 23)='" + TBSDSR.Text.Trim() + "' " +
                        " and ChqOK is null and typeofpay='Cheque' and CompanyId = '"
                        + Session["CompanyID"] + "' and BranchId ='" + Session["BranchID"] + "' order by " +
                          " ChqDat desc";



                 SqlCommand cmd = new SqlCommand(query, con);
                 SqlDataAdapter adp = new SqlDataAdapter(cmd);
                 adp.Fill(dt_);

                 if (dt_.Rows.Count > 0)
                 {
                     GVCheque.DataSource = dt_;
                     GVCheque.DataBind();
                     ViewState["vdsr"] = dt_;
                 }
                 else {
                     FillGrid();
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         protected void GVCheque_PageIndexChanging(object sender, GridViewPageEventArgs e)
         {

         }

         protected void GVCheque_RowCommand(object sender, GridViewCommandEventArgs e)
         {
             try
             {
                 //string id = GVCheque.DataKeys[row.RowIndex].Values[2].ToString();

                 if (e.CommandName == "showdsr")
                 {
                     GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                     string dsrid = GVCheque.DataKeys[row.RowIndex].Values[0].ToString();
                     string Username = GVCheque.DataKeys[row.RowIndex].Values[1].ToString();
          
                     //frm_loadsheet_ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( '.aspx?DSRID=" + id + "&DSRID=" +  + ",'_blank','height=600px,width=600px,scrollbars=1');", true);
                     ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/rpt_dsr.aspx?date=" + dsrid + "&USNAM=" + Username + "','_blank','height=600px,width=600px,scrollbars=1');", true);
                 }
                 else if (e.CommandName == "loadsht")
                 {
                     GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                     string dsrid = GVCheque.DataKeys[row.RowIndex].Values[0].ToString();
          
                     ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'frm_loadsheet_.aspx?LODSHT=" + dsrid + "','_blank','height=600px,width=600px,scrollbars=1');", true);
                 }
                 else if (e.CommandName == "IsDon")
                 {
                     GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                     string expenceid = GVCheque.DataKeys[row.RowIndex].Values[0].ToString();

                     string rmk = ((TextBox)row.FindControl("TBRmks")).Text;
          
                     int upd = 0;

                     upd = Isdone(expenceid, rmk);

                     if (upd == 1)
                     {
                         lb_error.Text = "Your Cheque has been authorized!!";
                         Response.Redirect("frm_VChq.aspx");

                     }else
                     {
                         lb_error.Text = "Oh Ho !! Something is wrong please Contact your adminitrator!!";
                     }
                 }
                 else if (e.CommandName == "NotDon")
                 {
                     GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                     string expenceid = GVCheque.DataKeys[row.RowIndex].Values[0].ToString();

                     string rmks = ((TextBox)row.FindControl("TBRmks")).Text;

                     int upd = 0;

                     upd = IsNotdone(expenceid, rmks);

                     if (upd == 1)
                     {
                         lb_error.Text = "Your Cheque has not been authorized!!";
                         Response.Redirect("frm_VChq.aspx");

                     }
                     else
                     {
                         lb_error.Text = "Oh Ho !! Something is wrong please Contact your adminitrator!!";
                     }
                 }
             }
             catch (Exception ex)
             {
                  throw ex;                 
             }
         }
         private int Isdone(string vdsr, string rmk)
         {
             int j = 1;

             con.Open();

             SqlCommand command = con.CreateCommand();

             SqlTransaction transaction;

             // Start a local transaction.
             transaction = con.BeginTransaction("VerifyChequeTrans");

             // Must assign both transaction object and connection 
             // to Command object for a pending local transaction
             command.Connection = con;
             command.Transaction = transaction;
             try
             {
                command.CommandText = "sp_vchq";
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@expenceid", vdsr.Trim());
                command.Parameters.AddWithValue("@expencermk", rmk.Trim());

                command.ExecuteNonQuery();
                
                transaction.Commit();
 
             }

             catch (Exception ex)
             {
                 Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                 Console.WriteLine("  Message: {0}", ex.Message);

                 // Attempt to roll back the transaction. 
                 try
                 {
                     transaction.Rollback();
                 }
                 catch (Exception ex2)
                 {
                     // This catch block will handle any errors that may have occurred 
                     // on the server that would cause the rollback to fail, such as 
                     // a closed connection.
                     Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                     Console.WriteLine("  Message: {0}", ex2.Message);
                 }
             }
             finally
             {
                 con.Close();
             }

             return j;

         }
         private int Isdones(string vdsr, string rmk)
         {
             int i = 1;

             try
             {
                 string query = "update tbl_expenses set ChqOK = '1', expencermk='" + rmk + "' where expenceid='" + vdsr + "'";


                 con.Open();

                 using (SqlCommand cmd = new SqlCommand(query, con))
                 {

                     cmd.ExecuteNonQuery();

                 }
                 con.Close();

             }
             catch (Exception ex)
             {
                 throw;
             }
             return i;
         }

         private int IsNotdone(string vdsr, string rmk)
         {
             int i = 1;

             try
             {
                 string query = "update tbl_expenses set ChqOK = '0', expencermk='" + rmk + "' where expenceid='" + vdsr + "'";

                 con.Open();

                 using (SqlCommand cmd = new SqlCommand(query, con))
                 {

                     cmd.ExecuteNonQuery();

                 }
                 con.Close();

             }
             catch (Exception ex)
             {
                 throw;
             }
             return i;
         }


         private int _counter;

         protected void GVCheque_RowDataBound(object sender, GridViewRowEventArgs e)
         {
             if (e.Row.RowType == DataControlRowType.DataRow)
             {       
                //dt_ = (DataTable)ViewState["vdsr"];
                //if (dt_.Rows.Count > 0)
                //{
                //    for (int l = 0; l < dt_.Rows.Count; l++)
                //    {
                //        string lock_ = dt_.Rows[l]["Isdon"].ToString();
                //        if (lock_ == "1")
                //        {
                            //for (int i = 0; i < GVCheque.Rows.Count; i++)
                            //{
                            //LinkButton lnkDon = (LinkButton)GVCheque.Rows[i].Cells[0].FindControl("lnkDon"); hflock
                            LinkButton lnkDon = e.Row.FindControl("lnkDon") as LinkButton;
                            HiddenField hflock = e.Row.FindControl("hflock") as HiddenField;
                            if (hflock.Value == "1")
                            {
                                lnkDon.Visible = false;
                            }
                            //}
                            //_counter++;
                //        }
                //    }                        
                //}   
             }
         }
    }
}