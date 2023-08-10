using ForumUniversitario.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumUniversitario.Entidades
{
    public class Membership
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public ForumUniversitarioUser User { get; set; }

        [ForeignKey("Community")]
        public int CommunityId { get; set; }
        public Community Community { get; set; }

        public DateTime JoinDate { get; set; }

        public int controlLevel { get; set; }
    }
}


