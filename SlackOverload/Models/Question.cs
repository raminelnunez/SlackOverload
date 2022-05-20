namespace SlackOverload.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public string UserName { get; set; }
        public int? UserReputation { get; set; }
        public DateTime DateTime { get; set; }
        public int Score { get; set; }
        public string? Tags { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Vote>? Votes { get; set; }

        //

        public string Title { get; set; }
        public int CommentCount { get; set; }
        public int? CorrectAnswerFunnyNumber { get; set; }

        public Question() { }
        public Question(ApplicationUser user, string title, string content)
        {
            User = user;
            UserName = user.UserName;
            UserReputation = user.Reputation;
            Content = content;
            DateTime = DateTime.Now;
            Score = 0;

            Title = title;
            CommentCount = 0;
        }
    }
}
