using System.ComponentModel.DataAnnotations;

namespace GAServices.BusinessEntities.Party
{
    public class District
    {
        public long DistrictId { get; set; }
        public string DistrictName { get; set; }
    }

    public class City
    {
        public long CityId { get; set; }
        public string CityName { get; set; }
    }

    public class State
    {
        public long StateId { get; set; }
        public string StateName { get; set; }

        public long GSTCode { get; set; }
    }

    public class Party
    {
        [Required]
        public string PartyName { get; set; } = null!;

        [Required]
        public string BranchName { get; set; } = null!;

        [Required]
        public string Address1 { get; set; } = null!;

        public string? Address2 { get; set; }

        public string? Address3 { get; set; }

        public long DistrictId { get; set; }

        public long CityId { get; set; }

        public string? PinCode { get; set; }

        [Required]
        public long StateId { get; set; }

        [Required]
        public string? GSTNo { get; set; }

        public string? EMailId { get; set; }

        public string? ContactNo { get; set; }

        public long CreatedByUserId { get; set; }

    }

    public class PartyDetails : Party
    {
        public long PartyId { get; set; }

        public string? DistrictName { get; set; }

        public string CityName { get; set; }

        public string? StateName { get; set; }

        public int? StateCode { get; set; }
    }

    public class DeleteParty
    {
        public long PartyId { get; set; }

        public long DeletedByUserId { get; set; }
    }
}
