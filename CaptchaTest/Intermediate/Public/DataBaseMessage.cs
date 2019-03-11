namespace CaptchaTest
{
    public partial class DataBaseMessage
    {
        public string Category { set; get; }
        public string ErrorCode { set; get; }
        public string LongDescription { set; get; }
        public string BriefDescription { set; get; }


        //public bool GetErrorBriefDescription()
        //{
        //    var result = ProfileApi.GetErrorBriefDescription(Category, ErrorCode);
        //    if (result.HasError)
        //    {
        //        return false;
        //    }

        //    BriefDescription = result.Value;
        //    return true;
        //}
    }
}