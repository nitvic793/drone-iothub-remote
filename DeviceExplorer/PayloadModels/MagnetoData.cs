using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceExplorer.PayloadModels
{
    public class MagnetoData
    {
        public double MX { get; set; }
        public double MY { get; set; }
        public double MZ { get; set; }
        public double Declination { get; set; }

    }
}
