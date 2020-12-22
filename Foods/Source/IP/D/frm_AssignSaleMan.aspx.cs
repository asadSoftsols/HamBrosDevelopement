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
    public partial class frm_AssignSaleMan : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        string query;
        DataTable dt_;
        SqlDataAdapter adp;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGrid();
                BindDll();
            }
        }
        public void FillGrid()
        {
            GV_Booker.DataSource = "";
            GV_Booker.DataBind();
        }

        public void FillGrid(string booker)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {

                    dt_ = new DataTable();

                    query = query = "select * from tbl_booksalman where bookerid='" + booker + "' and  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = query;

                    cmd.Connection = con;
                    con.Open();

                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                        GV_Booker.DataSource = dt_;
                        GV_Booker.DataBind();
                        HFBook.Value = dt_.Rows[0]["bookerid"].ToString();
                    }
                    else
                    {
                        GV_Booker.DataSource = "";
                        GV_Booker.DataBind();
                        HFBook.Value = "";
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_msg.Text = ex.Message;
                lbl_msg.ForeColor = System.Drawing.Color.Red;
            }

        }

        public void BindDll()
        {
            try
            {
                /// Booker
                /// ...
                /// 
 
                using (SqlCommand cmd = new SqlCommand())
                {

                    dt_ = new DataTable();

                    query = "select  Username,CompanyId,BranchId  from Users where level='2' and  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = query;

                    cmd.Connection = con;
                    con.Open();

                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                      
                        DDL_Booker.DataSource = dt_;
                        DDL_Booker.DataTextField = "Username";
                        DDL_Booker.DataValueField = "Username";
                        DDL_Booker.DataBind();
                        DDL_Booker.Items.Insert(0, new ListItem("--Select Booker--", "0"));

                    }

                    con.Close();
                }

                /// Sales Man
                /// ...
                ///


                using (SqlCommand cmd = new SqlCommand())
                {

                    dt_ = new DataTable();

                    query = "select  Username,CompanyId,BranchId  from Users where level='3' and  CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmd.CommandText = query;

                    cmd.Connection = con;
                    con.Open();

                    adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt_);
                        
                    if (dt_.Rows.Count > 0)
                    {
                        DDL_SalesMan.DataSource = dt_;
                        DDL_SalesMan.DataTextField = "Username";
                        DDL_SalesMan.DataValueField = "Username";
                        DDL_SalesMan.DataBind();
                        DDL_SalesMan.Items.Insert(0, new ListItem("--Select Sales Man--", "0"));
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private int Save()
        {

            int j = 0;

            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("AssignSalesManTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                #region SalesMan



                command.CommandText = " INSERT INTO tbl_booksalman  VALUES  ('" + DDL_Booker.SelectedValue.Trim() + "' ,'" + DDL_SalesMan.SelectedValue.Trim() + "' ,'" + Session["CompanyID"] + "' ,'" + Session["BranchID"] + "' ,'" + DDL_Assign.SelectedValue.Trim() + "')";

                command.ExecuteNonQuery();

                #endregion
              

                // Attempt to commit the transaction.
                transaction.Commit();
                j = 1;

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
                FillGrid(DDL_Booker.SelectedValue.Trim());
                clear();
            }

            return j;
        }


        private int update()
        {

            int j = 0;

            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("AssignSalesManTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                #region SalesMan

                command.CommandText = "select * from tbl_booksalman where bookerid ='" + DDL_Booker.SelectedValue.Trim() + "' and salmanid='" + DDL_SalesMan.SelectedValue.Trim() + "' and CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "'";
                command.CommandType = CommandType.Text;

                adp = new SqlDataAdapter(command);
                dt_ = new DataTable();

                adp.Fill(dt_);

                if (dt_.Rows.Count > 0)
                {
                    command.CommandText = " UPDATE tbl_booksalman SET isAssign='" + DDL_Assign.SelectedValue.Trim() + "' WHERE  bookerid='" + DDL_Booker.SelectedValue.Trim() + "' AND CompanyId='" + Session["CompanyID"] + "' and BranchId='" + Session["BranchID"] + "' and salmanid= '" + DDL_SalesMan.SelectedValue.Trim() + "'";
                }
                else 
                {
                    command.CommandText = " INSERT INTO tbl_booksalman  VALUES  ('" + DDL_Booker.SelectedValue.Trim() + "' ,'" + DDL_SalesMan.SelectedValue.Trim() + "' ,'" + Session["CompanyID"] + "' ,'" + Session["BranchID"] + "' ,'" + DDL_Assign.SelectedValue.Trim() + "')";
                }

                command.ExecuteNonQuery();

                #endregion


                // Attempt to commit the transaction.
                transaction.Commit();
                j = 1;

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
                FillGrid(DDL_Booker.SelectedValue.Trim());
                clear();
            }

            return j;
        }
        protected void btn_SavSalMan_Click(object sender, EventArgs e)
        {
            int i = 0;

            if (HFBook.Value == "")
            {
                i = Save();

                if (i == 1)
                {
                    lbl_msg.Text = "Sales Man has been assigned!";
                    lbl_msg.ForeColor = System.Drawing.Color.Green;                    
                }
                else
                {
                    lbl_msg.Text = "Some Problem has been occured Please Contact Administrator!";
                    lbl_msg.ForeColor = System.Drawing.Color.Red;

                }
            }
            else if (HFBook.Value != "")
            {
                i = update();

                if (i == 1)
                {
                    lbl_msg.Text = "Sales Man has been assigned!";
                    lbl_msg.ForeColor = System.Drawing.Color.Green;
               }
                else
                {
                    lbl_msg.Text = "Some Problem has been occured Please Contact Administrator!";
                    lbl_msg.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        public void clear()
        {
            DDL_Assign.SelectedValue = "";
            DDL_Booker.SelectedValue = "0";
            DDL_SalesMan.SelectedValue = "0";
            HFBook.Value = "";
        }

        protected void DDL_Booker_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillGrid(DDL_Booker.SelectedValue.Trim());
        }
    }
}