using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using customerSdmodule.Redis;
using MACOM.Contracts;
using MACOM.Contracts.Model;
using MACOM.Integrations;
using Serilog;
using System.Text.Json;
using static RedisCacheDemo.RedisCacheStore;

namespace customerSdmodule.ModelClass.PutBHBounceCheque
{
    public class PutBHBounceApi : BaseApi
    {

        private PutBHBounceData _data;
      //  private string _jwtToken;

        public PutBHBounceData Data { get => _data; set => _data = value; }
       // public string JwtToken { get => _jwtToken; set => _jwtToken = value; }

        public ResponseData Get(ModelContext db)
        {
            return Bhbounce(db);
        }


        private ResponseData Bhbounce(ModelContext db)
        {
            try
            {
            	ResponseData _Response = new ResponseData();
                SequenceGenerator dep = new SequenceGenerator();
                var uniqueKey = TokenManager.TokenManagement.Extract(JwtToken);
                var cacheDetails = JsonSerializer.Deserialize<CacheData>(RedisRun.Get(uniqueKey, null));
                var transId = (int)dep.getSequence(db, (byte)cacheDetails.FirmId, (byte)cacheDetails.BranchId, 4, 4);
                var customer = (from customerdetails in db.Customers
                                join master in db.SdMasters on customerdetails.CustId equals master.CustId
                                where master.DepositId == Data.DepositId
                                select new
                                {
                                    customerId=customerdetails.CustId,
                                    customerName=customerdetails.CustName,
                                }).FirstOrDefault(); 
                var existitem = db.SdChequereconcilations.FirstOrDefault(x => x.Chequeno == Data.Chequeno && x.DepositId == Data.DepositId);
                if (existitem == null)
                {
                    var Message = new { Status = "Not found" };
                    //return Results.NotFound(Message);
                    
                    _Response.responseCode = 404;
                    var JsonString = JsonSerializer.Serialize(Message);
                    _Response.data = JsonSerializer.Deserialize<dynamic>(JsonString);
                    //  _Response.data = JsonSerializer.Serialize(Message);
                    return _Response;

                }
                existitem.EmployeeCode = Data.EmpId;
                existitem.EmployeeVerifyDate = DateFunctions.sysdate(db);//DateTime.Now;
                existitem.BhVerifyDate = DateFunctions.sysdate(db);//DateTime.Now;
                existitem.StatusId = 4;
                // existitem.RejectReason = RejectReason;
                db.SaveChanges();




                string _clientID = Guid.NewGuid().ToString();

                // Console.WriteLine("Client Ready");

                SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");
                SubledgerRequest _subLedgerRequest = new SubledgerRequest();


                _subLedgerRequest.Header.ReplyAddress = " "; //ReplyAddress;  
                _subLedgerRequest.Header.MessageId = 12345; //unique number generator//MessageId;
                _subLedgerRequest.Header.MessageDate = DateTime.UtcNow; //MessageDate;
                _subLedgerRequest.Header.Subject = "cheque bounce"; //Subject;
                _subLedgerRequest.DocumentDetail.FirmId = (short)existitem.FirmId; //FirmId;
                _subLedgerRequest.DocumentDetail.BranchId = (short)cacheDetails.BranchId;
                _subLedgerRequest.DocumentDetail.ModuleId = 4;
                _subLedgerRequest.DocumentDetail.DocumentId = Data.DepositId;
                _subLedgerRequest.DocumentDetail.CustomerId = customer==null?" ":customer.customerId;
                _subLedgerRequest.DocumentDetail.CustomerName = existitem.CustomerName;
                
                _subLedgerRequest.DocumentDetail.UserId = cacheDetails.UserId;
                // _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
                // _subLedgerRequest.Notification.NotificationType = MACOM.Contracts.NotificationTypes.SMS;//(MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate = DateTime.UtcNow;// Data.RealizationDate;//ChequeDate;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo = existitem.Chequeno;// Data.ChequeNo;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID =(int) existitem.BranchbankId;//(short)Data.BranchBankid;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank =(int) existitem.SubsidiarybankAccountno;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.ClearanceDate =DateTime.UtcNow.Date;//(short)Data.SubsidiaryBankAccountno;
                _subLedgerRequest.AccountingVoucher.CurrencyId = 1;
                _subLedgerRequest.AccountingVoucher.ChequeDetail.CustomerBank = existitem.CustomerBank;
                _subLedgerRequest.AccountingVoucher.Narration = "BEING CHQ BOUNCED"+"CHEQUE NO:-"+existitem.Chequeno+",SEQ-"+existitem.ChequeSeq+"AMOUNT IS "+existitem.Amount;
                //if (cacheDetails.BranchId == existitem.BranchId)
                //{

                    var accountEntry1 = new AcountingEntry
                    {
                        
                        ModuleId = 1, //(short)cacheDetails.ModuleId,
                        AccountNo = 41241,//33000
                        SubAccountNo=200004,
                        Type = EntryTypes.D,
                        Amount =(decimal) existitem.Amount,
                        TransactionMode = TransactionModes.CH,
                        Description = "BY CHQ BOUNCED - " + cacheDetails.BranchId +",NO - " + existitem.Chequeno+",SEQ-"+existitem.ChequeSeq,
                        ContraNo = 32103,//40500
                        SegmentId = 1,
                        BranchId =(short) existitem.BranchbankId,
                        ReferenceId=existitem.DepositId,
                    };
                    var accountEntry2 = new AcountingEntry
                    {
                        ModuleId = 1,//(short)cacheDetails.ModuleId,
                        AccountNo = 32103,//33000
                        SubAccountNo=100004,
                        Type = EntryTypes.C,
                        Amount = (decimal)existitem.Amount,
                        TransactionMode = TransactionModes.CH,
                        Description = "BY CHQ BOUNCED - " + cacheDetails.BranchId + ",NO - " + existitem.Chequeno + ",SEQ-" + existitem.ChequeSeq,
                        ContraNo = 41241,//40500
                        SegmentId = 1,
                        BranchId =(short) existitem.BranchbankId,
                        ReferenceId = existitem.DepositId,
                    };

                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry1);//.AccountingEntries.Add(account);
                    _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(accountEntry2);
                //}




                _subLedgerRequest.AccountingVoucher.Amount = (decimal)existitem.Amount;
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


                Log.Information("/Success");
                var message = new { status = "Success" };
                
                _Response.responseCode = 200;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                //_Response.data = JsonSerializer.Serialize(message);
                return _Response;
                // return Results.Ok(message);
            }
            catch (Exception ex)
            {
                var message = new { Status = "something went wrong" };
                Log.Error(ex.InnerException.Message);
                ResponseData _Response = new ResponseData();
                _Response.responseCode = 400;
                var Jsonstring = JsonSerializer.Serialize(message);
                _Response.data = JsonSerializer.Deserialize<dynamic>(Jsonstring);
                // _Response.data = JsonSerializer.Serialize(message);
                return _Response;
                // return Results.NotFound(message);
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
            string _SerialisedDataBlockWithDeviceToken = JsonSerializer.Serialize<PutBHBounceData>(Data);
            Data.DeviceID = String.Empty;
            return _SerialisedDataBlockWithDeviceToken;
        }

        protected override List<Exception> CustomisedValidate(ModelContext db)
        {

            List<Exception> FailedValidations = new List<Exception>();

            if (Data.DepositId == null)
            {
                FailedValidations.Add(new ApplicationException("Depositid is invalid"));
            }
            return FailedValidations;
        }
    }
}
