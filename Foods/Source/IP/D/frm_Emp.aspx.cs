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
using System.Security.Cryptography;
using System.Text;
using DataAccess;

namespace Foods
{
    public partial class frm_Emp : System.Web.UI.Page
    {
        DBConnection connection;
        DataTable dt_;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        string query,pass;
        SqlDataAdapter adapter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillGrid();
                level();
            }
        }
        public void FillGrid()
        {
            try
            {
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData("   select * from users  where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

                GVEmp.DataSource = dt_;
                GVEmp.DataBind();
                ViewState["Emp"] = dt_;
            }
            catch (Exception ex)
            {
                //throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }
        }
        protected void TBSearch_TextChanged(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            try
            {
                FillGrid();
                DataTable _dt = (DataTable)ViewState["Emp"];
                DataView dv = new DataView(_dt, "Username LIKE '%" + TBSearch.Text.Trim().ToUpper() + "%'", "[Username] ASC", DataViewRowState.CurrentRows);
                DataTable dt_ = new DataTable();
                dt_ = dv.ToTable();
                GVEmp.DataSource = dt_;
                GVEmp.DataBind();
                TBSearch.Text = "";
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }

        }

        public void level()
        {
            DataTable dt_ = new DataTable();

            dt_.Columns.AddRange(new DataColumn[] { new DataColumn("Name"), new DataColumn("ID") });
            dt_.Clear();


            dt_.Rows.Add("Manager", "1");
            dt_.Rows.Add("Booker", "2");
            dt_.Rows.Add("Sales Man", "3");
            
            
            DDLLvl.DataSource = dt_;
            DDLLvl.DataTextField = "Name";
            DDLLvl.DataValueField = "ID";
            DDLLvl.DataBind();
            DDLLvl.Items.Insert(0, new ListItem("--Select Level--", "0"));
        }

        


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (TBEmpName.Value == "")
                {
                    v_name.Text = "Enter Employe Name";
                    v_des.Text = "";
                    TBEmpName.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showempyee();", true);

                }
                else if (TBdesig.Value == "")
                {
                    v_des.Text = "Enter Designation";
                    v_name.Text = "";
                    TBdesig.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showempyee();", true);
                }
                else if (HFUsrNam.Value == "")
                {
                    v_des.Text = "";
                    v_name.Text = "";

                    int a;
                    a = Save();

                    if (a == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Employee Has Been Saved!";
                        Clear();
                        FillGrid();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Some thing is wrong Call the Administrator!!";
                    }
                }
                else
                {
                    v_des.Text = "";
                    v_name.Text = "";
                    int b;
                    b = Update();

                    if (b == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Employee Has Been Update!";
                        Clear();
                        FillGrid();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Some thing is wrong Call the Administrator!!";
                    }

                }
            }

            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }
        }

        public void Clear()
        {
            HFUsrNam.Value = "";
            TBEmpName.Value = "";
            TBAdd.Value = "";
            TBdesig.Value = "";
            TBphno.Value = "";
            TBfxno.Value = "";
            TBmbno.Value = "";
            TBemal.Value = "";
            chkchgpass.Checked = false;
            DDLLvl.SelectedValue = "0";
            chkaccdisbl.Checked = false;
        }

        private int Save()
        {

            int j = 1;
            string UsrID = "";

            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("EmployeesTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                #region Employee

                pass = Encrypt(TBEmpName.Value.Trim());

                command.CommandText = " INSERT INTO Users (CompanyId ,BranchId ,Username ,Password ,Name " +
                    " ,Address ,Designation ,TelephoneNo ,FaxNo ,MobileNo ,Email,CanChangePassword,Level " +
                    " ,AccountDisable,CreateBy ,CreateTime ,CreateTerminal, CompanyName, Salary)  VALUES " +
                    " ('" + Session["CompanyID"] + "' ,'" + Session["BranchID"] + "', '" + TBEmpName.Value + "'" +
                    " ,'" + pass + "' ,'" + TBEmpName.Value + "','" + TBAdd.Value + "' ,'" + TBdesig.Value + "','" + TBphno.Value + "'" +
                    " ,'" + TBfxno.Value + "','" + TBmbno.Value + "','" + TBemal.Value + "'" +
                    " ,'" + chkchgpass.Checked + "','" + DDLLvl.SelectedValue + "','" + chkaccdisbl.Checked + "'" +
                    " ,'" + Session["user"].ToString() + "','" + DateTime.Now + "','::1'" +
                    " ,'" + Session["Company"] + "','" + TBSal.Text + "')";

                command.ExecuteNonQuery();

                #endregion


                #region Accounts


                /// For Account ID
                /// ...
                /// 

                if (SubHeadCatFiv.Value == "")
                {
                    //ShowAccountCategoryID();

                    command.CommandText = "select SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc";

                    SqlDataAdapter adp = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count < 0)
                    {
                        SubHeadCatFiv.Value = "1";
                    }
                    else
                    {
                        //SqlDataReader reader = cmd.ExecuteReader();

                        //while (reader.Read())
                        // {
                        if (SubHeadCatFiv.Value == "")
                        {
                            int v = Convert.ToInt32(dt.Rows[0]["SubHeadCategoriesID"].ToString());
                            int b = v + 1;
                            //SubHeadCatFiv.Value = "0021" + b.ToString();
                            SubHeadCatFiv.Value = b.ToString();
                        }
                    }
                }

                /// For Account NO..
                /// ...
                /// 

                if (SubHeadCatFivAcc.Value == "")
                {
                    command.CommandText = "select SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc";

                    SqlDataAdapter adp = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();

                    adp.Fill(dt);

                    if (dt.Rows.Count < 0)
                    {
                        SubHeadCatFivAcc.Value = "1";
                    }
                    else
                    {
                        if (SubHeadCatFivAcc.Value == "")
                        {
                            int v = Convert.ToInt32(dt.Rows[0]["SubHeadCategoriesID"].ToString());
                            int b = v + 1;
                            SubHeadCatFivAcc.Value = "0023" + b.ToString();
                        }
                    }
                }

                /// For account Entry..
                /// ...
                /// 

                command.CommandText = " INSERT INTO [dbo].[SubHeadCategories] ([SubHeadCategoriesID] " +
                    " ,[ven_id] ,[SubHeadCategoriesName] ,[SubHeadCategoriesGeneratedID] ,[HeadGeneratedID] " +
                    " ,[SubHeadGeneratedID] ,[CreatedAt] ,[CreatedBy] ,[SubCategoriesKey]) VALUES " +
                    " ('" + SubHeadCatFiv.Value + "' ,0,'" + TBEmpName.Value + "' " +
                    " ,'" + SubHeadCatFivAcc.Value + "', '002' " +
                    " ,'0023' ,'" + DateTime.Now + "','admin' " +
                    " ,'" + SubHeadCatFivAcc.Value + "') ";

                command.ExecuteNonQuery();


                command.CommandText = "select Username from users where Username ='" + TBEmpName.Value + "'";

                SqlDataAdapter adp1 = new SqlDataAdapter(command);

                DataTable dt1 = new DataTable();

                adp1.Fill(dt1);

                if (dt1.Rows.Count > 0)
                {
                    UsrID = dt1.Rows[0]["Username"].ToString();
                }

                /// For adding supplier account no 
                /// ...
                /// 

                command.CommandText = " update users set usr_acc='" + SubHeadCatFivAcc.Value + "' where Username ='" + UsrID + "'";

                command.ExecuteNonQuery();

                #endregion

                // Attempt to commit the transaction.
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
                Clear();
                //Response.Redirect("frm_Sal.aspx");
            }
            //}

            return j;
        }
        //private int Save()
        //{
        //    int j = 1;
        //    pass = Encrypt(TBEmpName.Value.Trim());

        //    query = " INSERT INTO Users (CompanyId ,BranchId ,Username ,Password ,Name " +
        //        " ,Address ,Designation ,TelephoneNo ,FaxNo ,MobileNo ,Email,CanChangePassword,Level " +
        //        " ,AccountDisable,CreateBy ,CreateTime ,CreateTerminal, CompanyName)  VALUES " +
        //        " ('" + Session["CompanyID"] + "' ,'" + Session["BranchID"] + "', '" + TBEmpName.Value + "'" +
        //        " ,'" + pass + "' ,'" + TBEmpName.Value + "','" + TBAdd.Value + "' ,'" + TBdesig.Value + "','" + TBphno.Value + "'" +
        //        " ,'" + TBfxno.Value + "','" + TBmbno.Value + "','" + TBemal.Value + "'" +
        //        " ,'" + chkchgpass.Checked + "','" + DDLLvl.SelectedValue + "','" + chkaccdisbl.Checked + "'" +
        //        " ,'" + Session["user"].ToString() + "','" + DateTime.Now + "','::1'" +
        //        " ,'" + Session["Company"] + "')";

        //    con.Open();


        //    using (SqlCommand cmd = new SqlCommand(query, con))
        //    {
        //        //cmd.Parameters.AddWithValue("@ProductTypeID", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@ProductName", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@PckSize", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@Cost", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@ProductDiscriptions", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@Supplier_Customer", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@Unit", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@ProductType", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@CreatedBy", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@CreatedAt", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@Pro_Code", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@IsActive", textbox2.text);


        //        cmd.ExecuteNonQuery();

        //    }
        //    con.Close();

        //    return j;
        //}

        protected void GVEmp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "AlertDelete();", true);
            lblmodaldelete.Text = "Are you sure you want to Delete !!";
            string id = GVEmp.DataKeys[e.RowIndex].Values[0] != null ? GVEmp.DataKeys[e.RowIndex].Values[0].ToString() : null;
            HFUsrId.Value = id; 
        }


        protected void linkmodaldelete_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlquery = " delete from  Users where Username = '" + HFUsrId.Value + "'";
                SqlCommand cmd = new SqlCommand(sqlquery, con);

                con.Open();
                cmd.ExecuteNonQuery();
                FillGrid();
                con.Close();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }

        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private int Update()
        {

            int j = 1;
            string UsrID = "";

            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("EmployeesUpdateTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                #region Employee

                pass = Encrypt(TBEmpName.Value.Trim());

                command.CommandText = " Update Users set CompanyId='" + Session["CompanyID"] + "', BranchId='" + Session["BranchID"] + "'" +
                            " , Username = '" + TBEmpName.Value + "', Password='" + pass + "', Name='" + TBEmpName.Value + "', Address='" + TBAdd.Value + "'" +
                            " , Designation ='" + TBdesig.Value + "', TelephoneNo='" + TBphno.Value + "', FaxNo = '" + TBfxno.Value + "'" +
                            " , MobileNo='" + TBmbno.Value + "', Email='" + TBemal.Value + "', CanChangePassword='" + chkchgpass.Checked +
                            "', Level='" + DDLLvl.SelectedValue.Trim() + "', AccountDisable='" + chkaccdisbl.Checked + "', CreateBy='" + Session["user"].ToString() + "', CreateTime='" + DateTime.Now +
                            "', CreateTerminal='::1', CompanyName='" + Session["Company"] + "' , Salary='" + TBSal.Text + "' where Username= '" + HFUsrNam.Value.Trim() + "'";

                command.ExecuteNonQuery();

                #endregion


                //#region Accounts


                ///// For Account ID
                ///// ...
                ///// 

                //if (SubHeadCatFiv.Value == "")
                //{
                //    //ShowAccountCategoryID();

                //    command.CommandText = " select top 1 SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc ";

                //    SqlDataAdapter adp = new SqlDataAdapter(command);

                //    DataTable dt = new DataTable();
                //    adp.Fill(dt);

                //    if (dt.Rows.Count < 0)
                //    {
                //        SubHeadCatFiv.Value = "1";

                //    }
                //    else
                //    {
                //        //SqlDataReader reader = cmd.ExecuteReader();

                //        //while (reader.Read())
                //        // {
                //        if (SubHeadCatFiv.Value == "")
                //        {
                //            int v = Convert.ToInt32(dt.Rows[0]["SubHeadCategoriesID"].ToString());
                //            int b = v + 1;
                //            //SubHeadCatFiv.Value = "0021" + b.ToString();
                //            SubHeadCatFiv.Value = b.ToString();
                //        }
                //    }
                //}

                ///// For Account NO..
                ///// ...
                ///// 

                //if (SubHeadCatFivAcc.Value == "")
                //{
                //    command.CommandText = " select top 1 SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc ";

                //    SqlDataAdapter adp = new SqlDataAdapter(command);

                //    DataTable dt = new DataTable();

                //    adp.Fill(dt);

                //    if (dt.Rows.Count < 0)
                //    {
                //        /// For account Entry..
                //        /// ...
                //        /// 

                //        command.CommandText = " UPDATE [dbo].[SubHeadCategories] SET [SubHeadCategoriesName] = '" + TBEmpName.Value + "' WHERE [SubHeadCategoriesGeneratedID] = " + usracc.Value.Trim() + "";

                //        command.ExecuteNonQuery();
                //    }
                //    else
                //    {
                //        if (SubHeadCatFivAcc.Value == "")
                //        {
                //            int v = Convert.ToInt32(dt.Rows[0]["SubHeadCategoriesID"].ToString());
                //            int b = v + 1;
                //            SubHeadCatFivAcc.Value = "0023" + b.ToString();

                //            /// For account Entry..
                //            /// ...
                //            /// 

                //            command.CommandText = " INSERT INTO [dbo].[SubHeadCategories] ([SubHeadCategoriesID] " +
                //                " ,[ven_id] ,[SubHeadCategoriesName] ,[SubHeadCategoriesGeneratedID] ,[HeadGeneratedID] " +
                //                " ,[SubHeadGeneratedID] ,[CreatedAt] ,[CreatedBy] ,[SubCategoriesKey]) VALUES " +
                //                " ('" + SubHeadCatFiv.Value + "' ,0,'" + TBEmpName.Value + "' " +
                //                " ,'" + SubHeadCatFivAcc.Value + "', '002' " +
                //                " ,'0023' ,'" + DateTime.Now + "','admin' " +
                //                " ,'" + SubHeadCatFivAcc.Value + "') ";

                //            command.ExecuteNonQuery();


                //            command.CommandText = "select Username from users where Username ='" + TBEmpName.Value + "'";

                //            SqlDataAdapter adp1 = new SqlDataAdapter(command);

                //            DataTable dt1 = new DataTable();

                //            adp1.Fill(dt1);

                //            if (dt1.Rows.Count > 0)
                //            {
                //                UsrID = dt1.Rows[0]["Username"].ToString();
                //            }

                //            /// For adding supplier account no 
                //            /// ...
                //            /// 

                //            command.CommandText = " update users set usr_acc='" + SubHeadCatFivAcc.Value + "' where Username ='" + UsrID + "'";

                //            command.ExecuteNonQuery();


                //        }
                //    }
                //}


                //#endregion

                // Attempt to commit the transaction.
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
                Clear();
                //Response.Redirect("frm_Sal.aspx");
            }
            //}

            return j;
        }
        //private int Update()
        //{
        //    int u = 1;

        //    pass = Encrypt(TBEmpName.Value.Trim());

        //    query = " Update Users set CompanyId='" + Session["CompanyID"] + "', BranchId='" + Session["BranchID"] + "'" +
        //        " , Username = '" + TBEmpName.Value + "', Password='" + pass + "', Name='" + TBEmpName.Value + "', Address='" + TBAdd.Value + "'" +
        //        " , Designation ='" + TBdesig.Value + "', TelephoneNo='" + TBphno.Value + "', FaxNo = '" + TBfxno.Value + "'" +
        //        " , MobileNo='" + TBmbno.Value + "', Email='" + TBemal.Value + "', CanChangePassword='" + chkchgpass.Checked +
        //        "', Level='" + DDLLvl.SelectedValue.Trim() + "', AccountDisable='" + chkaccdisbl.Checked + "', CreateBy='" + Session["user"].ToString() + "', CreateTime='" + DateTime.Now +
        //        "', CreateTerminal='::1', CompanyName='" + Session["Company"] + "' , Salary='" + TBSal.Text + "' where Username= '" + HFUsrNam.Value.Trim() + "'";


        //    con.Open();


        //    using (SqlCommand cmd = new SqlCommand(query, con))
        //    {
        //        //cmd.Parameters.AddWithValue("@ProductTypeID", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@ProductName", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@PckSize", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@Cost", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@ProductDiscriptions", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@Supplier_Customer", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@Unit", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@ProductType", textbox3.text);
        //        //cmd.Parameters.AddWithValue("@CreatedBy", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@CreatedAt", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@Pro_Code", textbox2.text);
        //        //cmd.Parameters.AddWithValue("@IsActive", textbox2.text);


        //        cmd.ExecuteNonQuery();

        //    }
        //    con.Close();

        //    return u;
        //}

        protected void GVEmp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string CusID = GVEmp.DataKeys[row.RowIndex].Values[0].ToString();
                    HFUsrNam.Value = CusID;

                    dt_ = DBConnection.GetQueryData("select * from Users  where  Username = '" + HFUsrNam.Value.Trim() + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");


                    if (dt_.Rows.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showempyee();", true);

                        TBEmpName.Value = dt_.Rows[0]["Name"].ToString();
                        TBAdd.Value = dt_.Rows[0]["Address"].ToString();
                        TBdesig.Value = dt_.Rows[0]["Designation"].ToString();
                        TBphno.Value = dt_.Rows[0]["TelephoneNo"].ToString();
                        TBfxno.Value = dt_.Rows[0]["FaxNo"].ToString();
                        TBmbno.Value = dt_.Rows[0]["MobileNo"].ToString();
                        TBemal.Value = dt_.Rows[0]["Email"].ToString();
                        chkchgpass.Checked = Convert.ToBoolean(dt_.Rows[0]["CanChangePassword"].ToString());
                        DDLLvl.SelectedValue = dt_.Rows[0]["Level"].ToString();
                        chkaccdisbl.Checked = Convert.ToBoolean(dt_.Rows[0]["AccountDisable"].ToString());
                        usracc.Value = dt_.Rows[0]["usr_acc"].ToString();

                        TBEmpName.Focus();

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Not Record Found!";
                    }

                }
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_Emp.aspx");
        }
    }
}