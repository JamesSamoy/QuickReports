using System.Linq;
using System.Web.Mvc;
using EmployeeDataAccess;

namespace MyWebApp.Controllers
{
    public class LoginController : Controller
    {
        // GET
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(LoginCredential credentials)
        {
            using (EmployeeDBEntities_Login db = new EmployeeDBEntities_Login())
            {
                var userDetails = db.LoginCredentials
                    .FirstOrDefault(x => x.UserName == credentials.UserName && x.Password == credentials.Password);
                if (userDetails == null)
                {
                    credentials.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", credentials);
                }
                
                Session["userId"] = credentials.UserId;
                Session["userName"] = credentials.UserName;
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}