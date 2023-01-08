using grocery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace grocery.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GroceryController : Controller
    {
        private readonly string connecstr = "Data Source=Server_name;Initial Catalog=hospital;Trusted_Connection=True;MultipleActiveResultSets=True";
        [HttpGet]
        [Route("testc")]
        public string Test()
        {
            return "anand";
        }
        [HttpPost]
        [Route("api/AdminSignUp")]
        public string SignUp(AdminSignUp adminSignUp)
        {
            try
            {
                SqlConnection connection = new(connecstr);
                connection.Open();
                string insertquery = "INSERT INTO AdminLogin(Name,Email,Password,Address,Mobile,created_by,Created_date,IsActive)" +
                                    "VALUES('" + adminSignUp.Name + "', '" + adminSignUp.Email + "', '" + adminSignUp.Password + "', '" + adminSignUp.Address + "', '" + adminSignUp.Mobile + "', '" + adminSignUp.Created_by + "', '" + DateTime.Now.ToString() + "', 1"+")";
                SqlCommand addCommand = new(insertquery, connection);
                addCommand.ExecuteReader();
                return "Admin details added sucessfully";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        [HttpPost]
        [Route("api/AdminLogin")]
        public ApiResponse SignIn(AdminLogin signIn)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                SqlConnection sqlconn = new SqlConnection(connecstr);
                string sqlquery = "select Mobile, Password from AdminLogin";
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                SqlDataReader rdr = sqlcomm.ExecuteReader();
                while (rdr.Read())
                {
                    string mobile = rdr[0].ToString();
                    string pasword = rdr[1].ToString();
                    if (mobile.Equals(signIn.Mobile) && pasword.Equals(signIn.Password))
                    {
                        response.Message = "login Success";
                        return response;
                    }
                }
                response.Message = "please check credentials";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }
        [HttpPost]
        [Route("api/AddItem")]
        public ApiResponse Additem(GroceryItem groceryItem)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                SqlConnection sqlconn = new SqlConnection(connecstr);
                string sqlquery = "INSERT INTO GroceryData (GroceryName, Price, Type, img) SELECT '" + groceryItem.GroceryName+"',"+groceryItem.Price+", '"+groceryItem.Type+"', BulkColumn FROM Openrowset(Bulk '"+groceryItem.Image+"', Single_Blob) as Logo";
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                sqlcomm.ExecuteReader();
                response.Message = "Item added";
                return response;
            }
            catch(Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpPost]
        [Route("api/DeleteItem")]
        public ApiResponse Additem(int itemId)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                SqlConnection sqlconn = new SqlConnection(connecstr);
                string selectQuery = "select GroceryId from GroceryData where GroceryId = "+itemId;
                sqlconn.Open();
                SqlCommand sqlcomm = new SqlCommand(selectQuery, sqlconn);
                SqlDataReader rdr = sqlcomm.ExecuteReader();
                if (!rdr.Read())
                {
                    response.Message = "The item does not exist.";
                    return response;
                }
                //sqlconn.Close();
                string deleteQuery = "DELETE FROM GroceryData WHERE GroceryId = "+itemId;
                //sqlconn.Open();
                sqlcomm = new SqlCommand(deleteQuery, sqlconn);
                sqlcomm.ExecuteReader();
                response.Message = "Item deleted";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpGet]
        [Route("api/getCategory")]
        public Response GetGrocery()
        {
            Response response = new Response();
            try
            {
                SqlConnection sqlconn = new SqlConnection(connecstr);
                string selectQuery = "select * from GroceryData;";
                sqlconn.Open();
                SqlCommand sqlCommand = new(selectQuery, sqlconn);
                SqlDataReader rdr = sqlCommand.ExecuteReader();
                List<GroceryItem> groceryItemList = new List<GroceryItem>();
                while (rdr.Read())
                {
                    GroceryItem groceryItem = new GroceryItem();
                    groceryItem.Id = Convert.ToInt32(rdr.GetValue(0));
                    groceryItem.GroceryName = rdr[1].ToString();
                    groceryItem.Price = Convert.ToDecimal(rdr.GetValue(2));
                    groceryItem.Type = rdr[3].ToString();
                    byte[] img = (byte[])rdr.GetValue(4);
                    groceryItem.Image = "data:image/png;base64, " + Convert.ToBase64String(img);
                    groceryItemList.Add(groceryItem);
                }
                response.GroceryItems = groceryItemList;
                response.Message = "Data returned for the requested sub cateroy";
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }

        public void CustomerSignUp()
        {
            //Need to implement
        }

        public void CustomerLogin()
        {
            //Need to implement
        }

        public void DisablingCustomer()
        {
            //Need to implement
        }
    }
}
