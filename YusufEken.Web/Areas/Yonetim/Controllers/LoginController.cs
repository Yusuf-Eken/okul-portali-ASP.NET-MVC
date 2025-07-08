using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using YusufEken.DataAccessLayer;

namespace YusufEken.Web.Areas.Yonetim.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string eposta, string parola, string returnUrl)
        {
            if (string.IsNullOrWhiteSpace(eposta))
            {
                ModelState.AddModelError("Eposta", "E-posta adresi boş geçilemez.");
            }
            if (string.IsNullOrWhiteSpace(parola))
            {
                ModelState.AddModelError("Parola", "Parola boş geçilemez.");
            }

            if (ModelState.IsValid)
            {
                using (var uow = new UnitOfWork())
                {
                    bool kullaniciVarMi = uow.KullaniciRepository.Login(eposta, parola);

                    if (kullaniciVarMi)
                    {
                        FormsAuthentication.SetAuthCookie(eposta, false);

                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Dashboard", new { area = "Yonetim" });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı adı ya da parola hatalı.");
                    }
                }
            }

            ViewBag.Eposta = eposta;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login", new { area = "Yonetim" });
        }
    }
}
