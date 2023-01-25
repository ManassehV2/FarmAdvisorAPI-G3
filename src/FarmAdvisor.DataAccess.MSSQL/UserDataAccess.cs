using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL
{
    public class UserDataAccess
    {
        //key declaration
        private FarmAdvisorDbContext _dbContext;
        public  UserDataAccess()
        {
            _dbContext = new FarmAdvisorDbContext();
        }

        public User? addUser(User user){
            _dbContext.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public User? getById(Guid userId)
        {
            var users = _dbContext.Users.Where(user => user.UserId == userId);
            return users.FirstOrDefault();
        }

        public User? getByPhone(string phone)
        {
            var users = _dbContext.Users.Where(user => user.Phone == phone);
            return users.FirstOrDefault();
        }

        public User? getByCredentials(string phone, string passwordHash){
            var users = _dbContext.Users.Where(user => user.Phone == phone).Where(user => user.PasswordHash == passwordHash);
            return users.FirstOrDefault();
        }
    }
}