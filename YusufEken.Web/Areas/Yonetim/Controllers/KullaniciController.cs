using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YusufEken.DataAccessLayer;
using YusufEken.EntityLayer;

namespace YusufEken.Web.Areas.Yonetim.Controllers
{
    [Authorize]
    public class KullaniciController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            var kullanicilar = _unitOfWork.KullaniciRepository.GetAll().ToList();
            return View(kullanicilar);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ad,Soyad,Eposta,Parola,ParolaTekrar")] Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                if (kullanici.Parola != kullanici.ParolaTekrar)
                {
                    ModelState.AddModelError("ParolaTekrar", "Parolalar eşleşmiyor.");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _unitOfWork.KullaniciRepository.Add(kullanici);
                        _unitOfWork.Save();
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla eklendi.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            return View(kullanici);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = _unitOfWork.KullaniciRepository.GetItem(id.Value);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            kullanici.Parola = "";
            kullanici.ParolaTekrar = "";
            return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ad,Soyad,Eposta,Parola,ParolaTekrar")] Kullanici kullanici)
        {
            if (ModelState.IsValid)
            {
                bool parolaDegistirilecek = !string.IsNullOrEmpty(kullanici.Parola) || !string.IsNullOrEmpty(kullanici.ParolaTekrar);

                if (parolaDegistirilecek)
                {
                    if (kullanici.Parola != kullanici.ParolaTekrar)
                    {
                        ModelState.AddModelError("ParolaTekrar", "Parolalar eşleşmiyor.");
                    }
                    else if (string.IsNullOrEmpty(kullanici.Parola))
                    {
                        ModelState.AddModelError("Parola", "Yeni parola boş olamaz.");
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var mevcutKullanici = _unitOfWork.KullaniciRepository.GetItem(kullanici.Id);
                        if (mevcutKullanici == null) return HttpNotFound();

                        mevcutKullanici.Ad = kullanici.Ad;
                        mevcutKullanici.Soyad = kullanici.Soyad;
                        mevcutKullanici.Eposta = kullanici.Eposta;

                        if (parolaDegistirilecek)
                        {
                            mevcutKullanici.Parola = kullanici.Parola;
                        }

                        _unitOfWork.KullaniciRepository.Update(mevcutKullanici);
                        _unitOfWork.Save();
                        TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                }
            }
            return View(kullanici);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kullanici kullanici = _unitOfWork.KullaniciRepository.GetItem(id.Value);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kullanici kullanici = _unitOfWork.KullaniciRepository.GetItem(id);
            if (kullanici == null)
            {
                TempData["ErrorMessage"] = "Silinmek istenen kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }
            if (User.Identity.Name.Equals(kullanici.Eposta, StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Aktif olarak giriş yapmış olduğunuz kullanıcıyı silemezsiniz.";
                return RedirectToAction("Index");
            }

            _unitOfWork.KullaniciRepository.Delete(kullanici);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
