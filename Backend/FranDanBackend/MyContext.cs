using FranDanBackend.Models;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Net.Mail;
using System.Linq;

namespace FranDanBackend
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<Verifier> Verifiers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Participation> Participations { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=FranDanDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            var mailAddressConverter = new ValueConverter<MailAddress, string>(
                v => v.Address,
                v => new MailAddress(v));

            modelBuilder.Entity<User>()
                .Property(u => u.email)
                .HasConversion(mailAddressConverter);

            modelBuilder.Entity<Plan>()
                .HasOne(p => p.creator)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.friend1)
                .WithMany()
                .HasForeignKey(f => f.friend1Id)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.friend2)
                .WithMany()
                .HasForeignKey(f => f.friend2Id)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Participation>()
                .HasOne(p => p.user)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Participation>()
                .HasOne(p => p.plan)
                .WithMany()
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public User getUserById(int id) {
            User foundUser = Users.Find(id);
            if (foundUser == null) throw new Exception("No user found!");
            else return foundUser;
        }
        public bool existUserByEmail(string email)
        {
            string processedEmail = email.Trim().ToLower();
            MailAddress searchAddress = new MailAddress(processedEmail);
            return Users.Any(user => user.email == searchAddress);
        }
        public User getUserByEmail(string email)
        {
            string processedEmail = email.Trim().ToLower();
            MailAddress searchAddress = new MailAddress(processedEmail);
            User foundUser = Users.Include(u => u.verifier).FirstOrDefault(user => user.email == searchAddress);
            if (foundUser == null) throw new Exception("No user found!");
            else return foundUser;
        }
        public bool existUserByUsername(string username)
        {
            string processedUsername = username.Trim().ToLower();
            return Users.Any(user => user.username.ToLower() == processedUsername);
        }
        public User getUserByUsername(string username)
        {
            string processedUsername = username.Trim().ToLower();
            User foundUser = Users.Include(u => u.verifier).FirstOrDefault(user => user.username.ToLower() == processedUsername);
            if (foundUser == null) throw new Exception("No user found!");
            else return foundUser;
        }
        public User getUserByUsernameOrEmail(string usernameOrEmail)
        {
            try
            {
                return getUserByEmail(usernameOrEmail);
            }
            catch (Exception e) {
                return getUserByUsername(usernameOrEmail);
            }
        }
        public Friendship.FriendshipState getFriendshipState(User user1, User user2)
        {
            Friendship potentialFriendShip;
            potentialFriendShip = Friendships
                .Include(u => u.friend1)
                .Include(u => u.friend2)
                .FirstOrDefault(friendship => (friendship.friend1 == user1 && friendship.friend2 == user2));
            if(potentialFriendShip != null) return potentialFriendShip.getFriendshipState(false);

            potentialFriendShip = Friendships
                .Include(u => u.friend1)
                .Include(u => u.friend1)
                .FirstOrDefault(friendship => (friendship.friend1 == user2 && friendship.friend2 == user1));
            if (potentialFriendShip != null) return potentialFriendShip.getFriendshipState(true);
            else return Friendship.FriendshipState.NONE;

        }
        public Friendship getFriendship(User user1, User user2)
        {
            Friendship potentialFriendShip;
            potentialFriendShip = Friendships
                .Include(u => u.friend1)
                .Include(u => u.friend2)
                .FirstOrDefault(friendship => (friendship.friend1 == user1 && friendship.friend2 == user2) || (friendship.friend1 == user2 && friendship.friend2 == user1));
            if (potentialFriendShip == null) throw new Exception("No friendship found");
            else return potentialFriendShip;
        }
        public List<User> getAllFriends(User user)
        {
            List<Friendship> usersFriendships = new List<Friendship>(Friendships
                .Include(u => u.friend1)
                .Include(u => u.friend2)
                .Where(friendship => friendship.accepted && (friendship.friend1 == user || friendship.friend2 == user)));
            List<User> friends=new List<User> ();
            foreach (Friendship  friendship in usersFriendships)
            {
                friends.Add(user== friendship.friend2 ? friendship.friend1:friendship.friend2);
            }
            return friends;
        }
        public List<User> getAllFriendInvitators(User user)
        {
            List<Friendship> usersFriendshipsInvitations = new List<Friendship>(Friendships
                .Include(u => u.friend1)
                .Include(u => u.friend2)
                .Where(friendship => !friendship.accepted && !friendship.blacklisted && friendship.friend2 == user));
            List<User> friendInvitators = new List<User>();
            foreach (Friendship friendshipInvitation in usersFriendshipsInvitations)
            {
                friendInvitators.Add(user == friendshipInvitation.friend2 ? friendshipInvitation.friend1 : friendshipInvitation.friend2);
            }
            return friendInvitators;
        }
        public Plan getPlanById(int id)
        {
            Plan foundPlan = Plans.Find(id);
            if (foundPlan == null) throw new Exception("No plan found!");
            else return foundPlan;
        }
        public Participation getParticipation(User user, Plan plan)
        {
            Participation potentialParticipation;
            potentialParticipation = Participations
                .Include(p => p.user)
                .Include(p => p.plan)
                    .ThenInclude(pl => pl.creator)
                .FirstOrDefault(participation => participation.user==user && participation.plan==plan);
            if (potentialParticipation == null) throw new Exception("No participation in plan found");
            else return potentialParticipation;
        }
        public Participation.Role getParticipationRole(User user, Plan plan)
        {
            Participation potentialParticipation;
            potentialParticipation = Participations
                .Include(p => p.user)
                .Include(p => p.plan)
                    .ThenInclude(pl => pl.creator)
                .FirstOrDefault(participation => participation.user == user && participation.plan == plan);
            if (potentialParticipation == null) return Participation.Role.NONE;
            if (potentialParticipation.plan.creator == user) return Participation.Role.CREATOR;
            if (!potentialParticipation.accepted)
                if (potentialParticipation.admin)
                    return Participation.Role.INVITED_ADMIN;
                else 
                    return Participation.Role.INVITED;
            else 
                if (potentialParticipation.admin)
                    return Participation.Role.ADMIN;
                   else
                       return Participation.Role.PARTICIPANT;
        }
        public List<(Plan,bool,bool)> getAllPlans(User user)
        {
            List<Participation> usersParticipations = new List<Participation>(Participations
                .Include(p => p.plan)
                    .ThenInclude(pl => pl.creator)
                .Include(p => p.user)
                .Where(participation => participation.accepted && participation.user==user));
            List<(Plan, bool, bool)> usersPlans = new List<(Plan, bool, bool)>();
            foreach (Participation participation in usersParticipations)
            {
                usersPlans.Add((participation.plan,participation.plan.creator==user,participation.admin));
            }
            return usersPlans;
        }
        public List<(Plan,bool)> getAllPlanInvitations(User user)
        {
            List<Participation> usersParticipations = new List<Participation>(Participations
                .Include(p => p.plan)
                    .ThenInclude(pl => pl.creator)
                .Include(p => p.user)
                .Where(participation => !participation.accepted && participation.user == user));
            List<(Plan,bool)> usersPlans = new List<(Plan,bool)>();
            foreach (Participation participation in usersParticipations)
            {
                usersPlans.Add((participation.plan,participation.admin));
            }
            return usersPlans;
        }
        public List<(User, bool,bool,bool)> getAllPlanParticipants(Plan plan)
        {
            List<Participation> planParticipations = new List<Participation>(Participations
                .Include(p => p.plan)
                    .ThenInclude(pl => pl.creator)
                .Include(p => p.user)
                .Where(participation => participation.plan==plan));
            List<(User, bool,bool,bool)> usersPlans = new List<(User, bool,bool,bool)>();
            foreach (Participation participation in planParticipations)
            {
                usersPlans.Add((participation.user,participation.accepted, plan.creator==participation.user, participation.admin));
            }
            return usersPlans;
        }
    }

}
