using FranDanBackend;
using FranDanBackend.DTO;
using FranDanBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Net.Mail;

namespace FranDanBackend.Services
{
    public class FriendService
    {
        public MyContext context {  get; set; }
        public FriendService(MyContext _context, JWTGenerator _generator)
        {
            context = _context;
        }
        
       public void inviteFriend(int invitorId,UserFindDTO dto)
       {
           User invitorUser,invitedUser;
           invitorUser=context.getUserById(invitorId);
           invitedUser = context.getUserByUsernameOrEmail(dto.usernameOrEmail);
           if (invitorUser == invitedUser) throw new Exception("Can't be friend with yourselve!");
           switch (context.getFriendshipState(invitorUser,invitedUser))
           {
               case Friendship.FriendshipState.FRIENDS: throw new Exception("Already friends!");
               case Friendship.FriendshipState.INVITED1: throw new Exception("Request already send! Wait for response.");
               case Friendship.FriendshipState.INVITED2: throw new Exception("You have been invited to be a friend! Answear instead of inviting.");
               case Friendship.FriendshipState.BLACKLISTED1: throw new Exception("You are blacklisted. You can't send another request after beeing rejected.");
               case Friendship.FriendshipState.NONE: break;
               case Friendship.FriendshipState.BLACKLISTED2:
                   context.Friendships.Remove(context.getFriendship(invitorUser, invitedUser));
                   break;
           }
           Friendship newFriendship=new Friendship(invitorUser,invitedUser);
           context.Friendships.Add(newFriendship);
           context.SaveChanges();
           if(invitedUser.emailNotifications) MailSender.sendFriendRequest(invitorUser, invitedUser);
       }
        public void acceptFriend(int invitedId, UserIdDTO dto)
        {
            User invitorUser, invitedUser;
            invitedUser = context.getUserById(invitedId);
            invitorUser = context.getUserById(dto.id);
            if (invitorUser == invitedUser) throw new Exception("Can't be friend with yourselve!");
            switch (context.getFriendshipState(invitorUser, invitedUser))
            {
                case Friendship.FriendshipState.INVITED1: break;
                default: throw new Exception("There was no invitation!");
            }

            Friendship toAcceptFriendship = context.getFriendship(invitedUser, invitorUser);
            toAcceptFriendship.accepted = true;
            context.SaveChanges();
        }
        public void rejectFriend(int invitedId, UserIdDTO dto)
        {
            User invitorUser, invitedUser;
            invitedUser = context.getUserById(invitedId);
            invitorUser = context.getUserById(dto.id);
            switch (context.getFriendshipState(invitorUser, invitedUser))
            {
                case Friendship.FriendshipState.INVITED1: break;
                default: throw new Exception("There was no invitation!");
            }

            Friendship toRejectFriendship = context.getFriendship(invitedUser, invitorUser);
            toRejectFriendship.blacklisted=true;
            context.SaveChanges();
        }
        public void deleteFriend(int removerId, UserIdDTO dto)
        {
            User removerUser, removedUser;
            removerUser = context.getUserById(removerId);
            removedUser = context.getUserById(dto.id);
            switch (context.getFriendshipState(removerUser, removedUser))
            {
                case Friendship.FriendshipState.FRIENDS: break;
                default: throw new Exception("You aren't friends!");
            }
            Friendship toDeleteFriendship = context.getFriendship(removerUser, removedUser);
            if (removerUser == toDeleteFriendship.friend1)
            {
                toDeleteFriendship.friend1Id=removedUser.id;
                toDeleteFriendship.friend1 = removedUser;
                toDeleteFriendship.friend2Id = removerUser.id;
                toDeleteFriendship.friend2 = removerUser;
            }
            toDeleteFriendship.accepted = false;
            toDeleteFriendship.blacklisted = true;
            context.SaveChanges();
        }
        /*
       public void acceptFriend(int invitorId, UserIdDTO dto)
       {
           User invitedUser = context.Users.Find(invitorId);
           if (invitedUser == null) throw new Exception("No user found!");
           User invitorUser = context.Users.Find(dto.id);
           if (invitorUser == null) throw new Exception("No user found!");
           invitedUser.inviteFriend(invitorUser);
           context.SaveChanges();
       }
       public void rejectFriend(int invitorId, UserIdDTO dto)
       {
           User invitedUser = context.Users.Find(invitorId);
           if (invitedUser == null) throw new Exception("No user found!");
           User invitorUser = context.Users.Find(dto.id);
           if (invitorUser == null) throw new Exception("No user found!");
           invitedUser.rejectFriend(invitorUser);
           context.SaveChanges();
       }
       public void removeFriend(int invitorId, UserIdDTO dto)
       {
           User removerUser = context.Users.Find(invitorId);
           if (removerUser == null) throw new Exception("No user found!");
           User removedUser = context.Users.Find(dto.id);
           if (removedUser == null) throw new Exception("No user found!");
           removerUser.removeFriend(removedUser);
           context.SaveChanges();
       }
       public UserFullDTO fullInfo(int userId)
       {
           User user = context.Users.Find(userId);
           if (user == null) throw new Exception("No user found!");
           return user.toUserFullDTO();

       }
       */
    }
}
