using System.ComponentModel.DataAnnotations;

namespace GAServices.BusinessEntities.FibrePOEntities
{
    public class CreateFibrePO
    {
        [Required]
        public DateTime POdate { get; set; }

        [Required]
        public string PONo { get; set; }

        [Required]
        public long PartyId { get; set; }

        public long CreatedBy { get; set; }

        public List<CreateFibrePODts> FibrePODts { get; set; }
    }

    public class CreateFibrePODts
    {
        [Required]
        public long FibreTypeId { get; set; }

        //public string Shade { get; set; }
        public long ShadeId { get; set; }

        [Required]
        public double Weight { get; set; }

        [Required]
        public double Rate { get; set; }

        [Required]
        public double GSTPercent { get; set; }
    }
}
