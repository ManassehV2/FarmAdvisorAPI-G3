using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL
{
    public class FarmDataAccess
    {
        //key declaration
        private FarmAdvisorDbContext _dbContext;
        public  FarmDataAccess()
        {
            _dbContext = new FarmAdvisorDbContext();
        }

        public Farm? add(Farm farm){
            _dbContext.Add(farm);
            _dbContext.SaveChanges();
            return farm;
        }

        public Farm? getById(string id)
        {
            var farms = _dbContext.Farms.Where(farm => farm.FarmId.ToString() == id);
            return farms.FirstOrDefault();
        }
    }
}