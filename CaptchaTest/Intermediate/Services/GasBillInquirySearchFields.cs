using System;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace CaptchaTest
{
    public class GasBillInquirySearchFields
    {
        public string GasParticipateCode { set; get; }
        public bool GasParticipateCodeIsValid { set; get; }


        public bool DoesGasParticipateCodeValid()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var number = new Regex(@"^[0-9]*$");
                if (string.IsNullOrWhiteSpace(GasParticipateCode))
                {
                    GasParticipateCodeIsValid = false;
                }
                else if (!number.IsMatch(GasParticipateCode))
                {
                    GasParticipateCodeIsValid = false;
                }
                else
                    GasParticipateCodeIsValid = true;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}