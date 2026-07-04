
using FranDanBackend.DTO;
using Microsoft.IdentityModel.SecurityTokenService;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mail;
using System.Numerics;

namespace FranDanBackend.Models
{
    public class User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public MailAddress email { get; set; }
        public string passwordHash { get; set; }
        [Column("email_notifications")]
        public bool emailNotifications { get; set; }
        public int verifierId { get; set; }
        [ForeignKey(nameof(verifierId))]
        public Verifier verifier { get; set; }
        public string occupation { get; set; }
        public DateOnly birthday { get; set; }
        public User() { }

        public User(string _username, string _email, bool _emailNotifications, string _occupation, string _passwordHash, string _birthday)
        {
            username = _username;
            email = new MailAddress(_email, _username);
            emailNotifications = _emailNotifications;
            passwordHash = _passwordHash;
            occupation = _occupation;
            try { birthday = DateOnly.Parse(_birthday); } catch (Exception) { throw new Exception("Date-exception"); }
            verifier = new Verifier();
        }
        /*
        public void inviteFriend(User user)
        {
            if (friends.Contains(user)) throw new Exception("Already a friend.");
            if (user.friendRequests.Contains(this)) throw new Exception("Request already send.");
            if (user.blackList.Contains(this)) throw new Exception("Your previous invitation was rejected. You are on black list.");
            blackList.Remove(user);
            user.friendRequests.Add(this);
            if (user.emailNotifications == true)
            {
                MailSender.sendFriendRequest(this, user);
            }
        }
        public void acceptFriend(User user)
        {
            if (friends.Contains(user)) throw new Exception("Already a friend.");
            if (!friendRequests.Contains(user)) throw new Exception("There wasn't any invitation.");
            friendRequests.Remove(user);
            friends.Add(user);
            user.friends.Add(this);
        }
        public void rejectFriend(User user)
        {
            if (friends.Contains(user)) throw new Exception("Already a friend.");
            if (!friendRequests.Contains(user)) throw new Exception("There wasn't any invitation.");
            friendRequests.Remove(user);
            blackList.Add(user);
        }
        public void removeFriend(User user)
        {
            if (!friends.Contains(user)) throw new Exception("Not a friend.");
            friends.Remove(user);
            blackList.Add(user);
        }
        */
        public RequestorDTO toRequestorDTO()
        {
            RequestorDTO dto = new RequestorDTO();
            dto.id = id;
            dto.username = username;
            dto.email = email.Address;
            dto.occupation = occupation;
            return dto;
        }
        public UserProtectedDTO toProtectedDTO()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly birthdayThisYear = new DateOnly(today.Year, birthday.Month, birthday.Day);
            DateOnly birthdayNextYear = new DateOnly(today.Year + 1, birthday.Month, birthday.Day);
            int days_to_birthday = birthdayThisYear.DayNumber - today.DayNumber > 0 ?
                birthdayThisYear.DayNumber - today.DayNumber :
                birthdayNextYear.DayNumber - today.DayNumber;
            UserProtectedDTO dto = new UserProtectedDTO();
            dto.id = id;
            dto.username = username;
            dto.email = email.Address;
            dto.occupation = occupation;
            dto.birthday = birthday.ToString("dd.MM.yyyy");
            dto.days_to_birthday = days_to_birthday;
            return dto;
        }
        public PlanMemberDTO toPlanMemberDTO(bool accepted, bool admin)
        {
            PlanMemberDTO dto = new PlanMemberDTO();
            dto.username = username;
            dto.occupation = occupation;
            dto.accepted = accepted;
            dto.admin = admin;
            return dto;
        }
    }
}
