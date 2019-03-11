namespace CaptchaTest
{
    public partial class Project
    {
        public string Key { set; get; }
        public string Value { set; get; }
        public string Name { set; get; }
        public string SecretKey { set; get; }
        public bool NameExists { set; get; }


        public bool GetProjectValues()
        {
            var result = DataBase.GetDatabaseInformation(Key);
            if (result.HasError)
            {
                return false;
            }

            var DR = result.DataSet.Tables[0].Rows[0];
            Value = (string)DR.ItemArray[0];
            return true;
        }
    }
}