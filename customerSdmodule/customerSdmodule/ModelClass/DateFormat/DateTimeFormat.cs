namespace customerSdmodule.ModelClass.DateFormat
{
    public class DateTimeFormat
    {
        public static DateTime yyyMMdd(DateTime date)
        {
            var stringFormat = date.ToString("yyyy-MM-dd");
            var dateFormat = Convert.ToDateTime(stringFormat);
            return dateFormat;
        }
        public static DateTime YYYYMMdd(DateTime date)
        {
            var dateFormat = date.Date;
            return dateFormat;
        }

    }
}
