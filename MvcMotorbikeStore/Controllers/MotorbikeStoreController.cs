using MvcMotorbikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace MvcMotorbikeStore.Controllers
{
    public class MotorbikeStoreController : Controller
    {
        // GET: MotorbikeStrore
        dbQLBanxeganmayDataContext data;

        public MotorbikeStoreController()
        {
            // Get the connection string from the Web.config file
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["QLBANXEGANMAYConnectionString"].ConnectionString;

            // Initialize the DataContext with the connection string
            data = new dbQLBanxeganmayDataContext(connectionString);
        }
        private List<XEGANMAY> LayXeMoi(int count)
        {
            return data.XEGANMAYs.OrderByDescending(x => x.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 3;
            int pageNum = (page ?? 1);

            var xemoi = LayXeMoi(15);
            return View(xemoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Loaixe()
        {
            var Loaixe = from lx in data.LOAIXEs select lx;
            return PartialView(Loaixe);
        }
        public ActionResult Nhaphanphoi()
        {
            var Nhaphanphoi = from npp in data.NHAPHANPHOIs select npp;
            return PartialView(Nhaphanphoi);
        }
        public ActionResult SPTheoloaixe(int id, int? page)
        {
            int pageSize = 3;
            int pageNum = (page ?? 1);
            var xe = data.XEGANMAYs.Where(s => s.MaLX == id).ToList();
            var pagedXe = xe.ToPagedList(pageNum, pageSize);

            return View(pagedXe);
        }
        public ActionResult SPTheoNPP(int id, int? page)
        {
            int pageSize = 3;
            int pageNum = (page ?? 1);
            var xe = data.XEGANMAYs.Where(s => s.MaNPP == id).ToList();
            var pagedXe = xe.ToPagedList(pageNum, pageSize);
            return View(pagedXe);
        }
        public ActionResult Details(int id)
        {
            var xe = from x in data.XEGANMAYs
                     where x.MaXe == id
                     select x;
            return View(xe.Single());
        }
    }
}