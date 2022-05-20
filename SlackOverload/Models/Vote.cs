namespace SlackOverload.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Question? Question { get; set; }
        public Comment? Comment { get; set; }
        public Reply? Reply { get; set; }
        public string PostType { get; set; }
        public int PostId { get; set; }
        public string PostUserName { get; set; }
        public string VoteMade { get; set; }

        public Vote() { }
        public Vote(ApplicationUser user, Question post, string vote)
        {
            User = user;
            UserId = user.Id;
            UserName = user.UserName;

            Question = post;
            PostType = "Question";
            PostId = post.Id;
            PostUserName = post.UserName;

            VoteMade = vote;
        }

        public Vote(ApplicationUser user, Comment post, string vote)
        {
            User = user;
            UserId = user.Id;
            UserName = user.UserName;

            Comment = post;
            PostType = "Comment";
            PostId = post.Id;
            PostUserName = post.UserName;

            VoteMade = vote;
        }

        public Vote(ApplicationUser user, Reply post, string vote)
        {
            User = user;
            UserId = user.Id;
            UserName = user.UserName;

            Reply = post;
            PostType = "Reply";
            PostId = post.Id;
            PostUserName = post.UserName;

            VoteMade = vote;
        }


    }
}
