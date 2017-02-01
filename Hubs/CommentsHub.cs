﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using NewsWebSite.Models.SignalrModels;
using Microsoft.AspNet.Identity;
using System.Threading;
using NewsWebSite.Models;
using NewsWebSite.Models.Repository;
using Microsoft.Security.Application;
using System.Threading.Tasks;
using System;

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

            var commentDepth = 0;
            if (replyCommentId > 0)
            {
                var baseComment = commentsRepository.GetCommentInfo(replyCommentId);
                if (baseComment == null) return;
                commentDepth = baseComment.Depth + 1;
            }
            else if (!articleRepository.IsExist(articleId)) return;
            Comment comment = new Comment();
            comment.ArticleId = articleId;
            comment.Text = text;
            comment.UserName = Context.User.Identity.Name.Split('@')[0];
            comment.Depth = commentDepth;
            comment.Created = DateTime.Now;
            commentsRepository.Save(comment);
            Clients.Group(articleId.ToString()).addMessage(comment.UserName, text, comment.Created, 0, 0);
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


    }


}