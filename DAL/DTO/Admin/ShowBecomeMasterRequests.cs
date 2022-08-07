
namespace DAL.DTO.Admin
{
    public class ShowBecomeMasterRequests
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public int UserId { get; set; }

        public string Description { get; set; }

        public string RequestDate { get; set; }
        public bool Approvement { get; set; }
    }
}
