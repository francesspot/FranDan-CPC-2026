using FranDanBackend.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace FranDanBackend.Models
{
    public class Plan
    {
        [Key]
        public int id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public DateTime startTime { get; set; }
        public int admininstratorId { get; set; }
        [ForeignKey(nameof(admininstratorId))]
        public User creator { get; set; }

        public Plan() { }

        public Plan(string _title, string _category, string _description, string _startTime, User _admininstrator)
        {
            title = _title;
            category = _category;
            description = _description;
            try
            {
                startTime = DateTime.Parse(_startTime, CultureInfo.GetCultureInfo("pl-PL"));
            }
            catch (Exception) { throw new Exception("Date-exception"); }
            admininstratorId = _admininstrator.id;
            creator = _admininstrator;
        }

        public PlanHeaderDTO toPlanHeaderDTO(bool creator,bool admin)
        {
            return new PlanHeaderDTO
            {
                id = id,
                title = title,
                category = category,
                startTime = startTime.ToString("yyyy-MM-dd HH.mm.ss"),
                creator = creator,
                admin = admin
            };
        }
    }
}