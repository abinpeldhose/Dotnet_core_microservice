using System.Text;

namespace customerSdmodule.ModelClass.DateFormat
{
    public class Security
    {
        public String create_hashs(String strData, String date1)
        {
            var message = Encoding.UTF8.GetBytes(strData + date1);
            using var alg = System.Security.Cryptography.SHA512.Create();

            var hashValue = alg.ComputeHash(message);

            return hashValue.Aggregate("", (current, x) => current + $"{x:x2}");
        }
    }
}
