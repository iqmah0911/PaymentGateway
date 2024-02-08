using Microsoft.Extensions.Configuration;
using PaymentGateway21052021.Areas.Dashboard.Models;
using PaymentGateway21052021.Areas.Reports.Models;
using PaymentGateway21052021.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PaymentGateway21052021.Helpers
{
    public partial class GeneralSqlClient
    {
        #region "private"
        private readonly static string _errorFolder;
        private static string _sqlConnection;
        static SqlConnection cn;

        #endregion


        static GeneralSqlClient()
        {
            _sqlConnection = Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>();
            cn = new SqlConnection(_sqlConnection);
        }

        public static async Task<List<InvoiceReportResponse>> RPT_InvoiceTrans(DateTime start, DateTime end, string prdtCategory)
        {
            List<InvoiceReportResponse> retObject = null; ;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_InvoiceSummary";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = prdtCategory;// "";
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = "";
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                //if (dsa.Tables.Count > 0)
                                //{
                                //    retObject = dsa.Tables[0].Rows.Count > 0 ? DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>(),//ds.Tables[0].AsEnumerable().Cast<SqlInvoice>().ToList(),

                                //        //Total = (dsa.Tables[1] != null && dsa.Tables[1].Rows.Count > 0) ? Convert.ToDouble(dsa?.Tables[1]?.Rows[0]["Total"]?.ToString()) : 0

                                //}
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }
        public static async Task<List<InvoiceReportResponse>> RPT_AggregatorCommission(DateTime start, DateTime end, string agentsids, string ReportType)
        {
            List<InvoiceReportResponse> retObject = null; ;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = agentsids;
                    com.Parameters.Add("ProductID", SqlDbType.Int).Value = 0;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = ReportType;
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                if (dsa.Tables.Count > 0)
                                {
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                }
                                return retObject;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }



        public static async Task<List<SettlementRViewModel>> RPT_Transaction(DateTime start, DateTime end, string productID, string reporttype = null, string UserID = null, string Business = null)
        {
            List<SettlementRViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = Business; //"";
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;
                    com.Parameters.Add("UserID", SqlDbType.VarChar, 50).Value = UserID;


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<SettlementRViewModel>(dsa.Tables[0]) : new List<SettlementRViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }
        
        public static async Task<List<OpeningWalletViewModel>> RPT_ATransaction(DateTime start, DateTime end, string productID, string reporttype = null, string UserID = null, string Business = null)
        {
            List<OpeningWalletViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = Business; //"";
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;
                    com.Parameters.Add("UserID", SqlDbType.VarChar, 50).Value = UserID;


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<OpeningWalletViewModel>(dsa.Tables[0]) : new List<OpeningWalletViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }


        public static async Task<List<SettlementRViewModel>> RPT_AgentsRef(DateTime start, DateTime end, string business, string productID)
        {
            List<SettlementRViewModel> retObject = null; ;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = business;//"";
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = "Report By Reference Summary";


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                //if (dsa.Tables.Count > 0)
                                //{
                                //    retObject = dsa.Tables[0].Rows.Count > 0 ? DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>(),//ds.Tables[0].AsEnumerable().Cast<SqlInvoice>().ToList(),

                                //        //Total = (dsa.Tables[1] != null && dsa.Tables[1].Rows.Count > 0) ? Convert.ToDouble(dsa?.Tables[1]?.Rows[0]["Total"]?.ToString()) : 0

                                //}
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<SettlementRViewModel>(dsa.Tables[0]) : new List<SettlementRViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }


        public static async Task<List<SettlementRViewModel>> RPT_Transacactionfee(DateTime start, DateTime end, string business, string productID)
        {
            List<SettlementRViewModel> retObject = null; ;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = business;//"";
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = "TRANSACTION FEES SUMMARY";


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                //if (dsa.Tables.Count > 0)
                                //{
                                //    retObject = dsa.Tables[0].Rows.Count > 0 ? DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>(),//ds.Tables[0].AsEnumerable().Cast<SqlInvoice>().ToList(),

                                //        //Total = (dsa.Tables[1] != null && dsa.Tables[1].Rows.Count > 0) ? Convert.ToDouble(dsa?.Tables[1]?.Rows[0]["Total"]?.ToString()) : 0

                                //}
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<SettlementRViewModel>(dsa.Tables[0]) : new List<SettlementRViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }





        public static async Task<List<AgentRViewModel>> RPT_InvoiceTransaction(DateTime start, DateTime end, string productID, string reporttype = null, string UserID = null, string walletID = null)
        {
            List<AgentRViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;
                    com.Parameters.Add("UserID", SqlDbType.VarChar, 50).Value = UserID;
                    com.Parameters.Add("RPTWalletID", SqlDbType.VarChar, 50).Value = walletID;


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                //if (dsa.Tables.Count > 0)
                                //{
                                //    retObject = dsa.Tables[0].Rows.Count > 0 ? DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>(),//ds.Tables[0].AsEnumerable().Cast<SqlInvoice>().ToList(),

                                //        //Total = (dsa.Tables[1] != null && dsa.Tables[1].Rows.Count > 0) ? Convert.ToDouble(dsa?.Tables[1]?.Rows[0]["Total"]?.ToString()) : 0

                                //}
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<AgentRViewModel>(dsa.Tables[0]) : new List<AgentRViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }

        public static async Task<List<AgentRViewModel>> RPT_ALLInvoiceTransaction(DateTime start, DateTime end, string reporttype = null)
        {
            List<AgentRViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// "";  
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;

                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value =  "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = ""; 
                    com.Parameters.Add("UserID", SqlDbType.VarChar, 50).Value = "";
                    com.Parameters.Add("RPTWalletID", SqlDbType.VarChar, 50).Value = "";


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                //if (dsa.Tables.Count > 0)
                                //{
                                //    retObject = dsa.Tables[0].Rows.Count > 0 ? DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>(),//ds.Tables[0].AsEnumerable().Cast<SqlInvoice>().ToList(),

                                //        //Total = (dsa.Tables[1] != null && dsa.Tables[1].Rows.Count > 0) ? Convert.ToDouble(dsa?.Tables[1]?.Rows[0]["Total"]?.ToString()) : 0

                                //}
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<AgentRViewModel>(dsa.Tables[0]) : new List<AgentRViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }

        public static async Task<List<AgentNasModel>> RPT_STransaction(DateTime start, DateTime end, string productID, string reporttype = null, string UserID = null, string walletID = null)
        {
            List<AgentNasModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;
                    com.Parameters.Add("UserID", SqlDbType.VarChar, 50).Value = "";
                    com.Parameters.Add("RPTWalletID", SqlDbType.VarChar, 50).Value = "";


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                //if (dsa.Tables.Count > 0)
                                //{
                                //    retObject = dsa.Tables[0].Rows.Count > 0 ? DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>(),//ds.Tables[0].AsEnumerable().Cast<SqlInvoice>().ToList(),

                                //        //Total = (dsa.Tables[1] != null && dsa.Tables[1].Rows.Count > 0) ? Convert.ToDouble(dsa?.Tables[1]?.Rows[0]["Total"]?.ToString()) : 0

                                //}
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<AgentNasModel>(dsa.Tables[0]) : new List<AgentNasModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }

        public static async Task<List<WeeklyRViewModel>> RPT_WeeklySummaryTransaction(string reporttype = null)
        {
            List<WeeklyRViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_WeeklyTransaction";
                    com.CommandTimeout = 0;  
                    com.Parameters.Add("Period", SqlDbType.VarChar, 50).Value = reporttype; 


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                //if (dsa.Tables.Count > 0)
                                //{
                                //    retObject = dsa.Tables[0].Rows.Count > 0 ? DataTableConvert.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>(),//ds.Tables[0].AsEnumerable().Cast<SqlInvoice>().ToList(),

                                //        //Total = (dsa.Tables[1] != null && dsa.Tables[1].Rows.Count > 0) ? Convert.ToDouble(dsa?.Tables[1]?.Rows[0]["Total"]?.ToString()) : 0

                                //}
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<WeeklyRViewModel>(dsa.Tables[0]) : new List<WeeklyRViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }





        public static async Task<List<AgentRViewModel>> RPT_InvoiceReversal(string walletacctnumber, string refNo, string reporttype = null)
        {
            List<AgentRViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "EGS_ReversalTransaction_Refund";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("WalletAccountNumber", SqlDbType.VarChar).Value = walletacctnumber;
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = refNo;
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {
                            using (da)
                            {
                                da.Fill(dsa);

                                if (dsa.Tables.Count > 0)
                                {
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<AgentRViewModel>(dsa.Tables[0]) : new List<AgentRViewModel>();
                                }
                                return retObject;
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }

        public static async Task<List<WalletHViewModel>> RPT_WalletTransaction(DateTime start, DateTime end, string productID, string reporttype = null, string UserID = null, string Business = null,string transType=null)
        {
            List<WalletHViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = Business;
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;
                    com.Parameters.Add("UserID", SqlDbType.VarChar, 50).Value = UserID;
                    com.Parameters.Add("RPTWalletID", SqlDbType.VarChar, 50).Value = transType;


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                if (dsa.Tables.Count > 0)
                                {
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<WalletHViewModel>(dsa.Tables[0]) : new List<WalletHViewModel>();
                                }
                                return retObject;
                                //return dsa;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }

        public static async Task<List<AuditTrailViewModel>> RPT_AuditTrail(DateTime start, DateTime end, string productID, string reporttype = null, string UserID = null, string walletID = null)
        {
            List<AuditTrailViewModel> retObject = null;
            try
            {
                {
                    SqlCommand com = new SqlCommand();
                    var con = new SqlConnection(Startup.StaticConfig.GetSection("ConnectionStrings:DefaultConnection").Get<string>());
                    con.Open();
                    com.Connection = con;
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "RPT_Transactions";
                    com.CommandTimeout = 0;
                    com.Parameters.Add("PeriodFrom", SqlDbType.Date, 50).Value = start;// "";
                    com.Parameters.Add("PeriodTo", SqlDbType.Date).Value = end;// ""; 
                    com.Parameters.Add("Business", SqlDbType.VarChar).Value = "";
                    com.Parameters.Add("ProductID", SqlDbType.VarChar).Value = productID;// "";
                    com.Parameters.Add("ProductItemCategory", SqlDbType.VarChar).Value = "";// "";
                    com.Parameters.Add("ReportType", SqlDbType.VarChar, 50).Value = reporttype;
                    com.Parameters.Add("UserID", SqlDbType.VarChar, 50).Value = UserID;
                    com.Parameters.Add("RPTWalletID", SqlDbType.VarChar, 50).Value = walletID;


                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = com;

                    DataSet dsa = new DataSet();

                    return await Task.Run(() =>
                    {
                        using (con)
                        {

                            //}
                            using (da)
                            {
                                da.Fill(dsa);
                                if (dsa.Tables.Count > 0)
                                {
                                    // retObject = dsa.Tables[0].Rows.Count > 0 ? General.ConvertDataTable<InvoiceReportResponse>(dsa.Tables[0]) : new List<InvoiceReportResponse>();
                                    retObject = dsa.Tables[0].Rows.Count > 0 ? Helpers.DataTableConvert.ConvertDataTable<AuditTrailViewModel>(dsa.Tables[0]) : new List<AuditTrailViewModel>();
                                }
                                return retObject;
                            }
                        }
                    });


                }
            }
            catch (Exception ex)
            {
                //General.LogToFile(ex.Message + " : " + ex.StackTrace);
                return null;
            }
        }

    }
}

