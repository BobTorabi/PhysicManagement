using PhysicManagement.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace PhysicManagement.Controllers
{
    public class AlarmConfigController : BaseController
    {
        readonly AlarmConfigService Service;
        public AlarmConfigController()
        {
            Service = new AlarmConfigService();
        }
        // GET: AlarmConfig
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {

            List<Model.AlarmConfig> alarmConfig = Service.GetAlarmConfigList();
            return View(alarmConfig);
        }

    }
}