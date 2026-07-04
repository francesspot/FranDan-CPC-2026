
using FranDanBackend.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Numerics;

namespace FranDanBackend.Models {
    public class Participation
    {
        public enum Role
        {
            CREATOR,
            ADMIN,
            INVITED_ADMIN,
            PARTICIPANT,
            INVITED,
            NONE
        }
        [Key]
        public int id { get; set; }
        public int userId { get; set; }
        [ForeignKey(nameof(userId))]
        public User user {  get; set; }
        public int planId { get; set; }
        [ForeignKey(nameof(planId))]
        public Plan plan {  get; set; }
        public bool accepted { get; set; }
        public bool admin { get; set; }
        public Participation() {}
        public Participation(User _user, Plan _plan,bool _admin)
        {
            userId = _user.id;
            user= _user;
            planId = _plan.id;
            plan = _plan;
            accepted = false;
            admin = _admin;
        }
    }
}
