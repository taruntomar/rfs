//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RFS.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class user
    {
        public string Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string logincode { get; set; }
        public string Name { get; set; }
        public string location { get; set; }
        public string phone { get; set; }
        public Nullable<bool> IsActivated { get; set; }
        public Nullable<bool> isAdmin { get; set; }
        public Nullable<bool> IsVerified { get; set; }
        public string VerificationCode { get; set; }
        public string passResetCode { get; set; }
    }
}
