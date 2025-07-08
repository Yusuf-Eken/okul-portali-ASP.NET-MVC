using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using YusufEken.DataAccessLayer;
using YusufEken.EntityLayer;
using YusufEken.Web.Models;

namespace YusufEken.Web.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork();

        private void LoadBolumlerForLayout()
        {
            ViewBag.LayoutBolumler = _unitOfWork.BolumRepository.GetAll().OrderBy(b => b.BolumAd).ToList();
        }

        public ActionResult Index()
        {
            LoadBolumlerForLayout();
            ViewBag.ActiveMenu = "AnaSayfa";
            var carouselImages = new List<string>
            {
                Url.Content("~/Images/carousel1.jpeg"),
                Url.Content("~/Images/carousel2.jpeg"),
            };
            ViewBag.CarouselImages = carouselImages;
            var personeller = _unitOfWork.PersonelRepository.GetAll()
                                         .Include(p => p.Bolum)
                                         .OrderBy(p => p.PerAd)
                                         .ToList();
            return View(personeller);
        }

        public ActionResult Iletisim()
        {
            LoadBolumlerForLayout();
            ViewBag.ActiveMenu = "Iletisim";
            return View();
        }

        public ActionResult Hakkinda()
        {
            LoadBolumlerForLayout();
            ViewBag.ActiveMenu = "Hakkinda";
            return View();
        }

        public ActionResult Bolum(int? id)
        {
            LoadBolumlerForLayout();
            ViewBag.ActiveMenu = "Bolumler";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Bolum bolum = _unitOfWork.BolumRepository.GetAll()
                                    .Include(b => b.Personeller)
                                    .Include(b => b.Dersler.Select(d => d.Donem))
                                    .Include(b => b.Dersler.Select(d => d.Personel))
                                    .FirstOrDefault(b => b.Id == id.Value);

            if (bolum == null)
            {
                return HttpNotFound();
            }

            List<BolumDersViewModel> derslerViewModelList = bolum.Dersler
                                     .Where(d => d.Donem != null)
                                     .OrderBy(d => d.Donem.Id)
                                     .ThenBy(d => d.DersAd)
                                     .GroupBy(d => d.Donem)
                                     .Select(g => new BolumDersViewModel
                                     {
                                         Donem = g.Key,
                                         Dersler = g.ToList(),
                                         ToplamKredi = g.Sum(d => d.Kredi),
                                         ToplamAKTS = g.Sum(d => d.AKTS)
                                     })
                                     .ToList();

            ViewBag.BolumDersleriByDonem = derslerViewModelList;

            return View(bolum);
        }

        public ActionResult Personel(int? id)
        {
            LoadBolumlerForLayout();
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

            var verdigiDersler = _unitOfWork.DersRepository.GetAll()
                                        .Include(d => d.Bolum)
                                        .Include(d => d.Donem)
                                        .Where(d => d.PersonelId == id.Value)
                                        .OrderBy(d => d.Donem.Id)
                                        .ThenBy(d => d.DersAd)
                                        .ToList();

            ViewBag.VerdigiDersler = verdigiDersler;

            return View(personel);
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