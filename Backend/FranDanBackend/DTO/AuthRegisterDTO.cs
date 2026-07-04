namespace FranDanBackend.DTO{
    public class AuthRegisterDTO
    {
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool emailNotifications {  get; set; }
        public string occupation { get; set; }
        public string birthday {  get; set; }
    }
}
