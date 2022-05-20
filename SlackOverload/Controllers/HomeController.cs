using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SlackOverload.Data;
using SlackOverload.Models;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SlackOverload.Controllers
{
    public class HomeController : Controller

    {
        public ApplicationDbContext db { get; set; }
        public UserManager<ApplicationUser> manager { get; set; }


        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext newContext, UserManager<ApplicationUser> Manager)
        {
            _logger = logger;
            db = newContext;
            manager = Manager;
        }

        public IActionResult Index()
        {
            int UserCount = 0;
            try
            {
                if (db.Users != null)
                {
                    if (db.Users.Any())
                        UserCount = db.Users.Count();
                }

            } catch(Exception ex)
            {
                return View();
            }

            ViewBag.UserCount = UserCount;

            return View();
        }


        public static int ScoreMultiplier = 1;
        public static int RepMultiplier = 5;

        [Authorize(Roles = "Admin")]
        public IActionResult AdminAllQuestions()
        {
            if (db.Questions.Any())
            {
                foreach (Question _question in db.Questions)
                {
                    _question.CommentCount = CalculateCommentCount(_question.Id);
                    _question.Score = CalculateScore(_question.Id, "Question");
                }
                db.SaveChanges();
            }

            List<Question> AllQuestions = db.Questions.ToList();
            return View(AllQuestions);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AdminAllQuestions(string? SearchKey)
        {
            
            List<Question> AllQuestions = new List<Question>();

            if (SearchKey != null)
            {
                string[] SearchKeyWords = SearchKey.ToLower().Split(',', '.', ' ');
                foreach (string SearchKeyWord in SearchKeyWords)
                {
                    AllQuestions.AddRange(db.Questions.Where(q => q.Title.ToLower().Contains(SearchKeyWord)).ToList());
                    AllQuestions.AddRange(db.Questions.Where(q => q.Content.ToLower().Contains(SearchKeyWord)).ToList());
                }
                
            }
            else
            {
                AllQuestions = db.Questions.ToList();
            }
            

            return View(AllQuestions);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteQuestion(int QuestionId)
        {
            Question TheQuestion = db.Questions.First(q => q.Id == QuestionId);
            UpdateVoteList(QuestionId, "Question");
            if (TheQuestion.Votes.Count() > 0)
            {
                List<Vote> VotesToDelete = db.Votes.Where(v => v.Question.Id == TheQuestion.Id).ToList();
                db.Votes.RemoveRange(VotesToDelete);
                db.SaveChanges();
            }
            if (TheQuestion.CommentCount > 0)
            {
                List<Comment> Comments = db.Comments.Where(c => c.QuestionFunnyNumber == QuestionId).ToList();
                foreach (Comment comment in Comments)
                {
                    UpdateVoteList(comment.Id, "Comment");
                    if (comment.Votes.Count() > 0)
                    {
                        List<Vote> VotesToDelete = db.Votes.Where(v => v.Comment.Id == comment.Id).ToList();
                        db.Votes.RemoveRange(VotesToDelete);
                        db.SaveChanges();
                    }
                    comment.ReplyCount = CalculateReplies(comment.Id);
                    if (comment.ReplyCount > 0)
                    {
                        List<Reply> Replies = db.Replies.Where(r => r.CommentId == comment.Id).ToList();
                        foreach(Reply reply in Replies)
                        {
                            UpdateVoteList(reply.Id, "Reply");
                            if (reply.Votes.Count() > 0)
                            {
                                List<Vote> VotesToDelete = db.Votes.Where(v => v.Reply.Id == reply.Id).ToList();
                                db.Votes.RemoveRange(VotesToDelete);
                                db.SaveChanges();
                            }
                        }
                        db.Replies.RemoveRange(Replies);
                        db.SaveChanges();
                        db.Comments.Remove(comment);
                    }
                }
            }
           

            db.Questions.Remove(TheQuestion);
            db.SaveChanges();

            return Redirect($"AdminAllQuestions");
        }

        public ApplicationUser GetUser()
        {
            try
            {
                ApplicationUser TheUser = (ApplicationUser)db.Users.First(u => u.UserName == User.Identity.Name);
                return TheUser;
            } catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public string[] GetTags(int QuestionId)
        {
            Question TheQuestion = db.Questions.First(q => q.Id == QuestionId);
            if (TheQuestion.Tags != null)
            {
                string[] Tags = TheQuestion.Tags.Split(',');
                return Tags;
            }
            else
                return null;
                
        }
        public IActionResult AddTag(int QuestionId, string TagToAdd)
        {
            try
            {
                if (TagToAdd != null)
                {
                    if (TagToAdd.Length > 0)
                    {
                        Question TheQuestion = db.Questions.First(q => q.Id == QuestionId);
                        TheQuestion.Tags += $",{TagToAdd.ToUpper()}";
                        db.SaveChanges();
                        return Redirect($"ViewQuestion?QuestionId={QuestionId}");
                    }
                    return Redirect($"ViewQuestion?QuestionId={QuestionId}");
                }
                
            }catch (Exception ex)
            {
                return Redirect($"ViewQuestion?QuestionId={QuestionId}");
            }
            
            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        public IActionResult RemoveTag(int QuestionId, string TagToRemove)
        {
            Question TheQuestion = db.Questions.First(q => q.Id == QuestionId);
            string[] SplitTags = TheQuestion.Tags.Split(',');
            string[] NewSplitTags = new string[SplitTags.Length];
            foreach (string tag in SplitTags)
            {
                if (tag != TagToRemove)
                {
                    NewSplitTags.Append(tag);
                }
            }
            string NewTags = String.Join(",", NewSplitTags);
            TheQuestion.Tags = NewTags;
            db.SaveChanges();

            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        public int CalculateReplies(int CommentId)
        {
            int Count = 0;

            List<Reply> CommentReplies = db.Replies.Where(r => r.CommentId == CommentId).ToList();
            Count = CommentReplies.Count;
                
            return Count;
        }
        public int CalculateCommentCount(int QuestionId)
        {
            int Count = 0;

            List<Comment> QuestionComments = db.Comments.Where(c => c.QuestionFunnyNumber == QuestionId).ToList();
            Count = QuestionComments.Count();
            foreach(Comment comment in QuestionComments)
            {
                Count += CalculateReplies(comment.Id);
            }

            return Count;
        }

        public void UpdateVoteList(int Id, string Type)
        {

            List<Vote> Votes = db.Votes.Where(v => v.PostId == Id && v.PostType == Type).ToList();
            if (Type == "Question")
            {
                Question TheQuestion = db.Questions.First(q => q.Id == Id);
                TheQuestion.Votes = Votes;
            }
            if (Type == "Comment")
            {
                Comment TheComment = db.Comments.First(c => c.Id == Id);
                TheComment.Votes = Votes;
            }
            if (Type == "Reply")
            {
                Reply TheReply = db.Replies.First(r => r.Id == Id);
                TheReply.Votes = Votes;
            }
            db.SaveChanges();
        }

        public int CalculateScore(int Id, string Type)
        {
            int Score = 0;

            List<Vote> Votes = new List<Vote>();
            Votes = db.Votes.Where(v => v.PostId == Id && v.PostType == Type).ToList();

            foreach(Vote vote in Votes)
            {
                if (vote.VoteMade == "Upvote")
                {
                    Score += ScoreMultiplier;
                }
                if (vote.VoteMade == "Downvote")
                {
                    Score -= ScoreMultiplier;
                }
            }
            return Score;
        }

        public int CalculateReputation(string UserName)
        {
            int Reputation = 0;

            List<Vote> Votes = db.Votes.Where(v => v.PostUserName == UserName).ToList();
            foreach(Vote vote in Votes)
            {
                if (vote.VoteMade == "Upvote")
                {
                    Reputation += RepMultiplier;
                }
                if (vote.VoteMade == "Downvote")
                {
                    Reputation -= RepMultiplier;
                }
            }

            return Reputation;
        }

        public List<Question> GetQuestionsByTag(string Tag)
        {
            List<Question> QuestionsToReturn = new List<Question>();
            List<Question> DbQuestions = db.Questions.ToList();
            try
            {
                foreach (Question question in DbQuestions)
                {
                    string[]? Tags = GetTags(question.Id);
                    if (Tags != null)
                    {
                        if (Tags.Contains(Tag.ToUpper()))
                            QuestionsToReturn.Add(question);
                    }
                }
            } catch(Exception ex)
            {

            }
            
            return QuestionsToReturn;
        }
        public IActionResult AllQuestions(int? SelectedPage, int? SelectedQuestionsPerPage, string? SelectedSortBy, string? Tag)
        {
            
            int Page = 0;
            int QuestionsPerPage = 5;
            string Sort = "MostRecent";
            int QuestionsCount = 0;
            

            if (SelectedPage != null)
                Page = (int)SelectedPage;

            if (SelectedQuestionsPerPage != null)
                QuestionsPerPage = (int)SelectedQuestionsPerPage;

            if (SelectedSortBy != null)
                Sort = (string)SelectedSortBy;

            ViewData["SelectedPage"] = Page;
            ViewData["SelectedQuestionsPerPage"] = QuestionsPerPage;
            ViewData["SelectedSortBy"] = Sort;

            if (db.Questions.Any())
            {
                QuestionsCount = db.Questions.Count();
                foreach(Question _question in db.Questions)
                {
                    _question.CommentCount = CalculateCommentCount(_question.Id);
                    _question.Score = CalculateScore(_question.Id, "Question");
                }
                db.SaveChanges();
            }

            List<Question> DbQuestions = new List<Question>();

            ViewData["QuestionsCount"] = QuestionsCount;

            if (Tag == null)
                DbQuestions = db.Questions.OrderByDescending(q => q.Id).ToList();
            else
                DbQuestions = GetQuestionsByTag(Tag);

            int PagesCount = DbQuestions.Count() / QuestionsPerPage;
            ViewData["PagesCount"] = PagesCount;
            int PageStart = QuestionsPerPage * Page;


            if (Sort == "MostAnswers")
            {
                DbQuestions = DbQuestions.OrderByDescending(q => q.CommentCount).ToList();
            }

            List<Question> QuestionsToShow = new List<Question>();
            for (int i = PageStart; i < PageStart + QuestionsPerPage && i < DbQuestions.Count(); i++)
            {
                //DbQuestions[i].CommentCount = CalculateCommentCount(DbQuestions[i].Id);
                //DbQuestions[i].Score = CalculateScore(DbQuestions[i].Id, "Question");
                QuestionsToShow.Add(DbQuestions[i]);
            }

            return View(QuestionsToShow);
        }

        public IActionResult ViewQuestion(int QuestionId)
        {

            Question Question = db.Questions.First(q => q.Id == QuestionId);
            ApplicationUser QuestionUser = db.Users.First(u => u.UserName == Question.UserName);

            ViewData["OP"] = false;
            if (GetUser() != null)
            {
                if (Question.UserName == GetUser().UserName)
                {
                    ViewData["OP"] = true;
                }
            }


            Question.Score = CalculateScore(Question.Id, "Question");
            QuestionUser.Reputation = CalculateReputation(QuestionUser.UserName);
            Question.UserReputation = QuestionUser.Reputation;
            if (Question.Tags != null)
            {
                ViewData["QuestionTags"] = GetTags(Question.Id);
            }

            Comment CorrectAnswer = null;
            if (Question.CorrectAnswerFunnyNumber != null)
            {
                CorrectAnswer = db.Comments.First(c => c.Id == Question.CorrectAnswerFunnyNumber);
                ApplicationUser CorrectAnswerUser = db.Users.First(u => u.UserName == CorrectAnswer.UserName);
                CorrectAnswer.Score = CalculateScore(CorrectAnswer.Id, "Comment");
                CorrectAnswerUser.Reputation = CalculateReputation(CorrectAnswerUser.UserName);
                CorrectAnswer.UserReputation = CorrectAnswerUser.Reputation;

                int i = 0;
                List<Reply> Replies = db.Replies.Where(r => r.CommentId == CorrectAnswer.Id).ToList();
                CorrectAnswer.ReplyCount = Replies.Count;
                foreach (Reply reply in Replies)
                {
                    reply.Score = CalculateScore(reply.Id, "Reply");
                    ApplicationUser ReplyUser = db.Users.First(u => u.UserName == reply.UserName);
                    ReplyUser.Reputation = CalculateReputation(ReplyUser.UserName);
                    reply.UserReputation = ReplyUser.Reputation;

                    ViewData[$"comment-{CorrectAnswer.Id}-reply-{i}-id"] = reply.Id;
                    ViewData[$"comment-{CorrectAnswer.Id}-reply-{i}-content"] = reply.Content;
                    ViewData[$"comment-{CorrectAnswer.Id}-reply-{i}-score"] = reply.Score;
                    ViewData[$"comment-{CorrectAnswer.Id}-reply-{i}-username"] = reply.UserName;
                    ViewData[$"comment-{CorrectAnswer.Id}-reply-{i}-userreputation"] = reply.UserReputation;
                    i++;
                }
                CorrectAnswer.Replies = Replies;
            }
            ViewData["CorrectAnswer"] = CorrectAnswer;

            if (db.Comments.Where(c => c.QuestionFunnyNumber == QuestionId).Any())
            {
                List<Comment> Comments = db.Comments.Where(c => c.QuestionFunnyNumber == QuestionId).ToList();
                foreach (Comment comment in Comments)
                {
                    ApplicationUser CommentUser = db.Users.First(u => u.UserName == comment.UserName);
                    CommentUser.Reputation = CalculateReputation(CommentUser.UserName);
                    comment.UserReputation = CommentUser.Reputation;
                    comment.Score = CalculateScore(comment.Id, "Comment");
                    comment.ReplyCount = CalculateReplies(comment.Id);

                    int i = 0;
                    List<Reply> Replies = db.Replies.Where(r => r.CommentId == comment.Id).ToList();
                    comment.ReplyCount = Replies.Count;
                    foreach (Reply reply in Replies)
                    {
                        reply.Score = CalculateScore(reply.Id, "Reply");
                        ApplicationUser ReplyUser = db.Users.First(u => u.UserName == reply.UserName);
                        ReplyUser.Reputation = CalculateReputation(ReplyUser.UserName);
                        reply.UserReputation = ReplyUser.Reputation;

                        ViewData[$"comment-{comment.Id}-reply-{i}-id"] = reply.Id;
                        ViewData[$"comment-{comment.Id}-reply-{i}-content"] = reply.Content;
                        ViewData[$"comment-{comment.Id}-reply-{i}-score"] = reply.Score;
                        ViewData[$"comment-{comment.Id}-reply-{i}-username"] = reply.UserName;
                        ViewData[$"comment-{comment.Id}-reply-{i}-userreputation"] = reply.UserReputation;
                        i++;
                    }
                    comment.Replies = Replies;
                }

                ViewData["Comments"] = Comments;
            }
            db.SaveChanges();

            return View(Question);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VoteReply(int ReplyId, string VoteMade, int QuestionId)
        {
            ApplicationUser TheUser = GetUser();
            Reply TheReply = db.Replies.First(r => r.Id == ReplyId);
            if (TheUser.UserName != TheReply.UserName)
            {
                bool VoteAlreadyExists = db.Votes.Any(v => v.PostId == ReplyId && v.UserName == TheUser.UserName);

                Vote Vote = null;
                if (VoteAlreadyExists)
                {
                    Vote ExistingVote = db.Votes.First(v => v.PostId == ReplyId && v.UserName == TheUser.UserName);
                    if (ExistingVote.VoteMade == VoteMade)
                    {
                        db.Remove(ExistingVote);
                    }
                    else
                    {
                        db.Remove(ExistingVote);
                        Vote = new Vote(TheUser, TheReply, VoteMade);
                        db.Votes.Add(Vote);
                    }

                }
                else
                {
                    Vote = new Vote(TheUser, TheReply, VoteMade);
                    db.Votes.Add(Vote);
                }
            }
            await db.SaveChangesAsync();

            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VoteComment(int CommentId, string VoteMade, int QuestionId)
        {
            ApplicationUser TheUser = GetUser();
            Comment TheComment = db.Comments.First(c => c.Id == CommentId);
            if (TheUser.UserName != TheComment.UserName)
            {
                bool VoteAlreadyExists = db.Votes.Any(v => v.PostId == CommentId && v.UserName == TheUser.UserName);

                Vote Vote = null;
                if (VoteAlreadyExists)
                {
                    Vote ExistingVote = db.Votes.First(v => v.PostId == CommentId && v.UserName == TheUser.UserName);
                    if (ExistingVote.VoteMade == VoteMade)
                    {
                        db.Remove(ExistingVote);
                    }
                    else
                    {
                        db.Remove(ExistingVote);
                        Vote = new Vote(TheUser, TheComment, VoteMade);
                        db.Votes.Add(Vote);
                    }

                }
                else
                {
                    Vote = new Vote(TheUser, TheComment, VoteMade);
                    db.Votes.Add(Vote);
                }

            }
            await db.SaveChangesAsync();

            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VoteQuestion( int QuestionId, string VoteMade)
        {
            ApplicationUser TheUser = GetUser();
            Question TheQuestion = db.Questions.First(q => q.Id == QuestionId);
            if (TheUser.UserName != TheQuestion.UserName)
            {
                bool VoteAlreadyExists = db.Votes.Any(v => v.PostId == QuestionId && v.UserName == TheUser.UserName);

                Vote Vote = null;
                if (VoteAlreadyExists)
                {
                    Vote ExistingVote = db.Votes.First(v => v.PostId == QuestionId && v.UserName == TheUser.UserName);
                    if (ExistingVote.VoteMade == VoteMade)
                    {
                        db.Remove(ExistingVote);
                    }
                    else
                    {
                        db.Remove(ExistingVote);
                        Vote = new Vote(TheUser, TheQuestion, VoteMade);
                        db.Votes.Add(Vote);
                    }

                }
                else
                {
                    Vote = new Vote(TheUser, TheQuestion, VoteMade);
                    db.Votes.Add(Vote);
                }


            }
            await db.SaveChangesAsync();

            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostReply(int QuestionId, int CommentId, string Content)
        {
            if (Content == null || Content.Trim() == null)
            {
                return Redirect($"ViewQuestion?QuestionId={QuestionId}"); ;
            }
            Comment TheComment = db.Comments.First(c => c.Id == CommentId);
            ApplicationUser TheUser = GetUser();
            Reply NewReply = new Reply(TheComment, TheUser, Content.Trim());

            db.Replies.Add(NewReply);
            await db.SaveChangesAsync();

            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostComment(int QuestionId, string Content)
        {
            if (Content == null || Content.Trim() == null)
            {
                return Redirect($"ViewQuestion?QuestionId={QuestionId}"); ;
            }
            Question TheQuestion = db.Questions.First(q => q.Id == QuestionId);
            ApplicationUser TheUser = GetUser();
            Comment NewComment = new Comment(TheQuestion, TheUser, Content.Trim());

            db.Comments.Add(NewComment);
            await db.SaveChangesAsync();

            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        public async Task<IActionResult> PostQuestion()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostQuestion(string Title, string Content)
        {
            if (Title.Trim() == null || Content.Trim() == null)
            {
                return Redirect("PostQuestion");
            }
            ApplicationUser TheUser = GetUser();
            Question NewQuestion = new Question(TheUser, Title.Trim(), Content.Trim());

            db.Questions.Add(NewQuestion);
            await db.SaveChangesAsync();

            return Redirect($"ViewQuestion?QuestionId={NewQuestion.Id}");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MakeCorrectAnswer(int QuestionId, int CommentId)
        {
            Question TheQuestion = db.Questions.First(q => q.Id == QuestionId);
            if (TheQuestion.CorrectAnswerFunnyNumber == CommentId)
            {
                TheQuestion.CorrectAnswerFunnyNumber = null;
            }
            else
            {
                TheQuestion.CorrectAnswerFunnyNumber = CommentId;
            }

            await db.SaveChangesAsync();

            return Redirect($"ViewQuestion?QuestionId={QuestionId}");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}