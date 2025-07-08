using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YusufEken.DataAccessLayer;

namespace YusufEken.Web.Areas.Yonetim.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            ViewBag.BolumSayisi = _unitOfWork.BolumRepository.GetAll().Count();
            ViewBag.PersonelSayisi = _unitOfWork.PersonelRepository.GetAll().Count();
            ViewBag.DersSayisi = _unitOfWork.DersRepository.GetAll().Count();
            ViewBag.KullaniciSayisi = _unitOfWork.KullaniciRepository.GetAll().Count();

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}