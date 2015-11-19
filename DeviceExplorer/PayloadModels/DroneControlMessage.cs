using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceExplorer.PayloadModels
{
    public class DroneControlMessage
    {
        public string Type { get; set; }

        public string Direction { get; set; }

        public double Thrust { get; set; }

        public int TimeInMilliSecond { get; set; }

    }
}
