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
    public partial class Supplier_ : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_;
        DBConnection db= new DBConnection();
        SqlDataAdapter adapter;


        string query, str;
        string accno = "";
        decimal ttlcre;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                FillGrid();
                BindDll();
                btnEdit.Enabled = false;
                //GridView1.Visible = false;
                lblcity.Visible = false;
                btnadd.Focus();
            }

        }

        public void FillGrid()
        {
            try
            {
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData(" SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID, supplierId, a.suppliername, a.contactperson, a.BackUpContact, a.City_, a.phoneno, a.mobile, a.faxno, a.postalcode, a.designation, " +
                    " a.AddressOne, a.AddressTwo, a.CNIC, a.Url, a.BusinessNature, a.Email, a.NTNNTRNo, a.CreatedBy, CONVERT(varchar, a.CreatedDate, 103) as [CreatedDate], a.IsActive, a.Sup_Code from supplier a where a.CompanyId = '" + Session["CompanyID"] + "' and a.BranchId= '" + Session["BranchID"] + "' order by a.supplierId desc");

                GVSupplier.DataSource = dt_;
                GVSupplier.DataBind();
                ViewState["Supplier"] = dt_;
            }
            catch (Exception ex)
            {
                   //throw;
                   ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                   lblalert.Text = ex.Message;
            }
        }

        public void BindDll()
        {
            try
            {
                dt_ = new DataTable();
                dt_ = DBConnection.GetQueryData("select CityID,City_ from City");
                DDLCity.DataSource = dt_;
                DDLCity.DataTextField = "City_";
                DDLCity.DataValueField = "CityID";
                DDLCity.DataBind();
                DDLCity.Items.Add(new ListItem("--Select--", "0"));


            }
            catch (Exception ex)
            {
                //   throw;
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
                DataTable _dt = (DataTable)ViewState["Supplier"];
                DataView dv = new DataView(_dt, "suppliername LIKE '%" + TBSearch.Text.Trim().ToUpper() + "%'", "[suppliername] ASC", DataViewRowState.CurrentRows);
                DataTable dt_ = new DataTable();
                dt_ = dv.ToTable();

                GVSupplier.DataSource = dt_;
                GVSupplier.DataBind();
                TBSearch.Text = "";
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
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
                string FileName = "SupplierList.xls";
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                GVSupplier.GridLines = GridLines.Both;
                GVSupplier.HeaderStyle.Font.Bold = true;

                GVSupplier.RenderControl(htmltextwrtter);

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
            //    //throw;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
            //    lblalert.Text = ex.Message;
            //}
        }

        private void Import_To_Grid(string FilePath, string Extension, string Step)
        {
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
            //        SheetName = "SupplierList$";
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
        }

        private void LoadData(DataTable dt_)
        {
            try
            {
                for (int i = 0; i < dt_.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt_.Rows[i]["ID"].ToString()))
                    {
                        supplier supplier = new supplier();

                        supplier.supplierId = dt_.Rows[i]["ID"].ToString();
                        supplier.suppliername = dt_.Rows[i]["Supplier Name"].ToString();
                        supplier.contactperson = dt_.Rows[i]["Contact Person"].ToString();
                        supplier.BackUpContact = dt_.Rows[i]["BackUp Contact"].ToString();
                        supplier.City_ = dt_.Rows[i]["City"].ToString();
                        supplier.phoneno = dt_.Rows[i]["Phone No"].ToString();
                        supplier.mobile = dt_.Rows[i]["Mobile"].ToString();
                        supplier.faxno = dt_.Rows[i]["Fax No"].ToString();
                        supplier.postalcode = dt_.Rows[i]["Postal Code"].ToString();
                        supplier.designation = dt_.Rows[i]["Designation"].ToString();
                        supplier.AddressOne = dt_.Rows[i]["Address One"].ToString();
                        supplier.AddressTwo = dt_.Rows[i]["Address Two"].ToString();
                        supplier.CNIC = dt_.Rows[i]["NIC"].ToString();
                        supplier.Url = dt_.Rows[i]["Url"].ToString();
                        supplier.BusinessNature = dt_.Rows[i]["Business Nature"].ToString();
                        supplier.Email = dt_.Rows[i]["Email"].ToString();
                        supplier.NTNNTRNo = dt_.Rows[i]["NTN/NTR No"].ToString();
                        supplier.CreatedBy = Session["user"].ToString();
                        supplier.CreatedDate = DateTime.Now;
                        supplier.IsActive = "True";

                        SupplierManager supmanag = new SupplierManager(supplier);
                        supmanag.Save();
                        FillGrid();

                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                //bool phn = false;
                //bool email = false;
                //Regex regexobjphn = new Regex(@"^([0-9]*|\d*\.\d{1}?\d*)$");
                //Regex regexobjemail = new Regex(@"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$");


                //if (TBSupplierName.Value == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write Supplier Name!!";
                //}

                //else if (TBPhoneNo.Value == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write Phone Num!!";
                //}
              
                //else if (TBMobile.Value == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write Cell No!!";
                //}
                //else if (TBDesignation.Value == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write Designation!!";
                //}
                //else if (TBNIC.Value == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write NIC!!";
                //}
                //else if (TBAddressOne.Value == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write Address!!";
                //}
                //else if (TBBusinessNature.Value == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write Business Nature!!";
                //}

                //else if (!regexobjphn.IsMatch(TBPhoneNo.Value))
                //{
                //    phn = false;
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //    lblalert.Text = "Please Write Correct Phone Number!!";
                //}
                
                //else
                //{
                if (TBSupplierName.Value == "")
                {
                    v_name.Text = "Enter Supplier Name";
                    v_contactperson.Text = "";
                    TBSupplierName.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);

                }
                else if (TBContactPerson.Value =="")
                {
                    v_contactperson.Text = "Enter Contact Person";
                    v_name.Text = "";
                    TBContactPerson.Focus();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);
                }
                else if (HFSupplierID.Value == "")
                {
                    v_contactperson.Text = "";
                       v_name.Text = "";

                    int a;
                    a = Save();

                    if (a == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Supplier Has Been Saved!";
                        Clear();
                        FillGrid();
                    }
                }
                else if (HFSupplierID.Value != "")
                {

                    v_contactperson.Text = "";
                    v_name.Text = "";
                    int b;
                    b = Update();

                    if (b == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lblalert.Text = "Supplier Has Been Updated!";
                        Clear();
                        FillGrid();
                    }
                    
                }
                    //else
                //    {
                //        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //        lblalert.Text = "Some thing is wrong Call the Administrator!!";
                //    }
                //}
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }
        }

        public void ShowAccountCategoryID()
        {
            try
            {

                str = "select SubHeadCategoriesID from SubHeadCategories order by SubHeadCategoriesID desc";
                SqlCommand cmd = new SqlCommand(str, con);
                //con.Open();

                DataTable dt_ = new DataTable();

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt_);
                if (dt_.Rows.Count <= 0)
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
                            int v = Convert.ToInt32(dt_.Rows[0]["SubHeadCategoriesID"].ToString());
                            int b = v + 1;
                            SubHeadCatFiv.Value = b.ToString();

                        }
                    //}
                }
                //con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ShowAccountCategoryFiveID()
        {
            try
            {

                string str = "select max(cast(subheadcategoryfiveID as int))  as [subheadcategoryfiveID] from subheadcategoryfive";
                SqlCommand cmd = new SqlCommand(str, con);
                con.Open();

                DataTable dt_ = new DataTable();

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt_);
                if (dt_.Rows.Count <= 0)
                {
                    SubHeadCatFiv.Value = "MB0000001";
                }
                else
                {
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        //SubHeadCatFiv.Value = "";

                        //if (string.IsNullOrEmpty(SubHeadCatFiv.Value) )                        
                        {
                            int v = Convert.ToInt32(reader["subheadcategoryfiveID"].ToString());
                            int b = v + 1;
                            SubHeadCatFiv.Value = "MB000000" + b.ToString();
                        }
                    }

                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
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

                command.CommandText = " update supplier set suppliername = '" + TBSupplierName.Value + "', contactperson = '" + TBContactPerson.Value +
                                " ', BackUpContact = '" + TBBackUpContact.Value + "', City_ ='" + DDLCity.SelectedItem.Text.ToString() +
                                " ', phoneno='" + TBPhoneNo.Value + "',mobile ='" + TBMobile.Value + "', faxno='" + TBFaxNo.Value +
                                " ', postalcode='" + TBPostalCode.Value + "', designation='" + TBDesignation.Value +
                                " ', AddressOne='" + TBAddressOne.Value + "', AddressTwo ='" + TBAddressTwo.Value +
                                " ', CNIC='" + TBNIC.Value + "', Url='" + TBUrl.Value + "', BusinessNature='" + TBBusinessNature.Value +
                                "', Email='" + TBEmail.Value + "', prebal='" + TBPrevBal.Value + "' where  supplierId='" + HFSupplierID.Value +
                                "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

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
                        if (HFAccountNO.Value == "")
                        {
                            int v = Convert.ToInt32(dt.Rows[0]["SubHeadCategoriesID"].ToString());
                            int b = v + 1;
                            SubHeadCatFivAcc.Value = "0021" + b.ToString();

                            /// For account Entry..
                            /// ...
                            /// 

                            command.CommandText = " INSERT INTO [dbo].[SubHeadCategories] ([SubHeadCategoriesID] " +
                                " ,[ven_id] ,[SubHeadCategoriesName] ,[SubHeadCategoriesGeneratedID] ,[HeadGeneratedID] " +
                                " ,[SubHeadGeneratedID] ,[CreatedAt] ,[CreatedBy] ,[SubCategoriesKey]) VALUES " +
                                " ('" + b + "' ,0,'" + TBSupplierName.Value + "' " +
                                " ,'" + SubHeadCatFivAcc.Value + "', '002' " +
                                " ,'0021' ,'" + DateTime.Now + "','admin' " +
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

                if (dt_.Rows.Count > 0)
                {
                    SubHeadCatFivAcc.Value = dt_.Rows[0]["SubHeadCategoriesGeneratedID"].ToString();
                }

                //Update Customer table for Account NO..

                command.CommandText = " update supplier set sup_acc='" + SubHeadCatFivAcc.Value + "' where supplierId ='" + HFSupplierID.Value.Trim() + "'";

                command.ExecuteNonQuery();

                //Update SubhaeadCategories table for Account NO..

                command.CommandText = " update SubHeadCategories set SubHeadCategoriesName='" + TBSupplierName.Value.Trim() +
                    "' where SubHeadCategoriesGeneratedID='" + SubHeadCatFivAcc.Value + "'";

                command.ExecuteNonQuery();


                #endregion

                #region Credit Sheets


                command.CommandText = "select CredAmt from tbl_Purcredit where supplierId='" + SubHeadCatFivAcc.Value.Trim() + "'";

                SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                DataTable dtsalcre = new DataTable();
                stksalcre.Fill(dtsalcre);

                if (dtsalcre.Rows.Count > 0)
                {

                    ttlcre = Convert.ToDecimal(TBPrevBal.Value.Trim());

                    command.CommandText = " Update tbl_Purcredit set CredAmt = '" + ttlcre + "' where supplierId='" + SubHeadCatFivAcc.Value.Trim() + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    //command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + DDL_CustAcc.SelectedValue.Trim() + "','" + ttlcre + "')";
                    command.CommandText = " insert into tbl_Purcredit (supplierId,CredAmt) values('" + SubHeadCatFivAcc.Value.Trim() + "','" + TBPrevBal.Value.Trim() + "')";
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
        //private int update()
        //{
        //    int k = 1;
        //    query = " update supplier set suppliername = '" + TBSupplierName.Value + "', contactperson = '" + TBContactPerson .Value +
        //            " ', BackUpContact = '" + TBBackUpContact.Value + "', City_ ='" + DDLCity.SelectedItem.Text.ToString() +
        //            " ', phoneno='" + TBPhoneNo.Value + "',mobile ='" + TBMobile.Value + "', faxno='"+ TBFaxNo.Value +
        //            " ', postalcode='" + TBPostalCode.Value + "', designation='" + TBDesignation.Value +
        //            " ', AddressOne='" + TBAddressOne.Value + "', AddressTwo ='" + TBAddressTwo.Value +
        //            " ', CNIC='" + TBNIC.Value + "', Url='" + TBUrl.Value + "', BusinessNature='" + TBBusinessNature.Value + "', Email='" + TBEmail.Value + "', NTNNTRNo='" + TBPrevBal.Value + "' where  supplierId='" + HFSupplierID.Value + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

        //    con.Open();

        //    using (SqlCommand cmd = new SqlCommand(query, con))
        //    {

        //        cmd.ExecuteNonQuery();

        //    }
        //    con.Close();
        //    return k;
        //}

      
        private int Save()
        {

            int j = 1;
            string SupID = "";

            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("SupplierTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {

                #region Supplier

                command.CommandText = " INSERT INTO [dbo].[supplier] " +
                               " ([suppliername],[contactperson],[BackUpContact],[City_],[phoneno] " +
                               " ,[mobile],[faxno],[postalcode],[designation] ,[AddressOne] ,[AddressTwo] " +
                               " ,[CNIC] ,[Url] ,[BusinessNature] ,[Email] ,[NTNNTRNo] ,[CreatedBy] " +
                               " ,[CreatedDate] ,[IsActive] ,[Sup_Code], [CompanyId], [BranchId], prebal)VALUES('" + TBSupplierName.Value + "','" + TBContactPerson.Value +
                               " ','" + TBBackUpContact.Value + "','" + DDLCity.SelectedItem.Text.ToString() + "','" + TBPhoneNo.Value +
                               " ','" + TBMobile.Value + "','" + TBFaxNo.Value + "','" + TBPostalCode.Value + "','" + TBDesignation.Value +
                               " ','" + TBAddressOne.Value + "','" + TBAddressTwo.Value + "','" + TBNIC.Value + "','" + TBUrl.Value +
                               " ','" + TBBusinessNature.Value + "','" + TBEmail.Value + "','','" + Session["user"].ToString() +
                               " ','" + DateTime.Now + "','" + SubHeadCatFiv.Value + "','true','" +
                               Session["CompanyID"] + "','" + Session["BranchID"] + "','" + TBPrevBal.Value + "')";

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

                    if (dt.Rows.Count <= 0)
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

                    if (dt.Rows.Count <= 0)
                    {
                        SubHeadCatFivAcc.Value = "1";
                    }
                    else
                    {
                        if (SubHeadCatFivAcc.Value == "")
                        {
                            int v = Convert.ToInt32(dt.Rows[0]["SubHeadCategoriesID"].ToString());
                            int b = v + 1;
                            SubHeadCatFivAcc.Value = "0021" + b.ToString();
                        }
                    }
                }
                
                /// For account Entry..
                /// ...
                /// 

                command.CommandText = " INSERT INTO [dbo].[SubHeadCategories] ([SubHeadCategoriesID] " +
                    " ,[ven_id] ,[SubHeadCategoriesName] ,[SubHeadCategoriesGeneratedID] ,[HeadGeneratedID] " +
                    " ,[SubHeadGeneratedID] ,[CreatedAt] ,[CreatedBy] ,[SubCategoriesKey]) VALUES " +
                    " ('" + SubHeadCatFiv.Value + "' ,0,'" + TBSupplierName.Value + "' " +
                    " ,'" + SubHeadCatFivAcc.Value + "', '002' " +
                    " ,'0021' ,'" + DateTime.Now + "','admin' " +
                    " ,'" + SubHeadCatFivAcc.Value + "') ";

                command.ExecuteNonQuery();


                command.CommandText = "select top 1 supplierId from supplier order by supplierId desc";

                SqlDataAdapter adp1 = new SqlDataAdapter(command);

                DataTable dt1 = new DataTable();

                adp1.Fill(dt1);

                if (dt1.Rows.Count > 0)
                {
                    SupID = dt1.Rows[0]["supplierId"].ToString();
                }

                /// For adding supplier account no 
                /// ...
                /// 

                command.CommandText = " update supplier set sup_acc='" + SubHeadCatFivAcc.Value + "' where supplierId ='" + SupID + "'";

                command.ExecuteNonQuery();



                #endregion


                #region Credit Sheets

                command.CommandText = " select SubHeadCategoriesGeneratedID,SubHeadCategoriesName from SubHeadCategories where SubHeadCategoriesName='" + TBSupplierName.Value.Trim() + "'";

                SqlDataAdapter adpchkcust = new SqlDataAdapter(command);

                DataTable dtchkcust = new DataTable();
                adpchkcust.Fill(dtchkcust);

                if (dtchkcust.Rows.Count > 0)
                {
                    accno = dtchkcust.Rows[0]["SubHeadCategoriesGeneratedID"].ToString();
                }

                command.CommandText = "select CredAmt from tbl_Purcredit where supplierId='" + accno.Trim() + "'";

                SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                DataTable dtsalcre = new DataTable();
                stksalcre.Fill(dtsalcre);

                if (dtsalcre.Rows.Count > 0)
                {
                    //double recv = Convert.ToDouble(lblOutstan) - Convert.ToDouble(TBRecy);

                    //avapre = Convert.ToDecimal(dtsalcre.Rows[0]["CredAmt"]);

                    ttlcre = Convert.ToDecimal(TBPrevBal.Value.Trim());

                    command.CommandText = " Update tbl_Purcredit set CredAmt = '" + ttlcre + "' where supplierId='" + accno.Trim() + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    //command.CommandText = " insert into tbl_Purcredit (CustomerID,CredAmt) values('" + DDL_CustAcc.SelectedValue.Trim() + "','" + ttlcre + "')";
                    command.CommandText = " insert into tbl_Purcredit (supplierId,CredAmt) values('" + accno.Trim() + "','" + TBPrevBal.Value.Trim() + "')";
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


        private int saveHeadFiv()
        {
            int j = 1;
            string command = "";
            string SupID = "";

            try
            {
                subheadcategoryfive subheadfive = new subheadcategoryfive();

                if (HFSubHeadCatFivID.Value == "")
                {
                    ShowAccountCategoryFiveID();
                }

                subheadfive.subheadcategoryfiveID = HFSubHeadCatFivID.Value;
                subheadfive.subheadcategoryfiveName = string.IsNullOrEmpty(TBSupplierName.Value) ? null : TBSupplierName.Value;
                subheadfive.subheadcategoryfiveGeneratedID = string.IsNullOrEmpty(SubHeadCatFiv.Value) ? null : SubHeadCatFiv.Value;
                subheadfive.HeadGeneratedID = string.IsNullOrEmpty("MB001") ? null : "MB001";
                subheadfive.SubHeadGeneratedID = string.IsNullOrEmpty("MB0001") ? null : "MB0001";
                subheadfive.SubHeadCategoriesGeneratedID = string.IsNullOrEmpty("MB00002") ? null : "MB00002";
                subheadfive.subheadcategoryfourGeneratedID = string.IsNullOrEmpty("MB000003") ? null : "MB000003";
                subheadfive.CreatedAt = DateTime.Now;
                subheadfive.CreatedBy = Session["user"].ToString();
                subheadfive.SubFiveKey = string.IsNullOrEmpty(SubHeadCatFiv.Value) ? null : SubHeadCatFiv.Value;


                subheadcategoryfiveManager subheadcatfive = new subheadcategoryfiveManager(subheadfive);
                subheadcatfive.Save();


                supplier suplier = new supplier();

                suplier.supplierId = HFSupplierID.Value;
                suplier.suppliername = string.IsNullOrEmpty(TBSupplierName.Value) ? null : TBSupplierName.Value;
                suplier.contactperson = string.IsNullOrEmpty(TBContactPerson.Value) ? null : TBContactPerson.Value;
                suplier.BackUpContact = string.IsNullOrEmpty(TBBackUpContact.Value) ? null : TBBackUpContact.Value;
                suplier.City_ = DDLCity.SelectedItem.Text.ToString() != "0" ? DDLCity.SelectedItem.Text.ToString() : null;
                suplier.phoneno = string.IsNullOrEmpty(TBPhoneNo.Value) ? null : TBPhoneNo.Value;
                suplier.mobile = string.IsNullOrEmpty(TBMobile.Value) ? null : TBMobile.Value;
                suplier.faxno = string.IsNullOrEmpty(TBFaxNo.Value) ? null : TBFaxNo.Value;
                suplier.postalcode = string.IsNullOrEmpty(TBPostalCode.Value) ? null : TBPostalCode.Value;
                suplier.designation = string.IsNullOrEmpty(TBDesignation.Value) ? null : TBDesignation.Value;
                suplier.AddressOne = string.IsNullOrEmpty(TBAddressOne.Value) ? null : TBAddressOne.Value;
                suplier.AddressTwo = string.IsNullOrEmpty(TBAddressTwo.Value) ? null : TBAddressTwo.Value;
                suplier.CNIC = string.IsNullOrEmpty(TBNIC.Value) ? null : TBNIC.Value;
                suplier.Url = string.IsNullOrEmpty(TBUrl.Value) ? null : TBUrl.Value;
                suplier.BusinessNature = string.IsNullOrEmpty(TBBusinessNature.Value) ? null : TBBusinessNature.Value;
                suplier.Email = string.IsNullOrEmpty(TBEmail.Value) ? null : TBEmail.Value;
                suplier.NTNNTRNo = string.IsNullOrEmpty(TBPrevBal.Value) ? null : TBPrevBal.Value;
                suplier.CreatedBy = Session["user"].ToString();
                suplier.CreatedDate = DateTime.Now;
                suplier.IsActive = "true";
                suplier.Sup_Code = string.IsNullOrEmpty(SubHeadCatFiv.Value) ? null : SubHeadCatFiv.Value;

                SupplierManager supmanag = new SupplierManager(suplier);
                supmanag.Save();
                Clear();
                FillGrid();


            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;

            }
            return j;
        }

        public void Clear()
        {
            HFSupplierID.Value = "";
            TBSupplierName.Value = "";
            TBContactPerson.Value = "";
            TBBackUpContact.Value = "";
            TBMobile.Value = "";
            TBFaxNo.Value = "";
            TBPostalCode.Value = "";
            TBDesignation.Value = "";
            TBAddressOne.Value = "";
            TBAddressTwo.Value = "";
            TBNIC.Value = "";
            TBUrl.Value = "";
            TBBusinessNature.Value = "";
            TBEmail.Value = "";
            TBPrevBal.Value = "";
            SubHeadCatFiv.Value = "";
            SubHeadCatFivAcc.Value = "";
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Clear();
            v_contactperson.Text = "";
            v_name.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);
            TBSupplierName.Focus();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnReset.Enabled = true;
            btnSubmit.Enabled = true;
            enabled();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);
        }

        protected void DDLCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLCity.Items.FindByText("< Add New >").Selected == true)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCity();", true);
                TBCity.Focus();

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);
            }
        }

        protected void DDLCity_TextChanged(object sender, EventArgs e)
        {
            if (DDLCity.Items.FindByText("< Add New >").Selected == true)
            {

                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "myModalCity();", true);
                TBCity.Focus();
               
            }
        }

        public void disabled()
        {
            TBSupplierName.Disabled = true;
            TBContactPerson.Disabled = true;
            TBBackUpContact.Disabled = true;
            DDLCity.Enabled = false;
            TBPhoneNo.Disabled = true;
            TBMobile.Disabled = true;
            TBFaxNo.Disabled = true;
            TBPostalCode.Disabled = true;
            TBDesignation.Disabled = true;
            TBAddressOne.Disabled = true;
            TBAddressTwo.Disabled = true;
            TBNIC.Disabled = true;
            TBUrl.Disabled = true;
            TBBusinessNature.Disabled = true;
            TBEmail.Disabled = true;
            TBPrevBal.Disabled = true;
        }

        public void enabled()
        {
            TBSupplierName.Disabled = false;
            TBContactPerson.Disabled = false;
            TBBackUpContact.Disabled = false;
            DDLCity.Enabled = true;
            TBPhoneNo.Disabled = false;
            TBMobile.Disabled = false;
            TBFaxNo.Disabled = false;
            TBPostalCode.Disabled = false;
            TBDesignation.Disabled = false;
            TBAddressOne.Disabled = false;
            TBAddressTwo.Disabled = false;
            TBNIC.Disabled = false;
            TBUrl.Disabled = false;
            TBBusinessNature.Disabled = false;
            TBEmail.Disabled = false;
            TBPrevBal.Disabled = false;
        }

        protected void GVSupplier_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Select")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    HFSupplierID.Value = GVSupplier.DataKeys[row.RowIndex].Values[1].ToString();
                    //string Query = "select * from supplier inner join subheadcategoryfive on supplier.Sup_Code = subheadcategoryfive.subheadcategoryfiveGeneratedID where supplier.Sup_Code ='" + SubHeadCatFiv.Value.Trim() + "'";
                    string Query = "select * from supplier  where supplierId ='" + HFSupplierID.Value.Trim() + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    dt_ = DBConnection.GetDataTable(Query);

                    if (dt_.Rows.Count > 0)
                    {
                        TBSupplierName.Value = dt_.Rows[0]["suppliername"].ToString();
                        TBContactPerson.Value = dt_.Rows[0]["contactperson"].ToString();
                        TBBackUpContact.Value = dt_.Rows[0]["BackUpContact"].ToString();
                        DDLCity.SelectedItem.Text = dt_.Rows[0]["City_"].ToString();
                        TBPhoneNo.Value = dt_.Rows[0]["phoneno"].ToString();
                        TBMobile.Value = dt_.Rows[0]["mobile"].ToString();
                        TBFaxNo.Value = dt_.Rows[0]["faxno"].ToString();
                        TBPostalCode.Value = dt_.Rows[0]["postalcode"].ToString();
                        TBDesignation.Value = dt_.Rows[0]["designation"].ToString();
                        TBAddressOne.Value = dt_.Rows[0]["AddressOne"].ToString();
                        TBAddressTwo.Value = dt_.Rows[0]["AddressTwo"].ToString();
                        TBNIC.Value = dt_.Rows[0]["CNIC"].ToString();
                        TBUrl.Value = dt_.Rows[0]["Url"].ToString();
                        TBBusinessNature.Value = dt_.Rows[0]["BusinessNature"].ToString();
                        TBEmail.Value = dt_.Rows[0]["Email"].ToString();
                        TBPrevBal.Value = dt_.Rows[0]["prebal"].ToString();
                        HFSupplierID.Value = dt_.Rows[0]["supplierId"].ToString();
                        HFAccountNO.Value = dt_.Rows[0]["sup_acc"].ToString();

                        //HFSubHeadCatFivID.Value = dt_.Rows[0]["subheadcategoryfiveID"].ToString();
                        //SubHeadCatFiv.Value = dt_.Rows[0]["subheadcategoryfiveGeneratedID"].ToString();
                    }
                    disabled();
                    btnEdit.Enabled = true;
                    btnReset.Enabled = false;
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                //   throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }


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
                        BindDll();
                        clearcity();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "showsupplier();", true);

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

        public void clearcity()
        {
            TBCity.Text = "";
        }

        protected void GVSupplier_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "AlertDelete();", true);
            lblmodaldelete.Text = "Are you sure you want to Delete !!";
            string id = GVSupplier.DataKeys[e.RowIndex].Values[1] != null ? GVSupplier.DataKeys[e.RowIndex].Values[1].ToString() : null;
            HFSupID.Value = id;
           
        } 

        protected void linkmodaldelete_Click(object sender, EventArgs e)
        {
            try
            {
                string sqlquery = " delete from  supplier where supplierId = '" + HFSupID.Value + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                db.CRUDRecords(sqlquery);
                FillGrid();

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