using FinalProject.Models;

namespace FinalProject.Interfaces
{
    public interface IComment
    {
         IEnumerable<Comment> GetAllComments();
         IEnumerable<Comment> GetCommentsByComposition(string compositionId);
        IEnumerable<Comment> GetCommentsByUser(string userId);
        void AddComment(Comment comment);   
    }
}
