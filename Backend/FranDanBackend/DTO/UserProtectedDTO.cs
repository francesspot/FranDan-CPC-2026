
namespace FranDanBackend.DTO{
    public class UserProtectedDTO
    {
        public int id {  get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string occupation { get; set; }
        public string birthday { get; set; }
        public int days_to_birthday {  get; set; }

    }
}
