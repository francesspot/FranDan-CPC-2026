
namespace FranDanBackend.DTO{
    public class PlanHeaderDTO
    {
        public int id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string startTime { get; set; }
        public bool creator { get; set; }
        public bool admin { get; set; }
    }
}
