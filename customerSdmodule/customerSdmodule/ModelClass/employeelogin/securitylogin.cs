namespace customerSdmodule.ModelClass.employeelogin
{
    public class securitylogin
    {
        public string CreateHash(string SourceText)
        {
            if (String.IsNullOrEmpty(SourceText))
            {
                return String.Empty;
            }
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(SourceText);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return Convert.ToHexString(hashBytes);
            }
        }
    }
}
