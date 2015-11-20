using DataProcessingModule.PayloadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessingModule
{
    public class GPSDataProcessing
    {

        private const int R = 6371000;
        private double haversineDistance;

        public double HaversineDistance
        {
            get { return haversineDistance; }
            set { haversineDistance = value; }
        }

        private double initBearing;

        public double InitBearing
        {
            get { return initBearing; }
            set { initBearing = value; }
        }

        private double finalBearing;

        public double FinalBearing
        {
            get { return finalBearing; }
            set { finalBearing = value; }
        }

        private double angle;           

        public double Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        
        public double GetHaversineDistance(GPSData source, GPSData destination)
        {
            var phi1 = GPSData.ToRadians(source.Latitude);
            var phi2 = GPSData.ToRadians(destination.Latitude);
            var delPhi = GPSData.ToRadians(destination.Latitude - source.Latitude);
            var delLambda = GPSData.ToRadians(destination.Longitude - source.Longitude);

            var a = Math.Sin(delPhi / 2) * Math.Sin(delPhi / 2) + Math.Cos(phi1) * Math.Cos(phi2) * Math.Sin(delLambda / 2) * Math.Sin(delLambda / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            HaversineDistance = R * c;
            return HaversineDistance;
        }

        public double GetInitBearing(GPSData source, GPSData destination)
        {
            GPSData sourceInRadians = new GPSData() { Latitude = GPSData.ToRadians(source.Latitude), Longitude = GPSData.ToRadians(source.Longitude), Elevation = source.Elevation };
            GPSData destinationInRadians = new GPSData() { Latitude = GPSData.ToRadians(destination.Latitude), Longitude = GPSData.ToRadians(destination.Longitude), Elevation = source.Elevation };

            var y = Math.Sin(destinationInRadians.Longitude - sourceInRadians.Longitude) * Math.Cos(destinationInRadians.Latitude);
            var x = Math.Cos((sourceInRadians.Latitude)) * Math.Sin((destinationInRadians.Latitude)) - Math.Sin((sourceInRadians.Latitude)) * Math.Cos((destinationInRadians.Latitude)) * Math.Cos(destinationInRadians.Longitude - sourceInRadians.Longitude); 
            InitBearing = GPSData.ToAngle(Math.Atan2(y, x));
            return InitBearing;
        }

        public double GetFinalBearing(GPSData source, GPSData destination)
        {
            InitBearing = GetInitBearing(source, destination);
            FinalBearing = (InitBearing + 180) % 360;
            return FinalBearing;
        }

        public GPSData GetIntermediatePoint(GPSData source, GPSData destination, double fraction)
        {
            GPSData sourceInRadians = new GPSData() { Latitude = GPSData.ToRadians(source.Latitude), Longitude = GPSData.ToRadians(source.Longitude), Elevation = source.Elevation };
            GPSData destinationInRadians = new GPSData() { Latitude = GPSData.ToRadians(destination.Latitude), Longitude = GPSData.ToRadians(destination.Longitude), Elevation = source.Elevation };

            var distance = GetHaversineDistance(source, destination);
            var a = Math.Sin((1 - fraction) * distance / R) / Math.Sin(distance / R);
            var b = Math.Sin(fraction - (distance / R)) / Math.Sin(distance / R);
            var x = (a * Math.Cos(sourceInRadians.Latitude) * Math.Cos(sourceInRadians.Longitude)) + (b * Math.Cos(destinationInRadians.Latitude) * Math.Cos(destinationInRadians.Longitude));
            var y = (a * Math.Cos(sourceInRadians.Latitude) * Math.Sin(sourceInRadians.Longitude)) + (b * Math.Cos(destinationInRadians.Latitude) * Math.Sin(destinationInRadians.Longitude));
            var z = (a * Math.Sin(sourceInRadians.Latitude)) + (b * Math.Sin(destinationInRadians.Latitude));

            var lat = Math.Atan2(z, Math.Sqrt((x * x) + (y * y)));
            var longit = Math.Atan2(y, x);
            GPSData interPoint = new GPSData() { Latitude = GPSData.ToAngle(lat), Longitude = GPSData.ToAngle(longit)};

            return interPoint;
        }

        public double GetCurrentDeviationFromNorth(MagnetoData current)
        {
            if(current.MY > 0)
            {
                return ((90 - GPSData.ToAngle(Math.Atan(current.MX / current.MY))) + current.Declination);

            }
            else if(current.MY > 0)
            {
                return ((270 - GPSData.ToAngle(Math.Atan(current.MX / current.MY))) + current.Declination);

            }
            else
            {
                if(current.MX < 0)
                {
                    return 180 + current.Declination;
                }
                else
                {
                    return 0 + current.Declination;
                }
            }            
        }

    }
}
