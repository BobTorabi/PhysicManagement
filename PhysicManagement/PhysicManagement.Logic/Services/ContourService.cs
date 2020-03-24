using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicManagement.Logic.Services
{
   public class ContourService
   {
        #region Contour section

        public List<Model.Contour> GetContourList()
        { }
        public Model.Contour GetContourById(int entityId)
        { }
        public bool AddContour(Model.Contour entity)
        { }
        public bool UpdateContour(Model.Contour entity)
        { }
        public bool DeleteContour(Model.Contour entityId)
        { }
        #endregion

        #region ContourDetail section

        public List<Model.ContourDetails> GetContourDetailsList()
        { }
        public Model.ContourDetails GetContourDetailsById(int entityId)
        { }
        public bool AddContourDetails(Model.ContourDetails entity)
        { }
        public bool UpdateContourDetails(Model.ContourDetails entity)
        { }
        public bool DeleteContourDetails(Model.ContourDetails entityId)
        { }
        #endregion


    }
}
