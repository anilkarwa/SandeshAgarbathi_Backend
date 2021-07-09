using SandeshAgarbathiAPI.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SandeshAgarbathiAPI.Models
{
    public class CustomerModal
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;

        public List<Customer> CustomerList()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            String query = "select CUSTID, CUSTCODE, CUSTNAME, BAdd1, BAdd2, BAdd3, City, State, Country, PinCode," +
                " GSTNO, EMAILID,Remarks, PHNO, MOBILE, CONTACT, CUSTGROUPID, CURID, Remarks, addedBy, addedOn, changedBy, changedOn, InActive from MstCUST where CUSTID > 0";
            SqlCommand cmd = new SqlCommand(query, connection);

            List<Customer> custList = new List<Customer>();

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customer custData = new Customer();
                    custData.id = reader.GetInt32(0);
                    custData.code = reader.GetString(1);
                    custData.name = reader.GetString(2);
                    custData.addressLine1 = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    custData.addressLine2 = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    custData.addressLine3 = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    custData.city = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    custData.pincode = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    custData.state = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    custData.country = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    custData.gstNo = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    custData.email = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    custData.phoneNumber = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    custData.mobileNumber = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    custData.contactPerson = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    custData.groupId = reader.GetInt32(16);
                    custData.currency = reader.GetInt32(17);
                    custData.remarks = reader.IsDBNull(18) ? "" : reader.GetString(18);
                    custData.addedBy = reader.IsDBNull(19) ? "" : reader.GetString(19);
                    custData.addedOn = reader.IsDBNull(20) ? "" : reader.GetDateTime(20).ToString("yyyy-MM-dd");
                    custData.changedBy = reader.IsDBNull(21) ? "" : reader.GetString(21);
                    custData.changedOn = reader.IsDBNull(22) ? "" : reader.GetDateTime(22).ToString("yyyy-MM-dd");
                    custData.inActive = reader.IsDBNull(23) ? "N" : reader.GetString(23);
                    custList.Add(custData);
                }

            }
            catch (Exception Ex)
            {
                custList = new List<Customer>();
            }
            finally
            {
                connection.Close();
            }

            return custList;
        }

        public Boolean SaveCustomers(List<Customer> customers)
        {
            Boolean isValid = false;
            var lastCustomerId = 0;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                // get last customer id
                
                String query = "SELECT TOP 1 CUSTID FROM MstCUST order by CUSTID DESC; ";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lastCustomerId = reader.GetInt32(0);
                }
                isValid = true;
            }
            catch (Exception Ex)
            {
                isValid = false;
            }
            finally {
                conn.Close();
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
                   
                    for (int i = 0; i < customers.Count; i++)
                    {
                        var cust = customers[i];
                        lastCustomerId = lastCustomerId + 1;
                        command.CommandText =
                        "insert into MstCUST(CUSTID,CUSTCODE, CUSTNAME, CUSTAliasName, InActive,"+ 
                        "BAdd1, BAdd2, BAdd3, BAdd4, City, PinCode, State, Country, AddedBy, AddedOn, GSTNO, EMAILID, Remarks,"+
                        " CUSTGROUPID, TELNO, MOBILE, CONTACT, CURID) "+
                        "values('"+ lastCustomerId + "','"+cust.code+ "','"+cust.name+ "','"+cust.name+ "','N', '"+cust.addressLine1+"',"+
                        "'"+cust.addressLine2+ "','"+cust.addressLine3+ "','','"+cust.city+ "','"+cust.pincode+"',"+
                        " '"+cust.state+ "','INDIA','"+cust.addedBy+ "','"+cust.addedOn+ "','"+cust.gstNo+ "','"+cust.email+"',"+
                        " '"+cust.remarks+ "','"+cust.groupId+ "','"+cust.phoneNumber+ "','"+cust.mobileNumber+ "','"+cust.contactPerson+"', 0)";
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
            return isValid;
        }

        public Boolean UpdateCustomer(List<Customer> customers)
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
                    for (int i = 0; i < customers.Count; i++)
                    {
                        var cust = customers[i];
                        command.CommandText = "update MstCUST set CUSTNAME='" + cust.name + "', CUSTAliasName='" + cust.name + "'," +
                            "BAdd1='" + cust.addressLine1 + "', BAdd2='" + cust.addressLine2 + "', BAdd3='" + cust.addressLine3 + "'," +
                            " City='" + cust.city + "', PinCode='" + cust.pincode + "', State='" + cust.state + "'," +
                            "ChangedBy='" + cust.changedBy + "', ChangedOn='" + cust.changedOn + "', GSTNO='" + cust.gstNo + "'," +
                            "EMAILID='" + cust.email + "', Remarks='" + cust.remarks + "',CONTACT='" + cust.contactPerson + "'," +
                            " CUSTGROUPID='" + cust.groupId + "', TELNO='" + cust.phoneNumber + "', MOBILE='" + cust.mobileNumber + "' " +
                            " where CUSTID='" + cust.id + "'";

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
            return isValid;
        }

        public Boolean DeleteCustomer(List<Customer> custIds)
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
                    for (int i = 0; i < custIds.Count; i++)
                    {

                        command.CommandText =
                         "Update MstCUST set InActive='Y' where CUSTID='" + custIds[i].id + "'";
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
            return isValid;
        }

        public List<CustomerGroup> getCustomerGroups()
        {

            SqlConnection connection = new SqlConnection(connectionString);
            String query = "select CUSTGROUPID, CUSTGROUPNAME from MstCUSTGROUP where InActive='N'";
            SqlCommand cmd = new SqlCommand(query, connection);

            List<CustomerGroup> custGroupList = new List<CustomerGroup>();

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    CustomerGroup groupData = new CustomerGroup();
                    groupData.id = reader.GetInt32(0);
                    groupData.name = reader.GetString(1);

                    custGroupList.Add(groupData);
                }

            }
            catch (Exception Ex)
            {
                custGroupList = new List<CustomerGroup>();
            }
            finally
            {
                connection.Close();
            }

            return custGroupList;
        }
    }
}