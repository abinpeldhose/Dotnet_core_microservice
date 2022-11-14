﻿using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using MACOM.Contracts;
using MACOM.Integrations;
using Serilog;
using MACOM.Contracts.Model;
using System.Text.Json;
using customerSdmodule.Redis;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.Deposit
{
    public class DepositApi : BaseApi
    {
       
        private DepositData _data;
      
      //  private string _jwtToken;
     
        string _clientID = Guid.NewGuid().ToString();

       

        SublegerClient _subLedgerClient = new SublegerClient(Guid.NewGuid().ToString(), "10.192.5.44:19092");


        SubledgerRequest _subLedgerRequest = new SubledgerRequest();
        Message.Message sms = new Message.Message();

      
        public DepositData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            if (Data.TransactionMethod.ToUpper()== "cash".ToUpper())
            {
                return SaveAll(db);
            }
            else if (Data.TransactionMethod.ToUpper() == "cheque".ToUpper())
            {
                return SaveCheque(db);
            }
            else
            { 
                return SaveOnline(db);
            }
        }

        private ResponseData SaveAll(ModelContext db)
        {
            try
            {
                ResponseData _Response = new ResponseData();
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

                var MaxAmount = db.GeneralParameters.Where(x => x.ParmtrId == 10 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();
                var MinAmount = db.GeneralParameters.Where(x => x.ParmtrId == 7 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();
                var _sdMaster = db.SdMasters.FirstOrDefault(x => x.DepositId == Data.DepositId);

                if (_sdMaster == null)
                {
                    _Response.responseCode = 404;
                    var JsonString = JsonSerializer.Serialize(new { status = "Failed" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                    return _Response;
                }
                if (DateFunctions.ValidateAmount(db, _sdMaster.DepositAmt, Data.Amount) == true)
                {
                    _Response.responseCode = 404;
                    var JsonString = JsonSerializer.Serialize(new { status = "This amount is exeeded" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                    return _Response;
                }
                   SequenceGenerator dep = new SequenceGenerator();
                //var chequeSequence = (int)dep.getSequence(db, Data.FirmId, Data.BranchId, 4, 5); // Not required to br checked
                var transId=0;
                
                SdTran _sdtran = new SdTran();
                ModesOfTransaction _modesOfTransaction = new ModesOfTransaction();
                TransactionModes _transactionModes = new TransactionModes();
                int contra_no = 0;
                int account_no = 0;
                int in_transit_no = 31003;
                account_no = 40500;
                string descr;
                if (Data.TransactionMethod.ToUpper() == "cash".ToUpper())
                {
                    contra_no = 33000;
                    descr = "BY CASH RCVD" + "-"+Data.BranchId+"-"+ Data.DepositId;
                    _modesOfTransaction = ModesOfTransaction.CASH;
                    _transactionModes = TransactionModes.CS;
                }                
                else
                {
                    contra_no = 40601;
                    descr = "BY ONLINE PAYMENT RCVD" + "-" + Data.DepositId;
                    _modesOfTransaction = ModesOfTransaction.PaymentGateway;
                    _transactionModes = TransactionModes.TR;
                }
                
                 if (Data.TransactionMethod.ToUpper() != "cheque".ToUpper())
                {
                    transId = (int)dep.getSequence(db, Data.FirmId, (short)cacheDetails.BranchId, 4, 4);
                    Log.Information(transId.ToString());

                    _sdtran.FirmId = Data.FirmId;
                    _sdtran.BranchId = Data.BranchId;
                    _sdtran.DepositId = Data.DepositId;
                    _sdtran.TransNo = 0;
                    _sdtran.ContraNo = contra_no;
                    _sdtran.Amount = Data.Amount;
                    _sdtran.Descr = descr;
                    _sdtran.Type = "C";
                    _sdtran.AccountNo = account_no;
                    _sdtran.TraDt = DateFunctions.sysdate(db);
                    _sdtran.ValueDt = DateFunctions.sysdate(db);
                    _sdtran.TransId = transId;
                    _sdtran.VouchId = 0;
                    db.SdTrans.Add(_sdtran);
                             
                    _sdMaster.DepositAmt = _sdMaster.DepositAmt + Data.Amount;
                    _sdMaster.TrancationDate = DateFunctions.sysdate(db);
                    db.SaveChanges();
                }
                string _clientID = Guid.NewGuid().ToString();

                // Console.WriteLine("Client Ready");

                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");



                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                _subLedgerRequest.Header.MessageId = dep.MessageId(db);  //unique number generator//MessageId;
                _subLedgerRequest.Header.MessageDate = DateFunctions.sysdate(db).Date; //MessageDate;
                _subLedgerRequest.Header.Subject = "Deposit"; //Subject;
                _subLedgerRequest.DocumentDetail.FirmId = Data.FirmId; //FirmId;
                _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                _subLedgerRequest.DocumentDetail.ModuleId = 4;
                _subLedgerRequest.DocumentDetail.DocumentId = Data.DepositId;
                _subLedgerRequest.DocumentDetail.CustomerId = _sdMaster.CustId;
                _subLedgerRequest.DocumentDetail.CustomerName = _sdMaster.CustName;
                _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
               
                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                _subLedgerRequest.AccountingVoucher.Narration = "BEING-" + _sdtran.Descr;
                //if ((cacheDetails.BranchId == _sdMaster.BranchId && Data.TransactionMethod.ToUpper() == "cash".ToUpper()) || (_sdMaster.BranchId == 0 && Data.TransactionMethod.ToUpper() == "online".ToUpper()))
                if (cacheDetails.BranchId == _sdMaster.BranchId)
                {

                    var accountEntry1 = new AcountingEntry
                    {
                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                        AccountNo = contra_no,//33000
                        Type = EntryTypes.D,
                        Amount = Data.Amount,
                        TransactionMode = _transactionModes,
                        Description = descr,
                        ContraNo = account_no,//40500
                        SegmentId = 1,
                        BranchId = _sdMaster.BranchId,
                        ReferenceId = Data.DepositId,

                    };
                    var accountEntry2 = new AcountingEntry
                    {
                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                        AccountNo = account_no,//40500
                        Type = EntryTypes.C,
                        Amount = _sdtran.Amount,
                        TransactionMode = _transactionModes,
                        Description = _sdtran.Descr,
                        ContraNo = contra_no,//33000
                        SegmentId = 1,
                        BranchId = _sdMaster.BranchId,
                        ReferenceId = _sdtran.DepositId,

                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                }
                if (cacheDetails.BranchId != _sdMaster.BranchId)
                {
                    var accountEntry3 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = contra_no,//33000
                        Type = EntryTypes.D,
                        Amount = _sdtran.Amount,
                        TransactionMode = _transactionModes,
                        Description = _sdtran.Descr,
                        ContraNo = in_transit_no,  //31003
                        SegmentId = 1,
                        BranchId = (short)cacheDetails.BranchId,
                        ReferenceId = _sdtran.DepositId,

                    };
                    var accountEntry4 = new AcountingEntry
                    {
                        ModuleId = 1,// (short)cacheDetails.ModuleId,
                        AccountNo = in_transit_no, //31003
                        Type = EntryTypes.C,
                        Amount = _sdtran.Amount,
                        TransactionMode = _transactionModes,
                        Description = _sdtran.Descr,
                        ContraNo = contra_no,   //33000
                        SegmentId = 1,
                        //branchid must insert
                        BranchId = (short)cacheDetails.BranchId,
                        ReferenceId = _sdtran.DepositId,

                    };

                    var accountEntry5 = new AcountingEntry
                    {
                        ModuleId = 1,// (short)cacheDetails.ModuleId,
                        AccountNo = in_transit_no,//31003
                        Type = EntryTypes.D,
                        Amount = _sdtran.Amount,
                        TransactionMode = _transactionModes,
                        Description = _sdtran.Descr,
                        ContraNo = account_no,//40500
                        SegmentId = 1,
                        //branchid must insert
                        BranchId = _sdMaster.BranchId,
                        ReferenceId = _sdtran.DepositId,
                    };


                    var accountEntry6 = new AcountingEntry
                    {
                        ModuleId = 4, // (short)cacheDetails.ModuleId,
                        AccountNo = account_no,//40500
                        Type = EntryTypes.C,
                        Amount = _sdtran.Amount,
                        TransactionMode = _transactionModes,
                        Description = _sdtran.Descr,
                        ContraNo = in_transit_no,//31003
                        SegmentId = 1,
                        BranchId = _sdMaster.BranchId,
                        ReferenceId = _sdtran.DepositId,
                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                }


                _subLedgerRequest.AccountingVoucher.Amount = _sdtran.Amount;
                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                _subLedgerRequest.AccountingVoucher.TransactionMode = _modesOfTransaction;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                Log.Information(_subLedgerRequest.ToString());                                                                              //_subLedgerRequest.AccountingVoucher.
                string transactionMessage = _subLedgerRequest.ToString();
                Log.Information(transactionMessage);

                string phone = GetPhoneNumber.getphone(Data.CustomerId, db);
                if (phone != "1111111111")
                {
                    sms.SendSms("Deposit Message Send To Customer", _sdMaster.FirmId, _sdMaster.BranchId, Data.DepositId, 4, Data.CustomerId, "MABDEP", phone, String.Format("Dear Member, INR {0} is credited in your  A/c {1} on {2}. Aval Bal is INR {3}", Data.Amount, Data.DepositId, _sdtran.TraDt, _sdMaster.DepositAmt), db);
                }
                    _subLedgerClient.PostMessage(transactionMessage);
                Log.Information("Success");


                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "Cash", transId = _sdtran.TransId });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                throw new Exception(ex.Message);
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }

        }

        private ResponseData SaveCash(ModelContext db)
        {
            try
            {
                ResponseData _Response = new ResponseData();

                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

                var MaxAmount = db.GeneralParameters.Where(x => x.ParmtrId == 10 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();
                var MinAmount = db.GeneralParameters.Where(x => x.ParmtrId == 7 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();

                SequenceGenerator dep = new SequenceGenerator();
                var chequeSequence = (int)dep.getSequence(db, Data.FirmId, (short)cacheDetails.BranchId, 4, 5);
                var transId = (int)dep.getSequence(db, Data.FirmId,Data.BranchId, 4, 4);
                Log.Information(transId.ToString());

                SdTran _sdtran = new SdTran();
               // {
                    _sdtran.FirmId = Data.FirmId;
                    _sdtran.BranchId = Data.BranchId;
                    _sdtran.DepositId = Data.DepositId;
                    _sdtran.TransNo = 0;
                    _sdtran.ContraNo = 33000;
                    _sdtran.Amount = Data.Amount;
                    _sdtran.Descr = "BY CASH RCVD" + "-" + Data.DepositId;
                    _sdtran.Type = "C";
                    _sdtran.AccountNo = 40500;
                    _sdtran.TraDt = DateFunctions.sysdate(db);
                    _sdtran.ValueDt = DateFunctions.sysdate(db);
                    _sdtran.TransId = transId;
                    _sdtran.VouchId = 0;
                    db.SdTrans.Add(_sdtran);
                //}

                var data3 = db.SdMasters.FirstOrDefault(x => x.DepositId == Data.DepositId);

                if (data3 == null)
                {
                    
                    _Response.responseCode = 404;
                    var JsonString = JsonSerializer.Serialize(new { status = "Failed" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                    return _Response;
                }
                   // return Results.NotFound(new { status = "Failed" });
                data3.DepositAmt = data3.DepositAmt + Data.Amount;
                data3.TrancationDate = DateFunctions.sysdate(db);


                //   db.SdMaster.Add(data3);





                db.SaveChanges();
                string _clientID = Guid.NewGuid().ToString();

                // Console.WriteLine("Client Ready");

                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");



                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                _subLedgerRequest.Header.MessageId = dep.MessageId(db);  //unique number generator//MessageId;
                _subLedgerRequest.Header.MessageDate = DateFunctions.sysdate(db).Date; //MessageDate;
                _subLedgerRequest.Header.Subject = "Deposit"; //Subject;
                _subLedgerRequest.DocumentDetail.FirmId = Data.FirmId; //FirmId;
                _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                _subLedgerRequest.DocumentDetail.ModuleId = 4;
                _subLedgerRequest.DocumentDetail.DocumentId = Data.DepositId;
                _subLedgerRequest.DocumentDetail.CustomerId = data3.CustId;
                _subLedgerRequest.DocumentDetail.CustomerName = data3.CustName;
                _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                //_subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate =  Data.RealizationDate;//ChequeDate;
                //_subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = Data.ChequNo;
                //_subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = BankBranchId;
                //_subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = BankBranch;
                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                _subLedgerRequest.AccountingVoucher.Narration = "BEING-"+_sdtran.Descr;
                if (cacheDetails.BranchId == data3.BranchId)
                {

                    var accountEntry1 = new AcountingEntry
                    {
                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                        AccountNo = 33000,//33000
                        Type = EntryTypes.D,
                        Amount = _sdtran.Amount,
                        TransactionMode = TransactionModes.CS,
                        Description = _sdtran.Descr,
                        ContraNo = 40500,//40500
                        SegmentId = 1,
                        BranchId = data3.BranchId,
                        ReferenceId=_sdtran.DepositId,

                    };
                    var accountEntry2 = new AcountingEntry
                    {
                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                        AccountNo = 40500,//33000
                        Type = EntryTypes.C,
                        Amount = _sdtran.Amount,
                        TransactionMode = TransactionModes.CS,
                        Description = _sdtran.Descr,
                        ContraNo = 33000,//40500
                        SegmentId = 1,
                        BranchId = data3.BranchId,
                        ReferenceId = _sdtran.DepositId,

                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                }
                if (cacheDetails.BranchId != data3.BranchId)
                {
                    var accountEntry3 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = 33000,//33000
                        Type = EntryTypes.D,
                        Amount = _sdtran.Amount,
                        TransactionMode = TransactionModes.CS,
                        Description = _sdtran.Descr,
                        ContraNo = 31003,//40500
                        SegmentId = 1,
                        BranchId =(short) cacheDetails.BranchId,
                        ReferenceId = _sdtran.DepositId,

                    };
                    var accountEntry4 = new AcountingEntry
                    {
                        ModuleId = 1,// (short)cacheDetails.ModuleId,
                        AccountNo = 31003,//33000
                        Type = EntryTypes.C,
                        Amount = _sdtran.Amount,
                        TransactionMode = TransactionModes.CS,
                        Description = _sdtran.Descr,
                        ContraNo = 33000,//40500
                        SegmentId = 1,
                        //branchid must insert
                        BranchId = (short)cacheDetails.BranchId,
                        ReferenceId = _sdtran.DepositId,

                    };

                    var accountEntry5 = new AcountingEntry
                    {
                        ModuleId = 1,// (short)cacheDetails.ModuleId,
                        AccountNo = 31003,//33000
                        Type = EntryTypes.D,
                        Amount = _sdtran.Amount,
                        TransactionMode = TransactionModes.CS,
                        Description = _sdtran.Descr,
                        ContraNo = 40500,//40500
                        SegmentId = 1,
                        //branchid must insert
                        BranchId = data3.BranchId,
                        ReferenceId = _sdtran.DepositId,
                    };


                    var accountEntry6 = new AcountingEntry
                    {
                        ModuleId = 4, // (short)cacheDetails.ModuleId,
                        AccountNo = 40500,//33000
                        Type = EntryTypes.C,
                        Amount = _sdtran.Amount,
                        TransactionMode = TransactionModes.CS,
                        Description = _sdtran.Descr,
                        ContraNo = 31003,//40500
                        SegmentId = 1,
                        BranchId = data3.BranchId,
                        ReferenceId = _sdtran.DepositId,
                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                }


                _subLedgerRequest.AccountingVoucher.Amount = _sdtran.Amount;
                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CASH;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                Log.Information(_subLedgerRequest.ToString());                                                                              //_subLedgerRequest.AccountingVoucher.
                string transactionMessage = _subLedgerRequest.ToString();
                Log.Information(transactionMessage);



                _subLedgerClient.PostMessage(transactionMessage);
                Log.Information("Success");




                
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "Cash", transId = _sdtran.TransId });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
                // return Results.Ok(new { status = "Success", type = "Cash", transId = _sdtran.TransId });
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                throw new Exception(ex.Message);
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
                //return Results.NotFound(message);
            }
        }

        private ResponseData SaveCheque(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));


                var MaxAmount = db.GeneralParameters.Where(x => x.ParmtrId == 10 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();
                var MinAmount = db.GeneralParameters.Where(x => x.ParmtrId == 7 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();

                SequenceGenerator dep = new SequenceGenerator();
                var chequeSequence = (int)dep.getSequence(db, Data.FirmId, Data.BranchId, 4, 5);
                SdChequereconcilation _cheq = new SdChequereconcilation();
                _cheq.BranchId = Data.BranchId;
                _cheq.FirmId = Data.FirmId;
                _cheq.Amount = Data.Amount;
                _cheq.SubsidiarybankAccountno = Data.SubsidiaryBankAccountno;
                _cheq.ChqSubmiteDate = DateFunctions.sysdate(db);
                _cheq.Chequeno = Data.ChequeNo;
                _cheq.CustomerBank = Data.CustomerBank;
                _cheq.DepositId = Data.DepositId;
                _cheq.SubsidiarybankName = Data.SubsidiaryBankName;
                _cheq.RealizationDate =DateTime.Parse( Data.RealizationDate);
                _cheq.EmployeeCode = Data.EmployeeCode;
                _cheq.CustomerName = Data.CustomerName;
                _cheq.ChequeSeq = chequeSequence;
                _cheq.StatusId = 0;
                _cheq.BranchbankId = Data.BranchBankid;
                db.SdChequereconcilations.Add(_cheq);
                db.SaveChanges();

                string _clientID = Guid.NewGuid().ToString();


                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");



                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                _subLedgerRequest.Header.Subject = "Cheque submit"; //Subject;
                _subLedgerRequest.DocumentDetail.FirmId = Data.FirmId; //FirmId;
                _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                _subLedgerRequest.DocumentDetail.ModuleId = 4;
                _subLedgerRequest.DocumentDetail.DocumentId = Data.DepositId;
                _subLedgerRequest.DocumentDetail.CustomerId = Data.CustomerId;
                _subLedgerRequest.DocumentDetail.CustomerName = Data.CustomerName;
                _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate =DateTime.Parse( Data.RealizationDate).Date;// Data.RealizationDate;//ChequeDate;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = Data.ChequeNo;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = (int)Data.BranchBankid;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = (int)Data.SubsidiaryBankAccountno;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.CustomerBank = Data.CustomerBank;
                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                _subLedgerRequest.AccountingVoucher.Narration = "BEING CHQ RCVD - " + cacheDetails.BranchId + ",NO - " + Data.ChequeNo+" - "+chequeSequence +"FOR " + Data.DepositId + " AMOUNT IS " + Data.Amount;//CQ Rcvd for " +Data.DepositId+"Amount is "+Data.Amount;
                //if (cacheDetails.BranchId == Data.BranchId)
                //{

                    var accountEntry1 = new AcountingEntry
                    {
                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                        AccountNo = 32103,
                        SubAccountNo=100004,
                        Type = EntryTypes.D,
                        Amount = Data.Amount,
                        TransactionMode = TransactionModes.CH,
                        Description = "BY CHQ RCVD - "+cacheDetails.BranchId+",NO - "+Data.ChequeNo+",SEQ-"+chequeSequence, //CHQ RCVD-"+Data.ChequeNo+"-"+Data.DepositId,
                        ContraNo = 41241,//40500
                        SegmentId = 1,
                        BranchId =(short) Data.BranchBankid,//(short)cacheDetails.BranchId,
                        ReferenceId = Data.DepositId,

                    };
                    var accountEntry2 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = 41241,
                        SubAccountNo=200004,
                        Type = EntryTypes.C,
                        Amount = Data.Amount,
                        TransactionMode = TransactionModes.CH,
                        Description = "BY CHQ RCVD - " + cacheDetails.BranchId + ",NO - " + Data.ChequeNo + ",SEQ-" + chequeSequence,
                        ContraNo = 32103,//40500
                        SegmentId = 1,
                        BranchId = (short)Data.BranchBankid,//(short)cacheDetails.BranchId,
                        ReferenceId = Data.DepositId,
                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
               
                _subLedgerRequest.AccountingVoucher.Amount = Data.Amount;
                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                _subLedgerRequest.AccountingVoucher.ReferenceNo = chequeSequence;//Convert.ToInt64(Data.DepositId);
                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CHEQUE;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                //_subLedgerRequest.AccountingVoucher.
                string transactionMessage = _subLedgerRequest.ToString();



                _subLedgerClient.PostMessage(transactionMessage);






                Log.Information("Success");
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "Cheque", transId = _cheq.ChequeSeq });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
                // return Results.Ok(new { status = "Success", type = "Cheque", transId = _cheq.ChequeSeq });
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
        public ResponseData SaveOnline(ModelContext db)
        {
            var MaxAmount = db.GeneralParameters.Where(x => x.ParmtrId == 10 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();
            var MinAmount = db.GeneralParameters.Where(x => x.ParmtrId == 7 && x.ModuleId == 4).Select(x => x.ParmtrValue).FirstOrDefault();
           
            SequenceGenerator dep = new SequenceGenerator();
           // var chequeSequence = (int)dep.getSequence(db, Data.FirmId, Data.BranchId, 4, 5);
            var transId = (int)dep.getSequence(db, Data.FirmId, Data.BranchId, 4, 4);
            try
            {
                ResponseData _Response = new ResponseData();
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));


                var data = new SdTran
                {
                    FirmId =  Data.FirmId,
                    BranchId = Data.BranchId,
                    DepositId = Data.DepositId,
                    TransNo = 0,
                    ContraNo = 40601,
                    Amount = Data.Amount,
                    Descr = "BY ONLINE PAYMENT RCVD" + "-",
                    Type = "C",
                    AccountNo = 40500,
                    TraDt = DateFunctions.sysdate(db),
                    ValueDt = DateFunctions.sysdate(db),
                    TransId = transId,
                    VouchId = 0,

                };

                var data3 = db.SdMasters.FirstOrDefault(x => x.DepositId == Data.DepositId);

                if (data3 == null)
                {
                    
                    _Response.responseCode = 404;
                    var JsonString = JsonSerializer.Serialize(new { status = "Failed" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                    return _Response;
                }
                  //  return Results.NotFound(new { status = "Failed" });
                data3.DepositAmt = data3.DepositAmt + Data.Amount;
                data3.TrancationDate = DateFunctions.sysdate(db);


                db.SdTrans.Add(data);
                //  db.SdMasters.Add(data2);
                db.SaveChanges();

                string _clientID = Guid.NewGuid().ToString();

                // Console.WriteLine("Client Ready");

                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");



                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                _subLedgerRequest.Header.MessageId = dep.MessageId(db);//unique number generator//MessageId;
                _subLedgerRequest.Header.MessageDate =DateFunctions.sysdate(db).Date; //MessageDate;
                _subLedgerRequest.Header.Subject = "Deposit"; //Subject;
                _subLedgerRequest.DocumentDetail.FirmId = Data.FirmId; //FirmId;
                _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                _subLedgerRequest.DocumentDetail.ModuleId = 4;
                _subLedgerRequest.DocumentDetail.DocumentId = Data.DepositId;
                _subLedgerRequest.DocumentDetail.CustomerId = data3.CustId;
                _subLedgerRequest.DocumentDetail.CustomerName = data3.CustName;
                _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
               
                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                _subLedgerRequest.AccountingVoucher.Narration = "BEING - "+data.Descr;
                  if(data3.BranchId==0)
                {
                    var accountEntry1 = new AcountingEntry
                    {
                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                        AccountNo = 40601,//33000
                        Type = EntryTypes.D,
                        Amount = data.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = data.Descr,
                        ContraNo = 40500,//40500
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = Data.DepositId,

                    };
                    var accountEntry2 = new AcountingEntry
                    {
                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                        AccountNo = 40500,//33000
                        Type = EntryTypes.C,
                        Amount = data.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = data.Descr,
                        ContraNo = 40601,//40500
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = Data.DepositId,
                    };
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                }
                else
                {
                    var accountEntry1 = new AcountingEntry
                    {
                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                        AccountNo = 40601,//33000
                        Type = EntryTypes.D,
                        Amount = data.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = data.Descr,
                        ContraNo = 31003,//40500
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = Data.DepositId,

                    };
                    var accountEntry2 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = 31003,//33000
                        Type = EntryTypes.C,
                        Amount = data.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = data.Descr,
                        ContraNo = 40601,//40500
                        SegmentId = 1,
                        BranchId = 0,
                        ReferenceId = Data.DepositId,
                    };

                    var accountEntry3 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = 31003,//33000
                        Type = EntryTypes.D,
                        Amount = data.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = data.Descr,
                        ContraNo = 40500,//40500
                        SegmentId = 1,
                        BranchId = data3.BranchId,
                        ReferenceId = Data.DepositId,
                    };

                    var accountEntry4 = new AcountingEntry
                    {
                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                        AccountNo = 40500,//33000
                        Type = EntryTypes.C,
                        Amount = data.Amount,
                        TransactionMode = TransactionModes.TR,
                        Description = data.Descr,
                        ContraNo = 31003,//40500
                        SegmentId = 1,
                        BranchId = data3.BranchId,
                        ReferenceId = Data.DepositId,
                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                }

                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.AgentCode = PaymentGateway.RazorPay;
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.RequestId = transId.ToString();
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.Amount = data.Amount;
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.Channel = Channels.Web;
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.GatewayTransactionId = Guid.NewGuid().ToString();
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.ServiceCharge = 1;
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.Fee = 0;
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.Status = 1;
                _subLedgerRequest.AccountingVoucher.PaymentGatewayDetails.PaymentModes = PaymentGatewayModes.UPI;




                _subLedgerRequest.AccountingVoucher.Amount = data.Amount;
                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;//Convert.ToInt64(data.DepositId);
                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.PaymentGateway;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                    //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                              //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                                                  //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                          
                string transactionMessage = _subLedgerRequest.ToString();

                _subLedgerClient.PostMessage(transactionMessage);
                string phone = GetPhoneNumber.getphone(Data.CustomerId, db);

                if (phone != "1111111111")
                {

                    sms.SendSms("Deposit Message Send To Customer", Data.FirmId, Data.BranchId, Data.DepositId, 4, Data.CustomerId, "MABDEP", phone, String.Format("Dear Member, INR {0} is credited in your  A/c {1} on {2}. Aval Bal is INR {3}", Data.Amount, Data.DepositId, DateTime.Now.ToString(), data3.DepositAmt), db);
                }
                Log.Information("Success");
               // ResponseData _Response = new ResponseData();
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "ONLINE PAYMENT", transId = data.TransId });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
                // return Results.Ok(new { status = "Success", type = "ONLINE PAYMENT", transId = data.TransId });

            }
            //catch (FileNotFoundException fx)
            //{
            //    throw new FileNotFoundException();
            //    return Results.BadRequest("NotFound");
            //}
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
        public void Message(ModelContext db)
        {
            
            var details = (from master in db.SdMasters
                           join customer in db.Customers on master.CustId equals customer.CustId
                           where master.DepositId ==Data.DepositId
                           select new
                           {
                               customerName=customer.CustName,
                               custId=master.CustId,
                    
                           }).SingleOrDefault();
           
        }
        protected override string GetSerialisedDataBlockWithDeviceToken()
        {

            Data.DeviceID = _cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<DepositData>(Data);//.Replace(@"\u002B", "+").Replace(@"\u0026", "&");


            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {

            List<Exception> FailedValidations = new List<Exception>();

            return FailedValidations;
        }
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

       
    }

}
