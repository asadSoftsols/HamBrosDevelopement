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
    public partial class Customers : System.Web.UI.Page
    {
        DBConnection connection;
        DataTable dt_;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        string query, lvl;
        decimal ttlcre;
        string subheadcatid;
        string accno = "";                
        SqlDataAdapter adapter;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                lvl = Session["Level"].ToString();

                if (lvl != "1")
                {
                    pnl_hide.Visible = false;
                }

                FillGrid();
                BindDll();
             
                //GridView1.Visible = false;
                lblcity.Visible = false;
                btnadd.Focus();
            }

        }

        public void FillGrid()
        {
            try
            {
                query = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,customerid, a.CustomerName, a.GST, a.ntn, " +
                    " a.area,   a.refnum, a.district, a.phoneno, a.email, a.cellno1, " +
                    " a.postalcode, a.CellNo2, a.PostalOfficeContact, a.CellNo3,  a.NIC, a.CellNo4, " +
                    " d.CityID, d.city_, a.Address  from Customers_ a  inner join City d on a.city_ = d.city_ where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(query);
                
                GVCutomers.DataSource = dt_;
                GVCutomers.DataBind();
                ViewState["Customer"] = dt_;

            }
            catch (Exception ex)
            {
                //throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }
        }

       

       
        //private int saveHeadFiv()
        //{
        //    int j = 1;
        //    try
        //    {                    
        //        subheadcategoryfive subheadfive = new subheadcategoryfive();

        //        if (HFSubHeadCatFivID.Value == "")
        //        {
        //            Common com = new Common();
        //            com.ShowAccountCategoryFiveID(SubHeadCatFiv);
        //        }
              
        //        subheadfive.subheadcategoryfiveID = HFSubHeadCatFivID.Value;
        //        subheadfive.subheadcategoryfiveName = string.IsNullOrEmpty(TBCustomersName.Value) ? null : TBCustomersName.Value;
        //        subheadfive.subheadcategoryfiveGeneratedID = string.IsNullOrEmpty(SubHeadCatFiv.Value) ? null : SubHeadCatFiv.Value;
        //        subheadfive.HeadGeneratedID = string.IsNullOrEmpty("MB001") ? null : "MB001";
        //        subheadfive.SubHeadGeneratedID = string.IsNullOrEmpty("MB0001") ? null : "MB0001";
        //        subheadfive.SubHeadCategoriesGeneratedID = string.IsNullOrEmpty("MB00002") ? null : "MB00002";
        //        subheadfive.subheadcategoryfourGeneratedID = string.IsNullOrEmpty("MB000003") ? null : "MB000003";
        //        subheadfive.CreatedAt = DateTime.Now;
        //        subheadfive.CreatedBy = Session["user"].ToString();
        //        subheadfive.SubFiveKey = string.IsNullOrEmpty(SubHeadCatFiv.Value) ? null : SubHeadCatFiv.Value;


        //        subheadcategoryfiveManager subheadcatfive = new subheadcategoryfiveManager(subheadfive);
        //        subheadcatfive.Save();


        //        Customers_ customers = new Customers_();

        //        customers.CustomerID = HFCustomerID.Value;
        //        customers.CustomerName = string.IsNullOrEmpty(TBCustomersName.Value) ? null : TBCustomersName.Value;
        //        customers.GST = string.IsNullOrEmpty(TBGST.Value) ? null : TBGST.Value;
        //        customers.category = DDLCategoryID.SelectedItem.Text.ToString() != "0" ? DDLCategoryID.SelectedItem.Text.ToString() : null;
        //        customers.NTN = string.IsNullOrEmpty(TBNTN.Value) ? null : TBNTN.Value;
        //        customers.customertype_ = DDLCustomerType.SelectedItem.Text.ToString() != "0" ? DDLCustomerType.SelectedItem.Text.ToString() : null;
        //        customers.Area = string.IsNullOrEmpty(DDLArea.SelectedValue) ? null : DDLArea.SelectedValue;
        //        customers.RefNum = string.IsNullOrEmpty(TBRefNum.Value) ? null : TBRefNum.Value;
        //        customers.District = string.IsNullOrEmpty(TBDistrict.Value) ? null : TBDistrict.Value;
        //        customers.PhoneNo = string.IsNullOrEmpty(TBPhone.Value) ? null : TBPhone.Value;
        //        customers.Email = string.IsNullOrEmpty(TBEmail.Value) ? null : TBEmail.Value;
        //        customers.CellNo1 = string.IsNullOrEmpty(TBCellNo.Value) ? null : TBCellNo.Value;
        //        customers.PostalCode = string.IsNullOrEmpty(TBPostalCode.Value) ? null : TBPostalCode.Value;
        //        customers.CellNo2 = string.IsNullOrEmpty(TBCellNo2.Value) ? null : TBCellNo2.Value;
        //        customers.PostalOfficeContact = string.IsNullOrEmpty(TBPostalOfficeContact.Value) ? null : TBPostalOfficeContact.Value;
        //        customers.CellNo3 = string.IsNullOrEmpty(TBCellNo3.Value) ? null : TBCellNo3.Value;
        //        customers.NIC = string.IsNullOrEmpty(TBNIC.Value) ? null : TBNIC.Value;
        //        customers.CellNo4 = string.IsNullOrEmpty(TBCellNo4.Value) ? null : TBCellNo4.Value;
        //        customers.city_ = DDLCityID.SelectedItem.Text.ToString() != "0" ? DDLCityID.SelectedItem.Text.ToString() : null;
        //        customers.Address = string.IsNullOrEmpty(TBAddress.Value) ? null : TBAddress.Value;
        //        customers.CreatedBy = Session["user"].ToString();
        //        customers.CreatedDate = DateTime.Now;
        //        customers.IsActive = "true";
        //        customers.Cus_Code = SubHeadCatFiv.Value.Trim();

        //        CustomerManager custmanag = new CustomerManager(customers);
        //        custmanag.Save();
        //    }
        //    catch (Exception ex)
        //    {
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
        //        lblalert.Text = ex.Message;

        //    }
        //    return j;
        //}

        private int Save()
        {

            int j = 1;
            string CustomerID = "";

            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("CustomersTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                #region Customers

                command.CommandText = " INSERT INTO [dbo].[Customers_] ([CustomerName] ,[GST] ,[category] ,[NTN] ,[customertype_],[areaid] " +
                                " ,[Area] ,[saleper] ,[District] ,[PhoneNo] ,[Email],[CellNo1],[PostalCode],[CellNo2] " +
                                " ,[PostalOfficeContact],[CellNo3] ,[NIC] ,[CellNo4],[city_],[Address] ,[CreatedBy] " +
                                " ,[CreatedDate] ,[IsActive] ,[Cus_Code] ,[CompanyId] ,[BranchId])  VALUES " +
                                " ('" + TBCustomersName.Value + "' ,'" + TBGST.Value + "','', '" + TBNTN.Value + "'" +
                                " ,'' ,'" + DDLArea.SelectedValue.Trim() + "','" + DDLArea.SelectedItem.Text + "','" + TBSalPer.Value + "' ,'" + TBDistrict.Value + "','" + TBPhone.Value + "'" +
                                " ,'" + TBEmail.Value + "','" + TBCellNo.Value + "','" + TBPostalCode.Value + "'" +
                                " ,'" + TBCellNo2.Value + "','" + TBPostalOfficeContact.Value + "','" + TBCellNo3.Value + "'" +
                                " ,'" + TBNIC.Value + "','" + TBCellNo4.Value + "','" + DDLCityID.SelectedItem.Text.ToString() + "'" +
                                " ,'" + TBAddress.Value + "','" + Session["user"].ToString() + "','" + DateTime.Now + "'" +
                                " ,'true' ,'','" + Session["CompanyID"] + "','" + Session["BranchID"] + "' )";

                command.ExecuteNonQuery();

                #endregion


                #region Accounts

                /// For Account ID
                /// ...
                /// 

                if (SubHeadCatFiv.Value == "")
                {

                    command.CommandText = "select top 1 SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc";

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
                    command.CommandText = "select top 1 SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc";

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
                            SubHeadCatFivAcc.Value = "0011" + b.ToString();
                        }
                    }
                }

                /// For account Entry..
                /// ...
                /// 

                command.CommandText = " INSERT INTO [dbo].[SubHeadCategories] ([SubHeadCategoriesID] " +
                    " ,[ven_id] ,[SubHeadCategoriesName] ,[SubHeadCategoriesGeneratedID] ,[HeadGeneratedID] " +
                    " ,[SubHeadGeneratedID] ,[CreatedAt] ,[CreatedBy] ,[SubCategoriesKey]) VALUES " +
                    " ('" + SubHeadCatFiv.Value + "' ,0,'" + TBCustomersName.Value + "' " +
                    " ,'" + SubHeadCatFivAcc.Value + "', '001' " +
                    " ,'0011' ,'" + DateTime.Now + "','admin' " +
                    " ,'" + SubHeadCatFivAcc.Value + "') ";

                command.ExecuteNonQuery();


                command.CommandText = "select top 1 CustomerID from Customers_ order by CustomerID desc";

                SqlDataAdapter adp1 = new SqlDataAdapter(command);

                DataTable dt1 = new DataTable();

                adp1.Fill(dt1);

                if (dt1.Rows.Count > 0)
                {
                    CustomerID = dt1.Rows[0]["CustomerID"].ToString();
                }

                /// For adding customer account no 
                /// ...
                /// 

                command.CommandText = " update Customers_ set cust_acc='" + SubHeadCatFivAcc.Value + "' where CustomerID ='" + CustomerID + "'";

                command.ExecuteNonQuery();


                #endregion


                #region Credit Sheets

                command.CommandText = " select SubHeadCategoriesGeneratedID,SubHeadCategoriesName from SubHeadCategories where SubHeadCategoriesName='" + TBCustomersName.Value.Trim() + "'";
                SqlDataAdapter adpchkcust = new SqlDataAdapter(command);

                DataTable dtchkcust = new DataTable();
                adpchkcust.Fill(dtchkcust);

                if (dtchkcust.Rows.Count > 0)
                {
                    accno = dtchkcust.Rows[0]["SubHeadCategoriesGeneratedID"].ToString();
                }

                command.CommandText = "select CredAmt from tbl_Salcredit where CustomerID='" + accno.Trim() + "'";

                SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                DataTable dtsalcre = new DataTable();
                stksalcre.Fill(dtsalcre);

                if (dtsalcre.Rows.Count > 0)
                {
                    //double recv = Convert.ToDouble(lblOutstan) - Convert.ToDouble(TBRecy);

                    //avapre = Convert.ToDecimal(dtsalcre.Rows[0]["CredAmt"]);

                    ttlcre = Convert.ToDecimal(TBPrevBal.Value.Trim());

                    command.CommandText = " Update tbl_Salcredit set CredAmt = '" + ttlcre + "' where CustomerID='" + accno.Trim() + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    //command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + DDL_CustAcc.SelectedValue.Trim() + "','" + ttlcre + "')";
                    command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + accno.Trim() + "','" + TBPrevBal.Value.Trim() + "')";
                    command.ExecuteNonQuery();
                }

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


        //    query = " INSERT INTO [dbo].[Customers_] ([CustomerName] ,[GST] ,[category] ,[NTN] ,[customertype_],[areaid] " +
        //            " ,[Area] ,[saleper] ,[District] ,[PhoneNo] ,[Email],[CellNo1],[PostalCode],[CellNo2] " +
        //            " ,[PostalOfficeContact],[CellNo3] ,[NIC] ,[CellNo4],[city_],[Address] ,[CreatedBy] " +
        //            " ,[CreatedDate] ,[IsActive] ,[Cus_Code] ,[CompanyId] ,[BranchId])  VALUES " +
        //            " ('" + TBCustomersName.Value + "' ,'" + TBGST.Value +"','', '" + TBNTN.Value + "'" +
        //            " ,'' ,'" + DDLArea.SelectedValue.Trim() + "','" + DDLArea.SelectedItem.Text + "','" + TBSalPer.Value + "' ,'" + TBDistrict.Value + "','" + TBPhone.Value + "'" +
        //            " ,'" + TBEmail.Value + "','" + TBCellNo.Value + "','" + TBPostalCode.Value + "'" +
        //            " ,'" + TBCellNo2.Value + "','"+ TBPostalOfficeContact.Value + "','" +TBCellNo3.Value + "'" +
        //            " ,'" + TBNIC.Value + "','" +TBCellNo4.Value +"','" + DDLCityID.SelectedItem.Text.ToString() + "'" +
        //            " ,'" + TBAddress.Value + "','" + Session["user"].ToString() + "','" + DateTime.Now+ "'" +
        //            " ,'true' ,'','" + Session["CompanyID"] + "','" + Session["BranchID"] + "' )";

        //        con.Open();


        //        using (SqlCommand cmd = new SqlCommand(query, con))
        //        {
        //            //cmd.Parameters.AddWithValue("@ProductTypeID", textbox2.text);
        //            //cmd.Parameters.AddWithValue("@ProductName", textbox3.text);
        //            //cmd.Parameters.AddWithValue("@PckSize", textbox2.text);
        //            //cmd.Parameters.AddWithValue("@Cost", textbox3.text);
        //            //cmd.Parameters.AddWithValue("@ProductDiscriptions", textbox2.text);
        //            //cmd.Parameters.AddWithValue("@Supplier_Customer", textbox3.text);
        //            //cmd.Parameters.AddWithValue("@Unit", textbox2.text);
        //            //cmd.Parameters.AddWithValue("@ProductType", textbox3.text);
        //            //cmd.Parameters.AddWithValue("@CreatedBy", textbox2.text);
        //            //cmd.Parameters.AddWithValue("@CreatedAt", textbox2.text);
        //            //cmd.Parameters.AddWithValue("@Pro_Code", textbox2.text);
        //            //cmd.Parameters.AddWithValue("@IsActive", textbox2.text);


        //            cmd.ExecuteNonQuery();

        //        }
        //        con.Close();

        //    return j;
        //}
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {

/*                bool phn = false;
                bool email = false;
                Regex regexobjphn = new Regex(@"^([0-9]*|\d*\.\d{1}?\d*)$");
                Regex regexobjemail = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");


                if (TBCustomersName.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write Customer Name!!";
                }

                else if (TBPhone.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write Phone Num!!";
                }

                else if (TBEmail.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write Email Add!!";
                }
                else if (TBCellNo.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write Cell No!!";
                }
                else if (TBNIC.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write NIC!!";
                }
                else if (TBAddress.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write Customer Address!!";
                }
                else if (!regexobjphn.IsMatch(TBPhone.Value))
                {
                    phn = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write Correct Phone Number!!";
                }
                else if (!regexobjemail.IsMatch(TBEmail.Value))
                {
                    email = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Please Write Correct Email Address!!";
                }
                else*/
                if (TBCustomersName.Value == "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
                    v_name.Text = "Enter Customer Name";
                    TBCustomersName.Focus();
                    

                }
                else if (HFCustomerID.Value == "")
                {
                    v_name.Text = "";

                    int a;
                    a = Save();

                    if (a == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Customers Has Been Saved!";
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
                    v_name.Text = "";
                    int b;
                    b= Update();

                    if (b == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Customers Has Been Update!";
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


        private int Update()
        {
            int h;

            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("UpdateTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                /// For Area if Required
                /// ...
                ///

                #region Area 
                    //if (TBArea.Text != "")
                    //{
                    //    query = "select * from tbl_area  where area_= '" + TBArea.Text.Trim() + "'";


                    //    dt_ = DBConnection.GetQueryData(query);

                    //    if (dt_.Rows.Count > 0)
                    //    {
                    //        areaid = dt_.Rows[0]["areaid"].ToString();
                    //    }
                    //}
                    //else
                    //{
                    //    areaid = "0";
                    //}
                #endregion

                command.CommandText = " Update Customers_ set CustomerName='" + TBCustomersName.Value + "', GST=''" +
                    " , category = '', NTN='" + TBNTN.Value + "', customertype_= '',areaid='" + DDLArea.SelectedValue.Trim() + "', Area='" + DDLArea.SelectedItem.Text.Trim() + "'" +
                    " , saleper ='"+ TBSalPer.Value.Trim() + "', District='', PhoneNo = '" + TBPhone.Value + "'" +
                    " , Email='" + TBEmail.Value + "', CellNo1='', PostalCode='', CellNo2='', " +
                    "  PostalOfficeContact='', CellNo3='', NIC='" + TBNIC.Value + "'" +
                    " , CellNo4='', city_='" + DDLCityID.SelectedItem.Text.Trim() + "', Address='" + TBAddress.Value + "', CreatedBy='" + 
                    Session["user"].ToString() + "', CreatedDate='" + DateTime.Now + "', IsActive='true', Cus_Code = '', CompanyId='" 
                    + Session["CompanyID"] + "'" + " , BranchId= '" + Session["BranchID"] + "' where CustomerID= '" 
                    + HFCustomerID.Value.Trim() + "'";

                command.ExecuteNonQuery();


                // For Account

                #region Accounts


                /// For Account ID
                /// ...
                /// 

                if (SubHeadCatFiv.Value == "")
                {
                    command.CommandText = "select top 1 SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc";

                    SqlDataAdapter adp = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        if (SubHeadCatFivAcc.Value == "")
                        {
                            int v = Convert.ToInt32(dt.Rows[0]["SubHeadCategoriesID"].ToString());
                            int b = v + 1;
                            SubHeadCatFivAcc.Value = "0011" + b.ToString();

                            /// For account Entry..
                            /// ...
                            /// 

                            command.CommandText = " INSERT INTO [dbo].[SubHeadCategories] ([SubHeadCategoriesID] " +
                                " ,[ven_id] ,[SubHeadCategoriesName] ,[SubHeadCategoriesGeneratedID] ,[HeadGeneratedID] " +
                                " ,[SubHeadGeneratedID] ,[CreatedAt] ,[CreatedBy] ,[SubCategoriesKey]) VALUES " +
                                " ('" + b + "' ,0,'" + TBCustomersName.Value + "' " +
                                " ,'" + SubHeadCatFivAcc.Value + "', '001' " +
                                " ,'0011' ,'" + DateTime.Now + "','admin' " +
                                " ,'" + SubHeadCatFivAcc.Value + "') ";

                            command.ExecuteNonQuery();

                        }
                    }
                }


                /// For adding customer account no 
                /// ...
                /// 

                command.CommandText = "select * from SubHeadCategories  where SubHeadCategoriesGeneratedID= '" + HFAccountNO.Value.Trim() + "'";
                adapter = new SqlDataAdapter(command);

                DataTable dt_ = new DataTable();
                adapter.Fill(dt_);

                if(dt_.Rows.Count > 0)
                {
                    SubHeadCatFivAcc.Value = dt_.Rows[0]["SubHeadCategoriesGeneratedID"].ToString();
                }   
                
                //Update Customer table for Account NO..

                command.CommandText = " update Customers_ set cust_acc='" + SubHeadCatFivAcc.Value + "' where CustomerID ='" + HFCustomerID.Value.Trim() + "'";

                command.ExecuteNonQuery();

                //Update SubhaeadCategories table for Account NO..

                command.CommandText = " update SubHeadCategories set SubHeadCategoriesName='" + TBCustomersName.Value.Trim() +
                    "' where SubHeadCategoriesGeneratedID='" + SubHeadCatFivAcc.Value + "'";

                command.ExecuteNonQuery();


                #endregion

                #region Credit Sheets


                command.CommandText = "select CredAmt from tbl_Salcredit where CustomerID='" + SubHeadCatFivAcc.Value.Trim() + "'";

                SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                DataTable dtsalcre = new DataTable();
                stksalcre.Fill(dtsalcre);

                if (dtsalcre.Rows.Count > 0)
                {

                    ttlcre = Convert.ToDecimal(TBPrevBal.Value.Trim());

                    command.CommandText = " Update tbl_Salcredit set CredAmt = '" + ttlcre + "' where CustomerID='" + SubHeadCatFivAcc.Value.Trim() + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    //command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + DDL_CustAcc.SelectedValue.Trim() + "','" + ttlcre + "')";
                    command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + SubHeadCatFivAcc.Value.Trim() + "','" + TBPrevBal.Value.Trim() + "')";
                    command.ExecuteNonQuery();
                }

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
                h = 1;
            }
            return h;

        }



        //private int Update()
        //{
        //    int u = 1;

        //    query = " Update Customers_ set CustomerName='" + TBCustomersName.Value + "', GST='" + TBGST.Value + "'" +
        //        " , category = '', NTN='" + TBNTN.Value + "', customertype_= '',areaid='" + DDLArea.SelectedValue + "', Area='" + DDLArea.SelectedItem.Text + "'" +
        //        " , saleper ='" + TBSalPer.Value + "', District='" + TBDistrict.Value + "', PhoneNo = '" + TBPhone.Value + "'" +
        //        " , Email='" + TBEmail.Value + "', CellNo1='" + TBCellNo.Value + "', PostalCode='" + TBPostalCode.Value + "', CellNo2='" + TBCellNo2.Value + "', PostalOfficeContact='" + TBPostalOfficeContact.Value + "', CellNo3='" + TBCellNo3.Value + "', NIC='" + TBNIC.Value + "'" +
        //        " , CellNo4='" + TBCellNo4.Value + "', city_='" + DDLCityID.SelectedItem.Text.ToString() + "', Address='" + TBAddress.Value + "', CreatedBy='" + Session["user"].ToString() + "', CreatedDate='" + DateTime.Now + "', IsActive='true', Cus_Code = '', CompanyId='" + Session["CompanyID"] + "'" +
        //        " , BranchId= '" + Session["BranchID"] + "' where CustomerID= '" + HFCustomerID.Value.Trim() + "'";

            
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
                string FileName = "CustomersList.xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                GVCutomers.GridLines = GridLines.Both;
                GVCutomers.HeaderStyle.Font.Bold = true;

                GVCutomers.RenderControl(htmltextwrtter);

                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }

        protected void LinkBtnUpload_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    lblMsg.Text = "";

            //    if (FileUploadToServer.HasFile)
            //    {
            //        System.Threading.Thread.Sleep(10000);

            //        // ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", " progressbar();", true);

            //        string FileName = Path.GetFileName(FileUploadToServer.PostedFile.FileName);
            //        string Extension = Path.GetExtension(FileUploadToServer.PostedFile.FileName);

            //        string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

            //        string FilePath = Server.MapPath(FolderPath + FileName);

            //        if (Extension == ".xlsx")
            //        {
            //            lblMsg.Text = "Uploading:";
            //            FileUploadToServer.SaveAs(FilePath);
            //            Import_To_Grid(FilePath, Extension, "1");
            //        }
            //        else
            //        {
            //            lblMsg.Text = "Please select .xlsx or Excel File!!";
            //        }
            //    }
            //    else
            //    {
            //        lblMsg.Text = "Please select some thing to upload!!";
            //    }
            //}
            //catch (Exception ex)
            //{
            //       //throw;
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
            //        lblalert.Text = ex.Message;
            //}
        }

        //public void Import_To_Grid(string FilePath, string Extension, string Step)
        //{
            //try
            //{
            //    string conStr = "";
            //    switch (Extension)
            //    {
            //        case ".xls": //Excel 97-03
            //            conStr = "Provider=Microsoft.ACE.OLEDB.8.0;Data Source=" + FilePath + ";Extended Properties=Excel 8.0 ";
            //            break;
            //        case ".xlsx": //Excel 07
            //            conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FilePath + ";Extended Properties=Excel 12.0 ";
            //            break;
            //    }
            //    conStr = String.Format(conStr, FilePath, 1);
            //    OleDbConnection connExcel = new OleDbConnection(conStr);
            //    OleDbCommand cmdExcel = new OleDbCommand();
            //    OleDbDataAdapter oda = new OleDbDataAdapter();
            //    DataTable dt = new DataTable();
            //    cmdExcel.Connection = connExcel;
            //    connExcel.Open();
            //    DataTable dtExcelSchema;
            //    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //    string SheetName = null;
            //    if (Step == "1")
            //    {
            //        SheetName = "CustomersList$";
            //    }
            //    connExcel.Close();

            //    //Read Data from First Sheet
            //    connExcel.Open();
            //    cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
            //    oda.SelectCommand = cmdExcel;
            //    oda.Fill(dt);

            //    GridView1.DataSource = dt;
            //    GridView1.DataBind();

            //    connExcel.Close();
            //    LoadData(dt);


            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
        //}

        private void LoadData(DataTable dt_)
        {
            try
            {
                for (int i = 0; i < dt_.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt_.Rows[i]["ID"].ToString()))
                    {
                        Customers_ customers = new Customers_();

                        customers.CustomerID = dt_.Rows[i]["ID"].ToString();
                        customers.CustomerName = dt_.Rows[i]["Customer Name"].ToString();
                        customers.GST = dt_.Rows[i]["GST"].ToString();
                        customers.category = dt_.Rows[i]["Category"].ToString();
                        customers.NTN = dt_.Rows[i]["NTN"].ToString();
                        customers.customertype_ = dt_.Rows[i]["Type"].ToString();
                        customers.Area = dt_.Rows[i]["Area"].ToString();
                        customers.RefNum = dt_.Rows[i]["Ref #"].ToString();
                        customers.District = dt_.Rows[i]["District"].ToString();
                        customers.PhoneNo = dt_.Rows[i]["Phone No"].ToString();
                        customers.Email = dt_.Rows[i]["Email"].ToString();
                        customers.CellNo1 = dt_.Rows[i]["Cell No1"].ToString();
                        customers.PostalCode = dt_.Rows[i]["Postal Code"].ToString();
                        customers.CellNo2 = dt_.Rows[i]["Cell No2"].ToString();
                        customers.PostalOfficeContact = dt_.Rows[i]["Postal Office Contact"].ToString();
                        customers.CellNo3 = dt_.Rows[i]["Cell No3"].ToString();
                        customers.NIC = dt_.Rows[i]["NIC"].ToString();
                        customers.CellNo4 = dt_.Rows[i]["Cell No4"].ToString();
                        customers.city_= dt_.Rows[i]["City"].ToString();
                        customers.Address = dt_.Rows[i]["Address"].ToString();
                        customers.CreatedBy = Session["user"].ToString();
                        customers.CreatedDate = DateTime.Now;
                        customers.IsActive = "true";

                        CustomerManager custmanag = new CustomerManager(customers);
                        custmanag.Save();
                        Clear();
                        FillGrid();

                    }
                }
            }
            catch (Exception ex)
            {
                   throw;
               // ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lblalert.Text = ex.Message;
            }
        }

        public void Clear()
        {
            HFCustomerID.Value = "";
            TBCustomersName.Value = "";
            TBGST.Value = "";
            TBNTN.Value = "";
            DDLArea.SelectedValue = "0";
            TBSalPer.Value = "";
            TBDistrict.Value = "";
            TBPhone.Value = "";
            TBEmail.Value = "";
            TBCellNo.Value = "";
            TBPostalCode.Value = "";
            TBCellNo2.Value = "";
            TBPostalOfficeContact.Value = "";
            TBCellNo3.Value = "";
            TBNIC.Value = "";
            TBCellNo4.Value = "";
            TBAddress.Value = "";
            HFSubHeadCatFivID.Value = "";
            SubHeadCatFiv.Value = "";
            SubHeadCatFivAcc.Value = "";
            TBPrevBal.Value = "";
        }

        protected void GVCutomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string CusID = GVCutomers.DataKeys[row.RowIndex].Values[0].ToString();
                    HFCustomerID.Value = CusID;

                    dt_ = DBConnection.GetQueryData("select * from Customers_  where  CustomerID = '" + HFCustomerID.Value.Trim() + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
                                       

                    if (dt_.Rows.Count > 0)
                    {
                        TBCustomersName.Value = dt_.Rows[0]["CustomerName"].ToString();
                        TBGST.Value = dt_.Rows[0]["GST"].ToString();
                        DDLCategoryID.SelectedItem.Text = dt_.Rows[0]["category"].ToString();
                        TBNTN.Value = dt_.Rows[0]["NTN"].ToString();                     
                        DDLCustomerType.SelectedItem.Text = dt_.Rows[0]["customertype_"].ToString();
                        DDLArea.SelectedValue = dt_.Rows[0]["areaid"].ToString();
                        DDLArea.SelectedItem.Text = dt_.Rows[0]["Area"].ToString();
                        TBSalPer.Value = dt_.Rows[0]["saleper"].ToString();
                        TBDistrict.Value = dt_.Rows[0]["District"].ToString();
                        TBPhone.Value = dt_.Rows[0]["PhoneNo"].ToString();
                        TBEmail.Value = dt_.Rows[0]["Email"].ToString();
                        TBCellNo.Value = dt_.Rows[0]["CellNo1"].ToString();
                        TBCellNo2.Value = dt_.Rows[0]["CellNo2"].ToString();
                        TBCellNo3.Value = dt_.Rows[0]["CellNo3"].ToString();
                        TBCellNo4.Value = dt_.Rows[0]["CellNo4"].ToString();
                        TBAddress.Value = dt_.Rows[0]["Address"].ToString();
                        TBPostalCode.Value = dt_.Rows[0]["PostalCode"].ToString();
                        TBPostalOfficeContact.Value = dt_.Rows[0]["PostalOfficeContact"].ToString();
                        TBNIC.Value = dt_.Rows[0]["NIC"].ToString();
                        DDLCityID.SelectedItem.Text = dt_.Rows[0]["city_"].ToString();
                        HFAccountNO.Value = dt_.Rows[0]["cust_acc"].ToString();

                        DataTable dtsalcre = new DataTable();

                        dtsalcre = DBConnection.GetQueryData("select CredAmt from tbl_Salcredit where CustomerID='" + HFAccountNO.Value.Trim() + "'");

                  
                        if (dtsalcre.Rows.Count > 0)
                        {
                            TBPrevBal.Value = dtsalcre.Rows[0]["CredAmt"].ToString();
                        }


                        TBCustomersName.Focus();

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Not Record Found!";
                    }

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);

                    //GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    //HFCustomerID.Value = GVCutomers.DataKeys[row.RowIndex].Values[0].ToString();
                    //TBCustomersName.Value = Server.HtmlDecode(row.Cells[1].Text);
                    //TBGST.Value = Server.HtmlDecode(row.Cells[2].Text);
                    //DDLCategoryID.SelectedValue = GVCutomers.DataKeys[row.RowIndex].Values[1].ToString();
                    //TBNTN.Value = Server.HtmlDecode(row.Cells[4].Text);
                    //DDLCustomerType.SelectedValue = GVCutomers.DataKeys[row.RowIndex].Values[2].ToString();
                    //DDLArea.SelectedValue = Server.HtmlDecode(row.Cells[6].Text);
                    //TBRefNum.Value = Server.HtmlDecode(row.Cells[7].Text);
                    //TBDistrict.Value = Server.HtmlDecode(row.Cells[8].Text);
                    //TBPhone.Value = Server.HtmlDecode(row.Cells[9].Text);
                    //TBEmail.Value = Server.HtmlDecode(row.Cells[10].Text);
                    //TBCellNo.Value = Server.HtmlDecode(row.Cells[11].Text);
                    //TBPostalCode.Value = Server.HtmlDecode(row.Cells[12].Text);
                    //TBCellNo2.Value = Server.HtmlDecode(row.Cells[13].Text);
                    //TBPostalOfficeContact.Value = Server.HtmlDecode(row.Cells[14].Text);
                    //TBCellNo3.Value = Server.HtmlDecode(row.Cells[15].Text);
                    //TBNIC.Value = Server.HtmlDecode(row.Cells[16].Text);
                    //TBCellNo4.Value = Server.HtmlDecode(row.Cells[17].Text);
                    //DDLCityID.SelectedValue = GVCutomers.DataKeys[row.RowIndex].Values[3].ToString();
                    //TBAddress.Value = Server.HtmlDecode(row.Cells[19].Text);
                    //disabled();
                    //btnEdit.Enabled = true;
                    //btnReset.Enabled = false;
                    //btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }

        }

        public void disabled()
        {
            TBCustomersName.Disabled = true;
            TBGST.Disabled = true;
            DDLCategoryID.Enabled = false;
            TBNTN.Disabled = true;
            DDLCustomerType.Enabled = false;
            DDLArea.Enabled = false;
            TBSalPer.Disabled = true;
            TBDistrict.Disabled = true;
            TBPhone.Disabled = true;
            TBEmail.Disabled = true;
            TBCellNo.Disabled = true;
            TBPostalCode.Disabled = true;
            TBCellNo2.Disabled = true;
            TBPostalOfficeContact.Disabled = true;
            TBCellNo3.Disabled = true;
            TBNIC.Disabled = true;
            TBCellNo4.Disabled = true;
            DDLCityID.Enabled = false;
            TBAddress.Disabled = true;

        }

        public void enabled()
        {
            TBCustomersName.Disabled = false;
            TBGST.Disabled = false;
            DDLCategoryID.Enabled = true;
            TBNTN.Disabled = false;
            DDLCustomerType.Enabled = true;
            DDLArea.Enabled = true;
            TBSalPer.Disabled = false;
            TBDistrict.Disabled = false;
            TBPhone.Disabled = false;
            TBEmail.Disabled = false;
            TBCellNo.Disabled = false;
            TBPostalCode.Disabled = false;
            TBCellNo2.Disabled = false;
            TBPostalOfficeContact.Disabled = false;
            TBCellNo3.Disabled = false;
            TBNIC.Disabled = false;
            TBCellNo4.Disabled = false;
            DDLCityID.Enabled = true;
            TBAddress.Disabled = false;

        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
            TBCustomersName.Focus();
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Clear();
            v_name.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
            TBCustomersName.Focus();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //btnEdit.Enabled = false;
            //btnReset.Enabled = true;
            //btnSubmit.Enabled = true;
            //enabled();


            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
        }

        protected void TBSearch_TextChanged(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
            try
            {
                FillGrid();
                DataTable _dt = (DataTable)ViewState["Customer"];
                DataView dv = new DataView(_dt, "CustomerName LIKE '%" + TBSearch.Text.Trim().ToUpper() + "%'", "[CustomerName] ASC", DataViewRowState.CurrentRows);
                DataTable dt_ = new DataTable();
                dt_ = dv.ToTable();
                GVCutomers.DataSource = dt_;
                GVCutomers.DataBind();
                TBSearch.Text = "";
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }

        }

        public void BindDll()
        {
            try
            {
                //Category

                DataTable dtCustCat = new DataTable();

                dtCustCat = DBConnection.GetQueryData("select CategoryID,Category from CustomerCategory");

                DDLCategoryID.DataSource = dtCustCat;
                DDLCategoryID.DataTextField = "Category";
                DDLCategoryID.DataValueField = "CategoryID";
                DDLCategoryID.DataBind();
                DDLCategoryID.Items.Add(new ListItem("--Select--", "0"));

                //Customer Type

                DataTable dtCusttype = new DataTable();

                dtCusttype = DBConnection.GetQueryData("select CustomerTypeID,CustomerType_ from CustomerType");

                DDLCustomerType.DataSource = dtCusttype;
                DDLCustomerType.DataTextField = "CustomerType_";
                DDLCustomerType.DataValueField = "CustomerTypeID";
                DDLCustomerType.DataBind();
                DDLCustomerType.Items.Add(new ListItem("--Select--", "0"));

                //City

                DataTable dtCity = new DataTable();

                dtCity = DBConnection.GetQueryData("select CityID,City_ from City");

                DDLCityID.DataSource = dtCity;
                DDLCityID.DataTextField = "City_";
                DDLCityID.DataValueField = "CityID";
                DDLCityID.DataBind();
                DDLCityID.Items.Add(new ListItem("--Select--", "0"));

                //Area

                DataTable dtArea = new DataTable();

                dtArea = DBConnection.GetQueryData("select areaid, area_ from tbl_area where CompanyId='" + Session["CompanyID"] + "' and BranchId = '" + Session["BranchID"] + "'");

                DDLArea.DataSource = dtArea;
                DDLArea.DataTextField = "area_";
                DDLArea.DataValueField = "areaid";
                DDLArea.DataBind();
                DDLArea.Items.Add(new ListItem("--Select Area--", "0"));

            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }
        }

        protected void DDLCategoryID_TextChanged(object sender, EventArgs e)
        {
            if (DDLCategoryID.Items.FindByText("< Add New >").Selected == true)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCustCategory();", true);
                TBCustomerCategoryName.Focus();

            }
        }

        protected void LinkBtnAddCustCategory_Click(object sender, EventArgs e)
        {
            if (TBCustomerCategoryName.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCustCategory();", true);
                TBCustomerCategoryName.Focus();
                BindDll();
            }
            else
            {
                //string query = "select * from CustomerCategory where Category ='" + TBCustomerCategoryName.Text + "'";

                //SqlCommand cmdcategoryname = new SqlCommand(query, con);

                DataTable dtcategoryname = new DataTable();

                //SqlDataAdapter adpcustomertype = new SqlDataAdapter(cmdcategoryname);

                //adpcustomertype.Fill(dtcategoryname);
                
                
                if (dtcategoryname.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Category Name is already Exits!!";
                    BindDll();

                }
                else
                {

                    try
                    {
                        CustomerCategory CustCat = new CustomerCategory();

                        CustCat.CategoryID = HFCustCategory.Value;
                        CustCat.Category = string.IsNullOrEmpty(TBCustomerCategoryName.Text) ? null : TBCustomerCategoryName.Text;

                        CustomerCategoryManager custcatmanag = new CustomerCategoryManager(CustCat);
                        custcatmanag.Save();
                        ClearCustCat();
                        BindDll();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);

                    }
                    catch (Exception ex)
                    {
                        //   throw;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = ex.Message;
                    }
                }

            }
        }

        public void ClearCustCat()
        {
            HFCustCategory.Value = "";
            TBCustomerCategoryName.Text = "";
        }

        protected void LinkBtnCancelCustCategory_Click(object sender, EventArgs e)
        {
            ClearCustCat();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
            BindDll();

        }

        protected void DDLCategoryID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DDLCategoryID.Items.FindByText("< Add New >").Selected == true)
            //{

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCustCategory();", true);
            //    TBCustomerCategoryName.Focus();

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
            //}
        }

        protected void DDLCityID_TextChanged(object sender, EventArgs e)
        {
            //if (DDLCityID.Items.FindByText("< Add New >").Selected == true)
            //{

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCity();", true);
            //    TBCity.Focus();

            //}

        }

        protected void DDLCityID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DDLCityID.Items.FindByText("< Add New >").Selected == true)
            //{

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCity();", true);
            //    TBCity.Focus();

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);
            //}
        }

        protected void DDLCustomerType_TextChanged(object sender, EventArgs e)
        {
            if (DDLCustomerType.Items.FindByText("< Add New >").Selected == true)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "MyModalCustomerType();", true);
                TBCustomerType.Focus();

            }

        }

        protected void DDLCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (DDLCustomerType.Items.FindByText("< Add New >").Selected == true)
            //{

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "MyModalCustomerType();", true);
            //    TBCustomerType.Focus();

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
            //}
        }

        protected void LinkBtnInsertCustomerType_Click(object sender, EventArgs e)
        {
            if (TBCustomerType.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "MyModalCustomerType();", true);
                TBCustomerType.Focus();
                BindDll();
            }
            else
            {
                string query = "select * from CustomerType where CustomerType_ ='" + TBCustomerType.Text + "'";

                SqlCommand cmdcustomertype = new SqlCommand(query, con);

                DataTable dtcustomertype = new DataTable();

                SqlDataAdapter adpcustomertype = new SqlDataAdapter(cmdcustomertype);

                adpcustomertype.Fill(dtcustomertype);

                if (dtcustomertype.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "Customer Type is already Exits!!";
                    BindDll();
                }
                else
                {
                    try
                    {
                        CustomerType custtype = new CustomerType();

                        custtype.CustomerTypeID = HFMyCustomerType.Value;
                        custtype.CustomerType_ = string.IsNullOrEmpty(TBCustomerType.Text) ? null : TBCustomerType.Text;

                        CustomerTypeManager custtypemanag = new CustomerTypeManager(custtype);
                        custtypemanag.Save();
                        ClearCustomerType();
                        BindDll();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);

                    }
                    catch (Exception ex)
                    {
                        //   throw;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = ex.Message;
                    }
                }

            }
        }

        protected void LinkBtnCancelCustomerType_Click(object sender, EventArgs e)
        {
            ClearCustomerType();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
            BindDll();

        }

        public void ClearCustomerType()
        {
            HFMyCustomerType.Value = "";
            TBCustomerType.Text = "";
        }

        protected void LinkBtnCityInsert_Click(object sender, EventArgs e)
        {

            if (TBCity.Text == "")
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCity();", true);
                TBCity.Focus();
                BindDll();
            }
            else
            {
                string query = "select * from City where City_ ='" + TBCity.Text + "'";

                SqlCommand cmdcity = new SqlCommand(query, con);

                DataTable dtcity = new DataTable();

                SqlDataAdapter adpcity = new SqlDataAdapter(cmdcity);

                adpcity.Fill(dtcity);

                if (dtcity.Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                    lblalert.Text = "City is already Exits!!";
                    BindDll();
                }
                else
                {
                    try
                    {
                        City city = new City();

                        city.CityID = HFCity.Value;
                        city.City_ = string.IsNullOrEmpty(TBCity.Text) ? null : TBCity.Text;

                        CityManager citymanag = new CityManager(city);

                        citymanag.Save();
                        ClearCustomerType();
                        BindDll();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);

                    }
                    catch (Exception ex)
                    {
                        //   throw;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = ex.Message;
                    }
                }
            }


        }

        protected void LinkBtnCityCancel_Click(object sender, EventArgs e)
        {
            ClearCity();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);
            BindDll();

        }

        public void ClearCity()
        {
            HFCity.Value = "";
            TBCity.Text = "";
        }

        protected void GVCutomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "AlertDelete();", true);
            lblmodaldelete.Text = "Are you sure you want to Delete !!";
            string id = GVCutomers.DataKeys[e.RowIndex].Values[0] != null ? GVCutomers.DataKeys[e.RowIndex].Values[0].ToString() : null;
            HFCustId.Value = id;
           
        }

        protected void linkmodaldelete_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlquery = " delete from  Customers_ where CustomerID = '" + HFCustId.Value + "'";
                SqlCommand cmd = new SqlCommand(sqlquery, con);

                con.Open();
                cmd.ExecuteNonQuery();
                FillGrid();
                con.Close();

            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;

            }

        }
        protected void btnalertOk_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showcustomer();", true);

            if (TBCustomersName.Value == "")
            {
                TBCustomersName.Focus();

            }
            else if (TBPhone.Value == "")
            {
                TBPhone.Focus();

            }
            else if (TBEmail.Value == "")
            {

                TBEmail.Focus();

            }
            else if (TBCellNo.Value == "")
            {
                TBCellNo.Focus();
            }
            else if (TBNIC.Value == "")
            {
                TBNIC.Focus();
            }
            else if (TBAddress.Value == "")
            {
                TBAddress.Focus();
            }
        }
    }
}