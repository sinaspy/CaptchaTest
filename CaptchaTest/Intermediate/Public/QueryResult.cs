using System.Data;

namespace CaptchaTest
{
    public partial class QueryResult
    {
        public string Text { set; get; }
        public int SpCode { set; get; }
        public int ReturnCode { set; get; }
        public DataSet DataSet { set; get; }
        public bool HasError { get { return InitiateHasError(); } } //skopro : => InitiateHasError();


        private bool InitiateHasError()
        {
            if (ReturnCode != 1 || SpCode != 1)
                return true;

            return false;
        }
    }
}