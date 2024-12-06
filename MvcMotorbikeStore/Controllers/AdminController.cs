using MvcMotorbikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace MvcMotorbikeStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLBanxeganmayDataContext data;
        public AdminController()
        {
            // Get the connection string from the Web.config file
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["QLBANXEGANMAYConnectionString"].ConnectionString;

            // Initialize the DataContext with the connection string
            data = new dbQLBanxeganmayDataContext(connectionString);
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Xe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(data.XEGANMAYs.ToList().OrderBy(n => n.MaXe).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"]; if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                // Gán giá trị cho đối tượng được tạo mới (ad)
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Themmoixe()
        {
            //Dua du lieu vao dropdownlist
            //Lay ds tu tabke chu de, såp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thì thi Tenchude
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe");
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoixe(XEGANMAY xe, HttpPostedFileBase fileupload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe");
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP");
            //Kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/hinhsanpham"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hình anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    xe.Anhbia = fileName;
                    //Luu vao CSDL
                    data.XEGANMAYs.InsertOnSubmit(xe);
                    data.SubmitChanges();
                }
                return RedirectToAction("Xe");

            }
        }

        //Chỉnh sửa sản phẩm
        [HttpGet]
        public ActionResult Suaxe(int id)
        {
            //Lay ra doi tuong xe theo ma
            XEGANMAY xe = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdowniist
            //Lay da tu tabke chu de, såp xep tang dan trheo ten chu de, chon lay gia tri Me CD, hien thi thi Tence
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n =>n.TenLoaiXe), "MaLX", "TenLoaixe", xe.MaLX); 
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", xe.MaNPP); 
            return View(xe);
        }
        /*
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaxe(XEGANMAY xe, HttpPostedFileBase fileupload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe", xe.MaLX);
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", xe.MaNPP);
            //Kiem tra duong dan file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System. 10;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/hinhsanpham"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hình anh vao duong dan
                        fileupload.SaveAs (path);
                    }
                    xe.Anhbia = fileName;
                    //Luu vao CSDL
                    UpdateModel(xe);
                    data.SubmitChanges();
                }
                return RedirectToAction("Xe");
            }
            
            /*
            // Repopulate dropdown lists for the form
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaiXe", xe.MaLX);
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", xe.MaNPP);

            // Kiểm tra tính hợp lệ của model
            if (ModelState.IsValid)
            {
                // Kiểm tra và xử lý ảnh
                if (fileupload != null && fileupload.ContentLength > 0)
                {
                    // Validate the file type
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                    string fileExtension = Path.GetExtension(fileupload.FileName).ToLower();

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("fileupload", "Chỉ hỗ trợ các định dạng ảnh: .jpg, .jpeg, .png, .gif");
                        return View(xe); // Return the view if the file type is not valid
                    }

                    // Tạo tên file duy nhất
                    string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;

                    // Đường dẫn lưu ảnh
                    var path = Path.Combine(Server.MapPath("~/Images"), uniqueFileName);

                    try
                    {
                        // Lưu ảnh vào thư mục
                        fileupload.SaveAs(path);

                        // Cập nhật thông tin ảnh vào đối tượng xe
                        xe.Anhbia = uniqueFileName; // Save the unique filename to the model
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("fileupload", "Có lỗi xảy ra khi tải lên ảnh: " + ex.Message);
                        return View(xe); // Return the view with the error message
                    }
                }
                else
                {
                    // If no new file is uploaded, retain the existing image
                    var existingXe = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == xe.MaXe);
                    if (existingXe != null)
                    {
                        xe.Anhbia = existingXe.Anhbia; // Retain the existing image if no new image is uploaded
                    }
                }

                // Cập nhật thông tin xe vào cơ sở dữ liệu
                var existingXeInDb = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == xe.MaXe);
                if (existingXeInDb != null)
                {
                    existingXeInDb.TenXe = xe.TenXe;
                    existingXeInDb.Giaban = xe.Giaban; // -strong / -heart:>:o: -((: -h existingXeInDb.Mota = xe.Mota;
                    existingXeInDb.Anhbia = xe.Anhbia; // Make sure Anhbia is updated
                    existingXeInDb.Ngaycapnhat = xe.Ngaycapnhat;
                    existingXeInDb.Soluongton = xe.Soluongton;
                    existingXeInDb.MaLX = xe.MaLX;
                    existingXeInDb.MaNPP = xe.MaNPP;

                    data.SubmitChanges(); // Save changes to the database
                }

                // Redirect to the list of cars after successful update
                return RedirectToAction("Xe");
            }

            // Return the view if model is not valid
            return View(xe);
            
        }
        */


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suaxe(XEGANMAY xe, HttpPostedFileBase fileupload)
        {
            // Gán dữ liệu vào dropdown
            ViewBag.MaLX = new SelectList(data.LOAIXEs.ToList().OrderBy(n => n.TenLoaiXe), "MaLX", "TenLoaixe", xe.MaLX);
            ViewBag.MaNPP = new SelectList(data.NHAPHANPHOIs.ToList().OrderBy(n => n.TenNPP), "MaNPP", "TenNPP", xe.MaNPP);

            // Kiểm tra fileupload
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View(xe); // Trả về model để giữ dữ liệu
            }

            if (ModelState.IsValid)
            {
                // Lấy tên file
                var fileName = Path.GetFileName(fileupload.FileName);
                var directory = Server.MapPath("~/hinhsanpham");

                // Kiểm tra thư mục tồn tại
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var path = Path.Combine(directory, fileName);

                // Kiểm tra hình ảnh đã tồn tại
                if (!System.IO.File.Exists(path))
                {
                    fileupload.SaveAs(path);
                    xe.Anhbia = fileName;

                    // Cập nhật dữ liệu
                    var xeToUpdate = data.XEGANMAYs.SingleOrDefault(x => x.MaXe == xe.MaXe);
                    if (xeToUpdate != null)
                    {
                        xeToUpdate.TenXe = xe.TenXe;
                        xeToUpdate.Giaban = xe.Giaban;
                        xeToUpdate.Mota = xe.Mota;
                        xeToUpdate.Anhbia = fileName;
                        xeToUpdate.Ngaycapnhat = xe.Ngaycapnhat;
                        xeToUpdate.Soluongton = xe.Soluongton;
                        xeToUpdate.MaLX = xe.MaLX;
                        xeToUpdate.MaNPP = xe.MaNPP;

                        data.SubmitChanges();
                    }
                }
                else
                {
                    ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    return View(xe);
                }

                return RedirectToAction("Xe");
            }

            return View(xe); // Nếu ModelState không hợp lệ, trả về View cùng dữ liệu đã nhập
        }





        //Hiển thị sản phẩm
        public ActionResult Chitietxe(int id)
        {
            //Lay ra doi tuong xe theo ma
            XEGANMAY xe = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.Maxe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoaxe(int id)
        {
            //Lay ra doi tuong xe can xoa theo ma
            XEGANMAY xe = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.Maxe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(xe);
        }

        [HttpPost, ActionName("Xoaxe")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong xe can xoa theo ma
            XEGANMAY xe = data.XEGANMAYs.SingleOrDefault(n => n.MaXe == id);
            ViewBag.Maxe = xe.MaXe;
            if (xe == null)
            {
                Response.StatusCode = 404;
                return null;
            }            
            data.XEGANMAYs.DeleteOnSubmit(xe);
            data.SubmitChanges();
            return RedirectToAction("Xe");
        }

        public ActionResult Thongkexe()
        {
            // Thống kê số lượng xe theo loại xe
            var dt = data.XEGANMAYs
                .GroupBy(s => s.MaLX) // Nhóm theo Loại xe
                .Select(g => new
                {
                    Loaixe = g.FirstOrDefault().LOAIXE.TenLoaiXe,
                    SoLuong = g.Count() // Đếm số lượng xe
                })
                .ToList()
                .Select(x => new ThongKeXeViewModel
                 {
                     TenLoaiXe = x.Loaixe ?? "Không xác định",
                     SoLuongXe = x.SoLuong
                 })
               .ToList();

            return View(dt);
        }
    }
}