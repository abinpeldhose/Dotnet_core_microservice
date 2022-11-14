using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using TokenManager;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;
using customerSdmodule.Redis;
using static RedisCacheDemo.RedisCacheStore;
using MACOM.Integrations;
using MACOM.Contracts;
using MACOM.Contracts.Model;
using Serilog;
using Microsoft.EntityFrameworkCore;

namespace customerSdmodule.ModelClass.BHApprove
{
    public class BHApproveAPI : BaseApi
    {
        //private string _jwtToken;
        private BHApproveData _chequereconcilation;
        string _clientID = Guid.NewGuid().ToString();

        // Console.WriteLine("Client Ready");

        SublegerClient _subLedgerClient = new SublegerClient(Guid.NewGuid().ToString(), "10.192.5.44:19092");
        SubledgerRequest _subLedgerRequest = new SubledgerRequest();


        //   private SdMaster _master;

        public BHApproveData Data { get => _chequereconcilation; set => _chequereconcilation = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        //public SdMaster Master { get => _master; set => _master = value; }

        public ResponseData Get(ModelContext db)
        {
            return BhApprove(db);
        }

        private ResponseData BhApprove(ModelContext db)
        {
            using (var transaction = db.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
            {
                try
                {
                    string uniqueKey = TokenManagement.Extract(JwtToken);
                    var cache = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                    int n = 9;
                    string depositId = _chequereconcilation.DepositId;
                    depositId.Substring(n);
                    String trimmed = depositId.Substring(n, depositId.Length - 9);

                    SequenceGenerator dep = new SequenceGenerator();


                    var transId = (int)dep.getSequence(db, (byte)cache.FirmId, (byte)cache.BranchId, 4, 4); ;
                    //  var chequeSequence = (int)dep.getSequence(db, firmid, branchid, 4, 5);
                    int depSequence = (int)dep.getSequence(db, (byte)cache.FirmId, (byte)cache.BranchId, 4, 1);
                    String padDepositid = depSequence.ToString();
                    String pdepositid = padDepositid.PadLeft(8, '0');

                    String padFirmid = cache.FirmId.ToString();//_chequereconcilation.FirmId.ToString();
                    String pfirmid = padFirmid.PadLeft(2, '0');

                    String padBranchid = cache.BranchId.ToString();
                    String pbranchid = padBranchid.PadLeft(4, '0');

                    String padModuleid = cache.ModuleId.ToString();
                    String pmoduleid = padModuleid.PadLeft(2, '0');

                    String depositid = pfirmid + pbranchid + pdepositid + pmoduleid;



                    var existitem = db.SdChequereconcilations.FirstOrDefault(x => x.DepositId == _chequereconcilation.DepositId && x.Chequeno == _chequereconcilation.ChequeNo && (x.StatusId == 1 || x.StatusId == 2));

                    if (existitem == null)
                    {
                        var result = new { status = "notFound" };
                        var JsonString = JsonSerializer.Serialize(result);
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;

                        _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                        Log.Information("not found");
                        return _Response;
                        //return Results.NotFound(new {status= "notFound" });
                    }
                    else
                    {
                        existitem.BhId = _chequereconcilation.BhId;
                        existitem.BhVerifyDate = DateFunctions.sysdate(db);//DateTime.Now;
                        existitem.StatusId = 3;

                        // db.SaveChanges();
                        var data5 = db.SdChequereconcilations.Where(x => x.DepositId == _chequereconcilation.DepositId && x.Chequeno == _chequereconcilation.ChequeNo).SingleOrDefault();
                        //  return Results.Ok(existitem);


                        //  var data = db.SdChequereconcilations.Where(x => x.DepositId == Depositid && x.Chequeno == chqNo).SingleOrDefault();


                        if (_chequereconcilation.DepositId.Length == 14)
                        {

                            var verificationDetails = db.SdVerifications.Where(x => x.VerifyId == _chequereconcilation.DepositId).SingleOrDefault();
                            var NomineeDetails = db.SdSubApplicants.Where(x => x.DocumentId == _chequereconcilation.DepositId).ToList();
                            if (verificationDetails == null)
                            {
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 404;


                                Log.Information("not found");
                                return _Response;
                                // return Results.NotFound(new { status = "There is no detail in verification details" });
                            }
                            else
                            {
                                verificationDetails.DepositId = depositid;
                                foreach (var nominee in NomineeDetails)
                                {
                                    nominee.DocumentId = depositid;
                                    db.SaveChanges();
                                }

                                db.SaveChanges();

                                //int a = 9;
                                //string V = depositid;
                                //V.Substring(n);
                                //String Newtrimmed = V.Substring(a, V.Length - 9);

                                var data1 = new SdTran
                                {
                                    FirmId = (byte)verificationDetails.FirmId,
                                    BranchId = (short)verificationDetails.BranchId,
                                    DepositId = verificationDetails.DepositId,
                                    TransNo = 0,
                                    Amount = (decimal)existitem.Amount,
                                    Descr = "BY CHQ RCVD - NEW SD - " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                    Type = "C",
                                    AccountNo = 40500,
                                    ContraNo = 32500,
                                    VouchId = 0,
                                    TraDt = DateTime.Parse(_chequereconcilation.ChequeClearDate),
                                    ValueDt = DateFunctions.sysdate(db),// DateTime.Now,
                                    TransId = transId,



                                };
                                var data3 = new SdMaster
                                {
                                    FirmId = verificationDetails.FirmId,
                                    BranchId = verificationDetails.BranchId,
                                    ModuleId = 4,
                                    DepositId = verificationDetails.DepositId,
                                    CustId = verificationDetails.CustId,
                                    CustName = verificationDetails.CustName,
                                    DepositType = verificationDetails.DepositType,
                                    DepositAmt = (decimal)verificationDetails.DepositAmt,
                                    IntRt = (decimal)verificationDetails.IntRt,
                                    DepositDate = DateTime.Parse(_chequereconcilation.ChequeClearDate),//DateTime.Now,
                                    TrancationDate = DateFunctions.sysdate(db),// DateTime.Now,
                                    CloseDate = DateFunctions.sysdate(db),//DateTime.Now,
                                    SchemeId = verificationDetails.SchemeId,
                                    Mobilizer = verificationDetails.Mobilizer,
                                    StatusId = 1,
                                    Lien = 0.ToString(),
                                    Nominee = 1,
                                    // TdsCode = true, 
                                    Minor = 0,
                                    IncentiveId = verificationDetails.IncentiveId,
                                    // Citizen = 0.ToString(), 
                                    Balance = verificationDetails.DepositAmt,

                                };

                                db.SdMasters.Add(data3);
                                db.SdTrans.Add(data1);

                                db.SaveChanges();
                                transaction.Commit();

                                string _clientID = Guid.NewGuid().ToString();

                                // Console.WriteLine("Client Ready");

                                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");

                                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                                _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                                _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                                _subLedgerRequest.Header.Subject = "New SD Cheque"; //Subject;
                                _subLedgerRequest.DocumentDetail.FirmId = verificationDetails.FirmId; //FirmId;
                                _subLedgerRequest.DocumentDetail.BranchId = verificationDetails.BranchId;
                                _subLedgerRequest.DocumentDetail.ModuleId = 4;
                                _subLedgerRequest.DocumentDetail.DocumentId = data1.DepositId;
                                _subLedgerRequest.DocumentDetail.CustomerId = verificationDetails.CustId;
                                _subLedgerRequest.DocumentDetail.CustomerName = verificationDetails.CustName;
                                _subLedgerRequest.DocumentDetail.UserId = cache.UserId;
                                // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                                // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate = (DateTime)data5.ChqSubmiteDate;//ChequeDate;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.CustomerBank = data5.CustomerBank;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = data5.Chequeno;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = (int)data5.BranchbankId;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.ClearanceDate = DateTime.UtcNow.Date;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = (short)data5.SubsidiarybankAccountno;
                                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                                _subLedgerRequest.AccountingVoucher.Narration = "BEING CHEQUE CLEARED - NEW SD,CHEQUE NO:-" + existitem.Chequeno + "AMOUNT IS " + existitem.Amount;
                                if (data5.BranchbankId == data3.BranchId)
                                {


                                    var accountEntry1 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 41241,
                                        SubAccountNo = 200004,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED-NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,//"CHQ RCVD-" + data1.DepositId,
                                        ContraNo = 32103,
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry2 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 32103,
                                        SubAccountNo = 100004,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED-NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 41241,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short) cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };

                                    var accountEntry3 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 32100,
                                        SubAccountNo = (int)data5.SubsidiarybankAccountno,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED-NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 40500,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry4 = new AcountingEntry
                                    {
                                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                                        AccountNo = 40500,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED-NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 32100,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };

                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);//.AccountingEntries.Add(account);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                                }
                                else
                                {
                                    var accountEntry1 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 41241,
                                        SubAccountNo = 200004,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED- NEW SD - " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 32103,
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry2 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 32103,
                                        SubAccountNo = 100004,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 41241,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };

                                    var accountEntry3 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 32100,
                                        SubAccountNo = (int)data5.SubsidiarybankAccountno,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 31003,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry4 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 31003,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -NEW SD - " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 32100,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };
                                    var accountEntry5 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 31003,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 40500,//40500
                                        SegmentId = 1,
                                        BranchId = data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };
                                    var accountEntry6 = new AcountingEntry
                                    {
                                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                                        AccountNo = 40500,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED-NEW SD- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 31003,//40500
                                        SegmentId = 1,
                                        BranchId = data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };

                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                                }



                                _subLedgerRequest.AccountingVoucher.Amount = (decimal)data5.Amount;
                                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CHEQUE;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                                  //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                                  //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                                  //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                                  //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                                  //_subLedgerRequest.AccountingVoucher.
                                string transactionMessage = _subLedgerRequest.ToString();



                                _subLedgerClient.PostMessage(transactionMessage);
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 200;
                                var Jsonstring = JsonSerializer.Serialize(existitem);
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                //_Response.data = JsonSerializer.Serialize(existitem);

                                Message.Message sms = new Message.Message();
                                //  sms.SendSms("New SD Account", data3.FirmId, data3.BranchId,data3.DepositId, 4, data3.CustId, "MABDEP", "8137047123", String.Format("Welcome to MABEN NIDHI LTD. Thanks for opening a SD account with us Aval Bal is INR  {0}", verificationDetails.DepositAmt));
                                string phone = GetPhoneNumber.getphone(data3.CustId, db);
                                if (phone != "1111111111")
                                {
                                    sms.SendSms("New SD Account", data3.FirmId, data3.BranchId, data3.DepositId, 4, data3.CustId, "MABDEP", phone, String.Format("Welcome to MABEN NIDHI LTD. Thanks for opening a SD account with us Aval Bal is INR  {0}", verificationDetails.DepositAmt), db);
                                }

                                return _Response;


                            }
                        }
                        else if (_chequereconcilation.DepositId.Length == 16)
                        {


                            var data1 = new SdTran
                            {
                                FirmId = (byte)data5.FirmId,
                                BranchId = (short)data5.BranchId,
                                DepositId = _chequereconcilation.DepositId,
                                TransNo = 0,
                                Amount = (decimal)data5.Amount,
                                Descr = "BY CHQ RCVD -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                Type = "C",
                                AccountNo = (int)data5.SubsidiarybankAccountno,
                                ContraNo = 32500,
                                TraDt = DateFunctions.sysdate(db),//DateTime.Now,
                                ValueDt = DateFunctions.sysdate(db),//DateTime.Now,
                                TransId = transId,

                            };
                            db.SdTrans.Add(data1);
                            var data3 = db.SdMasters.FirstOrDefault(x => x.DepositId == _chequereconcilation.DepositId);

                            if (data3 == null)
                            {
                                // return Results.NotFound();
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 404;


                                Log.Information("not found");
                                return _Response;
                            }
                            else
                            {
                                data3.DepositAmt = (decimal)(data3.DepositAmt + data5.Amount);
                                data3.TrancationDate = DateFunctions.sysdate(db);//DateTime.Now;
                                                                                 //db.SdMasters.Add(data3);
                                db.SaveChanges();
                                transaction.Commit();

                                string _clientID = Guid.NewGuid().ToString();

                                // Console.WriteLine("Client Ready");

                                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");

                                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                                _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                                _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                                _subLedgerRequest.Header.Subject = "cheque approve"; //Subject;
                                _subLedgerRequest.DocumentDetail.FirmId = (short)data3.FirmId; //FirmId;
                                _subLedgerRequest.DocumentDetail.BranchId = (short)data3.BranchId;
                                _subLedgerRequest.DocumentDetail.ModuleId = 4;
                                _subLedgerRequest.DocumentDetail.DocumentId = data1.DepositId;
                                _subLedgerRequest.DocumentDetail.CustomerId = (string)data3.CustId;
                                _subLedgerRequest.DocumentDetail.CustomerName = (string)data3.CustName;
                                _subLedgerRequest.DocumentDetail.UserId = cache.UserId;
                                // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                                // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate = (DateTime)data5.ChqSubmiteDate;//ChequeDate;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.CustomerBank = data5.CustomerBank;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = data5.Chequeno;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = (short)data5.BranchbankId;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = (short)data5.SubsidiarybankAccountno;
                                _subLedgerRequest.AccountingVoucher.ChequeDetail.ClearanceDate = DateTime.UtcNow.Date;
                                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                                _subLedgerRequest.AccountingVoucher.Narration = "BEING CHEQUE CLEARED-SD DEPOSIT-,CHEQUE NO:-" + data5.Chequeno + "AMOUNT IS " + data5.Amount;
                                if (data5.BranchbankId == data3.BranchId)
                                {


                                    var accountEntry1 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 41241,
                                        SubAccountNo = 200004,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 32103,
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//(short)cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry2 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 32103,
                                        SubAccountNo = 100004,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 41241,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//(short) cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };

                                    var accountEntry3 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 32100,
                                        SubAccountNo = (int)data5.SubsidiarybankAccountno,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 40500,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry4 = new AcountingEntry
                                    {
                                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                                        AccountNo = 40500,//33000
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 32100,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };

                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);//.AccountingEntries.Add(account);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                                }
                                else
                                {
                                    var accountEntry1 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 41241,
                                        SubAccountNo = 200004,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 32103,
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//(short)cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry2 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 32103,
                                        SubAccountNo = 100004,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 41241,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//(short)cache.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };

                                    var accountEntry3 = new AcountingEntry
                                    {
                                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                                        AccountNo = 32100,
                                        SubAccountNo = (int)data5.SubsidiarybankAccountno,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 31003,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,

                                    };
                                    var accountEntry4 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 31003,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 32100,//40500
                                        SegmentId = 1,
                                        BranchId = (short)data5.BranchbankId,//(short)Chequereconcilation.BranchId,//data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };
                                    var accountEntry5 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 31003,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.D,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 40500,//40500
                                        SegmentId = 1,
                                        BranchId = data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };
                                    var accountEntry6 = new AcountingEntry
                                    {
                                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                                        AccountNo = 40500,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = (decimal)data5.Amount,
                                        TransactionMode = TransactionModes.CH,
                                        Description = "BY CHQ CLEARED -SD DEPOSIT- " + data5.BranchId + ",NO - " + data5.Chequeno + ",SEQ-" + data5.ChequeSeq,
                                        ContraNo = 31003,//40500
                                        SegmentId = 1,
                                        BranchId = data3.BranchId,
                                        ReferenceId = data3.DepositId,
                                    };

                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                                }

                                _subLedgerRequest.AccountingVoucher.Amount = (decimal)data5.Amount;
                                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CHEQUE;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay


                                string transactionMessage = _subLedgerRequest.ToString();

                                _subLedgerClient.PostMessage(transactionMessage);
                                string phone = GetPhoneNumber.getphone(data3.CustId, db);
                                Message.Message sms = new Message.Message();
                                if (phone != "1111111111")
                                {
                                    sms.SendSms("Deposit Message Send To Customer", (byte)data5.FirmId, (short)data5.BranchId, data3.DepositId, 4, data3.CustId, "MABDEP", phone, String.Format("Dear Member, INR {0} is credited in your  A/c {1} on {2}. Aval Bal is INR {3}", (decimal)data5.Amount, data5.DepositId, DateFunctions.sysdate(db), data3.DepositAmt), db);
                                }
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 200;
                                var Jsonstring = JsonSerializer.Serialize(existitem);
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);

                                Log.Information("Success");
                                return _Response;
                                // return Results.Ok(new { details = existitem, token = JwtToken });
                            }

                        }
                        else
                        {
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 400;


                            Log.Information("not found");
                            return _Response;
                            // return Results.BadRequest(new { status = "depositId is not valid " });
                        }
                    }
                }

                catch (Exception ex)
                {
                    var message = new { Status = "something went wrong" };
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 400;
                    transaction.Rollback();


                    Log.Information("not found");
                    return _Response;

                    // return Results.NotFound(message);
                }
            }
         
        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<BHApproveData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            string uniqueKey = TokenManagement.Extract(JwtToken);
            var cache = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
            List<Exception> FailedValidations = new List<Exception>();
            //if(cache.FirmId != _chequereconcilation.FirmId)
            //{
            //    FailedValidations.Add(new ApplicationException("FirmId Is invalid"));
            //}
            //if (cache.BranchId != _chequereconcilation.BranchId)
            //{
            //    FailedValidations.Add(new ApplicationException("branchId Is invalid"));
            //}
            //if (_chequereconcilation.DepositId == null)
            //{
            //    FailedValidations.Add(new ApplicationException("Depositid is invalid"));
            //}

            return FailedValidations;
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

    }
}
