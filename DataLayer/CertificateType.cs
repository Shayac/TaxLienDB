//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class CertificateType
    {
        public CertificateType()
        {
            this.Certificates = new HashSet<Certificate>();
            this.Subsequents = new HashSet<Subsequent>();
        }
    
        public int Id { get; set; }
        public string CertificateType1 { get; set; }
    
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Subsequent> Subsequents { get; set; }
    }
}