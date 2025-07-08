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
    public class BolumController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        // GET: Yonetim/Bolum
        public ActionResult Index()
        {
            return View(_unitOfWork.BolumRepository.GetAll().ToList());
        }

        // GET: Yonetim/Bolum/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Yonetim/Bolum/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BolumAd")] Bolum bolum)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.BolumRepository.GetAll().Any(b => b.BolumAd.Equals(bolum.BolumAd, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("BolumAd", "Bu bölüm adı zaten sistemde mevcut.");
                    return View(bolum);
                }

                _unitOfWork.BolumRepository.Add(bolum);
                _unitOfWork.Save();
                TempData["SuccessMessage"] = "Bölüm başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            return View(bolum);
        }

        // GET: Yonetim/Bolum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolum bolum = _unitOfWork.BolumRepository.GetItem(id.Value);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            return View(bolum);
        }

        // POST: Yonetim/Bolum/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BolumAd")] Bolum bolum)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.BolumRepository.GetAll().Any(b => b.BolumAd.Equals(bolum.BolumAd, StringComparison.OrdinalIgnoreCase) && b.Id != bolum.Id))
                {
                    ModelState.AddModelError("BolumAd", "Bu bölüm adı zaten başka bir kayıt için kullanılıyor.");
                    return View(bolum);
                }

                _unitOfWork.BolumRepository.Update(bolum);
                _unitOfWork.Save();
                TempData["SuccessMessage"] = "Bölüm başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            return View(bolum);
        }

        // GET: Yonetim/Bolum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bolum bolum = _unitOfWork.BolumRepository.GetItem(id.Value);
            if (bolum == null)
            {
                return HttpNotFound();
            }
            return View(bolum);
        }

        // POST: Yonetim/Bolum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bolum bolum = _unitOfWork.BolumRepository.GetItem(id);
            if (bolum == null)
            {
                TempData["ErrorMessage"] = "Silinmek istenen bölüm bulunamadı.";
                return RedirectToAction("Index");
            }
            _unitOfWork.BolumRepository.Delete(bolum);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = "Bölüm başarıyla silindi.";
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