using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Services
{
    public class AlarmConfigService
    {
        public List<Model.AlarmConfig> GetAlarmConfigList()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.AlarmConfig.OrderBy(x => x.Id).ToList();
            }
        }
        public Model.AlarmConfig GetAlarmConfigById(int entityId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                var Entity = db.AlarmConfig.Find(entityId);
                return Entity;
            }
        }
    }
}
