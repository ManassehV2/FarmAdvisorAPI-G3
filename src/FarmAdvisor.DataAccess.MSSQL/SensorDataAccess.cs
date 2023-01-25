using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL
{
    public class SensorDataAccess
    {
        //key declaration
        private FarmAdvisorDbContext _dbContext;
        public SensorDataAccess()
        {
            _dbContext = new FarmAdvisorDbContext();
        }

        public Sensor add(Sensor sensor)
        {
            _dbContext.Add(sensor);
            _dbContext.SaveChanges();
            return sensor;
        }

        public Sensor? getById(Guid id)
        {
            var sensors = _dbContext.Sensors.Where(sensor => sensor.SensorId == id);
            return sensors.FirstOrDefault();
        }

        public Sensor[] getByFieldId(Guid id)
        {
            var sensors = _dbContext.Sensors.Where(sensor => sensor.FieldId == id);
            return sensors.ToArray<Sensor>();
        }

        public Sensor? getByUserAndSensorId(Guid userId, Guid sensorId)
        {
            var sensors = _dbContext.Sensors.Where(sensor => sensor.SensorId == sensorId).Where(sensor => sensor.UserId == userId);
            return sensors.FirstOrDefault();
        }

        public Sensor? deleteByUserAndSensorId(Guid userId, Guid sensorId)
        {
            var sensors = _dbContext.Sensors.Where(sensor => sensor.SensorId == sensorId).Where(sensor => sensor.UserId == userId);
            Sensor? sensor = sensors.FirstOrDefault();
            if (sensor == null)
                return null;
            _dbContext.Remove(sensor);
            _dbContext.SaveChanges();
            return sensor;
        }

        public Sensor? updateByUserAndSensorId(Guid userId, Guid sensorId, SensorUpdate sensorUpdate)
        {
            var sensors = _dbContext.Sensors.Where(sensor => sensor.SensorId == sensorId).Where(sensor => sensor.UserId == userId);
            Sensor? sensor = sensors.FirstOrDefault();
            if (sensor == null)
                return null;
            sensor.FieldId = sensorUpdate.FieldId ?? sensor.FieldId;
            sensor.SerialNo = sensorUpdate.SerialNo ?? sensor.SerialNo;
            sensor.GDD = sensorUpdate.GDD ?? sensor.GDD;
            sensor.DefaultGDD = sensorUpdate.DefaultGDD ?? sensor.DefaultGDD;
            sensor.Long = sensorUpdate.Long ?? sensor.Long;
            sensor.Lat = sensorUpdate.Lat ?? sensor.Lat;
            sensor.InstallationDate = sensorUpdate.InstallationDate ?? sensor.InstallationDate;
            sensor.LastCuttingDate = sensorUpdate.LastCuttingDate ?? sensor.LastCuttingDate;
            sensor.BaseTemperature = sensorUpdate.BaseTemperature ?? sensor.BaseTemperature;
            _dbContext.SaveChanges();
            return sensor;
        }
    }
}