using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Repository
{
    public class UserRepository : IUser
    {
        private readonly ApplicationContext _context;
        private User? _LoggedUser;
        public UserRepository(ApplicationContext context) 
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void RemoveUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.Include(x=>x.Compositions).Include(x=>x.Comments);
        }

        public User GetLoggedUser()
        {
            return _LoggedUser;
        }

        public User GetUser(string id)
        {
            var user=_context.Users.Include(x=>x.Comments).Include(x=>x.Compositions).FirstOrDefault(x=>x.Id == id);
            return user;
        }

        public void SetLoggedUser(User user)
        {
            _LoggedUser = user;
        }
        public void UpdateUser(User user)
        {
            var existingUser = _context.Users.Find(user.Id);

            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;

                _context.SaveChanges();
            }
        }
        public void DeleteUser(string userId)
        {
            var user = _context.Users.FirstOrDefault(x=>x.Id==userId);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
