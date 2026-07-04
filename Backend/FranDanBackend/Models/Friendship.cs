
using FranDanBackend.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Numerics;

namespace FranDanBackend.Models {
    public class Friendship
    {
        public enum FriendshipState {
            NONE,
            FRIENDS,
            INVITED1,
            INVITED2,
            BLACKLISTED1,
            BLACKLISTED2
        }
        [Key]
        public int id { get; set; }
        public int friend1Id { get; set; }
        [ForeignKey(nameof(friend1Id))]
        public User friend1 {  get; set; }
        public int friend2Id { get; set; }
        [ForeignKey(nameof(friend2Id))]
        public User friend2 { get; set; }
        public bool accepted {  get; set; }
        public bool blacklisted { get; set; }
        public Friendship() {}
        public Friendship(User _friend1, User _friend2)
        {
            friend1Id = _friend1.id;
            friend1 = _friend1;
            friend2Id = _friend2.id;
            friend2 = _friend2;
            accepted = false;
            blacklisted = false;
        }
        public FriendshipState getFriendshipState(bool reverse=false)
        {
            if (accepted)
                return FriendshipState.FRIENDS;
            else
                if(blacklisted)
                    if(!reverse)
                        return FriendshipState.BLACKLISTED1;
                    else
                        return FriendshipState.BLACKLISTED2;
                else
                    if (!reverse)
                        return FriendshipState.INVITED1;
                    else
                        return FriendshipState.INVITED2;
        }
    }
}
