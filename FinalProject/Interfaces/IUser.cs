using FinalProject.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Interfaces
{
    public interface IUser
    {
        IEnumerable<User> GetAllUsers();
        User GetLoggedUser();
        User GetUser(string username);
        void SetLoggedUser(User user = null);
        void AddUser(User user);
        void RemoveUser(User user);
        void UpdateUser(User user);
        void DeleteUser(string userId);
    }
}
