using customerSdmodule.Model1;
using Microsoft.EntityFrameworkCore;

namespace customerSdmodule.ModelClass.DateFormat
{
    public class DateFunctions
    {
        public static DateTime sysdate(ModelContext db)
        {
            var date = db.Duals.FromSqlRaw("Select sysdate from dual;").ToListAsync();
            var timeonly = date.Result.Select(x => x.SysDate).SingleOrDefault();
            return timeonly;
        }
        public static DateTime LastMonthDate(ModelContext db)
        {
            var firstDayMonth = new DateTime(sysdate(db).Year, sysdate(db).Month, 1);
            var lastDayMonth = firstDayMonth.AddMonths(-1);
            return lastDayMonth;
        }
        public static DateTime LastMonthDate1(ModelContext db)
        {
            var firstDayMonth = new DateTime(sysdate(db).Year, sysdate(db).Month, 1);
            var lastDayMonth = firstDayMonth.AddDays(-1);
            return firstDayMonth;
        }
        public static bool ValidateAmount(ModelContext db,decimal depositAmount,decimal Amount)
        {
            
            var maximumClosingAmount = db.GeneralParameters.Where(x => x.ParmtrId == 19 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();
            if((depositAmount+Amount)<= maximumClosingAmount)
            {
                return false;
            }
            return true;
           
        }
       

    }
}
