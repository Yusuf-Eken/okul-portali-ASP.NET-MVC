using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using YusufEken.DataAccessLayer;
using YusufEken.EntityLayer;

namespace YusufEken.Web.Areas.Yonetim.Controllers
{
    [Authorize]
    public class PersonelController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();


        public ActionResult Index()
        {
            var personeller = _unitOfWork.PersonelRepository.GetAll().Include(p => p.Bolum).ToList();
            return View(personeller);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personel personel = _unitOfWork.PersonelRepository.GetAll()
                                        .Include(p => p.Bolum)
                                        .FirstOrDefault(p => p.Id == id.Value);
            if (personel == null)
            {
                return HttpNotFound();
            }
            ViewBag.VerdigiDersler = _unitOfWork.DersRepository.GetAll()
                                        .Include(d => d.Donem)
                                        .Include(d => d.Bolum)
                                        .Where(d => d.PersonelId == id.Value)
                                        .ToList();
            return View(personel);
        }
        private void PopulateBolumlerDropdown(object selectedBolum = null)
        {
            ViewBag.BolumIdList = new SelectList(_unitOfWork.BolumRepository.GetAll().ToList(), "Id", "BolumAd", selectedBolum);
        }

        public ActionResult Create()
        {
            PopulateBolumlerDropdown();
            return View(new Personel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Unvan,PerAd,PerSoyad,PerEposta,DahiliNo,BolumId")] Personel personel, HttpPostedFileBase imageFile)
        {
            if (_unitOfWork.PersonelRepository.GetAll().Any(p => p.PerEposta.Equals(personel.PerEposta, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("PerEposta", "Bu e-posta adresi zaten kayıtlı.");
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    fileName = fileName.Replace(" ", "-") + DateTime.Now.ToString("yymmssfff") + extension;
                    personel.ImageUrl = "/Images/PersonelImages/" + fileName;
                    string serverPath = Path.Combine(Server.MapPath("~/Images/PersonelImages/"), fileName);

                    string directoryPath = Server.MapPath("~/Images/PersonelImages/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    imageFile.SaveAs(serverPath);
                }
                else
                {
                    personel.ImageUrl = "/Images/PersonelImages/default.png";
                }

                _unitOfWork.PersonelRepository.Add(personel);
                _unitOfWork.Save();
                TempData["SuccessMessage"] = "Personel başarıyla eklendi.";
                return RedirectToAction("Index");
            }

            PopulateBolumlerDropdown(personel.BolumId);
            return View(personel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personel personel = _unitOfWork.PersonelRepository.GetItem(id.Value);
            if (personel == null)
            {
                return HttpNotFound();
            }
            PopulateBolumlerDropdown(personel.BolumId);
            return View(personel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Unvan,PerAd,PerSoyad,PerEposta,DahiliNo,BolumId,ImageUrl")] Personel personel, HttpPostedFileBase imageFile)
        {
            if (_unitOfWork.PersonelRepository.GetAll().Any(p => p.PerEposta.Equals(personel.PerEposta, StringComparison.OrdinalIgnoreCase) && p.Id != personel.Id))
            {
                ModelState.AddModelError("PerEposta", "Bu e-posta adresi başka bir personele ait.");
            }

            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    if (!string.IsNullOrEmpty(personel.ImageUrl) && !personel.ImageUrl.EndsWith("default.png") && !personel.ImageUrl.Contains("lorempixel"))
                    {
                        string oldFilePath = Server.MapPath(personel.ImageUrl);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            try { System.IO.File.Delete(oldFilePath); }
                            catch (IOException) { }
                        }
                    }

                    string fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    string extension = Path.GetExtension(imageFile.FileName);
                    fileName = fileName.Replace(" ", "-") + DateTime.Now.ToString("yymmssfff") + extension;
                    personel.ImageUrl = "/Images/PersonelImages/" + fileName;
                    string serverPath = Path.Combine(Server.MapPath("~/Images/PersonelImages/"), fileName);

                    string directoryPath = Server.MapPath("~/Images/PersonelImages/");
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                    imageFile.SaveAs(serverPath);
                }

                _unitOfWork.PersonelRepository.Update(personel);
                _unitOfWork.Save();
                TempData["SuccessMessage"] = "Personel başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            PopulateBolumlerDropdown(personel.BolumId);
            return View(personel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personel personel = _unitOfWork.PersonelRepository.GetAll().Include(p => p.Bolum).FirstOrDefault(p => p.Id == id.Value);
            if (personel == null)
            {
                return HttpNotFound();
            }
            return View(personel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personel personel = _unitOfWork.PersonelRepository.GetItem(id);
            if (personel == null)
            {
                TempData["ErrorMessage"] = "Silinmek istenen personel bulunamadı.";
                return RedirectToAction("Index");
            }

            var iliskiliDersler = _unitOfWork.DersRepository.GetAll().Where(d => d.PersonelId == id).ToList();
            foreach (var ders in iliskiliDersler)
            {
                ders.PersonelId = null;
                _unitOfWork.DersRepository.Update(ders);
            }
            if (iliskiliDersler.Any()) _unitOfWork.Save();

            if (!string.IsNullOrEmpty(personel.ImageUrl) && !personel.ImageUrl.EndsWith("default.png") && !personel.ImageUrl.Contains("lorempixel"))
            {
                string filePath = Server.MapPath("~" + personel.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    try { System.IO.File.Delete(filePath); }
                    catch (IOException) { }
                }
            }

            _unitOfWork.PersonelRepository.Delete(personel);
            _unitOfWork.Save();
            TempData["SuccessMessage"] = "Personel başarıyla silindi.";
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
