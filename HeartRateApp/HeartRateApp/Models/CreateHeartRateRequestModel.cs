using System;
using System.Collections.Generic;
using System.Text;

namespace HeartRateApp.Models
{
    class CreateHeartRateRequestModel
    {
        public long UserId { get; set; }
        public float SystolicPressure { get; set; }
        public float ArterisPressure { get; set; }
    }
}
