using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingModule.PayloadModels
{
    public class GPSData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Elevation { get; set; }

        public static double ToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }

        public static double ToAngle(double radian)
        {
            return radian * 180 / Math.PI;
        }
    }
}
