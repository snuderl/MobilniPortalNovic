using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MobilniPortalNovicLib.Models;

namespace MobilniPortalNovicLib.Personalize
{
    class RadiusFilter : Filter
    {
        public int RadiusInKm { get; set; }
        public Coordinates GivenPosition { get; set; }

        public RadiusFilter(int RadiusInKm, Coordinates GivenPosition)
        {
            this.RadiusInKm = RadiusInKm;
            this.GivenPosition = GivenPosition;
        }

        public IQueryable<Models.ClickCounter> Filter(IQueryable<Models.ClickCounter> clicks)
        {
            return FilterByClicksInGivenRadius(clicks, RadiusInKm, GivenPosition);
        }

        public string GetMessage()
        {
            return "Radius filter applied " + RadiusInKm + "km.";
        }

        public IQueryable<ClickCounter> FilterByClicksInGivenRadius(IQueryable<ClickCounter> clicks, double radiusInKm, Coordinates GivenPosition)
        {
            var l = clicks.Where(x => x.Latitude != null && x.Longitude != null).ToList();
            var closest = l.Where(x => CoordinateHelper.DistanceInM(x.Coordinates, GivenPosition) < radiusInKm / 1000);
            return closest.AsQueryable();
        }
    }
}
