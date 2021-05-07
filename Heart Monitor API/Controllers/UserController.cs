using Heart_Monitor_API.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heart_Monitor_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public void Register(string username)
        {
            new HealthDB(Databases.ProductionDB).RegisterUser(username);
        }

        [HttpGet]
        public long Login(string username)
        {
            return new HealthDB(Databases.ProductionDB).GetUserId(username);
        }
    }
}
