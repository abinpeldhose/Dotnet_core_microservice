using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace customerSdmodule.Model1
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

        public virtual DbSet<AccessMaster> AccessMasters { get; set; } = null!;
        public virtual DbSet<AccountProfile> AccountProfiles { get; set; } = null!;
        public virtual DbSet<ActionTable> ActionTables { get; set; } = null!;
        public virtual DbSet<Alert> Alerts { get; set; } = null!;
        public virtual DbSet<AlertDetail> AlertDetails { get; set; } = null!;
        public virtual DbSet<Application> Applications { get; set; } = null!;
        public virtual DbSet<Blog> Blogs { get; set; } = null!;
        public virtual DbSet<Blog1> Blogs1 { get; set; } = null!;
        public virtual DbSet<BlockedDevice> BlockedDevices { get; set; } = null!;
        public virtual DbSet<BranchMaster> BranchMasters { get; set; } = null!;
        public virtual DbSet<Camp> Camps { get; set; } = null!;
        public virtual DbSet<CatrgoryMaster> CatrgoryMasters { get; set; } = null!;
        public virtual DbSet<CitizenMaster> CitizenMasters { get; set; } = null!;
        public virtual DbSet<CountryMaster> CountryMasters { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<CustomerBankDetail> CustomerBankDetails { get; set; } = null!;
        public virtual DbSet<CustomerDetail> CustomerDetails { get; set; } = null!;
        public virtual DbSet<DepositTypeMaster> DepositTypeMasters { get; set; } = null!;
        public virtual DbSet<DistrictMaster> DistrictMasters { get; set; } = null!;
        public virtual DbSet<Dual> Duals { get; set; }=null!;
        public virtual DbSet<EmployeeDepartmentMaster> EmployeeDepartmentMasters { get; set; } = null!;
        public virtual DbSet<EmployeeDesiginationMaster> EmployeeDesiginationMasters { get; set; } = null!;
        public virtual DbSet<EmployeeMaster> EmployeeMasters { get; set; } = null!;
        public virtual DbSet<EmployeePostMaster> EmployeePostMasters { get; set; } = null!;
        public virtual DbSet<FirmMaster> FirmMasters { get; set; } = null!;
        public virtual DbSet<GeneralParameter> GeneralParameters { get; set; } = null!;
        public virtual DbSet<GradeMaster> GradeMasters { get; set; } = null!;
        public virtual DbSet<IfscMaster> IfscMasters { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
       // public virtual DbSet<Item1> Items1 { get; set; } = null!;
        public virtual DbSet<Job> Jobs { get; set; } = null!;
        public virtual DbSet<KeyMaster> KeyMasters { get; set; } = null!;
        public virtual DbSet<LocalbodyMaster> LocalbodyMasters { get; set; } = null!;
        public virtual DbSet<LoginDeatil> LoginDeatils { get; set; } = null!;
        public virtual DbSet<Man> Men { get; set; } = null!;
        public virtual DbSet<MaritalStatusMaster> MaritalStatusMasters { get; set; } = null!;
        public virtual DbSet<ModuleMaster> ModuleMasters { get; set; } = null!;
        public virtual DbSet<ModuleTable> ModuleTables { get; set; } = null!;
        public virtual DbSet<NeftCustomer> NeftCustomers { get; set; } = null!;
        public virtual DbSet<Nomine> Nomines { get; set; } = null!;
        public virtual DbSet<Nominee> Nominees { get; set; } = null!;
        public virtual DbSet<OccupationMaster> OccupationMasters { get; set; } = null!;
        public virtual DbSet<OrganizationTable> OrganizationTables { get; set; } = null!;
        public virtual DbSet<Otp> Otps { get; set; } = null!;
        public virtual DbSet<Otp1> Otp1s { get; set; } = null!;
        public virtual DbSet<PasswordStrength> PasswordStrengths { get; set; } = null!;
        public virtual DbSet<PaymentgatewayDescription> PaymentgatewayDescriptions { get; set; } = null!;
        public virtual DbSet<PaymentgatewayMaster> PaymentgatewayMasters { get; set; } = null!;
        public virtual DbSet<PaymentgatewayTran> PaymentgatewayTrans { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostMaster> PostMasters { get; set; } = null!;
        public virtual DbSet<PrefixMaster> PrefixMasters { get; set; } = null!;
        public virtual DbSet<RegionMaster> RegionMasters { get; set; } = null!;
        public virtual DbSet<RegistrationMaster> RegistrationMasters { get; set; } = null!;
        public virtual DbSet<RegistrationMaster1> RegistrationMaster1s { get; set; } = null!;
        public virtual DbSet<RelationMaster> RelationMasters { get; set; } = null!;
        public virtual DbSet<RoleInfo> RoleInfos { get; set; } = null!;
        public virtual DbSet<RoleModule> RoleModules { get; set; } = null!;
        public virtual DbSet<RoleUser> RoleUsers { get; set; } = null!;
        public virtual DbSet<RolesInfo> RolesInfos { get; set; } = null!;
        public virtual DbSet<SdAgentMaster> SdAgentMasters { get; set; } = null!;
        public virtual DbSet<SdChequereconcilation> SdChequereconcilations { get; set; } = null!;
       // public virtual DbSet<SdChequqreconcilation> SdChequqreconcilations { get; set; } = null!;
        public virtual DbSet<SdDtl> SdDtls { get; set; } = null!;
        public virtual DbSet<SdInterest> SdInterests { get; set; } = null!;
        public virtual DbSet<SdMaster> SdMasters { get; set; } = null!;
        public virtual DbSet<SdMaster1> SdMaster1s { get; set; } = null!;
        public virtual DbSet<SdNote> SdNotes { get; set; } = null!;
        public virtual DbSet<SdRecurringTable> SdRecurringTables { get; set; } = null!;
        public virtual DbSet<SdScheduleMaster> SdScheduleMasters { get; set; } = null!;
        public virtual DbSet<SdScheduleTran> SdScheduleTrans { get; set; } = null!;
        public virtual DbSet<SdScheduledTranMaster> SdScheduledTranMasters { get; set; } = null!;
        public virtual DbSet<SdScheme> SdSchemes { get; set; } = null!;
        public virtual DbSet<SdSheduledTran> SdSheduledTrans { get; set; } = null!;
        public virtual DbSet<SdStatusMaster> SdStatusMasters { get; set; } = null!;
        public virtual DbSet<SdTran> SdTrans { get; set; } = null!;
        public virtual DbSet<SdSubApplicant> SdSubApplicants { get; set; } = null!;
        public virtual DbSet<SdVerification> SdVerifications { get; set; } = null!;
        public virtual DbSet<SessionDetail> SessionDetails { get; set; } = null!;
        public virtual DbSet<SessionDetails1> SessionDetails1s { get; set; } = null!;
        public virtual DbSet<StateMaster> StateMasters { get; set; } = null!;
        public virtual DbSet<StatusMaster> StatusMasters { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<SubsidaryMaster> SubsidaryMasters { get; set; } = null!;
        public virtual DbSet<Supermaster> Supermasters { get; set; } = null!;
        public virtual DbSet<UserLoginMst1> UserLoginMst1s { get; set; } = null!;
        public virtual DbSet<TdsMaster> TdsMasters { get; set; } = null!;
        public virtual DbSet<UserTable> UserTables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var A4 = System.Configuration.ConfigurationManager.AppSettings["ut"];
            // var encryptedText = EncryptPlainTextToCipherText(A4);
            var decryptedText = DecryptCipherTextToPlainText(A4);
            Console.WriteLine("Passed Text = " + A4);
            //    Console.WriteLine("EncryptedText = " + encryptedText);
            Console.WriteLine("@\"" + decryptedText + "\"");

            optionsBuilder.UseOracle("@\"" + decryptedText + "\"");
            optionsBuilder.UseOracle(decryptedText);
            optionsBuilder.EnableSensitiveDataLogging();

        }
        private const string SecurityKey = "ComplexKeyHere_12121";
        public string EncryptPlainTextToCipherText(string PlainText)

        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));

            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            objTripleDESCryptoService.Key = securityKeyArray;

            objTripleDESCryptoService.Mode = CipherMode.ECB;

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
            objTripleDESCryptoService.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public string DecryptCipherTextToPlainText(string CipherText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(CipherText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();


            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();

            objTripleDESCryptoService.Key = securityKeyArray;

            objTripleDESCryptoService.Mode = CipherMode.ECB;

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();


            return UTF8Encoding.UTF8.GetString(resultArray);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("SD")
                .UseCollation("USING_NLS_COMP");
            modelBuilder.Entity<Dual>(entity => 
            {
                entity.HasKey(e => e.SysDate);
                //await _dbContext.Dual.FromSqlRaw("Select sysdate from dual;").ToListAsync();
            });

            modelBuilder.Entity<AccessMaster>(entity =>
            {
                entity.HasKey(e => e.AccessId);

                entity.ToTable("ACCESS_MASTER");

                entity.Property(e => e.AccessId)
                    .HasPrecision(4)
                    .HasColumnName("ACCESS_ID");

                entity.Property(e => e.AccessName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ACCESS_NAME");
            });

            modelBuilder.Entity<AccountProfile>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ACCOUNT_PROFILE");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NAME");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.HoStatus)
                    .HasPrecision(1)
                    .HasColumnName("HO_STATUS");
            });

            modelBuilder.Entity<ActionTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ACTION_TABLE");

                entity.Property(e => e.ActionId)
                    .HasPrecision(6)
                    .HasColumnName("ACTION_ID");

                entity.Property(e => e.ActionName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ACTION_NAME");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(6)
                    .HasColumnName("MODULE_ID");
            });

            modelBuilder.Entity<Alert>(entity =>
            {
                entity.HasKey(e => e.AlertId)
                    .HasName("ALERT_ALERTID");

                entity.ToTable("ALERT");

                entity.Property(e => e.AlertId)
                    .HasPrecision(6)
                    .ValueGeneratedNever()
                    .HasColumnName("ALERT_ID");

                entity.Property(e => e.AlertDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ALERT_DATE");

                entity.Property(e => e.AlertDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ALERT_DESCRIPTION");

                entity.Property(e => e.Id)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.ReadDate)
                    .HasColumnType("DATE")
                    .HasColumnName("READ_DATE");

                entity.Property(e => e.Subject)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("SUBJECT");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");
            });
            modelBuilder.Entity<AlertDetail>(entity =>
            {
                // entity.HasNoKey();

                entity.ToTable("ALERT_DETAILS");

                entity.Property(e => e.Alerttype)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ALERTTYPE");

                entity.Property(e => e.Image)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");

                entity.Property(e => e.Slno)
                    .HasPrecision(2)
                    .HasColumnName("SLNO");
            });
            modelBuilder.Entity<Application>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("APPLICATIONS");

                entity.Property(e => e.AppNo)
                    .HasPrecision(6)
                    .HasColumnName("APP_NO");

                entity.Property(e => e.BuildDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BUILD_DATE");

                entity.Property(e => e.Builder)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BUILDER");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ModuleId)
                   .HasPrecision(2)
                   .HasColumnName("MODULE_ID");

                entity.Property(e => e.UserType)
                   .HasMaxLength(50)
                   .IsUnicode(false)
                   .HasColumnName("USER_TYPE");


                entity.Property(e => e.VersionNo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("VERSION_NO");
            });

            modelBuilder.Entity<BlockedDevice>(entity =>
            {
                //entity.HasNoKey();
                entity.HasKey(e => new { e.DeviceId});


                entity.ToTable("BLOCKED_DEVICES");

                entity.Property(e => e.ActiveStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACTIVE_STATUS");

                entity.Property(e => e.Attempt)
                    .HasPrecision(1)
                    .HasColumnName("ATTEMPT");

                entity.Property(e => e.DeviceId)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("DEVICE_ID");

                entity.Property(e => e.LastAttemptDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_ATTEMPT_DATE");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("BLOG");

                entity.Property(e => e.Blogid)
                    .HasColumnType("NUMBER")
                    .HasColumnName("BLOGID");

                entity.Property(e => e.Url)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("URL");
            });

            modelBuilder.Entity<Blog1>(entity =>
            {
                entity.HasKey(e => e.BlogId);

                entity.ToTable("Blogs");

                entity.Property(e => e.BlogId).HasPrecision(10);
            });

            modelBuilder.Entity<BranchMaster>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PK_BRANCH_ID");

                entity.ToTable("BRANCH_MASTER");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .ValueGeneratedNever()
                    .HasColumnName("BRANCH_ID");

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

                entity.Property(e => e.BranchName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_NAME");

                entity.Property(e => e.DistrictId)
                    .HasPrecision(5)
                    .HasColumnName("DISTRICT_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.InaugurationDt)
                    .HasColumnType("DATE")
                    .HasColumnName("INAUGURATION_DT");

                entity.Property(e => e.LocalBody)
                    .HasPrecision(2)
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
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.UptoDate)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("UPTO_DATE")
                    .IsFixedLength();

                entity.HasOne(d => d.District)
                    .WithMany(p => p.BranchMasters)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_BRANCH_DISTRICT");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.BranchMasters)
                    .HasForeignKey(d => d.FirmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BRANCH_FIRM");

                entity.HasOne(d => d.LocalBodyNavigation)
                    .WithMany(p => p.BranchMasters)
                    .HasForeignKey(d => d.LocalBody)
                    .HasConstraintName("FK_BRANCH_LOCAL_BODY");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.BranchMasters)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_BRANCH_REGION");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.BranchMasters)
                    .HasForeignKey(d => d.StateId)
                    .HasConstraintName("FK_BRANCH_STATE");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.BranchMasters)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_BRANCH_STATUS");
            });

            modelBuilder.Entity<Camp>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CAMP");

                entity.Property(e => e.Locality)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOCALITY");

                entity.Property(e => e.Studid)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STUDID");
            });

            modelBuilder.Entity<CatrgoryMaster>(entity =>
            {
                entity.HasKey(e => e.CatrgoryId)
                    .HasName("PK_CATEGORY");

                entity.ToTable("CATRGORY_MASTER");

                entity.Property(e => e.CatrgoryId)
                    .HasPrecision(1)
                    .HasColumnName("CATRGORY_ID");

                entity.Property(e => e.Catrgory)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CATRGORY");
            });

            modelBuilder.Entity<CitizenMaster>(entity =>
            {
                entity.HasKey(e => e.CitizenId)
                    .HasName("PK_CITIZEN_ID");

                entity.ToTable("CITIZEN_MASTER");

                entity.Property(e => e.CitizenId)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CITIZEN_ID")
                    .IsFixedLength();

                entity.Property(e => e.CitizenType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CITIZEN_TYPE");
            });

            modelBuilder.Entity<CountryMaster>(entity =>
            {
                entity.HasKey(e => e.CountryId)
                    .HasName("PK_COUNTRY_ID");

                entity.ToTable("COUNTRY_MASTER");

                entity.Property(e => e.CountryId)
                    .HasPrecision(5)
                    .ValueGeneratedNever()
                    .HasColumnName("COUNTRY_ID");

                entity.Property(e => e.CountryName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("COUNTRY_NAME");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.CustId);

                entity.ToTable("CUSTOMER");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.AltHouseName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ALT_HOUSE_NAME");

                entity.Property(e => e.AltLocality)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("ALT_LOCALITY");

                entity.Property(e => e.AltPinNo)
                    .HasPrecision(7)
                    .HasColumnName("ALT_PIN_NO");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CardNo)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("CARD_NO");

                entity.Property(e => e.CountryId)
                    .HasPrecision(5)
                    .HasColumnName("COUNTRY_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.FatherName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("FATHER_NAME");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.HouseName)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("HOUSE_NAME");

                entity.Property(e => e.LandMark)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("LAND_MARK");

                entity.Property(e => e.LastModiDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_MODI_DATE");

                entity.Property(e => e.Locality)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("LOCALITY");

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("MARITAL_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.MotherName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MOTHER_NAME");

                entity.Property(e => e.Phone1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE1");

                entity.Property(e => e.Phone2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE2");

                entity.Property(e => e.PinNo)
                    .HasPrecision(7)
                    .HasColumnName("PIN_NO");

                entity.Property(e => e.PsdNo)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("PSD_NO");

                entity.Property(e => e.SancationBy)
                    .HasPrecision(6)
                    .HasColumnName("SANCATION_BY");

                entity.Property(e => e.ShareNo)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("SHARE_NO");

                entity.Property(e => e.Sharecount)
                    .HasPrecision(3)
                    .HasColumnName("SHARECOUNT");

                entity.Property(e => e.SpouseName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SPOUSE_NAME");

                entity.Property(e => e.SpousePrefixId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("SPOUSE_PREFIX_ID");

                entity.Property(e => e.Street)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("STREET");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_CUSTOMER_BRANCH");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CountryId)
                    .HasConstraintName("FK_CUSTOMER_COUNTRY");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.FirmId)
                    .HasConstraintName("FK_CUSTOMER_FIRM");

                entity.HasOne(d => d.MaritalStatusNavigation)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.MaritalStatus)
                    .HasConstraintName("FK_CUSTOMER_MARITAL_STAUS");
            });

            modelBuilder.Entity<CustomerBankDetail>(entity =>
            {
                entity.HasKey(e => e.AccountNo)
                    .HasName("SYS_C009994");

                entity.ToTable("CUSTOMER_BANK_DETAILS");

                entity.Property(e => e.AccountNo)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.BankHolderName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("BANK_HOLDER_NAME");

                entity.Property(e => e.BankName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("BANK_NAME");

                entity.Property(e => e.BranchName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_NAME");

                entity.Property(e => e.CustId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.Ifsc)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("IFSC");
            });
            modelBuilder.Entity<CustomerDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("CUSTOMER_DETAIL");

                entity.Property(e => e.Age)
                    .HasPrecision(3)
                    .HasColumnName("AGE");

                entity.Property(e => e.Caste)
                    .HasPrecision(2)
                    .HasColumnName("CASTE");

                entity.Property(e => e.CerExpDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CER_EXP_DATE");

                entity.Property(e => e.Citizen)
                    .HasPrecision(2)
                    .HasColumnName("CITIZEN");

                entity.Property(e => e.CountryId)
                    .HasPrecision(2)
                    .HasColumnName("COUNTRY_ID");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustType)
                    .HasPrecision(2)
                    .HasColumnName("CUST_TYPE");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("DATE")
                    .HasColumnName("DATE_OF_BIRTH");

                entity.Property(e => e.EmailId)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL_ID");

                entity.Property(e => e.EmpCode)
                    .HasPrecision(6)
                    .HasColumnName("EMP_CODE");

                entity.Property(e => e.Gender)
                    .HasPrecision(2)
                    .HasColumnName("GENDER");

                entity.Property(e => e.GlEnhance)
                    .HasPrecision(2)
                    .HasColumnName("GL_ENHANCE");

                entity.Property(e => e.GstType)
                    .HasPrecision(1)
                    .HasColumnName("GST_TYPE");

                entity.Property(e => e.Gstin)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GSTIN");

                entity.Property(e => e.GuardianName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("GUARDIAN_NAME");

                entity.Property(e => e.LandCerNo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("LAND_CER_NO");

                entity.Property(e => e.LandCertId)
                    .HasPrecision(3)
                    .HasColumnName("LAND_CERT_ID");

                entity.Property(e => e.LandDtls)
                    .HasPrecision(2)
                    .HasColumnName("LAND_DTLS");

                entity.Property(e => e.MaritalStatus)
                    .HasPrecision(2)
                    .HasColumnName("MARITAL_STATUS");

                entity.Property(e => e.NumFchild)
                    .HasPrecision(2)
                    .HasColumnName("NUM_FCHILD");

                entity.Property(e => e.NumMchild)
                    .HasPrecision(2)
                    .HasColumnName("NUM_MCHILD");

                entity.Property(e => e.OccupationId)
                    .HasPrecision(2)
                    .HasColumnName("OCCUPATION_ID");

                entity.Property(e => e.OfficialEmailId)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("OFFICIAL_EMAIL_ID");

                entity.Property(e => e.Pan)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("PAN");

                entity.Property(e => e.PassportNo)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("PASSPORT_NO");

                entity.Property(e => e.PhotoPath)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("PHOTO_PATH");

                entity.Property(e => e.Purposeofloan)
                    .HasPrecision(2)
                    .HasColumnName("PURPOSEOFLOAN");

                entity.Property(e => e.RegDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REG_DATE");

                entity.Property(e => e.Religion)
                    .HasPrecision(2)
                    .HasColumnName("RELIGION");

                entity.Property(e => e.XplusStatus)
                    .HasPrecision(1)
                    .HasColumnName("XPLUS_STATUS")
                    .HasDefaultValueSql("0");
            });

            modelBuilder.Entity<DepositTypeMaster>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_DEP_TYPE");

                entity.ToTable("DEPOSIT_TYPE_MASTER");

                entity.Property(e => e.TypeId)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_ID")
                    .IsFixedLength();

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TypeName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("TYPE_NAME");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.DepositTypeMasters)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_DEPOSIT_TYPE_STATUS");
            });

            modelBuilder.Entity<DistrictMaster>(entity =>
            {
                entity.HasKey(e => e.DistrictId)
                    .HasName("PK_DISTRICT_ID");

                entity.ToTable("DISTRICT_MASTER");

                entity.Property(e => e.DistrictId)
                    .HasPrecision(5)
                    .ValueGeneratedNever()
                    .HasColumnName("DISTRICT_ID");

                entity.Property(e => e.DistrictName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT_NAME");

                entity.Property(e => e.StateId)
                    .HasPrecision(2)
                    .HasColumnName("STATE_ID");

                entity.HasOne(d => d.State)
                    .WithMany(p => p.DistrictMasters)
                    .HasForeignKey(d => d.StateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DISTRICT_STATE");
            });

            modelBuilder.Entity<EmployeeDepartmentMaster>(entity =>
            {
                entity.HasKey(e => e.DepId)
                    .HasName("PK_DESIGINATION");

                entity.ToTable("EMPLOYEE_DEPARTMENT_MASTER");

                entity.Property(e => e.DepId)
                    .HasPrecision(4)
                    .HasColumnName("DEP_ID");

                entity.Property(e => e.DepHead)
                    .HasPrecision(6)
                    .HasColumnName("DEP_HEAD");

                entity.Property(e => e.DepName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DEP_NAME");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.Status)
                    .HasPrecision(8)
                    .HasColumnName("STATUS");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.EmployeeDepartmentMasters)
                    .HasForeignKey(d => d.FirmId)
                    .HasConstraintName("FK_DEPARTMENT_FIRM");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.EmployeeDepartmentMasters)
                    .HasForeignKey(d => d.Status)
                    .HasConstraintName("FK_DEPARTMENT_STATUS");
            });

            modelBuilder.Entity<EmployeeDesiginationMaster>(entity =>
            {
                entity.HasKey(e => e.DesignationId)
                    .HasName("PK_DESIGINATION_MASTER");

                entity.ToTable("EMPLOYEE_DESIGINATION_MASTER");

                entity.Property(e => e.DesignationId)
                    .HasPrecision(3)
                    .HasColumnName("DESIGNATION_ID");

                entity.Property(e => e.Designation)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DESIGNATION");

                entity.Property(e => e.GradeId)
                    .HasPrecision(3)
                    .HasColumnName("GRADE_ID");

                entity.Property(e => e.ProfileGrade)
                    .HasPrecision(3)
                    .HasColumnName("PROFILE_GRADE");
            });

            //modelBuilder.Entity<EmployeeMaster>(entity =>
            //{
            //    entity.HasKey(e => e.EmpCode)
            //        .HasName("PK_EMOLPYEE_MASTER");

            //    entity.ToTable("EMPLOYEE_MASTER");

            //    entity.Property(e => e.EmpCode)
            //        .HasPrecision(6)
            //        .ValueGeneratedNever()
            //        .HasColumnName("EMP_CODE");

            //    entity.Property(e => e.AccessId)
            //        .HasPrecision(4)
            //        .HasColumnName("ACCESS_ID");

            //    entity.Property(e => e.BranchId)
            //        .HasPrecision(5)
            //        .HasColumnName("BRANCH_ID");

            //    entity.Property(e => e.DepartmentId)
            //        .HasPrecision(3)
            //        .HasColumnName("DEPARTMENT_ID");

            //    entity.Property(e => e.DesignationId)
            //        .HasPrecision(3)
            //        .HasColumnName("DESIGNATION_ID");

            //    entity.Property(e => e.EmpName)
            //        .HasMaxLength(80)
            //        .IsUnicode(false)
            //        .HasColumnName("EMP_NAME");

            //    entity.Property(e => e.EmpType)
            //        .HasPrecision(1)
            //        .HasColumnName("EMP_TYPE");

            //    entity.Property(e => e.FirmId)
            //        .HasPrecision(2)
            //        .HasColumnName("FIRM_ID");

            //    entity.Property(e => e.GradeId)
            //        .HasPrecision(2)
            //        .HasColumnName("GRADE_ID");

            //    entity.Property(e => e.JoinDt)
            //        .HasColumnType("DATE")
            //        .HasColumnName("JOIN_DT");

            //    entity.Property(e => e.Phone)
            //        .HasMaxLength(20)
            //        .IsUnicode(false)
            //        .HasColumnName("PHONE");

            //    entity.Property(e => e.PostId)
            //        .HasPrecision(4)
            //        .HasColumnName("POST_ID");

            //    entity.Property(e => e.StatusId)
            //        .HasPrecision(8)
            //        .HasColumnName("STATUS_ID");

            //    entity.HasOne(d => d.Access)
            //        .WithMany(p => p.EmployeeMasters)
            //        .HasForeignKey(d => d.AccessId)
            //        .HasConstraintName("FK_EMPLOYEE_ACCESS");

            //    entity.HasOne(d => d.Branch)
            //        .WithMany(p => p.EmployeeMasters)
            //        .HasForeignKey(d => d.BranchId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_EMPLOYEE_BRANCH");

            //    entity.HasOne(d => d.Department)
            //        .WithMany(p => p.EmployeeMasters)
            //        .HasForeignKey(d => d.DepartmentId)
            //        .HasConstraintName("FK_EMPLOYEE_DEPARTMENT");

            //    entity.HasOne(d => d.Designation)
            //        .WithMany(p => p.EmployeeMasters)
            //        .HasForeignKey(d => d.DesignationId)
            //        .HasConstraintName("FK_EMPLOYEE_DESIGINATION");

            //    entity.HasOne(d => d.Firm)
            //        .WithMany(p => p.EmployeeMasters)
            //        .HasForeignKey(d => d.FirmId)
            //        .HasConstraintName("FK_EMPLOYEE_FIRM");

            //    entity.HasOne(d => d.Post)
            //        .WithMany(p => p.EmployeeMasters)
            //        .HasForeignKey(d => d.PostId)
            //        .HasConstraintName("FK_EMPLOYEE_POST");

            //    entity.HasOne(d => d.Status)
            //        .WithMany(p => p.EmployeeMasters)
            //        .HasForeignKey(d => d.StatusId)
            //        .HasConstraintName("FK_EMPLOYEE_STATUS");
            //});

            modelBuilder.Entity<EmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.EmpCode)
                    .HasName("PK_EMOLPYEE_MASTER");

                entity.ToTable("EMPLOYEE_MASTER");


                entity.Property(e => e.EmpCode)
                    .HasPrecision(6)
                    .HasColumnName("EMP_CODE");


                entity.Property(e => e.AccessId)
                    .HasPrecision(4)
                    .HasColumnName("ACCESS_ID");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.DepartmentId)
                    .HasPrecision(3)
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.DesignationId)
                    .HasPrecision(3)
                    .HasColumnName("DESIGNATION_ID");

                entity.Property(e => e.EmpName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EMP_NAME");

                entity.Property(e => e.EmpType)
                    .HasPrecision(1)
                    .HasColumnName("EMP_TYPE");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.GradeId)
                    .HasPrecision(2)
                    .HasColumnName("GRADE_ID");

                entity.Property(e => e.JoinDt)
                    .HasColumnType("DATE")
                    .HasColumnName("JOIN_DT");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.PostId)
                    .HasPrecision(4)
                    .HasColumnName("POST_ID");

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.HasOne(d => d.Access)
                    .WithMany(p => p.EmployeeMasters)
                    .HasForeignKey(d => d.AccessId)
                    .HasConstraintName("FK_EMPLOYEE_ACCESS");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.EmployeeMasters)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EMPLOYEE_BRANCH");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.EmployeeMasters)
                    .HasForeignKey(d => d.DepartmentId)
                    .HasConstraintName("FK_EMPLOYEE_DEPARTMENT");
                entity.Property(e => e.Password).HasColumnName("PASSWORD");
                entity.HasOne(d => d.Designation)
                    .WithMany(p => p.EmployeeMasters)
                    .HasForeignKey(d => d.DesignationId)
                    .HasConstraintName("FK_EMPLOYEE_DESIGINATION");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.EmployeeMasters)
                    .HasForeignKey(d => d.FirmId)
                    .HasConstraintName("FK_EMPLOYEE_FIRM");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.EmployeeMasters)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK_EMPLOYEE_POST");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.EmployeeMasters)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_EMPLOYEE_STATUS");
            });

            modelBuilder.Entity<EmployeePostMaster>(entity =>
            {
                entity.HasKey(e => e.PostId)
                    .HasName("PK_POST_MASTER");

                entity.ToTable("EMPLOYEE_POST_MASTER");

                entity.Property(e => e.PostId)
                    .HasPrecision(4)
                    .HasColumnName("POST_ID");

                entity.Property(e => e.PostName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POST_NAME");
            });

            modelBuilder.Entity<FirmMaster>(entity =>
            {
                entity.HasKey(e => e.FirmId)
                    .HasName("PK_FIRM_ID");

                entity.ToTable("FIRM_MASTER");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.FirmAbbr)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FIRM_ABBR");

                entity.Property(e => e.FirmAddress)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("FIRM_ADDRESS");

                entity.Property(e => e.FirmName)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("FIRM_NAME");

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.FirmMasters)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("PK_FIRM_STATUS");
            });

            modelBuilder.Entity<GeneralParameter>(entity =>
            {
                entity.HasKey(e => new { e.ParmtrId, e.ModuleId })
                    .HasName("PK_GENERAL_PARAMENTER");

                entity.ToTable("GENERAL_PARAMETER");

                entity.Property(e => e.ParmtrId)
                    .HasPrecision(3)
                    .HasColumnName("PARMTR_ID");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.AccountType)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ParmtrName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PARMTR_NAME");

                entity.Property(e => e.ParmtrValue)
                    .HasMaxLength(42)
                    .IsUnicode(false)
                    .HasColumnName("PARMTR_VALUE");

                entity.Property(e => e.SubLedger)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("SUB_LEDGER")
                    .IsFixedLength();

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.GeneralParameters)
                    .HasForeignKey(d => d.FirmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_GENERAL_FIRM");
            });

            modelBuilder.Entity<GradeMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("GRADE_MASTER");

                entity.Property(e => e.Grade)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("GRADE");

                entity.Property(e => e.GradeAbbr)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("GRADE_ABBR");

                entity.Property(e => e.GradeId)
                    .HasPrecision(2)
                    .HasColumnName("GRADE_ID");
            });

            modelBuilder.Entity<SdNote>(entity =>
            {
                entity.HasKey(e => e.NoteId)
                    .HasName("SYS_C0010314");

                entity.ToTable("SD_NOTES");

                entity.Property(e => e.NoteId)
                    .HasPrecision(5)
                    .ValueGeneratedNever()
                    .HasColumnName("NOTE_ID");

                entity.Property(e => e.BrachId)
                    .HasPrecision(4)
                    .HasColumnName("BRACH_ID");

                entity.Property(e => e.EmployeeId)
                    .HasPrecision(6)
                    .HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(4)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.NoteDate)
                    .HasColumnType("DATE")
                    .HasColumnName("NOTE_DATE");

                entity.Property(e => e.NoteDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTE_DESCRIPTION");
            });
            modelBuilder.Entity<IfscMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("IFSC_MASTER");

                entity.Property(e => e.Address)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.Bankname)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("BANKNAME");

                entity.Property(e => e.Branch)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH");

                entity.Property(e => e.Centre)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("CENTRE");

                entity.Property(e => e.District)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DISTRICT");

                entity.Property(e => e.IfscCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("IFSC_CODE");

                entity.Property(e => e.State)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("STATE");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ITEM");

                entity.Property(e => e.Createddateutc)
                    .HasColumnType("DATE")
                    .HasColumnName("CREATEDDATEUTC")
                    
                    .IsRequired();

                //entity.Property(e => e.Id)
                //    .HasPrecision(6)
                //    .HasColumnName("ID");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("JOBS");

                entity.Property(e => e.Fullname)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FULLNAME");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID");
            });
            modelBuilder.Entity<KeyMaster>(entity =>
            {
                entity.HasKey(e => new { e.FirmId, e.BranchId, e.ModuleId, e.KeyId })
                    .HasName("PK_KEYMASTER");

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
                    .HasPrecision(8)
                    .HasColumnName("VALUE");
            });

            modelBuilder.Entity<LocalbodyMaster>(entity =>
            {
                entity.HasKey(e => e.LocalbodyId)
                    .HasName("PK_LOCALBODY_ID");

                entity.ToTable("LOCALBODY_MASTER");

                entity.Property(e => e.LocalbodyId)
                    .HasPrecision(2)
                    .HasColumnName("LOCALBODY_ID");

                entity.Property(e => e.LocalbodyName)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("LOCALBODY_NAME");
            });

            modelBuilder.Entity<LoginDeatil>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("LOGIN_DEATILS");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.LoginDate)
                    .HasColumnType("DATE")
                    .HasColumnName("LOGIN_DATE");

                entity.Property(e => e.SessionId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SESSION_ID");

                entity.Property(e => e.UserId)
                    .HasPrecision(6)
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<Man>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.ToTable("man");

                entity.Property(e => e.Age)
                    .HasPrecision(10)
                    .HasColumnName("age");
            });

            modelBuilder.Entity<MaritalStatusMaster>(entity =>
            {
                entity.HasKey(e => e.MaritalStatusId)
                    .HasName("PK_MARITAL_STATUS_ID");

                entity.ToTable("MARITAL_STATUS_MASTER");

                entity.Property(e => e.MaritalStatusId)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MARITAL_STATUS_ID")
                    .IsFixedLength();

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MARITAL_STATUS");
            });

            modelBuilder.Entity<ModuleMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MODULE_MASTER");

                entity.Property(e => e.AddedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDED_BY");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ModuleAbbr)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("MODULE_ABBR");

                entity.Property(e => e.ModuleDescr)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODULE_DESCR");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(5)
                    .HasColumnName("MODULE_ID");

                entity.HasOne(d => d.Branch)
                    .WithMany()
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("MODULE_BRANCH");

                entity.HasOne(d => d.Firm)
                    .WithMany()
                    .HasForeignKey(d => d.FirmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MODULE_FIRM");
            });

            modelBuilder.Entity<NeftCustomer>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NEFT_CUSTOMER");

                entity.Property(e => e.AccType)
                    .HasPrecision(2)
                    .HasColumnName("ACC_TYPE");

                entity.Property(e => e.AttachmentType)
                    .HasColumnType("NUMBER")
                    .HasColumnName("ATTACHMENT_TYPE");

                entity.Property(e => e.BankId)
                    .HasPrecision(5)
                    .HasColumnName("BANK_ID");

                entity.Property(e => e.BeneficiaryAccount)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("BENEFICIARY_ACCOUNT");

                entity.Property(e => e.BeneficiaryBranch)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("BENEFICIARY_BRANCH");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.CustRefId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_REF_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.IfscCode)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("IFSC_CODE");

                entity.Property(e => e.MobileNumber)
                    .HasPrecision(13)
                    .HasColumnName("MOBILE_NUMBER");

                entity.Property(e => e.ModifyDt)
                    .HasColumnType("DATE")
                    .HasColumnName("MODIFY_DT");

                entity.Property(e => e.Moduleid)
                    .HasPrecision(3)
                    .HasColumnName("MODULEID");

                entity.Property(e => e.ReasonPhone)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("REASON_PHONE");

                entity.Property(e => e.RejectReason)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("REJECT_REASON");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.UserId)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.VerifiedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("VERIFIED_BY");

                entity.Property(e => e.VerifiedDt)
                    .HasColumnType("DATE")
                    .HasColumnName("VERIFIED_DT");

                entity.Property(e => e.VerifyStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("VERIFY_STATUS")
                    .IsFixedLength();
            });

            modelBuilder.Entity<ModuleTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("MODULE_TABLE");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(6)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.ModuleName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("MODULE_NAME");
            });

            modelBuilder.Entity<Nomine>(entity =>
            {
                entity.HasKey(e => e.VerifyId)
                    .HasName("SYS_C009933");

                entity.ToTable("NOMINE");

                entity.Property(e => e.VerifyId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("VERIFY_ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.CoApplicantAddress)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CO_APPLICANT_ADDRESS");

                entity.Property(e => e.CoApplicantDob)
                    .HasColumnType("DATE")
                    .HasColumnName("CO_APPLICANT_DOB");

                entity.Property(e => e.CoApplicantHousename)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CO_APPLICANT_HOUSENAME");

                entity.Property(e => e.CoApplicantName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("CO_APPLICANT_NAME");

                entity.Property(e => e.CoApplicantPhno)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("CO_APPLICANT_PHNO");

                entity.Property(e => e.Dob)
                    .HasColumnType("DATE")
                    .HasColumnName("DOB");

                entity.Property(e => e.Fathername)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FATHERNAME");

                entity.Property(e => e.Housename)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("HOUSENAME");

                entity.Property(e => e.Location)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("PHONE_NUMBER");

                entity.Property(e => e.Relation)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RELATION");

                entity.Property(e => e.SalesCode)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SALES_CODE");
            });
            modelBuilder.Entity<Nominee>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("NOMINEE");

                entity.Property(e => e.Address)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ADDRESS");

                entity.Property(e => e.Dob)
                    .HasColumnType("DATE")
                    .HasColumnName("DOB");

                entity.Property(e => e.Fathername)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("FATHERNAME");

                entity.Property(e => e.Housename)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("HOUSENAME");

                entity.Property(e => e.Location)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.Name)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Relation)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("RELATION");
            });

            modelBuilder.Entity<OccupationMaster>(entity =>
            {
                entity.HasKey(e => e.OccupationId);

                entity.ToTable("OCCUPATION_MASTER");

                entity.Property(e => e.OccupationId)
                    .HasPrecision(10)
                    .ValueGeneratedNever()
                    .HasColumnName("OCCUPATION_ID");

                entity.Property(e => e.OccupationName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("OCCUPATION_NAME");

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.OccupationMasters)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OCCUPATION_STATUS");
            });

            modelBuilder.Entity<OrganizationTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ORGANIZATION_TABLE");

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

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.BranchName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("BRANCH_NAME");

                entity.Property(e => e.DistrictId)
                    .HasPrecision(5)
                    .HasColumnName("DISTRICT_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.InaugurationDt)
                    .HasColumnType("DATE")
                    .HasColumnName("INAUGURATION_DT");

                entity.Property(e => e.LocalBody)
                    .HasPrecision(2)
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
                    .HasPrecision(8)
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

            modelBuilder.Entity<Otp>(entity =>
            {
               // entity.HasNoKey();

                entity.ToTable("OTP");

                entity.Property(e => e.Mobilenumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MOBILENUMBER");

                entity.Property(e => e.Otp1)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("OTP");

                entity.Property(e => e.Status)
                    .HasPrecision(2)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("DATE")
                    .HasColumnName("TIME_STAMP");

                entity.Property(e => e.TransactionId)
                    .HasPrecision(6)
                    .HasColumnName("TRANSACTION_ID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.MaxTime)
                   .HasPrecision(3)
                   .HasColumnName("MAX_TIME");
            });


            modelBuilder.Entity<Otp1>(entity =>
            {
               

                entity.ToTable("OTP1");

                entity.Property(e => e.MaxTime)
                    .HasPrecision(2)
                    .HasColumnName("MAX_TIME");

                entity.Property(e => e.Mobilenumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("MOBILENUMBER");

                entity.Property(e => e.Otp)
                    .HasPrecision(6)
                    .HasColumnName("OTP");

                entity.Property(e => e.Status)
                    .HasPrecision(2)
                    .HasColumnName("STATUS");

                entity.Property(e => e.TimeStamp)
                    .HasColumnType("DATE")
                    .HasColumnName("TIME_STAMP");

                entity.Property(e => e.TransactionId)
                    .HasPrecision(6)
                    .HasColumnName("TRANSACTION_ID");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<PasswordStrength>(entity =>
            {
                entity.HasKey(e => e.SlNo)
                    .HasName("SYS_C009948");

                entity.ToTable("PASSWORD_STRENGTH");

                entity.Property(e => e.SlNo)
                    .HasPrecision(3)
                    .HasColumnName("SL_NO");

                entity.Property(e => e.Descr)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");
            });

            modelBuilder.Entity<PaymentgatewayDescription>(entity =>
            {
                entity.HasKey(e => e.ProviderId)
                    .HasName("SYS_C009839");

                entity.ToTable("PAYMENTGATEWAY_DESCRIPTION");

                entity.Property(e => e.ProviderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PROVIDER_ID");

                entity.Property(e => e.PaymentDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PAYMENT_DESCRIPTION");

                entity.Property(e => e.PaymentFlat)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PAYMENT_FLAT");
            });

            modelBuilder.Entity<PaymentgatewayMaster>(entity =>
            {
                entity.HasKey(e => e.ProviderId)
                    .HasName("PAYMENTGATEWAY_MASTER_PROVIDERID");

                entity.ToTable("PAYMENTGATEWAY_MASTER");

                entity.HasIndex(e => new { e.ProviderId, e.PaymentgatewayType, e.PaymentType }, "PAYMENTGATEWAY_MASTER_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ProviderId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PROVIDER_ID");

                entity.Property(e => e.ComissionflatDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COMISSIONFLAT_DESCRIPTION");

                entity.Property(e => e.CommissionFlat)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("COMMISSION_FLAT");

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PAYMENT_TYPE");

                entity.Property(e => e.PaymentgatewayCommission)
                    .HasColumnType("NUMBER(8,6)")
                    .HasColumnName("PAYMENTGATEWAY_COMMISSION");

                entity.Property(e => e.PaymentgatewayName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PAYMENTGATEWAY_NAME");

                entity.Property(e => e.PaymentgatewayType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PAYMENTGATEWAY_TYPE");

                entity.Property(e => e.UserType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("USER_TYPE");
            });

            modelBuilder.Entity<PaymentgatewayTran>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("PAYMENTGATEWAY_TRAN");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BankResponse)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("BANK_RESPONSE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("UNIQUE_ID");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("PHOTO");

                entity.Property(e => e.Custid)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("CUSTID");

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("IMAGE");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasIndex(e => e.BlogId, "IX_Posts_BlogId");

                entity.Property(e => e.PostId).HasPrecision(10);

                entity.Property(e => e.BlogId).HasPrecision(10);
            });
            modelBuilder.Entity<PostMaster>(entity =>
            {
                entity.HasKey(e => new { e.SrNumber, e.PinCode })
                    .HasName("PK_POST_PIN_MASTER");

                entity.ToTable("POST_MASTER");

                entity.Property(e => e.SrNumber)
                    .HasPrecision(7)
                    .HasColumnName("SR_NUMBER");

                entity.Property(e => e.PinCode)
                    .HasPrecision(6)
                    .HasColumnName("PIN_CODE");

                entity.Property(e => e.DistrictId)
                    .HasPrecision(5)
                    .HasColumnName("DISTRICT_ID");

                entity.Property(e => e.PostOffice)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("POST_OFFICE");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.PostMasters)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_POST_MASTER_DIST");
            });

            modelBuilder.Entity<PrefixMaster>(entity =>
            {
                entity.HasKey(e => e.PrefixId)
                    .HasName("PK_PREFIX");

                entity.ToTable("PREFIX_MASTER");

                entity.Property(e => e.PrefixId)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PREFIX_ID")
                    .IsFixedLength();

                entity.Property(e => e.Prefix)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PREFIX");
            });

            modelBuilder.Entity<RegionMaster>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("REGION_MASTER");

                entity.Property(e => e.RegionId)
                    .HasPrecision(4)
                    .HasColumnName("REGION_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.RegionName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("REGION_NAME");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.RegionMasters)
                    .HasForeignKey(d => d.FirmId)
                    .HasConstraintName("FK_REGION_FIRM");
            });

            modelBuilder.Entity<RegistrationMaster>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("SYS_C009694");

                entity.ToTable("REGISTRATION_MASTER");

                entity.Property(e => e.UserId)
                    .HasPrecision(6)
                    .ValueGeneratedNever()
                    .HasColumnName("USER_ID");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.PasswordUpdateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PASSWORD_UPDATE_DATE");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.RegistartionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REGISTARTION_DATE");
            });
            modelBuilder.Entity<RegistrationMaster1>(entity =>
            {
                entity.ToTable("REGISTRATION_MASTER1");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.MaxDay)
                    .HasPrecision(4)
                    .HasColumnName("MAX_DAY");

                entity.Property(e => e.Password)
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.PasswordRules)
                    .HasPrecision(4)
                    .HasColumnName("PASSWORD_RULES");

                entity.Property(e => e.PasswordUpdateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PASSWORD_UPDATE_DATE");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.RegistartionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REGISTARTION_DATE");

                entity.Property(e => e.UserId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<RelationMaster>(entity =>
            {
                entity.HasKey(e => e.RelationId);

                entity.ToTable("RELATION_MASTER");

                entity.Property(e => e.RelationId)
                    .HasPrecision(3)
                    .HasColumnName("RELATION_ID");

                entity.Property(e => e.RelationName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("RELATION_NAME");
            });

            modelBuilder.Entity<RoleInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ROLE_INFO");

                entity.Property(e => e.RoleId)
                    .HasPrecision(4)
                    .HasColumnName("ROLE_ID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ROLE_NAME");
            });

            modelBuilder.Entity<RoleModule>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ROLE_MODULE");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(6)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.RoleId)
                    .HasPrecision(6)
                    .HasColumnName("ROLE_ID");
            });
            modelBuilder.Entity<SdAgentMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SD_AGENT_MASTER");

                entity.Property(e => e.AgentId)
                    .HasPrecision(8)
                    .HasColumnName("AGENT_ID");

                entity.Property(e => e.AgentName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("AGENT_NAME");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CsaId)
                    .HasPrecision(7)
                    .HasColumnName("CSA_ID");

                entity.Property(e => e.Dob)
                    .HasColumnType("DATE")
                    .HasColumnName("DOB");

                entity.Property(e => e.Email)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.GstType)
                    .HasPrecision(1)
                    .HasColumnName("GST_TYPE");

                entity.Property(e => e.Gstin)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("GSTIN");

                entity.Property(e => e.HouseName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("HOUSE_NAME");

                entity.Property(e => e.LandMark)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("LAND_MARK");

                entity.Property(e => e.Language)
                    .HasPrecision(2)
                    .HasColumnName("LANGUAGE");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("MOBILE_NO");

                entity.Property(e => e.NomineeId)
                    .HasPrecision(4)
                    .HasColumnName("NOMINEE_ID");

                entity.Property(e => e.OldId)
                    .HasPrecision(7)
                    .HasColumnName("OLD_ID");

                entity.Property(e => e.Pan)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("PAN");

                entity.Property(e => e.PanStatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("PAN_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.PhoneNo)
                    .HasMaxLength(11)
                    .IsUnicode(false)
                    .HasColumnName("PHONE_NO");

                entity.Property(e => e.Pinserial)
                    .HasPrecision(7)
                    .HasColumnName("PINSERIAL");

                entity.Property(e => e.ProfessionId)
                    .HasPrecision(4)
                    .HasColumnName("PROFESSION_ID");

                entity.Property(e => e.RefAgentId)
                    .HasPrecision(8)
                    .HasColumnName("REF_AGENT_ID");

                entity.Property(e => e.StateId)
                    .HasPrecision(2)
                    .HasColumnName("STATE_ID");

                entity.Property(e => e.StatusId)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("STATUS_ID")
                    .IsFixedLength();

                entity.Property(e => e.StreetName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("STREET_NAME");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.UserId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");
            });


            modelBuilder.Entity<RoleUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ROLE_USER");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.EmpCode)
                    .HasPrecision(6)
                    .HasColumnName("EMP_CODE");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.RoleId)
                    .HasPrecision(4)
                    .HasColumnName("ROLE_ID");
            });

            modelBuilder.Entity<RolesInfo>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ROLES_INFO");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.RoleId)
                    .HasPrecision(4)
                    .HasColumnName("ROLE_ID");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ROLE_NAME");
            });

            modelBuilder.Entity<SdChequereconcilation>(entity =>
            {
                entity.HasKey(e => e.Chequeno)
                    .HasName("PK_CHEQUE_REC");

                entity.ToTable("SD_CHEQUERECONCILATION");

                entity.Property(e => e.Chequeno)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CHEQUENO");

                entity.Property(e => e.AbhId)
                    .HasPrecision(6)
                    .HasColumnName("ABH_ID");

                entity.Property(e => e.AbhVerifyDate)
                    .HasColumnType("DATE")
                    .HasColumnName("ABH_VERIFY_DATE");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BhId)
                    .HasPrecision(6)
                    .HasColumnName("BH_ID");

                entity.Property(e => e.BhVerifyDate)
                    .HasColumnType("DATE")
                    .HasColumnName("BH_VERIFY_DATE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.BranchbankId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCHBANK_ID");

                entity.Property(e => e.ChequeCleardt)
                    .HasColumnType("DATE")
                    .HasColumnName("CHEQUE_CLEARDT");

                entity.Property(e => e.ChequeSeq)
                    .HasPrecision(8)
                    .HasColumnName("CHEQUE_SEQ");

                entity.Property(e => e.ChqSubmiteDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CHQ_SUBMITE_DATE");

                entity.Property(e => e.CustomerBank)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_BANK");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("CUSTOMER_NAME");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.EmployeeCode)
                    .HasPrecision(8)
                    .HasColumnName("EMPLOYEE_CODE");

                entity.Property(e => e.EmployeeVerifyDate)
                    .HasColumnType("DATE")
                    .HasColumnName("EMPLOYEE_VERIFY_DATE");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.RealizationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REALIZATION_DATE");

                entity.Property(e => e.RejectReason)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("REJECT_REASON");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.SubsidiarybankAccountno)
                    .HasPrecision(16)
                    .HasColumnName("SUBSIDIARYBANK_ACCOUNTNO");

                entity.Property(e => e.SubsidiarybankName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUBSIDIARYBANK_NAME");
            });
            //modelBuilder.Entity<SdChequqreconcilation>(entity =>
            //{
            //    entity.HasKey(e => e.SlNo)
            //        .HasName("SYS_C009938");

            //    entity.ToTable("SD_CHEQUQRECONCILATION");

            //    entity.Property(e => e.SlNo)
            //        .HasPrecision(6)
            //        .ValueGeneratedNever()
            //        .HasColumnName("SL_NO");

            //    entity.Property(e => e.BranchId)
            //        .HasPrecision(5)
            //        .HasColumnName("BRANCH_ID");

            //    entity.Property(e => e.ChequeNumber)
            //        .HasMaxLength(30)
            //        .IsUnicode(false)
            //        .HasColumnName("CHEQUE_NUMBER");

            //    entity.Property(e => e.ChequesubmitDate)
            //        .HasColumnType("DATE")
            //        .HasColumnName("CHEQUESUBMIT_DATE");

            //    entity.Property(e => e.CustomerBank)
            //        .HasMaxLength(30)
            //        .IsUnicode(false)
            //        .HasColumnName("CUSTOMER_BANK");

            //    entity.Property(e => e.CustomerName)
            //        .HasMaxLength(30)
            //        .IsUnicode(false)
            //        .HasColumnName("CUSTOMER_NAME");

            //    entity.Property(e => e.Depositno)
            //        .HasMaxLength(16)
            //        .IsUnicode(false)
            //        .HasColumnName("DEPOSITNO");

            //    entity.Property(e => e.EmpCode)
            //        .HasPrecision(6)
            //        .HasColumnName("EMP_CODE");

            //    entity.Property(e => e.EmployeeName)
            //        .HasMaxLength(30)
            //        .IsUnicode(false)
            //        .HasColumnName("EMPLOYEE_NAME");

            //    entity.Property(e => e.EmployeeverifyDate)
            //        .HasColumnType("DATE")
            //        .HasColumnName("EMPLOYEEVERIFY_DATE");

            //    entity.Property(e => e.FirmId)
            //        .HasPrecision(2)
            //        .HasColumnName("FIRM_ID");

            //    entity.Property(e => e.RealizationDate)
            //        .HasColumnType("DATE")
            //        .HasColumnName("REALIZATION_DATE");

            //    entity.Property(e => e.ReceivedDate)
            //        .HasColumnType("DATE")
            //        .HasColumnName("RECEIVED_DATE");

            //    entity.Property(e => e.StatusId)
            //        .HasPrecision(2)
            //        .HasColumnName("STATUS_ID");

            //    entity.HasOne(d => d.Status)
            //        .WithMany(p => p.SdChequqreconcilations)
            //        .HasForeignKey(d => d.StatusId)
            //        .HasConstraintName("F_STATUS");
            //});

            modelBuilder.Entity<SdDtl>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SD_DTL");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.MinorDob)
                    .HasColumnType("DATE")
                    .HasColumnName("MINOR_DOB");

                entity.Property(e => e.NopDate)
                    .HasColumnType("DATE")
                    .HasColumnName("NOP_DATE");

                entity.Property(e => e.OtherAppl)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("OTHER_APPL")
                    .IsFixedLength();

                entity.Property(e => e.Repayable)
                    .HasPrecision(1)
                    .HasColumnName("REPAYABLE");

                entity.Property(e => e.ResiStat)
                    .HasPrecision(1)
                    .HasColumnName("RESI_STAT");

                entity.Property(e => e.SecAppl)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("SEC_APPL");

                entity.Property(e => e.SecApplAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("SEC_APPL_ADDRESS");

                entity.Property(e => e.ThirdApplAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("THIRD_APPL_ADDRESS");

                entity.Property(e => e.ThrdAppl)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("THRD_APPL");
            });

            modelBuilder.Entity<SdInterest>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SD_INTEREST");

                entity.HasIndex(e => new { e.FirmId, e.BranchId, e.SchemeId, e.ModuleId, e.PeriodFrom, e.PeriodTo, e.StatusId }, "I_DEPOSIT_INT");

                entity.Property(e => e.Annualyield)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("ANNUALYIELD");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CitizenRate)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("CITIZEN_RATE");

                entity.Property(e => e.CitizenYield)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("CITIZEN_YIELD")
                    .HasDefaultValueSql("0\n");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.IntRate)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("INT_RATE");

                entity.Property(e => e.LoanAdv)
                    .HasColumnType("NUMBER(5,2)")
                    .HasColumnName("LOAN_ADV");

                entity.Property(e => e.LoanRate)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("LOAN_RATE");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.PeriodFrom)
                    .HasPrecision(4)
                    .HasColumnName("PERIOD_FROM");

                entity.Property(e => e.PeriodTo)
                    .HasPrecision(4)
                    .HasColumnName("PERIOD_TO");

                entity.Property(e => e.PreRate)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("PRE_RATE");

                entity.Property(e => e.SchemeId)
                    .HasPrecision(5)
                    .HasColumnName("SCHEME_ID");

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.HasOne(d => d.Status)
                    .WithMany()
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_SD_INT_STATUS");

                entity.HasOne(d => d.SdScheme)
                    .WithMany()
                    .HasForeignKey(d => new { d.FirmId, d.BranchId, d.ModuleId, d.SchemeId })
                    .HasConstraintName("FK_SCHEME");
            });

            modelBuilder.Entity<SdMaster>(entity =>
            {
                entity.HasKey(e => e.DepositId);

                entity.ToTable("SD_MASTER");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CategoryId)
                    .HasPrecision(1)
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.Chqstatus)
                    .HasPrecision(8)
                    .HasColumnName("CHQSTATUS");

                entity.Property(e => e.Citizen)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CITIZEN")
                    .IsFixedLength();

                entity.Property(e => e.CloseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_DATE");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.DepositAmt)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("DEPOSIT_AMT");

                entity.Property(e => e.DepositDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DEPOSIT_DATE");

                entity.Property(e => e.DepositType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Mobilizer)
                    .HasPrecision(14)
                    .HasColumnName("MOBILIZER");

                entity.Property(e => e.FinInterest)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("FIN_INTEREST");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.IncentiveId)
                    .HasPrecision(8)
                    .HasColumnName("INCENTIVE_ID");

                entity.Property(e => e.IntRt)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("INT_RT");

                entity.Property(e => e.IntTfrType)
                    .HasPrecision(1)
                    .HasColumnName("INT_TFR_TYPE");

                entity.Property(e => e.Lien)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("LIEN")
                    .IsFixedLength();

                entity.Property(e => e.Minor)
                    .HasPrecision(8)
                    .HasColumnName("MINOR");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Nominee)
                    .HasPrecision(8)
                    .HasColumnName("NOMINEE");

                entity.Property(e => e.SchemeId)
                    .HasPrecision(4)
                    .HasColumnName("SCHEME_ID");

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TdsCode)
                    .HasPrecision(1)
                    .HasColumnName("TDS_CODE");

                entity.Property(e => e.TrancationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRANCATION_DATE");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.SdMasters)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SD_BRANCH");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SdMasters)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_SD_CATEGORY");

                entity.HasOne(d => d.ChqstatusNavigation)
                    .WithMany(p => p.SdMasterChqstatusNavigations)
                    .HasForeignKey(d => d.Chqstatus)
                    .HasConstraintName("FK_SD_CHEQUESTATUS");

                entity.HasOne(d => d.CitizenNavigation)
                    .WithMany(p => p.SdMasters)
                    .HasForeignKey(d => d.Citizen)
                    .HasConstraintName("FK_SD_CITIZEN");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.SdMasters)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SD_CUST_ID");

                entity.HasOne(d => d.DepositTypeNavigation)
                    .WithMany(p => p.SdMasters)
                    .HasForeignKey(d => d.DepositType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SD_TYPE");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.SdMasters)
                    .HasForeignKey(d => d.FirmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SD_FIRM");

                entity.HasOne(d => d.Incentive)
                    .WithMany(p => p.SdMasterIncentives)
                    .HasForeignKey(d => d.IncentiveId)
                    .HasConstraintName("FK_SD_INCENTIVE");

                entity.HasOne(d => d.MinorNavigation)
                    .WithMany(p => p.SdMasterMinorNavigations)
                    .HasForeignKey(d => d.Minor)
                    .HasConstraintName("FK_SD_MINOR");

                entity.HasOne(d => d.NomineeNavigation)
                    .WithMany(p => p.SdMasterNomineeNavigations)
                    .HasForeignKey(d => d.Nominee)
                    .HasConstraintName("FK_SD_NOMINEE");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.SdMasterStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_SD_STATUS");
            });



            modelBuilder.Entity<SdMaster1>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SD_MASTER1");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CategoryId)
                    .HasPrecision(1)
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.Chqstatus)
                    .HasPrecision(8)
                    .HasColumnName("CHQSTATUS");

                entity.Property(e => e.Citizen)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CITIZEN")
                    .IsFixedLength();

                entity.Property(e => e.CloseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_DATE");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.DepositAmt)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("DEPOSIT_AMT");

                entity.Property(e => e.DepositDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DEPOSIT_DATE");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.DepositType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_TYPE")
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

                entity.Property(e => e.IncentiveId)
                    .HasPrecision(8)
                    .HasColumnName("INCENTIVE_ID");

                entity.Property(e => e.IntRt)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("INT_RT");

                entity.Property(e => e.Minor)
                    .HasPrecision(8)
                    .HasColumnName("MINOR");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Nominee)
                    .HasPrecision(8)
                    .HasColumnName("NOMINEE");

                entity.Property(e => e.SchemeId)
                    .HasPrecision(4)
                    .HasColumnName("SCHEME_ID");

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TdsCode)
                    .HasPrecision(1)
                    .HasColumnName("TDS_CODE");

                entity.Property(e => e.TrancationDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TRANCATION_DATE");
            });

            modelBuilder.Entity<SdNote>(entity =>
            {
                entity.HasKey(e => e.NoteId)
                    .HasName("SYS_C0010314");

                entity.ToTable("SD_NOTES");

                entity.Property(e => e.NoteId)
                    .HasPrecision(5)
                    .ValueGeneratedNever()
                    .HasColumnName("NOTE_ID");

                entity.Property(e => e.BrachId)
                    .HasPrecision(4)
                    .HasColumnName("BRACH_ID");

                entity.Property(e => e.EmployeeId)
                    .HasPrecision(6)
                    .HasColumnName("EMPLOYEE_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(4)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.NoteDate)
                    .HasColumnType("DATE")
                    .HasColumnName("NOTE_DATE");

                entity.Property(e => e.NoteDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NOTE_DESCRIPTION");
            });
            modelBuilder.Entity<SdRecurringTable>(entity =>
            {
               // entity.HasNoKey();

                entity.ToTable("SD_RECURRING_TABLE");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.Frequency)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FREQUENCY");

                entity.Property(e => e.FromDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FROM_DATE");

                entity.Property(e => e.LastUpdateTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_UPDATE_TIME");

                entity.Property(e => e.NoTransactions)
                    .HasPrecision(3)
                    .HasColumnName("NO_TRANSACTIONS");

                entity.Property(e => e.RtId)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("RT_ID");

                entity.Property(e => e.Status)
                    .HasPrecision(2)
                    .HasColumnName("STATUS");

                entity.Property(e => e.ToDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TO_DATE");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TraType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TRA_TYPE");
            });


            modelBuilder.Entity<SdScheduledTranMaster>(entity =>
            {
              //  entity.HasNoKey();

                entity.ToTable("SD_SCHEDULED_TRAN_MASTER");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(38)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.FromDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FROM_DATE");

                entity.Property(e => e.NextTransaction)
                    .HasColumnType("DATE")
                    .HasColumnName("NEXT_TRANSACTION");

                entity.Property(e => e.NoTransactions)
                    .HasPrecision(3)
                    .HasColumnName("NO_TRANSACTIONS");

                entity.Property(e => e.RecurringType)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RECURRING_TYPE");

                entity.Property(e => e.Status)
                    .HasPrecision(2)
                    .HasColumnName("STATUS");

                entity.Property(e => e.ToDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TO_DATE");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.TraType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TRA_TYPE");
            });


            modelBuilder.Entity<SdScheme>(entity =>
            {
                entity.HasKey(e => new { e.FirmId, e.BranchId, e.ModuleId, e.SchemeId });

                entity.ToTable("SD_SCHEME");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.SchemeId)
                    .HasPrecision(5)
                    .HasColumnName("SCHEME_ID");

                entity.Property(e => e.FromDate)
                    .HasColumnType("DATE")
                    .HasColumnName("FROM_DATE");

                entity.Property(e => e.IntPay)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("INT_PAY");

                entity.Property(e => e.IntProc)
                    .HasPrecision(2)
                    .HasColumnName("INT_PROC");

                entity.Property(e => e.MaxAmount)
                    .HasPrecision(12)
                    .HasColumnName("MAX_AMOUNT");

                entity.Property(e => e.MinAmount)
                    .HasPrecision(12)
                    .HasColumnName("MIN_AMOUNT");

                entity.Property(e => e.MinPeriod)
                    .HasPrecision(3)
                    .HasColumnName("MIN_PERIOD");

                entity.Property(e => e.Scheme)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("SCHEME");

                entity.Property(e => e.SchemeStatus)
                    .HasPrecision(8)
                    .HasColumnName("SCHEME_STATUS");

                entity.Property(e => e.ToDate)
                    .HasColumnType("DATE")
                    .HasColumnName("TO_DATE");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.SdSchemes)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SCHEME_BRANCH");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.SdSchemes)
                    .HasForeignKey(d => d.FirmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SCHEME_FIRM");

                entity.HasOne(d => d.SchemeStatusNavigation)
                    .WithMany(p => p.SdSchemes)
                    .HasForeignKey(d => d.SchemeStatus)
                    .HasConstraintName("SCHEME_STATUS");
            });
            modelBuilder.Entity<SdScheduleMaster>(entity =>
            {
              //  entity.HasNoKey();

                entity.ToTable("SD_SCHEDULE_MASTER");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NUMBER");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CloseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_DATE");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.Frequency)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("FREQUENCY");

                entity.Property(e => e.Ifsc)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("IFSC");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.NoOccurance)
                    .HasPrecision(3)
                    .HasColumnName("NO_OCCURANCE");

                entity.Property(e => e.RtId)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("RT_ID");

                entity.Property(e => e.ScheduledDate)
                    .HasColumnType("DATE")
                    .HasColumnName("SCHEDULED_DATE");

                entity.Property(e => e.StartDate)
                    .HasColumnType("DATE")
                    .HasColumnName("START_DATE");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("TYPE");

                entity.Property(e => e.UserId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.UserType)
                    .HasPrecision(1)
                    .HasColumnName("USER_TYPE");
            });

            modelBuilder.Entity<SdScheduleTran>(entity =>
            {
                entity.HasKey(e => new { e.RtId, e.TraDt })
                    .HasName("SYS_C0010579");

                entity.ToTable("SD_SCHEDULE_TRAN");

                entity.Property(e => e.RtId)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("RT_ID");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NUMBER");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BhId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("BH_ID");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CloseDate)
                    .HasColumnType("DATE")
                    .HasColumnName("CLOSE_DATE");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.Ifsc)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("IFSC");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TransId)
                    .HasPrecision(7)
                    .HasColumnName("TRANS_ID");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();

                entity.Property(e => e.UserId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.Property(e => e.UserType)
                    .HasPrecision(1)
                    .HasColumnName("USER_TYPE");
            });



            modelBuilder.Entity<SdSheduledTran>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SD_SHEDULED_TRAN");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.RtId)
                    .HasColumnType("NUMBER(20)")
                    .HasColumnName("RT_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");
                entity.Property(e => e.Status)
                   .HasPrecision(2)
                   .HasColumnName("STATUS");

                entity.Property(e => e.TraDt)
                    .HasColumnType("DATE")
                    .HasColumnName("TRA_DT");

                entity.Property(e => e.Type)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();
            });
            modelBuilder.Entity<SdStatusMaster>(entity =>
            {
                entity.HasKey(e => new { e.StatusId, e.TableName, e.ColumnName })
                    .HasName("PK_SDSTATUSMASTER");

                entity.ToTable("SD_STATUS_MASTER");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TableName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("TABLE_NAME");

                entity.Property(e => e.ColumnName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("COLUMN_NAME");

                entity.Property(e => e.Credit)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("CREDIT")
                    .IsFixedLength();

                entity.Property(e => e.Debit)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("DEBIT")
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");
            });


            modelBuilder.Entity<SdTran>(entity =>
            {
                // entity.HasNoKey();
                entity.HasKey(e => new { e.TransId,e.Amount,e.Type});

                entity.ToTable("SD_TRAN");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(6)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Amount)
                    .HasColumnType("NUMBER(10,2)")
                    .HasColumnName("AMOUNT");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.ContraNo)
                    .HasPrecision(6)
                    .HasColumnName("CONTRA_NO");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.Descr)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("DESCR");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

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
            modelBuilder.Entity<SdSubApplicant>(entity =>
            {
                entity.HasKey(e => e.NomineeId)
                    .HasName("PK_NOMINEE_ID");

                entity.ToTable("SD_SUB_APPLICANTS");

                entity.Property(e => e.NomineeId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("NOMINEE_ID");

                entity.Property(e => e.Adduser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDUSER");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CancelDt)
                    .HasColumnType("DATE")
                    .HasColumnName("CANCEL_DT");

                entity.Property(e => e.Category)
                    .HasPrecision(1)
                    .HasColumnName("CATEGORY");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.Deluser)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("DELUSER");

                entity.Property(e => e.Dob)
                    .HasColumnType("DATE")
                    .HasColumnName("DOB");

                entity.Property(e => e.DocumentId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DOCUMENT_ID");

                entity.Property(e => e.FatHus)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("FAT_HUS");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.House)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasColumnName("HOUSE");

                entity.Property(e => e.Location)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("LOCATION");

                entity.Property(e => e.MinorGuardian)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("MINOR_GUARDIAN");

                entity.Property(e => e.MinorStatus)
                    .HasPrecision(2)
                    .HasColumnName("MINOR_STATUS");

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("NAME");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.Relation)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("RELATION");

                entity.Property(e => e.SubType)
                    .HasPrecision(2)
                    .HasColumnName("SUB_TYPE");
            });

            modelBuilder.Entity<SdVerification>(entity =>
            {
                entity.HasKey(e => e.VerifyId)
                    .HasName("PK_VERIFICATION");

                entity.ToTable("SD_VERIFICATION");

                entity.Property(e => e.VerifyId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("VERIFY_ID");

                entity.Property(e => e.AbhId)
                    .HasPrecision(6)
                    .HasColumnName("ABH_ID");

                entity.Property(e => e.AbhStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("ABH_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.BhId)
                    .HasPrecision(6)
                    .HasColumnName("BH_ID");

                entity.Property(e => e.BhStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("BH_STATUS")
                    .IsFixedLength();

                entity.Property(e => e.BranchCounter)
                    .HasPrecision(1)
                    .HasColumnName("BRANCH_COUNTER");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.CategoryId)
                    .HasPrecision(1)
                    .HasColumnName("CATEGORY_ID");

                entity.Property(e => e.Chqstatus)
                    .HasPrecision(1)
                    .HasColumnName("CHQSTATUS");

                entity.Property(e => e.CustId)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUST_ID");

                entity.Property(e => e.CustName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("CUST_NAME");

                entity.Property(e => e.DepositAmt)
                    .HasColumnType("NUMBER(12,2)")
                    .HasColumnName("DEPOSIT_AMT");

                entity.Property(e => e.DepositDate)
                    .HasColumnType("DATE")
                    .HasColumnName("DEPOSIT_DATE");

                entity.Property(e => e.DepositId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_ID");

                entity.Property(e => e.DepositType)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("DEPOSIT_TYPE")
                    .IsFixedLength();

                entity.Property(e => e.Mobilizer)
                    .HasPrecision(14)
                    .HasColumnName("MOBILIZER");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.IncentiveId)
                    .HasPrecision(1)
                    .HasColumnName("INCENTIVE_ID");

                entity.Property(e => e.IntRt)
                    .HasColumnType("NUMBER(8,4)")
                    .HasColumnName("INT_RT");

                entity.Property(e => e.Minor)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("MINOR")
                    .IsFixedLength();

                entity.Property(e => e.ModuleId)
                    .HasPrecision(2)
                    .HasColumnName("MODULE_ID");

                entity.Property(e => e.Nominee)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("NOMINEE")
                    .IsFixedLength();

                entity.Property(e => e.ProcessPeriod)
                    .HasPrecision(2)
                    .HasColumnName("PROCESS_PERIOD");

                entity.Property(e => e.RejectId)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("REJECT_ID")
                    .IsFixedLength();

                entity.Property(e => e.RejectReason)
                    .HasMaxLength(400)
                    .IsUnicode(false)
                    .HasColumnName("REJECT_REASON");

                entity.Property(e => e.SchemeId)
                    .HasPrecision(4)
                    .HasColumnName("SCHEME_ID");

                entity.Property(e => e.StatusAppweb)
                    .HasPrecision(3)
                    .HasColumnName("STATUS_APPWEB");

                entity.Property(e => e.TdsCode)
                    .HasPrecision(1)
                    .HasColumnName("TDS_CODE");

                entity.Property(e => e.UserId)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.SdVerifications)
                    .HasForeignKey(d => d.BranchId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_VERIFICATION_BRANCH");

                entity.HasOne(d => d.Cust)
                    .WithMany(p => p.SdVerifications)
                    .HasForeignKey(d => d.CustId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_VERIFICATION_CUSTID");

                entity.HasOne(d => d.DepositTypeNavigation)
                    .WithMany(p => p.SdVerifications)
                    .HasForeignKey(d => d.DepositType)
                    .HasConstraintName("PK_VERIFICATION_DEPTYPE");

                entity.HasOne(d => d.Firm)
                    .WithMany(p => p.SdVerifications)
                    .HasForeignKey(d => d.FirmId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("PK_VERIFICATION_FIRM");
            });



            modelBuilder.Entity<SessionDetail>(entity =>
            {
                // entity.HasNoKey();

                entity.ToTable("SESSION_DETAILS");

                entity.Property(e => e.LastUpdatetime)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_UPDATETIME");

                entity.Property(e => e.LoginTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LOGIN_TIME");

                entity.Property(e => e.SessionId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SESSION_ID");

                entity.Property(e => e.UserId)
                    .HasPrecision(6)
                    .HasColumnName("USER_ID");
            });
            modelBuilder.Entity<SessionDetails1>(entity =>
            {
                entity.HasKey(e => e.SessionId)
                    .HasName("SYS_C0010187");

                entity.ToTable("SESSION_DETAILS1");

                entity.Property(e => e.SessionId)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("SESSION_ID");

                entity.Property(e => e.LastUpdateTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LAST_UPDATE_TIME");

                entity.Property(e => e.LoginTime)
                    .HasColumnType("DATE")
                    .HasColumnName("LOGIN_TIME");

                entity.Property(e => e.MaxTime)
                    .HasPrecision(4)
                    .HasColumnName("MAX_TIME");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<StateMaster>(entity =>
            {
                entity.HasKey(e => e.StateId)
                    .HasName("PK_STATE_ID");

                entity.ToTable("STATE_MASTER");

                entity.Property(e => e.StateId)
                    .HasPrecision(2)
                    .HasColumnName("STATE_ID");

                entity.Property(e => e.CountryId)
                    .HasPrecision(5)
                    .HasColumnName("COUNTRY_ID");

                entity.Property(e => e.StateAbbr)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("STATE_ABBR");

                entity.Property(e => e.StateName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("STATE_NAME");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.StateMasters)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_STATE_COUNTRY");
            });

            modelBuilder.Entity<StatusMaster>(entity =>
            {
                entity.HasKey(e => e.StatusId);

                entity.ToTable("STATUS_MASTER");

                entity.Property(e => e.StatusId)
                    .HasPrecision(8)
                    .ValueGeneratedNever()
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.AddedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ADDED_BY");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Studid);

                entity.ToTable("student");

                entity.Property(e => e.Studid).HasPrecision(10);
            });
            modelBuilder.Entity<SubsidaryMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SUBSIDARY_MASTER");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ACCOUNT_NAME");

                entity.Property(e => e.AccountNo)
                    .HasPrecision(8)
                    .HasColumnName("ACCOUNT_NO");

                entity.Property(e => e.Balance)
                    .HasColumnType("NUMBER(15,2)")
                    .HasColumnName("BALANCE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(4)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.FirmId)
                    .HasPrecision(3)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.ParentAcc)
                    .HasPrecision(6)
                    .HasColumnName("PARENT_ACC");

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

            modelBuilder.Entity<Supermaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SUPERMASTER");

                entity.Property(e => e.CountryId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("COUNTRY_ID");

                entity.Property(e => e.StateId)
                    .HasColumnType("NUMBER")
                    .HasColumnName("STATE_ID");

                entity.Property(e => e.StateName)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STATE_NAME");

                entity.Property(e => e.StsteAbbr)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("STSTE_ABBR");
            });


            modelBuilder.Entity<UserLoginMst1>(entity =>
            {
                entity.ToTable("USER_LOGIN_MST_1");

                entity.Property(e => e.Id)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("ID");

                entity.Property(e => e.Appwebstatus)
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("APPWEBSTATUS");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.Custid)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("CUSTID");

                entity.Property(e => e.Devicetoken)
                    .HasMaxLength(700)
                    .IsUnicode(false)
                    .HasColumnName("DEVICETOKEN");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");

                entity.Property(e => e.Imeinumber)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("IMEINUMBER");

                entity.Property(e => e.MaxDay)
                    .HasPrecision(4)
                    .HasColumnName("MAX_DAY");

                entity.Property(e => e.Mpin)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MPIN");

                entity.Property(e => e.MpinDt)
                    .HasColumnType("DATE")
                    .HasColumnName("MPIN_DT");

                entity.Property(e => e.Password)
                    .HasMaxLength(2048)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORD");

                entity.Property(e => e.PasswordRules)
                    .HasPrecision(4)
                    .HasColumnName("PASSWORD_RULES");

                entity.Property(e => e.PasswordUpdateDate)
                    .HasColumnType("DATE")
                    .HasColumnName("PASSWORD_UPDATE_DATE");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("PHONE");

                entity.Property(e => e.RegistartionDate)
                    .HasColumnType("DATE")
                    .HasColumnName("REGISTARTION_DATE");

                entity.Property(e => e.Sharedby)
                    .HasMaxLength(14)
                    .IsUnicode(false)
                    .HasColumnName("SHAREDBY");

                entity.Property(e => e.SmsRefId)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("SMS_REF_ID");

                entity.Property(e => e.Status)
                    .HasPrecision(1)
                    .HasColumnName("STATUS");

                entity.Property(e => e.UserId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("USER_ID");
            });


            modelBuilder.Entity<TdsMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("TDS_MASTER");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.StatusId)
                    .HasPrecision(2)
                    .HasColumnName("STATUS_ID");

                entity.Property(e => e.TdsCode)
                    .HasPrecision(2)
                    .HasColumnName("TDS_CODE");

                entity.Property(e => e.TdsRate)
                    .HasColumnType("NUMBER(5,2)")
                    .HasColumnName("TDS_RATE");
            });

            modelBuilder.Entity<UserTable>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("USER_TABLE");

                entity.Property(e => e.BranchId)
                    .HasPrecision(5)
                    .HasColumnName("BRANCH_ID");

                entity.Property(e => e.DepName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("DEP_NAME");

                entity.Property(e => e.DepartmentId)
                    .HasPrecision(3)
                    .HasColumnName("DEPARTMENT_ID");

                entity.Property(e => e.EmpName)
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasColumnName("EMP_NAME");

                entity.Property(e => e.FirmId)
                    .HasPrecision(2)
                    .HasColumnName("FIRM_ID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
