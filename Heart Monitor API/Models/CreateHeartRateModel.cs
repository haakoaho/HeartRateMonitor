using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heart_Monitor_API.Models
{
    public class CreateHeartRateModel
    {
        public long UserId { get; set; }
        public float SystolicPressure { get; set; }
        public float ArterisPressure { get; set; }
    }
}
