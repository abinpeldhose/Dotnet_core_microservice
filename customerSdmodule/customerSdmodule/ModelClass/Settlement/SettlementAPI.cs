using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using MACOM.Contracts;
using MACOM.Integrations;
using Serilog;
using MACOM.Contracts.Model;
using System.Text.Json;
using customerSdmodule.Redis;
using static RedisCacheDemo.RedisCacheStore;
using customerSdmodule;

namespace customerSdmodule.ModelClass.Settlement
{
    public class SettlementAPI : BaseApi
    {
        private SettlementData _data;
      //  private string _jwtToken;

        string _clientID = Guid.NewGuid().ToString();

        // Console.WriteLine("Client Ready");

        SublegerClient _subLedgerClient = new SublegerClient(Guid.NewGuid().ToString(), "10.192.5.44:19092");


        SubledgerRequest _subLedgerRequest = new SubledgerRequest();

        public SettlementData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return settlement(db);
        }
        public ResponseData settlement(ModelContext db)
        {
            try
            {

                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));


                var Master = db.SdMasters.FirstOrDefault(x => x.CustId == Data.CustomerId && x.DepositId == Data.AccountNumber && x.StatusId == 1);




                if (Master == null)
                {
                    var message = new { Status = "There is no accounts" };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(message);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;

                    //return Results.NotFound(message);
                }

                else if (Data.Transactiontype.ToUpper() == "CASH")
                {
                    SequenceGenerator dep = new SequenceGenerator();
                    var transId = (int)dep.getSequence(db, Master.FirmId, (short)cacheDetails.BranchId, 4, 4);
                    DateTime Todate = DateFunctions.sysdate(db);

                    InterestCalculator intr = new InterestCalculator();
                    string todate = Todate.ToString("yyyy-MM-dd");
                    Decimal amount = intr.getInterest(db, Data.AccountNumber, Master.CloseDate.ToString("yyyy-MM-dd"), todate);
                    //  var SettledAmount = db.SdMasters.Where(x => x.DepositId == AccountNumber && x.CustId == CustomerId ).Select(x => x.DepositAmt).SingleOrDefault();
                    var settle = Math.Round(Master.DepositAmt + amount);
                    var Notround = Master.DepositAmt + amount;
                    var round = settle - Notround;
                    decimal validate = db.GeneralParameters.Where(x => x.ParmtrId == 19 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();

                    decimal int_charge = db.GeneralParameters.Where(x => x.ParmtrId == 15 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();
                    var frm_date = db.SdMasters.Where(x => x.DepositId == Data.AccountNumber).Select(x => x.DepositDate).SingleOrDefault();
                    var interval = frm_date.AddMonths(6);
                    if (Todate < interval)
                    {
                        settle = settle - int_charge;
                    }


                    if (settle > validate)
                    {

                        Log.Error("Limit Exceed please use another transaction method");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Please use another transaction method" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                        // return Results.NotFound(new { status = "Please use another transaction method" });
                    }

                    else
                    {

                        if (Todate < interval)
                        {
                            var prematureCharge = new SdTran
                            {
                                FirmId = Master.FirmId,
                                BranchId = (short)cacheDetails.BranchId,
                                DepositId = Data.AccountNumber,
                                TransNo = 0,
                                ContraNo = 10300,
                                Amount = int_charge,
                                Descr = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                                Type = "D",
                                AccountNo = 40500,
                                TraDt = DateFunctions.sysdate(db),
                                ValueDt = DateFunctions.sysdate(db),
                                TransId = transId,
                                VouchId = 0,
                            };
                            db.SdTrans.Add(prematureCharge);

                        }

                        var TranInterest = new SdTran
                        {
                            FirmId = Master.FirmId,
                            BranchId = (short)cacheDetails.BranchId,
                            DepositId = Data.AccountNumber,
                            TransNo = 0,
                            ContraNo = 25002,
                            Amount = amount,
                            Descr = "INT SD INTEREST UPTO" + "-" + DateFunctions.sysdate(db),
                            Type = "C",
                            AccountNo = 40500,
                            TraDt = DateFunctions.sysdate(db),
                            ValueDt = DateFunctions.sysdate(db),
                            TransId = transId,
                            VouchId = 0,
                        };
                        //  db.SdTrans.Add(TranInterest);
                        // db.SaveChanges();
                        var RoundDifference = new SdTran
                        {
                            FirmId = Master.FirmId,
                            BranchId = (short)cacheDetails.BranchId,
                            DepositId = Data.AccountNumber,
                            TransNo = 0,
                            ContraNo = 25002,
                            Amount = round < 0 ? round * -1 : round,
                            Descr = "ROUNDING DIFFERENCE ADJUSTED" + "-" + Data.AccountNumber,
                            Type = round < 0 ? "D" : "C",
                            AccountNo = 40500,
                            TraDt = DateFunctions.sysdate(db),
                            ValueDt = DateFunctions.sysdate(db),
                            TransId = transId,
                            VouchId = 0,
                        };
                        //  db.SdTrans.Add(RoundDifference);
                        // db.SaveChanges();
                        var settleDetail = new SdTran
                        {
                            FirmId = Master.FirmId,
                            BranchId = (short)cacheDetails.BranchId,
                            DepositId = Data.AccountNumber,
                            TransNo = 0,
                            ContraNo = 33000,
                            Amount = settle,//Math.Round(Master.DepositAmt + amount),
                            Descr = "By CASH-SD SETTLEMENT" + "-" + Data.AccountNumber,
                            Type = "D",
                            AccountNo = 40500,
                            TraDt = DateFunctions.sysdate(db),
                            ValueDt = DateFunctions.sysdate(db),
                            TransId = transId,
                            VouchId = 0,
                        };

                        db.SdTrans.Add(TranInterest);
                        db.SdTrans.Add(RoundDifference);
                        db.SdTrans.Add(settleDetail);

                        //db.SaveChanges();

                        Master.Balance = settle;//Math.Round(Master.DepositAmt + amount);
                        Master.DepositAmt = 0;
                        Master.StatusId = 0;


                        Master.TrancationDate = DateFunctions.sysdate(db);
                        Master.CloseDate = DateFunctions.sysdate(db);

                        db.SaveChanges();

                        string _clientID = Guid.NewGuid().ToString();

                        // Console.WriteLine("Client Ready");

                        SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");

                        _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                        _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                        _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                        _subLedgerRequest.Header.Subject = "Settlement"; //Subject;
                        _subLedgerRequest.DocumentDetail.FirmId = Master.FirmId; //FirmId;
                        _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                        _subLedgerRequest.DocumentDetail.ModuleId = 4;
                        _subLedgerRequest.DocumentDetail.DocumentId = Master.DepositId;
                        _subLedgerRequest.DocumentDetail.CustomerId = Master.CustId;
                        _subLedgerRequest.DocumentDetail.CustomerName = Master.CustName;
                        _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;

                        //_subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = BankBranch;
                        _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                        _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + settleDetail.Descr;
                        if (Todate < interval)
                        {
                            var accountEntryPremature = new AcountingEntry
                            {
                                ModuleId = 1, //(short)cacheDetails.ModuleId,
                                AccountNo = 40500,//33000
                                Type = EntryTypes.D,
                                Amount = int_charge,
                                TransactionMode = TransactionModes.CS,
                                Description = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                                ContraNo = 10300,//40500
                                SegmentId = 1,
                                BranchId = Master.BranchId,
                                ReferenceId = Master.DepositId,

                            };
                            var accountEntryPremature1 = new AcountingEntry
                            {
                                ModuleId = 1, //(short)cacheDetails.ModuleId,
                                AccountNo = 10300,//33000
                                Type = EntryTypes.C,
                                Amount = int_charge,
                                TransactionMode = TransactionModes.CS,
                                Description = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                BranchId = Master.BranchId,
                                ReferenceId = Master.DepositId,

                            };
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntryPremature);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntryPremature1);

                        }


                        var accountEntry1 = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 25002,//33000
                            Type = EntryTypes.D,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.CS,
                            Description = TranInterest.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        var accountEntry2 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.C,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.CS,
                            Description = TranInterest.Descr,
                            ContraNo = 25002,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };

                        var accountEntry3 = new AcountingEntry
                        {
                            ModuleId = 1,//(short)cacheDetails.ModuleId,
                            AccountNo = 25002,//33000
                            Type = round < 0 ? EntryTypes.C : EntryTypes.D,//round < 0 ? "D" : "C",
                            Amount = RoundDifference.Amount,
                            TransactionMode = TransactionModes.CS,
                            Description = RoundDifference.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };


                        var accountEntry4 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = round < 0 ? EntryTypes.D : EntryTypes.C,
                            Amount = RoundDifference.Amount,
                            TransactionMode = TransactionModes.CS,
                            Description = RoundDifference.Descr,
                            ContraNo = 25002,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };





                        var accountEntry5 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,
                            SubAccountNo = 0,
                            Type = EntryTypes.D,
                            Amount = settleDetail.Amount,
                            TransactionMode = TransactionModes.CS,
                            Description = settleDetail.Descr,
                            ContraNo = 31003,
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = settleDetail.DepositId,
                        };

                        if (cacheDetails.BranchId == Master.BranchId)
                        {

                            var accountEntry6 = new AcountingEntry
                            {
                                ModuleId = 1,//(short)cacheDetails.ModuleId,
                                AccountNo = 33000,//33000
                                Type = EntryTypes.C,
                                Amount = settleDetail.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = settleDetail.Descr,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                BranchId = Master.BranchId,
                                ReferenceId = Master.DepositId,
                            };
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                        }
                        else// if (cacheDetails.BranchId != Master.BranchId)
                        {
                            var accountEntry6 = new AcountingEntry
                            {
                                ModuleId = 1,//(short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                Type = EntryTypes.C,
                                Amount = settleDetail.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = settleDetail.Descr,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                BranchId = Master.BranchId,
                                ReferenceId = Master.DepositId,
                            };
                            var accountEntry7 = new AcountingEntry
                            {
                                ModuleId = 1,//(short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                Type = EntryTypes.D,
                                Amount = settleDetail.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = settleDetail.Descr,
                                ContraNo = 33000,//40500
                                SegmentId = 1,
                                BranchId = (short)cacheDetails.BranchId,
                                ReferenceId = Master.DepositId,
                            };

                            var accountEntry8 = new AcountingEntry
                            {
                                ModuleId = 1,//(short)cacheDetails.ModuleId,
                                AccountNo = 33000,//33000
                                Type = EntryTypes.C,
                                Amount = settleDetail.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = settleDetail.Descr,
                                ContraNo = 31003,//40500
                                SegmentId = 1,
                                BranchId = (short)cacheDetails.BranchId,
                                ReferenceId = Master.DepositId,
                            };
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry7);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry8);

                        }

                        _subLedgerRequest.AccountingVoucher.Amount = settleDetail.Amount;
                        _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                        _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                        _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                        _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CASH;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                        //_subLedgerRequest.AccountingVoucher.
                        string transactionMessage = _subLedgerRequest.ToString();

                        _subLedgerClient.PostMessage(transactionMessage);
                        string phone = GetPhoneNumber.getphone(Master.CustId, db);

                        if (phone != "1111111111")
                        {
                            Message.Message sms = new Message.Message();

                            sms.SendSms("Settlement", (byte)Master.FirmId, Master.BranchId, Master.DepositId, 4, Master.CustId, "MABDEP", phone, String.Format("Dear Member, Your SD  A/c {0} is settled for Rs {1} on {2}. Thanks for your valuable support. Aval Bal is INR 0 - MABEN NIDHI LTD", Data.AccountNumber, settleDetail.Amount, DateFunctions.sysdate(db)), db);
                        }

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                    }

                }

                else if (Data.Transactiontype.ToUpper() == "CHEQUE")
                {
                    SequenceGenerator dep = new SequenceGenerator();
                    var transId = (int)dep.getSequence(db, Master.FirmId, (short)cacheDetails.BranchId, 4, 4);
                    DateTime Todate = DateFunctions.sysdate(db);

                    InterestCalculator intr = new InterestCalculator();
                    string todate = Todate.ToString("yyyy-MM-dd");
                    Decimal amount = intr.getInterest(db, Data.AccountNumber, Master.CloseDate.ToString("yyyy-MM-dd"), todate);
                    //  var SettledAmount = db.SdMasters.Where(x => x.DepositId == AccountNumber && x.CustId == CustomerId ).Select(x => x.DepositAmt).SingleOrDefault();
                    var settle = Math.Round(Master.DepositAmt + amount);
                    var Notround = Master.DepositAmt + amount;
                    var round = settle - Notround;
                    decimal validate = db.GeneralParameters.Where(x => x.ParmtrId == 19 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();

                    decimal int_charge = db.GeneralParameters.Where(x => x.ParmtrId == 15 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();
                    var frm_date = db.SdMasters.Where(x => x.DepositId == Data.AccountNumber).Select(x => x.DepositDate).SingleOrDefault();
                    var interval = frm_date.AddMonths(6);
                    if (Todate < interval)
                    {
                        settle = settle - int_charge;
                    }



                    if (Todate < interval)
                    {
                        var prematureCharge = new SdTran
                        {
                            FirmId = Master.FirmId,
                            BranchId = (short)cacheDetails.BranchId,
                            DepositId = Data.AccountNumber,
                            TransNo = 0,
                            ContraNo = 10300,
                            Amount = int_charge,
                            Descr = /*Data.ChequeNo + "-" +*/ "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                            Type = "D",
                            AccountNo = 40500,
                            TraDt = DateFunctions.sysdate(db),
                            ValueDt = DateFunctions.sysdate(db),
                            TransId = transId,
                            VouchId = 0,
                        };
                        db.SdTrans.Add(prematureCharge);

                    }

                    var TranInterest = new SdTran
                    {
                        FirmId = Master.FirmId,
                        BranchId = (short)cacheDetails.BranchId,
                        DepositId = Data.AccountNumber,
                        TransNo = 0,
                        ContraNo = 25002,
                        Amount = amount,
                        Descr = /*Data.ChequeNo + "-" +*/ "INT SD INTEREST UPTO" + "-" + DateFunctions.sysdate(db).ToString("dd-MM-yyyy"),
                        Type = "C",
                        AccountNo = 40500,
                        TraDt = DateFunctions.sysdate(db),
                        ValueDt = DateFunctions.sysdate(db),
                        TransId = transId,
                        VouchId = 0,
                    };
                    //  db.SdTrans.Add(TranInterest);
                    // db.SaveChanges();
                    var RoundDifference = new SdTran
                    {
                        FirmId = Master.FirmId,
                        BranchId = (short)cacheDetails.BranchId,
                        DepositId = Data.AccountNumber,
                        TransNo = 0,
                        ContraNo = 25002,
                        Amount = round < 0 ? round * -1 : round,
                        Descr = /*Data.ChequeNo + "-" +*/ "ROUNDING DIFFERENCE ADJUSTED" + "-" + Data.AccountNumber,
                        Type = round < 0 ? "D" : "C",
                        AccountNo = 40500,
                        TraDt = DateFunctions.sysdate(db),
                        ValueDt = DateFunctions.sysdate(db),
                        TransId = transId,
                        VouchId = 0,
                    };
                    //  db.SdTrans.Add(RoundDifference);
                    // db.SaveChanges();
                    var settleDetail = new SdTran
                    {
                        FirmId = Master.FirmId,
                        BranchId = (short)cacheDetails.BranchId,
                        DepositId = Data.AccountNumber,
                        TransNo = 0,
                        ContraNo = 32100,
                        Amount = settle,
                        Descr = "BY CHQ" + "-" + cacheDetails.BranchId + "-" + Data.ChequeNo + "-" + "SD SETTLEMENT" + "-" + Data.AccountNumber,
                        Type = "D",
                        AccountNo = 40500,
                        TraDt = DateFunctions.sysdate(db),
                        ValueDt = DateFunctions.sysdate(db),
                        TransId = transId,
                        VouchId = 0,
                    };

                    db.SdTrans.Add(TranInterest);
                    db.SdTrans.Add(RoundDifference);
                    db.SdTrans.Add(settleDetail);

                    //db.SaveChanges();
                    if (Todate < interval)
                    {
                        Master.Balance = settle;
                        Master.DepositAmt = 0;
                        Master.StatusId = 0;
                    }
                    else
                    {
                        Master.Balance = Math.Round(Master.DepositAmt + amount);
                        Master.DepositAmt = 0;
                        Master.StatusId = 0;
                    }

                    Master.TrancationDate = DateFunctions.sysdate(db);
                    Master.CloseDate = DateFunctions.sysdate(db);

                    db.SaveChanges();

                    string _clientID = Guid.NewGuid().ToString();

                    // Console.WriteLine("Client Ready");

                    SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");

                    _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                    _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                    _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                    _subLedgerRequest.Header.Subject = "Settlement"; //Subject;
                    _subLedgerRequest.DocumentDetail.FirmId = Master.FirmId; //FirmId;
                    _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                    _subLedgerRequest.DocumentDetail.ModuleId = 4;
                    _subLedgerRequest.DocumentDetail.DocumentId = Master.DepositId;
                    _subLedgerRequest.DocumentDetail.CustomerId = Master.CustId;
                    _subLedgerRequest.DocumentDetail.CustomerName = Master.CustName;
                    _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                    // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate = DateTime.UtcNow;//ChequeDate;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.CustomerBank = Data.CustomerBank;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = Data.ChequeNo;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = (int)Data.BranchBankid;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = (short)Data.SubsidiaryBankAccountno;
                    _subLedgerRequest.AccountingVoucher.CurrencyId = 1;

                    _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + settleDetail.Descr;
                    if (Todate < interval)
                    {
                        var accountEntryPremature = new AcountingEntry
                        {
                            ModuleId = 4, //(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.D,
                            SubAccountNo = 0,
                            Amount = int_charge,
                            TransactionMode = TransactionModes.CH,
                            Description = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                            ContraNo = 10300,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        var accountEntryPremature1 = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 10300,//33000
                            Type = EntryTypes.C,
                            SubAccountNo = 0,
                            Amount = int_charge,
                            TransactionMode = TransactionModes.CH,
                            Description = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntryPremature);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntryPremature1);

                    }

                    if (Master.BranchId == Data.BranchBankid)
                    {
                        var accountEntry1 = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 25002,//33000
                            Type = EntryTypes.D,
                            SubAccountNo = 0,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = TranInterest.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        var accountEntry2 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.C,
                            SubAccountNo = 0,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = TranInterest.Descr,
                            ContraNo = 25002,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };

                        var accountEntry3 = new AcountingEntry
                        {
                            ModuleId = 1,//(short)cacheDetails.ModuleId,
                            AccountNo = 25002,//33000
                            Type = round < 0 ? EntryTypes.C : EntryTypes.D,
                            SubAccountNo = 0,
                            Amount = RoundDifference.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = RoundDifference.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };


                        var accountEntry4 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = round < 0 ? EntryTypes.D : EntryTypes.C,
                            SubAccountNo = 0,
                            Amount = RoundDifference.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = RoundDifference.Descr,
                            ContraNo = 25002,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };

                        var accountEntry5 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,
                            SubAccountNo = 0,
                            Type = EntryTypes.D,
                            Amount = settleDetail.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = settleDetail.Descr,
                            ContraNo = 32100,
                            SegmentId = 1,
                            BranchId = settleDetail.BranchId,
                            ReferenceId = settleDetail.DepositId,
                        };



                        var accountEntry6 = new AcountingEntry
                        {
                            ModuleId = 1,//(short)cacheDetails.ModuleId,
                            AccountNo = 32100,
                            SubAccountNo = (int)Data.SubsidiaryBankAccountno,
                            Type = EntryTypes.C,
                            Amount = settleDetail.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = settleDetail.Descr,
                            ContraNo = 40500,
                            SegmentId = 1,
                            BranchId = settleDetail.BranchId,
                            ReferenceId = settleDetail.DepositId,
                        };


                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);

                    }
                    else// if (cacheDetails.BranchId != Master.BranchId)
                    {


                        var accountEntry1 = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 25002,//33000
                            Type = EntryTypes.D,
                            SubAccountNo = 0,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = TranInterest.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        var accountEntry2 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.C,
                            SubAccountNo = 0,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = TranInterest.Descr,
                            ContraNo = 25002,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };

                        var accountEntry3 = new AcountingEntry
                        {
                            ModuleId = 1,//(short)cacheDetails.ModuleId,
                            AccountNo = 25002,//33000
                            Type = round < 0 ? EntryTypes.C : EntryTypes.D,
                            SubAccountNo = 0,
                            Amount = RoundDifference.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = RoundDifference.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };


                        var accountEntry4 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = round < 0 ? EntryTypes.D : EntryTypes.C,
                            SubAccountNo = 0,
                            Amount = RoundDifference.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = RoundDifference.Descr,
                            ContraNo = 25002,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };





                        var accountEntry5 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,
                            SubAccountNo = 0,
                            Type = EntryTypes.D,
                            Amount = settleDetail.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = settleDetail.Descr,
                            ContraNo = 31003,
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = settleDetail.DepositId,
                        };



                        var accountEntry7 = new AcountingEntry
                        {
                            ModuleId = 1,//(short)cacheDetails.ModuleId,
                            AccountNo = 31003,//33000
                            Type = EntryTypes.C,
                            SubAccountNo = 0,
                            Amount = settleDetail.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = settleDetail.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };
                        var accountEntry8 = new AcountingEntry
                        {
                            ModuleId = 1,//(short)cacheDetails.ModuleId,
                            AccountNo = 31003,//33000
                            Type = EntryTypes.D,
                            SubAccountNo = 0,
                            Amount = settleDetail.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = settleDetail.Descr,
                            ContraNo = 32100,//40500
                            SegmentId = 1,
                            BranchId = (short)Data.BranchBankid,
                            ReferenceId = Master.DepositId,
                        };

                        var accountEntry9 = new AcountingEntry
                        {
                            ModuleId = 1,//(short)cacheDetails.ModuleId,
                            AccountNo = 32100,//33000
                            Type = EntryTypes.C,
                            SubAccountNo = (int)Data.SubsidiaryBankAccountno,
                            Amount = settleDetail.Amount,
                            TransactionMode = TransactionModes.CH,
                            Description = settleDetail.Descr,
                            ContraNo = 31003,//40500
                            SegmentId = 1,
                            BranchId = (short)Data.BranchBankid,
                            ReferenceId = Master.DepositId,
                        };
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry7);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry8);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry9);

                    }

                    _subLedgerRequest.AccountingVoucher.Amount = settle;
                    _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                    _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                    _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                    _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CHEQUE;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                      //_subLedgerRequest.AccountingVoucher.
                    string transactionMessage = _subLedgerRequest.ToString();

                    _subLedgerClient.PostMessage(transactionMessage);
                    Message.Message sms = new Message.Message();
                    string phone = GetPhoneNumber.getphone(Master.CustId, db);
                    if (phone != "1111111111")
                    {
                        sms.SendSms("Settlement", (byte)Master.FirmId, Master.BranchId, Master.DepositId, 4, Master.CustId, "MABDEP", phone, String.Format("Dear Member, Your SD  A/c {0} is settled for Rs {1} on {2}. Thanks for your valuable support. Aval Bal is INR 0 - MABEN NIDHI LTD", Data.AccountNumber, Master.Balance, DateFunctions.sysdate(db)), db);

                    }
                   // return Results.Ok(new { status = "success" });

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;

                }
                else
                {

                    var bankCommission = db.PaymentgatewayMasters.FirstOrDefault(x => x.PaymentType.ToUpper() == "BANK");
                    if (bankCommission.PaymentgatewayCommission == null)
                        bankCommission.PaymentgatewayCommission = 0;
                    SequenceGenerator dep = new SequenceGenerator();
                    var transId = (int)dep.getSequence(db, Master.FirmId, (short)cacheDetails.BranchId, 4, 4);
                    DateTime Todate = DateFunctions.sysdate(db);

                    InterestCalculator intr = new InterestCalculator();
                    string todate = Todate.ToString("yyyy-MM-dd");
                    Decimal amount = intr.getInterest(db, Data.AccountNumber, Master.CloseDate.ToString("yyyy-MM-dd"), todate);
                    //  var SettledAmount = db.SdMasters.Where(x => x.DepositId == AccountNumber && x.CustId == CustomerId ).Select(x => x.DepositAmt).SingleOrDefault();
                    var settle = Math.Round(Master.DepositAmt + amount);
                    var Notround = Master.DepositAmt + amount;
                    var round = settle - Notround;

                    var tran = (from master in db.SdMasters
                                join checkBranchId in db.SdMasters on master.BranchId equals checkBranchId.BranchId
                                join depositids in db.SdMasters on checkBranchId.DepositId equals depositids.DepositId
                                join transaction in db.SdTrans on depositids.DepositId equals transaction.DepositId
                                where master.DepositId == Data.AccountNumber && DateTime.Parse(transaction.TraDt.Value.ToShortDateString())== DateFunctions.sysdate(db).Date && transaction.Type == "C" && transaction.ContraNo == 33000

                                select transaction.Amount
                                ).Sum();
                    decimal validate = db.GeneralParameters.Where(x => x.ParmtrId == 19 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();

                    decimal int_charge = db.GeneralParameters.Where(x => x.ParmtrId == 15 && x.ModuleId == 4).Select(x => Convert.ToDecimal(x.ParmtrValue)).SingleOrDefault();
                    var frm_date = db.SdMasters.Where(x => x.DepositId == Data.AccountNumber).Select(x => x.DepositDate).SingleOrDefault();
                    var interval = frm_date.AddMonths(6);
                    decimal bankCharges = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9;
                    if (Todate < interval)
                    {
                        settle = settle - int_charge;
                    }
                    settle = settle - bankCharges;

                    if (Todate < interval)
                    {
                        var prematureCharge = new SdTran
                        {
                            FirmId = Master.FirmId,
                            BranchId = (short)cacheDetails.BranchId,
                            DepositId = Data.AccountNumber,
                            TransNo = 0,
                            ContraNo = 10300,
                            Amount = int_charge,
                            Descr = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                            Type = "D",
                            AccountNo = 40500,
                            TraDt = DateFunctions.sysdate(db),
                            ValueDt = DateFunctions.sysdate(db),
                            TransId = transId,
                            VouchId = 0,
                        };
                        db.SdTrans.Add(prematureCharge);

                    }

                    // var transId = (int)dep.getSequence(db, Master.FirmId, Master.BranchId, 4, 4);

                    var TranInterest = new SdTran
                    {
                        FirmId = Master.FirmId,
                        BranchId = (short)cacheDetails.BranchId,
                        DepositId = Data.AccountNumber,
                        TransNo = 0,
                        ContraNo = 25002,
                        Amount = amount,
                        Descr = "INT SD INTEREST UPTO" + "-" + DateFunctions.sysdate(db),
                        Type = "C",
                        AccountNo = 40500,
                        TraDt = DateFunctions.sysdate(db),
                        ValueDt = DateFunctions.sysdate(db),
                        TransId = transId,
                        VouchId = 0,
                    };
                    //  db.SdTrans.Add(TranInterest);
                    // db.SaveChanges();
                    var RoundDifference = new SdTran
                    {
                        FirmId = Master.FirmId,
                        BranchId = (short)cacheDetails.BranchId,
                        DepositId = Data.AccountNumber,
                        TransNo = 0,
                        ContraNo = 25002,
                        Amount = round < 0 ? round * -1 : round,
                        Descr = "ROUNDING DIFFERENCE ADJUSTED" + "-" + Data.AccountNumber,
                        Type = round < 0 ? "D" : "C",
                        AccountNo = 40500,
                        TraDt = DateFunctions.sysdate(db),
                        ValueDt = DateFunctions.sysdate(db),
                        TransId = transId,
                        VouchId = 0,
                    };
                    //  db.SdTrans.Add(RoundDifference);
                    // db.SaveChanges();
                    var settleDetail = new SdTran
                    {
                        FirmId = Master.FirmId,
                        BranchId = (short)cacheDetails.BranchId,
                        DepositId = Data.AccountNumber,
                        TransNo = 0,
                        ContraNo = 33000,
                        // Amount = Math.Round(Master.DepositAmt + amount),
                        Amount = settle,
                        Descr = " ONLINE SD SETTTLEMENT - " + Data.AccountNumber, //"BY ACC TRANSFER" + "-" + Data.AccountNumber,
                        Type = "D",
                        AccountNo = 40500,
                        TraDt = DateFunctions.sysdate(db),
                        ValueDt = DateFunctions.sysdate(db),
                        TransId = transId,
                        VouchId = 0,
                    };
                    var Commssion = new SdTran
                    {
                        FirmId = Master.FirmId,
                        BranchId = (short)cacheDetails.BranchId,
                        DepositId = Data.AccountNumber,
                        TransNo = 0,
                        ContraNo = 33000,
                        Amount = bankCharges,
                        Descr = "ONLINE SD SETTLEMENT CHARGE - " + Data.AccountNumber,
                        Type = "D",
                        AccountNo = 40500,
                        TraDt = DateFunctions.sysdate(db),
                        ValueDt = DateFunctions.sysdate(db),
                        TransId = transId,
                        VouchId = 0,
                    };

                    db.SdTrans.Add(TranInterest);
                    db.SdTrans.Add(Commssion);
                    db.SdTrans.Add(RoundDifference);
                    db.SdTrans.Add(settleDetail);
                    //db.SaveChanges();

                    Master.Balance = settle;//Math.Round(Master.DepositAmt + amount);
                    Master.DepositAmt = 0;
                    Master.StatusId = 0;


                    Master.TrancationDate = DateFunctions.sysdate(db);
                    Master.CloseDate = DateFunctions.sysdate(db);

                    db.SaveChanges();

                    string _clientID = Guid.NewGuid().ToString();

                    // Console.WriteLine("Client Ready");

                    SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");

                    _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                    _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                    _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                    _subLedgerRequest.Header.Subject = "Settlement"; //Subject;
                    _subLedgerRequest.DocumentDetail.FirmId = Master.FirmId; //FirmId;
                    _subLedgerRequest.DocumentDetail.BranchId = Master.BranchId;
                    _subLedgerRequest.DocumentDetail.ModuleId = 4;
                    _subLedgerRequest.DocumentDetail.DocumentId = Master.DepositId;
                    _subLedgerRequest.DocumentDetail.CustomerId = Master.CustId;
                    _subLedgerRequest.DocumentDetail.CustomerName = Master.CustName;
                    _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;

                    _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                    _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + settleDetail.Descr;
                    if (Todate < interval)
                    {
                        var accountEntryPremature = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.D,
                            Amount = int_charge,
                            TransactionMode = TransactionModes.TR,
                            Description = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                            ContraNo = 10300,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        var accountEntryPremature1 = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 10300,//33000
                            Type = EntryTypes.C,
                            Amount = int_charge,
                            TransactionMode = TransactionModes.TR,
                            Description = "SD SETTLEMENT LESS CHRGE" + " " + Data.AccountNumber,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntryPremature);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntryPremature1);

                    }

                    if (TranInterest.Amount > 0)
                    {
                        var accountEntry1 = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 25002,//33000
                            Type = EntryTypes.D,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = TranInterest.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,

                        };
                        var accountEntry2 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.C,
                            Amount = TranInterest.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = TranInterest.Descr,
                            ContraNo = 25002,//40500
                            SegmentId = 1,
                            BranchId = Master.BranchId,
                            ReferenceId = Master.DepositId,
                        };
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);

                    }

                    var accountEntry3 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = 25002,//33000
                        Type = round < 0 ? EntryTypes.C : EntryTypes.D,
                        Amount = RoundDifference.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = RoundDifference.Descr,
                        ContraNo = 40500,//40500
                        SegmentId = 1,
                        BranchId = Master.BranchId,
                        ReferenceId = Master.DepositId,
                    };


                    var accountEntry4 = new AcountingEntry
                    {
                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                        AccountNo = 40500,//33000
                        Type = round < 0 ? EntryTypes.D : EntryTypes.C,
                        Amount = RoundDifference.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = RoundDifference.Descr,
                        ContraNo = 25002,//40500
                        SegmentId = 1,
                        BranchId = Master.BranchId,
                        ReferenceId = Master.DepositId,
                    };

                    var accountEntry5 = new AcountingEntry
                    {
                        ModuleId = 4,// (short)cacheDetails.ModuleId,
                        AccountNo = 40500,//33000
                        SubAccountNo = 0,
                        Type = EntryTypes.D,
                        Amount = bankCharges,
                        TransactionMode = TransactionModes.TR,
                        Description = settleDetail.Descr,
                        ContraNo = 31003,//40500
                        SegmentId = 1,
                        //branchid must insert
                        BranchId = Master.BranchId,
                        ReferenceId = settleDetail.DepositId,
                    };


                    var accountEntry6 = new AcountingEntry
                    {
                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                        AccountNo = 31003,//33000
                        SubAccountNo = 0,
                        Type = EntryTypes.C,
                        Amount = bankCharges,
                        TransactionMode = TransactionModes.TR,
                        Description = Commssion.Descr,
                        ContraNo = 40500,
                        SegmentId = 1,
                        BranchId = Master.BranchId,
                        ReferenceId = settleDetail.DepositId,
                    };


                    var accountEntry7 = new AcountingEntry
                    {
                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                        AccountNo = 40500,
                        SubAccountNo = 0,
                        Type = EntryTypes.D,
                        Amount = settleDetail.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = settleDetail.Descr,
                        ContraNo = 31003,
                        SegmentId = 1,
                        BranchId = Master.BranchId,
                        ReferenceId = settleDetail.DepositId,
                    };
                    var accountEntry8 = new AcountingEntry
                    {
                        ModuleId = 1,// (short)cacheDetails.ModuleId,
                        AccountNo = 31003,
                        SubAccountNo = 0,
                        Type = EntryTypes.C,
                        Amount = settleDetail.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = settleDetail.Descr,
                        ContraNo = 40500,
                        SegmentId = 1,
                        BranchId = Master.BranchId,
                        ReferenceId = settleDetail.DepositId,
                    };

                    var accountEntry9 = new AcountingEntry
                    {
                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                        AccountNo = 31003,//33000
                        SubAccountNo = 0,
                        Type = EntryTypes.D,
                        Amount = bankCharges,
                        TransactionMode = TransactionModes.TR,
                        Description = Commssion.Descr,
                        ContraNo = 40602,
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = settleDetail.DepositId,
                    };
                    var accountEntry10 = new AcountingEntry
                    {
                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                        AccountNo = 40602,//33000
                        SubAccountNo = 0,
                        Type = EntryTypes.C,
                        Amount = bankCharges,
                        TransactionMode = TransactionModes.TR,
                        Description = Commssion.Descr,
                        ContraNo = 31003,
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = settleDetail.DepositId,
                    };

                    var accountEntry11 = new AcountingEntry
                    {
                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                        AccountNo = 31003,//33000
                        SubAccountNo = 0,
                        Type = EntryTypes.D,
                        Amount = settleDetail.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = settleDetail.Descr,
                        ContraNo = 40602,
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = settleDetail.DepositId,
                    };
                    var accountEntry12 = new AcountingEntry
                    {
                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                        AccountNo = 40602,//33000
                        SubAccountNo = 0,
                        Type = EntryTypes.C,
                        Amount = settleDetail.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = settleDetail.Descr,
                        ContraNo = 31003,
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = settleDetail.DepositId,
                    };
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry7);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry8);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry9);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry10);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry11);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry12);



                    _subLedgerRequest.AccountingVoucher.Amount = settleDetail.Amount;
                    _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                    _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                    _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                    _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.Bank;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                    //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                    //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                    //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                    //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                    //_subLedgerRequest.AccountingVoucher.
                    string transactionMessage = _subLedgerRequest.ToString();

                    _subLedgerClient.PostMessage(transactionMessage);
                    Message.Message sms = new Message.Message();
                    string phone = GetPhoneNumber.getphone(Master.CustId, db);
                    if (phone != "1111111111")
                    {
                        sms.SendSms("Settlement", (byte)Master.FirmId, Master.BranchId, Master.DepositId, 4, Master.CustId, "MABDEP", phone, String.Format("Dear Member, Your SD  A/c {0} is settled for Rs {1} on {2}. Thanks for your valuable support. Aval Bal is INR 0 - MABEN NIDHI LTD", Data.AccountNumber, Master.Balance, DateFunctions.sysdate(db)), db);
                    }

                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                   // return Results.Ok(new { status = "success" });
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
                return _Response;
               // return Results.NotFound(message);
            }
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();
            if (Data.CustomerId == null)
            {
                FailedValidations.Add(new ApplicationException("Customer id not found"));
            }

            return FailedValidations;
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<SettlementData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
    }
}
