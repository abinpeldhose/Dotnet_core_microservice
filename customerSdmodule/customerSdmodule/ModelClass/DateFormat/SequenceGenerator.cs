using customerSdmodule.Model1;

namespace customerSdmodule.ModelClass.DateFormat
{
    public class SequenceGenerator
    {
        public int getSequence(ModelContext db, byte pfirmid, short pbranchid, byte pmoduleid, int pkeyid)
        {
            var data = db.KeyMasters.FirstOrDefault(x => x.FirmId == pfirmid && x.BranchId == pbranchid && x.ModuleId == pmoduleid && x.KeyId == pkeyid);
            int result = 0;
            if (data != null)
                result = (int)(data.Value = data.Value + 1);

            //int result=(int)(++data!.Value);
            return result;



       
        }

        public long MessageId(ModelContext db)
        {
            var details = db.KeyMasters.Where(x => x.KeyId == 11 && x.Description == "MESSEGE_ID").FirstOrDefault();
            details.Value = details.Value + 1;
            db.SaveChanges();
            return (long)details.Value;
        }
    }
}
