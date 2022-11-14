using customerSdmodule.Model1;
using customerSdmodule.Redis;
using MACOM.Contracts;
using MACOM.Integrations;
using System.Text.Json;
using Serilog;
using static RedisCacheDemo.RedisCacheStore;
using customerSdmodule.ModelClass.DateFormat;
using MACOM.Contracts.Model;

namespace customerSdmodule.ModelClass.NewSD
{
    public class NewSDApi :BaseApi
    {
        private NewSDData _data;
       // private string _jwtToken;
      

        string _clientID = Guid.NewGuid().ToString();


        SublegerClient _subLedgerClient = new SublegerClient(Guid.NewGuid().ToString(), "10.192.5.44:19092");


        SubledgerRequest _subLedgerRequest = new SubledgerRequest();

        public NewSDData Data { get => _data; set => _data = value; }
      //  public string JwtToken { get => _jwtToken; set => _jwtToken = value; }
     

        public ResponseData Get(ModelContext db)
        {
          return SaveCash(db);
        }


        private ResponseData SaveCash(ModelContext db)
        {
            try
            {

                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));

                String Nomineeid = null;
                var Message = new { Status = "Success" };
                var Error = new { Status = "Failed" };
                //long NomineeId = 0,  Id; 
                long Id;
                String coapplicantId = 0.ToString();

                SequenceGenerator dep = new SequenceGenerator();


                int depSequence;
                String padFirmid = Data.Firmid.ToString();
                String pfirmid = padFirmid.PadLeft(2, '0');

                String padBranchid = Data.Branchid.ToString();
                String pbranchid = padBranchid.PadLeft(4, '0');

                String padModuleid = Data.Moduleid.ToString();
                String pmoduleid = padModuleid.PadLeft(2, '0');
                String padDepositid;
                String pdepositid = null;

                int depverifysequence = (int)dep.getSequence(db, Data.Firmid, Data.Branchid, Data.Moduleid, 3);
                int depNomineeSequence = (int)dep.getSequence(db, Data.Firmid, Data.Branchid, Data.Moduleid, 2);
                int transId = (int)dep.getSequence(db, Data.Firmid, Data.Branchid, Data.Moduleid, 4);
                var chequeSequence = (int)dep.getSequence(db, Data.Firmid, Data.Branchid, 4, 5);

                if (Data.Transactionmethod.ToUpper() == "CASH" || Data.Transactionmethod.ToUpper() == "ONLINE PAYMENT")
                {
                    depSequence = (int)dep.getSequence(db, Data.Firmid, Data.Branchid, Data.Moduleid, 1);
                    padDepositid = depSequence.ToString();
                    pdepositid = padDepositid.PadLeft(8, '0');
                }

                // String padVerifyid = depSequence.ToString(); 
                String padVerifyid = depverifysequence.ToString();
                String pverifyid = padVerifyid.PadLeft(6, '0');

                int CoApplicantSequence;
                String padCoApplicantid;
                String pcoapplicant;

                //var documentid = db.SdSubApplicants.Select(x => Convert.ToInt64(x.DocumentId)).Max() + 1; 
                if (Data.Coapplicantcustomerid != null)
                {

                    CoApplicantSequence = dep.getSequence(db, Data.Firmid, (short)Data.Branchid, Data.Moduleid, 2);
                    //  int CoApplicantSequence = depSequence; 
                    padCoApplicantid = CoApplicantSequence.ToString();
                    pcoapplicant = padCoApplicantid.PadLeft(6, '0');

                    coapplicantId = pfirmid + pbranchid + pmoduleid + pcoapplicant;

                }

                // int nomineeSequence = depSequence; 
                String padNomineeid = depNomineeSequence.ToString();
                String pnomineeid = padNomineeid.PadLeft(6, '0');
                Nomineeid = pfirmid + pbranchid + pmoduleid + pnomineeid;



                String nominee;

                if (Data.Nomineename != null)
                {
                    nominee = "T";
                }
                else
                {
                    nominee = "F";
                }

                String Verifyid = pfirmid + pbranchid + pmoduleid + pverifyid;
                if (Data.Transactionmethod.ToUpper() == "CASH")
                {
                    if (DateFunctions.ValidateAmount(db, 0, Data.Amount) == false)
                    {
                        String depositid = pfirmid + pbranchid + pmoduleid + pdepositid;



                        Console.WriteLine(depositid);
                        Console.WriteLine(Verifyid);


                        var data = new SdTran
                        {

                            FirmId = Data.Firmid,
                            BranchId = Data.Branchid,
                            DepositId = depositid,
                            TransNo = 0,
                            TraDt = DateFunctions.sysdate(db),//DateTime.Now,
                            TransId = transId,//rnd.Next() / 10000,
                            Amount = Data.Amount,
                            Descr = "BY CASH - " + Data.Branchid + "- NEW SD" + "-" + depositid,
                            Type = "C",
                            AccountNo = 40500,
                            ContraNo = 33000,
                            ValueDt = DateFunctions.sysdate(db),// DateTime.Now,
                            VouchId = 0,
                        };


                        var data1 = new SdVerification
                        {

                            CustId = Data.Customerid,
                            CustName = Data.Customername,
                            DepositDate = DateFunctions.sysdate(db), //DateTime.Now,
                            IntRt = Data.Interestrate,
                            FirmId = Data.Firmid,
                            BranchId = Data.Branchid,
                            DepositType = Data.Deposittype,
                            SchemeId = (byte)Data.Schemeid,
                            DepositAmt = Data.Amount,
                            VerifyId = Verifyid,
                            DepositId = depositid,
                            UserId = Data.Userid.ToString(),
                            Nominee = nominee,
                            AbhId = 0,
                            TdsCode = Data.Tds_code,
                            Mobilizer = Data.Sales_code,
                            IncentiveId = Data.Type,

                            CategoryId = 0,
                        };

                        var nomineeData = new SdSubApplicant
                        {
                            Name = Data.Nomineename,
                            Dob = DateTime.Parse(Data.Nomineedob),
                            Relation = Data.Nomineerelation,
                            FatHus = Data.Nomineefathus,
                            House = Data.Nomineehousename,
                            Location = Data.Nomineelocation,
                            Phone = Data.Nomineephoneno,
                            DocumentId = depositid,
                            MinorGuardian = Data.Nomineegurdianname,
                            MinorStatus = (Data.Nomineegurdianname == null) ? (byte)0 : (byte)1,
                            FirmId = Data.Firmid,
                            BranchId = Data.Branchid,
                            Category = (Data.Nomineename == null) ? 0 : 2,
                            Adduser = Data.Userid.ToString(),
                            NomineeId = Nomineeid,
                            ModuleId = Data.Moduleid,
                        };
                        var data3 = new SdMaster
                        {
                            FirmId = Data.Firmid,
                            BranchId = Data.Branchid,
                            ModuleId = 4,
                            DepositId = depositid,
                            CustId = Data.Customerid,
                            CustName = Data.Customername,
                            DepositType = Data.Deposittype,
                            DepositAmt = Data.Amount,
                            TdsCode = Data.Tds_code,
                            IntRt = Data.Interestrate,
                            DepositDate = DateTime.Now,
                            TrancationDate = DateTime.Now,
                            CloseDate = DateTime.Now,
                            SchemeId = Data.Schemeid,
                            Mobilizer = Data.Sales_code,
                            StatusId = 1,
                            Lien =0.ToString(),
                            Nominee = (Data.Nomineename == null) ? 0 : 1,
                            // TdsCode = true, 
                            Minor = 0,
                            IncentiveId = Data.Type,
                            // Citizen = 0.ToString(), 
                            Balance = Data.Amount,

                        };


                        if (Data.Coapplicantcustomerid != null)
                        {
                            var coApplicantData = new SdSubApplicant
                            {
                                Name = Data.Coapplicantname,
                                CustId = Data.Coapplicantcustomerid,
                                NomineeId = coapplicantId.ToString(),

                                Category = 1,
                                DocumentId = depositid,
                                FirmId = Data.Firmid,
                                BranchId = Data.Branchid,
                                ModuleId = Data.Moduleid,

                                SubType = (byte)Data.Subtype,


                            };
                            Console.WriteLine(coApplicantData.CustId);
                            Console.WriteLine(coApplicantData.NomineeId);
                            db.SdSubApplicants.Add(coApplicantData);
                            db.SaveChanges();

                        }



                        db.SdVerifications.Add(data1);
                        db.SdSubApplicants.Add(nomineeData);
                        db.SdMasters.Add(data3);
                        //  db.SaveChanges();
                        db.SdTrans.Add(data);
                        db.SaveChanges();




                        Log.Information("Success");


                        string _clientID = Guid.NewGuid().ToString();

                        // Console.WriteLine("Client Ready");

                        SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");



                        _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                        _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                        _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                        _subLedgerRequest.Header.Subject = "NEW SD"; //Subject;
                        _subLedgerRequest.DocumentDetail.FirmId = Data.Firmid; //FirmId;
                        _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                        _subLedgerRequest.DocumentDetail.ModuleId = 4;
                        _subLedgerRequest.DocumentDetail.DocumentId = depositid;
                        _subLedgerRequest.DocumentDetail.CustomerId = data3.CustId;
                        _subLedgerRequest.DocumentDetail.CustomerName = data3.CustName;
                        _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;

                        _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                        _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + data.Descr;
                        if (cacheDetails.BranchId == data3.BranchId)
                        {

                            var accountEntry1 = new AcountingEntry
                            {
                                ModuleId = 1, //(short)cacheDetails.ModuleId,
                                AccountNo = 33000,//33000
                                Type = EntryTypes.D,
                                Amount = data.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = data.Descr,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                BranchId = data3.BranchId,
                                ReferenceId = data.DepositId,

                            };
                            var accountEntry2 = new AcountingEntry
                            {
                                ModuleId = 4,//(short)cacheDetails.ModuleId,
                                AccountNo = 40500,//33000
                                Type = EntryTypes.C,
                                Amount = data.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = data.Descr,
                                ContraNo = 33000,//40500
                                SegmentId = 1,
                                BranchId = data3.BranchId,
                                ReferenceId = data.DepositId,
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
                                Amount = data.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = data.Descr,
                                ContraNo = 31003,//40500
                                SegmentId = 1,
                                BranchId = (short)cacheDetails.BranchId,
                                ReferenceId = data.DepositId,
                            };
                            var accountEntry4 = new AcountingEntry
                            {
                                ModuleId = 1,// (short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                Type = EntryTypes.C,
                                Amount = data.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = data.Descr,
                                ContraNo = 33000,//40500
                                SegmentId = 1,
                                //branchid must insert
                                BranchId = (short)cacheDetails.BranchId,
                                ReferenceId = data.DepositId,
                            };

                            var accountEntry5 = new AcountingEntry
                            {
                                ModuleId = 1,// (short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                Type = EntryTypes.D,
                                Amount = data.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = data.Descr,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                //branchid must insert
                                BranchId = data3.BranchId,
                                ReferenceId = data.DepositId,
                            };


                            var accountEntry6 = new AcountingEntry
                            {
                                ModuleId = 4, // (short)cacheDetails.ModuleId,
                                AccountNo = 40500,//33000
                                Type = EntryTypes.C,
                                Amount = data.Amount,
                                TransactionMode = TransactionModes.CS,
                                Description = data.Descr,
                                ContraNo = 31003,//40500
                                SegmentId = 1,
                                BranchId = data3.BranchId,
                                ReferenceId = data.DepositId,
                            };

                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                        }


                        _subLedgerRequest.AccountingVoucher.Amount = data.Amount;
                        _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                        _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;//Convert.ToInt64(data.DepositId);
                        _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                        _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CASH;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                        //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                        //_subLedgerRequest.AccountingVoucher.
                        string transactionMessage = _subLedgerRequest.ToString();


                        Message.Message sms = new Message.Message();
                        string phone = GetPhoneNumber.getphone(Data.Customerid, db);
                        if (phone != "1111111111")
                        {
                            sms.SendSms("New SD Account", (byte)Data.Firmid, Data.Branchid, depositid, 4, Data.Customerid, "MABDEP", phone, String.Format("Welcome to MABEN NIDHI LTD. Thanks for opening a {0} {1} account with us Aval Bal is INR {2} - MABEN NIDHI LTD.", "Savings", "Deposit", Data.Amount), db);
                        }
                        _subLedgerClient.PostMessage(transactionMessage);
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "cash", depositId = data1.DepositId });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                    }
                    else
                    {
                        Log.Error("This amount is exceeded");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "This amount is reached" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                        //  return Results.NotFound(new { status = "This amount is reached" });
                    }


                }
                else if (Data.Transactionmethod.ToUpper() == "CHEQUE")
                {

                    // String Verifyid = pfirmid + pbranchid + pmoduleid + pverifyid;
                    //  String depositid = pfirmid + pbranchid + pmoduleid + pdepositid;
                    var data = new SdVerification
                    {
                        CustId = Data.Customerid,
                        CustName = Data.Customername,
                        DepositDate = DateFunctions.sysdate(db),//DateTime.Now,
                        IntRt = Data.Interestrate,
                        FirmId = Data.Firmid,
                        BranchId = Data.Branchid,
                        ModuleId = Data.Moduleid,
                        DepositType = Data.Deposittype,
                        SchemeId = (byte)Data.Schemeid,
                        DepositAmt = Data.Amount,
                        VerifyId = Verifyid,
                        UserId = Data.Userid.ToString(),
                        Nominee = nominee,
                        AbhId = 0,
                        TdsCode = Data.Tds_code,
                        Mobilizer = Data.Sales_code,
                        IncentiveId = Data.Type,//employeeId == null || agentId == null ? 0 : 1,
                        CategoryId = 0,
                    };

                    var nomineeData = new SdSubApplicant
                    {
                        Name = Data.Nomineename,
                        Dob = DateTime.Parse(Data.Nomineedob),
                        Relation = Data.Nomineerelation,
                        FatHus = Data.Nomineefathus,
                        House = Data.Nomineehousename,
                        Location = Data.Nomineelocation,
                        Phone = Data.Nomineephoneno,
                        MinorGuardian = Data.Nomineegurdianname,
                        MinorStatus = Data.Nomineegurdianname == null ? (byte)0 : (byte)1,
                        Adduser = Data.Userid.ToString(),
                        DocumentId = Verifyid,
                        FirmId = Data.Firmid,
                        BranchId = Data.Branchid,
                        Category = (Data.Nomineename == null) ? 0 : 2,
                        SubType = (byte)Data.Statusid,
                        NomineeId = Nomineeid,
                        ModuleId = Data.Moduleid,
                    };


                    var data3 = new SdChequereconcilation
                    {
                        BranchId = Data.Branchid,
                        FirmId = Data.Firmid,
                        Amount = Data.Amount,
                        Chequeno = Data.Chno,
                        SubsidiarybankAccountno = Data.Subsidaryaccountno,
                        CustomerBank = Data.Customerbank,
                        SubsidiarybankName = Data.Subsidiarybankname,
                        ChqSubmiteDate = DateFunctions.sysdate(db),//DateTime.Now,
                        RealizationDate = DateTime.Parse(Data.Chequedate),
                        EmployeeCode = Convert.ToInt32(Data.Userid),
                        CustomerName = Data.Customername,
                        ChequeSeq = chequeSequence,
                        StatusId = 0,
                        BranchbankId = Data.BranchBankid,

                        DepositId = Verifyid,
                    };

                    db.SdVerifications.Add(data);
                    db.SdChequereconcilations.Add(data3);
                    db.SdSubApplicants.Add(nomineeData);

                    if (Data.Coapplicantcustomerid != null)
                    {
                        var coApplicantData = new SdSubApplicant
                        {
                            Name = Data.Coapplicantname,
                            CustId = Data.Coapplicantcustomerid,
                            NomineeId = coapplicantId.ToString(),
                            Category = 1,
                            DocumentId = Verifyid,
                            FirmId = Data.Firmid,
                            BranchId = (byte)Data.Branchid,
                            ModuleId = Data.Moduleid,
                            SubType = (byte)Data.Subtype,


                        };
                        db.SdSubApplicants.Add(coApplicantData);
                        db.SaveChanges();
                    }

                    db.SaveChanges();
                    Log.Information("Success");



                    string _clientID = Guid.NewGuid().ToString();

                    // Console.WriteLine("Client Ready");

                    SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");



                    _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                    _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                    _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                    _subLedgerRequest.Header.Subject = "cheque submit"; //Subject;
                    _subLedgerRequest.DocumentDetail.FirmId = data.FirmId; //FirmId;
                    _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                    _subLedgerRequest.DocumentDetail.ModuleId = 4;
                    _subLedgerRequest.DocumentDetail.DocumentId = Verifyid;
                    _subLedgerRequest.DocumentDetail.CustomerId = data.CustId;
                    _subLedgerRequest.DocumentDetail.CustomerName = data.CustName;
                    _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                    // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                    // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate = DateTime.UtcNow.Date;//ChequeDate;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = data3.Chequeno;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = (short)data3.BranchbankId;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = (short)data3.SubsidiarybankAccountno;
                    _subLedgerRequest.AccountingVoucher.ChequeDetail.CustomerBank = data3.CustomerBank;
                    _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                    _subLedgerRequest.AccountingVoucher.Narration = "BEING CHEQ RCVD NEW SD- " + cacheDetails.BranchId + ",NO- " + data3.Chequeno + " - " + chequeSequence + "FOR " + data3.DepositId + " AMOUNT IS " + Data.Amount;
                    //if (cacheDetails.BranchId == Data.Branchid)
                    //{
                    var accountEntry1 = new AcountingEntry
                    {
                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                        AccountNo = 32103,//33000
                        SubAccountNo = 100004,
                        Type = EntryTypes.D,
                        Amount = Data.Amount,
                        TransactionMode = TransactionModes.CH,
                        Description = "BY CHQ RCVD - NEW SD" + cacheDetails.BranchId + ",NO - " + data3.Chequeno + ",SEQ-" + chequeSequence,///"BY CHQ RCVD - " + cacheDetails.BranchId + ",NUM - " + data3.Chequeno,
                        ContraNo = 41241,//40500
                        SegmentId = 1,
                        BranchId = (short)Data.BranchBankid,
                        ReferenceId = data3.DepositId,
                    };
                    var accountEntry2 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = 41241,//33000
                        SubAccountNo = 200004,
                        Type = EntryTypes.C,
                        Amount = Data.Amount,
                        TransactionMode = TransactionModes.CH,
                        Description = "BY CHQ RCVD - NEW SD" + cacheDetails.BranchId + ",NO - " + data3.Chequeno + ",SEQ-" + chequeSequence,
                        ContraNo = 32103,//40500
                        SegmentId = 1,
                        BranchId = (short)Data.BranchBankid,                            ///(short)cacheDetails.BranchId,
                        ReferenceId = data3.DepositId,

                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                    //}




                    _subLedgerRequest.AccountingVoucher.Amount = Data.Amount;
                    _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                    _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;// Convert.ToInt64(Verifyid);
                    _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                    _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CHEQUE;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                      //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                      //_subLedgerRequest.AccountingVoucher.
                    string transactionMessage = _subLedgerRequest.ToString();



                    _subLedgerClient.PostMessage(transactionMessage);


                    // return data3 == null ? Results.NotFound(Error) : Results.Ok(new { status = "Success", type = "cheque", depositId = Verifyid });
                    if (data3 == null)
                    {
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(Error);
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                    else
                    {
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "cheque", depositId = Verifyid });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else if (Data.Transactionmethod.ToLower() == "online payment")
                {

                    String depositid = pfirmid + pbranchid + pmoduleid + pdepositid;

                    Console.WriteLine(depositid);
                    Console.WriteLine(Verifyid);


                    var data = new SdTran
                    {

                        FirmId = Data.Firmid,
                        BranchId = Data.Branchid,
                        DepositId = depositid,
                        TransNo = 0,
                        TraDt = DateFunctions.sysdate(db),//DateTime.Now,
                        TransId = transId,//rnd.Next() / 10000,
                        Amount = Data.Amount,
                        Descr = "BY" + Data.Transactionmethod + "-" + "NEW SD" + "-" + depositid,
                        Type = "C",
                        AccountNo = 40500,
                        ContraNo = 33000,
                        ValueDt = DateFunctions.sysdate(db),// DateTime.Now,
                        VouchId = 0,
                    };


                    var data1 = new SdVerification
                    {

                        CustId = Data.Customerid,
                        CustName = Data.Customername,
                        DepositDate = DateFunctions.sysdate(db),//DateTime.Now,
                        IntRt = Data.Interestrate,
                        FirmId = Data.Firmid,
                        BranchId = Data.Branchid,
                        DepositType = Data.Deposittype,
                        SchemeId = (byte)Data.Schemeid,
                        DepositAmt = Data.Amount,
                        VerifyId = Verifyid,
                        DepositId = depositid,
                        UserId = Data.Userid.ToString(),
                        Nominee = nominee,
                        AbhId = 0,
                        TdsCode = Data.Tds_code,
                        Mobilizer = Data.Sales_code,
                        IncentiveId = Data.Type,

                        CategoryId = 0,
                    };

                    var nomineeData = new SdSubApplicant
                    {
                        Name = Data.Nomineename,
                        Dob = DateTime.Parse(Data.Nomineedob),
                        Relation = Data.Nomineerelation,
                        FatHus = Data.Nomineefathus,
                        House = Data.Nomineehousename,
                        Location = Data.Nomineelocation,
                        Phone = Data.Nomineephoneno,
                        DocumentId = depositid,
                        MinorGuardian = Data.Nomineegurdianname,
                        MinorStatus = (Data.Nomineegurdianname == null) ? (byte)0 : (byte)1,
                        FirmId = Data.Firmid,
                        BranchId = Data.Branchid,
                        Category = (Data.Nomineename == null) ? 0 : 2,
                        Adduser = Data.Userid.ToString(),
                        NomineeId = Nomineeid,
                        ModuleId = Data.Moduleid,
                    };
                    var data3 = new SdMaster
                    {
                        FirmId = Data.Firmid,
                        BranchId = Data.Branchid,
                        ModuleId = 4,
                        DepositId = depositid,
                        CustId = Data.Customerid,
                        CustName = Data.Customername,
                        DepositType = Data.Deposittype,
                        DepositAmt = Data.Amount,
                        TdsCode = Data.Tds_code,
                        IntRt = Data.Interestrate,
                        DepositDate = DateTime.Now,
                        TrancationDate = DateTime.Now,
                        CloseDate = DateTime.Now,
                        SchemeId = Data.Schemeid,
                        Mobilizer = Data.Sales_code,
                        StatusId = 1,
                        Lien =0.ToString(),
                        Nominee = (Data.Nomineename == null) ? 0 : 1,
                        // TdsCode = true, 
                        Minor = 0,
                        IncentiveId = Data.Type,
                        // Citizen = 0.ToString(), 
                        Balance = Data.Amount,

                    };

                    db.SdVerifications.Add(data1);
                    db.SdSubApplicants.Add(nomineeData);
                    if (Data.Coapplicantcustomerid != null)
                    {
                        var coApplicantData = new SdSubApplicant
                        {
                            Name = Data.Coapplicantname,
                            CustId = Data.Coapplicantcustomerid,
                            NomineeId = coapplicantId.ToString(),

                            Category = 1,
                            DocumentId = depositid,
                            FirmId = Data.Firmid,
                            BranchId = Data.Branchid,
                            ModuleId = Data.Moduleid,

                            SubType = (byte)Data.Subtype,


                        };

                        db.SdSubApplicants.Add(coApplicantData);
                        db.SaveChanges();

                    }


                    db.SdMasters.Add(data3);

                    db.SdTrans.Add(data);
                    db.SaveChanges();

                    string _clientID = Guid.NewGuid().ToString();

                    // Console.WriteLine("Client Ready");

                    SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");



                    _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                    _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                    _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                    _subLedgerRequest.Header.Subject = "NEW SD"; //Subject;
                    _subLedgerRequest.DocumentDetail.FirmId = Data.Firmid; //FirmId;
                    _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                    _subLedgerRequest.DocumentDetail.ModuleId = 4;
                    _subLedgerRequest.DocumentDetail.DocumentId = depositid;
                    _subLedgerRequest.DocumentDetail.CustomerId = data3.CustId;
                    _subLedgerRequest.DocumentDetail.CustomerName = data3.CustName;
                    _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;

                    //_subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = BankBranch;
                    _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                    _subLedgerRequest.AccountingVoucher.Narration = "BEING ONLINE PAYMENT FROM" + data3.DepositId + "Amount is " + Data.Amount;
                    if (cacheDetails.BranchId == data3.BranchId)
                    {

                        var accountEntry1 = new AcountingEntry
                        {
                            ModuleId = 1, //(short)cacheDetails.ModuleId,
                            AccountNo = 33000,//33000
                            Type = EntryTypes.D,
                            Amount = data.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = data.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            BranchId = data3.BranchId,
                            ReferenceId = data3.DepositId,

                        };
                        var accountEntry2 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.C,
                            Amount = data.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = data.Descr,
                            ContraNo = 33000,//40500
                            SegmentId = 1,
                            BranchId = data3.BranchId,
                            ReferenceId = data3.DepositId,
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
                            Amount = data.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = data.Descr,
                            ContraNo = 31003,//40500
                            SegmentId = 1,
                            BranchId = 0,
                            ReferenceId = data3.DepositId,
                        };
                        var accountEntry4 = new AcountingEntry
                        {
                            ModuleId = 1,// (short)cacheDetails.ModuleId,
                            AccountNo = 31003,//33000
                            Type = EntryTypes.C,
                            Amount = data.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = data.Descr,
                            ContraNo = 33000,//40500
                            SegmentId = 1,
                            //branchid must insert
                            BranchId = 0,
                            ReferenceId = data3.DepositId,
                        };

                        var accountEntry5 = new AcountingEntry
                        {
                            ModuleId = 1,// (short)cacheDetails.ModuleId,
                            AccountNo = 31003,//33000
                            Type = EntryTypes.C,
                            Amount = data.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = data.Descr,
                            ContraNo = 40500,//40500
                            SegmentId = 1,
                            //branchid must insert
                            BranchId = data3.BranchId,
                            ReferenceId = data3.DepositId,
                        };


                        var accountEntry6 = new AcountingEntry
                        {
                            ModuleId = 4, // (short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            Type = EntryTypes.C,
                            Amount = data.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = data.Descr,
                            ContraNo = 31003,//40500
                            SegmentId = 1,
                            BranchId = data3.BranchId,
                            ReferenceId = data3.DepositId,
                        };

                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                    }


                    _subLedgerRequest.AccountingVoucher.Amount = data.Amount;
                    _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                    _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;// Convert.ToInt64(data.DepositId);
                    _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Receipt;  //Enum
                    _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.PaymentGateway;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay

                    string transactionMessage = _subLedgerRequest.ToString();

                    _subLedgerClient.PostMessage(transactionMessage);

                    Message.Message sms = new Message.Message();
                    string phone = GetPhoneNumber.getphone(Data.Customerid, db);
                    if (phone != "1111111111")
                    {
                        sms.SendSms("New SD Account", (byte)Data.Firmid, Data.Branchid, depositid, 4, Data.Customerid, "MABDEP", phone, String.Format("Welcome to MABEN NIDHI LTD. Thanks for opening a {0} {1} account with us Aval Bal is INR {2} - MABEN NIDHI LTD.", "Savings", "Deposit", Data.Amount), db);
                    }
                    Log.Information("Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "online payment", depositId = data1.DepositId });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
                else
                {
                    Log.Warning("TransactionMethod is not available");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "TransactionMethod is not available" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
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
                return _Response;
            }

        }



        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            List<Exception> FailedValidations = new List<Exception>();


            var verification = db.SdVerifications.Where(x => x.CustId == Data.Customerid && x.SchemeId == Data.Schemeid && x.ModuleId == Data.Moduleid && x.FirmId == Data.Firmid && x.BranchId == Data.Branchid).ToList();
            var master = db.SdMasters.Where(x => x.CustId ==Data.Customerid && x.SchemeId == Data.Schemeid && x.ModuleId == Data.Moduleid && x.FirmId == Data.Firmid && x.BranchId == Data.Branchid).ToList();

            if (Data.Amount == 0)
            {
                FailedValidations.Add(new ApplicationException("Invalid data is entered"));
            }        

            return FailedValidations;
        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            Data.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<NewSDData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }
        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

      
    }
}
