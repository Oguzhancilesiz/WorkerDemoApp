using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerDemoApp.Core.Enums
{
    public enum VerificationType { 
        EmailConfirm = 1,
        PhoneConfirm = 2,
        TwoFactorAuth = 3,
        PasswordReset = 4,
        AccountUnlock = 5
    }
}
