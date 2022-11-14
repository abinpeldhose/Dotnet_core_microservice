using customerSdmodule.Model1;

namespace customerSdmodule
{
    public class GetPhoneNumber
    {
        public static string getphone(string customerid, ModelContext db)
        {


            var cusdata = (from customer in db.Customers

                           where customer.CustId == customerid



                           select new
                           {
                               phone1 = customer.Phone1 == null ? "0" : customer.Phone1 == "0" ? "0" : customer.Phone1.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                               phone2 = customer.Phone2 == null ? "0" : customer.Phone1 == "0" ? "0" : customer.Phone2.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                               //phone1 = customer.Phone1.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                               //phone2 = customer.Phone2.TrimStart(new char[] { '0', '1', '2', '3', '4' }),
                               firm = customer.FirmId,
                               branch = customer.BranchId,

                           }).SingleOrDefault();
            // 
            string PhoneNumber = "1111111111";
            if (cusdata != null)
            {

                try
                {
                    if (cusdata.phone1.StartsWith("6") || cusdata.phone1.StartsWith("7") || cusdata.phone1.StartsWith("8") || cusdata.phone1.StartsWith("9") || cusdata.phone2.StartsWith("6") || cusdata.phone2.StartsWith("7") || cusdata.phone2.StartsWith("8") || cusdata.phone2.StartsWith("9"))
                    {

                        if (cusdata.phone1.Length == 10 || cusdata.phone2.Length == 10)
                        {
                            var showdata = new
                            {

                                Phone1 = cusdata.phone1.Length == 10 ? cusdata.phone1 : cusdata.phone2,


                            };
                            PhoneNumber = showdata.Phone1;

                        }
                    }
                }
                catch (Exception ex)
                {
                    PhoneNumber = "1111111111";
                }

            }
            return PhoneNumber;



        }
    }
}
