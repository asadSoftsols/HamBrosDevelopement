﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Cfg;
using Foods;
using System.Globalization;
using DataAccess;


namespace Foods
{
    public partial class frm_Purchase : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["D"].ConnectionString);       
        string MPurId, Mstk_id, Mvch_id, MSTkId, stkqty, PurItm, PurItmDscptin, PurItmQty, PurRate, SalRat, Particulars, PurNetTtl, str;
        decimal totalprev;
        int chkdetails = 0;
        SqlTransaction tran;
        DBConnection c = new DBConnection();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            v_vendor.Text = "Please Select Vendor";
            DDL_Paytyp.Enabled = false;
            DDL_Vchtyp.Enabled = false;

            if (!this.IsPostBack)
            {

                FillGrid();
                SetInitRowPuritm();
                ptnSno();
                TBPONum.Enabled = false;
                GVPurItems.Visible = true;
                PanelShowClosed.Visible = false;
                TBPurDat.Text = DateTime.Now.ToShortDateString();
                ddlVenNam.Enabled = true;
                chk_Act.Checked = true;
                chk_prtd.Checked = true;
                ddlVenNam.Focus();
                BindDll();
               
                pnl_bnk.Visible = false;
                pnl_chqamt.Visible = false;
                pnl_Chqno.Visible = false;
                pnl_cshamt.Visible = false;

                lbl_Gttl.Visible = false;
                lbl_ttl.Visible = false;
                lbl_ChkAmt.Visible = false;
                lbl_Chkpurrat.Visible = false;
                lbl_Itmqty.Visible = false;
                lbl_Pros.Visible = false;
                lbl_purdat.Visible = false;
                lbl_Ven.Visible = false;
                lbl_vtyp.Visible = false;
                lbl_paytyp.Visible = false;
                lbl_cshamt.Visible = false;
                lbl_chqamt.Visible = false;
                lbl_bnk.Visible = false;
                lbl_chqno.Visible = false;

            }        
        }


        private void ptnSno()
        {
            try
            {
                
                str = "select isnull(max(cast(MPurID as int)),0) as [MPurID]  from MPurchase  where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                    SqlCommand cmd = new SqlCommand(str, con);
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        if (string.IsNullOrEmpty(TBPONum.Text))
                        {
                            int v = Convert.ToInt32(reader["MPurID"].ToString());
                            int b = v + 1;
                            TBPONum.Text = "PUR00" + Session["CompanyID"] + b.ToString();
                        }
                    }
                    con.Close();
                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }
        }


        private void SetInitRowPuritm()
        {
            DataTable dt = new DataTable();
            DataRow dr = null;

            dt.Columns.Add(new DataColumn("productid", typeof(string)));
            dt.Columns.Add(new DataColumn("ProNam", typeof(string)));
            dt.Columns.Add(new DataColumn("percent", typeof(string)));
            dt.Columns.Add(new DataColumn("QTY", typeof(string)));
            dt.Columns.Add(new DataColumn("Weight", typeof(string)));
            dt.Columns.Add(new DataColumn("Rate", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit", typeof(string)));
            dt.Columns.Add(new DataColumn("Cost", typeof(string)));
            dt.Columns.Add(new DataColumn("Sales Tax", typeof(string)));
            dt.Columns.Add(new DataColumn("Purchase Rate", typeof(string)));
            dt.Columns.Add(new DataColumn("Sale Rate", typeof(string)));
            dt.Columns.Add(new DataColumn("Particulars", typeof(string)));
            dt.Columns.Add(new DataColumn("Debit Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("Credit Amount", typeof(string)));
            dt.Columns.Add(new DataColumn("NetTotal", typeof(string)));
            dt.Columns.Add(new DataColumn("DPurID", typeof(string)));

            dr = dt.NewRow();

            dr["productid"] = string.Empty;
            dr["ProNam"] = string.Empty;
            dr["percent"] = "0.00";
            dr["QTY"] = "0.00";
            dr["Weight"] = "0.00";
            dr["Rate"] = "0.00";
            dr["Unit"] = string.Empty;
            dr["Cost"] = "0.00";
            dr["Sales Tax"] = "0.00";
            dr["Purchase Rate"] = "0.00";
            dr["Sale Rate"] = "0.00";
            dr["Particulars"] = string.Empty;
            dr["Debit Amount"] = "0.00";
            dr["Credit Amount"] = "0.00";
            dr["NetTotal"] = "0.00";
            dr["DPurID"] = "0";

            dt.Rows.Add(dr);

            //Store the DataTable in ViewState
            ViewState["dt_adItm"] = dt;

            GVPurItems.DataSource = dt;
            GVPurItems.DataBind();
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
                        //extract the TextBox values
                        DropDownList ddlPurItm = (DropDownList)GVPurItems.Rows[rowIndex].Cells[0].FindControl("ddlPurItm");
                        TextBox TbaddPurItmDscptin = (TextBox)GVPurItems.Rows[rowIndex].Cells[1].FindControl("TbaddPurItmDscptin");
                        TextBox TbAddPurItmQty = (TextBox)GVPurItems.Rows[rowIndex].Cells[2].FindControl("TbAddPurItmQty");
                        //TextBox Tbwght = (TextBox)GVPurItems.Rows[rowIndex].Cells[3].FindControl("Tbwght");
                        //TextBox Tbrat = (TextBox)GVPurItems.Rows[rowIndex].Cells[4].FindControl("Tbrat");
                        //DropDownList ddlPurUnit = (DropDownList)GVPurItems.Rows[rowIndex].Cells[5].FindControl("ddlPurUnit");
                        //TextBox TbAddCost = (TextBox)GVPurItems.Rows[rowIndex].Cells[6].FindControl("TbAddCosts");
                        //TextBox TbSalTax = (TextBox)GVPurItems.Rows[rowIndex].Cells[7].FindControl("TbSalTax");
                        Label TbPurRat = (Label)GVPurItems.Rows[rowIndex].Cells[3].FindControl("TbPurRat");
                        Label TbSalRat = (Label)GVPurItems.Rows[rowIndex].Cells[4].FindControl("TbSalRat");
                        //DropDownList ddl_Prac = (DropDownList)GVPurItems.Rows[rowIndex].Cells[10].FindControl("ddl_Prac");
                        //TextBox Tbdbamt = (TextBox)GVPurItems.Rows[rowIndex].Cells[11].FindControl("Tbdbamt");
                        //TextBox Tbcramt = (TextBox)GVPurItems.Rows[rowIndex].Cells[12].FindControl("Tbcramt");
                        TextBox TbAddPurNetTtl = (TextBox)GVPurItems.Rows[rowIndex].Cells[5].FindControl("TbAddPurNetTtl");
                        HiddenField HFDPur = (HiddenField)GVPurItems.Rows[rowIndex].Cells[5].FindControl("HFDPur");
                        
                        drRow = dt.NewRow();

                        dt.Rows[i - 1]["productid"] = ddlPurItm.SelectedValue;
                        dt.Rows[i - 1]["ProNam"] = ddlPurItm.SelectedValue;
                        dt.Rows[i - 1]["percent"] = TbaddPurItmDscptin.Text;
                        dt.Rows[i - 1]["QTY"] = TbAddPurItmQty.Text;
                        //dt.Rows[i - 1]["Weight"] = Tbwght.Text;
                        //dt.Rows[i - 1]["Rate"] = Tbrat.Text;
                        //dt.Rows[i - 1]["Unit"] = ddlPurUnit.Text;
                        //dt.Rows[i - 1]["Cost"] = TbAddCost.Text;
                        //dt.Rows[i - 1]["Sales Tax"] = TbSalTax.Text;
                        dt.Rows[i - 1]["Purchase Rate"] = TbPurRat.Text;
                        dt.Rows[i - 1]["Sale Rate"] = TbSalRat.Text;
                        //dt.Rows[i - 1]["Particulars"] = ddl_Prac.SelectedValue;
                        //dt.Rows[i - 1]["Debit Amount"] = Tbdbamt.Text;
                        //dt.Rows[i - 1]["Credit Amount"] = Tbcramt.Text;
                        dt.Rows[i - 1]["NetTotal"] = TbAddPurNetTtl.Text;
                        //if (HFDPur.Value != "")
                        //{
                            //dt.Rows[i - 1]["DPurID"] = HFDPur.Value;
                        //}
                        //else
                        //{
                            //dt.Rows[i - 1]["DPurID"] = "0";
                        //}
                        rowIndex++;

                        //float GTotal = 0, CRAmt = 0, DBAmt = 0;
                        //for (int j = 0; j < GVPurItems.Rows.Count; j++)
                        //{
                        //    TextBox total = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurNetTtl");
                        //    //TextBox CRAmtttl = (TextBox)GVPurItems.Rows[j].FindControl("Tbcramt");
                        //    //TextBox DBAmtttl = (TextBox)GVPurItems.Rows[j].FindControl("Tbdbamt");
                            
                        //    GTotal += Convert.ToSingle(total.Text);

                        //    /*if (CRAmtttl.Text != "0.00" || DBAmtttl.Text != "0.00")
                        //    {   
                        //        CRAmt += Convert.ToSingle(CRAmtttl.Text);
                        //        DBAmt += Convert.ToSingle(DBAmtttl.Text);
                        //    }*/
                        //}

                        //TBGrssTotal.Text = GTotal.ToString();
                        //TBTtl.Text = GTotal.ToString();

                        //if (DDL_Paytyp.SelectedValue == "2")
                        //{
                        //    TB_ChqAmt.Text = TBTtl.Text;
                        //    TB_ChqAmt.Enabled = false;
                        //}
                        //else
                        //{
                        //    TB_CshAmt.Text = TBTtl.Text;
                        //    TB_CshAmt.Enabled = false;
                        //}
                        //TBCRAmtttl.Text = CRAmt.ToString();
                        //Customersttl.Text = DBAmt.ToString();
                        

                        //ddlPurItm.Focus();
                    }

                    dt.Rows.Add(drRow);
                    ViewState["dt_adItm"] = dt;

                    GVPurItems.DataSource = dt;
                    GVPurItems.DataBind();
                }
            }
            else
            {
                Response.Write("ViewState is null");
            }

            //Set Previous Data on Postbacks
            SetPreRowitm();
        }

       
        private void SetPreRowitm()
        {
            try
            {
                //BindDll();

                int rowIndex = 0;
                if (ViewState["dt_adItm"] != null)
                {
                    DataTable dt = (DataTable)ViewState["dt_adItm"];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DropDownList ddlPurItm = (DropDownList)GVPurItems.Rows[rowIndex].Cells[0].FindControl("ddlPurItm");
                            TextBox TbaddPurItmDscptin = (TextBox)GVPurItems.Rows[rowIndex].Cells[1].FindControl("TbaddPurItmDscptin");
                            TextBox TbAddPurItmQty = (TextBox)GVPurItems.Rows[rowIndex].Cells[2].FindControl("TbAddPurItmQty");
                            //TextBox Tbwght = (TextBox)GVPurItems.Rows[rowIndex].Cells[3].FindControl("Tbwght");
                            //TextBox Tbrat = (TextBox)GVPurItems.Rows[rowIndex].Cells[4].FindControl("Tbrat");
                            //DropDownList ddlPurUnit = (DropDownList)GVPurItems.Rows[rowIndex].Cells[5].FindControl("ddlPurUnit");
                            //TextBox TbAddCost = (TextBox)GVPurItems.Rows[rowIndex].Cells[6].FindControl("TbAddCosts");
                            //TextBox TbSalTax = (TextBox)GVPurItems.Rows[rowIndex].Cells[7].FindControl("TbSalTax");
                            Label TbPurRat = (Label)GVPurItems.Rows[rowIndex].Cells[3].FindControl("TbPurRat");
                            Label TbSalRat = (Label)GVPurItems.Rows[rowIndex].Cells[4].FindControl("TbSalRat");
                            //DropDownList ddl_Prac = (DropDownList)GVPurItems.Rows[rowIndex].Cells[10].FindControl("ddl_Prac");
                            //TextBox Tbdbamt = (TextBox)GVPurItems.Rows[rowIndex].Cells[11].FindControl("Tbdbamt");
                            //TextBox Tbcramt = (TextBox)GVPurItems.Rows[rowIndex].Cells[12].FindControl("Tbcramt");
                            TextBox TbAddPurNetTtl = (TextBox)GVPurItems.Rows[rowIndex].Cells[5].FindControl("TbAddPurNetTtl");
                            HiddenField HFDPur = (HiddenField)GVPurItems.Rows[rowIndex].Cells[5].FindControl("HFDPur");
                            Label lbl_Flag = (Label)GVPurItems.Rows[i].FindControl("lbl_Flag");

                            //DPurID       //TextBox TbAddPurTol = (TextBox)GVPurItems.Rows[rowIndex].Cells[3].FindControl("TbAddPurTol");
                            //TextBox TbAddSubTotl = (TextBox)GVPurItems.Rows[rowIndex].Cells[4].FindControl("TbAddSubTotl");
                            //TextBox TbAddPurSalTax = (TextBox)GVPurItems.Rows[rowIndex].Cells[5].FindControl("TbAddPurSalTax");
                            //DropDownList DDLProTyp = (DropDownList)GVPurItems.Rows[rowIndex].Cells[9].FindControl("DDLProTyp");

                            if (ddlPurItm.SelectedValue == "")
                            {
                                forDetalItm();
                            }
                            ddlPurItm.SelectedValue = dt.Rows[i]["ProNam"].ToString();

                            TbaddPurItmDscptin.Text = dt.Rows[i]["percent"].ToString();


                            string qty = dt.Rows[i]["QTY"].ToString();

                            if (qty != "")
                            {
                                TbAddPurItmQty.Text = dt.Rows[i]["QTY"].ToString();
                            }
                            else
                            {
                                TbAddPurItmQty.Text = "0.00";
                            }

                            //Tbwght.Text = dt.Rows[i]["Weight"].ToString();
                            //Tbrat.Text = dt.Rows[i]["Rate"].ToString();
                            //ddlPurUnit.Text = dt.Rows[i]["Unit"].ToString();
                            //TbAddCost.Text = dt.Rows[i]["Cost"].ToString();
                            //TbSalTax.Text = dt.Rows[i]["Sales Tax"].ToString();
                            string purrat = dt.Rows[i]["Purchase Rate"].ToString();

                            if (purrat != "")
                            {
                                TbPurRat.Text = dt.Rows[i]["Purchase Rate"].ToString();
                            }
                            else
                            {
                                TbPurRat.Text = "0.00";
                            }

                            string salrat = dt.Rows[i]["Sale Rate"].ToString();

                            if (salrat != "")
                            {
                                TbSalRat.Text = dt.Rows[i]["Sale Rate"].ToString();
                            }
                            else
                            {
                                TbSalRat.Text = "0.00";
                            }
                            //TbPurRat.Text = dt.Rows[i]["Purchase Rate"].ToString();
                            //TbSalRat.Text = dt.Rows[i]["Sale Rate"].ToString();

                            //if (ddl_Prac.SelectedValue == "")
                            //{
                              //  forDetalPract();
                           // }
                            //ddl_Prac.SelectedValue = dt.Rows[i]["Particulars"].ToString();
                            //Tbdbamt.Text = dt.Rows[i]["Debit Amount"].ToString();
                            //Tbcramt.Text = dt.Rows[i]["Credit Amount"].ToString();

                            string netttl = dt.Rows[i]["NetTotal"].ToString();

                            if (netttl != "")
                            {
                                TbAddPurNetTtl.Text = dt.Rows[i]["NetTotal"].ToString();
                            }
                            else
                            {
                                TbAddPurNetTtl.Text = "0.00";
                            }
                            
                            HFDPur.Value = dt.Rows[i]["DPurID"].ToString();


                            if (ddlPurItm.SelectedValue == "0")
                            {
                                lbl_Flag.Text = "0";
                            }
                            else
                            {
                                lbl_Flag.Text = "1";
                            }

                            //DDLProTyp.SelectedItem.Text = dt.Rows[i]["Product Type"].ToString();
                            //ChkClosed.Checked = dt.Rows[i]["CLOSED"] == DBNull.Value ? false : Convert.ToBoolean(dt.Rows[i]["CLOSED"]);
                            //ChkClosed.Checked = Convert.ToBoolean(dt.Rows[i]["CLOSED"] == DBNull.Value) ? true : Convert.ToBoolean(dt.Rows[i]["CLOSED"] == DBNull.Value);

                            rowIndex++;

                            // ADDTOTAL();


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;

            }
        }

        protected void linkbtnadd_Click(object sender, EventArgs e)
        {
            AddNewRow();
        }


        public void forDetalItm()
        {
                using (SqlCommand cmdpro = new SqlCommand())
                {
                    cmdpro.CommandText = " select *  from Products where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    cmdpro.Connection = con;
                    con.Open();

                    DataTable dtSupNam = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmdpro);
                    adp.Fill(dtSupNam);

                    for (int o = 0; o < GVPurItems.Rows.Count; o++)
                    {
                        DropDownList ddlitem = (DropDownList)GVPurItems.Rows[o].FindControl("ddlPurItm");

                        ddlitem.DataSource = dtSupNam;
                        ddlitem.DataTextField = "ProductName";
                        ddlitem.DataValueField = "ProductID";
                        ddlitem.DataBind();
                        ddlitem.Items.Insert(0, new ListItem("--Select--", "0"));

                    }

                    con.Close();
                }
            }

        public void forDetalPract()
        {
            //For Particulars

            using (SqlCommand cmdpart = new SqlCommand())
            {
                cmdpart.CommandText = " select subheadcategoriesID as [AccID] ,rtrim('[' + CAST(subheadcategoriesID AS VARCHAR(200)) + ']-' + subheadcategoriesName ) as [AccName]  from subheadcategories ";

                cmdpart.Connection = con;
                //con.Open();

                DataTable dtpart = new DataTable();
                SqlDataAdapter adppart = new SqlDataAdapter(cmdpart);
                adppart.Fill(dtpart);

                for (int k = 0; k < GVPurItems.Rows.Count; k++)
                {
                    DropDownList ddl_Praci = (DropDownList)GVPurItems.Rows[k].FindControl("ddl_Prac");

                    ddl_Praci.DataSource = dtpart;
                    ddl_Praci.DataTextField = "AccName";
                    ddl_Praci.DataValueField = "AccID";
                    ddl_Praci.DataBind();
                    ddl_Praci.Items.Insert(0, new ListItem("--Select--", "0"));

                }
                con.Close();
            }

        }
        public void FillGrid()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = " select ROW_NUMBER() OVER(ORDER BY (select 1)) AS ID,MPurID, suppliername,VndrAdd,VndrCntct,PurNo,convert(varchar, MPurDate, 23) as [PurDat],MPurDate,'' as [CNIC],'' as [NTNNo], " +
                        " MPurchase.CreatedBy,MPurchase.CreatedAt,ToBePrntd from MPurchase  inner join " +
                        " supplier on MPurchase.ven_id = supplier.supplierid and MPurchase.CompanyId = '" + Session["CompanyID"] + "' and MPurchase.BranchId= '" + Session["BranchID"] + "' order by MPurID desc";

                    cmd.Connection = con;
                    con.Open();

                    DataTable dt_ = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dt_);

                    GVScrhMPur.DataSource = dt_;
                    GVScrhMPur.DataBind();

                    ViewState["MPur"] = dt_;

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

        }

        public void BindDll()
        {
            try
            {
                //For Requisition

                //using (SqlCommand cmdReq = new SqlCommand())
                //{
                //    cmdReq.CommandText = " select rtrim('[' + CAST(MReq_sono AS VARCHAR(200)) + ']-' + Depart_nam) as [MReq], MReq_id from tbl_MReq  inner join tbl_Depart on tbl_MReq.Depart_id = tbl_Depart.Depart_id";

                //    cmdReq.Connection = con;
                //    con.Open();

                //    DataTable dtreq = new DataTable();
                //    SqlDataAdapter adp = new SqlDataAdapter(cmdReq);
                //    adp.Fill(dtreq);

                //    DDl_Req.DataSource = dtreq;
                //    DDl_Req.Items.Add(new ListItem("--Select--", "0"));
                //    DDl_Req.DataTextField = "MReq";
                //    DDl_Req.DataValueField = "MReq_id";
                //    DDl_Req.DataBind();


                //    con.Close();
                //}

                //For Voucher Type

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = " select rtrim('[' + CAST(vchtyp_id AS VARCHAR(200)) + ']-' + vchtyp_nam ) as [vchtyp_nam], vchtyp_id  from tbl_vchtyp where ISActive = 'True'";

                    cmd.Connection = con;
                    con.Open();

                    DataTable dtvch = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtvch);

                    DDL_Vchtyp.DataSource = dtvch;
                    DDL_Vchtyp.DataTextField = "vchtyp_nam";
                    DDL_Vchtyp.DataValueField = "vchtyp_id";
                    DDL_Vchtyp.DataBind();
                    DDL_Vchtyp.Items.Insert(0, new ListItem("--Select--", "0"));


                    con.Close();
                }



                //Payment Type
                //using (SqlCommand cmd = new SqlCommand())
                //{
                //    //cmd.CommandText = " select rtrim('[' + CAST(PayTyp_id AS VARCHAR(200)) + ']-' + PayTyp_nam ) as [PayTyp_nam], PayTyp_id  from tbl_PayTyp where ISActive = 'True'";
                //    cmd.CommandText = " select distinct rtrim('[' + CAST(tbl_PayTyp.PayTyp_id AS VARCHAR(200)) + ']-' + PayTyp_nam ) as [PayTyp_nam], vchtyp_id,tbl_PayTyp.PayTyp_id  from tbl_PayTyp " +
                //                       " inner join tbl_vchtyp on tbl_PayTyp.PayTyp_id= tbl_vchtyp.PayTyp_id " +
                //                       "  where tbl_PayTyp.ISActive = 'True'  and tbl_vchtyp.vchtyp_id = '" + DDL_Vchtyp.SelectedValue.Trim() + "'";

                //    cmd.Connection = con;
                //    con.Open();

                //    DataTable dtPaytyp = new DataTable();
                //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                //    adp.Fill(dtPaytyp);

                //    DDL_Paytyp.DataSource = dtPaytyp;
                //    DDL_Paytyp.DataTextField = "PayTyp_nam";
                //    DDL_Paytyp.DataValueField = "PayTyp_id";
                //    DDL_Paytyp.DataBind();
                //    DDL_Paytyp.Items.Insert(0, new ListItem("--Select--", "0"));

                //    con.Close();
                //}

                // For Supplier

                using (SqlCommand cmdSupNam = new SqlCommand())
                {
                    //cmd.CommandText = " select rtrim('[' + CAST(ven_id AS VARCHAR(200)) + ']-' + ven_nam ) as [ven_nam], ven_id from t_ven";
                    cmdSupNam.CommandText = "select supplierId,suppliername from supplier";
                    //cmdSupNam.CommandText = "select rtrim('[' + CAST(supplierId AS VARCHAR(200)) + ']-' + suppliername ) as [suppliername], supplierId from supplier where CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                    cmdSupNam.Connection = con;
                    con.Open();

                    DataTable dtSupNam = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmdSupNam);
                    adp.Fill(dtSupNam);

                    ddlVenNam.DataSource = dtSupNam;
                    ddlVenNam.DataTextField = "suppliername";
                    ddlVenNam.DataValueField = "supplierId";
                    ddlVenNam.DataBind();
                    ddlVenNam.Items.Insert(0, new ListItem("--Select--", "0"));


                    con.Close();
                }

                //For Bank

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = " select rtrim('[' + CAST(CashBnk_id AS VARCHAR(200)) + ']-' + CashBnk_nam ) as [CashBnk_nam], CashBnk_id  from tbl_CashBnk where ISActive = 'True'";

                    cmd.Connection = con;
                    con.Open();

                    DataTable dtbnk = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtbnk);

                    DDL_Bnk.DataSource = dtbnk;
                    DDL_Bnk.DataTextField = "CashBnk_nam";
                    DDL_Bnk.DataValueField = "CashBnk_id";
                    DDL_Bnk.DataBind();
                    DDL_Bnk.Items.Insert(0, new ListItem("--Select--", "0"));

                    con.Close();
                }

                // For Customer

                //using (SqlCommand cmd = new SqlCommand())
                //{
                //    cmd.CommandText = " select rtrim('[' + CAST(CustomerID AS VARCHAR(200)) + ']-' + CustomerName ) as [CustomerName], CustomerID from Customers_";

                //    cmd.Connection = con;
                //    con.Open();

                //    DataTable dtCust = new DataTable();
                //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                //    adp.Fill(dtCust);

                //    DDL_Cust.DataSource = dtCust;
                //    DDL_Cust.DataTextField = "CustomerName";
                //    DDL_Cust.DataValueField = "CustomerID";
                //    DDL_Cust.DataBind();
                //    DDL_Cust.Items.Add(new ListItem("--Select--", "0"));

                //    con.Close();
                //}
                //Items Name

                forDetalItm();
                //using (SqlCommand cmdpro = new SqlCommand())
                //{
                //    cmdpro.CommandText = " select rtrim('[' + CAST(ProductID AS VARCHAR(200)) + ']-' + ProductName ) as [ProductName], ProductID  from Products";

                //    cmdpro.Connection = con;
                //    con.Open();

                //    DataTable dtSupNam = new DataTable();
                //    SqlDataAdapter adp = new SqlDataAdapter(cmdpro);
                //    adp.Fill(dtSupNam);

                //    for (int i = 0; i < GVPurItems.Rows.Count; i++)
                //    {
                //        DropDownList ddlitem = (DropDownList)GVPurItems.Rows[i].FindControl("ddlPurItm");

                //        ddlitem.DataSource = dtSupNam;
                //        ddlitem.DataTextField = "ProductName";
                //        ddlitem.DataValueField = "ProductID";
                //        ddlitem.DataBind();
                //        ddlitem.Items.Insert(0, new ListItem("--Select--", "0"));

                //    }

                //    //for (int i = 0; i < GVAddEdtPur.Rows.Count; i++)
                //    //{
                //    //    DropDownList ddlEdtitem = (DropDownList)GVAddEdtPur.Rows[i].FindControl("ddl_EdtPurProNam");

                //    //    ddlEdtitem.DataSource = dtSupNam;
                //    //    ddlEdtitem.DataTextField = "ProductName";
                //    //    ddlEdtitem.DataValueField = "ProductID";
                //    //    ddlEdtitem.DataBind();
                //    //    ddlEdtitem.Items.Insert(0, new ListItem("--Select--", "0"));

                //    //}
                //    con.Close();
                //}
                // for Accounts
                //using (SqlCommand cmdacct = new SqlCommand())
                //{
                //    //cmdacct.CommandText = " select subheadcategoryfourID as [AccID] ,rtrim('[' + CAST(subheadcategoryfourID AS VARCHAR(200)) + ']-' + subheadcategoryfourName ) as [AccName] from subheadcategoryfour";
                //    cmdacct.CommandText = "select subheadcategoryfourID,subheadcategoryfourName from subheadcategoryfour where subheadcategoryfourName <> ''";
                //    cmdacct.Connection = con;
                //    con.Open();

                //    DataTable dtacct = new DataTable();
                //    SqlDataAdapter adpacct = new SqlDataAdapter(cmdacct);
                //    adpacct.Fill(dtacct);

                //    DDL_Acct.DataSource = dtacct;
                //    DDL_Acct.DataTextField = "subheadcategoryfourName";
                //    DDL_Acct.DataValueField = "subheadcategoryfourID";
                //    DDL_Acct.DataBind();
                //    DDL_Acct.Items.Insert(0, new ListItem("--Select--", "1"));
                //                        con.Close();
                //}

                //For Particulars

                forDetalPract();
                //using (SqlCommand cmdpart = new SqlCommand())
                //{
                //    cmdpart.CommandText = " select subheadcategoriesID as [AccID] ,rtrim('[' + CAST(subheadcategoriesID AS VARCHAR(200)) + ']-' + subheadcategoriesName ) as [AccName]  from subheadcategories ";

                //    cmdpart.Connection = con;
                //    //con.Open();

                //    DataTable dtpart = new DataTable();
                //    SqlDataAdapter adppart = new SqlDataAdapter(cmdpart);
                //    adppart.Fill(dtpart);

                //    for (int i = 0; i < GVPurItems.Rows.Count; i++)
                //    {
                //        DropDownList ddl_Prac = (DropDownList)GVPurItems.Rows[i].FindControl("ddl_Prac");

                //        ddl_Prac.DataSource = dtpart;
                //        ddl_Prac.DataTextField = "AccName";
                //        ddl_Prac.DataValueField = "AccID";
                //        ddl_Prac.DataBind();
                //        ddl_Prac.Items.Insert(0, new ListItem("--Select--", "0"));

                //    }
                //    con.Close();
                //}
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }
        }


        protected void TBSearchPONum_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (TBSearchPONum.Text != "")
                {
                    DataTable dt_po = new DataTable();

                    string queryString = " select MPurID, suppliername,VndrAdd,VndrCntct,PurNo,MPurDate,supplier.CNIC,NTNNo, " +
                    " MPurchase.CreatedBy,MPurchase.CreatedAt,ToBePrntd from MPurchase  inner join " +
                    " supplier on MPurchase.ven_id = supplier.supplierId where PurNo= '" + TBSearchPONum.Text + "' and  MPurchase.CompanyId = '" + Session["CompanyID"] + "' and MPurchase.BranchId= '" + Session["BranchID"] + "'";

                    dt_po = DBConnection.GetQueryData(queryString);

                    GVScrhMPur.DataSource = dt_po;
                    GVScrhMPur.DataBind();
                }
                else if (TBSearchPONum.Text == "")
                {
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                //throw;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = ex.Message;
            }

        }


        protected void GVScrhMPur_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVScrhMPur.PageIndex = e.NewPageIndex;
            FillGrid();
        }

        protected void GVScrhMPur_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow row;

                if (e.CommandName == "Show")
                {
                    row =(GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string PURID = GVScrhMPur.DataKeys[row.RowIndex].Values[0].ToString();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/rpt_Purchase.aspx?ID=PR&PURID=" + PURID + "','_blank','height=600px,width=600px,scrollbars=1');", true);                
                }

                if (e.CommandName == "Select")
                {
                      row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    string MPurID = Server.HtmlDecode(GVScrhMPur.Rows[row.RowIndex].Cells[0].Text.ToString());

                    string cmdtxt = "   select MPurchase.MPurID,DPurID,PurNo,DCNO, DatTim, BiltyNo, VehicalNo, DriverNam, DriverMobilno, Transporter, station, DilverOrdr, frieght, mPurDate,ven_id,vchtyp_id,PayTyp_id,subheadcategoryfourID,csh_amt,CashBnk_id,chque_No,MPurRmk,GrossTtal,tobePrntd,ck_Act,Out_Standing,grssttl,Out_Standing from MPurchase " +
                        " inner join DPurchase on MPurchase.MPurID = DPurchase.MPurID where MPurchase.MPurID ='" + MPurID + "' and MPurchase.CompanyId = '" + Session["CompanyID"] + "' and MPurchase.BranchId= '" + Session["BranchID"] + "'";

                    //string cmdtxt = " select a.mPurID, b.dPurId, b.mPurId, a.ven_id, a.VndrAdd, a.VndrCntct,a.PurNo, a.mPurDate, a.CreatedBy, a.CreatedAt, a.cnic, a.ntnno, a.tobePrntd,b.Dpurid, b.ProNam, b.ProDes, b.Qty, b.Total, b.subtotl, b.unit, b.cost, b.protyp,b.grossttal from MPurchase a  inner join DPurchase b on a.mPurID = b.mPurID where a.MPurID =" + MPurID + "";

                    SqlCommand cmdSlct = new SqlCommand(cmdtxt, con);
                    SqlDataAdapter adp = new SqlDataAdapter(cmdSlct);

                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        HFMPur.Value = dt.Rows[0]["MPurID"].ToString();
                        TBPONum.Text = dt.Rows[0]["PurNo"].ToString();
                        TBOutstand.Text = dt.Rows[0]["Out_Standing"].ToString();
                        txt_outstand.Text = dt.Rows[0]["Out_Standing"].ToString();                        
                        TBPurDat.Text = dt.Rows[0]["mPurDate"].ToString();                        
                        //DDL_Acct.SelectedValue = dt.Rows[0]["subheadcategoryfourID"].ToString();
                        ddlVenNam.SelectedValue = dt.Rows[0]["ven_id"].ToString();
                        DDL_Vchtyp.SelectedValue = dt.Rows[0]["vchtyp_id"].ToString();

                        getPay_Typ(DDL_Vchtyp.SelectedValue.Trim());

                        int vchtyp = Convert.ToInt32(dt.Rows[0]["vchtyp_id"]);
                        
                        DDL_Paytyp.SelectedValue = dt.Rows[0]["PayTyp_id"].ToString();
                        //|| vchtyp == "3"
                        if (vchtyp == 1 || vchtyp == 3)
                        {
                            pnl_bnk.Visible = true;
                            pnl_Chqno.Visible = true;
                            pnl_chqamt.Visible = true;
                            pnl_cshamt.Visible = false;

                            DDL_Bnk.SelectedValue = dt.Rows[0]["CashBnk_id"].ToString();
                            TBChqNo.Text = dt.Rows[0]["chque_No"].ToString();
                            TB_ChqAmt.Text = dt.Rows[0]["grssttl"].ToString();
                            TBTtl.Text = dt.Rows[0]["grssttl"].ToString();
                            TBGrssTotal.Text = dt.Rows[0]["grssttl"].ToString();
                            TBOutstand.Text = dt.Rows[0]["Out_Standing"].ToString(); ;

                        }
                        else if (vchtyp == 2 || vchtyp == 4)
                        {
                            pnl_bnk.Visible = false;
                            pnl_Chqno.Visible = false;
                            pnl_chqamt.Visible = false;
                            pnl_cshamt.Visible = true;
                            
                            TB_CshAmt.Text = dt.Rows[0]["csh_amt"].ToString();
                            TBTtl.Text = dt.Rows[0]["csh_amt"].ToString();
                            TBGrssTotal.Text = dt.Rows[0]["grssttl"].ToString();
                            TBOutstand.Text = dt.Rows[0]["Out_Standing"].ToString(); 
                           
                        }

                        TBRmk.Text = dt.Rows[0]["MPurRmk"].ToString();
                        //TBGrssTotal.Text = dt.Rows[0]["GrossTtal"].ToString();
                        chk_prtd.Checked = Convert.ToBoolean(dt.Rows[0]["tobePrntd"].ToString());
                        chk_Act.Checked = Convert.ToBoolean(dt.Rows[0]["ck_Act"].ToString());

                        TBDCNo.Text = dt.Rows[0]["DCNO"].ToString();
                        TBDatTim.Text = dt.Rows[0]["DatTim"].ToString();
                        TBBiltyNo.Text = dt.Rows[0]["BiltyNo"].ToString();
                        TBVNo.Text = dt.Rows[0]["VehicalNo"].ToString();
                        TBDrvNam.Text = dt.Rows[0]["DriverNam"].ToString();
                        TBDrvMobNo.Text = dt.Rows[0]["DriverMobilno"].ToString();
                        TBTrans.Text = dt.Rows[0]["Transporter"].ToString();
                        TbStaton.Text = dt.Rows[0]["station"].ToString();
                        TBDelivOrd.Text = dt.Rows[0]["DilverOrdr"].ToString();
                        TBFright.Text = dt.Rows[0]["frieght"].ToString();

                        //string cmdDettxt = "select MPurchase.MPurID,'0.00' as [ProNam],  '0.00' as [Particulars],DPurchase.DPurID as [DPurID], productid,ProDes,Qty,'0.00' as [Weight],  '0.00' as [Rate], unit,Cost, SaleTax as 'Sales Tax','0.00' as [Purchase Rate], '0.00' as [Sale Rate], " +
                        //    " '0.00' as [Debit Amount], '0.00' as [Credit Amount],NetTotal from MPurchase " +
                        //    " inner join DPurchase on MPurchase.MPurID = DPurchase.MPurID where ck_Act = 1 and MPurchase.MPurID ='" + MPurID + "'";

                        // 2nd Query

                        //string cmdDettxt = " select distinct(tbl_Mstk.Mstk_id),MPurchase.MPurID,'0.00' as [ProNam],  '0.00' as [Particulars], " +
                        //    " DPurchase.DPurID as [DPurID], DPurchase.productid,ProDes,Qty,'0.00' as [Weight], " +
                        //    " '0.00' as [Rate], unit,Cost, SaleTax as 'Sales Tax', " +
                        //    " Dstk_salrat as [Sale Rate],Dstk_purrat as [Purchase Rate],'0.00' as [Debit Amount], '0.00' as [Credit Amount],NetTotal from MPurchase  " +
                        //    " inner join DPurchase on MPurchase.MPurID = DPurchase.MPurID " +
                        //    " inner join tbl_Mstk on tbl_Mstk.MPurID = MPurchase.MPurID " +
                        //    " inner join tbl_Dstk on tbl_Mstk.Mstk_id = tbl_Dstk.Mstk_id " +
                        //    " where ck_Act = 1 and MPurchase.MPurID = '" + MPurID + "'";

                        string cmdDettxt = " select distinct(DPurchase.productid), DPurchase.productid as [ProNam],ProDes,Qty,'0.00' as [percent],'0.00' as [Weight],'0.00' as [Rate], unit,Cost, SaleTax as 'Sales Tax', " +
                            " Dstk_salrat as [Sale Rate],Dstk_purrat as [Purchase Rate], DPurID, " +
                            " '0.00' as [Debit Amount], '0.00' as [Credit Amount],NetTotal from DPurchase " +
                            " inner join tbl_Dstk on DPurchase.ProductID = tbl_Dstk.ProductID " +
                            " where DPurchase.MPurID = " + MPurID + " and DPurchase.CompanyId = '" + Session["CompanyID"] + "' and DPurchase.BranchId= '" + Session["BranchID"] + "'";

                        DataTable dt_Det = new DataTable();
                        dt_Det = DataAccess.DBConnection.GetDataTable(cmdDettxt);

                        if (dt_Det.Rows.Count > 0)
                        {
                            GVPurItems.DataSource = dt_Det;
                            GVPurItems.DataBind();
                            ViewState["dt_adItm"] = dt_Det;

                            forDetalItm();
                            for (int j = 0; j < dt_Det.Rows.Count; j++)
                            {
                                for (int i = 0; i < GVPurItems.Rows.Count; i++)
                                {
                                    Label lbl_pro = (Label)GVPurItems.Rows[i].FindControl("lblPurItm");
                                    DropDownList DDL_Pro = (DropDownList)GVPurItems.Rows[i].FindControl("ddlPurItm");
                                    DDL_Pro.SelectedValue = lbl_pro.Text.Trim();
                                    HiddenField HFDPur = (HiddenField)GVPurItems.Rows[i].FindControl("HFDPur");
                                    HFDPur.Value = dt_Det.Rows[j]["DPurID"].ToString();
                                    Label lbl_Flag = (Label)GVPurItems.Rows[i].FindControl("lbl_Flag");
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lbl_Heading.Text = "Alert!";
                        lblalert.Text ="No Record Found!!";
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }
        }

        protected void GVScrhMPur_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string MPurID = Server.HtmlDecode(GVScrhMPur.Rows[e.RowIndex].Cells[0].Text.ToString());
                string MPurNo = Server.HtmlDecode(GVScrhMPur.Rows[e.RowIndex].Cells[1].Text.ToString());

                SqlCommand cmd = new SqlCommand();

                cmd = new SqlCommand("sp_del_Pur", con);
                cmd.Parameters.Add("@mPurID", SqlDbType.Int).Value = MPurID;
                cmd.Parameters.Add("@CompanyId", SqlDbType.VarChar).Value = Session["CompanyID"];
                cmd.Parameters.Add("@BranchId", SqlDbType.VarChar).Value = Session["BranchID"];

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lblalert.Text = MPurNo + " has been Deleted!";

                Response.Redirect("frm_Purchase.aspx");

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }
        }

        protected void GVScrhMPur_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //GVScrhMPur.PageIndex = e.NewSelectedIndex;
            //FillGrid();
        }

        protected void DDL_Paytyp_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (DDL_Paytyp.SelectedValue == "2")
            {
                v_vendor.Text = "";
                DDL_Paytyp.Enabled = true;
                DDL_Vchtyp.Enabled = true;
                pnl_bnk.Visible = true;
                pnl_Chqno.Visible = true;
                pnl_chqamt.Visible = true;
                pnl_cshamt.Visible = false;
                TB_CshAmt.Text = "";
                //DDL_Bnk.Focus();
            }
            else if (DDL_Paytyp.SelectedValue != "2")
            {
                v_vendor.Text = "";
                DDL_Paytyp.Enabled = true;
                DDL_Vchtyp.Enabled = true;
                pnl_bnk.Visible = false;
                pnl_chqamt.Visible = false;
                pnl_Chqno.Visible = false;
                pnl_cshamt.Visible = true;
                //TB_CshAmt.Focus();

                TBChqNo.Text = "";
                TB_ChqAmt.Text = "";
                DDL_Bnk.SelectedValue = "0";
            }
        }



        private void Update()
        {
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
                if (TBOutstand.Text != "0.00")
                {
                    totalprev = Convert.ToDecimal(txt_outstand.Text) + Convert.ToDecimal(TBOutstand.Text);
                }
                else if (TBOutstand.Text == "0.00")
                {
                    totalprev = 0;
                }//else
               // {
                //    totalprev = Convert.ToInt32(txt_outstand.Text);
               // }

                #region Credit Sheets

                command.CommandText = "select CredAmt from tbl_Purcredit where supplierId='" + HFSupAcc.Value.Trim() + "'";

                SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                DataTable dtsalcre = new DataTable();
                stksalcre.Fill(dtsalcre);

                if (dtsalcre.Rows.Count > 0)
                {
                    //double recv = Convert.ToDecimal(lblOutstan) - Convert.ToDecimal(TBRecy);

                    command.CommandText = " Update tbl_Purcredit set CredAmt = '" + totalprev + "' where supplierId='" + HFSupAcc.Value.Trim() + "'";
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.CommandText = " insert into tbl_Purcredit (supplierId,CredAmt) values('" + HFSupAcc.Value.Trim() + "','" + totalprev + "')";
                    command.ExecuteNonQuery();
                }

                #endregion

                command.CommandText =
                    " update MPurchase set VndrNam = '" + ddlVenNam.SelectedItem.Text + "',subheadcategoryfourID = '', VndrAdd = '', VndrCntct = '', PurNo = '" + TBPONum.Text.Trim() + "', MPurDate = '" + Convert.ToDateTime(TBPurDat.Text).ToString("MM/dd/yyyy") + "', CreatedBy = '" + Session["user"].ToString() + "', CreatedAt = '" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "', CNIC = '', NTNNo = '', ToBePrntd = '" + chk_prtd.Checked.ToString() + "', ck_Act = '" + chk_Act.Checked.ToString() + "', MPurRmk = '" + TBRmk.Text + "', ven_id = '" + ddlVenNam.SelectedValue.Trim() + "', PayTyp_id = '" + DDL_Paytyp.SelectedValue.Trim() + "', vchtyp_id = '" + DDL_Vchtyp.SelectedValue.Trim() + "', csh_amt = '" + TB_CshAmt.Text + "', CashBnk_id = '" + DDL_Bnk.SelectedValue.Trim() + "', chque_No = '" + TBChqNo.Text + "', Out_Standing =" + totalprev + ", grssttl='" + TBGrssTotal.Text + "', CompanyId='" + Session["CompanyID"] + "', BranchId='" + Session["BranchID"] + "', DCNO='" + TBDCNo.Text + "', DatTim='" + TBDatTim.Text + "', BiltyNo='" + TBBiltyNo.Text + "', VehicalNo='" + TBVNo.Text + "', DriverNam='" + TBDrvNam.Text + "', DriverMobilno='" + TBDrvMobNo.Text + "', Transporter='" + TBTrans.Text + "', station='" + TbStaton.Text + "', DilverOrdr='" + TBDelivOrd.Text + "', frieght='" + TBFright.Text + "' where MPurID ='" + HFMPur.Value.Trim() + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                command.ExecuteNonQuery();

                 
                //Master Stock

                command.CommandText = "UPDATE [dbo].[tbl_Mstk]   SET [Mstk_sono] = '" + TBPONum.Text.Trim() + "'" +
                    " ,[Mstk_dat] = '" + Convert.ToDateTime(TBPurDat.Text).ToString("MM/dd/yyyy") + "'" +
                    " ,[Mstk_PurDat] = '" + Convert.ToDateTime(TBPurDat.Text).ToString("MM/dd/yyyy") + "'" +
                    " ,[Mstk_Rmk] = '" + TBRmk.Text + "' ,[ven_id] = " + ddlVenNam.SelectedValue.Trim() +
                    " ,[CustomerID] = '',[MPurID] = " + HFMPur.Value.Trim() +
                    " ,[CreatedBy] = '" + Session["user"].ToString() + "'" +
                    " ,[CreatedAt] = '" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "'" +
                    " ,[ISActive] = '" + chk_Act.Checked.ToString() + "'" + " ,[MSal_id] = '' ,[wh_id] = 1 WHERE Mstk_sono ='" + TBPONum.Text.Trim() + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                command.ExecuteNonQuery();

                foreach (GridViewRow g1 in GVPurItems.Rows)
                {
                    string PurItm = (g1.FindControl("ddlPurItm") as DropDownList).SelectedValue;
                    string PurItmDscptin = (g1.FindControl("TbaddPurItmDscptin") as TextBox).Text;
                    string PurItmQty = (g1.FindControl("TbAddPurItmQty") as TextBox).Text;
                    //string Weight = (g1.FindControl("Tbwght") as TextBox).Text;
                    //string PurUnit = (g1.FindControl("ddlPurUnit") as DropDownList).SelectedItem.Text;
                    //string Cost = (g1.FindControl("TbAddCosts") as TextBox).Text;
                    //string SalTax = (g1.FindControl("TbSalTax") as TextBox).Text;
                    string PurRate = (g1.FindControl("TbPurRat") as Label).Text;
                    string SalRat = (g1.FindControl("TbSalRat") as Label).Text;
                    string Particulars = (g1.FindControl("ddl_Prac") as DropDownList).SelectedValue;
                    //string DBAmt = (g1.FindControl("Tbdbamt") as TextBox).Text;
                    //string CRAmt = (g1.FindControl("Tbcramt") as TextBox).Text;
                    string PurNetTtl = (g1.FindControl("TbAddPurNetTtl") as TextBox).Text;
                    string DPurID = (g1.FindControl("HFDPur") as HiddenField).Value;
                    string lbl_Flag = (g1.FindControl("lbl_Flag") as Label).Text;

                    //Detail Purchase

                    if (DPurID != "")
                    {
                        command.CommandText =
                            " update DPurchase set  ProductID ='" + PurItm + "' , ProDes='" + PurItmDscptin + "', Qty = '" + PurItmQty + "', Total=" + PurNetTtl + ", SubTotl = '0.00', SaleTax='0.00', NetTotal =" + PurNetTtl + ", Unit='0.00', Cost='0.00', ProTyp='', GrossTtal=" + TBGrssTotal.Text.Trim() + ", CreatedBy='" + Session["user"].ToString() + "', CreatedAt='" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "' where MPurID ='" + HFMPur.Value.Trim() + "' and DPurID='" + DPurID + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                       // command.CommandText =
                       //" INSERT INTO DPurchase (MPurID, ProductID, ProDes, Qty, Total, SubTotl, SaleTax, NetTotal, Unit, Cost, ProTyp, GrossTtal, CreatedBy, CreatedAt) " +
                       //" VALUES " +
                       //" ('" + HFMPur.Value.Trim() + "', '" + PurItm + "', '" + PurItmDscptin + "','" + PurItmQty + "'," + PurNetTtl + ", '0.00', '0.00', " + PurNetTtl + ",'0.00','0.00', '', " + TBGrssTotal.Text.Trim() + ",'" + Session["user"].ToString() + "','" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "')";
                       // command.ExecuteNonQuery();

                        command.CommandText =
                           " INSERT INTO DPurchase (MPurID, ProductID, ProDes, Qty, Total, SubTotl, SaleTax, NetTotal, Unit, Cost, ProTyp, GrossTtal, CreatedBy, CreatedAt,CompanyId, BranchId) " +
                           " VALUES " +
                           " ('" + HFMPur.Value.Trim() + "', '" + PurItm + "', '" + PurItmDscptin + "','" + PurItmQty + "','" + PurNetTtl + "', '0.00', '0.00', '" + PurNetTtl + "','0.00',0.00, '', 0.00,'" + Session["user"].ToString() + "','" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "','" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";
                        command.ExecuteNonQuery();
                    }



                    //Detail Stock

                    command.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + PurItm + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    DataTable dtstkqt = new DataTable();

                    SqlDataAdapter Adapter = new SqlDataAdapter(command);
                    Adapter.Fill(dtstkqt);

                    if (dtstkqt.Rows.Count > 0)
                    {
                        for (int t = 0; t < dtstkqt.Rows.Count; t++)
                        {
                            stkqty = dtstkqt.Rows[t]["Dstk_ItmQty"].ToString();

                            decimal qty = Convert.ToDecimal(stkqty) + Convert.ToDecimal(PurItmQty);
                            
                            if (lbl_Flag == "0")
                            {
                                command.CommandText = " UPDATE tbl_Dstk SET Dstk_ItmQty = " + qty + " where  ProductID = " + PurItm + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        // Master Stock
                        command.CommandText = "select Mstk_id from tbl_Mstk where Mstk_sono = '" + TBPONum.Text.Trim() + "' and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                        SqlDataAdapter adpstk = new SqlDataAdapter(command);

                        DataTable dtstk = new DataTable();
                        adpstk.Fill(dtstk);

                        if (dtstk.Rows.Count > 0)
                        {
                            MSTkId = dtstk.Rows[0]["Mstk_id"].ToString();
                        }

                        //command.CommandText = " INSERT INTO tbl_Dstk (Dstk_ItmDes, Dstk_ItmQty, Dstk_Itmwght, Dstk_ItmUnt, Dstk_rat, Dstk_salrat, Dstk_purrat, Mstk_id, ProductID) " +
                        //        " VALUES " +
                        //        " ('" + PurItmDscptin + "','" + PurItmQty + "', '0.00','0.00','0.00','" + SalRat + "', '" + PurRate + "', '" + MSTkId + "', '" + PurItm + "')";
                        //command.ExecuteNonQuery();
                        //command.CommandText = " UPDATE [dbo].[tbl_Dstk] SET [Dstk_ItmDes] = "+ PurItmDscptin +
                        //    " ,[Dstk_ItmQty] = " + PurItmQty + " ,[Dstk_Itmwght] = '' ,[Dstk_ItmUnt] = '' " +
                        //    " ,[Dstk_rat] = '' ,[Dstk_salrat] = " + SalRat +
                        //    " ,[Dstk_purrat] =  " + PurRate + " ,[Mstk_id] = MSTkId " +
                        //    " ,[ProductID] = " + PurItm + " WHERE [Mstk_id] = MSTkId ";
 
                        //command.ExecuteNonQuery();
                        command.CommandText = " INSERT INTO tbl_Dstk (Dstk_ItmDes, Dstk_ItmQty, Dstk_Itmwght, Dstk_ItmUnt, Dstk_rat, Dstk_salrat, Dstk_purrat, Mstk_id, ProductID, CompanyId, BranchId) " +
                                  " VALUES " +
                                  " ('" + PurItmDscptin + "','" + PurItmQty + "', '0.00','0.00','0.00','" + SalRat + "', '" + PurRate + "', '" + MSTkId + "', '" + PurItm + "','" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";
                        command.ExecuteNonQuery();

                    }

                }
                // Attempt to commit the transaction.
                transaction.Commit();

                if (chk_prtd.Checked == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/rpt_Purchase.aspx?ID=PR','_blank','height=600px,width=600px,scrollbars=1');", true);
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
                //FillGrid();
                //ptnSno();
                Response.Redirect("frm_Purchase.aspx");
                //BindDll();
            }

        }
        //private void SaveStk()
        //{

        //    try
        //    {
        //        //AddPurchase();
        //        command.CommandText =
        //            " INSERT INTO tbl_Mstk(Mstk_sono, Mstk_dat, Mstk_PurDat, Mstk_Rmk, ven_id, CustomerID, MPurID, CreatedBy, CreatedAt, ISActive) " +
        //            " VALUES " +
        //            " ('','2018/11/28','2018/11/28','','" + ddlVenNam.SelectedValue.Trim() + "', '','" + MPurId + "','" + Session["user"].ToString() + "','" + DateTime.Today.ToString("yyyy/MM/dd") + "','true')";
        //        command.ExecuteNonQuery();

                
        //        //if (chk_prt.Checked == true)
        //        //{
        //            //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'ReportViewer.aspx?ID=PR','_blank','height=600px,width=600px,scrollbars=1');", true);
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
        //        Console.WriteLine("  Message: {0}", ex.Message);
        //    }
 
        //}

        private int SavePur()
        {

            con.Open();

            int res;

            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("SampleTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                #region Purchase Record
                

                string totalprev = "0.0";



                if (TBOutstand.Text != "0.00")
                {
                    if (txt_outstand.Text == "")
                    {
                        txt_outstand.Text = "0.00";
                    }
                    else
                    {
                        totalprev = (Convert.ToDecimal(txt_outstand.Text) + Convert.ToDecimal(TBOutstand.Text)).ToString();
                    }
                }
                else
                {
                    totalprev = txt_outstand.Text;
                }
                if (HFMPur.Value == "0")
                {
                    command.CommandText =
                                " INSERT INTO MPurchase(VndrNam,subheadcategoryfourID, VndrAdd, VndrCntct, PurNo, MPurDate, CreatedBy, CreatedAt, CNIC, NTNNo, ToBePrntd, ck_Act, MPurRmk, ven_id, PayTyp_id, vchtyp_id, csh_amt, CashBnk_id, chque_No, Out_Standing, grssttl, CompanyId, BranchId) " +
                                " VALUES " +
                                " ('" + ddlVenNam.SelectedItem.Text + "', '', '', '', '" +
                                TBPONum.Text.Trim() + "','" + Convert.ToDateTime(TBPurDat.Text).ToString("MM/dd/yyyy") + "','" +
                                Session["user"].ToString() + "','" +
                                DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "', '', '', '" +
                                chk_prtd.Checked.ToString() + "', '" + chk_Act.Checked.ToString() + "', '" + TBRmk.Text +
                                "', '" + ddlVenNam.SelectedValue.Trim() + "', '" + DDL_Paytyp.SelectedValue.Trim() +
                                "', '" + DDL_Vchtyp.SelectedValue.Trim() + "', '" + TB_CshAmt.Text + "', '" +
                                DDL_Bnk.SelectedValue.Trim() + "', '" + TBChqNo.Text + "','" + totalprev + "','" + TBGrssTotal.Text + "','" + Session["CompanyID"] +
                                "','" + Session["BranchID"] + "')";

                    command.ExecuteNonQuery();
                }
                else
                {
                    totalprev = (Convert.ToDecimal(txt_outstand.Text) - Convert.ToDecimal(TBOutstand.Text)).ToString();

                    command.CommandText = " UPDATE MPurchase SET Out_Standing = '" + totalprev + "' where  ven_id =" + ddlVenNam.SelectedValue + "";

                    command.ExecuteNonQuery();
                }

                // Master Purchase
                command.CommandText = "select MPurID from mpurchase where PurNo= '" + TBPONum.Text.Trim() + "'";

                SqlDataAdapter adp = new SqlDataAdapter(command);

                DataTable dt = new DataTable();
                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    MPurId = dt.Rows[0]["MPurID"].ToString();
                }

                //Save Master Stock
                command.CommandText =
                    " INSERT INTO tbl_Mstk(Mstk_sono, Mstk_dat, Mstk_PurDat, Mstk_Rmk, ven_id, CustomerID, MPurID, CreatedBy, CreatedAt, ISActive, CompanyId, BranchId) " +
                    " VALUES " +
                    " ('" + TBPONum.Text.Trim() + "','2018/11/28','2018/11/28','" + TBRmk.Text.Trim() + "','" + ddlVenNam.SelectedValue.Trim() + "', '','" + MPurId + "','" + Session["user"].ToString() + "','" + DateTime.Today.ToString("yyyy/MM/dd") + "','true', '" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";
                command.ExecuteNonQuery();

                // Master Stock
                command.CommandText = "select Mstk_id from tbl_Mstk where Mstk_sono = '" + TBPONum.Text.Trim() + "'";

                SqlDataAdapter adpstk = new SqlDataAdapter(command);

                DataTable dtstk = new DataTable();
                adpstk.Fill(dtstk);

                if (dtstk.Rows.Count > 0)
                {
                    MSTkId = dtstk.Rows[0]["Mstk_id"].ToString();
                }


                foreach (GridViewRow g1 in GVPurItems.Rows)
                {
                    PurItm = (g1.FindControl("ddlPurItm") as DropDownList).SelectedValue;
                    PurItmDscptin = (g1.FindControl("TbaddPurItmDscptin") as TextBox).Text;
                    PurItmQty = (g1.FindControl("TbAddPurItmQty") as TextBox).Text;
                    //string Weight = (g1.FindControl("Tbwght") as TextBox).Text;
                    //string PurUnit = (g1.FindControl("ddlPurUnit") as DropDownList).SelectedItem.Text;
                    //string Cost = (g1.FindControl("TbAddCosts") as TextBox).Text;
                    //string SalTax = (g1.FindControl("TbSalTax") as TextBox).Text;
                    PurRate = (g1.FindControl("TbPurRat") as Label).Text;
                    SalRat = (g1.FindControl("TbSalRat") as Label).Text;
                    Particulars = (g1.FindControl("ddl_Prac") as DropDownList).SelectedValue;
                    //string DBAmt = (g1.FindControl("Tbdbamt") as TextBox).Text;
                    //string CRAmt = (g1.FindControl("Tbcramt") as TextBox).Text;
                    PurNetTtl = (g1.FindControl("TbAddPurNetTtl") as TextBox).Text;

                    //Detail Purchase,
                    command.CommandText =
                        " INSERT INTO DPurchase (MPurID, ProductID, ProDes, Qty, Total, SubTotl, SaleTax, NetTotal, Unit, Cost, ProTyp, GrossTtal, CreatedBy, CreatedAt,CompanyId, BranchId) " +
                        " VALUES " +
                        " ('" + MPurId + "', '" + PurItm + "', '" + PurItmDscptin + "','" + PurItmQty + "','" + PurNetTtl + "', '0.00', '0.00', '" + PurNetTtl + "','0.00',0.00, '', 0.00,'" + Session["user"].ToString() + "','" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "','" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";
                    command.ExecuteNonQuery();

                    //Detail Stock

                    /*DataTable dtstkqty = new DataTable();


                    command.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + PurItm + "";

                    SqlDataAdapter Adapter = new SqlDataAdapter(command);
                    Adapter.Fill(dtstkqty);

                    if (dtstkqty.Rows.Count > 0)
                    {
                        for (int t = 0; t < dtstkqty.Rows.Count; t++)
                        {
                            stkqty = dtstkqty.Rows[t]["Dstk_ItmQty"].ToString();

                            int qty = Convert.ToInt32(stkqty) + Convert.ToInt32(PurItmQty);
                            command.CommandText = " UPDATE tbl_Dstk SET Dstk_ItmQty = " + qty + " where  ProductID = " + PurItm + "";
                            command.ExecuteNonQuery();

                        }
                    }
                    else*/
                    //{
                        command.CommandText = " INSERT INTO tbl_Dstk (Dstk_ItmDes, Dstk_ItmQty, Dstk_Itmwght, Dstk_ItmUnt, Dstk_rat, Dstk_salrat, Dstk_purrat, Mstk_id, ProductID, CompanyId, BranchId) " +
                                " VALUES " +
                                " ('" + PurItmDscptin + "','" + PurItmQty + "', '0.00','0.00','0.00','" + SalRat + "', '" + PurRate + "', '" + MSTkId + "', '" + PurItm + "','" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";
                        command.ExecuteNonQuery();

                    //}

                }
                #endregion
                #region Stock Record


                #endregion

                // Attempt to commit the transaction.
                transaction.Commit();

                if (chk_prtd.Checked == true)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'ReportViewer.aspx?ID=PR','_blank','height=600px,width=600px,scrollbars=1');", true);
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
                res = 1;

            }

            return res;
        }
        private int SavePurchase()
        {

            con.Open();

            int res;
             
            SqlCommand command = con.CreateCommand();
            SqlTransaction transaction;

            // Start a local transaction.
            transaction = con.BeginTransaction("SampleTransaction");

            // Must assign both transaction object and connection 
            // to Command object for a pending local transaction
            command.Connection = con;
            command.Transaction = transaction;

            try
            {
                #region Purchase Record

                //if (TBOutstand.Text != "0.00")
                //    {
                //        if (txt_outstand.Text == "")
                //        {
                //            txt_outstand.Text = "0.00";
                //        }
                //        else
                //        {
                //            totalprev = Convert.ToDecimal(txt_outstand.Text) + Convert.ToDecimal(TBOutstand.Text);
                //        }
                //    }
                //    else
                //    {
                //        totalprev = Convert.ToInt32(txt_outstand.Text);
                //    }

                    string totalprev = "0.0";



                    if (TBOutstand.Text != "0.00")
                    {
                        if (txt_outstand.Text == "")
                        {
                            txt_outstand.Text = "0.00";
                        }
                        else
                        {
                            totalprev = (Convert.ToDecimal(txt_outstand.Text) + Convert.ToDecimal(TBOutstand.Text)).ToString();
                        }
                    }
                    else
                    {
                        totalprev = txt_outstand.Text;
                    }

                    #region Credit Sheets

                    command.CommandText = "select CredAmt from tbl_Purcredit where supplierId='" + HFSupAcc.Value.Trim() + "'";

                    SqlDataAdapter stksalcre = new SqlDataAdapter(command);

                    DataTable dtsalcre = new DataTable();
                    stksalcre.Fill(dtsalcre);

                    if (dtsalcre.Rows.Count > 0)
                    {
                        //double recv = Convert.ToDecimal(lblOutstan) - Convert.ToDecimal(TBRecy);

                        command.CommandText = " Update tbl_Purcredit set CredAmt = '" + TBGrssTotal.Text.Trim() + "' where supplierId='" + HFSupAcc.Value.Trim() + "'";
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = " insert into tbl_Purcredit (supplierId,CredAmt) values('" + HFSupAcc.Value.Trim() + "','" + TBGrssTotal.Text.Trim() + "')";
                        command.ExecuteNonQuery();
                    }

                    #endregion

                    if (HFMPur.Value == "0")
                    {
                        command.CommandText =
                                    " INSERT INTO MPurchase(VndrNam,subheadcategoryfourID, VndrAdd, VndrCntct, PurNo, MPurDate, CreatedBy, CreatedAt, " +
                                    " CNIC, NTNNo, ToBePrntd, ck_Act, MPurRmk, ven_id, PayTyp_id, vchtyp_id, csh_amt, CashBnk_id, chque_No, Out_Standing, " +
                                    " grssttl, CompanyId, BranchId, DCNO, DatTim, BiltyNo, VehicalNo, DriverNam, DriverMobilno, Transporter, station, DilverOrdr, frieght, NetTotal, GrossTtal, PayAcc) " +
                                    " VALUES " +
                                    " ('" + ddlVenNam.SelectedItem.Text + "', '', '', '', '" +
                                    TBPONum.Text.Trim() + "','" + Convert.ToDateTime(TBPurDat.Text).ToString("MM/dd/yyyy") + "','" +
                                    Session["user"].ToString() + "','" +
                                    DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "', '', '', '" +
                                    chk_prtd.Checked.ToString() + "', '" + chk_Act.Checked.ToString() + "', '" + TBRmk.Text +
                                    "', '" + ddlVenNam.SelectedValue.Trim() + "', '" + DDL_Paytyp.SelectedValue.Trim() +
                                    "', '" + DDL_Vchtyp.SelectedValue.Trim() + "', '" + TB_CshAmt.Text + "', '" +
                                    DDL_Bnk.SelectedValue.Trim() + "', '" + TBChqNo.Text + "','" + totalprev + "','" + TBGrssTotal.Text + "','"+ Session["CompanyID"] +
                                    "','" + Session["BranchID"] + "','" + TBDCNo.Text + "','" + TBDatTim.Text + "','" + TBBiltyNo.Text + "','" + TBVNo.Text +
                                    "','" + TBDrvNam.Text + "','" + TBDrvMobNo.Text + "','" + TBTrans.Text + "','" + TbStaton.Text +
                                    "','" + TBDelivOrd.Text + "','" + TBFright.Text + "','" + TBTtl.Text.Trim() + "','" + TBGrssTotal.Text.Trim() + "','" + HFSupAcc.Value.Trim() + "')";

                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        totalprev = (Convert.ToDecimal(txt_outstand.Text) - Convert.ToDecimal(TBOutstand.Text)).ToString();

                        command.CommandText = " UPDATE MPurchase SET Out_Standing = '" + totalprev + "' where  ven_id =" + ddlVenNam.SelectedValue + "";

                        command.ExecuteNonQuery();
                    }

                    // Master Purchase
                    command.CommandText = "select MPurID from mpurchase where PurNo= '" + TBPONum.Text.Trim() + "'";

                    SqlDataAdapter adp = new SqlDataAdapter(command);

                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        MPurId = dt.Rows[0]["MPurID"].ToString();
                    }

                        //Save Master Stock
                        command.CommandText =
                            " INSERT INTO tbl_Mstk(Mstk_sono, Mstk_dat, Mstk_PurDat, Mstk_Rmk, ven_id, CustomerID, MPurID, CreatedBy, CreatedAt, ISActive, CompanyId, BranchId) " +
                            " VALUES " +
                            " ('" + TBPONum.Text.Trim() + "','2018/11/28','2018/11/28','" + TBRmk.Text.Trim() + "','" + ddlVenNam.SelectedValue.Trim() + "', '','" + MPurId + "','" + Session["user"].ToString() + "','" + DateTime.Today.ToString("yyyy/MM/dd") + "','true', '" + Session["CompanyID"] + "','"+ Session["BranchID"] + "')";
                        command.ExecuteNonQuery();

                        // Master Stock
                        command.CommandText = "select Mstk_id from tbl_Mstk where Mstk_sono = '" + TBPONum.Text.Trim() + "'";

                        SqlDataAdapter adpstk = new SqlDataAdapter(command);

                        DataTable dtstk = new DataTable();
                        adpstk.Fill(dtstk);

                        if (dtstk.Rows.Count > 0)
                        {
                            MSTkId = dtstk.Rows[0]["Mstk_id"].ToString();
                        }


                    foreach (GridViewRow g1 in GVPurItems.Rows)
                    {
                        PurItm = (g1.FindControl("ddlPurItm") as DropDownList).SelectedValue;
                        PurItmDscptin = (g1.FindControl("TbaddPurItmDscptin") as TextBox).Text;
                        PurItmQty = (g1.FindControl("TbAddPurItmQty") as TextBox).Text;
                        //string Weight = (g1.FindControl("Tbwght") as TextBox).Text;
                        //string PurUnit = (g1.FindControl("ddlPurUnit") as DropDownList).SelectedItem.Text;
                        //string Cost = (g1.FindControl("TbAddCosts") as TextBox).Text;
                        //string SalTax = (g1.FindControl("TbSalTax") as TextBox).Text;
                        PurRate = (g1.FindControl("TbPurRat") as Label).Text;
                        SalRat = (g1.FindControl("TbSalRat") as Label).Text;
                        Particulars = (g1.FindControl("ddl_Prac") as DropDownList).SelectedValue;
                        //string DBAmt = (g1.FindControl("Tbdbamt") as TextBox).Text;
                        //string CRAmt = (g1.FindControl("Tbcramt") as TextBox).Text;
                        PurNetTtl = (g1.FindControl("TbAddPurNetTtl") as TextBox).Text;

                        //Detail Purchase,
                        command.CommandText =
                            " INSERT INTO DPurchase (MPurID, ProductID, ProDes, Qty, Total, SubTotl, SaleTax, NetTotal, Unit, Cost, ProTyp, GrossTtal, CreatedBy, CreatedAt,CompanyId, BranchId) " +
                            " VALUES " +
                            " ('" + MPurId + "', '" + PurItm + "', '" + PurItmDscptin + "','" + PurItmQty + "','" + PurNetTtl + "', '0.00', '0.00', '" + PurNetTtl + "','0.00',0.00, '', 0.00,'" + Session["user"].ToString() + "','" + DateTime.Today.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "','" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";
                        command.ExecuteNonQuery();


                        DataTable dtstkqty = new DataTable();

                        //Detail Stock

                        command.CommandText = "select Dstk_ItmQty from tbl_Dstk where ProductID = " + PurItm + "";

                        SqlDataAdapter Adapter = new SqlDataAdapter(command);
                        Adapter.Fill(dtstkqty);

                        if (dtstkqty.Rows.Count > 0)
                        {
                            for (int t = 0; t < dtstkqty.Rows.Count; t++)
                            {
                                stkqty = dtstkqty.Rows[t]["Dstk_ItmQty"].ToString();

                                decimal qty = Convert.ToDecimal(stkqty) + Convert.ToDecimal(PurItmQty);
                                command.CommandText = " UPDATE tbl_Dstk SET Dstk_ItmQty = " + qty + ", Dstk_rat='" + PurRate + "', Dstk_purrat='" +
                                    PurRate + "', Dstk_salrat='" + SalRat + "'  where  ProductID = '" + PurItm + "'";

                                command.ExecuteNonQuery();

                            }
                        }
                        else
                        {
                            command.CommandText = " INSERT INTO tbl_Dstk (Dstk_ItmDes, Dstk_ItmQty, Dstk_Itmwght, Dstk_ItmUnt, Dstk_rat, Dstk_salrat, Dstk_purrat, Mstk_id, ProductID, CompanyId, BranchId) " +
                                    " VALUES " +
                                    " ('" + PurItmDscptin + "','" + PurItmQty + "', '0.00','0.00','0.00','" + SalRat + "', '" + PurRate + "', '" + MSTkId + "', '" + PurItm + "','" + Session["CompanyID"] + "','" + Session["BranchID"] + "')";
                            command.ExecuteNonQuery();

                        }

                    }
                   #endregion
                #region Stock Record


                    #endregion

                // Attempt to commit the transaction.
                transaction.Commit();

                if (chk_prtd.Checked == true)
                {
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/rpt_Purchase.aspx?ID=PR','_blank','height=600px,width=600px,scrollbars=1');", true);                

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "onclick", "javascript:window.open( 'Reports/rpt_Purchase.aspx?ID=PR&PURID=" + MPurId + "','_blank','height=600px,width=600px,scrollbars=1');", true);                

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
                res = 1;

            }

            return res;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            if (HFMPur.Value == "0")
            {
                int i = 0;
                if (TBPurDat.Text == "")
                {
                    lbl_Gttl.Visible = false;
                    lbl_ttl.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkpurrat.Visible = false;
                    lbl_Itmqty.Visible = false;
                    lbl_Pros.Visible = false;
                    lbl_Ven.Visible = false;
                    lbl_vtyp.Visible = false;
                    lbl_paytyp.Visible = false;
                    lbl_cshamt.Visible = false;
                    lbl_chqamt.Visible = false;
                    lbl_bnk.Visible = false;
                    lbl_chqno.Visible = false;

                    lbl_purdat.Visible = true;
                    lbl_purdat.Text = "Please Select Date...";

                }
                else if (ddlVenNam.SelectedValue == "0")
                {
                    lbl_Gttl.Visible = false;
                    lbl_ttl.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkpurrat.Visible = false;
                    lbl_Itmqty.Visible = false;
                    lbl_Pros.Visible = false;
                    lbl_purdat.Visible = false;
                    lbl_vtyp.Visible = false;
                    lbl_paytyp.Visible = false;
                    lbl_cshamt.Visible = false;
                    lbl_chqamt.Visible = false;
                    lbl_bnk.Visible = false;
                    lbl_chqno.Visible = false;

                    lbl_Ven.Visible = true;
                    lbl_Ven.Text = "Please Select vendor...";
                }
                else if (DDL_Vchtyp.SelectedValue == "0")
                {
                    lbl_Gttl.Visible = false;
                    lbl_ttl.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkpurrat.Visible = false;
                    lbl_Itmqty.Visible = false;
                    lbl_Pros.Visible = false;
                    lbl_purdat.Visible = false;
                    lbl_Ven.Visible = false;
                    lbl_paytyp.Visible = false;
                    lbl_cshamt.Visible = false;
                    lbl_chqamt.Visible = false;
                    lbl_bnk.Visible = false;
                    lbl_chqno.Visible = false;

                    lbl_vtyp.Visible = true;
                    lbl_vtyp.Text = "Please Select Voucher...";
                }
                else if (DDL_Paytyp.SelectedValue == "0")
                {
                    lbl_Gttl.Visible = false;
                    lbl_ttl.Visible = false;
                    lbl_ChkAmt.Visible = false;
                    lbl_Chkpurrat.Visible = false;
                    lbl_Itmqty.Visible = false;
                    lbl_Pros.Visible = false;
                    lbl_purdat.Visible = false;
                    lbl_Ven.Visible = false;
                    lbl_vtyp.Visible = false;
                    lbl_cshamt.Visible = false;
                    lbl_chqamt.Visible = false;
                    lbl_bnk.Visible = false;
                    lbl_chqno.Visible = false;

                    lbl_paytyp.Visible = true;
                    lbl_paytyp.Text = "Please Select Payment Type...";

                }
                //else if (DDL_Paytyp.SelectedValue != "0")
                //{
                //    if (DDL_Paytyp.SelectedValue == "1")
                //    {
                //        if (TB_CshAmt.Text == "" || TB_CshAmt.Text == "0.00" || TB_CshAmt.Text == "0")
                //        {
                //            lbl_Gttl.Visible = false;
                //            lbl_ttl.Visible = false;
                //            lbl_ChkAmt.Visible = false;
                //            lbl_Chkpurrat.Visible = false;
                //            lbl_Itmqty.Visible = false;
                //            lbl_Pros.Visible = false;
                //            lbl_purdat.Visible = false;
                //            lbl_Ven.Visible = false;
                //            lbl_vtyp.Visible = false;
                //            lbl_paytyp.Visible = false;
                //            lbl_chqamt.Visible = false;
                //            lbl_bnk.Visible = false;
                //            lbl_chqno.Visible = false;

                //            lbl_cshamt.Visible = true;
                //            lbl_cshamt.Text = "Please Write Some thing in Cash Amount...";
                //        }

                //    }
                //    else if (DDL_Paytyp.SelectedValue == "2")
                //    {
                //        if (TB_ChqAmt.Text == "" || TB_ChqAmt.Text == "0.00" || TB_ChqAmt.Text == "0")
                //        {
                //            lbl_Gttl.Visible = false;
                //            lbl_ttl.Visible = false;
                //            lbl_ChkAmt.Visible = false;
                //            lbl_Chkpurrat.Visible = false;
                //            lbl_Itmqty.Visible = false;
                //            lbl_Pros.Visible = false;
                //            lbl_purdat.Visible = false;
                //            lbl_Ven.Visible = false;
                //            lbl_vtyp.Visible = false;
                //            lbl_paytyp.Visible = false;
                //            lbl_cshamt.Visible = false;
                //            lbl_bnk.Visible = false;
                //            lbl_chqno.Visible = false;

                //            lbl_chqamt.Visible = true;
                //            lbl_chqamt.Text = "Please Write Some thing in Cheque Amount...";
                //        }
                //        else if (DDL_Bnk.SelectedValue == "0")
                //        {
                //            lbl_Gttl.Visible = false;
                //            lbl_ttl.Visible = false;
                //            lbl_ChkAmt.Visible = false;
                //            lbl_Chkpurrat.Visible = false;
                //            lbl_Itmqty.Visible = false;
                //            lbl_Pros.Visible = false;
                //            lbl_purdat.Visible = false;
                //            lbl_Ven.Visible = false;
                //            lbl_vtyp.Visible = false;
                //            lbl_paytyp.Visible = false;
                //            lbl_cshamt.Visible = false;
                //            lbl_chqamt.Visible = false;
                //            lbl_chqno.Visible = false;

                //            lbl_bnk.Visible = true;
                //            lbl_bnk.Text = "Please Select Bank...";
                //        }
                //        else if (TBChqNo.Text == "")
                //        {
                //            lbl_Gttl.Visible = false;
                //            lbl_ttl.Visible = false;
                //            lbl_ChkAmt.Visible = false;
                //            lbl_Chkpurrat.Visible = false;
                //            lbl_Itmqty.Visible = false;
                //            lbl_Pros.Visible = false;
                //            lbl_purdat.Visible = false;
                //            lbl_Ven.Visible = false;
                //            lbl_vtyp.Visible = false;
                //            lbl_paytyp.Visible = false;
                //            lbl_cshamt.Visible = false;
                //            lbl_chqamt.Visible = false;
                //            lbl_bnk.Visible = false;

                //            lbl_chqno.Visible = true;
                //            lbl_chqno.Text = "Please Write CHeque No..";
                //        }
                //    }
                    else
                    {
                        lbl_Gttl.Visible = false;
                        lbl_ttl.Visible = false;
                        lbl_ChkAmt.Visible = false;
                        lbl_Chkpurrat.Visible = false;
                        lbl_Itmqty.Visible = false;
                        lbl_Pros.Visible = false;
                        lbl_purdat.Visible = false;
                        lbl_Ven.Visible = false;
                        lbl_vtyp.Visible = false;
                        lbl_paytyp.Visible = false;
                        lbl_cshamt.Visible = false;
                        lbl_chqamt.Visible = false;
                        lbl_bnk.Visible = false;
                        lbl_chqno.Visible = false;

                        chkdetails = 0;

                        for (int j = 0; j < GVPurItems.Rows.Count; j++)
                        {
                            DropDownList ddlPurItm = (DropDownList)GVPurItems.Rows[j].FindControl("ddlPurItm");
                            TextBox TbAddPurItmQty = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurItmQty");
                            TextBox TbAddPurNetTtl = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurNetTtl");

                            if (ddlPurItm.SelectedValue == "0")
                            {
                                lbl_Pros.Text = "Please Select Product!!";
                                lbl_Pros.Visible = true;

                                lbl_Gttl.Visible = false;
                                lbl_ttl.Visible = false;
                                lbl_ChkAmt.Visible = false;
                                lbl_Chkpurrat.Visible = false;
                                lbl_Itmqty.Visible = false;
                                lbl_purdat.Visible = false;
                                lbl_Ven.Visible = false;
                                lbl_vtyp.Visible = false;
                                lbl_paytyp.Visible = false;
                                lbl_cshamt.Visible = false;
                                lbl_chqamt.Visible = false;
                                lbl_bnk.Visible = false;
                                lbl_chqno.Visible = false;
                            }
                            else if (TbAddPurItmQty.Text == "" || TbAddPurItmQty.Text == "0.00" || TbAddPurItmQty.Text == "0")
                            {
                                lbl_Itmqty.Text = "Please Write Quantity!!";
                                lbl_Itmqty.Visible = true;

                                lbl_Gttl.Visible = false;
                                lbl_ttl.Visible = false;
                                lbl_ChkAmt.Visible = false;
                                lbl_Chkpurrat.Visible = false;
                                lbl_Pros.Visible = false;
                                lbl_purdat.Visible = false;
                                lbl_Ven.Visible = false;
                                lbl_vtyp.Visible = false;
                                lbl_paytyp.Visible = false;
                                lbl_cshamt.Visible = false;
                                lbl_chqamt.Visible = false;
                                lbl_bnk.Visible = false;
                                lbl_chqno.Visible = false;
                            }
                            else if (TbAddPurNetTtl.Text == "" || TbAddPurNetTtl.Text == "0.00" || TbAddPurNetTtl.Text == "0")
                            {
                                lbl_ChkAmt.Text = "Please Write in Amount!!";
                                lbl_ChkAmt.Visible = true;

                                lbl_Gttl.Visible = false;
                                lbl_ttl.Visible = false;
                                lbl_Chkpurrat.Visible = false;
                                lbl_Itmqty.Visible = false;
                                lbl_Pros.Visible = false;
                                lbl_purdat.Visible = false;
                                lbl_Ven.Visible = false;
                                lbl_vtyp.Visible = false;
                                lbl_paytyp.Visible = false;
                                lbl_cshamt.Visible = false;
                                lbl_chqamt.Visible = false;
                                lbl_bnk.Visible = false;
                                lbl_chqno.Visible = false;
                            }
                            else if (TBGrssTotal.Text == "" || TBGrssTotal.Text == "0" || TBGrssTotal.Text == "0.00")
                            {
                                lbl_ttl.Visible = false;
                                lbl_ChkAmt.Visible = false;
                                lbl_Chkpurrat.Visible = false;
                                lbl_Itmqty.Visible = false;
                                lbl_Pros.Visible = false;
                                lbl_purdat.Visible = false;
                                lbl_Ven.Visible = false;
                                lbl_vtyp.Visible = false;
                                lbl_paytyp.Visible = false;
                                lbl_cshamt.Visible = false;
                                lbl_chqamt.Visible = false;
                                lbl_bnk.Visible = false;
                                lbl_chqno.Visible = false;

                                lbl_Gttl.Visible = true;
                                lbl_Gttl.Text = "Please Write Some thing in Grand Total...";
                            }
                            else if (TBTtl.Text == "" || TBTtl.Text == "0.00" || TBTtl.Text == "0")
                            {
                                lbl_Gttl.Visible = false;
                                lbl_ChkAmt.Visible = false;
                                lbl_Chkpurrat.Visible = false;
                                lbl_Itmqty.Visible = false;
                                lbl_Pros.Visible = false;
                                lbl_purdat.Visible = false;
                                lbl_Ven.Visible = false;
                                lbl_vtyp.Visible = false;
                                lbl_paytyp.Visible = false;
                                lbl_cshamt.Visible = false;
                                lbl_chqamt.Visible = false;
                                lbl_bnk.Visible = false;
                                lbl_chqno.Visible = false;

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
                            //Save Purchase
                            i = SavePurchase();
                        }
                    }

                    if (i == 1)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lbl_Heading.Text = "Saved!";
                        lblalert.Text = "Purchase has Been Saved!!";

                        Response.Redirect("frm_Purchase.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                        lbl_Heading.Text = "Error!";
                        lblalert.Text = "Some thins is worng Please Contact Administrator";
                    }
                }
                else
                {
                    Update();
                }
            }
        

        public void Clear()
        {
            //TBVenAdd.Text = "";
            //TBVenCon.Text = "";
            TBPONum.Text = "";
            //TBPurDat.Text = "";
            SetInitRowPuritm();
            // BindDll();
            TBGrssTotal.Text = "";
            //TBNIC.Text = "";
            //TBNTN.Text = "";
            
        }

        protected void btnRevert_Click(object sender, EventArgs e)
        {
            Response.Redirect("frm_Purchase.aspx");
        }

        protected void ddlPurItm_SelectedIndexChanged(object sender, EventArgs e)
        {
            v_vendor.Text = "";
            DDL_Vchtyp.Enabled = true;
            DDL_Paytyp.Enabled = true;
            
            try
            {
                for (int j = 0; j < GVPurItems.Rows.Count; j++)
                {
                    DropDownList ddlPurItm = (DropDownList)GVPurItems.Rows[j].FindControl("ddlPurItm");
                    TextBox TBItmDes = (TextBox)GVPurItems.Rows[j].FindControl("TbaddPurItmDscptin");
                    TextBox TBItmQty = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurItmQty");
                    //TextBox Tbwght = (TextBox)GVPurItems.Rows[j].FindControl("Tbwght");
                    //TextBox Tbrat = (TextBox)GVPurItems.Rows[j].FindControl("Tbrat");
                    //TextBox TbItmCst = (TextBox)GVPurItems.Rows[j].FindControl("TbAddCosts");
                    //TextBox TbSalTax = (TextBox)GVPurItems.Rows[j].FindControl("TbSalTax");

                    Label TbPurRat = (Label)GVPurItems.Rows[j].FindControl("TbPurRat");
                    Label TbSalRat = (Label)GVPurItems.Rows[j].FindControl("TbSalRat");

                    //DropDownList ddlPurUnit = (DropDownList)GVPurItems.Rows[j].FindControl("ddlPurUnit");
                    TextBox TBNetTtl = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurNetTtl");
                    Label lbl_Flag = (Label)GVPurItems.Rows[j].FindControl("lbl_Flag");


                    string query = "select ProductID,ProductName,ProductDiscriptions,PckSize,Cost as [Rate],PurchasePrice,SalePrice, Unit, " +
                        " '' as [Net Total] " +
                        " from Products where ProductID = " + ddlPurItm.SelectedValue.Trim() + " and CompanyId = '" + Session["CompanyID"] + "' and BranchId= '" + Session["BranchID"] + "'";

                    SqlCommand cmd = new SqlCommand(query, con);
                    DataTable dt_ = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);

                    adp.Fill(dt_);

                    if (dt_.Rows.Count > 0)
                    {
                        TBItmDes.Text = "0.00";//dt_.Rows[0]["ProductDiscriptions"].ToString();
                        //ddlPurUnit.SelectedItem.Text = dt_.Rows[0]["Unit"].ToString();
                        //TBItmQty.Text = "0.00";
                        //Tbwght.Text = "0.00";
                        //Tbrat.Text = "0.00";
                        //TbItmCst.Text = dt_.Rows[0]["Rate"].ToString();
                        //TbSalTax.Text = "0.00";
                        TbPurRat.Text = dt_.Rows[0]["PurchasePrice"].ToString();
                        TbSalRat.Text = dt_.Rows[0]["SalePrice"].ToString();
                        //lbl_Flag.Text = "1";
                    }

                    //TBItmQty.Focus();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
                //lbl_err.Text = ex.Message.ToString();
            }

        }

        
        protected void ddlVenNam_SelectedIndexChanged(object sender, EventArgs e)
        {
            DDL_Vchtyp.Enabled = true;
            v_vendor.Text = "";
            string query;
            SqlCommand command;

            try
            {
                /// Getting Cust Acc
                /// ...
                /// 

                query = " select sup_acc from supplier where supplierId ='" + ddlVenNam.SelectedValue + "'";

                command = new SqlCommand(query, con);
                con.Open();
                DataTable dtvenacc = new DataTable();
                SqlDataAdapter adp1 = new SqlDataAdapter(command);
                adp1.Fill(dtvenacc);

                if (dtvenacc.Rows.Count > 0)
                {
                    HFSupAcc.Value = dtvenacc.Rows[0]["sup_acc"].ToString();
                }


                /// Getting credit Amount
                /// ...
                /// 

                query = " select isnull(SUM(CredAmt) , 0)as 'OUTSTAND' from tbl_Purcredit where supplierId ='" + HFSupAcc.Value.Trim() + "'";

                command = new SqlCommand(query, con);
                
                DataTable dtven = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(command);
                adp.Fill(dtven);
                command.ExecuteNonQuery();

                if (dtven.Rows.Count > 0)
                {
                    txt_outstand.Text = dtven.Rows[0]["OUTSTAND"].ToString();
                    //DDL_Vchtyp.Focus();
                }
                else
                {
                    txt_outstand.Text = ("0.00").ToString();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }

        }

        protected void TbAddPurItmQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                for (int j = 0; j < GVPurItems.Rows.Count; j++)
                {
                    DropDownList ddlPurItm = (DropDownList)GVPurItems.Rows[j].FindControl("ddlPurItm");
                    TextBox TBItmDes = (TextBox)GVPurItems.Rows[j].FindControl("TbaddPurItmDscptin");
                    TextBox TBItmQty = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurItmQty");
                    //DropDownList ddlPurUnit = (DropDownList)GVPurItems.Rows[j].FindControl("ddlPurUnit");
                    TextBox TbItmCst = (TextBox)GVPurItems.Rows[j].FindControl("TbAddCosts");
                    TextBox TBNetTtl = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurNetTtl");
                    Label TbPurRat = (Label)GVPurItems.Rows[j].FindControl("TbPurRat");
                    Label TbSalRat = (Label)GVPurItems.Rows[j].FindControl("TbSalRat");
                    Label lbl_Flag = (Label)GVPurItems.Rows[j].FindControl("lbl_Flag");

                    

                    //TextBox DBAmtttl = (TextBox)GVPurItems.Rows[j].FindControl("Tbdbamt");
                    //if (TBItmQty.Text != "" && TbItmCst.Text != "")
                    {
                        TBNetTtl.Text = (Convert.ToDecimal(TBItmQty.Text.Trim()) * Convert.ToDecimal(TbPurRat.Text.Trim())).ToString();
                        float GTotal = 0;
                        for (int k = 0; k < GVPurItems.Rows.Count; k++)
                        {
                            TextBox total = (TextBox)GVPurItems.Rows[k].FindControl("TbAddPurNetTtl");
                            GTotal += Convert.ToSingle(total.Text);
                        }

                        //double salrat = Convert.ToDecimal(TBNetTtl.Text.Trim()) * 0.1;

                        //TbSalRat.Text = (Convert.ToDecimal(TBNetTtl.Text.Trim()) + Convert.ToDecimal(salrat)).ToString();


                        TBGrssTotal.Text = GTotal.ToString();
                        TBTtl.Text = GTotal.ToString();

                        if (DDL_Paytyp.SelectedValue == "2")
                        {
                            TB_ChqAmt.Text = TBTtl.Text;
                            TB_ChqAmt.Enabled = false;
                        }
                        else
                        {
                            TB_CshAmt.Text = TBTtl.Text;
                            TB_CshAmt.Enabled = false;
                        }

                        if (ddlPurItm.SelectedValue == "0")
                        {
                            lbl_Flag.Text = "0";
                        }
                        else
                        {
                            //lbl_Flag.Text = "1";
                        }
                        //TBNetTtl.Focus();

                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }
        }

        protected void GVPurItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
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

                    GVPurItems.DataSource = dt;
                    GVPurItems.DataBind();

                    SetPreRowitm();

                    decimal GTotal = 0;
                    for (int j = 0; j < GVPurItems.Rows.Count; j++)
                    {
                        TextBox total = (TextBox)GVPurItems.Rows[j].FindControl("TbAddPurNetTtl");
                        GTotal += Convert.ToDecimal(total.Text);

                        TBGrssTotal.Text = GTotal.ToString();
                        //total.Focus();
                    }

                   
                }
            }

        }

        

        public void getPay_Typ(string Vchtyp)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //cmd.CommandText = " select rtrim('[' + CAST(PayTyp_id AS VARCHAR(200)) + ']-' + PayTyp_nam ) as [PayTyp_nam], PayTyp_id  from tbl_PayTyp where ISActive = 'True'";
                    cmd.CommandText = " select distinct rtrim('[' + CAST(tbl_PayTyp.PayTyp_id AS VARCHAR(200)) + ']-' + PayTyp_nam ) as [PayTyp_nam], tbl_PayTyp.vchtyp_id,tbl_PayTyp.PayTyp_id  from tbl_PayTyp " +
                                       " inner join tbl_vchtyp on tbl_PayTyp.PayTyp_id= tbl_vchtyp.PayTyp_id " +
                                       "  where tbl_PayTyp.ISActive = 'True'  and tbl_vchtyp.vchtyp_id = '" + Vchtyp + "'";

                    cmd.Connection = con;
                    con.Open();

                    DataTable dtPaytyp = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(dtPaytyp);

                    DDL_Paytyp.DataSource = dtPaytyp;
                    DDL_Paytyp.DataTextField = "PayTyp_nam";
                    DDL_Paytyp.DataValueField = "PayTyp_id";
                    DDL_Paytyp.DataBind();
                    DDL_Paytyp.Items.Insert(0, new ListItem("--Select--", "0"));

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = ex.Message;
            }

        }
        protected void DDL_Vchtyp_SelectedIndexChanged(object sender, EventArgs e)
        {

           
             if (DDL_Vchtyp.SelectedValue != "")
            {
                getPay_Typ(DDL_Vchtyp.SelectedValue.Trim());
                v_vendor.Text = "";
                DDL_Vchtyp.Enabled = true;
                DDL_Paytyp.Enabled = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "isActive", "Alert();", true);
                lbl_Heading.Text = "Error!";
                lblalert.Text = "Please Select The Voucher Type First!";
            }
        }

        protected void TBOutstand_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (TBOutstand.Text != "")
                {
                    decimal ttlafoutstand = Convert.ToDecimal(TBGrssTotal.Text) - (Convert.ToDecimal(txt_outstand.Text) + Convert.ToDecimal(TBOutstand.Text));
                    TBTtl.Text = ttlafoutstand.ToString();
                    TB_CshAmt.Text = TBTtl.Text;
                }
                else
                {
                    TBOutstand.Text = "0.00";
                    TBTtl.Text = TBGrssTotal.Text;
                    TB_CshAmt.Text = TBTtl.Text;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void TBAmtPaid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (TBAmtPaid.Text != "")
                {
                    decimal ttlamtpad = (Convert.ToDecimal(txt_outstand.Text) - Convert.ToDecimal(TBAmtPaid.Text));
                    txt_outstand.Text = ttlamtpad.ToString();
                    //TB_CshAmt.Text = TBTtl.Text;
                }
                else
                {
                    TBAmtPaid.Text = "0.00";
                    TBTtl.Text = TBGrssTotal.Text;
                    TB_CshAmt.Text = TBTtl.Text;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
    }
}