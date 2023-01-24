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

        public User? getById(string userId)
        {
            var users = _dbContext.Users.Where(user => user.userId.ToString() == userId);
            return users.FirstOrDefault();
        }

        public User? getByPhone(string phone)
        {
            var users = _dbContext.Users.Where(user => user.phone == phone);
            return users.FirstOrDefault();
        }

        public User? getByCredentials(string phone, string passwordHash){
            var users = _dbContext.Users.Where(user => user.phone == phone).Where(user => user.passwordHash == passwordHash);
            return users.FirstOrDefault();
        }
    }
}