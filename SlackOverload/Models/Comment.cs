using Microsoft.AspNetCore.Identity;
using SlackOverload.Models;

namespace SlackOverload.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string UserName { get; set; }
        public int UserReputation { get; set; }
        public DateTime DateTime { get; set; }
        public int Score { get; set; }
        public int ReplyCount { get; set; }
        public List<Reply>? Replies { get; set; }
        public List<Vote>? Votes { get; set; }

        public virtual Question? Question { get; set; }
        public int QuestionFunnyNumber { get; set; }
        public string QuestionUserName { get; set; }
        public int? QuestionUserReputation { get; set; }

        //
        public Comment() { }
        public Comment(Question question, ApplicationUser user, string content)

        {
            Question = question;
            User = user;
            Content = content;

            UserName = user.UserName;
            UserReputation = user.Reputation;
            DateTime = DateTime.Now;
            Score = 0;

            QuestionFunnyNumber = question.Id;
            QuestionUserName = question.UserName;
            QuestionUserReputation = question.UserReputation;
        }
    }
}
