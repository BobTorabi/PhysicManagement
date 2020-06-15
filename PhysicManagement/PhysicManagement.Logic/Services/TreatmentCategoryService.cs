using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Services
{
    public class TreatmentCategoryService
    {
        public static List<Model.KFactor> GetAllKFaktors() {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.KFactor.ToList();
            }
        }
        public static Model.KFactor GetKFaktorByYear(string year)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.KFactor.Where(x=>x.Year == year).FirstOrDefault();
            }
        }
        public static List<Model.TreatmentCategory> GetAllTreatmentCategory()
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return db.TreatmentCategory.ToList();
            }
        }
        public static List<Model.TreatmentCategoryService> 
            GetTreatmentCategoryServiceByTreatmentCategoryId(int TreatmentCategoryId)
        {
            using (var db = new Model.PhysicManagementEntities())
            {
                return 
                    db.TreatmentCategoryService
                    .Where(x=>x.TreatmentCategoryId == TreatmentCategoryId)
                    .ToList();
            }
        }
    }
}
