//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace GameStore.Domain.NorthwindDb
{
	public partial class CustomerDemographic
    {
        public CustomerDemographic()
        {
            this.Customers = new HashSet<Customer>();
        }
    
        public string CustomerTypeID { get; set; }
        public string CustomerDesc { get; set; }
    
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
