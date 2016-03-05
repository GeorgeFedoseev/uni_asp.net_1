using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace uni_aspnet_1.Controllers
{
    public class AdminUser {
        public string email, password_md5, session_id;
        public AdminUser(string _email, string _password) {
            email = _email;
            password_md5 = AdminUser.CalculateMD5Hash(_password);
            updateSession();
        }

        public bool compareUser(string _email, string _password) { 
            if(_email == email && AdminUser.CalculateMD5Hash(_password) == password_md5)
                return true;
            return false;
        }

        public string updateSession() {            
            return session_id = RandomString(16);             
        }

        public static string RandomString(int length){
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string CalculateMD5Hash(string input){
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++){
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static bool IsValidEmail(string email){
            try{
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }catch{
                return false;
            }
        }
    }

    public class AdminController : Controller
    {
        static List<AdminUser> users;

        public AdminController() {
            if(users == null)
                users = new List<AdminUser>();
        }

        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public ActionResult SignIn() {
            ViewBag.errors = TempData.ContainsKey("errors")?TempData["errors"]:new List<string>();
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string email, string password){
            var u = findUser(email, password);
            if(u != null){
                var cookie = new HttpCookie("session");
                cookie["id"] = u.updateSession();
                Response.Cookies.Add(cookie);
                return RedirectToAction("Index");            
            }

            // redirect back to sign in page with message about wrong credentials
            TempData["errors"] = new List<string>() {"No user with such email and password"};
            return RedirectToAction("SignIn");
        }

        [HttpGet]
        public ActionResult SignIn() {
            ViewBag.errors = TempData.ContainsKey("errors") ? TempData["errors"] : new List<string>();
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(string email, string password, string password_again) {
            var errors = new List<string>();
            if (password == password_again) {
                if (AdminUser.IsValidEmail(email)) {
                    if (!doesUserExist(email)) {
                        var new_user = new AdminUser(email, password);
                        users.Add(new_user);
                        var cookie = new HttpCookie("session");
                        cookie["id"] = new_user.session_id;
                        Response.Cookies.Add(cookie);                        
                    }else{
                        errors.Add("Пользователь с таким e-mail уже существует");
                    }
                }else{
                    errors.Add("Неверный e-mail");
                }
            }else{
                errors.Add("Пароли не совпадают");
            }

            if (errors.Count > 0) {
                TempData["errors"] = errors;
                return RedirectToAction("SignUp");
            }

            return RedirectToAction("Index");            
        }

        private AdminUser findUser(string email, string password){
            foreach(var u in users){
                if(u.compareUser(email, password))
                    return u;
            }

            return null;
        }

        private bool doesUserExist(string email){
            foreach(var u in users){
                if(u.email == email)
                    return true;
            }

            return false;
        }

    }
}
