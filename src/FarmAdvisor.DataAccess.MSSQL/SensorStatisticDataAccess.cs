using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL
{
    public class SensorStatisticsDataAccess
    {
        private readonly FarmAdvisorDbContext _dbContext = new FarmAdvisorDbContext();

        public bool addMany(List<SensorStatistic> sensorStatistics)
        {
            _dbContext.AddRange(sensorStatistics);
            _dbContext.SaveChanges();
            return true;
        }

        public SensorStatistic? getBySensorIdAndDate(Guid id, DateTime date)
        {
            var sensorStatistics = _dbContext.SensorStatistics.Where(sensorStatistics => sensorStatistics.Date == date).Where(sensorStatistics => sensorStatistics.SensorId == id);
            return sensorStatistics.FirstOrDefault();
        }

        public SensorStatistic[] getBySensorId(Guid id)
        {
            DateTime today = DateTime.UtcNow.Date;
            var sensorStatistics = _dbContext.SensorStatistics.Where(sensorStatistics => sensorStatistics.SensorId == id).Where(sensorStatistics => sensorStatistics.Date <= today);
            return sensorStatistics.ToArray<SensorStatistic>();
        }
    }
}