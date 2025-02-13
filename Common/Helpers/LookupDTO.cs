namespace Common.Helpers
{
    public class LookupDTO : LocalizedObject
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