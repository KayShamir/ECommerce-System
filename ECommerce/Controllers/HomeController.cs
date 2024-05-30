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
        public ActionResult postCustomer(HttpPostedFileBase cus_file)
        {
            var data = new List<object>();
            string cus_firstname = Request["cus_firstname"];
            string cus_lastname = Request["cus_lastname"];
            string cus_birthdate = Request["cus_birthdate"];
            string cus_address = Request["cus_address"];
            string cus_phonenumber = Request["cus_number"];
            string cus_email = Request["cus_email"];
            string cus_pass = Request["cus_pass"];

            string image = Path.GetFileName(cus_file.FileName);
            string file_path = "C:\\Uploads";
            string filepath = Path.Combine(file_path, image);
            cus_file.SaveAs(filepath);


            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var cmd = db.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO CUSTOMER (CUS_FIRSTNAME, CUS_LASTNAME, CUS_BIRTHDATE, CUS_ADDRESS, CUS_PHONENUMBER, CUS_EMAIL, CUS_PASS, [CUS_FILE]) " +
                        "VALUES (@CUS_FIRSTNAME, @cus_lastname, @cus_birthdate, @cus_address, @cus_phonenumber, @cus_email,@cus_pass, @cus_file)";
                    cmd.Parameters.AddWithValue("@cus_firstname", cus_firstname);
                    cmd.Parameters.AddWithValue("@cus_lastname", cus_lastname);
                    cmd.Parameters.AddWithValue("@cus_birthdate", cus_birthdate);
                    cmd.Parameters.AddWithValue("@cus_address", cus_address);
                    cmd.Parameters.AddWithValue("@cus_phonenumber", cus_phonenumber);
                    cmd.Parameters.AddWithValue("@cus_email", cus_email);
                    cmd.Parameters.AddWithValue("@cus_pass", cus_pass);
                    cmd.Parameters.AddWithValue("@cus_file", image);
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
            var insert_file = Request.Files["insert_file"];

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

                    if (insert_file != null && insert_file.ContentLength > 0)
                    {
                        var filePath = Path.Combine(Server.MapPath("~/Uploads"), Path.GetFileName(insert_file.FileName));
                        insert_file.SaveAs(filePath);
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
        public ActionResult Login_Page()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login()
        {
            var response = new { success = false, admin = false, message = "Invalid email or password." };
            string email = Request["cus_email"];
            string pass = Request["cus_pass"];

            if (email == "admin@admin.com" && pass == "admin")
            {
                response = new { success = true, admin = true, message = "Login successful." };
            }
            else
            {
                using (var db = new SqlConnection(conn_str))
                {
                    db.Open();
                    using (var cmd = db.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT * FROM [Customer] WHERE CUS_EMAIL = @email";
                        cmd.Parameters.AddWithValue("@email", email);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string userId = reader["cus_id"].ToString();
                                string check_pass = reader["CUS_PASS"].ToString();
                                if (check_pass == pass)
                                {
                                    Session["UserId"] = userId;
                                    response = new { success = true, admin = false, message = "Login successful." };
                                }
                            }
                        }
                    }
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            var response = new { success = false, message = "Error adding to cart" };
            var userId = Session["UserId"];

            if (userId == null)
            {
                response = new { success = false, message = "User not logged in" };
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (quantity <= 0)
            {
                response = new { success = false, message = "Quantity must be greater than 0" };
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            string conn_str = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\kaysh\source\repos\ECommerce\ECommerce\App_Data\Database1.mdf;Integrated Security=True";

            using (var db = new SqlConnection(conn_str))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        using (var cmd = db.CreateCommand())
                        {
                            cmd.Transaction = transaction;
                            cmd.CommandType = CommandType.Text;

                            // Check if the product already exists in the cart
                            cmd.CommandText = "SELECT cart_qty FROM Cart WHERE cus_id = @cus_id AND prod_id = @prod_id";
                            cmd.Parameters.AddWithValue("@cus_id", userId);
                            cmd.Parameters.AddWithValue("@prod_id", productId);

                            var existingCartCount = cmd.ExecuteScalar();

                            int newCartCount;
                            decimal prodPrice;

                            // Fetch the product price
                            cmd.CommandText = "SELECT prod_price FROM Product WHERE prod_id = @prod_id";
                            cmd.Parameters.Clear(); // Clear parameters before setting new ones
                            cmd.Parameters.AddWithValue("@prod_id", productId);
                            prodPrice = (decimal)cmd.ExecuteScalar();

                            if (existingCartCount != null)
                            {
                                // If the product exists, update the quantity and total price
                                newCartCount = Convert.ToInt32(existingCartCount) + quantity;
                                cmd.CommandText = "UPDATE Cart SET cart_qty = @newCartCount, cart_total = @cart_total WHERE cus_id = @cus_id AND prod_id = @prod_id";
                                cmd.Parameters.Clear(); // Clear parameters before setting new ones
                                cmd.Parameters.AddWithValue("@newCartCount", newCartCount);
                                cmd.Parameters.AddWithValue("@cart_total", newCartCount * prodPrice);
                                cmd.Parameters.AddWithValue("@cus_id", userId);
                                cmd.Parameters.AddWithValue("@prod_id", productId);
                                cmd.ExecuteNonQuery();

                                response = new { success = true, message = "Product quantity updated successfully" };
                            }
                            else
                            {
                                // If the product does not exist, insert a new record
                                newCartCount = quantity;
                                cmd.CommandText = "INSERT INTO Cart (cus_id, prod_id, cart_qty, cart_total) VALUES (@cus_id, @prod_id, @cart_qty, @cart_total)";
                                cmd.Parameters.Clear(); // Clear parameters before setting new ones
                                cmd.Parameters.AddWithValue("@cus_id", userId);
                                cmd.Parameters.AddWithValue("@prod_id", productId);
                                cmd.Parameters.AddWithValue("@cart_qty", newCartCount);
                                cmd.Parameters.AddWithValue("@cart_total", newCartCount * prodPrice);
                                cmd.ExecuteNonQuery();

                                response = new { success = true, message = "Product added to cart successfully" };
                            }

                            // Update the product's stock
                            cmd.CommandText = "UPDATE Product SET prod_stock = prod_stock - @quantity WHERE prod_id = @prod_id";
                            cmd.Parameters.Clear(); // Clear parameters before setting new ones
                            cmd.Parameters.AddWithValue("@quantity", quantity);
                            cmd.Parameters.AddWithValue("@prod_id", productId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response = new { success = false, message = "Error adding to cart: " + ex.Message };
                        return Json(response, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteCart(int prod_id)
        {
            var response = new { success = false, message = "Error deleting from cart" };

            var userId = Session["UserId"];
            if (userId == null)
            {
                response = new { success = false, message = "User not logged in" };
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var db = new SqlConnection(conn_str))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.Transaction = transaction;
                                cmd.CommandType = CommandType.Text;

                                // Get the quantity of the product in the cart
                                cmd.CommandText = "SELECT cart_qty FROM Cart WHERE cus_id = @cus_id AND prod_id = @prod_id";
                                cmd.Parameters.AddWithValue("@cus_id", userId);
                                cmd.Parameters.AddWithValue("@prod_id", prod_id);

                                var cartQty = cmd.ExecuteScalar();
                                if (cartQty == null)
                                {
                                    response = new { success = false, message = "Product not found in cart" };
                                    return Json(response, JsonRequestBehavior.AllowGet);
                                }
                                int quantity = Convert.ToInt32(cartQty);
                                cmd.Parameters.Clear(); // Clear parameters after executing the query

                                // Delete the product from the cart
                                cmd.CommandText = "DELETE FROM Cart WHERE cus_id = @cus_id AND prod_id = @prod_id";
                                cmd.Parameters.AddWithValue("@cus_id", userId);
                                cmd.Parameters.AddWithValue("@prod_id", prod_id);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear(); // Clear parameters after executing the query

                                // Update the product's stock
                                cmd.CommandText = "UPDATE Product SET prod_stock = prod_stock + @quantity WHERE prod_id = @prod_id";
                                cmd.Parameters.AddWithValue("@quantity", quantity);
                                cmd.Parameters.AddWithValue("@prod_id", prod_id);
                                cmd.ExecuteNonQuery();

                                transaction.Commit();

                                response = new { success = true, message = "Product removed from cart and stock updated" };
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response = new { success = false, message = "Error deleting from cart: " + ex.Message };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = "Database error: " + ex.Message };
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateCartQuantity(int prodId, int newQty)
        {
            var response = new { success = false, message = "Error updating cart quantity" };

            var userId = Session["UserId"];
            if (userId == null)
            {
                response = new { success = false, message = "User not logged in" };
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            try
            {
                using (var db = new SqlConnection(conn_str))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                            using (var cmd = db.CreateCommand())
                            {
                                cmd.Transaction = transaction;
                                cmd.CommandType = CommandType.Text;

                                // Get the current quantity in the cart
                                cmd.CommandText = "SELECT cart_qty FROM Cart WHERE cus_id = @cus_id AND prod_id = @prod_id";
                                cmd.Parameters.AddWithValue("@cus_id", userId);
                                cmd.Parameters.AddWithValue("@prod_id", prodId);

                                var cartQty = cmd.ExecuteScalar();
                                if (cartQty == null)
                                {
                                    response = new { success = false, message = "Product not found in cart" };
                                    return Json(response, JsonRequestBehavior.AllowGet);
                                }
                                int oldQty = Convert.ToInt32(cartQty);
                                cmd.Parameters.Clear(); // Clear parameters after executing the query

                                // Update the cart quantity
                                cmd.CommandText = "UPDATE Cart SET cart_qty = @newQty, cart_total = @newQty * prod_price FROM Cart c JOIN Product p ON c.prod_id = p.prod_id WHERE c.cus_id = @cus_id AND c.prod_id = @prod_id";
                                cmd.Parameters.AddWithValue("@newQty", newQty);
                                cmd.Parameters.AddWithValue("@cus_id", userId);
                                cmd.Parameters.AddWithValue("@prod_id", prodId);
                                cmd.ExecuteNonQuery();
                                cmd.Parameters.Clear(); // Clear parameters after executing the query

                                // Update the product stock
                                int qtyDifference = newQty - oldQty;
                                cmd.CommandText = "UPDATE Product SET prod_stock = prod_stock - @qtyDifference WHERE prod_id = @prod_id";
                                cmd.Parameters.AddWithValue("@qtyDifference", qtyDifference);
                                cmd.Parameters.AddWithValue("@prod_id", prodId);
                                cmd.ExecuteNonQuery();

                                transaction.Commit();

                                response = new { success = true, message = "Cart quantity updated and stock adjusted" };
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            response = new { success = false, message = "Error updating cart quantity: " + ex.Message };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response = new { success = false, message = "Database error: " + ex.Message };
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Product_Details()
        {
            return View();
        }
    }
}
