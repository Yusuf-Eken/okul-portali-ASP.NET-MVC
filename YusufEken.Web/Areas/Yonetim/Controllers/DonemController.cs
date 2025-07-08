using System;
using System.Linq;
using System.Web.Mvc;
using YusufEken.DataAccessLayer;
using YusufEken.EntityLayer;

namespace YusufEken.Web.Areas.Yonetim.Controllers
{
    [Authorize]
    public class DonemController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        public ActionResult Index()
        {
            var donemler = _unitOfWork.DonemRepository.GetAll().ToList();
            return View(donemler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DonemAd")] Donem donem)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.DonemRepository.GetAll().Any(d => d.DonemAd.Equals(donem.DonemAd, StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("DonemAd", "Bu dönem adı zaten sistemde mevcut.");
                    return View(donem);
                }

                _unitOfWork.DonemRepository.Add(donem);
                _unitOfWork.Save();

                TempData["SuccessMessage"] = "Dönem başarıyla eklendi.";
                return RedirectToAction("Index");
            }
            return View(donem);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Donem donem = _unitOfWork.DonemRepository.GetItem(id.Value);
            if (donem == null)
            {
                return HttpNotFound();
            }
            return View(donem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DonemAd")] Donem donem)
        {
            if (ModelState.IsValid)
            {
                if (_unitOfWork.DonemRepository.GetAll().Any(d => d.DonemAd.Equals(donem.DonemAd, StringComparison.OrdinalIgnoreCase) && d.Id != donem.Id))
                {
                    ModelState.AddModelError("DonemAd", "Bu dönem adı zaten başka bir kayıt için kullanılıyor.");
                    return View(donem);
                }

                _unitOfWork.DonemRepository.Update(donem);
                _unitOfWork.Save();

                TempData["SuccessMessage"] = "Dönem başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            return View(donem);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Donem donem = _unitOfWork.DonemRepository.GetItem(id.Value);
            if (donem == null)
            {
                return HttpNotFound();
            }
            return View(donem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donem donem = _unitOfWork.DonemRepository.GetItem(id);
            if (donem == null)
            {
                TempData["ErrorMessage"] = "Silinmek istenen dönem bulunamadı.";
                return RedirectToAction("Index");
            }

            try
            {
                _unitOfWork.DonemRepository.Delete(donem);
                _unitOfWork.Save();
                TempData["SuccessMessage"] = "Dönem başarıyla silindi.";
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
            {
                TempData["ErrorMessage"] = "Bu döneme bağlı dersler olabilir. ilişkili dersleri kontrol edin." + ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Dönem silinirken beklenmedik bir hata oluştu: " + ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}
