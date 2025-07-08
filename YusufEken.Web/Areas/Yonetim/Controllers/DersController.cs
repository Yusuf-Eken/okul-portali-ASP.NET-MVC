using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using YusufEken.DataAccessLayer;
using YusufEken.EntityLayer;

namespace YusufEken.Web.Areas.Yonetim.Controllers
{
    [Authorize]
    public class DersController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        private void PopulateDropdowns(object selectedBolum = null, object selectedDonem = null, object selectedPersonel = null)
        {
            ViewBag.BolumId = new SelectList(_unitOfWork.BolumRepository.GetAll().OrderBy(b => b.BolumAd).ToList(), "Id", "BolumAd", selectedBolum);
            ViewBag.DonemId = new SelectList(_unitOfWork.DonemRepository.GetAll().OrderBy(d => d.DonemAd).ToList(), "Id", "DonemAd", selectedDonem);
            ViewBag.PersonelId = new SelectList(_unitOfWork.PersonelRepository.GetAll().OrderBy(p => p.PerAd).ThenBy(p => p.PerSoyad)
                .Select(p => new { Id = p.Id, AdSoyad = p.Unvan + " " + p.PerAd + " " + p.PerSoyad }).ToList(), "Id", "AdSoyad", selectedPersonel);
        }

        // GET: Yonetim/Ders
        public ActionResult Index()
        {
            var dersler = _unitOfWork.DersRepository.GetAll()
                                     .Include(d => d.Bolum)
                                     .Include(d => d.Donem)
                                     .Include(d => d.Personel)
                                     .ToList();
            return View(dersler);
        }

        // GET: Yonetim/Ders/Create
        public ActionResult Create()
        {
            PopulateDropdowns();
            return View();
        }

        // POST: Yonetim/Ders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DersAd,Teori,Uygulama,Kredi,AKTS,PersonelId,BolumId,DonemId")] Ders ders)
        {
            if (ModelState.IsValid)
            {

                _unitOfWork.DersRepository.Add(ders);
                _unitOfWork.Save();
                TempData["SuccessMessage"] = "Ders başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            PopulateDropdowns(ders.BolumId, ders.DonemId, ders.PersonelId);
            return View(ders);
        }

        // GET: Yonetim/Ders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ders ders = _unitOfWork.DersRepository.GetItem(id.Value);
            if (ders == null)
            {
                return HttpNotFound();
            }
            PopulateDropdowns(ders.BolumId, ders.DonemId, ders.PersonelId);
            return View(ders);
        }

        // POST: Yonetim/Ders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DersAd,Teori,Uygulama,Kredi,AKTS,PersonelId,BolumId,DonemId")] Ders ders)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.DersRepository.Update(ders);
                _unitOfWork.Save();
                TempData["SuccessMessage"] = "Ders başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            PopulateDropdowns(ders.BolumId, ders.DonemId, ders.PersonelId);
            return View(ders);
        }

        // GET: Yonetim/Ders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ders ders = _unitOfWork.DersRepository.GetAll()
                                .Include(d => d.Bolum)
                                .Include(d => d.Donem)
                                .Include(d => d.Personel)
                                .FirstOrDefault(d => d.Id == id.Value);
            if (ders == null)
            {
                return HttpNotFound();
            }
            return View(ders);
        }

        // POST: Yonetim/Ders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ders ders = _unitOfWork.DersRepository.GetItem(id);
            if (ders == null)
            {
                TempData["ErrorMessage"] = "Silinmek istenen ders bulunamadı.";
                return RedirectToAction("Index");
            }
            _unitOfWork.DersRepository.Delete(ders);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = "Ders başarıyla silindi.";
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