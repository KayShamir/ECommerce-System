using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        string conn_str = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kaysh\source\repos\ECommerce\ECommerce\App_Data\Database1.mdf;Integrated Security=True";
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Product()
        {
            return View();
        }
        [HttpPost]
        public ActionResult postProduct(HttpPostedFileBase prod_file)
        {
            string prod_name = Request["prod_name"];
            string prod_desc = Request["prod_desc"];
            double prod_price = Convert.ToDouble(Request["prod_price"]);
            string prod_gender = Request["prod_gender"];
            string prod_color = Request["prod_color"];
            string prod_size = Request["prod_size"];
            string prod_material = Request["prod_material"];
            string prod_category = Request["prod_category"];
            int prod_stock = Convert.ToInt32(Request["prod_stock"]);

            string image = Path.GetFileName(prod_file.FileName);
            string file_path = "C:\\Uploads";
            string filepath = Path.Combine(file_path, image);
            prod_file.SaveAs(filepath);

            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO PRODUCT (PROD_NAME, PROD_DESC, PROD_PRICE, PROD_GENDER, PROD_COLOR, PROD_SIZE, PROD_MATERIAL, PROD_CATEGORY, PROD_STOCK, [PROD_FILE]) " +
                        "VALUES (@prod_name, @prod_desc, @prod_price, @prod_gender, @prod_color, @prod_size, @prod_material, @prod_category, @prod_stock, @prod_file)";
                    cmd.Parameters.AddWithValue("@PROD_NAME", prod_name);                    
                    cmd.Parameters.AddWithValue("@PROD_DESC", prod_desc);                    
                    cmd.Parameters.AddWithValue("@PROD_PRICE", prod_price);                    
                    cmd.Parameters.AddWithValue("@PROD_GENDER", prod_gender);                    
                    cmd.Parameters.AddWithValue("@PROD_COLOR", prod_color);                    
                    cmd.Parameters.AddWithValue("@PROD_SIZE", prod_size);                    
                    cmd.Parameters.AddWithValue("@PROD_MATERIAL", prod_material);                    
                    cmd.Parameters.AddWithValue("@PROD_CATEGORY", prod_category);                    
                    cmd.Parameters.AddWithValue("@PROD_STOCK", prod_stock);                    
                    cmd.Parameters.AddWithValue("@PROD_FILE", image);

                    cmd.ExecuteNonQuery();
                }
            }

            var data = new List<object>();

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Customer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult postCustomer(HttpPostedFileBase prod_file)
        {
            var data = new List<object>();
            string cus_firstname = Request["cus_firstname"];
            string cus_lastname = Request["cus_lastname"];
            int cus_birthdate = Convert.ToInt32(Request["cus_birthdate"]);
            string cus_address = Request["cus_address"];
            int cus_phonenumber = Convert.ToInt32(Request["cus_number"]);
            string cus_email = Request["cus_email"];
            string cus_pass = Request["cus_pass"];

            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO CUSTOMER (CUS_FIRSTNAME, CUS_LASTNAME, CUS_BIRTHDATE, CUS_ADDRESS, CUS_PHONENUMBER, CUS_EMAIL, CUS_PASS) " +
                        "VALUES (@cus_firstname, @cus_lastname, @cus_birthdate, @cus_address, @cus_phonenumber, @cus_email,@cus_pass )";
                    cmd.Parameters.AddWithValue("@cus_firstname", cus_firstname);
                    cmd.Parameters.AddWithValue("@cus_lastname", cus_lastname);
                    cmd.Parameters.AddWithValue("@cus_birthdate", cus_birthdate);
                    cmd.Parameters.AddWithValue("@cus_address", cus_address);
                    cmd.Parameters.AddWithValue("@cus_phonenumber", cus_phonenumber);
                    cmd.Parameters.AddWithValue("@cus_email", cus_email);
                    cmd.Parameters.AddWithValue("@cus_pass", cus_pass);
                    cmd.ExecuteNonQuery();
                }
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dashboard_Admin()
        {
            return View();
        }
        [HttpGet]
        public FileResult Image(string filename)
        {
            var folder = "C:\\Uploads";
            var filepath = Path.Combine(folder, filename);

            if (!System.IO.File.Exists(filepath))
            {
                throw new FileNotFoundException("File not found", filename);
            }

            var mime = System.Web.MimeMapping.GetMimeMapping(Path.GetFileName(filepath));
            Response.Headers.Add("content-disposition", "inline");
            return new FilePathResult(filepath, mime);
        }
        public ActionResult DeleteProduct()
        {
            var data = new List<object>();
            var prod_id = Request["prod_id"];

            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "DELETE FROM PRODUCT WHERE PROD_ID = @PROD_ID";
                    cmd.Parameters.AddWithValue("@PROD_ID", prod_id);

                    var ctr = cmd.ExecuteNonQuery();

                    if (ctr >= 1)
                    {
                        data.Add(new
                        {
                            mess = 1
                        });
                    }

                }
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetProduct(int prod_id)
        {
            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT * FROM PRODUCT WHERE PROD_ID = @prod_id";
                    cmd.Parameters.AddWithValue("@prod_id", prod_id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var prod_data = new
                            {
                                prod_name = reader["PROD_NAME"].ToString(),
                                prod_desc = reader["PROD_DESC"].ToString(),
                                prod_price = reader["PROD_PRICE"].ToString(),
                                prod_gender = reader["PROD_GENDER"].ToString(),
                                prod_color = reader["PROD_COLOR"].ToString(),
                                prod_size = reader["PROD_SIZE"].ToString(),
                                prod_material = reader["PROD_MATERIAL"].ToString(),
                                prod_category = reader["PROD_CATEGORY"].ToString(),
                                prod_stock = reader["PROD_STOCK"].ToString(),
                                prod_file = reader["PROD_FILE"].ToString(),
                            };
                            return Json(prod_data, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(null, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
        }
        [HttpPost]
        public ActionResult PostProductUpdate()
        {
            var prod_id = Request.Form["prod_id"];
            var prod_name = Request.Form["prod_name"];
            var prod_desc = Request.Form["prod_desc"];
            var prod_price = Request.Form["prod_price"];
            var prod_gender = Request.Form["prod_gender"];
            var prod_color = Request.Form["prod_color"];
            var prod_size = Request.Form["prod_size"];
            var prod_material = Request.Form["prod_material"];
            var prod_category = Request.Form["prod_category"];
            var prod_stock = Request.Form["prod_stock"];
            var insert_file = Request.Files["insert_file"]; // Handle the file if needed

            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE PRODUCT SET PROD_NAME = @prod_name, PROD_DESC = @prod_desc, PROD_PRICE = @prod_price, PROD_GENDER = @prod_gender, PROD_COLOR = @prod_color, PROD_SIZE = @prod_size, PROD_MATERIAL = @prod_material, PROD_CATEGORY = @prod_category, PROD_STOCK = @prod_stock WHERE PROD_ID = @prod_id";
                    cmd.Parameters.AddWithValue("@prod_id", prod_id);
                    cmd.Parameters.AddWithValue("@prod_name", prod_name);
                    cmd.Parameters.AddWithValue("@prod_desc", prod_desc);
                    cmd.Parameters.AddWithValue("@prod_price", prod_price);
                    cmd.Parameters.AddWithValue("@prod_gender", prod_gender);
                    cmd.Parameters.AddWithValue("@prod_color", prod_color);
                    cmd.Parameters.AddWithValue("@prod_size", prod_size);
                    cmd.Parameters.AddWithValue("@prod_material", prod_material);
                    cmd.Parameters.AddWithValue("@prod_category", prod_category);
                    cmd.Parameters.AddWithValue("@prod_stock", prod_stock);

                    // Handle file upload if applicable
                    if (insert_file != null && insert_file.ContentLength > 0)
                    {
                        var filePath = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(insert_file.FileName));
                        insert_file.SaveAs(filePath);
                        // Update the file path in the database if needed
                        cmd.CommandText += ", PROD_FILE = @prod_file";
                        cmd.Parameters.AddWithValue("@prod_file", filePath);
                    }

                    var ctr = cmd.ExecuteNonQuery();
                    var result = new { success = ctr > 0 };

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult User_Dashboard()
        {
            return View();
        }

    }
}