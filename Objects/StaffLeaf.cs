//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Objects
{
    using System;
    using System.Collections.Generic;
    
    public partial class StaffLeaf
    {
        public int StaffLeaveID { get; set; }
        public string StaffID { get; set; }
        public Nullable<System.DateTime> LeaveDate { get; set; }
        public Nullable<System.DateTime> LeaveTime { get; set; }
        public Nullable<int> NumberofDays { get; set; }
        public Nullable<int> NumberofSessionsLeft { get; set; }
        public Nullable<int> ReasontoLeave { get; set; }
    }
}
