using FarmAdvisor.Models;

namespace FarmAdvisor.DataAccess.MSSQL
{
    public class FieldDataAccess
    {
        //key declaration
        private FarmAdvisorDbContext _dbContext;
        public FieldDataAccess()
        {
            _dbContext = new FarmAdvisorDbContext();
        }

        public Field add(Field field)
        {
            _dbContext.Add(field);
            _dbContext.SaveChanges();
            return field;
        }

        public Field? getById(Guid id)
        {
            var fields = _dbContext.Fields.Where(field => field.FieldId == id);
            return fields.FirstOrDefault();
        }

        public Field[] getByFarmId(Guid id)
        {
            var fields = _dbContext.Fields.Where(field => field.FarmId == id);
            return fields.ToArray<Field>();
        }

        public Field? getByUserAndFieldId(Guid userId, Guid fieldId)
        {
            var fields = _dbContext.Fields.Where(field => field.FieldId == fieldId).Where(field => field.UserId == userId);
            return fields.FirstOrDefault();
        }

        public Field? deleteByUserAndFieldId(Guid userId, Guid fieldId)
        {
            var fields = _dbContext.Fields.Where(field => field.FieldId == fieldId).Where(field => field.UserId == userId);
            Field? field = fields.FirstOrDefault();
            if (field == null)
                return null;
            _dbContext.Remove(field);
            _dbContext.SaveChanges();
            return field;
        }

        public Field? updateByUserAndFieldId(Guid userId, Guid fieldId, FieldUpdate fieldUpdate )
        {
            var fields = _dbContext.Fields.Where(field => field.FieldId == fieldId).Where(field => field.UserId == userId);
            Field? field = fields.FirstOrDefault();
            if (field == null)
                return null;
            field.Name = fieldUpdate.Name ?? field.Name;
            field.Altitude = fieldUpdate.Altitude ?? field.Altitude;
            field.Polygon = fieldUpdate.Polygon ?? field.Polygon ;
            _dbContext.SaveChanges();
            return field;
        }
    }
}