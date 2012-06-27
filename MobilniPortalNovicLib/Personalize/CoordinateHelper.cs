using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    static class CoordinateHelper
    {
        static double _eQuatorialEarthRadius = 6378.1370D;
        static double _d2r = (Math.PI / 180D);

        static private int HaversineInM(double lat1, double long1, double lat2, double long2)
        {
            return (int)(1000D * HaversineInKM(lat1, long1, lat2, long2));
        }

        static private double HaversineInKM(double lat1, double long1, double lat2, double long2)
        {
            double dlong = (long2 - long1) * _d2r;
            double dlat = (lat2 - lat1) * _d2r;
            double a = Math.Pow(Math.Sin(dlat / 2D), 2D) + Math.Cos(lat1 * _d2r) * Math.Cos(lat2 * _d2r) * Math.Pow(Math.Sin(dlong / 2D), 2D);
            double c = 2D * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1D - a));
            double d = _eQuatorialEarthRadius * c;

            return d;
        }

        public static double DistanceInM(Coordinates c1, Coordinates c2)
        {
            return HaversineInM(c1.Latitude, c1.Longitude, c2.Latitude, c2.Longitude);
        }
    }
}
