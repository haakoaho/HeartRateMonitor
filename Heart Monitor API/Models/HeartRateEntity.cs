using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heart_Monitor_API.NewFolder
{
    public class HeartRateEntity
    {
        public long RecordId { get; set; }
        public long UserId { get; set; }
        public float SystolicPressure { get; set; }
        public float ArterisPressure { get; set; }
        public DateTime CreatedAt { get; set; }   
    }
}
