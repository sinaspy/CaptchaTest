using System;

namespace CaptchaTest
{
    public class GasBill
    {
        public string FullName { set; get; }
        public string Address { set; get; }
        public DateTime? CurrentDate { set; get; }
        public DateTime? PreviousDate { set; get; }
        public DateTime? PaymentDate { set; get; }
        public string PaymentID { set; get; }
        public string BillID { set; get; }
        public long Amount { set; get; }
        public string ExtraInfo { set; get; }
    }
}
//public class GasBill
//{
//    public string FullName { get; set; }
//    public string Address { get; set; }
//    public GasBillDetail[] Details { get; set; }
//}
//public class GasBillDetail
//{
//    public string Amount { get; set; }
//    public string PaymentID { get; set; }
//    public string BillID { get; set; }
//    public string PaymentDate { get; set; }
//    public string PreviousDate { get; set; }
//    public string CurrentDate { get; set; }
//}