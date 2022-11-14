using customerSdmodule.Model1;

namespace customerSdmodule.ModelClass.DateFormat
{
    public class InterestCalculator
    {
        public decimal getInterest1(ModelContext db, string DocumentID, String fromDate, String toDate)
        {
            decimal minimumBalance, currentBalance, accumulatedBalance;
            DateTime lastProcessedDate;
            DateTime tempdt;

            //DateTime u =  DateTime.Parse(fromDate);

            decimal openingBalance = db.SdTrans.Where(x => x.DepositId == DocumentID && x.TraDt <= DateTime.Parse(fromDate)).Select(x => (x.Type == "C" ? x.Amount : x.Amount * -1)).Sum();
            TimeSpan ts = Convert.ToDateTime(toDate) - Convert.ToDateTime(fromDate);
            var numberOfDays = ts.Days;
            Console.WriteLine(numberOfDays);
            minimumBalance = openingBalance;
            currentBalance = openingBalance;
            accumulatedBalance = 0;
            lastProcessedDate = DateTime.Parse(fromDate).AddDays(1);
            var dataTran = from sdtran in db.SdTrans
                           where sdtran.DepositId == DocumentID && sdtran.TraDt > Convert.ToDateTime(fromDate)
                           group sdtran by sdtran.TraDt into Trans
                           select new
                           {
                               traDt = Trans.Key,
                               Amount = Trans.Sum(x => x.Type == "C" ? x.Amount : x.Amount * -1),


                           };
            decimal product = 0;

            foreach (var data in dataTran)
            {
                if (lastProcessedDate.ToString() != toDate)
                {
                    if (data.Amount < 0)
                    {
                        numberOfDays = ((TimeSpan)(DateTime.Parse(data.traDt.ToString()) - lastProcessedDate)).Days;
                        tempdt = DateTime.Parse(data.traDt.ToString());
                        lastProcessedDate = DateTime.Parse(data.traDt.ToString());
                        //String closedatee = closedate.ToString();
                    }
                    else
                    {
                        numberOfDays = ((TimeSpan)(DateTime.Parse(data.traDt.ToString()) - lastProcessedDate)).Days + 1;
                        //  lastProcessedDate = DateTime.Parse(data.traDt.Value.ToShortDateString()).AddDays(1).Date;
                        lastProcessedDate = DateTime.Parse(data.traDt.ToString()).AddDays(1).Date;
                    }
                    if (minimumBalance > 100000)
                    {
                        product = numberOfDays * 100000;
                    }
                    else
                    {
                        product = numberOfDays * minimumBalance;
                    }
                    accumulatedBalance = accumulatedBalance + product;

                };
                if (data.Amount >= 0)
                {
                    currentBalance = currentBalance + Math.Abs(data.Amount);
                }
                else
                {
                    currentBalance = currentBalance - Math.Abs(data.Amount);
                }
                if (minimumBalance == openingBalance)
                {
                    minimumBalance = currentBalance;
                }
                if (currentBalance != minimumBalance)
                {
                    minimumBalance = currentBalance;

                }


            }
            if (lastProcessedDate.ToString() != toDate)
            {
                numberOfDays = ((TimeSpan)(DateTime.Parse(toDate) - lastProcessedDate)).Days;
                if (minimumBalance > 100000)
                {
                    product = numberOfDays * 100000;
                }
                else
                {
                    product = numberOfDays * minimumBalance;
                }
                accumulatedBalance = accumulatedBalance + product;

            }



            decimal interest = accumulatedBalance * 5 / 36500;
            interest = Math.Round(interest, 2);

            return interest;




            //var dataOpening = (from sdtran in db.SdTrans
            //                   where DocumentID == sdtran.DepositId && sdtran.TraDt < Convert.ToDateTime(fromDate)
            //                   select new
            //                   {


            //                   }



        }
        public decimal getInterest(ModelContext db, string DocumentID, String fromDate, String toDate)
        {
            decimal minimumBalance, currentBalance, accumulatedBalance;
            DateTime lastProcessedDate;

            //DateTime u =  DateTime.Parse(fromDate);

            decimal openingBalance = db.SdTrans.Where(x => x.DepositId == DocumentID && x.TraDt <= DateTime.Parse(fromDate)).Select(x => (x.Type == "C" ? x.Amount : x.Amount * -1)).Sum();
            TimeSpan ts = Convert.ToDateTime(toDate) - Convert.ToDateTime(fromDate);
            var numberOfDays = ts.Days;
            Console.WriteLine(numberOfDays);
            minimumBalance = openingBalance;
            currentBalance = openingBalance;
            accumulatedBalance = 0;
            lastProcessedDate = DateTime.Parse(fromDate).AddDays(1);
            var dataTran = from sdtran in db.SdTrans
                           where sdtran.DepositId == DocumentID && sdtran.TraDt > Convert.ToDateTime(fromDate)
                           group sdtran by sdtran.TraDt into Trans
                           select new
                           {
                               traDt = Trans.Key,
                               Amount = Trans.Sum(x => x.Type == "C" ? x.Amount : x.Amount * -1),


                           };
            decimal product = 0;

            foreach (var data in dataTran)
            {
                if (lastProcessedDate.ToString() != toDate)
                {
                    if (data.Amount < 0)
                    {
                        numberOfDays = ((TimeSpan)(DateTime.Parse(data.traDt.ToString()) - lastProcessedDate)).Days;
                        lastProcessedDate = DateTime.Parse(data.traDt.ToString());
                    }
                    else
                    {
                        numberOfDays = ((TimeSpan)(DateTime.Parse(data.traDt.ToString()) - lastProcessedDate)).Days + 1;
                        lastProcessedDate = DateTime.Parse(data.traDt.ToString()).AddDays(1);
                    }
                    if (minimumBalance > 100000)
                    {
                        product = numberOfDays * 100000;
                    }
                    else
                    {
                        product = numberOfDays * minimumBalance;
                    }
                    accumulatedBalance = accumulatedBalance + product;

                };
                if (data.Amount >= 0)
                {
                    currentBalance = currentBalance + Math.Abs(data.Amount);
                }
                else
                {
                    currentBalance = currentBalance - Math.Abs(data.Amount);
                }
                if (minimumBalance == openingBalance)
                {
                    minimumBalance = currentBalance;
                }
                if (currentBalance != minimumBalance)
                {
                    minimumBalance = currentBalance;

                }


            }
            if (lastProcessedDate.ToString() != toDate)
            {
                numberOfDays = ((TimeSpan)(DateTime.Parse(toDate) - lastProcessedDate)).Days + 1;
                // numberOfDays = ((TimeSpan)(DateTime.Parse(toDate) - lastProcessedDate)).Days; // Changed on 2022-09-13
                if (minimumBalance > 100000)
                {
                    product = numberOfDays * 100000;
                }
                else
                {
                    product = numberOfDays * minimumBalance;
                }
                accumulatedBalance = accumulatedBalance + product;

            }

            decimal intr = db.GeneralParameters.Where(x => x.ParmtrId == 58 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();
            decimal interest = accumulatedBalance * intr / 36500;
            interest = Math.Round(interest, 2);

            return interest;




           



        }
    }

}
