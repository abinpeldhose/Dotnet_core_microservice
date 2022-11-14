using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Redis;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.ReportCompany
{
    public class ReportCompanyApi : BaseApi
    {

       
        private ReportCompanyData _data;

      //  private string _jwtToken;
    
        public ReportCompanyData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return ReportCompany(db);
        }
        public ResponseData ReportCompany(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var data = new
                {
                    status = "Failed",
                };
                var firstDayOfMonth = new DateTime(DateFunctions.sysdate(db).Year, DateFunctions.sysdate(db).Month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
                if (Data.Flag == 0)///////today ////////////////
                {

                    if (cacheDetails.BranchId == 0)
                    {
                        var reports = (from report in db.SdMasters
                                       join branch in db.BranchMasters on report.BranchId equals branch.BranchId
                                       join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                       join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                       where report.FirmId == Data.Firmid && report.DepositDate < DateFunctions.sysdate(db).AddDays(1) && report.DepositDate > DateFunctions.sysdate(db).AddDays(-1)
                                       orderby report.DepositDate descending
                                       select new
                                       {
                                           region = region.RegionId,
                                           area = "MABEN" + area.DistrictName,
                                           branchId = branch.BranchId,
                                           branchName = branch.BranchName,
                                           count = 0,
                                           amount = report.DepositAmt,

                                       }).ToList();
                        var Summery = reports.GroupBy(t => t.branchId).
                   Select(t => new
                   {
                       branchId = t.Key,
                       Count = t.Count(),
                       Amount = t.Sum(ta => ta.amount),

                   }).ToList();

                        var merge = (from m1 in Summery
                                     join branch in db.BranchMasters on m1.branchId equals branch.BranchId
                                     join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                     join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                     orderby branch.BranchId ascending
                                     select new
                                     {
                                         regionName = region.RegionName,
                                         area = "MABEN " + area.DistrictName,
                                         branchId = branch.BranchId,
                                         branchName = branch.BranchName,
                                         Count = m1.Count,
                                         amount = m1.Amount,
                                     }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();

                        if (reports == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            // _Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            // return Results.Ok(merge);
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(merge);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                        }

                    }
                    else
                    {
                        var reports = (from report in db.SdMasters
                                       join branch in db.BranchMasters on report.BranchId equals branch.BranchId
                                       join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                       join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                       where branch.BranchId == cacheDetails.BranchId && report.FirmId == Data.Firmid && report.DepositDate < DateFunctions.sysdate(db).AddDays(1) && report.DepositDate > DateFunctions.sysdate(db).AddDays(-1)
                                       orderby report.DepositDate descending
                                       select new
                                       {
                                           region = region.RegionId,
                                           area = "MABEN" + area.DistrictName,
                                           branchId = branch.BranchId,
                                           branchName = branch.BranchName,
                                           count = 0,
                                           amount = report.DepositAmt,

                                       }).ToList();
                        var Summery = reports.GroupBy(t => t.branchId).
                   Select(t => new
                   {
                       branchId = t.Key,
                       Count = t.Count(),
                       Amount = t.Sum(ta => ta.amount),

                   }).ToList();

                        var merge = (from m1 in Summery
                                     join branch in db.BranchMasters on m1.branchId equals branch.BranchId
                                     join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                     join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                     orderby branch.BranchId ascending
                                     select new
                                     {
                                         regionName = region.RegionName,
                                         area = "MABEN " + area.DistrictName,
                                         branchId = branch.BranchId,
                                         branchName = branch.BranchName,
                                         Count = m1.Count,
                                         amount = m1.Amount,
                                     }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();

                        if (reports == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(merge);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            /// _Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                            //  return Results.Ok(merge);
                        }

                    }



                }
                else if (Data.Flag == 1)//month
                {

                    if (cacheDetails.BranchId == 0)
                    {
                        var reports = (from report in db.SdMasters
                                       join branch in db.BranchMasters on report.BranchId equals branch.BranchId
                                       join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                       join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                       where report.FirmId == Data.Firmid && report.DepositDate >= firstDayOfMonth && report.DepositDate <= lastDayOfMonth
                                       orderby report.DepositDate descending
                                       select new
                                       {
                                           region = region.RegionId,
                                           area = "MABEN " + area.DistrictName,
                                           branchId = branch.BranchId,
                                           branchName = branch.BranchName,
                                           count = 0,
                                           amount = report.DepositAmt,

                                       }).ToList();
                        var Summery = reports.GroupBy(t => t.branchId).
                   Select(t => new
                   {
                       branchId = t.Key,
                       Count = t.Count(),
                       Amount = t.Sum(ta => ta.amount),

                   }).ToList();

                        var merge = (from m1 in Summery
                                     join branch in db.BranchMasters on m1.branchId equals branch.BranchId
                                     join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                     join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                     orderby branch.BranchId ascending
                                     select new
                                     {
                                         regionName = region.RegionName,
                                         area = "MABEN " + area.DistrictName,
                                         branchId = branch.BranchId,
                                         branchName = branch.BranchName,
                                         Count = m1.Count,
                                         amount = m1.Amount,
                                     }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();

                        if (reports == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(merge);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            /// _Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                            //  return Results.Ok(merge);
                        }

                    }

                    else
                    {

                        var reports = (from report in db.SdMasters
                                       join branch in db.BranchMasters on report.BranchId equals branch.BranchId
                                       join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                       join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                       where branch.BranchId == cacheDetails.BranchId && report.FirmId == Data.Firmid && report.DepositDate >= firstDayOfMonth && report.DepositDate <= lastDayOfMonth
                                       orderby report.DepositDate descending
                                       select new
                                       {
                                           region = region.RegionId,
                                           area = "MABEN " + area.DistrictName,
                                           branchId = branch.BranchId,
                                           branchName = branch.BranchName,
                                           count = 0,
                                           amount = report.DepositAmt,

                                       }).ToList();
                        var Summery = reports.GroupBy(t => t.branchId).
                   Select(t => new
                   {
                       branchId = t.Key,
                       Count = t.Count(),
                       Amount = t.Sum(ta => ta.amount),

                   }).ToList();

                        var merge = (from m1 in Summery
                                     join branch in db.BranchMasters on m1.branchId equals branch.BranchId
                                     join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                     join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                     orderby branch.BranchId ascending
                                     select new
                                     {
                                         regionName = region.RegionName,
                                         area = "MABEN " + area.DistrictName,
                                         branchId = branch.BranchId,
                                         branchName = branch.BranchName,
                                         Count = m1.Count,
                                         amount = m1.Amount,
                                     }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();

                        if (reports == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(merge);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            /// _Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                            //  return Results.Ok(merge);
                        }

                    }


                }
                else if (Data.Flag == 2) //today settled
                {

                    if (cacheDetails.BranchId == 0)
                    {
                        // var DepositId = db.SdMasters.Where(x => x.CloseDate.Date == DateFunctions.sysdate(db).Date).ToList();
                        var CountDetails = db.SdMasters.Where(x => x.CloseDate.Date == DateFunctions.sysdate(db).Date && x.StatusId == 0).
                        GroupBy(x => x.BranchId).
                        Select(x => new
                        {

                            branchId = x.Key,
                            count = x.Count(),
                            amount = x.Sum(t => t.Balance)


                        }).ToList();
                        //var AmountDetails = (from trans in db.SdTrans
                        //                     join master in db.SdMasters on trans.DepositId equals master.DepositId
                        //                     where master.CloseDate.Date == DateFunctions.sysdate(db).Date
                        //                     group trans by trans.BranchId into g
                        //                     select new
                        //                     {
                        //                         branchId = g.Key,
                        //                         amount = g.Sum(t => t.Type == "C" ? t.Amount : t.Amount * -1),
                        //                     }).ToList();



                        //var TotalAount=
                        //{
                        //    branchId = x.Key,
                        //    Count = x.Count(),
                        //    Amount = x.Sum(t => t.Amount),
                        //});
                        var report = (from countdata in CountDetails
                                          // join amountdata in AmountDetails on countdata.branchId equals amountdata.branchId

                                      join branch in db.BranchMasters on countdata.branchId equals branch.BranchId
                                      join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                      join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId

                                      orderby branch.BranchId ascending
                                      select new
                                      {
                                          regionName = region.RegionName,
                                          area = "MABEN " + area.DistrictName,
                                          branchId = branch.BranchId,
                                          branchName = branch.BranchName,
                                          Count = countdata.count,
                                          amount = countdata.amount
                                      }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();
                        if (report == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(report);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            /// _Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                            //  return Results.Ok(merge);
                        }

                    }

                    else
                    {


                        // var DepositId = db.SdMasters.Where(x => x.CloseDate.Date == DateFunctions.sysdate(db).Date).ToList();
                        var CountDetails = db.SdMasters.Where(x => x.CloseDate.Date == DateFunctions.sysdate(db).Date && x.StatusId == 0).
                        GroupBy(x => x.BranchId).
                        Select(x => new
                        {

                            branchId = x.Key,
                            count = x.Count(),
                            amount = x.Sum(t => t.Balance)


                        }).ToList();
                        //var AmountDetails = (from trans in db.SdTrans
                        //                     join master in db.SdMasters on trans.DepositId equals master.DepositId
                        //                     where master.CloseDate.Date == DateFunctions.sysdate(db).Date
                        //                     group trans by trans.BranchId into g
                        //                     select new
                        //                     {
                        //                         branchId = g.Key,
                        //                         amount = g.Sum(t => t.Type == "C" ? t.Amount : t.Amount * -1),
                        //                     }).ToList();



                        //var TotalAount=
                        //{
                        //    branchId = x.Key,
                        //    Count = x.Count(),
                        //    Amount = x.Sum(t => t.Amount),
                        //});
                        var report = (from countdata in CountDetails
                                          // join amountdata in AmountDetails on countdata.branchId equals amountdata.branchId

                                      join branch in db.BranchMasters on countdata.branchId equals branch.BranchId
                                      join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                      join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                      where branch.BranchId == cacheDetails.BranchId
                                      orderby branch.BranchId ascending
                                      select new
                                      {
                                          regionName = region.RegionName,
                                          area = "MABEN " + area.DistrictName,
                                          branchId = branch.BranchId,
                                          branchName = branch.BranchName,
                                          Count = countdata.count,
                                          amount = countdata.amount
                                      }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();
                        if (report == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(report);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            /// _Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                            //  return Results.Ok(merge);
                        }


                    }


                }
                else if (Data.Flag == 3)//monthly settled
                {
                    if (cacheDetails.BranchId == 0)
                    {


                        var CountDetails = db.SdMasters.Where(x => x.CloseDate.Date > DateFunctions.LastMonthDate1(db).Date &&
                 x.CloseDate.Date <= DateFunctions.sysdate(db).Date &&
                 x.StatusId == 0).GroupBy(x => x.BranchId).
                 Select(x => new
                 {

                     branchId = x.Key,
                     count = x.Count(),
                     amount = x.Sum(t => t.Balance),


                 }).ToList();



                        var report = (from countdata in CountDetails
                                          // join amountdata in AmountDetails on countdata.branchId equals amountdata.branchId
                                      join branch in db.BranchMasters on countdata.branchId equals branch.BranchId
                                      join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                      join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                      orderby branch.BranchId ascending
                                      select new
                                      {
                                          regionName = region.RegionName,
                                          area = "MABEN " + area.DistrictName,
                                          branchId = branch.BranchId,
                                          branchName = branch.BranchName,
                                          Count = countdata.count,
                                          amount = countdata.amount,
                                      }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();

                        if (report == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(report);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            /// _Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                            //  return Results.Ok(merge);
                        }
                    }
                    else
                    {

                        var CountDetails = db.SdMasters.Where(x => x.CloseDate.Date > DateFunctions.LastMonthDate1(db).Date &&
                 x.CloseDate.Date <= DateFunctions.sysdate(db).Date &&
                 x.StatusId == 0).GroupBy(x => x.BranchId).
                 Select(x => new
                 {

                     branchId = x.Key,
                     count = x.Count(),
                     amount = x.Sum(t => t.Balance),


                 }).ToList();



                        var report = (from countdata in CountDetails
                                          // join amountdata in AmountDetails on countdata.branchId equals amountdata.branchId
                                      join branch in db.BranchMasters on countdata.branchId equals branch.BranchId
                                      join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                      join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                      where branch.BranchId == cacheDetails.BranchId
                                      orderby branch.BranchId ascending
                                      select new
                                      {
                                          regionName = region.RegionName,
                                          area = "MABEN " + area.DistrictName,
                                          branchId = branch.BranchId,
                                          branchName = branch.BranchName,
                                          Count = countdata.count,
                                          amount = countdata.amount,
                                      }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();

                        if (report == null)
                        {
                            Log.Error("Report Failed");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(data);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            //_Response.data = JsonSerializer.Serialize(data);
                            return _Response;
                            // return Results.NotFound(data);
                        }
                        else
                        {
                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(report);
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            /// _Response.data = JsonSerializer.Serialize(merge);
                            return _Response;
                            //  return Results.Ok(merge);
                        }

                    }

                }
                else if (Data.Flag == 4)//Growth OS
                {
                    var untilToday = (from report in db.SdTrans
                                      join settle in db.SdMasters on report.DepositId equals settle.DepositId
                                      join branch in db.BranchMasters on report.BranchId equals branch.BranchId
                                      join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                      join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                      where report.FirmId == Data.Firmid && report.TraDt < DateFunctions.sysdate(db).AddDays(1)

                                      select new
                                      {
                                          region = region.RegionId,
                                          area = "MABEN " + area.DistrictName,
                                          branchId = branch.BranchId,
                                          branchName = branch.BranchName,
                                          count = 0,
                                          amount = report.Type == "C" ? report.Amount : report.Amount * -1,

                                      }).ToList();
                    var untilYesterDay = (from report in db.SdTrans
                                          join settle in db.SdMasters on report.DepositId equals settle.DepositId
                                          join branch in db.BranchMasters on report.BranchId equals branch.BranchId
                                          join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                          join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                          where report.FirmId == Data.Firmid && report.TraDt < DateFunctions.sysdate(db)
                                          orderby report.TraDt descending
                                          select new
                                          {
                                              region = region.RegionId,
                                              area = "MABEN " + area.DistrictName,
                                              branchId = branch.BranchId,
                                              branchName = branch.BranchName,
                                              count = 0,
                                              amount = report.Type == "C" ? report.Amount : report.Amount * -1,

                                          }).ToList();

                    var untilLastMonth = (from report in db.SdTrans
                                          join settle in db.SdMasters on report.DepositId equals settle.DepositId
                                          join branch in db.BranchMasters on report.BranchId equals branch.BranchId
                                          join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                          join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                          where report.FirmId == Data.Firmid && report.TraDt <= DateFunctions.LastMonthDate(db)
                                          orderby report.TraDt descending
                                          select new
                                          {
                                              region = region.RegionId,
                                              area = "MABEN " + area.DistrictName,
                                              branchId = branch.BranchId,
                                              branchName = branch.BranchName,
                                              count = 0,
                                              amount = report.Type == "C" ? report.Amount : report.Amount * -1,

                                          }).ToList();
                    var TodayTotal = untilToday.GroupBy(t => t.branchId).
                   Select(t => new
                   {
                       branchId = t.Key,
                       Count = t.Count(),
                       Amount = t.Sum(ta => ta.amount),

                   }).ToList();
                    var YesterDayTotal = untilYesterDay.GroupBy(t => t.branchId).
                   Select(t => new
                   {
                       branchId = t.Key,
                       Count = t.Count(),
                       Amount = t.Sum(ta => ta.amount),

                   }).ToList();
                    var LastMonthTotal = untilLastMonth.GroupBy(t => t.branchId).
                   Select(t => new
                   {
                       branchId = t.Key,
                       Count = t.Count(),
                       Amount = t.Sum(ta => ta.amount),


                   }).ToList();
                    var merge = (from today in TodayTotal
                                 join yesterDay in YesterDayTotal on today.branchId equals yesterDay.branchId
                                 join lastMonth in LastMonthTotal on today.branchId equals lastMonth.branchId
                                 join branch in db.BranchMasters on today.branchId equals branch.BranchId
                                 join region in db.RegionMasters on branch.RegionId equals region.RegionId
                                 join area in db.DistrictMasters on branch.DistrictId equals area.DistrictId
                                 orderby branch.BranchId ascending
                                 select new
                                 {
                                     regionName = region.RegionName,
                                     area = "MABEN " + area.DistrictName,
                                     branchId = branch.BranchId,
                                     branchName = branch.BranchName,
                                     dailyGrowth = today.Amount - yesterDay.Amount,
                                     monthlyGrowth = today.Amount - lastMonth.Amount,
                                     outStanding = today.Amount,
                                 }).Skip((Data.Page - 1) * Data.Pagesize).Take(Data.Pagesize).ToList();
                    Log.Information("Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(merge);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(merge);
                    return _Response;
                }
                else
                {
                    Log.Error("Wrong flag");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "wrong Flag" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    // _Response.data = JsonSerializer.Serialize(new { status = "wrong Flag" });
                    return _Response;
                }
            }

            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(message);
                return _Response;
            }


        }
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;
        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<ReportCompanyData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> failedvalidation = new List<Exception>();
            return failedvalidation;
        }
    }
}
