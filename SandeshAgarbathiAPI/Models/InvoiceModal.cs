﻿using SandeshAgarbathiAPI.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace SandeshAgarbathiAPI.Models
{
    public class InvoiceModal
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;

        public List<Invoice> InvoiceList(String prefix)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            String query = "select top 100 VINVID, VINVNO, Prefix, VINVDate," +
                           "CUSTID, PartyName, PartyAdd1, PartyAdd2, PartyAdd3, PartyCity, PartyPinCode, PartyState," +
                           "PartyCountry, Remarks, GROSSAMT, TOTAL, RNDOFFAMT," +
                           "GRANDTOT,TOTCGSTAMT, TOTSGSTAMT, TOTDISCAMT, AGENT, TIME from TrnHdrVINV where VINVID > 0 and Prefix = '"+ prefix + "' order by VINVDate desc";
            SqlCommand cmd = new SqlCommand(query, connection);

            List<Invoice> invoiceList = new List<Invoice>();

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Invoice invoiceData = new Invoice();
                    invoiceData.invoiceId = reader.GetInt32(0);
                    invoiceData.invoiceNo = reader.GetString(1);
                    invoiceData.prefix = reader.GetString(2);
                    invoiceData.invoiceDate = reader.GetDateTime(3).ToString("yyyy-MM-dd");
                    invoiceData.custId = reader.GetInt32(4);
                    invoiceData.partyName = reader.GetString(5);
                    invoiceData.addressLine1 = reader.GetString(6);
                    invoiceData.addressLine2 = reader.GetString(7);
                    invoiceData.addressLine3 = reader.GetString(8);
                    invoiceData.city = reader.GetString(9);
                    invoiceData.pincode = reader.GetString(10);
                    invoiceData.state = reader.GetString(11);
                    invoiceData.country = reader.GetString(12);
                    invoiceData.remarks = reader.GetString(13);
                    invoiceData.grossAmt = reader.GetDecimal(14);
                    invoiceData.totalAmt = reader.GetDecimal(15);
                    invoiceData.roundOffAmt = reader.GetDecimal(16);
                    invoiceData.grandTotalAmt = reader.GetDecimal(17);
                    invoiceData.totalCGSTAmt = reader.GetDecimal(18);
                    invoiceData.totalSGSTAmt = reader.GetDecimal(19);
                    invoiceData.discAmt = reader["TOTDISCAMT"] == DBNull.Value ? 0 : reader.GetDecimal(19);
                    invoiceData.agent = reader.GetString(21);
                    invoiceData.time = reader.GetString(22);

                    invoiceList.Add(invoiceData);
                }

            }
            catch (Exception Ex)
            {
                invoiceList = new List<Invoice>();
            }
            finally
            {
                connection.Close();
            }

        
            for (int i = 0; i < invoiceList.Count; i++)
            {
                var invoiceData = invoiceList[i];
                List<InvoiceItem> itemList = new List<InvoiceItem>();

                String query2 = "select VINVID, VINVDtlID, d.ITEMID, VINVQty, VINVAmt, VINVRate," +
                  "DISCPER, DISCAMT, NETAMT, CGSTPER, CGSTAMT, SGSTPER, SGSTAMT, ITMTOTAMT, i.ITEMNAME from TrnDtlVINV as d"+
                 " INNER JOIN MstITEM as i on d.ITEMID = i.ITEMID  where VINVDtlID > 0 and VINVID = '" + invoiceData.invoiceId + "' ";
                SqlCommand cmd2 = new SqlCommand(query2, connection);
                try
                {

                    connection.Open();
                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    while (reader2.Read())
                    {
                        var itemData = new InvoiceItem();
                        itemData.invoiceId = reader2.GetInt32(0);
                        itemData.invoiceItemId = reader2.GetInt32(1);
                        itemData.itemId = reader2.GetInt32(2);
                        itemData.quantity = reader2.GetDecimal(3);
                        itemData.grossAmt = reader2.GetDecimal(4);
                        itemData.rate = reader2.GetDecimal(5);
                        itemData.discPer = reader2.GetDecimal(6);
                        itemData.discAmt = reader2.GetDecimal(7);
                        itemData.netAmt = reader2.GetDecimal(8);
                        itemData.cgstPer = reader2.GetDecimal(9);
                        itemData.cgstAmt = reader2.GetDecimal(10);
                        itemData.sgstPer = reader2.GetDecimal(11);
                        itemData.sgstAmt = reader2.GetDecimal(12);
                        itemData.totalAmt = reader2.GetDecimal(13);
                        itemData.itemName = reader2.GetString(14);

                        itemList.Add(itemData);

                    }
                    invoiceData.items = itemList;
                }
                catch (Exception Ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            }

            return invoiceList;
        }

        public Boolean SaveInvoices(List<Invoice> invoiceList)
        {
            Boolean isValid = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    for (int i = 0; i < invoiceList.Count; i++)
                    {
                        var invoice = invoiceList[i];
                        DateTime dt = DateTime.Parse(invoice.invoiceDate.ToString());
                        String time = dt.ToString("HH:mm");
                        String billNo = invoice.invoiceNo.Remove(0, 4);


                        command.CommandText =
                           "insert into TrnHdrVINV(VINVNO, Prefix, PrintVINVNO, VINVDate," +
                           "CUSTID, PartyName, PartyAdd1, PartyAdd2, PartyAdd3,PartyAdd4, PartyCity, PartyPinCode, PartyState," +
                           "PartyCountry, AddedBy, AddedOn, Cancelled, Remarks," +
                           " CASHCOLLECTED, BALANCE, TIME, BILLNO, DEVID, AMTINWRDS, GROSSAMT, TOTAL, RNDOFFAMT," +
                           "GRANDTOT,TOTCGSTAMT, TOTSGSTAMT, TOTDISCAMT, AGENT) " +
                           "values('" + invoice.invoiceNo + "', '" + invoice.prefix + "', '" + invoice.invoiceNo + "'," +
                           "'" + dt + "', '" + invoice.custId + "', '" + invoice.partyName + "'," +
                           "'" + invoice.addressLine1 + "', '" + invoice.addressLine2 + "','" + invoice.addressLine3 + "', ''," +
                           "'" + invoice.city + "','" + invoice.pincode + "','" + invoice.state + "','" + invoice.country + "'," +
                           "'" + invoice.addedBy + "','" + invoice.addedOn + "','N','" + invoice.remarks + "'," +
                           "'" + invoice.totalAmt + "',0.00, '" + time + "','" + billNo + "'," +
                           "1,'', '" + invoice.grossAmt + "','" + invoice.totalAmt + "','" + invoice.roundOffAmt + "'," +
                           "'" + invoice.grandTotalAmt + "', '" + invoice.totalCGSTAmt + "','" + invoice.totalSGSTAmt + "'," +
                           "'" + invoice.discAmt + "','" + invoice.agent + "')";
    
                         command.ExecuteNonQuery();
                    }

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    isValid = true;

                }
                catch (Exception ex)
                {

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                        isValid = false;
                    }
                    catch (Exception ex2)
                    {
                        isValid = false;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }

            if (!isValid)
            {
                return isValid;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");

                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    for (int i = 0; i < invoiceList.Count; i++)
                    {
                        var invoice = invoiceList[i];
                        var invoiceNo = 0;

                        command.CommandText = "select VINVID from TrnHdrVINV where VINVNO= '" + invoice.invoiceNo+ "'";

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            invoiceNo = reader.GetInt32(0);
                        }
                        reader.Close();
                        for (int j = 0; j < invoice.items.Count; j++)
                        {
                            var item = invoice.items[j];

                            command.CommandText =
                              "insert into TrnDtlVINV(VINVID, SlNo, ITEMID, ItemSlNo, VINVQty, VINVAmt, VINVRate," +
                              "DISCPER, DISCAMT, NETAMT, CGSTPER, CGSTAMT, SGSTPER, SGSTAMT, ITMTOTAMT) " +
                              "values('" + invoiceNo + "', '" + (j + 1) + "', '" + item.itemId + "', '" + (j + 1) + "'," +
                              "'" + item.quantity + "', '" + item.grossAmt + "','" + item.rate + "'," +
                              "'" + item.discPer + "', '" + item.discAmt + "', '" + item.netAmt + "'," +
                              "'" + item.cgstPer + "','" + item.cgstAmt + "', '" + item.sgstPer + "', '" + item.sgstAmt + "'," +
                              "'" + item.totalAmt + "')";

                            command.ExecuteNonQuery();
                        }

                    }

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    isValid = true;

                }
                catch (Exception ex)
                {

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                        isValid = false;
                    }
                    catch (Exception ex2)
                    {
                        isValid = false;
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return isValid;
        }
    }
}