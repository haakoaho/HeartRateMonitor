using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace HeartRateApp.Models
{
    public class HeartRateModel
    {
        public long RecordId { get;  set; }
        public long UserId { get; set; }
        public float SystolicPressure { get; set; }
        public float ArterisPressure { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
