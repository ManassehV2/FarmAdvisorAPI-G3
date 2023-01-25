using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL
{
    public class FarmDataAccess
    {
        //key declaration
        private FarmAdvisorDbContext _dbContext;
        public FarmDataAccess()
        {
            _dbContext = new FarmAdvisorDbContext();
        }

        public Farm add(Farm farm)
        {
            _dbContext.Add(farm);
            _dbContext.SaveChanges();
            return farm;
        }

        public Farm? getById(Guid id)
        {
            var farms = _dbContext.Farms.Where(farm => farm.FarmId == id);
            return farms.FirstOrDefault();
        }

        public Farm[] getByUserId(Guid id)
        {
            var farms = _dbContext.Farms.Where(farm => farm.UserId == id);
            return farms.ToArray<Farm>();
        }

        public Farm? getByUserAndFarmId(Guid userId, Guid farmId)
        {
            var farms = _dbContext.Farms.Where(farm => farm.FarmId == farmId).Where(farm => farm.UserId == userId);
            return farms.FirstOrDefault();
        }

        public Farm? deleteByUserAndFarmId(Guid userId, Guid farmId)
        {
            var farms = _dbContext.Farms.Where(farm => farm.FarmId == farmId).Where(farm => farm.UserId == userId);
            Farm? farm = farms.FirstOrDefault();
            if (farm == null)
                return null;
            _dbContext.Remove(farm);
            _dbContext.SaveChanges();
            return farm;
        }

        public Farm? updateByUserAndFarmId(Guid userId, Guid farmId, FarmUpdate farmUpdate )
        {
            var farms = _dbContext.Farms.Where(farm => farm.FarmId == farmId).Where(farm => farm.UserId == userId);
            Farm? farm = farms.FirstOrDefault();
            if (farm == null)
                return null;
            farm.Name = farmUpdate.Name ?? farm.Name;
            farm.Postcode = farmUpdate.Postcode ?? farm.Postcode;
            farm.City = farmUpdate.City ?? farm.City ;
            farm.Country = farmUpdate.Country ?? farm.Country;
            _dbContext.SaveChanges();
            return farm;
        }
    }
}