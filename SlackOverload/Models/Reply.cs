using Microsoft.AspNetCore.Identity;
using SlackOverload.Models;

namespace SlackOverload.Models
{
    public class Reply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string UserName { get; set; }
        public int UserReputation { get; set; }
        public DateTime DateTime { get; set; }
        public int Score { get; set; }
        public List<Vote>? Votes { get; set; }

        //
        public virtual Comment? Comment { get; set; }
        public int CommentId { get; set; }
        public string CommentUserName { get; set; }
        public int? CommentUserReputation { get; set; }

        //
        public Reply() { }
        public Reply(Comment comment, ApplicationUser user, string content)

        {
            Comment = comment;
            User = user;
            Content = content;

            UserName = user.UserName;
            UserReputation = user.Reputation;
            DateTime = DateTime.Now;
            Score = 0;

            CommentId = comment.Id;
            CommentUserName = comment.UserName;
            CommentUserReputation = comment.UserReputation;
        }
    }
}
