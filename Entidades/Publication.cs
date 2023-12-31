﻿using ForumUniversitario.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumUniversitario.Entidades
{
    public class Publication
    {

        public int  Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]

        public string Title { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }
        
        public string UserId { get; set; }

        [NotMapped]
        public string UserName { get; set; }
        public ForumUniversitarioUser User { get; set; }

        [Required(ErrorMessage = "Selecione uma comunidade.")]
        public int CommunityId { get; set; }

        [NotMapped]
        public string CommunityName { get; set; }
        public Community Community { get; set; }

        public List<Like> Likes { get; set; }
    }

}
