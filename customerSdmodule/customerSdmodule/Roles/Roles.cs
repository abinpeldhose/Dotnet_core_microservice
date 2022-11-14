namespace customerSdmodule.Roles
{
    public class Role
    {
        public RoleData checkRoles(string userType)
        {
            if(userType.ToLower()=="customer")
            {
                var details = new RoleData
                {
                    customerSearch = true,
                    chequeReconciliation = false,
                    bHApproval = false,
                    reports = false,
                    home = true,
                    menuNewSavingAccount = true,
                    menuDeposit = true,
                    menuWithdrawal = true,
                    customerNewSd = true,
                    customerDeposit = true,
                    customerWithdrawal = true,
                    customerSignatureUpload = false,
                    customerAccountStatement = true,
                    customerAccountDetails = true,
                    customerAccountSettlement = true
                };
                return details;
            }
            else if(userType.ToLower()=="employee")
            {
                var details = new RoleData
                {
                    customerSearch = true,
                    chequeReconciliation = true,
                    bHApproval = false,
                    reports = true,
                    home = true,
                    menuNewSavingAccount = true,
                    menuDeposit = true,
                    menuWithdrawal = true,
                    customerNewSd = true,
                    customerDeposit = true,
                    customerWithdrawal = true,
                    customerSignatureUpload = false,
                    customerAccountStatement = true,
                    customerAccountDetails = true,
                    customerAccountSettlement = true
                };
                return details;
            }
            else if(userType.ToLower()=="bh" || userType.ToLower() == "abh")
            {
                var details = new RoleData
                {
                    customerSearch = true,
                    chequeReconciliation = true,
                    bHApproval = true,
                    reports = true,
                    home = true,
                    menuNewSavingAccount = true,
                    menuDeposit = true,
                    menuWithdrawal = true,
                    customerNewSd = true,
                    customerDeposit = true,
                    customerWithdrawal = true,
                    customerSignatureUpload = true,
                    customerAccountStatement = true,
                    customerAccountDetails = true,
                    customerAccountSettlement = true
                };
                return details;
            }
            else
            {
                return null;
            }
           
        }
    }
    public class RoleData
    {
        public bool customerSearch { get; set; }
        public bool chequeReconciliation { get; set; }
        public bool bHApproval { get; set; }
        public bool reports { get; set; }   
        public bool home { get; set; }
        public bool menuNewSavingAccount { get; set; }
        public bool menuDeposit { get; set; }
        public bool menuWithdrawal { get; set; }
        public bool customerNewSd { get; set; }
        public bool customerDeposit { get; set; }
        public bool customerWithdrawal { get; set; }
        public bool customerSignatureUpload { get; set; }
        public bool customerAccountStatement { get; set; }
        public bool customerAccountDetails { get; set; }
        public bool customerAccountSettlement { get; set; }
  
    }
}
