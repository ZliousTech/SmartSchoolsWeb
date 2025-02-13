using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Objects.DTO
{
    public class LookupDTOTwo : LocalizedObject
    {
        public int ID { get; set; }
        public string id { get; set; }
        
    }


    public class LocalizedObject
    {
        public string Description { get; set; }
        public string DescriptionAR { get; set; }
    }
}