using System;
using System.Collections.Generic;
using System.Text;

namespace WhoMasterDataCollection.ForgotPassword.Dto
{
   public class ForgotPasswordDto
    {       
            public string Email { get; set; }
            public string Token { get; set; }
            public string UserId { get; set; }
            public string NewPassword { get; set; }
            public string OldPassword { get; set; }
        
    }
}
