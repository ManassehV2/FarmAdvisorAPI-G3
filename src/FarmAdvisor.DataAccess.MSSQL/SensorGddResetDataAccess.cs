using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL
{
    public class SensorGddResetDataAccess
    {
        private readonly FarmAdvisorDbContext _dbContext = new FarmAdvisorDbContext();

        public SensorGddReset add(SensorGddReset sensorGddReset)
        {
            _dbContext.Add(sensorGddReset);
            _dbContext.SaveChanges();
            return sensorGddReset;
        }

        public SensorGddReset[] getBySensorId(Guid id)
        {
            var sensorGddResets = _dbContext.SensorGddResets.Where(sensorGddReset => sensorGddReset.SensorId == id);
            return sensorGddResets.ToArray<SensorGddReset>();
        }
    }
}