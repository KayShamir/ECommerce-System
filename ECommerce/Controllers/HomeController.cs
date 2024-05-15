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
            string prod_category = Request["prod_category"];
            string prod_color = Request["prod_color"];
            string prod_size = Request["prod_size"];
            string prod_material = Request["prod_material"];
            string prod_origin = Request["prod_origin"];
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
                    cmd.CommandText = "INSERT INTO PRODUCT (PROD_NAME, PROD_DESC, PROD_PRICE, PROD_CATEGORY, PROD_COLOR, PROD_SIZE, PROD_MATERIAL, PROD_ORIGIN, PROD_STOCK, [PROD_FILE]) " +
                        "VALUES (@prod_name, @prod_desc, @prod_price, @prod_category, @prod_color, @prod_size, @prod_material, @prod_origin, @prod_stock, @prod_file)";
                    cmd.Parameters.AddWithValue("@PROD_NAME", prod_name);                    
                    cmd.Parameters.AddWithValue("@PROD_DESC", prod_desc);                    
                    cmd.Parameters.AddWithValue("@PROD_PRICE", prod_price);                    
                    cmd.Parameters.AddWithValue("@PROD_CATEGORY", prod_category);                    
                    cmd.Parameters.AddWithValue("@PROD_COLOR", prod_color);                    
                    cmd.Parameters.AddWithValue("@PROD_SIZE", prod_size);                    
                    cmd.Parameters.AddWithValue("@PROD_MATERIAL", prod_material);                    
                    cmd.Parameters.AddWithValue("@PROD_ORIGIN", prod_origin);                    
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
    }
}