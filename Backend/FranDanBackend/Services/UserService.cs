using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.SecurityTokenService;
using System.IO;
using System.Numerics;

namespace FranDanBackend.Services
{
    public class UserService
    {
        public MyContext context { get; set; }
        public UserService(MyContext _context)
        {
            context = _context;
        }
        public UserFullDTO getUserFullDTO(int userId)
        {
            User user = context.getUserById(userId);

            List<User> friendsList = context.getAllFriends(user);
            List<UserProtectedDTO> friendsListDTO = new List<UserProtectedDTO>();
            foreach (User friend in friendsList)
            {
                friendsListDTO.Add(friend.toProtectedDTO());
            }
            friendsListDTO = friendsListDTO.OrderBy(f => f.days_to_birthday).ToList();

            List<User> friendInvitationsList = context.getAllFriendInvitators(user);
            List<UserProtectedDTO> friendInvitationsListDTO = new List<UserProtectedDTO>();
            foreach (User friend in friendInvitationsList)
            {
                friendInvitationsListDTO.Add(friend.toProtectedDTO());
            }
            friendInvitationsListDTO = friendInvitationsListDTO.OrderBy(f => f.days_to_birthday).ToList();

            var plansList = context.getAllPlans(user);
            List<PlanHeaderDTO> plansListDTO = new List<PlanHeaderDTO>();
            foreach (var (plan, creator, admin) in plansList)
            {
                plansListDTO.Add(plan.toPlanHeaderDTO(creator, admin));
            }

            var planInvitationsList = context.getAllPlanInvitations(user);
            List<PlanHeaderDTO> planInvitationsListDTO = new List<PlanHeaderDTO>();
            foreach (var (plan, admin) in planInvitationsList)
            {
                planInvitationsListDTO.Add(plan.toPlanHeaderDTO(false, admin));
            }
            return new UserFullDTO
            {
                id = user.id,
                username = user.username,
                email = user.email.Address,
                occupation = user.occupation,
                birthday = user.birthday.ToString("dd.MM.yyyy"),
                friends = friendsListDTO,
                friendInvitations = friendInvitationsListDTO,
                plans = plansListDTO,
                planInvitations = planInvitationsListDTO
            };
        }
        public void delete(int userId)
        {
            User deleteUser = context.getUserById(userId);
            context.Users.Remove(deleteUser);
            context.SaveChanges();
        }
        public void edit(int userId, UserEditDTO dto)
        {
            User editUser = context.getUserById(userId);
            editUser.username = dto.username;
            editUser.emailNotifications = dto.emailNotifications;
            editUser.occupation = dto.occupation;
            try { editUser.birthday = DateOnly.Parse(dto.birthday); } catch (Exception) { throw new Exception("Date-exception"); }
            context.SaveChanges();
        }
    }
}
