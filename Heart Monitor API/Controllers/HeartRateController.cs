using Heart_Monitor_API.Constants;
using Heart_Monitor_API.Models;
using Heart_Monitor_API.NewFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heart_Monitor_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeartRateController : ControllerBase
    {

        private readonly ILogger<HeartRateController> _logger;

        public HeartRateController(ILogger<HeartRateController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public void CreateHeartRateRecord(CreateHeartRateModel heartRateModel) 
        {
            new HealthDB(Databases.ProductionDB).InsertHeartRateRecord(heartRateModel);
           
        }
        [HttpGet]
        public List<HeartRateEntity> GetAllHeartRatesForUser(long userId)
        {
            return  new HealthDB(Databases.ProductionDB).FindHeartRateByName(userId);
        }
        [HttpDelete]
        public void DeleteRecord(long recordId)
        {
            new HealthDB(Databases.ProductionDB).DeleteHeartRate(recordId);
        }
    }
}
