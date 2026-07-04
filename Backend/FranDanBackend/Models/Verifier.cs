using System.ComponentModel.DataAnnotations;

namespace FranDanBackend.Models
{
    public class Verifier
    {
        [Key]
        public int id { get; set; }
        public bool verified { get; set; }
        public string code { get; set; }
        public Verifier() {
            verified=false;
            code = VerificationCodeGenerator.generate();
        }
        public void genCode()
        {
            verified = false;
            code = VerificationCodeGenerator.generate();
        }
        public bool verify(string userCode)
        {
            verified = (code == userCode);
            return verified;
        }
    }
}

