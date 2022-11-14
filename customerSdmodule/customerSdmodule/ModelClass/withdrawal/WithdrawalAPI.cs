using Confluent.Kafka;
using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Redis;
using customerSdmodule.sample;
using MACOM.Contracts;
using MACOM.Contracts.Model;
using MACOM.Integrations;
using Serilog;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.withdrawal
{
    public class WithdrawalAPI : BaseApi
    {
        private withdrawalData _withdrawalDetails;

        ResponseData _Response = new ResponseData();
        public withdrawalData WithdrawalDetails { get => _withdrawalDetails; set => _withdrawalDetails = value; }
       
        SequenceGenerator dep = new SequenceGenerator();
        public ResponseData Get(ModelContext db)
        {
            var status = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).Select(x => x.StatusId).SingleOrDefault();
            var lien = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).Select(x => x.Lien).SingleOrDefault();
            if (status == 1)
            {
                if (WithdrawalDetails.TransactionMethod.ToLower() == "cash")
                {
                    return SaveCash(db);
                }
                else if (WithdrawalDetails.TransactionMethod.ToLower() == "bank")
                {
                    return SaveBank(db);
                }
                else if (WithdrawalDetails.TransactionMethod.ToLower() == "sd")
                {
                    return SaveSd(db);
                }
                else if (WithdrawalDetails.TransactionMethod.ToLower() == "rd")
                {
                    return SaveRd(db);
                }
                else if (WithdrawalDetails.TransactionMethod.ToLower() == "goldloan")
                {
                    return SaveGoldLoan(db);
                }
                else if (WithdrawalDetails.TransactionMethod.ToLower() == "cheque")
                {
                    return SaveCheque(db);
                }
                else
                {
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "There is no other transaction methods available" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    
                    return _Response;
                   
                }
            }
            else if (status == 0)
            {
                _Response.responseCode = 404;
                var Jsonstring = JsonSerializer.Serialize(new { status = "This account is already closed" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "There is no other transaction methods available" });
                return _Response;
            }
            else if (status == 9)
            {
                _Response.responseCode = 404;
                var Jsonstring = JsonSerializer.Serialize(new { status = "This account is NonOperative" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "There is no other transaction methods available" });
                return _Response;
            }
            else if (status == 2)
            {
                _Response.responseCode = 404;
                var Jsonstring = JsonSerializer.Serialize(new { status = "You have a standing instruction pending" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "There is no other transaction methods available" });
                return _Response;
            }
            else if (status == 3)
            {
                _Response.responseCode = 404;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Withdrawal not permitted" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "There is no other transaction methods available" });
                return _Response;
            }
            else if (status == 5)
            {
                _Response.responseCode = 404;
                var Jsonstring = JsonSerializer.Serialize(new { status = "Account is terminated" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "There is no other transaction methods available" });
                return _Response;
            }
            else if(lien ==0.ToString())
            {
                _Response.responseCode = 404;
                var Jsonstring = JsonSerializer.Serialize(new { status = " This Document Id is lien marked, hence cannot be withdraw" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;

            }
            else
            {
                _Response.responseCode = 404;
                var Jsonstring = JsonSerializer.Serialize(new { status = "wrong status" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(new { status = "There is no other transaction methods available" });
                return _Response;
            }
         



        }
        public ResponseData SaveCash(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var transId = (int)dep.getSequence(db, WithdrawalDetails.FirmId, (short)cacheDetails.BranchId, (byte)WithdrawalDetails.ModuleId, 4);


                if (WithdrawalDetails.StartDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") &&  ////Today transaction
        WithdrawalDetails.EndDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") && WithdrawalDetails.NoOfOccurence == 1 && WithdrawalDetails.UserType == 0)
                {
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    if (sdMasterData != null)
                    {
                        if (DateFunctions.ValidateAmount(db, sdMasterData.DepositAmt, 0) == false)
                        {
                            if (sdMasterData.DepositAmt - WithdrawalDetails.Amount > 0)
                            {
                                var withdrawData = new SdTran
                                {
                                    FirmId = WithdrawalDetails.FirmId,
                                    BranchId = (short)cacheDetails.BranchId,
                                    DepositId = WithdrawalDetails.DepositId,
                                    TransId = transId,
                                    TransNo = 0,
                                    TraDt = DateFunctions.sysdate(db),
                                    Descr = "BY CASH-SD WITHDRAWAL" + " - " + cacheDetails.BranchId + " - " + WithdrawalDetails.DepositId,

                                    Amount = WithdrawalDetails.Amount,
                                    Type = "D",
                                    AccountNo = 40500,
                                    ContraNo = 33000,
                                    ValueDt = DateFunctions.sysdate(db),
                                    VouchId = 0,
                                };
                                db.SdTrans.Add(withdrawData);

                                sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;

                                sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                                db.SaveChanges();
                                Log.Information("/Success");

                                Message.Message sms = new Message.Message();
                                string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                                sms.SendSms("Withdrawal", WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateTime.Now, sdMasterData.DepositAmt), db);


                                string _clientID = Guid.NewGuid().ToString();

                                // Console.WriteLine("Client Ready");

                                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");
                                SubledgerRequest _subLedgerRequest = new SubledgerRequest();

                                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                                _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                                _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                                _subLedgerRequest.Header.Subject = "Withdrawal"; //Subject;
                                _subLedgerRequest.DocumentDetail.FirmId = WithdrawalDetails.FirmId; //FirmId;
                                _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                                _subLedgerRequest.DocumentDetail.ModuleId = WithdrawalDetails.ModuleId;
                                _subLedgerRequest.DocumentDetail.DocumentId = WithdrawalDetails.DepositId;
                                _subLedgerRequest.DocumentDetail.CustomerId = sdMasterData.CustId;
                                _subLedgerRequest.DocumentDetail.CustomerName = sdMasterData.CustName;
                                _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;

                                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                                _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + withdrawData.Descr;
                                if (cacheDetails.BranchId == sdMasterData.BranchId)
                                {
                                    var accountEntry1 = new AcountingEntry
                                    {
                                        ModuleId = 4, //(short)cacheDetails.ModuleId,
                                        AccountNo = 40500,//33000
                                        Type = EntryTypes.D,
                                        SubAccountNo = 0,
                                        Amount = withdrawData.Amount,
                                        TransactionMode = TransactionModes.CS,
                                        Description = withdrawData.Descr,
                                        ContraNo = 33000,//40500
                                        SegmentId = 1,
                                        BranchId = WithdrawalDetails.BranchId,
                                        ReferenceId = withdrawData.DepositId,
                                    };
                                    var accountEntry2 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 33000,//33000
                                        Type = EntryTypes.C,
                                        SubAccountNo = 0,
                                        Amount = withdrawData.Amount,
                                        TransactionMode = TransactionModes.CS,
                                        Description = withdrawData.Descr,
                                        ContraNo = 40500,
                                        SegmentId = 1,
                                        BranchId = WithdrawalDetails.BranchId,
                                        ReferenceId = withdrawData.DepositId,

                                    };

                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                                }
                                else


                                {
                                    var accountEntry3 = new AcountingEntry
                                    {
                                        ModuleId = 4, //(short)cacheDetails.ModuleId,
                                        AccountNo = 40500,//33000
                                        Type = EntryTypes.D,
                                        SubAccountNo = 0,
                                        Amount = withdrawData.Amount,
                                        TransactionMode = TransactionModes.CS,
                                        Description = withdrawData.Descr,
                                        ContraNo = 31003,//40500
                                        SegmentId = 1,
                                        BranchId = sdMasterData.BranchId,
                                        ReferenceId = withdrawData.DepositId,
                                    };
                                    var accountEntry4 = new AcountingEntry
                                    {
                                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                                        AccountNo = 31003,//33000
                                        Type = EntryTypes.C,
                                        SubAccountNo = 0,
                                        Amount = withdrawData.Amount,
                                        TransactionMode = TransactionModes.CS,
                                        Description = withdrawData.Descr,
                                        ContraNo = 40500,
                                        SegmentId = 1,
                                        BranchId = sdMasterData.BranchId,
                                        ReferenceId = withdrawData.DepositId,

                                    };


                                    var accountEntry5 = new AcountingEntry
                                    {
                                        ModuleId = 1,// (short)cacheDetails.ModuleId,
                                        AccountNo = 31003,//33000
                                        Type = EntryTypes.D,
                                        SubAccountNo = 0,
                                        Amount = withdrawData.Amount,
                                        TransactionMode = TransactionModes.CS,
                                        Description = withdrawData.Descr,
                                        ContraNo = 33000,//40500
                                        SegmentId = 1,
                                        //branchid must insert
                                        BranchId = WithdrawalDetails.BranchId,
                                        ReferenceId = withdrawData.DepositId,

                                    };


                                    var accountEntry6 = new AcountingEntry
                                    {
                                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                                        AccountNo = 33000,//33000
                                        Type = EntryTypes.C,
                                        SubAccountNo = 0,
                                        Amount = withdrawData.Amount,
                                        TransactionMode = TransactionModes.CS,
                                        Description = withdrawData.Descr,
                                        ContraNo = 31003,//40500
                                        SegmentId = 1,
                                        BranchId = WithdrawalDetails.BranchId,
                                        ReferenceId = withdrawData.DepositId,
                                    };
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                                }
                                _subLedgerRequest.AccountingVoucher.Amount = withdrawData.Amount;
                                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CASH;
                                //_subLedgerRequest.AccountingVoucher.
                                string transactionMessage = _subLedgerRequest.ToString();



                                _subLedgerClient.PostMessage(transactionMessage);
                                Log.Information("/accounts success");
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 200;
                                var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "cash", transId = withdrawData.TransId });
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                return _Response;



                            }
                            else
                            {
                                Log.Error("This amount is less than of zero");
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 404;
                                var Jsonstring = JsonSerializer.Serialize(new { status = "This amount is less than of zero" });
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                return _Response;
                                //return Results.NotFound(new { status = "This amount is less than of zero" });
                            }
                        }
                        else
                        {
                            Log.Error("This amount is exceeded");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(new { status = "This amount is reached" });
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            return _Response;
                            // return Results.NotFound(new { status = "This amount is reached" });
                        }

                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                        // return Results.NotFound(new { status = "There is no account" });
                    }
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "This method is not applicable" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                    // return Results.NotFound(new { status = "This method is not applicable" });
                }
            }

            catch (Exception ex)
            {
                
                Log.Error("ex.Message");
                throw new Exception(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }

        }
        public ResponseData SaveBank(ModelContext db)
        {
            try
            {
                var bankCommission = db.PaymentgatewayMasters.FirstOrDefault(x => x.PaymentType.ToUpper() == "BANK");
                if (bankCommission.PaymentgatewayCommission == null)
                    bankCommission.PaymentgatewayCommission = 0;

                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                decimal bankCharges = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9;
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var transId = (int)dep.getSequence(db, WithdrawalDetails.FirmId, (short)cacheDetails.BranchId, WithdrawalDetails.ModuleId, 4);

                if (WithdrawalDetails.StartDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") &&   //bank
   WithdrawalDetails.EndDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") && WithdrawalDetails.NoOfOccurence == 1)
                {
                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    //var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    if (sdMasterData.DepositAmt - (WithdrawalDetails.Amount + bankCharges) < 100)
                    {
                        Log.Error("This Amount is exeeded");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "This amount is exeeded " });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                        //  return Results.NotFound(new { status = "This amount is exeeded " });
                    }
                    if (sdMasterData != null)
                    {
                        if (sdMasterData.DepositAmt - WithdrawalDetails.Amount > 0)
                        {
                            var withdrawData = new SdTran
                            {
                                FirmId = WithdrawalDetails.FirmId,
                                BranchId = (short)cacheDetails.BranchId,
                                DepositId = WithdrawalDetails.DepositId,
                                TransId = transId,
                                TransNo = transNo,
                                TraDt = DateFunctions.sysdate(db),
                                Descr = "ONLINE SD WITHDRAWAL  - " + WithdrawalDetails.DepositId,
                                Amount = WithdrawalDetails.Amount,
                                Type = "D",
                                AccountNo = 40500,
                                ContraNo = 40602,
                                ValueDt = DateFunctions.sysdate(db),
                                VouchId = 0,
                            };
                            var Commission = new SdTran
                            {
                                FirmId = WithdrawalDetails.FirmId,
                                BranchId = WithdrawalDetails.BranchId,
                                DepositId = WithdrawalDetails.DepositId,
                                TransId = transId,
                                TransNo = transNo,
                                TraDt = DateFunctions.sysdate(db),
                                Descr = "ONLINE SD WITHDRAWAL CHARGE - " + WithdrawalDetails.DepositId,
                                Amount = bankCharges,
                                Type = "D",
                                AccountNo = 40500,
                                ContraNo = 40602,
                                ValueDt = DateFunctions.sysdate(db),
                                VouchId = 0,
                            };

                            db.SdTrans.Add(withdrawData);
                            db.SdTrans.Add(Commission);



                            sdMasterData.DepositAmt = sdMasterData.DepositAmt - (WithdrawalDetails.Amount + bankCharges);
                            sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                            db.SaveChanges();
                            Log.Information("/Success");

                            Message.Message sms = new Message.Message();
                            string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                            if (phone != "1111111111")
                            {
                                sms.SendSms("Withdrawal", WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone,
                            String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);
                            }



                            string _clientID = Guid.NewGuid().ToString();

                            // Console.WriteLine("Client Ready");

                            SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");
                            SubledgerRequest _subLedgerRequest = new SubledgerRequest();

                            _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                            _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                            _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                            _subLedgerRequest.Header.Subject = "Withdrawal"; //Subject;
                            _subLedgerRequest.DocumentDetail.FirmId = WithdrawalDetails.FirmId; //FirmId;
                            _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                            _subLedgerRequest.DocumentDetail.ModuleId = WithdrawalDetails.ModuleId;
                            _subLedgerRequest.DocumentDetail.DocumentId = WithdrawalDetails.DepositId;
                            _subLedgerRequest.DocumentDetail.CustomerId = sdMasterData.CustId;
                            _subLedgerRequest.DocumentDetail.CustomerName = sdMasterData.CustName;
                            _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                            // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                            // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                            _subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = (short)_withdrawalDetails.ModuleId;
                            _subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = _withdrawalDetails.AccountNumber;
                            _subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = _withdrawalDetails.Ifsc;
                            _subLedgerRequest.AccountingVoucher.TransferDetail.Type = OutwardTransferTypes.Bank;
                            _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                            _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + withdrawData.Descr;

                            var accountEntry1 = new AcountingEntry
                            {
                                ModuleId = 4,//(short)cacheDetails.ModuleId,
                                AccountNo = 40500,
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 31003,
                                SegmentId = 1,
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };
                            var accountEntry2 = new AcountingEntry
                            {
                                ModuleId = 1,// (short)cacheDetails.ModuleId,
                                AccountNo = 31003,
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 40500,
                                SegmentId = 1,
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };

                            var accountEntry3 = new AcountingEntry
                            {
                                ModuleId = 4,// (short)cacheDetails.ModuleId,
                                AccountNo = 40500,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                                TransactionMode = TransactionModes.TR,
                                Description = Commission.Descr,
                                ContraNo = 31003,//40500
                                SegmentId = 1,
                                //branchid must insert
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };


                            var accountEntry4 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                                TransactionMode = TransactionModes.TR,
                                Description = Commission.Descr,
                                ContraNo = 40500,
                                SegmentId = 1,
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };

                            var accountEntry5 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 40602,
                                SegmentId = 1,
                                BranchId = 0,
                                ReferenceId = withdrawData.DepositId,
                            };
                            var accountEntry6 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 40602,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 31003,
                                SegmentId = 1,
                                BranchId = 0,
                                ReferenceId = withdrawData.DepositId,
                            };
                            var accountEntry7 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                                TransactionMode = TransactionModes.TR,
                                Description = Commission.Descr,
                                ContraNo = 40602,
                                SegmentId = 1,
                                BranchId = 0,
                                ReferenceId = withdrawData.DepositId,
                            };
                            var accountEntry8 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 40602,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                                TransactionMode = TransactionModes.TR,
                                Description = Commission.Descr,
                                ContraNo = 31003,
                                SegmentId = 1,
                                BranchId = 0,
                                ReferenceId = withdrawData.DepositId,
                            };


                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);

                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry7);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry8);

                            _subLedgerRequest.AccountingVoucher.Amount = withdrawData.Amount;
                            _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                            _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                            _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                            _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.Bank;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay

                            string transactionMessage = _subLedgerRequest.ToString();



                            _subLedgerClient.PostMessage(transactionMessage);

                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = WithdrawalDetails.TransactionMethod, transId = withdrawData.TransId });
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            return _Response;
                        }
                        else
                        {
                            Log.Error("This amount is smaller than the minimum amount");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(new { status = "This amount is smaller than the minimum amount" });
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            return _Response;
                        }
                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else if (DateTime.Parse(WithdrawalDetails.StartDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&  //bank
        DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 0)// && frquency.ToLower() == "day")
                {


                    var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                    var sheduleData = new SdScheduleMaster
                    {
                        RtId = rtId + 1,
                        FirmId = WithdrawalDetails.FirmId,
                        BranchId = WithdrawalDetails.BranchId,
                        ModuleId = WithdrawalDetails.ModuleId,
                        DepositId = WithdrawalDetails.DepositId,
                        ScheduledDate = DateFunctions.sysdate(db),
                        StartDate = DateTime.Parse(WithdrawalDetails.StartDate),
                        CloseDate = DateTime.Parse(WithdrawalDetails.EndDate),
                        NoOccurance = (byte)WithdrawalDetails.NoOfOccurence,
                        Amount = WithdrawalDetails.Amount,
                        Type = "D",
                        Frequency = WithdrawalDetails.Frequency.ToUpper(),
                        StatusId = 1,
                        Ifsc = WithdrawalDetails.Ifsc,
                        AccountNumber = WithdrawalDetails.AccountNumber,
                        UserType = WithdrawalDetails.UserType,
                        UserId = WithdrawalDetails.UserId,
                    };
                    for (int i = 0; i < sheduleData.NoOccurance; i++)
                    {
                        var scheduleDetail = new SdScheduleTran
                        {
                            RtId = (decimal)sheduleData.RtId,
                            FirmId = (byte)sheduleData.FirmId,
                            BranchId = (short)sheduleData.BranchId,
                            ModuleId = (byte)sheduleData.ModuleId,
                            DepositId = sheduleData.DepositId,
                            TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(sheduleData.StartDate).AddMonths(i)
                            : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(sheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(sheduleData.StartDate).AddDays(i),
                            Amount = Convert.ToDecimal(sheduleData.Amount),
                            Type = sheduleData.Type,
                            StatusId = (byte)sheduleData.StatusId,
                            Ifsc = sheduleData.Ifsc,
                            AccountNumber = sheduleData.AccountNumber,
                            UserType = (int)sheduleData.UserType,
                            UserId = sheduleData.UserId,
                            TransId = 0,

                        };
                        db.SdScheduleTrans.Add(scheduleDetail);
                    }
                    db.SdScheduleMasters.Add(sheduleData);
                    db.SaveChangesAsync();
                    Log.Information("/Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = WithdrawalDetails.Ifsc != null ? "BANK" : "OTHER", transId = sheduleData.RtId });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
                else if (DateTime.Parse(WithdrawalDetails.StartDate) == DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&   //bank
                DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 1)

                {



                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    // var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    if (sdMasterData != null)
                    {
                        if (sdMasterData.DepositAmt - (WithdrawalDetails.Amount + bankCharges) < 100)
                        {
                            Log.Error("This Amount is exeeded");
                            ResponseData _response = new ResponseData();
                            _response.responseCode = 404;
                            var JsonString = JsonSerializer.Serialize(new { status = "This amount is exeeded" });
                            _response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                            return _response;
                        }
                        var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                        var scheduleData = new SdScheduleMaster
                        {
                            RtId = rtId + 1,
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            ModuleId = WithdrawalDetails.ModuleId,
                            DepositId = WithdrawalDetails.DepositId,
                            ScheduledDate = DateFunctions.sysdate(db),
                            StartDate = DateTime.Parse(WithdrawalDetails.StartDate),
                            CloseDate = DateTime.Parse(WithdrawalDetails.EndDate),
                            NoOccurance = (byte?)WithdrawalDetails.NoOfOccurence,
                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            Frequency = WithdrawalDetails.Frequency.ToUpper(),
                            StatusId = 1,
                            Ifsc = WithdrawalDetails.Ifsc,
                            AccountNumber = WithdrawalDetails.AccountNumber,
                            UserType = WithdrawalDetails.UserType,
                            UserId = WithdrawalDetails.UserId,
                        };

                        for (int i = 1; i < scheduleData.NoOccurance; i++)
                        {
                            var scheduleDetail = new SdScheduleTran
                            {
                                RtId = (decimal)scheduleData.RtId,
                                FirmId = (byte)scheduleData.FirmId,
                                BranchId = (short)scheduleData.BranchId,
                                ModuleId = (byte)scheduleData.ModuleId,
                                DepositId = scheduleData.DepositId,
                                TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(scheduleData.StartDate).AddMonths(i)
                                : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(scheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(scheduleData.StartDate).AddDays(i),
                                Amount = Convert.ToDecimal(scheduleData.Amount),
                                Type = scheduleData.Type,
                                StatusId = (byte)scheduleData.StatusId,
                                Ifsc = scheduleData.Ifsc,
                                AccountNumber = scheduleData.AccountNumber,
                                UserType = (int)scheduleData.UserType,
                                UserId = scheduleData.UserId,
                                TransId = 0,

                            };
                            db.SdScheduleTrans.Add(scheduleDetail);
                        }
                        db.SdScheduleMasters.Add(scheduleData);

                        var withdrawData = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = 0,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "BY ACC TRANSFER" + " - " + WithdrawalDetails.DepositId,

                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            AccountNo = 40500,
                            ContraNo = 32100,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };

                        var Commission = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = 0,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "BY ACC TRANSFER" + " - " + WithdrawalDetails.DepositId,
                            Amount = bankCharges,
                            Type = "D",
                            AccountNo = 40500,
                            ContraNo = 32100,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };
                        db.SdTrans.Add(withdrawData);

                        sdMasterData.DepositAmt = sdMasterData.DepositAmt - (WithdrawalDetails.Amount + bankCharges);
                        sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                        db.SaveChanges();
                        Log.Information("/Success");

                        Message.Message sms = new Message.Message();
                        string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                        if (phone != "1111111111")
                        {
                            sms.SendSms("Withdrawal", (byte)WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);

                        }

                        string _clientID = Guid.NewGuid().ToString();

                        // Console.WriteLine("Client Ready");

                        SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");
                        SubledgerRequest _subLedgerRequest = new SubledgerRequest();

                        _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                        _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                        _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                        _subLedgerRequest.Header.Subject = "Withdrawal"; //Subject;
                        _subLedgerRequest.DocumentDetail.FirmId = WithdrawalDetails.FirmId; //FirmId;
                        _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                        _subLedgerRequest.DocumentDetail.ModuleId = WithdrawalDetails.ModuleId;
                        _subLedgerRequest.DocumentDetail.DocumentId = WithdrawalDetails.DepositId;
                        _subLedgerRequest.DocumentDetail.CustomerId = sdMasterData.CustId;
                        _subLedgerRequest.DocumentDetail.CustomerName = sdMasterData.CustName;
                        _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                        // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                        // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                        //_subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate =  Data.RealizationDate;//ChequeDate;
                        //_subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = Data.ChequNo;
                        //_subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = BankBranchId;
                        //_subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = BankBranch;
                        _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                        _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + withdrawData.Descr;

                        var accountEntry1 = new AcountingEntry
                        {
                            ModuleId = 4,//(short)cacheDetails.ModuleId,
                            AccountNo = 40500,
                            SubAccountNo = 0,
                            Type = EntryTypes.D,
                            Amount = withdrawData.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 31003,
                            SegmentId = 1,
                            BranchId = WithdrawalDetails.BranchId,
                            ReferenceId = withdrawData.DepositId,
                        };
                        var accountEntry2 = new AcountingEntry
                        {
                            ModuleId = 1,// (short)cacheDetails.ModuleId,
                            AccountNo = 31003,
                            SubAccountNo = 0,
                            Type = EntryTypes.C,
                            Amount = withdrawData.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 40500,
                            SegmentId = 1,
                            BranchId = WithdrawalDetails.BranchId,
                            ReferenceId = withdrawData.DepositId,
                        };

                        var accountEntry3 = new AcountingEntry
                        {
                            ModuleId = 4,// (short)cacheDetails.ModuleId,
                            AccountNo = 40500,//33000
                            SubAccountNo = 0,
                            Type = EntryTypes.D,
                            Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 31003,//40500
                            SegmentId = 1,
                            //branchid must insert
                            BranchId = WithdrawalDetails.BranchId,
                            ReferenceId = withdrawData.DepositId,
                        };


                        var accountEntry4 = new AcountingEntry
                        {
                            ModuleId = 1, // (short)cacheDetails.ModuleId,
                            AccountNo = 31003,//33000
                            SubAccountNo = 0,
                            Type = EntryTypes.C,
                            Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 40500,
                            SegmentId = 1,
                            BranchId = WithdrawalDetails.BranchId,
                            ReferenceId = withdrawData.DepositId,
                        };

                        var accountEntry5 = new AcountingEntry
                        {
                            ModuleId = 1, // (short)cacheDetails.ModuleId,
                            AccountNo = 31003,//33000
                            SubAccountNo = 0,
                            Type = EntryTypes.D,
                            Amount = withdrawData.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 40602,
                            SegmentId = 1,
                            BranchId = 0,
                            ReferenceId = withdrawData.DepositId,
                        };
                        var accountEntry6 = new AcountingEntry
                        {
                            ModuleId = 1, // (short)cacheDetails.ModuleId,
                            AccountNo = 40602,//33000
                            SubAccountNo = 0,
                            Type = EntryTypes.C,
                            Amount = withdrawData.Amount,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 31003,
                            SegmentId = 1,
                            BranchId = 0,
                            ReferenceId = withdrawData.DepositId,
                        };
                        var accountEntry7 = new AcountingEntry
                        {
                            ModuleId = 1, // (short)cacheDetails.ModuleId,
                            AccountNo = 31003,//33000
                            SubAccountNo = 0,
                            Type = EntryTypes.D,
                            Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 40602,
                            SegmentId = 1,
                            BranchId = 0,
                            ReferenceId = withdrawData.DepositId,
                        };
                        var accountEntry8 = new AcountingEntry
                        {
                            ModuleId = 1, // (short)cacheDetails.ModuleId,
                            AccountNo = 40602,//33000
                            Type = EntryTypes.C,
                            Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                            TransactionMode = TransactionModes.TR,
                            Description = withdrawData.Descr,
                            ContraNo = 31003,
                            SegmentId = 1,
                            BranchId = 0,
                            ReferenceId = withdrawData.DepositId,
                        };

                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                        if (sdMasterData.BranchId != WithdrawalDetails.BranchId)
                        {
                            var accountEntry9 = new AcountingEntry
                            {
                                ModuleId = 4,//(short)cacheDetails.ModuleId,
                                AccountNo = 40500,
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 31003,
                                SegmentId = 1,
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };
                            var accountEntry10 = new AcountingEntry
                            {
                                ModuleId = 1,// (short)cacheDetails.ModuleId,
                                AccountNo = 31003,
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 40500,
                                SegmentId = 1,
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };
                            var accountEntry11 = new AcountingEntry
                            {
                                ModuleId = 4,// (short)cacheDetails.ModuleId,
                                AccountNo = 40500,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 31003,//40500
                                SegmentId = 1,
                                //branchid must insert
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };


                            var accountEntry12 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 31003,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = (decimal)bankCommission.PaymentgatewayCommission + (decimal)0.9,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 40500,
                                SegmentId = 1,
                                BranchId = sdMasterData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };

                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry9);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry10);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry11);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry12);
                        }
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry7);
                        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry8);


                        _subLedgerRequest.AccountingVoucher.Amount = withdrawData.Amount;
                        _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                        _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                        _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                        _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.Bank;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay

                        string transactionMessage = _subLedgerRequest.ToString();



                        _subLedgerClient.PostMessage(transactionMessage);

                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = WithdrawalDetails.TransactionMethod, tranId = withdrawData.TransId });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "This method is not applicable" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
            }
            catch (Exception ex)
            {
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }

        }
        public ResponseData SaveSd(ModelContext db)
        {
            try
            {

                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var transId = (int)dep.getSequence(db, WithdrawalDetails.FirmId, (short)cacheDetails.BranchId, (byte)WithdrawalDetails.ModuleId, 4);
                if (WithdrawalDetails.StartDate== DateFunctions.sysdate(db).ToString("yyyy-MM-dd") &&  //sd
                 WithdrawalDetails.EndDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") && WithdrawalDetails.NoOfOccurence == 1)
                {
                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    //var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    var sdMasterData1 = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.AccountNumber).SingleOrDefault();
                    if (sdMasterData != null)
                    {
                        if (sdMasterData1 != null)
                        {

                            if (sdMasterData.DepositAmt - WithdrawalDetails.Amount > 0)
                            {
                                var withdrawDataFrom = new SdTran
                                {
                                    FirmId = WithdrawalDetails.FirmId,
                                    BranchId = (short)cacheDetails.BranchId,  //sdmaster.branchid
                                    DepositId = WithdrawalDetails.DepositId,
                                    TransId = transId,
                                    TransNo = transNo,
                                    TraDt = DateFunctions.sysdate(db),
                                    Descr = "TFR SD ACCOUNT TO" + " - " + WithdrawalDetails.AccountNumber,
                                    Amount = WithdrawalDetails.Amount,
                                    Type = "D",
                                    AccountNo = 40500,
                                    ContraNo = 40500,
                                    ValueDt = DateFunctions.sysdate(db),
                                    VouchId = 0,
                                };
                                db.SdTrans.Add(withdrawDataFrom);
                                sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;
                                sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                                //  db.SaveChanges();
                                Message.Message sms = new Message.Message();

                                string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);

                                if (phone != "1111111111")
                                {
                                    sms.SendSms("Withdrawal", WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);
                                }

                                var withdrawDataTo = new SdTran
                                {
                                    FirmId = sdMasterData1.FirmId,
                                    BranchId = sdMasterData1.BranchId,
                                    DepositId = WithdrawalDetails.AccountNumber,
                                    TransId = transId,
                                    TransNo = transNo,
                                    TraDt = DateFunctions.sysdate(db),
                                    Descr = "TFR FROM SD ACCOUNT" + " - " + WithdrawalDetails.DepositId,

                                    Amount = WithdrawalDetails.Amount,
                                    Type = "C",
                                    AccountNo = 40500,
                                    ContraNo = 40500,
                                    ValueDt = DateFunctions.sysdate(db),
                                    VouchId = 0,
                                };
                                db.SdTrans.Add(withdrawDataTo);
                                sdMasterData1.DepositAmt = sdMasterData1.DepositAmt + WithdrawalDetails.Amount;
                                sdMasterData1.TrancationDate = DateFunctions.sysdate(db);

                                db.SaveChanges();


                                Log.Information("Success");
                                //string Phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                                //sms.SendSms("Withdrawal", WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", Phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt),db);

                                string _clientID = Guid.NewGuid().ToString();

                                // Console.WriteLine("Client Ready");

                                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");
                                SubledgerRequest _subLedgerRequest = new SubledgerRequest();

                                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                                _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                                _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                                _subLedgerRequest.Header.Subject = "Withdrawal"; //Subject;
                                _subLedgerRequest.DocumentDetail.FirmId = WithdrawalDetails.FirmId; //FirmId;
                                _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                                _subLedgerRequest.DocumentDetail.ModuleId = WithdrawalDetails.ModuleId;
                                _subLedgerRequest.DocumentDetail.DocumentId = WithdrawalDetails.DepositId;
                                _subLedgerRequest.DocumentDetail.CustomerId = sdMasterData.CustId;
                                _subLedgerRequest.DocumentDetail.CustomerName = sdMasterData.CustName;
                                _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                                _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + withdrawDataFrom.Descr;
                                if (sdMasterData.BranchId == sdMasterData1.BranchId)
                                {
                                    var accountEntry1 = new AcountingEntry
                                    {
                                        ModuleId = 4,//(short)cacheDetails.ModuleId,
                                        AccountNo = 40500,//33000
                                        SubAccountNo = 0,
                                        Type = EntryTypes.D,
                                        Amount = withdrawDataFrom.Amount,
                                        TransactionMode = TransactionModes.TR,
                                        Description = withdrawDataFrom.Descr,
                                        ContraNo = 40500,//40500
                                        SegmentId = 1,
                                        BranchId = withdrawDataFrom.BranchId,
                                        ReferenceId = withdrawDataFrom.DepositId,
                                    };
                                    var accountEntry2 = new AcountingEntry
                                    {
                                        ModuleId = 4,
                                        AccountNo = 40500,
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = withdrawDataTo.Amount,
                                        TransactionMode = TransactionModes.TR,
                                        Description = withdrawDataTo.Descr,
                                        ContraNo = 40500,
                                        SegmentId = 1,
                                        //branchid must insert
                                        BranchId = withdrawDataTo.BranchId,
                                        ReferenceId = withdrawDataTo.DepositId,
                                    };
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);

                                }
                                else
                                {
                                    var accountEntry1 = new AcountingEntry
                                    {
                                        ModuleId = 4,// (short)cacheDetails.ModuleId,
                                        AccountNo = 40500,
                                        SubAccountNo = 0,
                                        Type = EntryTypes.D,
                                        Amount = withdrawDataFrom.Amount,
                                        TransactionMode = TransactionModes.TR,
                                        Description = withdrawDataFrom.Descr,
                                        ContraNo = 31003,
                                        SegmentId = 1,
                                        //branchid must insert
                                        BranchId = sdMasterData.BranchId,
                                        ReferenceId = withdrawDataFrom.DepositId,
                                    };
                                    var accountEntry2 = new AcountingEntry
                                    {
                                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                                        AccountNo = 31003,
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = withdrawDataFrom.Amount,
                                        TransactionMode = TransactionModes.TR,
                                        Description = withdrawDataFrom.Descr,
                                        ContraNo = 40500,//40500
                                        SegmentId = 1,
                                        BranchId = sdMasterData.BranchId,
                                        ReferenceId = withdrawDataFrom.DepositId,

                                    };
                                    var accountEntry3 = new AcountingEntry
                                    {
                                        ModuleId = 1, // (short)cacheDetails.ModuleId,
                                        AccountNo = 31003,
                                        SubAccountNo = 0,
                                        Type = EntryTypes.D,
                                        Amount = withdrawDataTo.Amount,
                                        TransactionMode = TransactionModes.TR,
                                        Description = withdrawDataTo.Descr,
                                        ContraNo = 40500,//40500
                                        SegmentId = 1,
                                        BranchId = sdMasterData1.BranchId,
                                        ReferenceId = withdrawDataTo.DepositId,

                                    };
                                    var accountEntry4 = new AcountingEntry
                                    {
                                        ModuleId = 4, // (short)cacheDetails.ModuleId,
                                        AccountNo = 40500,
                                        SubAccountNo = 0,
                                        Type = EntryTypes.C,
                                        Amount = withdrawDataTo.Amount,
                                        TransactionMode = TransactionModes.TR,
                                        Description = withdrawDataTo.Descr,
                                        ContraNo = 31003,
                                        SegmentId = 1,
                                        BranchId = sdMasterData1.BranchId,
                                        ReferenceId = withdrawDataTo.DepositId,
                                    };
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                                }

                                _subLedgerRequest.AccountingVoucher.Amount = withdrawDataFrom.Amount;
                                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;// Convert.ToInt64(withdrawDataFrom.DepositId);
                                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.OwnAccount;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay

                                string transactionMessage = _subLedgerRequest.ToString();



                                _subLedgerClient.PostMessage(transactionMessage);
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 200;
                                var Jsonstring = JsonSerializer.Serialize(new { status = "success", type = WithdrawalDetails.TransactionMethod, transId = withdrawDataFrom.TransId });
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                return _Response;
                            }
                            else
                            {
                                Log.Error("This amount is smaller than the minimum amount");
                                ResponseData _Response = new ResponseData();
                                _Response.responseCode = 404;
                                var Jsonstring = JsonSerializer.Serialize(new { status = "This amount is smaller than the minimum amount" });
                                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                                return _Response;
                            }
                        }
                        else
                        {
                            Log.Error("There is no account");
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            return _Response;
                        }
                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else if (DateTime.Parse(WithdrawalDetails.StartDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&  //sd
       DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 0)// && frquency.ToLower() == "day")
                {
                    var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                    var sheduleData = new SdScheduleMaster
                    {
                        RtId = rtId + 1,
                        FirmId = WithdrawalDetails.FirmId,
                        BranchId = WithdrawalDetails.BranchId,
                        ModuleId = WithdrawalDetails.ModuleId,
                        DepositId = WithdrawalDetails.DepositId,
                        ScheduledDate = DateFunctions.sysdate(db),
                        StartDate = DateTime.Parse(WithdrawalDetails.StartDate),
                        CloseDate = DateTime.Parse(WithdrawalDetails.EndDate),
                        NoOccurance = (byte?)WithdrawalDetails.NoOfOccurence,
                        Amount = WithdrawalDetails.Amount,
                        Type = "D",
                        Frequency = WithdrawalDetails.Frequency.ToUpper(),
                        StatusId = 1,
                        Ifsc = WithdrawalDetails.Ifsc,
                        AccountNumber = WithdrawalDetails.AccountNumber,
                        UserType = WithdrawalDetails.UserType,
                        UserId = WithdrawalDetails.UserId,
                    };
                    for (int i = 0; i < sheduleData.NoOccurance; i++)
                    {
                        var scheduleDetail = new SdScheduleTran
                        {
                            RtId = (decimal)sheduleData.RtId,
                            FirmId = (byte)sheduleData.FirmId,
                            BranchId = (short)sheduleData.BranchId,
                            ModuleId = (byte)sheduleData.ModuleId,
                            DepositId = sheduleData.DepositId,
                            TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(sheduleData.StartDate).AddMonths(i)
                            : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(sheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(sheduleData.StartDate).AddDays(i),
                            Amount = Convert.ToDecimal(sheduleData.Amount),
                            Type = sheduleData.Type,
                            StatusId = (byte)sheduleData.StatusId,
                            Ifsc = sheduleData.Ifsc,
                            AccountNumber = sheduleData.AccountNumber,
                            UserType = (int)sheduleData.UserType,
                            UserId = sheduleData.UserId,
                            TransId = 0,

                        };
                        db.SdScheduleTrans.Add(scheduleDetail);
                    }
                    db.SdScheduleMasters.Add(sheduleData);
                    db.SaveChanges();
                    Log.Information("/Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "success", type = WithdrawalDetails.Ifsc != null ? "" : "OTHER", transId = sheduleData.RtId });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }

                else if (DateTime.Parse(WithdrawalDetails.StartDate) == DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&   //sd
               DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 1)

                {
                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    // var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    var sdMasterData1 = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.AccountNumber).SingleOrDefault();
                    if (sdMasterData != null)
                    {

                        var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                        var scheduleData = new SdScheduleMaster
                        {
                            RtId = rtId + 1,
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            ModuleId = WithdrawalDetails.ModuleId,
                            DepositId = WithdrawalDetails.DepositId,
                            ScheduledDate = DateFunctions.sysdate(db),
                            StartDate = DateTime.Parse(WithdrawalDetails.StartDate),
                            CloseDate = DateTime.Parse(WithdrawalDetails.EndDate),
                            NoOccurance = (byte?)WithdrawalDetails.NoOfOccurence,
                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            Frequency = WithdrawalDetails.Frequency.ToUpper(),
                            StatusId = 1,
                            Ifsc = WithdrawalDetails.Ifsc,
                            AccountNumber = WithdrawalDetails.AccountNumber,
                            UserType = WithdrawalDetails.UserType,
                            UserId = WithdrawalDetails.UserId,
                        };

                        for (int i = 1; i < scheduleData.NoOccurance; i++)
                        {
                            var scheduleDetail = new SdScheduleTran
                            {
                                RtId = (decimal)scheduleData.RtId,
                                FirmId = (byte)scheduleData.FirmId,
                                BranchId = (short)scheduleData.BranchId,
                                ModuleId = (byte)scheduleData.ModuleId,
                                DepositId = scheduleData.DepositId,
                                TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(scheduleData.StartDate).AddMonths(i)
                                : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(scheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(scheduleData.StartDate).AddDays(i),
                                Amount = Convert.ToDecimal(scheduleData.Amount),
                                Type = scheduleData.Type,
                                StatusId = (byte)scheduleData.StatusId,
                                Ifsc = scheduleData.Ifsc,
                                AccountNumber = scheduleData.AccountNumber,
                                UserType = (int)scheduleData.UserType,
                                UserId = scheduleData.UserId,
                                TransId = 0,

                            };
                            db.SdScheduleTrans.Add(scheduleDetail);
                        }


                        db.SdScheduleMasters.Add(scheduleData);





                        db.SaveChanges();
                        Message.Message sms = new Message.Message();
                        string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                        sms.SendSms("Withdrawal", (byte)WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);


                        var withdrawData = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = 0,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "TFR TO SD TO" + " - " + WithdrawalDetails.DepositId,

                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            AccountNo = 40500,
                            ContraNo = 10300,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };

                        var withdrawData1 = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = 0,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "TFR TO SD FROM" + " - " + WithdrawalDetails.DepositId,

                            Amount = WithdrawalDetails.Amount,
                            Type = "C",
                            AccountNo = 40500,
                            ContraNo = 10300,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };
                        db.SdTrans.Add(withdrawData);
                        db.SdTrans.Add(withdrawData1);
                        sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;
                        sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                        sdMasterData1.DepositAmt = sdMasterData1.DepositAmt + WithdrawalDetails.Amount;
                        sdMasterData1.TrancationDate = DateFunctions.sysdate(db);
                        db.SaveChanges();
                        Log.Information("/Success");
                        ////   sms.SendSms("Withdrawal", (byte)WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, "", "MABDEP", "8137047123", String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt));


                        string _clientID = Guid.NewGuid().ToString();

                        // Console.WriteLine("Client Ready");

                        SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");
                        SubledgerRequest _subLedgerRequest = new SubledgerRequest();

                        _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                        _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                        _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                        _subLedgerRequest.Header.Subject = "Withdrawal"; //Subject;
                        _subLedgerRequest.DocumentDetail.FirmId = WithdrawalDetails.FirmId; //FirmId;
                        _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                        _subLedgerRequest.DocumentDetail.ModuleId = WithdrawalDetails.ModuleId;
                        _subLedgerRequest.DocumentDetail.DocumentId = WithdrawalDetails.DepositId;
                        _subLedgerRequest.DocumentDetail.CustomerId = sdMasterData.CustId;
                        _subLedgerRequest.DocumentDetail.CustomerName = sdMasterData.CustName;
                        _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                        _subLedgerRequest.AccountingVoucher.Narration = "BEING - " + withdrawData.Descr;
                        if (withdrawData.BranchId == withdrawData1.BranchId)
                        {
                            var accountEntry1 = new AcountingEntry
                            {
                                ModuleId = 4,//(short)cacheDetails.ModuleId,
                                AccountNo = 40500,//33000
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData.Descr,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                BranchId = withdrawData.BranchId,
                                ReferenceId = withdrawData.DepositId,

                            };
                            var accountEntry2 = new AcountingEntry
                            {
                                ModuleId = 1,
                                AccountNo = 40500,
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData1.Descr,
                                ContraNo = 40500,
                                SegmentId = 1,
                                //branchid must insert
                                BranchId = withdrawData.BranchId,
                                ReferenceId = withdrawData.DepositId,

                            };
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);

                        }
                        else
                        {
                            var accountEntry1 = new AcountingEntry
                            {
                                ModuleId = 4,// (short)cacheDetails.ModuleId,
                                AccountNo = 40500,
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData1.Descr,
                                ContraNo = 31003,
                                SegmentId = 1,
                                //branchid must insert
                                BranchId = withdrawData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };


                            var accountEntry2 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 31003,
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData1.Descr,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                BranchId = withdrawData.BranchId,
                                ReferenceId = withdrawData.DepositId,
                            };
                            var accountEntry3 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 31003,
                                SubAccountNo = 0,
                                Type = EntryTypes.D,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData1.Descr,
                                ContraNo = 40500,//40500
                                SegmentId = 1,
                                BranchId = withdrawData1.BranchId,
                                ReferenceId = withdrawData1.DepositId,
                            };
                            var accountEntry4 = new AcountingEntry
                            {
                                ModuleId = 1, // (short)cacheDetails.ModuleId,
                                AccountNo = 40500,
                                SubAccountNo = 0,
                                Type = EntryTypes.C,
                                Amount = withdrawData.Amount,
                                TransactionMode = TransactionModes.TR,
                                Description = withdrawData1.Descr,
                                ContraNo = 31003,
                                SegmentId = 1,
                                BranchId = withdrawData1.BranchId,
                                ReferenceId = withdrawData1.DepositId,
                            };

                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                            _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);

                        }

                        _subLedgerRequest.AccountingVoucher.Amount = withdrawData.Amount;
                        _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                        _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                        _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;  //Enum
                        _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.OwnAccount;  // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay

                        string transactionMessage = _subLedgerRequest.ToString();



                        _subLedgerClient.PostMessage(transactionMessage);
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "success", type = WithdrawalDetails.TransactionMethod, tranId = withdrawData.TransId });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "This method is not applicable" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }


            }
            catch (Exception ex)
            {
                Log.Error("ex.Message");


                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }
        }

        public ResponseData SaveRd(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var transId = (int)dep.getSequence(db, WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, (byte)WithdrawalDetails.ModuleId, 4);
                if (WithdrawalDetails.StartDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") &&  //sd
                 WithdrawalDetails.EndDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") && WithdrawalDetails.NoOfOccurence == 1)
                {
                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    //var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    // var RdDetails = db_acc.DepositMsts.FirstOrDefault(x => x.DocId == _withdrawalDetails.DepositId);
                    if (sdMasterData.DepositAmt - WithdrawalDetails.Amount > 0)
                    {
                        var withdrawDataFrom = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = sdMasterData.BranchId,  //sdmaster.branchid
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = transNo,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "TFR SD ACCOUNT TO" + " - " + WithdrawalDetails.AccountNumber,
                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            AccountNo = 40500,
                            ContraNo = 40504,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };
                        db.SdTrans.Add(withdrawDataFrom);
                        sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;
                        sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                        ////////////////////////////////////////////////////////////////////////////////



                        ApiHandler handler = new ApiHandler();
                        RdVrdData data = new RdVrdData();

                        var ifscCodeBase64 = EncryptAuthenticationTokenAes(WithdrawalDetails.DepositId, WithdrawalDetails.phoneNo, WithdrawalDetails.tfrsdno, WithdrawalDetails.tframt, WithdrawalDetails.odint, WithdrawalDetails.currinstno, WithdrawalDetails.statusAppWeb);

                        data.sdno = ifscCodeBase64;
                        data.phoneNo = ifscCodeBase64;
                        data.tfrsdno = ifscCodeBase64;
                        data.tframt = ifscCodeBase64;
                        data.odint = ifscCodeBase64;
                        data.currinstno = ifscCodeBase64;
                        data.statusAppWeb = ifscCodeBase64;

                        var response = handler.InvokeHttpClient<RdVrd>("getSdfundtfrRD_VRDPayment", data);




                        //////////////////////////////////////////////////////////////////////////////
                        db.SaveChanges();
                        Message.Message sms = new Message.Message();
                        string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                        if (phone != "1111111111")
                        {
                            sms.SendSms("Withdrawal", WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);
                        }
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success", transId = transId, type = "RD" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else if (DateTime.Parse(WithdrawalDetails.StartDate) == DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&
            DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 1)
                {
                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    // var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    if (sdMasterData != null)
                    {

                        var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                        var scheduleData = new SdScheduleMaster
                        {
                            RtId = rtId + 1,
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            ModuleId = WithdrawalDetails.ModuleId,
                            DepositId = WithdrawalDetails.DepositId,
                            ScheduledDate = DateFunctions.sysdate(db),
                            StartDate = DateTime.Parse( WithdrawalDetails.StartDate),
                            CloseDate = DateTime.Parse(WithdrawalDetails.EndDate),
                            NoOccurance = (byte?)WithdrawalDetails.NoOfOccurence,
                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            Frequency = WithdrawalDetails.Frequency.ToUpper(),
                            StatusId = 1,
                            Ifsc = WithdrawalDetails.Ifsc,
                            AccountNumber = WithdrawalDetails.AccountNumber,
                            UserType = WithdrawalDetails.UserType,
                            UserId = WithdrawalDetails.UserId,
                        };

                        for (int i = 1; i < scheduleData.NoOccurance; i++)
                        {
                            var scheduleDetail = new SdScheduleTran
                            {
                                RtId = (decimal)scheduleData.RtId,
                                FirmId = (byte)scheduleData.FirmId,
                                BranchId = (short)scheduleData.BranchId,
                                ModuleId = (byte)scheduleData.ModuleId,
                                DepositId = scheduleData.DepositId,
                                TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(scheduleData.StartDate).AddMonths(i)
                                : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(scheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(scheduleData.StartDate).AddDays(i),
                                Amount = Convert.ToDecimal(scheduleData.Amount),






                                Type = scheduleData.Type,
                                StatusId = (byte)scheduleData.StatusId,
                                Ifsc = scheduleData.Ifsc,
                                AccountNumber = scheduleData.AccountNumber,
                                UserType = (int)scheduleData.UserType,
                                UserId = scheduleData.UserId,
                                TransId = 0,

                            };
                            db.SdScheduleTrans.Add(scheduleDetail);
                        }


                        db.SdScheduleMasters.Add(scheduleData);

                        db.SaveChanges();
                        Message.Message sms = new Message.Message();
                        string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                        if (phone != "1111111111")
                        {
                            sms.SendSms("Withdrawal", (byte)WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);
                        }

                        var withdrawData = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = 0,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "TFR TO SD TO" + " - " + WithdrawalDetails.DepositId,

                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            AccountNo = 40500,
                            ContraNo = 40504,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };
                        db.SdTrans.Add(withdrawData);
                        sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;
                        sdMasterData.TrancationDate = DateFunctions.sysdate(db);



                        //////////////////////////////////////////////////////////////////////////////////////////

                        ApiHandler handler = new ApiHandler();
                        RdVrdData data = new RdVrdData();

                        var ifscCodeBase64 = EncryptAuthenticationTokenAes(WithdrawalDetails.DepositId, WithdrawalDetails.phoneNo, WithdrawalDetails.tfrsdno, WithdrawalDetails.tframt, WithdrawalDetails.odint, WithdrawalDetails.currinstno, WithdrawalDetails.statusAppWeb);

                        data.sdno = ifscCodeBase64;
                        data.phoneNo = ifscCodeBase64;
                        data.tfrsdno = ifscCodeBase64;
                        data.tframt = ifscCodeBase64;
                        data.odint = ifscCodeBase64;
                        data.currinstno = ifscCodeBase64;
                        data.statusAppWeb = ifscCodeBase64;

                        var response = handler.InvokeHttpClient<RdVrd>("getSdfundtfrRD_VRDPayment", data);



                        ///////////////////////////////////////////////////////////////////////////////////////////////



                        db.SaveChanges();
                        Log.Information("/Success");
                        string phonenum = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                        if (phonenum != "1111111111")
                        {
                            sms.SendSms("Withdrawal", (byte)WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phonenum, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);

                        }
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = WithdrawalDetails.TransactionMethod, tranId = withdrawData.TransId });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                    else
                    {
                        
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else if (DateTime.Parse(WithdrawalDetails.StartDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&  //sd
   DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 0)// && frquency.ToLower() == "day")
                {
                    var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                    var sheduleData = new SdScheduleMaster
                    {
                        RtId = rtId + 1,
                        FirmId = WithdrawalDetails.FirmId,
                        BranchId = WithdrawalDetails.BranchId,
                        ModuleId = WithdrawalDetails.ModuleId,
                        DepositId = WithdrawalDetails.DepositId,
                        ScheduledDate = DateFunctions.sysdate(db),
                        StartDate = DateTime.Parse( WithdrawalDetails.StartDate),
                        CloseDate = DateTime.Parse( WithdrawalDetails.EndDate),
                        NoOccurance = (byte?)WithdrawalDetails.NoOfOccurence,
                        Amount = WithdrawalDetails.Amount,
                        Type = "D",
                        Frequency = WithdrawalDetails.Frequency.ToUpper(),
                        StatusId = 1,
                        Ifsc = WithdrawalDetails.Ifsc,
                        AccountNumber = WithdrawalDetails.AccountNumber,
                        UserType = WithdrawalDetails.UserType,
                        UserId = WithdrawalDetails.UserId,
                    };
                    for (int i = 0; i < sheduleData.NoOccurance; i++)
                    {
                        var scheduleDetail = new SdScheduleTran
                        {
                            RtId = (decimal)sheduleData.RtId,
                            FirmId = (byte)sheduleData.FirmId,
                            BranchId = (short)sheduleData.BranchId,
                            ModuleId = (byte)sheduleData.ModuleId,
                            DepositId = sheduleData.DepositId,
                            TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(sheduleData.StartDate).AddMonths(i)
                            : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(sheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(sheduleData.StartDate).AddDays(i),
                            Amount = Convert.ToDecimal(sheduleData.Amount),
                            Type = sheduleData.Type,
                            StatusId = (byte)sheduleData.StatusId,
                            Ifsc = sheduleData.Ifsc,
                            AccountNumber = sheduleData.AccountNumber,
                            UserType = (int)sheduleData.UserType,
                            UserId = sheduleData.UserId,
                            TransId = 0,

                        };
                        db.SdScheduleTrans.Add(scheduleDetail);
                    }
                    db.SdScheduleMasters.Add(sheduleData);






                    db.SaveChanges();
                    Log.Information("/Success");
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "RD", transId = sheduleData.RtId });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "There is no other method" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }
        }

        public ResponseData SaveGoldLoan(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var transId = (int)dep.getSequence(db, WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, (byte)WithdrawalDetails.ModuleId, 4);
                if (WithdrawalDetails.StartDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") &&  //sd
                 WithdrawalDetails.EndDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") && WithdrawalDetails.NoOfOccurence == 1)
                {
                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    //var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    // var RdDetails = db_acc.DepositMsts.FirstOrDefault(x => x.DocId == _withdrawalDetails.DepositId);
                    if (sdMasterData.DepositAmt - WithdrawalDetails.Amount > 0)
                    {
                        var withdrawDataFrom = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = sdMasterData.BranchId,  //sdmaster.branchid
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = transNo,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "TFR SD ACCOUNT TO" + " - " + WithdrawalDetails.Plgno,
                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            AccountNo = 40500,
                            ContraNo = 34400,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };
                        db.SdTrans.Add(withdrawDataFrom);
                        sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;
                        sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                        ////////////////////////////////////////////////////////
                        ApiHandler handler = new ApiHandler();
                        PledgeData data = new PledgeData();


                        var ifscCodeBase64 = EncryptAuthenticationTokenAes(WithdrawalDetails.tfr_data);
                        var ifscCodeBase641 = EncryptAuthenticationTokenAes(WithdrawalDetails.statusAppWeb);

                        data.tfr_data = ifscCodeBase64;
                        data.statusAppWeb = ifscCodeBase641;

                        var response = handler.InvokeHttpClient<Pledge>("getSdfundtfrpledgePayment", data);


                        ///////////////////////////////////////////////////////////////////////////////////////

                        db.SaveChanges();
                        Message.Message sms = new Message.Message();
                        string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);

                        sms.SendSms("Withdrawal", WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "goldLoan", transId = transId });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;

                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else if (DateTime.Parse(WithdrawalDetails.StartDate) == DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&   //RD
            DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 1)
                {
                    var transNo = (int)dep.getSequence(db, 2, 0, 4, 6);
                    // var transId = (int)dep.getSequence(db, 2, 0, 4, 5);
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    if (sdMasterData != null)
                    {

                        var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                        var scheduleData = new SdScheduleMaster
                        {
                            RtId = rtId + 1,
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            ModuleId = WithdrawalDetails.ModuleId,
                            DepositId = WithdrawalDetails.DepositId,
                            ScheduledDate = DateFunctions.sysdate(db),
                            StartDate = DateTime.Parse( WithdrawalDetails.StartDate),
                            CloseDate = DateTime.Parse( WithdrawalDetails.EndDate),
                            NoOccurance = (byte?)WithdrawalDetails.NoOfOccurence,
                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            Frequency = WithdrawalDetails.Frequency.ToUpper(),
                            StatusId = 1,
                            Ifsc = WithdrawalDetails.Ifsc,
                            AccountNumber = WithdrawalDetails.AccountNumber,
                            UserType = WithdrawalDetails.UserType,
                            UserId = WithdrawalDetails.UserId,
                        };

                        for (int i = 1; i < scheduleData.NoOccurance; i++)
                        {
                            var scheduleDetail = new SdScheduleTran
                            {
                                RtId = (decimal)scheduleData.RtId,
                                FirmId = (byte)scheduleData.FirmId,
                                BranchId = (short)scheduleData.BranchId,
                                ModuleId = (byte)scheduleData.ModuleId,
                                DepositId = scheduleData.DepositId,
                                TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(scheduleData.StartDate).AddMonths(i)
                                : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(scheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(scheduleData.StartDate).AddDays(i),
                                Amount = Convert.ToDecimal(scheduleData.Amount),
                                Type = scheduleData.Type,
                                StatusId = (byte)scheduleData.StatusId,
                                Ifsc = scheduleData.Ifsc,
                                AccountNumber = scheduleData.AccountNumber,
                                UserType = (int)scheduleData.UserType,
                                UserId = scheduleData.UserId,
                                TransId = 0,

                            };
                            db.SdScheduleTrans.Add(scheduleDetail);
                        }

                        db.SdScheduleMasters.Add(scheduleData);

                        db.SaveChanges();


                        var withdrawData = new SdTran
                        {
                            FirmId = WithdrawalDetails.FirmId,
                            BranchId = WithdrawalDetails.BranchId,
                            DepositId = WithdrawalDetails.DepositId,
                            TransId = transId,
                            TransNo = 0,
                            TraDt = DateFunctions.sysdate(db),
                            Descr = "TFR TO SD TO" + " - " + WithdrawalDetails.Plgno,

                            Amount = WithdrawalDetails.Amount,
                            Type = "D",
                            AccountNo = 40500,
                            ContraNo = 34400,
                            ValueDt = DateFunctions.sysdate(db),
                            VouchId = 0,
                        };
                        db.SdTrans.Add(withdrawData);
                        sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;
                        sdMasterData.TrancationDate = DateFunctions.sysdate(db);


                        ////////////////////////////////////////////////
                        ///
                        ApiHandler handler = new ApiHandler();
                        PledgeData data = new PledgeData();


                        var ifscCodeBase64 = EncryptAuthenticationTokenAes(WithdrawalDetails.tfr_data);
                        var ifscCodeBase641 = EncryptAuthenticationTokenAes(WithdrawalDetails.statusAppWeb);

                        data.tfr_data = ifscCodeBase64;
                        data.statusAppWeb = ifscCodeBase641;

                        var response = handler.InvokeHttpClient<Pledge>("getSdfundtfrpledgePayment", data);


                        //////////////////////////////////////////////////////////////



                        db.SaveChanges();
                        Log.Information("/Success");

                        Message.Message sms = new Message.Message();
                        string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);
                        if (phone != "1111111111")
                        {
                            sms.SendSms("Withdrawal", (byte)WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi {1} A/c {2} on {3}. Aval Bal is INR {4} - MABEN NIDHI LTD.", WithdrawalDetails.Amount, "SD", WithdrawalDetails.DepositId, DateFunctions.sysdate(db), sdMasterData.DepositAmt), db);
                        }
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 200;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = WithdrawalDetails.TransactionMethod, tranId = withdrawData.TransId });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else if (DateTime.Parse(WithdrawalDetails.StartDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) &&  //sd
   DateTime.Parse(WithdrawalDetails.EndDate) > DateTimeFormat.yyyMMdd(DateFunctions.sysdate(db)) && WithdrawalDetails.NoOfOccurence > 0)// && frquency.ToLower() == "day")
                {
                    var rtId = db.SdScheduleMasters.Select(x => x.RtId).Max();
                    var sheduleData = new SdScheduleMaster
                    {
                        RtId = rtId + 1,
                        FirmId = WithdrawalDetails.FirmId,
                        BranchId = WithdrawalDetails.BranchId,
                        ModuleId = WithdrawalDetails.ModuleId,
                        DepositId = WithdrawalDetails.DepositId,
                        ScheduledDate = DateFunctions.sysdate(db),
                        StartDate = DateTime.Parse(WithdrawalDetails.StartDate),
                        CloseDate = DateTime.Parse( WithdrawalDetails.EndDate),
                        NoOccurance = (byte?)WithdrawalDetails.NoOfOccurence,
                        Amount = WithdrawalDetails.Amount,
                        Type = "D",
                        Frequency = WithdrawalDetails.Frequency.ToUpper(),
                        StatusId = 1,
                        Ifsc = WithdrawalDetails.Ifsc,
                        AccountNumber = WithdrawalDetails.AccountNumber,
                        UserType = WithdrawalDetails.UserType,
                        UserId = WithdrawalDetails.UserId,
                    };
                    for (int i = 0; i < sheduleData.NoOccurance; i++)
                    {
                        var scheduleDetail = new SdScheduleTran
                        {
                            RtId = (decimal)sheduleData.RtId,
                            FirmId = (byte)sheduleData.FirmId,
                            BranchId = (short)sheduleData.BranchId,
                            ModuleId = (byte)sheduleData.ModuleId,
                            DepositId = sheduleData.DepositId,
                            TraDt = WithdrawalDetails.Frequency.ToLower() == "month" ? Convert.ToDateTime(sheduleData.StartDate).AddMonths(i)
                            : WithdrawalDetails.Frequency.ToLower() == "week" ? Convert.ToDateTime(sheduleData.StartDate).AddDays(i * 7) : Convert.ToDateTime(sheduleData.StartDate).AddDays(i),
                            Amount = Convert.ToDecimal(sheduleData.Amount),
                            Type = sheduleData.Type,
                            StatusId = (byte)sheduleData.StatusId,
                            Ifsc = sheduleData.Ifsc,
                            AccountNumber = sheduleData.AccountNumber,
                            UserType = (int)sheduleData.UserType,
                            UserId = sheduleData.UserId,
                            TransId = 0,

                        };
                        db.SdScheduleTrans.Add(scheduleDetail);
                    }
                    db.SdScheduleMasters.Add(sheduleData);
                    db.SaveChanges();
                    Log.Information("/Success");
                    _Response.responseCode = 200;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = WithdrawalDetails.Ifsc != null ? "" : "OTHER", transId = sheduleData.RtId });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "There is no other method" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }
        }
        private ResponseData SaveCheque(ModelContext db)
        {
            try
            {
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);

                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var transId = (int)dep.getSequence(db, WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, (byte)WithdrawalDetails.ModuleId, 4);
                string _clientID = Guid.NewGuid().ToString();
                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");
                SubledgerRequest _subLedgerRequest = new SubledgerRequest();
                if (WithdrawalDetails.StartDate== DateFunctions.sysdate(db).ToString("yyyy-MM-dd") &&  ////Today transaction
        WithdrawalDetails.EndDate == DateFunctions.sysdate(db).ToString("yyyy-MM-dd") && WithdrawalDetails.NoOfOccurence == 1 && WithdrawalDetails.UserType == 0)
                {
                    var sdMasterData = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).SingleOrDefault();
                    if (sdMasterData != null)
                    {

                        if (sdMasterData.DepositAmt - WithdrawalDetails.Amount > 0)
                        {
                            var withdrawData = new SdTran
                            {
                                FirmId = WithdrawalDetails.FirmId,
                                BranchId = (short)cacheDetails.BranchId,
                                DepositId = WithdrawalDetails.DepositId,
                                TransId = transId,
                                TransNo = 0,
                                TraDt = DateFunctions.sysdate(db),
                                Descr = "BY CHQ " + "-" + cacheDetails.BranchId + "-" + WithdrawalDetails.ChequNo + "-" + "SD WITHDRAWAL" + " - " + WithdrawalDetails.DepositId,

                                Amount = WithdrawalDetails.Amount,
                                Type = "D",
                                AccountNo = 40500,
                                ContraNo = 32100,
                                ValueDt = DateFunctions.sysdate(db),
                                VouchId = 0,
                            };
                            db.SdTrans.Add(withdrawData);

                            sdMasterData.DepositAmt = sdMasterData.DepositAmt - WithdrawalDetails.Amount;

                            sdMasterData.TrancationDate = DateFunctions.sysdate(db);
                            db.SaveChanges();
                            Log.Information("/Success");
                            Message.Message sms = new Message.Message();
                            string phone = GetPhoneNumber.getphone(WithdrawalDetails.Customerid, db);

                            if (phone != "1111111111")
                            {
                                sms.SendSms("Withdrawal", WithdrawalDetails.FirmId, WithdrawalDetails.BranchId, WithdrawalDetails.DepositId, 4, WithdrawalDetails.Customerid, "MABDEP", phone, String.Format("Dear Member, INR {0} is debited from your MABEN Nidhi SD A/c {1}  on {2}. Aval Bal is INR {3}", WithdrawalDetails.Amount, WithdrawalDetails.DepositId, DateTime.Now, sdMasterData.DepositAmt), db);
                            }

                            //string _clientID = Guid.NewGuid().ToString();

                            // Console.WriteLine("Client Ready");



                            _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                            _subLedgerRequest.Header.MessageId = dep.MessageId(db); //unique number generator//MessageId;
                            _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                            _subLedgerRequest.Header.Subject = "Cheque submit"; //Subject;
                            _subLedgerRequest.DocumentDetail.FirmId = WithdrawalDetails.FirmId; //FirmId;
                            _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                            _subLedgerRequest.DocumentDetail.ModuleId = 4;
                            _subLedgerRequest.DocumentDetail.DocumentId = WithdrawalDetails.DepositId;
                            _subLedgerRequest.DocumentDetail.CustomerId = WithdrawalDetails.Customerid;
                            _subLedgerRequest.DocumentDetail.CustomerName = WithdrawalDetails.CustomerName;
                            _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                            // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                            // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                            _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate = DateTime.Parse(WithdrawalDetails.RealizationDate).Date;// Data.RealizationDate;//ChequeDate;
                            _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = WithdrawalDetails.ChequNo;
                            _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID = (int)WithdrawalDetails.BranchBankid;
                            _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = (int)WithdrawalDetails.SubsidiaryBankAccountno;
                            _subLedgerRequest.AccountingVoucher.ChequeDetail.CustomerBank = WithdrawalDetails.CustomerBank;
                            _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                            _subLedgerRequest.AccountingVoucher.Narration = "BEING CHQ SD WITHDRAWAL" + "-" + WithdrawalDetails.ChequNo + "-" + "for" + "-" + WithdrawalDetails.DepositId + "-" + "Amount is " + " " + WithdrawalDetails.Amount;
                            if (sdMasterData.BranchId == WithdrawalDetails.BranchBankid)
                            {

                                var accountEntry1 = new AcountingEntry
                                {
                                    ModuleId = 4, //(short)cacheDetails.ModuleId,
                                    AccountNo = 40500,
                                    SubAccountNo = 0,
                                    Type = EntryTypes.D,
                                    Amount = WithdrawalDetails.Amount,
                                    TransactionMode = TransactionModes.CH,
                                    Description = "BY CHQ " + "-" + cacheDetails.BranchId + "-" + WithdrawalDetails.ChequNo + "-" + "SD WITHDRAWAL" + " - " + WithdrawalDetails.DepositId,
                                    ContraNo = 32100,//40500
                                    SegmentId = 1,
                                    BranchId = (short)WithdrawalDetails.BranchBankid,//(short)cacheDetails.BranchId,
                                    ReferenceId = WithdrawalDetails.DepositId,

                                };
                                var accountEntry2 = new AcountingEntry
                                {
                                    ModuleId = 1,//(short)cacheDetails.ModuleId,
                                    AccountNo = 32100,
                                    SubAccountNo = (int)WithdrawalDetails.SubsidiaryBankAccountno,
                                    Type = EntryTypes.C,
                                    Amount = WithdrawalDetails.Amount,
                                    TransactionMode = TransactionModes.CH,
                                    Description = "BY CHQ " + "-" + cacheDetails.BranchId + "-" + WithdrawalDetails.ChequNo + "-" + "SD WITHDRAWAL" + " - " + WithdrawalDetails.DepositId,
                                    ContraNo = 40500,//40500
                                    SegmentId = 1,
                                    BranchId = (short)WithdrawalDetails.BranchBankid,//(short)cacheDetails.BranchId,
                                    ReferenceId = WithdrawalDetails.DepositId,
                                };

                                _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                                _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                                _subLedgerRequest.AccountingVoucher.Amount = WithdrawalDetails.Amount;
                                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;
                                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CHEQUE;



                            }
                            // Enum CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
                            else
                            {
                                var accountEntry3 = new AcountingEntry
                                {
                                    ModuleId = 1, //(short)cacheDetails.ModuleId,
                                    AccountNo = 32100,
                                    SubAccountNo = (int)WithdrawalDetails.SubsidiaryBankAccountno,
                                    Type = EntryTypes.C,
                                    Amount = WithdrawalDetails.Amount,
                                    TransactionMode = TransactionModes.CH,
                                    Description = "BY CHQ " + "-" + cacheDetails.BranchId + "-" + WithdrawalDetails.ChequNo + "-" + "SD WITHDRAWAL" + " - " + WithdrawalDetails.DepositId,
                                    ContraNo = 31003,//40500
                                    SegmentId = 1,
                                    BranchId = (short)WithdrawalDetails.BranchBankid,//(short)cacheDetails.BranchId,
                                    ReferenceId = WithdrawalDetails.DepositId,

                                };
                                var accountEntry4 = new AcountingEntry
                                {
                                    ModuleId = 1,//(short)cacheDetails.ModuleId,
                                    AccountNo = 31003,
                                    SubAccountNo = 0,
                                    Type = EntryTypes.D,
                                    Amount = WithdrawalDetails.Amount,
                                    TransactionMode = TransactionModes.CH,
                                    Description = "BY CHQ " + "-" + cacheDetails.BranchId + "-" + WithdrawalDetails.ChequNo + "-" + "SD WITHDRAWAL" + " - " + WithdrawalDetails.DepositId,
                                    ContraNo = 32100,//40500
                                    SegmentId = 1,
                                    BranchId = (short)WithdrawalDetails.BranchBankid,//(short)cacheDetails.BranchId,
                                    ReferenceId = WithdrawalDetails.DepositId,
                                };
                                var accountEntry5 = new AcountingEntry
                                {
                                    ModuleId = 4, //(short)cacheDetails.ModuleId,
                                    AccountNo = 40500,
                                    SubAccountNo = 0,
                                    Type = EntryTypes.D,
                                    Amount = WithdrawalDetails.Amount,
                                    TransactionMode = TransactionModes.CH,
                                    Description = "BY CHQ " + "-" + cacheDetails.BranchId + "-" + WithdrawalDetails.ChequNo + "-" + "SD WITHDRAWAL" + " - " + WithdrawalDetails.DepositId,
                                    ContraNo = 31003,//40500
                                    SegmentId = 1,
                                    BranchId = (short)WithdrawalDetails.BranchId,//(short)cacheDetails.BranchId,
                                    ReferenceId = WithdrawalDetails.DepositId,

                                };
                                var accountEntry6 = new AcountingEntry
                                {
                                    ModuleId = 1,//(short)cacheDetails.ModuleId,
                                    AccountNo = 31003,
                                    SubAccountNo = 0,
                                    Type = EntryTypes.C,
                                    Amount = WithdrawalDetails.Amount,
                                    TransactionMode = TransactionModes.CH,
                                    Description = "BY CHQ " + "-" + cacheDetails.BranchId + "-" + WithdrawalDetails.ChequNo + "-" + "SD WITHDRAWAL" + " - " + WithdrawalDetails.DepositId,
                                    ContraNo = 40500,
                                    SegmentId = 1,
                                    BranchId = (short)WithdrawalDetails.BranchId,//(short)cacheDetails.BranchId,
                                    ReferenceId = WithdrawalDetails.DepositId,
                                };
                                _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry3);
                                _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry4);
                                _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry5);
                                _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry6);



                                _subLedgerRequest.AccountingVoucher.Amount = WithdrawalDetails.Amount;
                                _subLedgerRequest.AccountingVoucher.CashCounter = 1;
                                _subLedgerRequest.AccountingVoucher.ReferenceNo = transId;
                                _subLedgerRequest.AccountingVoucher.TransactionType = TransactionTypes.Payment;
                                _subLedgerRequest.AccountingVoucher.TransactionMode = ModesOfTransaction.CHEQUE;
                            }                                                                                  //_subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId = TransferDetailModuleId;
                                                                                                               //_subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
                                                                                                               //_subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
                                                                                                               //_subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
                                                                                                               //_subLedgerRequest.AccountingVoucher.
                            string transactionMessage = _subLedgerRequest.ToString();



                            _subLedgerClient.PostMessage(transactionMessage);


                            db.SaveChanges();



                            Log.Information("Success");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 200;
                            var Jsonstring = JsonSerializer.Serialize(new { status = "Success", type = "Cheque", transId = transId });
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            return _Response;



                        }
                        else
                        {
                            Log.Error("This amount is exceeded");
                            ResponseData _Response = new ResponseData();
                            _Response.responseCode = 404;
                            var Jsonstring = JsonSerializer.Serialize(new { status = "This amount is exceeded" });
                            _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                            return _Response;
                        }
                    }
                    else
                    {
                        Log.Error("There is no account");
                        ResponseData _Response = new ResponseData();
                        _Response.responseCode = 404;
                        var Jsonstring = JsonSerializer.Serialize(new { status = "There is no account" });
                        _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                        return _Response;
                    }
                }
                else
                {
                    ResponseData _Response = new ResponseData();
                    _Response.responseCode = 404;
                    var Jsonstring = JsonSerializer.Serialize(new { status = "This method is not applicable" });
                    _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                    return _Response;
                }
            }

            catch (Exception ex)
            {
                Log.Error("ex.Message");
                
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(new { status = "something went wrong" });
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                return _Response;
            }
        }

        protected override ResponseData OnValidationSuccess(ModelContext db)
        {

            ResponseData _Response = Get(db);
            return _Response;

        }

        string EncryptAuthenticationTokenAes(string plainText)
        {

            byte[] encrypted;
            string key = "7x!A%D*G-KaPdSgV";
            byte[] iv = new byte[16];
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                aesAlg.Padding = PaddingMode.None;
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);

        }
        string EncryptAuthenticationTokenAes1(string plainText)
        {

            byte[] encrypted;
            string key = "7x!A%D*G-KaPdSgV";
            byte[] iv = new byte[16];
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                aesAlg.Padding = PaddingMode.None;
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);

        }



        string EncryptAuthenticationTokenAes(string plainText, string Text, string text1, string text2, string text3, string text4, string text5)
        {

            byte[] encrypted;
            string key = "7x!A%D*G-KaPdSgV";
            byte[] iv = new byte[16];
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                aesAlg.Padding = PaddingMode.None;
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);

        }

        protected override string GetSerialisedDataBlockWithDeviceToken()
        {
            WithdrawalDetails.DeviceID = base._cache.DeviceId;
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<withdrawalData>(WithdrawalDetails);
            WithdrawalDetails.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }


        protected override List<Exception> CustomisedValidate(ModelContext db)
        {
            var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
            var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
            var customerId = db.SdMasters.Where(x => x.DepositId == WithdrawalDetails.DepositId).Select(x => x.CustId).SingleOrDefault();
            List<Exception> FailedValidations = new List<Exception>();
            if(customerId!=cacheDetails.UserId)
            {
                FailedValidations.Add(new Exception("this deposit id is invalid"));
            }
            return FailedValidations;
        }


    }
}

