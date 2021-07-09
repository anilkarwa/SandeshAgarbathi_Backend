using SandeshAgarbathiAPI.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SandeshAgarbathiAPI.Models
{
    public class ItemModal
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;

        public List<Item> GetAllItems()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            String query = "select ITEMID, ITEMCODE, ITEMNAME, ITEMRATE from MstITEM where InActive='N' and ITEMID > 0";
            SqlCommand cmd = new SqlCommand(query, connection);

            List<Item> itemList = new List<Item>();

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Item itemData = new Item();
                    itemData.id = reader.GetInt32(0);
                    itemData.code = reader.GetString(1);
                    itemData.name = reader.GetString(2);
                    itemData.rate = reader.GetDecimal(3);

                    itemList.Add(itemData);
                    
                }

            }
            catch (Exception Ex)
            {
                itemList = new List<Item>();
            }
            finally
            {
                connection.Close();
            }

            return itemList;
        }
    }
}