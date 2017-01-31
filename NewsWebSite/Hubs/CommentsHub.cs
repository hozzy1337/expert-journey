using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using NewsWebSite.Models.SignalrModels;
using Microsoft.AspNet.Identity;
using System.Threading;
using NewsWebSite.Models;
using NewsWebSite.Models.Repository;
using Microsoft.Security.Application;
using System.Threading.Tasks;

namespace NewsWebSite.Hubs
{
    public class CommentsHub : Hub
    {
        readonly IArticleRepository articleRepository;
        readonly IUserRepository usersRepository;
        readonly ICommentsRepository commentsRepository;
        public CommentsHub(IArticleRepository articleRepository, IUserRepository usersRepository, ICommentsRepository commentsRepository)
        {
            this.articleRepository = articleRepository;
            this.usersRepository = usersRepository;
            this.commentsRepository = commentsRepository;
        }

       // static readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        //static readonly List<CommentsUser> Users = new List<CommentsUser>();

        // Отправка сообщений
        [Authorize]
        public void Send(int articleId = 0, int replyCommentId = 0, string text = "")
        {

            text = Sanitizer.GetSafeHtmlFragment(text);
            if (text.Length <= 0 || text.Length > 100)
                Clients.Caller.InvalidTextLength();
            else
            if (replyCommentId < 0) return;
            Comment comment = new Comment();
            if (replyCommentId > 0)
            {
                var baseComment = commentsRepository.GetCommentInfo(replyCommentId);
                if (baseComment != null)
                {
                    if (articleId != baseComment.ArticleId) return;
                    comment.ArticleId = articleId;
                    comment.Depth = ++baseComment.Depth;
                    comment.Text = text;

                    Clients.Group(articleId.ToString()).addMessage();
                }    
            }
                
        }

        // Подключение нового пользователя
        public void Connect(int articleId)
        {
            var id = Context.ConnectionId;
            Groups.Add(id, articleId.ToString());
            //var user = Context.User;
            //if (!user.Identity.IsAuthenticated) return;
            //if (!Users.Any(u => u.ConnectionId == id))
            //{
            //    locker.EnterWriteLock();
            //    try
            //    {
            //        Users.Add(new CommentsUser { ConnectionId = id, UserName = user.Identity.Name, AppUserId = user.Identity.GetUserId<int>(), ArticleId = articleId });
            //    }
            //    finally
            //    {
            //        locker.ExitWriteLock();
            //    }
            //}
        }

        // Отключение пользователя
        public override Task OnDisconnected(bool stopCalled)
        {
            // var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            //  if (item != null)
            // {
            //locker.EnterWriteLock();
            //try
            //{
            //    Users.Remove(item);
            //}
            //finally
            //{
            //    locker.ExitWriteLock();
            //}
            Groups.Remove(Context.ConnectionId, "");
                var id = Context.ConnectionId;
                //Clients.All.onUserDisconnected(id, item.Name);
    //        }

            return base.OnDisconnected(stopCalled);
        }
    }


}