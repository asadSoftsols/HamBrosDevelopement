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
    public partial class frm_Sal : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);
        DataTable dt_ = null;
        DBConnection db = new DBConnection();
        int i = 0;
        int chkdetails = 0;
        string MSTkId, DDL_Par, DDL_Prorefcde, TBItmDes, custid, TBItmQty, TbItmunt, Tbrat, Tbamt, HFDSal, stkqty, query, TBrat;
        decimal totalprev;

        protected void Page_Load(object sender, EventArgs e)
        {
            v_custumer.Text = "Please Select Customer";
            TBoutstan.Enabled = false;
          
            #region Initials
            if (!IsPostBack)
            {
                TBRecov.Text = "0.00";
                lbl_outstan.Text = "0.00";
                TBoutstan.Text = "0.00";
                TBDIS.Text = "0.00";

                SetInitRowSal();
                FillGrid();
                ////BindPar();
                BindCus();
                //BindSO();
                TBSalDat.Text = DateTime.Now.ToShortDateString();
                ptnSno();
                chk_prtd.Checked = true;
                chk_Act.Checked = true;
                pnlSO.Visible = false;
                //pnl_sch.Visible = false;

                lbl_Cust.Visible = false;
                lbl_book.Visible = false;
                lbl_Gttl.Visible = false;
                lbl_ttl.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chksalrat.Visible = false;
                lbl_Itmqty.Visible = false;
                lbl_Pros.Visible = false;
                lbl_saldat.Visible = false;
            }
            #endregion
        }




        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetCust(string prefixText)
        {
            SqlConnection con = DataAccess.DBConnection.connection();
            SqlDataAdapter da;
            DataTable dt;
            DataTable Result = new DataTable();
            string str = "select CustomerName from Customers_ where CustomerName like '%" + prefixText + "%'";
            da = new SqlDataAdapter(str, con);
            dt = new DataTable();
            da.Fill(dt);
            List<string> Output = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
                Output.Add(dt.Rows[i][0].ToString());
            return Output;
        }

        [System.Web.Script.Services.ScriptMethod()]
        [System.Web.Services.WebMethod]
        public static List<string> GetSearch(string prefixText)
        {
            SqlConnection con = DataAccess.DBConnection.connection();
            SqlDataAdapter da;
            DataTable dt;
            DataTable Result = new DataTable();
            //string str = "select ProductName from Products where ProductName like '" + prefixText + "%'";
            string str = "select distinct(ProductName) from tbl_dstk " +
                        " inner join products on tbl_dstk.productid = Products.ProductID where ProductName like '" + prefixText + "%'";

            da = new SqlDataAdapter(str, con);
            dt = new DataTable();
            da.Fill(dt);
            List<string> Output = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
                Output.Add(dt.Rows[i][0].ToString());
            return Output;
        } 

        #region Method

        public void BindCus()
        {
            try
            {
                DataTable dt_Cust = new DataTable();

                dt_Cust = DBConnection.GetQueryData("select  CustomerName, cust_acc  from Customers_ where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");
                //dt_Cust = DBConnection.GetQueryData("select SubHeadCategoriesGeneratedID as [CustomerID], SubHeadCategoriesName as [CustomerName] from SubHeadCategories where SubHeadGeneratedID = '0011'");

                DDL_Cust.DataSource = dt_Cust;
                DDL_Cust.DataTextField = "CustomerName";
                DDL_Cust.DataValueField = "cust_acc";
                DDL_Cust.DataBind();
                DDL_Cust.Items.Insert(0, new ListItem("--Select Customer--", "0"));

                //Bookers

                DataTable dt_book = new DataTable();

                dt_book = DBConnection.GetQueryData("select  *  from Users where Level = 2 and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

                DDL_Book.DataSource = dt_book;
                DDL_Book.DataTextField = "Username";
                DDL_Book.DataValueField = "Username";
                DDL_Book.DataBind();
                DDL_Book.Items.Insert(0, new ListItem("--Select Bookers--", "0"));

                //Sales Man

                DataTable dt_salman = new DataTable();

                dt_salman = DBConnection.GetQueryData("select  *  from Users where Level = 3 and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'");

                DDL_SalMan.DataSource = dt_salman;
                DDL_SalMan.DataTextField = "Username";
                DDL_SalMan.DataValueField = "Username";
                DDL_SalMan.DataBind();
                DDL_SalMan.Items.Insert(0, new ListItem("--Select Sales Man--", "0"));
            }
            catch (Exception ex)
            {
                throw ex;
                //lbl_err.Text = ex.Message.ToString();
            }
        }
        
        public void BindSO()
        {
            try
            {
                DataTable dt_SO = new DataTable();
                 
                dt_SO = tbl_MSalManager.GetSO();
                
                DDL_SO.DataSource = dt_SO;
                DDL_SO.DataTextField = "MSalOrdsono";
                DDL_SO.DataValueField = "MSalOrdid";
                DDL_SO.DataBind();
                DDL_SO.Items.Insert(0, new ListItem("--Select SO--", "0"));             
            }
            catch (Exception ex)
            {
                throw ex;
                //lbl_err.Text = ex.Message.ToString();
            }
        }

        private void ptnSno()
        {
            try
            {
                //string str = "select isnull(max(cast(MSal_id as int)),0) as [MSal_id]  from tbl_MSal";
                string str = " select isnull(max(cast(tbl_Mstk.Mstk_id as int)),0) as [MSal_id]  from tbl_Mstk " +
                    " inner join tbl_MSal " +
                    " on tbl_Mstk.MSal_id = tbl_MSal.MSal_id";

                SqlCommand cmd = new SqlCommand(str, con);
                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    if (string.IsNullOrEmpty(TBSalesNum.Text))
                    {
                        int v = Convert.ToInt32(reader["MSal_id"].ToString());
                        int b = v + 1;
                        TBSalesNum.Text = "SAL00" + b.ToString(); 
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
                //lbl_err.Text = ex.Message.ToString();
            }
        }

        public void BindPar()
        {
            //Items Name
            try
            {
                using (SqlCommand cmdpro = new SqlCommand())
                {
                    con.Close();
                    //cmdpro.CommandText = " select rtrim('[' + CAST(ProductID AS VARCHAR(200)) + ']-' + ProductName ) as [ProductName], ProductID from Products ";
                    cmdpro.CommandText = " select rtrim('[' + CAST(ProductTypeID AS VARCHAR(200)) + ']-' + ProductTypeName ) as [ProductTypeName], ProductTypeID from tbl_producttype ";

                    cmdpro.Connection = con;
                    con.Open();

                    DataTable dtpro = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmdpro);
                    adp.Fill(dtpro);

                    for (int i = 0; i < GVItms.Rows.Count; i++)
                    {
                        DropDownList DDLPro = (DropDownList)GVItms.Rows[i].Cells[0].FindControl("DDL_Par");
                        DDLPro.DataSource = dtpro;
                        DDLPro.DataTextField = "ProductTypeName";
                        DDLPro.DataValueField = "ProductTypeID";                        
                        DDLPro.DataBind();
                        DDLPro.Items.Insert(0, new ListItem("--Select Product Type--", "0"));
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetInitRowSal()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("PARTICULARS", typeof(string)));
            dt.Columns.Add(new DataColumn("PRODUCT", typeof(string)));
            dt.Columns.Add(new DataColumn("PRODUCTNAM", typeof(string)));

            
            dt.Columns.Add(new DataColumn("DIS", typeof(string)));
            dt.Columns.Add(new DataColumn("QTY", typeof(string)));
            dt.Columns.Add(new DataColumn("QTYAVAIL", typeof(string)));
            dt.Columns.Add(new DataColumn("UNIT", typeof(string)));
            dt.Columns.Add(new DataColumn("RATE", typeof(string)));
            dt.Columns.Add(new DataColumn("AMT", typeof(string)));
            dt.Columns.Add(new DataColumn("DSal_id",typeof(string)));

            dr = dt.NewRow();
            dr["PARTICULARS"] = string.Empty;
            dr["PRODUCT"] = string.Empty;
            dr["PRODUCTNAM"] = string.Empty;

            dr["DIS"] = "0.00";
            dr["QTYAVAIL"] = "";            
            dr["QTY"] = "0.00";
            dr["UNIT"] = "0.00";
            dr["RATE"] = "0.00";
            dr["AMT"] = "0.00";
            dr["DSal_id"] = "0";

            dt.Rows.Add(dr);

            //Store the DataTable in ViewState
            ViewState["dt_adItm"] = dt;

            GVItms.DataSource = dt;
            GVItms.DataBind();
        }

        private void AddNewRow()
        {
            int rowIndex = 0;

            if (ViewState["dt_adItm"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt_adItm"];
                DataRow drRow = null;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 1; i <= dt.Rows.Count; i++)
                    {
                        //extract the Controls values
                        DropDownList DDLPro = (DropDownList)GVItms.Rows[rowIndex].Cells[0].FindControl("DDL_Par");
                        //DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[rowIndex].Cells[1].FindControl("DDL_Prorefcde");
                        TextBox TBDIS = (TextBox)GVItms.Rows[rowIndex].Cells[2].FindControl("TBDIS");
                        TextBox tbprodt = (TextBox)GVItms.Rows[rowIndex].Cells[2].FindControl("tbprodt");
                        Label lblItmQty = (Label)GVItms.Rows[rowIndex].Cells[3].FindControl("lblItmQty");
                        TextBox TBItmQty = (TextBox)GVItms.Rows[rowIndex].Cells[4].FindControl("TBItmQty");
                        TextBox TbItmunt = (TextBox)GVItms.Rows[rowIndex].Cells[5].FindControl("TbItmunt");
                        TextBox Tbrat = (TextBox)GVItms.Rows[rowIndex].Cells[6].FindControl("Tbrat");
                        TextBox Tbamt = (TextBox)GVItms.Rows[rowIndex].Cells[7].FindControl("Tbamt");
                        HiddenField HFDSal = (HiddenField)GVItms.Rows[rowIndex].Cells[8].FindControl("HFDSal");
                        Label lblPro = (Label)GVItms.Rows[rowIndex].Cells[2].FindControl("lbl_Pro");
                        //CheckBox ChkCls = (CheckBox)GVItms.Rows[rowIndex].Cells[2].FindControl("ChkClosed");


                        drRow = dt.NewRow();

                        dt.Rows[i - 1]["PARTICULARS"] = DDLPro.Text;
                        dt.Rows[i - 1]["PRODUCT"] = lblPro.Text;
                        dt.Rows[i - 1]["PRODUCTNAM"] = tbprodt.Text;
                        
                        dt.Rows[i - 1]["DIS"] = TBDIS.Text;
                        dt.Rows[i - 1]["QTYAVAIL"] = lblItmQty.Text; //QTYAVAIL
                        dt.Rows[i - 1]["QTY"] = TBItmQty.Text; //QTYAVAIL
                        dt.Rows[i - 1]["UNIT"] = TbItmunt.Text;
                        dt.Rows[i - 1]["RATE"] = Tbrat.Text;
                        dt.Rows[i - 1]["AMT"] = Tbamt.Text;
                        dt.Rows[i - 1]["DSal_id"] = HFDSal.Value;
                        //dt.Rows[i - 1]["CLOSED"] = ChkCls.Checked;

                        rowIndex++;

                        //float GTotal = 0;
                        //for (int j = 0; j < GVItms.Rows.Count; j++)
                        //{
                        //    TextBox total = (TextBox)GVItms.Rows[j].FindControl("Tbamt");
                        //    //GTotal = Convert.ToSingle(TbAddPurNetTtl.Text);
                        //    if (total.Text != "")
                        //    {
                        //        GTotal += Convert.ToSingle(total.Text);
                        //        TBTotal.Text = GTotal.ToString();
                        //        //int GST = Convert.ToInt32(TBGST.Text.Trim());
                        //        //string dis = (GTotal / GST * 100).ToString();

                        //        TBGTtl.Text = GTotal.ToString();//GTotal.ToString();
                        //    }
                        //}

                        //DDLPro.Focus();
                    }
                    dt.Rows.Add(drRow);
                    ViewState["dt_adItm"] = dt;

                    GVItms.DataSource = dt;
                    GVItms.DataBind();

                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreRowSal();
        }

        private void SetPreRowSal()
        {
            try
            {
                
                int rowIndex = 0;
                if (ViewState["dt_adItm"] != null)
                {
                    DataTable dt = (DataTable)ViewState["dt_adItm"];

                   

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList DDLPro = (DropDownList)GVItms.Rows[rowIndex].Cells[0].FindControl("DDL_Par");
                            //DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[rowIndex].Cells[1].FindControl("DDL_Prorefcde");
                            Label lbl_Pro = (Label)GVItms.Rows[rowIndex].Cells[1].FindControl("lbl_Pro");
                            TextBox TBDIS = (TextBox)GVItms.Rows[rowIndex].Cells[2].FindControl("TBDIS");
                            TextBox tbprodt = (TextBox)GVItms.Rows[rowIndex].Cells[2].FindControl("tbprodt");
                            TextBox TBItmQty = (TextBox)GVItms.Rows[rowIndex].Cells[2].FindControl("TBItmQty");
                            TextBox TbItmunt = (TextBox)GVItms.Rows[rowIndex].Cells[3].FindControl("TbItmunt");
                            TextBox Tbrat = (TextBox)GVItms.Rows[rowIndex].Cells[4].FindControl("Tbrat");
                            TextBox Tbamt = (TextBox)GVItms.Rows[rowIndex].Cells[5].FindControl("Tbamt");
                            HiddenField HFDSal = (HiddenField)GVItms.Rows[rowIndex].Cells[7].FindControl("HFDSal");
                            Label lbl_Flag = (Label)GVItms.Rows[i].FindControl("lbl_Flag");
                            Label lblItmQty = (Label)GVItms.Rows[i].FindControl("lblItmQty");

                            tbprodt.Text = dt.Rows[i]["PRODUCTNAM"].ToString();

                            DDLPro.Text = dt.Rows[i]["PARTICULARS"].ToString();

                            string DIS = dt.Rows[i]["DIS"].ToString();

                            if (DIS != "")
                            {
                                TBDIS.Text = dt.Rows[i]["DIS"].ToString();
                            }
                            else
                            {
                                TBDIS.Text = "0.00";
                            }

                            string QTYAvail = dt.Rows[i]["QTYAVAIL"].ToString();
                            if (QTYAvail != "")
                            {
                                lblItmQty.Text = dt.Rows[i]["QTYAVAIL"].ToString();
                            }
                            else
                            {
                                lblItmQty.Text = "0.00";
                            }

                            string QTY = dt.Rows[i]["QTY"].ToString();
                            if (QTY != "")
                            {
                                TBItmQty.Text = dt.Rows[i]["QTY"].ToString();
                            }
                            else
                            {
                                TBItmQty.Text = "0.00";
                            }
                            TbItmunt.Text = dt.Rows[i]["UNIT"].ToString();

                            string RATE = dt.Rows[i]["RATE"].ToString();
                            if (RATE != "")
                            {
                                Tbrat.Text = dt.Rows[i]["RATE"].ToString();
                            }
                            else
                            {
                                Tbrat.Text = "0.00";
                            }

                            string AMT = dt.Rows[i]["AMT"].ToString();
                            if (AMT != "")
                            {
                                Tbamt.Text = dt.Rows[i]["AMT"].ToString();
                            }
                            else
                            {
                                Tbamt.Text = "0.00";
                            }

                            HFDSal.Value = dt.Rows[i]["DSal_id"].ToString();

                            //if (DDL_Prorefcde.SelectedValue == "0")
                            //{
                            //    lbl_Flag.Text = "0";
                            //}
                            //else
                            //{
                            //    lbl_Flag.Text = "1";
                            //}
                            rowIndex++;

                            //DDLPro.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //lbl_err.Text = ex.Message.ToString();
            }
        }



        private void update()
        {
            string MSalId = "";
            //using (SqlConnection consnection = new SqlConnection(connectionString))
            //{
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("SalesTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                
                if (TBoutstan.Text != "0.00")
                {
                    totalprev = Convert.ToDecimal(lbl_outstan.Text) + Convert.ToDecimal(TBoutstan.Text);
                }
                else
                {
                    totalprev = Convert.ToDecimal(lbl_outstan.Text);
                }

                //Master Sales 
                command.CommandText =
                    " Update tbl_MSal set MSal_dat ='" + TBSalDat.Text + "' , MSal_Rmk ='" + TBRmk.Text.Trim() +
                    "', MSalOrdid = '" + DDL_SO.SelectedValue + "',CustomerID ='" + DDL_Cust.SelectedValue + "', CreatedBy ='" + Session["user"].ToString() +
                    "', CreatedAt='" + DateTime.Today.ToString("MM/dd/yyyy") + "', ISActive ='" + chk_Act.Checked + "', iscre='" + ckcrdt.Checked + "',iscash='" +
                    ckcsh.Checked + "',gatpassno ='" + TBGPNo.Text + "',schm ='" + TBSchm.Text + "',bons='" + TBBns.Text + "', Recovery='" + TBRecov.Text.Trim() + "', Outstanding='" + totalprev + "',username='" + Session["Username"] + "' where Msal_sono ='" + TBSalesNum.Text.Trim() + "' and MSal_id='" + HFMSal.Value.Trim() + "'";
                
                    //" ('" + TBSalesNum.Text.Trim() + "','" + TBSalDat.Text.Trim() + "','" + TBRmk.Text.Trim() + "','" + DDL_SO.SelectedValue.Trim() + "', '" + DDL_Cust.SelectedValue.Trim() + "','" + Session["user"].ToString() + "','" + DateTime.Today.ToString("MM/dd/yyyy") + "','" + chk_Act.Checked + "','" + ckcrdt.Checked + "','" + ckcsh.Checked + "','" + TBGPNo.Text.Trim() + "','" + TBSchm.Text.Trim() + "','" + TBBns.Text.Trim() + "')";
                command.ExecuteNonQuery();

                // Master Purchase " + TBSalDat.Text.Trim() + " , " + DateTime.Today + "
                command.CommandText = "select MSal_id from tbl_MSal where MSal_sono = '" + TBSalesNum.Text.Trim() + "'";

                SqlDataAdapter adp = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MSalId = dt.Rows[0]["MSal_id"].ToString();
                }

                //Detail Sales

                foreach (GridViewRow g1 in GVItms.Rows)
                {
                    string DDL_Par = (g1.FindControl("DDL_Par") as DropDownList).SelectedValue;
                    string DDL_Prorefcde = (g1.FindControl("DDL_Prorefcde") as DropDownList).SelectedValue;
                    //string TBItmDes = (g1.FindControl("TBDIS") as Label).Text;
                    string TBItmQty = (g1.FindControl("TBItmQty") as TextBox).Text;
                    string TbItmunt = (g1.FindControl("TbItmunt") as TextBox).Text;
                    string Tbrat = (g1.FindControl("Tbrat") as TextBox).Text;
                    string Tbamt = (g1.FindControl("Tbamt") as TextBox).Text;
                    string HFDSal = (g1.FindControl("HFDSal") as HiddenField).Value;

                    
                    if (HFDSal != "")
                    {
                        command.CommandText =
                        " Update tbl_DSal set DSal_ItmDes='" + TBItmDes + "', DSal_ItmQty ='" + TBItmQty + "' , DSal_ItmUnt ='" + TbItmunt +
                        "',DSal_netttl ='" + TBTotal.Text + "', DSal_ttl ='" + TBTotal.Text +
                        "', MSal_id='" + MSalId + "', ProductID ='" + DDL_Prorefcde + "', ProductTypeID='" + DDL_Par + "',Dis='" +
                        TBDIS.Text + "',rat ='" + Tbrat + "',Amt='" + Tbamt + "', GST ='" + TBGST.Text + "', GTtl ='" + TBGTtl.Text + "'where MSal_id ='" + HFMSal.Value.Trim() + "' and DSal_id='" + HFDSal + "'";

                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText =
                                              " INSERT INTO tbl_DSal (DSal_ItmDes, DSal_ItmQty, DSal_ItmUnt, DSal_salcst, DSal_netttl, DSal_ttl, MSal_id, ProductID, ProductTypeID, Dis, rat, Amt, GST, GTtl) " +
                                              " VALUES " +
                                              " ('','" + TBItmQty + "', '" + TbItmunt + "','0.00','" + TBTotal.Text + "', '" + TBTotal.Text + "', '" + MSalId + "', '" + DDL_Prorefcde + "','" + DDL_Par + "','" + TBItmDes + "','" + Tbrat + "','" + Tbamt + "','" + TBGST.Text + "','" + TBGTtl.Text + "')";
                        command.ExecuteNonQuery();
                    }                    
                }

                //Stock Update

                #region Stock Record

                //command.CommandText =
                //     " Update tbl_Mstk set Mstk_sono='', Mstk_dat='', Mstk_PurDat='', Mstk_Rmk='', ven_id='', CustomerID='', MPurID='', CreatedBy, CreatedAt, ISActive, MSal_id, CompanyId, BranchId) " +
                //     " VALUES " +
                //     " ('" + TBSalesNum.Text.Trim() + "','" + TBSalDat.Text.Trim() + "','" + TBSalDat.Text.Trim() + "','', '','" +
                //     DDL_Cust.SelectedValue.Trim() + "','','" + Session["user"].ToString() + "','" +
                //     DateTime.Today.ToString("MM/dd/yyyy") + "','" + chk_Act.Checked + "','" +
                //     MSalId + "','" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";

                //command.ExecuteNonQuery();

                //command.CommandText = "select Mstk_id from tbl_Mstk where Mstk_sono = '" + TBSalesNum.Text.Trim() + "'";

                //SqlDataAdapter stkadp = new SqlDataAdapter(command);

                //DataTable stkdt = new DataTable();
                //stkadp.Fill(stkdt);

                //if (stkdt.Rows.Count > 0)
                //{
                  //  MSTkId = stkdt.Rows[0]["Mstk_id"].ToString();
                //}

                //foreach (GridViewRow g1 in GVItms.Rows)
                //{
                //    DDL_Par = (g1.FindControl("DDL_Par") as DropDownList).SelectedValue;
                //    DDL_Prorefcde = (g1.FindControl("DDL_Prorefcde") as DropDownList).SelectedValue;
                //    //TBItmDes = (g1.FindControl("TBDIS") as Label).Text;
                //    TBItmQty = (g1.FindControl("TBItmQty") as TextBox).Text;

                //    DataTable dtstkqty = new DataTable();

                //    //Detail Stock

                //    command.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + DDL_Prorefcde + "";

                //    SqlDataAdapter Adapter = new SqlDataAdapter(command);
                //    Adapter.Fill(dtstkqty);

                //    if (dtstkqty.Rows.Count > 0)
                //    {
                //        for (int t = 0; t < dtstkqty.Rows.Count; t++)
                //        {
                //            stkqty = dtstkqty.Rows[t]["Dstk_ItmQty"].ToString();

                //            int qty = Convert.ToInt32(stkqty) - Convert.ToInt32(TBItmQty);
                //            command.CommandText = " UPDATE tbl_Dstk SET Dstk_ItmQty = " + qty + " where  ProductID = " + DDL_Prorefcde + "";
                //            command.ExecuteNonQuery();

                //        }
                //    }
                //    else
                //    {
                //        command.CommandText = " UPDATE tbl_Dstk set Dstk_ItmDes='" + TBItmDes + "', Dstk_ItmQty='" + TBItmQty + "', Dstk_Itmwght='0.00', Dstk_ItmUnt='0.00', Dstk_rat='" + Tbrat + "', Dstk_salrat='0.00', Dstk_purrat='0.00', ProductID='" + DDL_Prorefcde + "', CompanyId='" + Session["CompanyID"] + "', BranchId='" + Session["BranchID"] + "' where Mstk_id='" + MSTkId + "'" +
                                
                //        command.ExecuteNonQuery();

                //    }
                //}

                //foreach (GridViewRow g1 in GVStkItems.Rows)
                //{

                    //ddlstkItm = (g1.FindControl("ddlstkItm") as DropDownList).SelectedValue.Trim();
                    //ItmDscptin = (g1.FindControl("ItmDscptin") as TextBox).Text.Trim();
                    //ItmQty = (g1.FindControl("ItmQty") as TextBox).Text.Trim();
                    //Tbwght = (g1.FindControl("Tbwght") as TextBox).Text.Trim();
                    //Tbunts = (g1.FindControl("Tbunts") as TextBox).Text.Trim();
                    //Tbrat = (g1.FindControl("Tbrat") as TextBox).Text.Trim();
                    //Tbsalrat = (g1.FindControl("Tbsalrat") as TextBox).Text.Trim();
                    //Tbpurrat = (g1.FindControl("Tbpurrat") as TextBox).Text.Trim();
                    //lblPurItm = (g1.FindControl("lblPurItm") as Label).Text.Trim();

                    //dtstkqty = DataAccess.DBConnection.GetDataTable(cmdtxt);

                    //command.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + lblPurItm + "";

                    //SqlDataAdapter Adapter = new SqlDataAdapter(command);
                    //Adapter.Fill(dtstkqty);

                    //if (dtstkqty.Rows.Count > 0)
                    //{
                    //    for (int t = 0; t < dtstkqty.Rows.Count; t++)
                    //    {
                    //        stkqty = dtstkqty.Rows[t]["Dstk_ItmQty"].ToString();

                    //        int qty = Convert.ToInt32(stkqty) - Convert.ToInt32(ItmQty);
                    //        command.CommandText = " UPDATE tbl_Dstk SET Dstk_ItmQty = " + qty + " where  ProductID = " + ddlstkItm + "";
                    //        command.ExecuteNonQuery();

                    //    }
                    //}
                    //else
                    //{
                    //    command.CommandText = " INSERT INTO tbl_Dstk (Dstk_ItmDes, Dstk_ItmQty, Dstk_Itmwght, Dstk_ItmUnt, Dstk_rat, Dstk_salrat, Dstk_purrat, Mstk_id, ProductID) " +
                    //            " VALUES " +
                    //            " ('" + ItmDscptin + "','" + ItmQty + "', '" + Tbwght + "','" + Tbunts + "','" + Tbrat + "','" + Tbsalrat + "', '" + Tbpurrat + "', '" + MSTkId + "', '" + ddlstkItm + "')";
                    //    command.ExecuteNonQuery();

                    //}
                //}
                #endregion

                // Attempt to commit the transaction.
                transaction.Commit();

                if (chk_prtd.Checked == true)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'ReportViewer.aspx?ID=PR','_blank','height=600px,width=600px,scrollbars=1');", true);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/rpt_salinv.aspx?ID=SAL&SalID=" + MSalId + "&CUST=" + DDL_Cust.SelectedItem.Text + "','_blank','height=600px,width=600px,scrollbars=1');", true);                
                }

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
            }
        }

        private void Save()
        {
            string MSalId = "";
            decimal cre;
            decimal TBTtl_;


            //using (SqlConnection consnection = new SqlConnection(connectionString))
            //{
            con.Open();

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("SalesTrans");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                //if (TBoutstan.Text != "0.00" && TBRecov.Text == "0.00")

                if (TBRecov.Text != "0.00")
                {
                    totalprev = Convert.ToDecimal(lbl_outstan.Text) - Convert.ToDecimal(TBoutstan.Text);
                    lbl_outstan.Text = totalprev.ToString();
                }
                if (TBoutstan.Text != "0.00")
                {
                    totalprev = Convert.ToDecimal(lbl_outstan.Text) + Convert.ToDecimal(TBoutstan.Text); 
                }
                //else Outstanding  + totalprev + "','"

                //for outstanding

                #region Credit Sheets

                command.CommandText = "select CredAmt from tbl_Salcredit where CustomerID='" + HFSupAcc.Value.Trim() + "'";

                SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                DataTable dtsalcre = new DataTable();
                stksalcre.Fill(dtsalcre);

                if (dtsalcre.Rows.Count > 0)
                {
                    //double recv = Convert.ToDecimal(lblOutstan) - Convert.ToDecimal(TBRecy);
                    cre = Convert.ToDecimal(dtsalcre.Rows[0]["CredAmt"].ToString());
                    TBTtl_ = cre + Convert.ToDecimal(TBTtl.Text.Trim());
                    command.CommandText = " Update tbl_Salcredit set CredAmt = '" + TBTtl_ + "' where CustomerID='" + HFSupAcc.Value.Trim() + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = " insert into tbl_Salcredit (CustomerID,CredAmt) values('" + HFSupAcc.Value.Trim() + "','" + TBTtl.Text.Trim() + "')";
                    command.ExecuteNonQuery();
                }

                #endregion

                //command.CommandText = "  UPDATE tbl_Salcredit SET CredAmt = '" + totalprev + "' where CustomerID = '" + DDL_Cust.SelectedValue.Trim() + "'";
                
                //command.ExecuteNonQuery();


                //Master Sales
                command.CommandText =
                    " INSERT INTO tbl_MSal(MSal_sono, MSal_dat, MSal_Rmk, MSalOrdid, CustomerID, CreatedBy, CreatedAt, ISActive, iscre, iscash, " +
                    " gatpassno, schm, bons, Recovery, Outstanding,  CompanyId, BranchId, username, Booker, SalMan, custacc, gst, othtax, Amt, GTtl, Dis, SDis, SDisAmt ) " +
                    "             VALUES  ('" + TBSalesNum.Text.Trim() + "','" + TBSalDat.Text.Trim() + "','" + TBRmk.Text.Trim() + "','" 
                    + DDL_SO.SelectedValue.Trim() + "', '" + DDL_Cust.SelectedValue.Trim() + "','"
                    + Session["user"].ToString() + "','" + DateTime.Today.ToString("MM/dd/yyyy")
                    + "','" + chk_Act.Checked + "','" + ckcrdt.Checked + "','" + ckcsh.Checked + "','"
                    + TBGPNo.Text.Trim() + "','" + TBSchm.Text.Trim() + "','" + TBBns.Text.Trim() + "','" + TBRecov.Text.Trim() + "','" + totalprev + "','" + Session["CompanyID"] +
                    "','" + Session["BranchID"] + "','" + Session["Username"] + "','"+ DDL_Book.SelectedValue.Trim() + "','" +  DDL_SalMan.SelectedValue.Trim() + "','" + HFSupAcc.Value.Trim()
                    + "', '" + TBGST.Text.Trim() + "', '" + TBOthTax.Text.Trim() + "','" + TBTtl.Text + "','" + TBGTtl.Text + "','" + TBDIS.Text + "','" + TB_SDIS.Text + "','" + lbl_SdisAmt.Text+ "')";

                command.ExecuteNonQuery();

                // Master Purchase " + TBSalDat.Text.Trim() + " , " + DateTime.Today + "
                command.CommandText = "select MSal_id from tbl_MSal where MSal_sono = '" + TBSalesNum.Text.Trim() + "'";

                SqlDataAdapter adp = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MSalId = dt.Rows[0]["MSal_id"].ToString();
                }

                //Detail Sales

                foreach (GridViewRow g1 in GVItms.Rows)
                {
                    DDL_Par = (g1.FindControl("DDL_Par") as DropDownList).SelectedValue;
                    //DDL_Prorefcde = (g1.FindControl("DDL_Prorefcde") as DropDownList).SelectedValue;
                    string lbl_Pro = (g1.FindControl("lbl_Pro") as Label).Text;
                    TBItmQty = (g1.FindControl("TBItmQty") as TextBox).Text;
                    TbItmunt = (g1.FindControl("TbItmunt") as TextBox).Text;
                    Tbrat = (g1.FindControl("Tbrat") as TextBox).Text;
                    Tbamt = (g1.FindControl("Tbamt") as TextBox).Text;
                    HFDSal = (g1.FindControl("HFDSal") as HiddenField).Value;

                    command.CommandText =
                        " INSERT INTO tbl_DSal (DSal_ItmDes, DSal_ItmQty, DSal_ItmUnt, DSal_salcst, DSal_netttl, DSal_ttl, MSal_id, ProductID, ProductTypeID, Dis, rat, Amt, GST, GTtl, CompanyId, BranchId) " +
                        " VALUES " +
                        " ('','" + TBItmQty + "', '" + TbItmunt + "','0.00','" + TBTotal.Text + "', '" + TBTotal.Text + "', '" + MSalId + "', '" + lbl_Pro + "','" + DDL_Par + "','" + TBDIS.Text + "','" + Tbrat + "','" + TBTtl.Text + "','" + TBGST.Text + "','" + TBGTtl.Text + "','" + Session["CompanyID"] +
                                    "','" + Session["BranchID"] + "')";
                    command.ExecuteNonQuery();
                }

                #region Stock Record

                command.CommandText =
                     " INSERT INTO tbl_Mstk(Mstk_sono, Mstk_dat, Mstk_PurDat, Mstk_Rmk, ven_id, CustomerID, MPurID, CreatedBy, CreatedAt, ISActive, MSal_id, CompanyId, BranchId) " +
                     " VALUES " +
                     " ('" + TBSalesNum.Text.Trim() + "','" + TBSalDat.Text.Trim() + "','" + TBSalDat.Text.Trim() + "','', '','" +
                     DDL_Cust.SelectedValue.Trim() + "','','" + Session["user"].ToString() + "','" +
                     DateTime.Today.ToString("MM/dd/yyyy") + "','" + chk_Act.Checked + "','" +
                     MSalId + "','" + Session["CompanyID"] +  "','" + Session["BranchID"] + "')";

                command.ExecuteNonQuery();

                command.CommandText = "select Mstk_id from tbl_Mstk where Mstk_sono = '" + TBSalesNum.Text.Trim() + "'";

                SqlDataAdapter stkadp = new SqlDataAdapter(command);

                DataTable stkdt = new DataTable();
                stkadp.Fill(stkdt);

                if (stkdt.Rows.Count > 0)
                {
                    MSTkId = stkdt.Rows[0]["Mstk_id"].ToString();
                }

                foreach (GridViewRow g1 in GVItms.Rows)
                {
                    DDL_Par = (g1.FindControl("DDL_Par") as DropDownList).SelectedValue;
                    //DDL_Prorefcde = (g1.FindControl("DDL_Prorefcde") as DropDownList).SelectedValue;
                    string lbl_Pro = (g1.FindControl("lbl_Pro") as Label).Text;
                    TBItmQty = (g1.FindControl("TBItmQty") as TextBox).Text;
                    TBrat = (g1.FindControl("TBrat") as TextBox).Text;

                    DataTable dtstkqty = new DataTable();

                    //Detail Stock

                    command.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + lbl_Pro.Trim() + "";

                    SqlDataAdapter Adapter = new SqlDataAdapter(command);
                    Adapter.Fill(dtstkqty);

                    if (dtstkqty.Rows.Count > 0)
                    {
                        for (int t = 0; t < dtstkqty.Rows.Count; t++)
                        {
                            stkqty = dtstkqty.Rows[t]["Dstk_ItmQty"].ToString();

                            decimal qty = Convert.ToDecimal(stkqty) - Convert.ToDecimal(TBItmQty);
                            command.CommandText = " UPDATE tbl_Dstk SET Dstk_ItmQty = " + qty + " where  ProductID = " + lbl_Pro.Trim() + "";
                            command.ExecuteNonQuery();

                        }
                    }
                    else
                    {
                        command.CommandText = " INSERT INTO tbl_Dstk (Dstk_ItmDes, Dstk_ItmQty, Dstk_Itmwght, Dstk_ItmUnt, Dstk_rat, Dstk_salrat, Dstk_purrat, Mstk_id, ProductID, CompanyId, BranchId) " +
                                " VALUES " +
                                " ('" + TBItmDes + "','" + TBItmQty + "', '0.00','0.00','" + Tbrat + "','0.00', '0.00', '" + MSTkId + "', '" + lbl_Pro.Trim() + "','" + Session["CompanyID"] +
                                        "','" + Session["BranchID"] + "')";
                        command.ExecuteNonQuery();

                    }
                }
                
                #endregion

                // Attempt to commit the transaction.
                transaction.Commit();

                if (chk_prtd.Checked == true)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'rpt_salInv.aspx?ID=SAL&SalID=" + MSalId + "&CUSTID=" + DDL_Cust.SelectedValue.Trim() + "','_blank','height=600px,width=600px,scrollbars=1');", true);                

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open('Reports/rpt_salinv1.aspx?ID=SAL&SalID=" + MSalId + "&CUST=" + DDL_Cust.SelectedItem.Text + "','_blank','height=600px,width=600px,scrollbars=1');", true);                


                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'rpt_salInv.aspx?ID=PR','_blank','height=600px,width=600px,scrollbars=1');", true);
                }

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
                ptnSno();
                //Response.Redirect("frm_Sal.aspx");
            }
            //}
        }


        //public void Save()
        //{
        //    try
        //    {
        //        tbl_MSal msal = new tbl_MSal();
               
        //        msal.MSal_id = HFMSal.Value;
        //        msal.MSal_sono = string.IsNullOrEmpty(TBSalesNum.Text) ? null : TBSalesNum.Text;
        //        msal.MSal_dat = Convert.ToDateTime(string.IsNullOrEmpty(TBSalDat.Text) ? null : TBSalDat.Text);
        //        msal.MSal_Rmk = string.IsNullOrEmpty(TBRmk.Text) ? null : TBRmk.Text;
        //        msal.MSalOrdid = string.IsNullOrEmpty(DDL_SO.SelectedValue) ? null : DDL_SO.SelectedValue;
        //        msal.CustomerID = string.IsNullOrEmpty(DDL_Cust.SelectedValue) ? null : DDL_Cust.SelectedValue; 
        //        msal.CreatedBy = Session["user"].ToString();
        //        msal.CreatedAt = DateTime.Now;
        //        msal.ISActive = Convert.ToBoolean(chk_Act.Checked).ToString();
        //        msal.iscre = Convert.ToBoolean(ckcrdt.Checked).ToString();
        //        msal.iscash = Convert.ToBoolean(ckcsh.Checked).ToString();
        //        msal.gatpassno = string.IsNullOrEmpty(TBGPNo.Text) ? null : TBGPNo.Text;
        //        msal.schm = string.IsNullOrEmpty(TBSchm.Text) ? null : TBSchm.Text;
        //        msal.bons = string.IsNullOrEmpty(TBBns.Text) ? null : TBBns.Text;
                

        //        tbl_MSalManager salmanag = new tbl_MSalManager(msal);
                
        //        salmanag.Save();
                
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw;
        //        lbl_err.Text = ex.Message.ToString();
        //    }
        //}

        public void FillGrid()
        {
            try
            {
                //Sales Order
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = " SELECT ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,MSal_id,tbl_MSal.custacc, tbl_MSal.CustomerID, " +
                        " MSal_sono,CustomerName,MSal_dat,CONVERT(varchar, MSal_dat, 103) as [SalDate], " +
                        " tbl_MSal.CreatedBy, " +
                        " CONVERT(varchar, tbl_MSal.CreatedAt, 103) as [CreatDate],tbl_MSal.CreatedAt from tbl_MSal " +
                    " inner join Customers_ on tbl_MSal.CustomerID = Customers_.cust_acc where tbl_MSal.CompanyId = '" +
                    Session["CompanyID"] + "' and tbl_MSal.BranchId= '" + Session["BranchID"] + "' order by MSal_id desc";

                    cmd.Connection = con;
                    con.Open();

                    DataTable dtSal_ = new DataTable();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtSal_);

                    GVScrhMSal.DataSource = dtSal_;
                    GVScrhMSal.DataBind();
                    ViewState["GetSal"] = dtSal_;

                    con.Close();
                }


            }
            catch (Exception ex)
            {
                throw ex;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lbl_Heading.Text = "Error!";
                //lblalert.Text = ex.Message;
            }
            try
            {
                
                //dtSal_ = tbl_MSalManager.GetMSalList();

                
            }
            catch (Exception ex)
            {
                //throw;
                throw ex;
            }
        }

        //private void SavDet()
        //{
        //    try
        //    {
        //        string MSalid = "";

        //        string cmds = "select max(cast(MSal_id as int)) as [MSal_id] from tbl_MSal";

        //        SqlCommand cmd = new SqlCommand(cmds, con);
                

        //        DataTable dt_ = new DataTable();
        //        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //        adp.Fill(dt_);
        //        {
        //            if (HFMSal.Value == "")
        //            {
        //                MSalid = dt_.Rows[0]["MSal_id"].ToString();
        //            }
        //            else if (HFMSal.Value != "")
        //            {
        //                MSalid = HFMSal.Value.Trim();
        //            }
        //        }
        //        if (dt_.Rows.Count > 0)
        //        {
        //            foreach (GridViewRow g1 in GVItms.Rows)
        //            {
        //                string DDL_Par = (g1.FindControl("DDL_Par") as DropDownList).SelectedValue;
        //                string DDL_Prorefcde = (g1.FindControl("DDL_Prorefcde") as DropDownList).SelectedValue;
        //                string TBItmDes = (g1.FindControl("TBDIS") as TextBox).Text;
        //                string TBItmQty = (g1.FindControl("TBItmQty") as TextBox).Text;
        //                string TbItmunt = (g1.FindControl("TbItmunt") as TextBox).Text;
        //                string Tbrat = (g1.FindControl("Tbrat") as TextBox).Text;
        //                string Tbamt = (g1.FindControl("Tbamt") as TextBox).Text;


        //                tbl_DSal dsal = new tbl_DSal();

        //                dsal.DSal_id = HFDSal.Value;
        //                dsal.MSal_id = MSalid;
        //                dsal.ProductID = string.IsNullOrEmpty(DDL_Par) ? null : DDL_Par;
        //                dsal.ProductTypeID = string.IsNullOrEmpty(DDL_Prorefcde) ? null : DDL_Prorefcde;
        //                dsal.DSal_ItmDes = string.IsNullOrEmpty(TBItmDes) ? null : TBItmDes;
        //                dsal.DSal_ItmQty = string.IsNullOrEmpty(TBItmQty) ? null : TBItmQty;
        //                dsal.DSal_ItmUnt = string.IsNullOrEmpty(TbItmunt) ? null : TbItmunt;
        //                dsal.DSal_salcst = string.IsNullOrEmpty(Tbrat) ? null : Tbrat;
        //                dsal.Dis =  string.IsNullOrEmpty(TBItmDes) ? null : TBItmDes;
        //                dsal.rat = string.IsNullOrEmpty(Tbrat) ? null : Tbrat;
        //                dsal.Amt = string.IsNullOrEmpty(Tbamt) ? null : Tbamt;                        
        //                dsal.DSal_netttl = string.IsNullOrEmpty(Tbamt) ? null : Tbamt;
        //                dsal.DSal_ttl = string.IsNullOrEmpty(TBTotal.Text) ? null : TBTotal.Text;
        //                dsal.GST = string.IsNullOrEmpty(TBGST.Text) ? null : TBGST.Text;
        //                dsal.GTtl = string.IsNullOrEmpty(TBGTtl.Text) ? null : TBGTtl.Text;                        

        //                tbl_DSalManager dsalmanag = new tbl_DSalManager(dsal);
        //                dsalmanag.Save();
        //            }
        //        }
        //        else
        //        {
        //            lbl_err.Text = "Sory Record has not been Saved! Inner Error";
        //        }
                

        //    }
        //    catch (Exception ex)
        //    {
        //        lbl_err.Text = ex.Message.ToString();
        //    }

        //}
        #endregion
        protected void TBSearchSalesNum_TextChanged(object sender, EventArgs e)
        {
            try
            {  
                if (TBSearchSales.Text != "")
                {
                    DataTable dt_sal = new DataTable();
                    //dt_sal = tbl_MSalManager.GetMSalList(TBSearchSales.Text.Trim());// MPurchaseManager.GetMPurList(TBSearchSales.Text);
                    string queryString = " select MSal_id,MSal_sono,tbl_MSal.CustomerID,CustomerName,MSal_dat as [SalDate],tbl_MSal.CreatedBy,tbl_MSal.CreatedAt as [CreatDate] from tbl_MSal " +
                   " inner join Customers_ on tbl_MSal.CustomerID = Customers_.CustomerID where CustomerName = '" + TBSearchSales.Text.Trim() + "'";

                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter(queryString, con);
                    da.Fill(dt_sal);

                    con.Close();
                    GVScrhMSal.DataSource = dt_sal;
                    GVScrhMSal.DataBind();
                }
                else if (TBSearchSales.Text == "")
                {
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                //lblalert.Text = ex.Message;
            }
        }

        protected void GVScrhMSal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVScrhMSal.PageIndex = e.NewPageIndex;
            FillGrid();
        }

        protected void GVScrhMSal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row;

            try
            {
                if (e.CommandName == "Select")
                {   
                    row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    
                    string SID = GVScrhMSal.DataKeys[row.RowIndex].Values[0].ToString();
                    HFMSal.Value = SID;


                    string query1 = " select MSal_id,MSal_sono,CustomerID,gatpassno, Booker, SalMan, Outstanding, Recovery,  convert(date, cast(MSal_dat as date) ,105) as [MSal_dat],iscash, iscre,MSal_Rmk, " +
                        " MSalOrdid,CustomerID,ISActive from tbl_MSal where MSal_id = '" + HFMSal.Value + "'";

                    DataTable dtMSal = new DataTable();
                    SqlCommand cmd = new SqlCommand(query1, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtMSal);

                    if (dtMSal.Rows.Count > 0)
                    {
                        HFMSal.Value = dtMSal.Rows[0]["MSal_id"].ToString();
                        chk_Act.Checked = Convert.ToBoolean(dtMSal.Rows[0]["ISActive"].ToString());
                        DDL_SO.SelectedValue = dtMSal.Rows[0]["MSalOrdid"].ToString();
                        DDL_Cust.SelectedValue = dtMSal.Rows[0]["CustomerID"].ToString();
                        TBSalDat.Text = dtMSal.Rows[0]["MSal_dat"].ToString();
                        TBSalesNum.Text = dtMSal.Rows[0]["MSal_sono"].ToString();
                        TBRmk.Text = dtMSal.Rows[0]["MSal_Rmk"].ToString();
                        TBGPNo.Text = dtMSal.Rows[0]["gatpassno"].ToString();
                        ckcsh.Checked = Convert.ToBoolean(dtMSal.Rows[0]["iscash"]);
                        ckcrdt.Checked = Convert.ToBoolean(dtMSal.Rows[0]["iscre"]);
                        TBoutstan.Text = dtMSal.Rows[0]["Outstanding"].ToString();
                        if (TBoutstan.Text == "")
                        {
                            TBoutstan.Text = "0.00";
                        }
                        TBRecov.Text = dtMSal.Rows[0]["Recovery"].ToString();
                        if (TBRecov.Text == "")
                        {
                            TBRecov.Text = "0.00";
                        }
                        DDL_Book.SelectedValue = dtMSal.Rows[0]["Booker"].ToString();
                        DDL_SalMan.SelectedValue = dtMSal.Rows[0]["SalMan"].ToString();

                        //lbl_outstan.Text = "0.00";
                    }

                    //string query2 = " select tbl_DSal.ProductTypeID as [PARTICULARS] ,tbl_DSal.ProductID as [PRODUCT] , " +
                    //    " isnull(0.0, DSal_ItmDes) as [DIS],DSal_ItmQty as [QTY],DSal_ItmUnt as [UNIT],GTtl , tbl_DSal.Dsal_id,DSal_salcst as [RATE],DSal_netttl as [AMT],DSal_ttl " +
                    //    " from tbl_DSal inner join Products on tbl_DSal.ProductID =  Products.ProductID " +
                    //    " where MSal_id = '" + HFMSal.Value + "'";

                    string query2 = " select tbl_DSal.ProductTypeID as [PARTICULARS] ,tbl_DSal.ProductID as [PRODUCT] , " +
                        " DIS,DSal_ItmQty as [QTY],DSal_ItmUnt as [UNIT],GTtl ,tbl_Dstk.Dstk_ItmQty as [QTYAVAIL], " +
                        " tbl_DSal.Dsal_id, rat as [RATE],Amt as [AMT], GTtl  from tbl_DSal  inner join Products " +
                        " on tbl_DSal.ProductID =  Products.ProductID " +
                        " inner join tbl_Dstk on Products.ProductID = tbl_Dstk.ProductID " +
                        " where  MSal_id = '" + HFMSal.Value + "'";

                    DataTable dtDSO = new DataTable();
                    SqlCommand cmdcn = new SqlCommand(query2, con);
                    SqlDataAdapter adpcn = new SqlDataAdapter(cmdcn);
                    adpcn.Fill(dtDSO);

                    GVItms.DataSource = dtDSO;
                    GVItms.DataBind();

                    ViewState["dt_adItm"] = dtDSO;

                    
                    for (int k = 0; k < GVItms.Rows.Count; k++)
                    {
                        DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[k].Cells[1].FindControl("DDL_Prorefcde");
                        DropDownList DDL_Par = (DropDownList)GVItms.Rows[k].Cells[1].FindControl("DDL_Par");
                        Label lbl_Par = (Label)GVItms.Rows[k].FindControl("lbl_Par");
                        Label lbl_Pro = (Label)GVItms.Rows[k].FindControl("lbl_Pro");

                        for (int j = 0; j < dtDSO.Rows.Count; j++)
                        {
                            DDL_Prorefcde.SelectedValue = lbl_Pro.Text;
                            DDL_Par.SelectedValue = lbl_Par.Text;
                        }
                    }
                    if (dtDSO.Rows.Count > 0)
                    {
                        for (int j = 0; j < dtDSO.Rows.Count; j++)
                        {
                            TBTotal.Text = dtDSO.Rows[j]["PRODUCT"].ToString(); 
                            TBGTtl.Text = dtDSO.Rows[j]["GTtl"].ToString();
                            TBDIS.Text = dtDSO.Rows[j]["DIS"].ToString();
                            TBTtl.Text = dtDSO.Rows[j]["AMT"].ToString();
                        }
                    }
                #region OLD LOGIC
                    
                                //if (dtDSO.Rows.Count > 0)
                                //{
                                //    int rowIndex = 0;
                                //    for (int k = 0; k < GVItms.Rows.Count; k++)
                                //    {
                                //        DropDownList DDL_Par = (DropDownList)GVItms.Rows[k].Cells[0].FindControl("DDL_Par");
                                //        DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[k].Cells[1].FindControl("DDL_Prorefcde");
                                //        Label lbl_Par = (Label)GVItms.Rows[k].FindControl("lbl_Par");

                                //        for (int j = 0; j < dtDSO.Rows.Count; j++)
                                //        {
                                //            //DDL_Par.SelectedValue = dtDSO.Rows[j]["ProductTypeID"].ToString();
                                //            lbl_Par.Text = dtDSO.Rows[j]["PRODUCT"].ToString();
                                //            //getPro(DDL_Par.SelectedValue.Trim());
                                //            //DDL_Prorefcde.SelectedValue = dtDSO.Rows[j]["ProductID"].ToString();
                                //        }
                                //        //BindPar();
                                //        //DDL_Par.SelectedValue = dtDSO.Rows[j]["ProductTypeID"].ToString();
                                //    }
                                //    rowIndex++;
                                //}
                                //else
                                //{
                                //    SetInitRowSal();
                                //}
            #endregion
                }
                if (e.CommandName == "Show")
                {
                    row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string SalID = GVScrhMSal.DataKeys[row.RowIndex].Values[0].ToString();
                    string CUST = GVScrhMSal.Rows[row.RowIndex].Cells[1].Text;
                    //string CUSTID = GVScrhMSal.DataKeys[row.RowIndex].Values[1].ToString();

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'rpt_salInv.aspx?ID=SAL&SalID=" + SalID + "','_blank','height=600px,width=600px,scrollbars=1');", true);                
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/rpt_salinv1.aspx?ID=SAL&SalID=" + SalID + "&CUST=" + CUST + "','_blank','height=600px,width=600px,scrollbars=1');", true);                
                }
            }
            catch (Exception ex)
            {
                throw ex;                
            }
        }

        protected void GVScrhMSal_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string MSalSNO = Server.HtmlDecode(GVScrhMSal.Rows[e.RowIndex].Cells[0].Text.ToString());

                SqlCommand cmd = new SqlCommand();

                cmd = new SqlCommand("sp_del_Sal", con);
                cmd.Parameters.Add("@MSalsono", SqlDbType.VarChar).Value = MSalSNO;
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                lbl_err.Text = "Sales # " + MSalSNO + " has been Deleted!";
                FillGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        protected void GVScrhMSal_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            GVScrhMSal.PageIndex = e.NewSelectedIndex;
            FillGrid();
        }

        protected void ddl_Cust_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {
                SqlCommand cmd;
                SqlDataAdapter adp;
                string query;

                DataTable dtout_ = new DataTable();

                query = "select * from tbl_Salcredit where CustomerID ='"  + DDL_Cust.SelectedValue.Trim() +"'";
                 cmd = new SqlCommand(query, con);

                 adp = new SqlDataAdapter(cmd);

                adp.Fill(dtout_);

                if (dtout_.Rows.Count > 0)
                {
                    v_custumer.Text = "";
                    lbl_outstan.Text = dtout_.Rows[0]["CredAmt"].ToString();
                }
                else
                {
                    lbl_outstan.Text = "0.00";
                }

                query = " select saleper  from Customers_ where CustomerID = " + DDL_Cust.SelectedValue.Trim() +"";

                DataTable dtper = new DataTable();
                cmd = new SqlCommand(query, con);

                adp = new SqlDataAdapter(cmd);

                adp.Fill(dtper);

                if (dtper.Rows.Count > 0)
                {
                   // v_salmen.Text = "";
                    TBDIS.Text = dtper.Rows[0]["saleper"].ToString();
                    
                    if (TBDIS.Text == "")
                    {
                       // v_brooker.Text = "";
                        TBDIS.Text = "0.00";
                    }
                }
                else
                {
                    TBDIS.Text = "0.00";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            } 
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (HFMSal.Value == "")
                {
                    if (TBSalDat.Text == "")
                    {
                        lbl_Cust.Visible = false;
                        lbl_book.Visible = false;
                        lbl_Gttl.Visible = false;
                        lbl_ttl.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Itmqty.Visible = false;
                        lbl_Pros.Visible = false; 
                        
                        lbl_saldat.Visible = true;
                        lbl_saldat.Text = "Please Select Date...";

                    }
                    else if (DDL_Cust.SelectedValue == "0")
                    {
                        lbl_saldat.Visible = false;
                        lbl_book.Visible = false;
                        lbl_Gttl.Visible = false;
                        lbl_ttl.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Itmqty.Visible = false;
                        lbl_Pros.Visible = false;

                        lbl_Cust.Visible = true;
                        lbl_Cust.Text = "Please Select Customer...";
                    }
                    else if (DDL_Book.SelectedValue == "0")
                    {
                        lbl_saldat.Visible = false;
                        lbl_Cust.Visible = false;
                        lbl_Gttl.Visible = false;
                        lbl_ttl.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Itmqty.Visible = false;
                        lbl_Pros.Visible = false;

                        lbl_book.Visible = true;
                        lbl_book.Text = "Please Select Booker...";
                    }
                    else if (DDL_SalMan.SelectedValue == "0")
                    {
                        lbl_saldat.Visible = false;
                        lbl_Cust.Visible = false;
                        lbl_book.Visible = false;
                        lbl_Gttl.Visible = false;
                        lbl_ttl.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Itmqty.Visible = false;
                        lbl_Pros.Visible = false;

                        lbl_SalMan.Visible = true;
                        lbl_SalMan.Text = "Please Select SalesMan...";
                    }
                    else
                    {
                        lbl_saldat.Visible = false;
                        lbl_Cust.Visible = false;
                        lbl_book.Visible = false;
                        lbl_SalMan.Visible = false;
                        lbl_Gttl.Visible = false;
                        lbl_ttl.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chksalrat.Visible = false;
                        lbl_Itmqty.Visible = false;
                        lbl_Pros.Visible = false;

                        chkdetails = 0;

                        for (int j = 0; j < GVItms.Rows.Count; j++)
                        {
                            TextBox tbprodt = (TextBox)GVItms.Rows[j].FindControl("tbprodt");
                            TextBox TBItmQty = (TextBox)GVItms.Rows[j].FindControl("TBItmQty");
                            TextBox TBrat = (TextBox)GVItms.Rows[j].FindControl("TBrat");
                            TextBox TBamt = (TextBox)GVItms.Rows[j].FindControl("TBamt");

                            if (tbprodt.Text == "")
                            {
                                lbl_Pros.Text = "Please Select Product!!";
                                lbl_Pros.Visible = true;

                                lbl_saldat.Visible = false;
                                lbl_Cust.Visible = false;
                                lbl_book.Visible = false;
                                lbl_SalMan.Visible = false;
                                lbl_Gttl.Visible = false;
                                lbl_ttl.Visible = false;
                            }
                            else if (TBItmQty.Text == "" || TBItmQty.Text == "0.00" || TBItmQty.Text == "0")
                            {
                                lbl_Itmqty.Text = "Please Write Quantity!!";
                                lbl_Itmqty.Visible = true;
                                lbl_Pros.Visible = false;

                                lbl_saldat.Visible = false;
                                lbl_Cust.Visible = false;
                                lbl_book.Visible = false;
                                lbl_SalMan.Visible = false;
                                lbl_Gttl.Visible = false;
                                lbl_ttl.Visible = false;
                            }
                            else if (TBrat.Text == "" || TBrat.Text == "0.00" || TBrat.Text == "0")
                            {
                                lbl_Chksalrat.Text = "Please Write Sale Rate!!";
                                lbl_Chksalrat.Visible = true;
                                lbl_Itmqty.Visible = false;
                                lbl_Pros.Visible = false;

                                lbl_saldat.Visible = false;
                                lbl_Cust.Visible = false;
                                lbl_book.Visible = false;
                                lbl_SalMan.Visible = false;
                                lbl_Gttl.Visible = false;
                                lbl_ttl.Visible = false;
                            }
                            else if (TBamt.Text == "" || TBamt.Text == "0.00" || TBamt.Text == "0")
                            {
                                lbl_ChkAmt.Text = "Please Write Amount!!";
                                lbl_ChkAmt.Visible = true;
                                lbl_Chksalrat.Visible = false;
                                lbl_Itmqty.Visible = false;
                                lbl_Pros.Visible = false;

                                lbl_saldat.Visible = false;
                                lbl_Cust.Visible = false;
                                lbl_book.Visible = false;
                                lbl_SalMan.Visible = false;
                                lbl_Gttl.Visible = false;
                                lbl_ttl.Visible = false;
                            }
                            else if (TBGTtl.Text == "" || TBGTtl.Text == "0" || TBGTtl.Text == "0.00")
                            {

                                lbl_saldat.Visible = false;
                                lbl_Cust.Visible = false;
                                lbl_book.Visible = false;
                                lbl_SalMan.Visible = false;
                                lbl_ttl.Visible = false;
                                lbl_ChkAmt.Visible = false;
                                lbl_Chksalrat.Visible = false;
                                lbl_Itmqty.Visible = false;
                                lbl_Pros.Visible = false;

                                lbl_Gttl.Visible = true;
                                lbl_Gttl.Text = "Please Write Some thing in Grand Total...";
                            }
                            else if (TBTtl.Text == "" || TBTtl.Text == "0.00" || TBTtl.Text == "0")
                            {
                                lbl_saldat.Visible = false;
                                lbl_Cust.Visible = false;
                                lbl_book.Visible = false;
                                lbl_SalMan.Visible = false;
                                lbl_Gttl.Visible = false;
                                lbl_ChkAmt.Visible = false;
                                lbl_Chksalrat.Visible = false;
                                lbl_Itmqty.Visible = false;
                                lbl_Pros.Visible = false;


                                lbl_ttl.Visible = true;
                                lbl_ttl.Text = "Please Write Some thing in Total...";
                            }
                     

                            else
                            {
                                chkdetails = 1;
                            }

                              
                        }
                        if (chkdetails == 1)
                        {
                            Save();
                        }
                    }
                }
                else if (HFMSal.Value != "")
                {
                    update();
                }
                //using (SqlCommand cmdsal = new SqlCommand())
                //{
                //    con.Close();
                //    //cmdpro.CommandText = " select rtrim('[' + CAST(ProductID AS VARCHAR(200)) + ']-' + ProductName ) as [ProductName], ProductID from Products ";
                //    cmdsal.CommandText = " select MSal_id, MSal_sono from tbl_MSal where MSal_sono = '" + TBSalesNum.Text.Trim() + "'";

                //    cmdsal.Connection = con;
                //    con.Open();

                //    DataTable dtsal = new DataTable();
                //    SqlDataAdapter adp = new SqlDataAdapter(cmdsal);
                //    adp.Fill(dtsal);
                //    if (dtsal.Rows.Count > 0)
                //    {
                //        SavDet();
                //    }
                //    else
                //    {
                //        cmdsal.CommandText = " Delete from tbl_MSal where MSal_sono = '" + TBSalesNum.Text.Trim() + "'";
                //        cmdsal.ExecuteNonQuery();

                //    }
                //    con.Close();
                //}
                //if (chk_prtd.Checked == true)
                //{
                //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'ReportViewer.aspx?ID=ReqReport&REQID=" + txtsono.Text.Trim() + "','_blank','height=600px,width=600px,scrollbars=1');", true);
                //}
                //FillGrid();
                //Clear();
                //ptnSno();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnRevert_Click(object sender, EventArgs e)
        {
            //Clear();
            Response.Redirect("frm_Sal.aspx");
        }

        protected void GVItms_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (ViewState["dt_adItm"] != null)
            {
                DataTable dt = (DataTable)ViewState["dt_adItm"];
                DataRow drCurrentRow = null;
                int rowIndex = Convert.ToInt32(e.RowIndex);
                if (dt.Rows.Count > 1)
                {
                    dt.Rows.Remove(dt.Rows[rowIndex]);
                    drCurrentRow = dt.NewRow();
                    ViewState["dt_adItm"] = dt;

                    GVItms.DataSource = dt;
                    GVItms.DataBind();

                    SetPreRowSal();

                    decimal GTotal = 0;
                    for (int j = 0; j < GVItms.Rows.Count; j++)
                    {
                        TextBox total = (TextBox)GVItms.Rows[j].FindControl("Tbamt");

                        GTotal += Convert.ToDecimal(total.Text);
                    }
                    TBTotal.Text = GTotal.ToString();
                    TBGTtl.Text = GTotal.ToString();

                    ptnSno();
                }
            }
        }

        protected void linkbtnadd_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }

        protected void TBItmQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (int j = 0; j < GVItms.Rows.Count; j++)
                {
                    TextBox TBItmQty = (TextBox)GVItms.Rows[j].FindControl("TBItmQty");
                    Label lbshw = (Label)GVItms.Rows[j].FindControl("lbshw");
                    TextBox TbItmCst = (TextBox)GVItms.Rows[j].FindControl("Tbrat");
                    TextBox Tbamt = (TextBox)GVItms.Rows[j].FindControl("Tbamt");
                    //DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[j].FindControl("DDL_Prorefcde");
                    Label Isupdat =(Label)GVItms.Rows[j].FindControl("Isupdat");
                    LinkButton lnkbtnadd = (LinkButton)GVItms.Rows[j].FindControl("lnkbtnadd");
                    Label lbl_Pro = (Label)GVItms.Rows[j].FindControl("lbl_Pro");

                    DataTable stkqty = new DataTable();

                    SqlCommand commnd = new SqlCommand();
                    commnd.Connection = con;
                    commnd.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + lbl_Pro.Text.Trim() + "";
                    con.Open();
                    SqlDataAdapter Adapter = new SqlDataAdapter(commnd);
                    Adapter.Fill(stkqty);

                    if (stkqty.Rows.Count > 0)
                    {
                        for (int t = 0; t < stkqty.Rows.Count; t++)
                        {
                            decimal stkquanty = Convert.ToDecimal(stkqty.Rows[t]["Dstk_ItmQty"]);
                            decimal qty = Convert.ToDecimal(TBItmQty.Text);
                            if (qty > stkquanty)
                            {
                                lbshw.Text = "Item Qunatity is less in Store!!";
                                lbshw.ForeColor = System.Drawing.Color.Red;
                                TbItmCst.Enabled = false;
                                Tbamt.Enabled = false;
                                Tbamt.Text = "0.00";
                                lnkbtnadd.Enabled = false;
                                btnSave.Enabled = false;
                                
                            }
                            else
                            {
                                lbshw.Text = "";
                                TbItmCst.Enabled = true;
                                Tbamt.Enabled = true;
                                lnkbtnadd.Enabled = true;
                                btnSave.Enabled = true;

                                decimal result = (Convert.ToDecimal(TBItmQty.Text.Trim()) * Convert.ToDecimal(TbItmCst.Text));
                                Tbamt.Text = result.ToString();


                                decimal GTotal = 0;
                                for (int k = 0; k < GVItms.Rows.Count; k++)
                                {
                                    TextBox total = (TextBox)GVItms.Rows[k].FindControl("TBAmt");
                                    GTotal += Convert.ToDecimal(total.Text);
                                    TBGTtl.Text = GTotal.ToString();
                                    TBTtl.Text = Tbamt.Text;
                                }
                                Isupdat.Text = "1";
                            }
                        }
                    }
                    con.Close();
                }
                if (TBDIS.Text != "0.00")
                {
                    decimal discount = Convert.ToDecimal(TBGTtl.Text) * Convert.ToDecimal(TBDIS.Text.Trim()) / 100;
                    TBTtl.Text = (Convert.ToDecimal(TBGTtl.Text) - discount).ToString();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        protected void GV_DSR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GV_DSR.PageIndex = e.NewPageIndex;
            FillGrid(tb_cust.Text.Trim(),TBSalDat.Text.Trim());
        }
        protected void GV_DSR_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                double result;
                float GTotal = 0;

                if (e.CommandName == "Select")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    //For Main

                    string DSRID = GV_DSR.DataKeys[row.RowIndex].Values[0].ToString();

                    query = " select tbl_Mdsr.dsrid, tbl_ddsr.ProductTypeID,tbl_Mdsr.CustomerID,tbl_Mdsr.saleper,tbl_ddsr.recvry, " +
                        " CustomerName, dsrrmk from tbl_MDSR inner join tbl_ddsr  on tbl_Mdsr.dsrid = tbl_ddsr.dsrid " +
                        " inner join Customers_ on tbl_Mdsr.CustomerID = Customers_.cust_acc " +
                        " where tbl_Mdsr.dsrid='" + DSRID + "'";


                    DataTable dt_ = new DataTable();
                    dt_ = DataAccess.DBConnection.GetDataTable(query);

                    if (dt_.Rows.Count > 0)
                    {
                        HFDSRID.Value = dt_.Rows[0]["dsrid"].ToString();
                        DDL_Cust.SelectedValue = dt_.Rows[0]["CustomerID"].ToString();
                        TBDIS.Text = dt_.Rows[0]["saleper"].ToString();
                        TBRecov.Text = dt_.Rows[0]["recvry"].ToString();
                     
                        //Sale Percent

                        query = "select * from tbl_Salcredit where CustomerID ='" + DDL_Cust.SelectedValue.Trim() + "'";

                        DataTable dtsalcre = new DataTable();

                        dtsalcre = DBConnection.GetQueryData(query);

                        if (dtsalcre.Rows.Count > 0)
                        {
                            lbl_outstan.Text = dtsalcre.Rows[0]["CredAmt"].ToString();
                        }
                        else
                        {
                            lbl_outstan.Text = "0.00";
                        }

                        TBRmk.Text = dt_.Rows[0]["dsrrmk"].ToString();
                    }

                    //for account

                    query = " select subheadcategoryfourGeneratedID,subheadcategoryfourName from subheadcategoryfour where subheadcategoryfourName='" + tb_cust.Text.Trim() + "' and  SubHeadCategoriesGeneratedID='MB00004'";

                    dt_ = DBConnection.GetQueryData(query);

                    if (dt_.Rows.Count > 0)
                    {
                        //DDL_CustAcc.SelectedValue = dt_.Rows[0]["subheadcategoryfourGeneratedID"].ToString();
                        //TBCust.Text = dt_.Rows[0]["subheadcategoryfourName"].ToString();
                    }

                    //query = " select  '' as [PARTICULARS], tbl_Dstk.ProductID as [PRODUCT], " +
                    //    "  '' as [DIS], tbl_Dstk.Dstk_ItmQty as [QTYAVAIL], tbl_DDSR.Qty as [QTY], " +
                    //    " Products.Unit as [UNIT],tbl_DDSR.ProductID,  tbl_DDSR.salrat as [RATE], productname as [PRODUCTNAM], '' as [DSal_id], " +
                    //    " Amt as [AMT], ttlamt from tbl_MDSR  inner join tbl_DDSR on " +
                    //    "  tbl_MDSR.dsrid = tbl_DDSR.dsrid  inner join Products on  tbl_DDSR.ProductID = Products.ProductID  " +
                    //    " left join  tbl_Dstk on Products.ProductID = tbl_Dstk.ProductID  where tbl_MDSR.dsrid='" + DSRID + "'";


                    query = " select  '' as [PARTICULARS],tbl_Dstk.ProductID as [PRODUCT],'' as [DIS], " +
                        " tbl_Dstk.Dstk_ItmQty as [QTYAVAIL] , isnull(tbl_DDSR.finlqry,qty) as [QTY],  tbl_DDSR.salrat as [RATE],'' as [DSal_id], " +
                        " ProductName as [PRODUCTNAM], '' as [UNIT], Amt as [AMT], ttlamt from tbl_Mdsr  inner join tbl_ddsr " +
                        " on tbl_Mdsr.dsrid = tbl_ddsr.dsrid inner join tbl_Dstk on tbl_ddsr.ProductID = tbl_Dstk.ProductID " +
                        " inner join Products on tbl_Dstk.ProductID  = Products.ProductID " +
                        " where tbl_MDSR.dsrid='" + DSRID + "'";

                    dt_ = DataAccess.DBConnection.GetDataTable(query);

                    if (dt_.Rows.Count > 0)
                    {
                        GVItms.DataSource = dt_;
                        GVItms.DataBind();


                        ViewState["dt_adItm"] = dt_;
                        //TBTtl.Text = dt_.Rows[0]["ttlamt"].ToString();

                        TBTtl.Text = dt_.Rows[0]["ttlamt"].ToString();

                        for (int i = 0; i < dt_.Rows.Count; i++)
                        {

                        for (int j = 0; j < GVItms.Rows.Count; j++)
                        {
                            Label lbl_Pro = (Label)GVItms.Rows[j].FindControl("lbl_Pro");
                            //DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[j].FindControl("DDL_Prorefcde");
                            TextBox tbprodt = (TextBox)GVItms.Rows[j].FindControl("tbprodt");
                            TextBox TBItmQty = (TextBox)GVItms.Rows[j].FindControl("TBItmQty");
                            Label lbshw = (Label)GVItms.Rows[j].FindControl("lbshw");
                            TextBox TbItmCst = (TextBox)GVItms.Rows[j].FindControl("Tbrat");
                            TextBox Tbamt = (TextBox)GVItms.Rows[j].FindControl("Tbamt");
                            Label Isupdat = (Label)GVItms.Rows[j].FindControl("Isupdat");
                            LinkButton lnkbtnadd = (LinkButton)GVItms.Rows[j].FindControl("lnkbtnadd");
                            Label lblPro = (Label)GVItms.Rows[j].FindControl("lbl_Pro");
                            
                            //DDL_Prorefcde.SelectedValue = lbl_Pro.Text.Trim();

                           
                            DataTable stkqty = new DataTable();

                            SqlCommand commnd = new SqlCommand();
                            commnd.Connection = con;
                            commnd.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + lblPro.Text.Trim() + "";
                            con.Open();
                            SqlDataAdapter Adapter = new SqlDataAdapter(commnd);
                            Adapter.Fill(stkqty);

                            if (stkqty.Rows.Count > 0)
                            {
                                for (int t = 0; t < stkqty.Rows.Count; t++)
                                {
                                    decimal stkquanty = Convert.ToDecimal(stkqty.Rows[t]["Dstk_ItmQty"]);
                                    decimal qty = Convert.ToDecimal(TBItmQty.Text);
                                    if (qty > stkquanty)
                                    {
                                        lbshw.Text = "Item Qunatity is less in Store!!";
                                        lbshw.ForeColor = System.Drawing.Color.Red;
                                        TbItmCst.Enabled = false;
                                        Tbamt.Enabled = false;
                                        Tbamt.Text = "0.00";
                                        lnkbtnadd.Enabled = false;
                                        btnSave.Enabled = false;

                                    }
                                    else
                                    {
                                        lbshw.Text = "";
                                        TbItmCst.Enabled = true;
                                        Tbamt.Enabled = true;
                                        lnkbtnadd.Enabled = true;
                                        btnSave.Enabled = true;
                                        Isupdat.Text = "1";
                                    }
                                }
                            }
                            con.Close();
                            
                            }
                        }

                        for (int k = 0; k < GVItms.Rows.Count; k++)
                        {
                            TextBox total = (TextBox)GVItms.Rows[k].FindControl("TBAmt");
                            GTotal += Convert.ToSingle(total.Text);

                        }
                        TBGTtl.Text = GTotal.ToString();                         

                        if ((TBDIS.Text != "0.00") || (TBDIS.Text != "0"))
                        {

                            decimal discount = Convert.ToDecimal(TBGTtl.Text.Trim()) * Convert.ToDecimal(TBDIS.Text.Trim()) / 100;

                            lbl_discamt.Text = discount.ToString();

                        }
                        else
                        {
                            //TBTtl.Text = TBGTtl.Text.Trim().ToString();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }
        }


        public void Clear()
        {
            SetInitRowSal();
            ptnSno();
            BindPar();
            TBSalDat.Text = DateTime.Now.ToShortDateString();
            TBRmk.Text = "";
            chk_prtd.Checked = true;
            TBTotal.Text = "";
            TBGPNo.Text = "GP000";
            DDL_Cust.SelectedValue = "0";
            TBDIS.Text = "0.00";
            TBGTtl.Text = "0.00";
            TBRecov.Text = "0.00";
            TBoutstan.Text = "0.00";
            TBTtl.Text = "0.00";
            HFMSal.Value = "";
            FillGrid();
            DDL_SalMan.SelectedValue = "0";
            DDL_Book.SelectedValue = "0";
            TB_SDIS.Text = "0.00";
            lbl_SdisAmt.Text = "0.00";
            TB_ttl.Text = "0.00";
        }

        protected void chk_SO_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_SO.Checked == true)
            {
                pnlSO.Visible = true;
            }
            else if (chk_SO.Checked == false)
            {
                pnlSO.Visible = false; 
            }
        }

        protected void ckSch_CheckedChanged(object sender, EventArgs e)
        {
            if (ckSch.Checked == true)
            {
                pnl_sch.Visible = true;
            }
            else if (ckSch.Checked == false)
            {
                pnl_sch.Visible = false;
            }
        }

        protected void DDL_Par_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
                {
                    DropDownList ddl = (DropDownList)sender;
                    GridViewRow row = (GridViewRow)ddl.NamingContainer;

                    if (row != null)
                    {
                        string selectedValue = ((DropDownList)(row.FindControl("DDL_Par"))).SelectedValue;
                        DropDownList DDL_Prorefcde = (DropDownList)row.FindControl("DDL_Prorefcde");
                        
                        using (SqlCommand cmdpro = new SqlCommand())
                        {
                            cmdpro.CommandText = " select rtrim('[' + CAST(ProductID AS VARCHAR(200)) + ']-' + ProductName ) as [ProductName], ProductID from Products  where ProductTypeID = '" + selectedValue.Trim() + "'";

                            cmdpro.Connection = con;
                            con.Open();

                            DataTable dtpro = new DataTable();
                            SqlDataAdapter adp = new SqlDataAdapter(cmdpro);
                            adp.Fill(dtpro);

                            if (dtpro.Rows.Count > 0)
                            {
                                DDL_Prorefcde.DataSource = dtpro;
                                DDL_Prorefcde.DataValueField = "ProductID";
                                DDL_Prorefcde.DataTextField = "ProductName";
                                DDL_Prorefcde.DataBind();
                                DDL_Prorefcde.Items.Insert(0, new ListItem("--Select Product--", "0"));
                            }
                            else {

                                DDL_Prorefcde.Items.Clear();
                            }
                            con.Close();
                        }
                        //DDL_Prorefcde.Focus();
                    }
                }

                catch (Exception ex)
                {
                    throw ex;
                    //lbl_err.Text = ex.Message.ToString();
                }
        }

        public void getPro()
        {
            try
            {
                //using (SqlCommand cmdpro = new SqlCommand())
                //{
                //    con.Close();

                //    //cmdpro.CommandText = " select rtrim('[' + CAST(ProductID AS VARCHAR(200)) + ']-' + ProductName ) as [ProductName], ProductID from Products where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                //    cmdpro.CommandText = "select ProductName, tbl_dstk.ProductID from tbl_dstk " +
                //        " inner join products on tbl_dstk.productid = Products.ProductID";

                //    cmdpro.Connection = con;
                //    con.Open();

                //    DataTable dtpro = new DataTable();
                //    SqlDataAdapter adp = new SqlDataAdapter(cmdpro);
                //    adp.Fill(dtpro);

                //    for (int i = 0; i < GVItms.Rows.Count; i++)
                //    {
                //        DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[i].Cells[1].FindControl("DDL_Prorefcde");

                //        if (DDL_Prorefcde.SelectedValue == "")
                //        {
                //            DDL_Prorefcde.DataSource = dtpro;
                //            DDL_Prorefcde.DataTextField = "ProductName";
                //            DDL_Prorefcde.DataValueField = "ProductID";
                //            DDL_Prorefcde.DataBind();
                //            DDL_Prorefcde.Items.Insert(0, new ListItem("--Select--", "0"));
                //        }
                //    }

                //    con.Close();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void getPro(string DDLPro)
        {
            try
            {
                using (SqlCommand cmdpro = new SqlCommand())
                {
                    con.Close();

                    if (DDLPro != "")
                    {
                        cmdpro.CommandText = " select rtrim('[' + CAST(ProductID AS VARCHAR(200)) + ']-' + ProductName ) as [ProductName], ProductID from Products  where ProductTypeID = '" + DDLPro + "'";
                        //cmdpro.CommandText = " select rtrim('[' + CAST(ProductID AS VARCHAR(200)) + ']-' + ProductName ) as [ProductName], ProductID from Products";
                    }

                    cmdpro.Connection = con;
                    con.Open();

                    DataTable dtpro = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmdpro);
                    adp.Fill(dtpro);

                    for (int i = 0; i < GVItms.Rows.Count; i++)
                    {
                        //DropDownList DDL_Prorefcde = (DropDownList)GVItms.Rows[i].Cells[1].FindControl("DDL_Prorefcde");

                        //if (DDL_Prorefcde.SelectedValue == "")
                        //{
                        //    DDL_Prorefcde.DataSource = dtpro;
                        //    DDL_Prorefcde.DataTextField = "ProductName";
                        //    DDL_Prorefcde.DataValueField = "ProductID";
                        //    DDL_Prorefcde.DataBind();
                        //    DDL_Prorefcde.Items.Insert(0, new ListItem("--Select--", ""));
                        //}
                    }

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        public void getProCod()
         
        {
            try
            {
                for (int j = 0; j < GVItms.Rows.Count; j++)
                {
                    DropDownList DDL_Par = (DropDownList)GVItms.Rows[j].FindControl("DDL_Prorefcde");
                    //Label TBItmDes = (Label)GVItms.Rows[j].FindControl("TBDIS");
                    Label lblItmQty = (Label)GVItms.Rows[j].FindControl("lblItmQty");
                    TextBox TBItmQty = (TextBox)GVItms.Rows[j].FindControl("TBItmQty");
                    TextBox TbItmunt = (TextBox)GVItms.Rows[j].FindControl("TbItmunt");
                    TextBox Tbrat = (TextBox)GVItms.Rows[j].FindControl("Tbrat");
                    TextBox Tbamt = (TextBox)GVItms.Rows[j].FindControl("Tbamt");
                    Label lbl_Flag = (Label)GVItms.Rows[j].FindControl("lbl_Flag");

                    string query = "select tbl_Mstk.Mstk_id, Dstk_id,ProductID,'0.00' as [UNIT], '0.00' as [DIS],Dstk_ItmQty as [Qty], Dstk_salrat as [RATE],'0.00' as [AMT] from tbl_Mstk " +
                        " inner join tbl_Dstk on tbl_Mstk.Mstk_id = tbl_Dstk.Mstk_id where ProductID = '" + DDL_Par.SelectedValue.Trim() + "'";
                    SqlCommand cmd = new SqlCommand(query, con);
                    DataTable dt_ = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);

                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                        //TBItmDes.Text = dt_.Rows[0]["DIS"].ToString();
                        TbItmunt.Text = dt_.Rows[0]["UNIT"].ToString();
                        //TBItmQty.Text = dt_.Rows[0]["Qty"].ToString();
                        Tbrat.Text = dt_.Rows[0]["RATE"].ToString();
                        //Tbrat.Text = dt_.Rows[0]["AMT"].ToString();

                        //Tbamt.Text = (Convert.ToInt32(TBItmQty.Text.Trim()) * Convert.ToInt32(Tbrat.Text.Trim())).ToString();
                        if (lbl_Flag.Text == "0")
                        {
                            //TBItmQty.Text = dt_.Rows[0]["Qty"].ToString();
                            lblItmQty.Text = dt_.Rows[0]["Qty"].ToString();
                        }
                    }

                    //TBItmDes.Focus();
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //lbl_err.Text = ex.Message.ToString();
            }
        }

        protected void DDL_Prorefcde_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getProCod();
            }
            catch (Exception ex)
            {
                throw ex;
                //lbl_err.Text = ex.Message.ToString(); 
            }
        }

        protected void GVItms_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                BindPar();
                getPro();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //getPro();
        }

        protected void GVItms_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DropDownList ddl_par = (DropDownList)GVItms.FindControl("DDL_Par");
         
            if (e.CommandName == "Add")
            {
                int i = ddl_par.SelectedIndex;
            }
        }

        protected void TBoutstan_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string recovery;

                recovery = (Convert.ToDecimal(TBGTtl.Text) - Convert.ToDecimal(TBoutstan.Text)).ToString();

                if (TBGTtl.Text == "")
                {
                    TBGTtl.Text = "0.00";
                }
                else
                {
                    TBGTtl.Text = recovery;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void TBRecov_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string recov;

                recov = (Convert.ToDecimal(lbl_outstan.Text.Trim()) - Convert.ToDecimal(TBRecov.Text.Trim())).ToString();

                if (lbl_outstan.Text == "")
                {
                    TBoutstan.Text = "0.00";
                }
                else
                {
                    lbl_outstan.Text = recov;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void FillGrid(string custname, string dsrdat)
        {
            try
            {
                DataTable dtGetdsr_ = new DataTable();

                query = " select Customers_.CustomerID, cust_acc from Customers_ " +
                    " inner join tbl_Mdsr on Customers_.cust_acc = tbl_Mdsr.CustomerID " +
                    " where CustomerName = '" + tb_cust.Text.Trim() + "' and dsrdat='" + TBDSRDat.Text.Trim() + "' and tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and tbl_Mdsr.BranchId= '" + Session["BranchID"] + "'";

                dt_ = DBConnection.GetQueryData(query);
                if (dt_.Rows.Count > 0)
                {
                    custid = dt_.Rows[0]["CustomerID"].ToString();
                    HFSupAcc.Value = dt_.Rows[0]["cust_acc"].ToString();
                }

                query = " select distinct(CustomerID), tbl_mdsr.dsrid as [Voucher], dsrdat as [Date], " +
                    "  CustomerID from tbl_mdsr inner join tbl_Ddsr on tbl_mdsr.dsrid =  tbl_ddsr.dsrid  " +
                    "  where CustomerID = '" + HFSupAcc.Value.Trim() + "'  and dsrdat='" + TBDSRDat.Text.Trim() + "' and tbl_Mdsr.CompanyId = '" + Session["CompanyID"] + "' and tbl_Mdsr.BranchId= '" + Session["BranchID"] + "'";

                dtGetdsr_ = DBConnection.GetQueryData(query);

                if (dtGetdsr_.Rows.Count > 0)
                {
                    HFDSRID.Value = dtGetdsr_.Rows[0]["Voucher"].ToString();
                }
                GV_DSR.DataSource = dtGetdsr_;
                GV_DSR.DataBind();

            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }
        }


        protected void btn_searchdsr_Click(object sender, EventArgs e)
        {
            FillGrid(tb_cust.Text.Trim(), TBDSRDat.Text.Trim());

            ModalPopupExtender1.Show();
        }
        
        protected void TBDIS_TextChanged(object sender, EventArgs e)
        {

            try 
            {
                for (int j = 0; j < GVItms.Rows.Count; j++)
                {
                    TextBox TBDIS = (TextBox)GVItms.Rows[j].FindControl("TBDIS");
                    TextBox TBrat = (TextBox)GVItms.Rows[j].FindControl("TBrat");
                    TextBox TBamt = (TextBox)GVItms.Rows[j].FindControl("TBamt");
                    TextBox TBItmQty = (TextBox)GVItms.Rows[j].FindControl("TBItmQty");

                    if (TBDIS.Text != "")
                    {
                        decimal per, percent, salerat;
                        per = (Convert.ToDecimal(TBDIS.Text.Trim()) / 100) * Convert.ToDecimal(TBrat.Text.Trim());
                        salerat = Convert.ToDecimal(TBrat.Text.Trim()) - per;
                        TBamt.Text = (salerat * Convert.ToDecimal(TBItmQty.Text.Trim())).ToString();
                    }
                    else
                    {
                        TBDIS.Text = "0.00";
                        TBamt.Text = (Convert.ToDecimal(TBrat.Text.Trim()) * Convert.ToDecimal(TBItmQty.Text.Trim())).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void GVItms_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void tbprodt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (int j = 0; j < GVItms.Rows.Count; j++)
                {
                    Label lblItmQty = (Label)GVItms.Rows[j].FindControl("lblItmQty");
                    TextBox TBItmQty = (TextBox)GVItms.Rows[j].FindControl("TBItmQty");
                    TextBox TbItmunt = (TextBox)GVItms.Rows[j].FindControl("TbItmunt");
                    TextBox Tbrat = (TextBox)GVItms.Rows[j].FindControl("Tbrat");
                    TextBox Tbamt = (TextBox)GVItms.Rows[j].FindControl("Tbamt");
                    TextBox tbprodt = (TextBox)GVItms.Rows[j].FindControl("tbprodt");
                    Label lbl_Pro = (Label)GVItms.Rows[j].FindControl("lbl_Pro");
                    Label lbl_Flag = (Label)GVItms.Rows[j].FindControl("lbl_Flag");

                    string query = " select * from Products inner join tbl_Dstk on Products.ProductID = tbl_Dstk.ProductID " +
                    " where Products.ProductName='" + tbprodt.Text.Trim() + 
                    "' and Products.CompanyId = '" + Session["CompanyID"] + "' and Products.BranchId= '" 
                    + Session["BranchID"] + "'";
               
                    DataTable dt_ = new DataTable();

                    dt_ = DBConnection.GetQueryData(query);

                    if (dt_.Rows.Count > 0)
                    {
                        string proid = dt_.Rows[0]["ProductID"].ToString();
                        lbl_Pro.Text = proid;
                    }

                     query = "select tbl_Mstk.Mstk_id, Dstk_id,ProductID,'0.00' as [UNIT], '0.00' as [DIS],Dstk_ItmQty as [Qty], " +
                         " Dstk_salrat as [RATE],'0.00' as [AMT] from tbl_Mstk " +
                        " inner join tbl_Dstk on tbl_Mstk.Mstk_id = tbl_Dstk.Mstk_id where ProductID = '" 
                        + lbl_Pro.Text.Trim() + "' and tbl_Mstk.CompanyId = '" + Session["CompanyID"] +
                        "' and tbl_Mstk.BranchId= '" + Session["BranchID"] + "'";

                    SqlCommand cmd = new SqlCommand(query, con);

                    DataTable dtdtl = new DataTable();

                    SqlDataAdapter adp = new SqlDataAdapter(cmd);

                    adp.Fill(dtdtl);

                    if (dtdtl.Rows.Count > 0)
                    {
                        TbItmunt.Text = dtdtl.Rows[0]["UNIT"].ToString();
                        
                        if (Tbrat.Text != "0")
                        {
                            // Do nothing

                        }else if(Tbrat.Text == "0")
                        {
                            Tbrat.Text = dtdtl.Rows[0]["RATE"].ToString();
                        }
                        if (lbl_Flag.Text == "0")
                        {
                            lblItmQty.Text = dtdtl.Rows[0]["Qty"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }
        }

        protected void TBGST_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (TBGST.Text != "")
                {

                    string gst = (Convert.ToDecimal(TBGTtl.Text.Trim()) * Convert.ToDecimal(TBGST.Text.Trim()) / 100).ToString();

                    TBTtl.Text = (Convert.ToDecimal(TBTtl.Text.Trim()) + Convert.ToDecimal(gst.Trim())).ToString();
                    //TBGTtl.Text = (Convert.ToDecimal(gst.Trim()) + Convert.ToDecimal(TBGTtl.Text.Trim())).ToString();
                }
            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }
        }

        protected void TBOthTax_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (TBOthTax.Text != "")
                {
                    string othtax = (Convert.ToDecimal(TBGTtl.Text.Trim()) * Convert.ToDecimal(TBOthTax.Text.Trim()) / 100).ToString();

                    TBTtl.Text = (Convert.ToDecimal(TBTtl.Text.Trim()) + Convert.ToDecimal(othtax.Trim())).ToString();
                    //TBGTtl.Text = (Convert.ToDecimal(othtax.Trim()) + Convert.ToDecimal(TBGTtl.Text.Trim())).ToString();
                }
            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }

        }

        protected void TBDIS_TextChanged1(object sender, EventArgs e)
        {
            try
            {
                lbl_discamt.Text = (Convert.ToDecimal(TBDIS.Text) / 100 * Convert.ToDecimal(TBGTtl.Text)).ToString();
                TBTtl.Text = (Convert.ToDecimal(TBTtl.Text) - Convert.ToDecimal(lbl_discamt.Text)).ToString();
                TB_ttl.Text = TBTtl.Text;
            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }
        }

        protected void TB_SDIS_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lbl_SdisAmt.Text = (Convert.ToDecimal(TB_SDIS.Text) / 100 * Convert.ToDecimal(TBTtl.Text)).ToString();
                TB_ttl.Text = (Convert.ToDecimal(TBTtl.Text) - Convert.ToDecimal(lbl_SdisAmt.Text)).ToString();
            }
            catch (Exception ex)
            {
                lbl_err.Text = ex.Message;
            }
        }

      
    }
}