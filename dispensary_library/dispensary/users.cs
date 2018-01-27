using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Dispensary
{
    /*
     * 
     *  Created By -- Vam$hedhar Reddy C
     *  All Rights Reserved
     *  Product of IMG Labs IIT Roorkee, Saharanpur Campus
     *  
     * */
    /// <summary>
    /// 
    /// This Class consists all user related functions.
    /// 
    /// </summary>
    public class users
    {
        public string error;
        public string post;
        public string fname;
        public string lname;

        // Generate MD5 Hash of a string.
        public static string GetMD5Hash(string input)
        {
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            return s.ToString();

        }

        // To Get First Name and Last Name from username.
        public users(string username)
        {
            username = GetMD5Hash(username);
            dbconnect dbco = new dbconnect();
            string qu = "SELECT * FROM users WHERE userhash='" + username + "'";
            dbco.command_reader(qu, dbco.con);

            if (dbco.dr.Read())
            {
                fname = dbco.dr[2].ToString();
                lname = dbco.dr[3].ToString();
            }
            else
            {
                error = "error";
            }
            dbco.dbclose();
        }

        // Check username and password match.
        public users(string username,string password)
        {
            username = GetMD5Hash(username);
            password = GetMD5Hash(password);
            dbconnect dbco = new dbconnect();
            string qu = "SELECT * FROM users WHERE userhash='" + username + "'";
            dbco.command_reader(qu, dbco.con);

            if (dbco.dr.Read())
            {
                if (password == dbco.dr[5].ToString())
                {
                    post = dbco.dr[4].ToString();
                    fname = dbco.dr[2].ToString();
                    error = "sucess";
                }
                else
                {
                    error = "error";
                }
            }
            else
            {
                error = "error";
            }
            
            dbco.dbclose();   
        }
        
        // Change user Password
        public void changePassword(string username, string password)
        {
            username = GetMD5Hash(username);
            string pass = password;
            password = GetMD5Hash(password);
            dbconnect dbco = new dbconnect();
            string qu = "UPDATE users SET password='"+ pass +"', passhash='"+password+"' WHERE userhash='" + username + "'";
            dbco.command_nonquery(qu, dbco.con);
            error = dbco.reader;
            dbco.dbclose();
        }
       
    }
}
