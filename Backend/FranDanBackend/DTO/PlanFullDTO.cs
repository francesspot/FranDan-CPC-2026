
namespace FranDanBackend.DTO{
    public class PlanFullDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string startTime { get; set; }
        public PlanMemberDTO creator { get; set; }
        public List<PlanMemberDTO> participants { get; set; }
    }
}
