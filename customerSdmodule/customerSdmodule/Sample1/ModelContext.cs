using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace customerSdmodule.Sample1
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountMaster> AccountMasters { get; set; } = null!;
        public virtual DbSet<AccountProfile> AccountProfiles { get; set; } = null!;
        public virtual DbSet<AccountStatus> AccountStatuses { get; set; } = null!;
        public virtual DbSet<BranchMaster> BranchMasters { get; set; } = null!;
        public virtual DbSet<CashTransaction> CashTransactions { get; set; } = null!;
        public virtual DbSet<ChequeRegister> ChequeRegisters { get; set; } = null!;
        public virtual DbSet<CustomerPhoto> CustomerPhotos { get; set; } = null!;
        public virtual DbSet<DepositMst> DepositMsts { get; set; } = null!;
        public virtual DbSet<DepositTran> DepositTrans { get; set; } = null!;
        public virtual DbSet<KeyMaster> KeyMasters { get; set; } = null!;
        public virtual DbSet<PbonlineWithdrawaldetail> PbonlineWithdrawaldetails { get; set; } = null!;
        public virtual DbSet<PledgeMaster> PledgeMasters { get; set; } = null!;
        public virtual DbSet<PledgeStatus> PledgeStatuses { get; set; } = null!;
        public virtual DbSet<SubsidaryMaster> SubsidaryMasters { get; set; } = null!;
        public virtual DbSet<SubsidaryTransaction> SubsidaryTransactions { get; set; } = null!;
        public virtual DbSet<TransactionDetail> TransactionDetails { get; set; } = null!;
        public virtual DbSet<TransactionHistory> TransactionHistories { get; set; } = null!;
        public virtual DbSet<TransidDetail> TransidDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseOracle("User Id=accounts;Password=pass#1234;Data Source=10.192.5.15:1521/macuaT");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("ACCOUNTS");

            modelBuilder.Entity<AccountMaster>(entity =>
            {
                entity.HasKey(e => new { e.BranchId, e.FirmId, e.AccountNo })
                    .HasName("P_ACCOUNT_MASTER");

                entity.ToTable("ACCOUNT_MASTER");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<AccountProfile>(entity =>
            {
                entity.HasKey(e => new { e.AccountNo, e.HoStatus })
                    .HasName("P_ACCOUNT_PROFILE");

                entity.ToTable("ACCOUNT_PROFILE");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.HoStatus)
                    .HasPrecision(1)
                    .HasColumnName("HO_STATUS");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NAME");
            });

            modelBuilder.Entity<AccountStatus>(entity =>
            {
                entity.HasKey(e => new { e.BranchId, e.FirmId, e.AccountNo })
                    .HasName("P_ACCOUNT_STATUS");

                entity.ToTable("ACCOUNT_STATUS");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.StatusId)
                    .HasPrecision(1)
                    .HasColumnName("STATUS_ID");
            });

            modelBuilder.Entity<BranchMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("BRANCH_MASTER");

                entity.Property(e => e.BranchAbbr)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_ABBR");

                entity.Property(e => e.BranchAdd1)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_ADD1");

                entity.Property(e => e.BranchAdd2)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_ADD2");

                entity.Property(e => e.BranchAdd3)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_ADD3");

                entity.Property(e => e.BranchAdd4)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_ADD4");

                entity.Property(e => e.BranchAdd5)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_ADD5");

                entity.Property(e => e.BranchAddr)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_ADDR");

                entity.Property(e => e.BranchCode)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_CODE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.BranchName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_NAME");

                entity.Property(e => e.BranchNo)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_NO");

                entity.Property(e => e.DistrictId)
                    .HasPrecision(5)
                    .HasColumnName("DISTRICT_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.InaugurationDt)
                    .HasColumnType("DATE")
                    .HasColumnName("INAUGURATION_DT");

                entity.Property(e => e.IntWaiverApprd)
                    .HasPrecision(1)
                    .HasColumnName("INT_WAIVER_APPRD");

                entity.Property(e => e.LocalBody)
                    .HasPrecision(1)
                    .HasColumnName("LOCAL_BODY");

                entity.Property(e => e.Phone1)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("PHONE1");

                entity.Property(e => e.Phone2)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("PHONE2");

                entity.Property(e => e.Pincode)
                    .HasPrecision(7)
                    .HasColumnName("PINCODE");

                entity.Property(e => e.RegionId)
                    .HasPrecision(2)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.StateId)
                    .HasPrecision(3)
                    .HasColumnName("STATE_ID");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.UptoDate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UPTO_DATE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CashTransaction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CASH_TRANSACTION");

                entity.HasIndex(e => new { e.RefId, e.FirmId }, "CASHT1");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CashId)
                    .HasPrecision(7)
                    .HasColumnName("CASH_ID");

                entity.Property(e => e.Counter)
                    .HasPrecision(1)
                    .HasColumnName("COUNTER");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.RefId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("REF_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TransId)
                    .HasPrecision(7)
                    .HasColumnName("TRANS_ID");

                entity.Property(e => e.TransNo)
                    .HasPrecision(8)
                    .HasColumnName("TRANS_NO");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<ChequeRegister>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CHEQUE_REGISTER");

                entity.Property(e => e.BankCode)
                    .HasPrecision(6)
                    .HasColumnName("BANK_CODE");

                entity.Property(e => e.BankName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("BANK_NAME");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.ChequeAmount)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("CHEQUE_AMOUNT");

                entity.Property(e => e.ChequeDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CHEQUE_DATE");

                entity.Property(e => e.ChequeNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CHEQUE_NO");

                entity.Property(e => e.Descr)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.StatusId)
                    .HasPrecision(3)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TransId)
                    .HasPrecision(7)
                    .HasColumnName("TRANS_ID");

                entity.Property(e => e.TransNo)
                    .HasPrecision(8)
                    .HasColumnName("TRANS_NO");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<CustomerPhoto>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CUSTOMER_PHOTO");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.KycPhoto)
                    .HasColumnType("BLOB")
                    .HasColumnName("KYC_PHOTO");

                entity.Property(e => e.PledgePhoto)
                    .HasColumnType("BLOB")
                    .HasColumnName("PLEDGE_PHOTO");

                entity.Property(e => e.Signature)
                    .HasColumnType("BLOB")
                    .HasColumnName("SIGNATURE");
            });

            modelBuilder.Entity<DepositMst>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DEPOSIT_MST");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CBranch)
                    .HasPrecision(4)
                    .HasColumnName("C_BRANCH");

                entity.Property(e => e.Chqstatus)
                    .HasPrecision(1)
                    .HasColumnName("CHQSTATUS");

                entity.Property(e => e.Citizen)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CITIZEN")
                    .IsFixedLength();

                entity.Property(e => e.ClsDt)
                    .HasColumnType("DATE")
                    .HasColumnName("CLS_DT");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.DepAmt)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("DEP_AMT");

                entity.Property(e => e.DepDt)
                    .HasColumnType("DATE")
                    .HasColumnName("DEP_DT");

                entity.Property(e => e.DepPrd)
                    .HasPrecision(3)
                    .HasColumnName("DEP_PRD");

                entity.Property(e => e.DepType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DEP_TYPE");

                entity.Property(e => e.DocId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DOC_ID");

                entity.Property(e => e.DueDt)
                    .HasColumnType("DATE")
                    .HasColumnName("DUE_DT");

                entity.Property(e => e.DuplFlag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DUPL_FLAG")
                    .IsFixedLength();

                entity.Property(e => e.EmpId)
                    .HasPrecision(8)
                    .HasColumnName("EMP_ID");

                entity.Property(e => e.FinInterest)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("FIN_INTEREST");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.InstNo)
                    .HasPrecision(2)
                    .HasColumnName("INST_NO");

                entity.Property(e => e.IntAcrued)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("INT_ACRUED");

                entity.Property(e => e.IntRt)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("INT_RT");

                entity.Property(e => e.IntTfrType)
                    .HasPrecision(2)
                    .HasColumnName("INT_TFR_TYPE");

                entity.Property(e => e.IntimationId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("INTIMATION_ID");

                entity.Property(e => e.IrBranch)
                    .HasPrecision(5)
                    .HasColumnName("IR_BRANCH");

                entity.Property(e => e.Lean)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LEAN")
                    .IsFixedLength();

                entity.Property(e => e.LetterFlag)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LETTER_FLAG")
                    .IsFixedLength();

                entity.Property(e => e.LockPrd)
                    .HasPrecision(5)
                    .HasColumnName("LOCK_PRD");

                entity.Property(e => e.MatVal)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("MAT_VAL");

                entity.Property(e => e.Minor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MINOR")
                    .IsFixedLength();

                entity.Property(e => e.MobFlg)
                    .HasPrecision(1)
                    .HasColumnName("MOB_FLG");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Nominee)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NOMINEE")
                    .IsFixedLength();

                entity.Property(e => e.PreRate)
                    .HasColumnType("NUMBER(6,3)")
                    .HasColumnName("PRE_RATE");

                entity.Property(e => e.ProcessPrd)
                    .HasPrecision(2)
                    .HasColumnName("PROCESS_PRD");

                entity.Property(e => e.Renew)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("RENEW")
                    .IsFixedLength();

                entity.Property(e => e.SchemeId)
                    .HasPrecision(4)
                    .HasColumnName("SCHEME_ID");

                entity.Property(e => e.SpecialCategory)
                    .HasPrecision(1)
                    .HasColumnName("SPECIAL_CATEGORY");

                entity.Property(e => e.StatusId)
                    .HasPrecision(1)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TdsCode)
                    .HasPrecision(1)
                    .HasColumnName("TDS_CODE");

                entity.Property(e => e.TdsStatus)
                    .HasPrecision(2)
                    .HasColumnName("TDS_STATUS");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");
            });

            modelBuilder.Entity<DepositTran>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("DEPOSIT_TRAN");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.ContraNo)
                    .HasPrecision(6)
                    .HasColumnName("CONTRA_NO");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.DocId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DOC_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TransId)
                    .HasPrecision(7)
                    .HasColumnName("TRANS_ID");

                entity.Property(e => e.TransNo)
                    .HasPrecision(7)
                    .HasColumnName("TRANS_NO");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();

                entity.Property(e => e.ValueDt)
                    .HasColumnType("DATE")
                    .HasColumnName("VALUE_DT");

                entity.Property(e => e.VouchId)
                    .HasPrecision(7)
                    .HasColumnName("VOUCH_ID");
            });

            modelBuilder.Entity<KeyMaster>(entity =>
            {
                entity.HasKey(e => new { e.FirmId, e.BranchId, e.ModuleId, e.KeyId })
                    .HasName("P_KEY_MASTER");

                entity.ToTable("KEY_MASTER");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.KeyId)
                    .HasPrecision(10)
                    .HasColumnName("KEY_ID");

                entity.Property(e => e.Description)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION");

                entity.Property(e => e.Value)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<PbonlineWithdrawaldetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PBONLINE_WITHDRAWALDETAILS");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.AppFlg)
                    .HasPrecision(1)
                    .HasColumnName("APP_FLG");

                entity.Property(e => e.BeneficiaryAccountno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BENEFICIARY_ACCOUNTNO");

                entity.Property(e => e.BeneficiaryMmid)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BENEFICIARY_MMID");

                entity.Property(e => e.BeneficiaryMobileno)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BENEFICIARY_MOBILENO");

                entity.Property(e => e.BeneficiaryName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("BENEFICIARY_NAME");

                entity.Property(e => e.BrTransid)
                    .HasPrecision(10)
                    .HasColumnName("BR_TRANSID");

                entity.Property(e => e.Charge)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("CHARGE");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.DebitAccountNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("DEBIT_ACCOUNT_NO");

                entity.Property(e => e.DebitNarration)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DEBIT_NARRATION");

                entity.Property(e => e.DocId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DOC_ID");

                entity.Property(e => e.FileName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("FILE_NAME");

                entity.Property(e => e.HoTransid)
                    .HasPrecision(10)
                    .HasColumnName("HO_TRANSID");

                entity.Property(e => e.IfscCode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("IFSC_CODE");

                entity.Property(e => e.InitialStatus)
                    .HasPrecision(2)
                    .HasColumnName("INITIAL_STATUS");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Reason)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("REASON");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("REMARKS");

                entity.Property(e => e.SequenceNo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("SEQUENCE_NO");

                entity.Property(e => e.ServiceTax)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("SERVICE_TAX");

                entity.Property(e => e.Status)
                    .HasPrecision(2)
                    .HasColumnName("STATUS");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("STATUS_CODE");

                entity.Property(e => e.SubStatusCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUB_STATUS_CODE");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TraFlag)
                    .HasPrecision(1)
                    .HasColumnName("TRA_FLAG");

                entity.Property(e => e.TransactionReferenceNo)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("TRANSACTION_REFERENCE_NO");

                entity.Property(e => e.XmlString)
                    .IsUnicode(false)
                    .HasColumnName("XML_STRING");
            });

            modelBuilder.Entity<PledgeMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PLEDGE_MASTER");

                entity.Property(e => e.ActWeight)
                    .HasColumnType("NUMBER(6,2)")
                    .HasColumnName("ACT_WEIGHT");

                entity.Property(e => e.AppRate)
                    .HasColumnType("NUMBER(6,2)")
                    .HasColumnName("APP_RATE");

                entity.Property(e => e.ApxVal)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("APX_VAL");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CollateralValue)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("COLLATERAL_VALUE");

                entity.Property(e => e.Counter)
                    .HasPrecision(1)
                    .HasColumnName("COUNTER");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.EnhancementId)
                    .HasPrecision(1)
                    .HasColumnName("ENHANCEMENT_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.IntAcrud)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("INT_ACRUD");

                entity.Property(e => e.IntRate)
                    .HasColumnType("NUMBER(6,2)")
                    .HasColumnName("INT_RATE");

                entity.Property(e => e.LndRate)
                    .HasColumnType("NUMBER(6,2)")
                    .HasColumnName("LND_RATE");

                entity.Property(e => e.MaturityDt)
                    .HasColumnType("DATE")
                    .HasColumnName("MATURITY_DT");

                entity.Property(e => e.NetWeight)
                    .HasColumnType("NUMBER(6,2)")
                    .HasColumnName("NET_WEIGHT");

                entity.Property(e => e.OvrDue)
                    .HasColumnType("NUMBER(5,2)")
                    .HasColumnName("OVR_DUE");

                entity.Property(e => e.Period)
                    .HasPrecision(5)
                    .HasColumnName("PERIOD");

                entity.Property(e => e.PledgeNo)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("PLEDGE_NO");

                entity.Property(e => e.PledgeVal)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("PLEDGE_VAL");

                entity.Property(e => e.SchemeId)
                    .HasPrecision(4)
                    .HasColumnName("SCHEME_ID");

                entity.Property(e => e.SchemeNm)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("SCHEME_NM");

                entity.Property(e => e.SerRate)
                    .HasColumnType("NUMBER(6,2)")
                    .HasColumnName("SER_RATE");

                entity.Property(e => e.StoneWeight)
                    .HasColumnType("NUMBER(10,3)")
                    .HasColumnName("STONE_WEIGHT");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.Tradate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRADATE");

                entity.Property(e => e.WbStatus)
                    .HasPrecision(2)
                    .HasColumnName("WB_STATUS");
            });

            modelBuilder.Entity<PledgeStatus>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PLEDGE_STATUS");

                entity.Property(e => e.AuctionDt)
                    .HasColumnType("DATE")
                    .HasColumnName("AUCTION_DT");

                entity.Property(e => e.AuctionId)
                    .HasPrecision(2)
                    .HasColumnName("AUCTION_ID");

                entity.Property(e => e.ClassificationId)
                    .HasPrecision(2)
                    .HasColumnName("CLASSIFICATION_ID");

                entity.Property(e => e.CloseDt)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_DT");

                entity.Property(e => e.DClassification)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("D_CLASSIFICATION");

                entity.Property(e => e.DueDt)
                    .HasColumnType("DATE")
                    .HasColumnName("DUE_DT");

                entity.Property(e => e.FestivalOffer)
                    .HasPrecision(2)
                    .HasColumnName("FESTIVAL_OFFER");

                entity.Property(e => e.InterestFlag)
                    .HasPrecision(2)
                    .HasColumnName("INTEREST_FLAG");

                entity.Property(e => e.InventoryId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("INVENTORY_ID");

                entity.Property(e => e.InventoryidTemp)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("INVENTORYID_TEMP");

                entity.Property(e => e.LastUptodate)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_UPTODATE");

                entity.Property(e => e.LetterStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("LETTER_STATUS");

                entity.Property(e => e.PledgeNo)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("PLEDGE_NO");

                entity.Property(e => e.ReleaseId)
                    .HasPrecision(2)
                    .HasColumnName("RELEASE_ID");

                entity.Property(e => e.ShelfNo)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("SHELF_NO");

                entity.Property(e => e.SmsCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("SMS_CODE");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.StickerNo)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STICKER_NO");

                entity.Property(e => e.TareWeight)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("TARE_WEIGHT");

                entity.Property(e => e.TfrDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TFR_DT");

                entity.Property(e => e.TfrFlg)
                    .HasPrecision(1)
                    .HasColumnName("TFR_FLG");
            });

            modelBuilder.Entity<SubsidaryMaster>(entity =>
            {
                entity.HasKey(e => new { e.BranchId, e.FirmId, e.ParentAcc, e.AccountNo })
                    .HasName("P_SUBSIDARY_MASTER");

                entity.ToTable("SUBSIDARY_MASTER");

                entity.HasIndex(e => new { e.BranchId, e.FirmId, e.ParentAcc }, "I_SUBSIDARY_MASTER");

                entity.HasIndex(e => new { e.ParentAcc, e.BranchId, e.FirmId, e.StatusId }, "I_SUBSIDARY_MASTER1");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ParentAcc)
                    .HasPrecision(6)
                    .HasColumnName("PARENT_ACC");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(8)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NAME");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.SubId)
                    .HasPrecision(2)
                    .HasColumnName("SUB_ID")
                    .HasDefaultValueSql("0\n");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<SubsidaryTransaction>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SUBSIDARY_TRANSACTION");

                entity.HasIndex(e => new { e.BranchId, e.FirmId, e.ParentAcc, e.AccountNo, e.TraDt }, "I_SUBSIDARY_TRAN");

                entity.HasIndex(e => new { e.BranchId, e.FirmId, e.ParentAcc, e.AccountNo }, "I_SUBSIDARY_TRAN1");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(8)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ParentAcc)
                    .HasPrecision(6)
                    .HasColumnName("PARENT_ACC");

                entity.Property(e => e.PayMode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PAY_MODE")
                    .IsFixedLength();

                entity.Property(e => e.SubId)
                    .HasPrecision(7)
                    .HasColumnName("SUB_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TransId)
                    .HasPrecision(8)
                    .HasColumnName("TRANS_ID");

                entity.Property(e => e.TransNo)
                    .HasPrecision(8)
                    .HasColumnName("TRANS_NO");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();
            });

            modelBuilder.Entity<TransactionDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TRANSACTION_DETAIL");

                entity.HasIndex(e => new { e.RefId, e.AccountNo }, "IDX$$_00010021");

                entity.HasIndex(e => new { e.AccountNo, e.Descr, e.BranchId }, "IDX$$_00010071");

                entity.HasIndex(e => new { e.FirmId, e.BranchId, e.TransId }, "I_BRID");

                entity.HasIndex(e => e.ModuleId, "TRANSACTION_DET_IDX$$_2D010000");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.ContraNo)
                    .HasPrecision(6)
                    .HasColumnName("CONTRA_NO");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Narration)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NARRATION");

                entity.Property(e => e.PayMode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PAY_MODE")
                    .IsFixedLength();

                entity.Property(e => e.RefId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("REF_ID");

                entity.Property(e => e.SegmentId)
                    .HasPrecision(2)
                    .HasColumnName("SEGMENT_ID")
                    .HasDefaultValueSql("0\n");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TransId)
                    .HasPrecision(8)
                    .HasColumnName("TRANS_ID");

                entity.Property(e => e.Transno)
                    .HasPrecision(8)
                    .HasColumnName("TRANSNO");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.ValueDt)
                    .HasColumnType("DATE")
                    .HasColumnName("VALUE_DT");
            });

            modelBuilder.Entity<TransactionHistory>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TRANSACTION_HISTORY");

                entity.HasIndex(e => new { e.BranchId, e.FirmId, e.AccountNo, e.Tradt1 }, "I_TRANSACTION_HIS");

                entity.HasIndex(e => new { e.FirmId, e.AccountNo, e.Tradt1 }, "I_TRANSACTION_HIS1");

                entity.HasIndex(e => new { e.FirmId, e.AccountNo, e.Transno }, "I_TRANSACTION_HIS2");

                entity.HasIndex(e => new { e.BranchId, e.FirmId, e.Tradt1 }, "I_TRANSACTION_HIS3");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.ContraNo)
                    .HasPrecision(6)
                    .HasColumnName("CONTRA_NO");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Narration)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NARRATION");

                entity.Property(e => e.PayMode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PAY_MODE")
                    .IsFixedLength();

                entity.Property(e => e.RefId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("REF_ID");

                entity.Property(e => e.SegmentId)
                    .HasPrecision(2)
                    .HasColumnName("SEGMENT_ID")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.SysDt)
                    .HasColumnType("DATE")
                    .HasColumnName("SYS_DT");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.Tradt1)
                    .HasColumnType("DATE")
                    .HasColumnName("TRADT");

                entity.Property(e => e.TransId)
                    .HasPrecision(8)
                    .HasColumnName("TRANS_ID");

                entity.Property(e => e.Transno)
                    .HasPrecision(8)
                    .HasColumnName("TRANSNO");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.ValueDt)
                    .HasColumnType("DATE")
                    .HasColumnName("VALUE_DT");
            });

            modelBuilder.Entity<TransidDetail>(entity =>
            {
                entity.HasKey(e => new { e.BranchId, e.FirmId })
                    .HasName("P_TRANSID_DETAIL");

                entity.ToTable("TRANSID_DETAIL");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.CashPay)
                    .HasPrecision(8)
                    .HasColumnName("CASH_PAY");

                entity.Property(e => e.CashPay1)
                    .HasPrecision(8)
                    .HasColumnName("CASH_PAY1");

                entity.Property(e => e.CashReceipt)
                    .HasPrecision(8)
                    .HasColumnName("CASH_RECEIPT");

                entity.Property(e => e.CashReceipt1)
                    .HasPrecision(8)
                    .HasColumnName("CASH_RECEIPT1");

                entity.Property(e => e.CustId)
                    .HasPrecision(10)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.OtherCustId)
                    .HasPrecision(10)
                    .HasColumnName("OTHER_CUST_ID");

                entity.Property(e => e.STransid)
                    .HasPrecision(8)
                    .HasColumnName("S_TRANSID");

                entity.Property(e => e.SubsidaryPay)
                    .HasPrecision(8)
                    .HasColumnName("SUBSIDARY_PAY");

                entity.Property(e => e.SubsidaryReceipt)
                    .HasPrecision(8)
                    .HasColumnName("SUBSIDARY_RECEIPT");

                entity.Property(e => e.TransferPay)
                    .HasPrecision(8)
                    .HasColumnName("TRANSFER_PAY");

                entity.Property(e => e.TransferReceipt)
                    .HasPrecision(8)
                    .HasColumnName("TRANSFER_RECEIPT");

                entity.Property(e => e.Transno)
                    .HasPrecision(8)
                    .HasColumnName("TRANSNO");

                entity.Property(e => e.VouchId)
                    .HasPrecision(8)
                    .HasColumnName("VOUCH_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
