using customerSdmodule.Model1;
using customerSdmodule.ModelClass.DateFormat;
using MACOM.Contracts;
using MACOM.Contracts.Alerts;
using MACOM.Contracts.Model;
using MACOM.Integrations;
namespace customerSdmodule.sublegder
{
    //public class Subledger
    //{
    //    private string _replyAddress;
    //    private long _messageId;
    //    private DateTime _messageDate;
    //    private string _subject;
    //    private short _firmId;
    //    private short _branchId;
    //    private short _moduleId;
    //    private string _documentId;
    //    private string _customerId;
    //    private string _customerName;
    //    private string _userId;
    //    private SMS _sms;
    //    private MACOM.Contracts.NotificationTypes _notificationType;
    //    private DateTime _chequeDate;
    //    private string _chequeNo;
    //    private short _bankBranchId;
    //    private short _bankBranch;
    //    private short _currencyId;
    //    private string _narration;
    //    private AcountingEntry _accountingEntries;
    //    private decimal _amount;
    //    private short _cashCounter;
    //    private long _referenceNo;
    //    private TransactionTypes _transactionType;
    //    private ModesOfTransaction _transactionMode;
    //    private short _transferDetailModuleId;
    //    private string _transferDetailIfsc;
    //    private string _transferDetailAccountNo;
    //    private int _transferDetailType;

    //    public string ReplyAddress { get => _replyAddress; set => _replyAddress = value; }
    //    public long MessageId { get => _messageId; set => _messageId = value; }
    //    public DateTime MessageDate { get => _messageDate; set => _messageDate = value; }
    //    public string Subject { get => _subject; set => _subject = value; }
    //    public short FirmId { get => _firmId; set => _firmId = value; }
    //    public short BranchId { get => _branchId; set => _branchId = value; }
    //    public short ModuleId { get => _moduleId; set => _moduleId = value; }
    //    public string DocumentId { get => _documentId; set => _documentId = value; }
    //    public string CustomerId { get => _customerId; set => _customerId = value; }
    //    public string CustomerName { get => _customerName; set => _customerName = value; }
    //    public string UserId { get => _userId; set => _userId = value; }
    //    public SMS Sms { get => _sms; set => _sms = value; }
    //    public MACOM.Contracts.NotificationTypes NotificationType { get => _notificationType; set => _notificationType = value; }
    //    public DateTime ChequeDate { get => _chequeDate; set => _chequeDate = value; }
    //    public string ChequeNo { get => _chequeNo; set => _chequeNo = value; }
    //    public short BankBranchId { get => _bankBranchId; set => _bankBranchId = value; }
    //    public short BankBranch { get => _bankBranch; set => _bankBranch = value; }
    //    public short CurrencyId { get => _currencyId; set => _currencyId = value; }
    //    public string Narration { get => _narration; set => _narration = value; }
    //    public AcountingEntry AccountingEntries { get => _accountingEntries; set => _accountingEntries = value; }
    //    public decimal Amount { get => _amount; set => _amount = value; }
    //    public short CashCounter { get => _cashCounter; set => _cashCounter = value; }
    //    public long ReferenceNo { get => _referenceNo; set => _referenceNo = value; }
    //    public short TransferDetailModuleId { get => _transferDetailModuleId; set => _transferDetailModuleId = value; }
    //    public string TransferDetailIfsc { get => _transferDetailIfsc; set => _transferDetailIfsc = value; }
    //    public string TransferDetailAccountNo { get => _transferDetailAccountNo; set => _transferDetailAccountNo = value; }
    //    public int TransferDetailType { get => _transferDetailType; set => _transferDetailType = value; }
    //    public TransactionTypes TransactionType { get => _transactionType; set => _transactionType = value; }
    //    public ModesOfTransaction TransactionMode { get => _transactionMode; set => _transactionMode = value; }

    //    public void Ledger()
    //    {
    //        string _clientID = Guid.NewGuid().ToString();

    //        // Console.WriteLine("Client Ready");

    //        SublegerClient _subLedgerClient = new SublegerClient(_clientID, "10.192.5.44:19092");


    //        SubledgerRequest _subLedgerRequest = new SubledgerRequest();

    //        _subLedgerRequest.Header.ReplyAddress = " ";//ReplyAddress;  
    //        _subLedgerRequest.Header.MessageId = 12345;//unique number generator//MessageId;
    //        _subLedgerRequest.Header.MessageDate = DateTime.Now; //MessageDate;
    //        _subLedgerRequest.Header.Subject = "Deposit";//Subject;
    //        _subLedgerRequest.DocumentDetail.FirmId = 2;//FirmId;
    //        _subLedgerRequest.DocumentDetail.BranchId = BranchId;
    //        _subLedgerRequest.DocumentDetail.ModuleId = ModuleId;
    //        _subLedgerRequest.DocumentDetail.DocumentId = DocumentId;
    //        _subLedgerRequest.DocumentDetail.CustomerId = CustomerId;
    //        _subLedgerRequest.DocumentDetail.CustomerName =CustomerName;
    //        _subLedgerRequest.DocumentDetail.UserId = UserId;
    //        _subLedgerRequest.Notification.SmsBatch.sms.Add(Sms);
    //        _subLedgerRequest.Notification.NotificationType = (MACOM.Contracts.Model.NotificationTypes)NotificationType;   //SMS=0   MAIL=1
    //        _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeDate= ChequeDate;
    //        _subLedgerRequest.AccountingVoucher.ChequeDetail.ChequeNo=ChequeNo;
    //        _subLedgerRequest.AccountingVoucher.ChequeDetail.BankBranchID=BankBranchId;
    //        _subLedgerRequest.AccountingVoucher.ChequeDetail.BranchBank = BankBranch;
    //        _subLedgerRequest.AccountingVoucher.CurrencyId = CurrencyId;
    //        _subLedgerRequest.AccountingVoucher.Narration = Narration;
    //        _subLedgerRequest.AccountingVoucher.AccountingEntries.Add(AccountingEntries);
    //        _subLedgerRequest.AccountingVoucher.Amount = Amount;
    //        _subLedgerRequest.AccountingVoucher.CashCounter =CashCounter;       
    //        _subLedgerRequest.AccountingVoucher.ReferenceNo = ReferenceNo;
    //        _subLedgerRequest.AccountingVoucher.TransactionType = TransactionType;  //Receipt=0  ,payment=1
    //        _subLedgerRequest.AccountingVoucher.TransactionMode = TransactionMode;  // CASH, CHEQUE, OwnAccount,Bank,PayU,BillDesk,TechProcess,CCAvenue,RazorPay
    //        _subLedgerRequest.AccountingVoucher.TransferDetail.ModuleId =TransferDetailModuleId;
    //        _subLedgerRequest.AccountingVoucher.TransferDetail.IFSC = TransferDetailIfsc;
    //        _subLedgerRequest.AccountingVoucher.TransferDetail.AccountNo = TransferDetailAccountNo;
    //        _subLedgerRequest.AccountingVoucher.TransferDetail.Type = 0;//own account ,bank
    //        string transactionMessage = _subLedgerRequest.ToString();
    //        //Console.WriteLine("Send Message {0} ", transactionMessage);
    //        _subLedgerClient.PostMessage(transactionMessage);
    //    }

    //}
}
