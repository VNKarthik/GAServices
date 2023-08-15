namespace GAServices.BusinessEntities.Yarn
{
    public class YarnBlendCreate
    {
        public List<YarnBlendFibres> Fibres { get; set; }

        public long CreatedByUserId { get; set; }
    }

    public class YarnBlendFibres //YarnFibreCategory
    {
        public long FibreCategoryId { get; set; }

        public string FibreCategory { get; set; }

        public double Percentage { get; set; }
    }

    public class YarnBlend
    {
        public long BlendId { get; set; }

        public string BlendName { get; set; }

        public List<YarnBlendFibres> Fibres { get; set; }
    }
}
