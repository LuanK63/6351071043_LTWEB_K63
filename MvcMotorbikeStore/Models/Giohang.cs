using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace MvcMotorbikeStore.Models
{
    public class Giohang
    {
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo.
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["QLBANXEGANMAYConnectionString"].ConnectionString;
        private dbQLBanxeganmayDataContext data;

        // Properties
        public int iMaXe { get; set; }
        public string sTenXe { get; set; }
        public string sAnhbia { get; set; }
        public double dDongia { get; set; }
        public int iSoluong { get; set; }
        public double dThanhtien
        {
            get { return iSoluong * dDongia; }
        }

        // Constructor
        public Giohang(int MaXe)
        {
            // Initialize the data context
            data = new dbQLBanxeganmayDataContext(connectionString);
            iMaXe = MaXe;
            // Assign the motorcycle information based on MaXe
            var xe = data.XEGANMAYs.SingleOrDefault(x => x.MaXe == MaXe);
            sTenXe = xe.TenXe;
            sAnhbia = xe.Anhbia;
            dDongia = double.Parse(xe.Giaban.ToString());
            iSoluong = 1;
        }
    }
}