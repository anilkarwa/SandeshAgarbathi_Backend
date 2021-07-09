using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using SandeshAgarbathiAPI.BusinessEntities;

namespace SandeshAgarbathiAPI.Models
{
    public class LoginModal
    {
        private TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
        private MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

        string connectionString = ConfigurationManager.ConnectionStrings["DBConStr"].ConnectionString;

        public User checkLogin(String email, String password)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            String encryptedPwd = EncryptPassword(password);
           // String decryptPwd = DecryptPassword("g8c0Ochq2qA=");
            String query = "select * from MstUser where EmailID='"+email+"' and LoginPwd='"+ encryptedPwd + "' and Inactive='N'";
            SqlCommand cmd = new SqlCommand(query, connection);

            var userData = new User();

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userData.id = reader.GetInt32(0);
                    userData.code = reader.GetString(1);
                    userData.name = reader.GetString(3);
                    userData.email = reader.GetString(4);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return userData;

        }

        private String EncryptPassword(String password)
        {
            String strPassKey = "svt$$12@#";
            DES.Key = MD5Hash(strPassKey);
            DES.Mode = CipherMode.ECB;
            byte[] Buffer = ASCIIEncoding.ASCII.GetBytes(password);
            return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        private String DecryptPassword(String password)
        {
            String strPassKey = "svt$$12@#";
            DES.Key = MD5Hash(strPassKey);
            DES.Mode = CipherMode.ECB;
            byte[] Buffer = Convert.FromBase64String(password);
            return ASCIIEncoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
        }

        private byte[] MD5Hash(String value)
        {
            return MD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value));
        }
    }

}