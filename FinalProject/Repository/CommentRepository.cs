using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repository
{
    public class CommentRepository : IComment
    {
        private readonly ApplicationContext _context;
        public CommentRepository(ApplicationContext context)
        {
            _context=context;
        }

        public IEnumerable<Comment> GetAllComments()
        {
            return _context.Comments.Include(x=>x.Composition).Include(x=>x.User).ToList();
        }

        public IEnumerable<Comment> GetCommentsByComposition(string compositionId)
        {
            var comments = _context.Comments.Where(x=>x.CompositionId == compositionId).ToList();

            if(!comments.Any())
            {
                comments = new List<Comment>();
            }
            return comments;
        }

        public IEnumerable<Comment> GetCommentsByUser(string userId)
        {
            var comments = _context.Comments.Where(x=>x.UserId == userId).ToList();

            if (!comments.Any())
            {
                comments = new List<Comment>();
            }
            return comments;
        }
        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }
    }
}
