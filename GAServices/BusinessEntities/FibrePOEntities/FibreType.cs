namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class FiberType
    {
        public long FibreTypeId { get; set; }

        public string FibreType { get; set; }

        public long FibreCategoryId { get; set; }
    }

    public class FibreCategory
    {
        public long FibreCategoryId { get; set; }

        public string FibreCategoryName { get; set; }

        public string CategoryCode { get; set; }

        public long CategoryOrder { get; set; }
    }
}
