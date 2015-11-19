using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceExplorer.PayloadModels
{
    class DroneDataMessage
    {
        public string Type { get; set; }
        public GPSData Gps { get; set; }
        public MagnetoData Magneto { get; set; }
    }
}
