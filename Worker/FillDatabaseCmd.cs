using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using MobilniPortalNovicLib.Models;

namespace Worker
{
    class FillDatabaseCmd
    {
        public static void SimulateClicks()
        {
            try
            {
                using (var context = new MobilniPortalNovicContext12())
                {
                    //UserId
                    Console.WriteLine("UserId:");
                    int userId;
                    var i1 = Console.ReadLine();
                    if (Int32.TryParse(i1, out userId) == false)
                    {
                        userId = context.Users.Where(x => x.Username == i1).First().UserId;
                    }


                    //Number clicks
                    Console.WriteLine("Number of clicks");
                    var count = Int32.Parse(Console.ReadLine());

                    //CategoryId
                    Console.WriteLine("Category number");
                    int category;
                    var i2 = Console.ReadLine();
                    if (Int32.TryParse(i2, out category) == false)
                    {
                        category = context.Categories.Where(x => x.Name == i2).First().CategoryId;
                    }

                    //Time function
                    Console.WriteLine("Hour offset from now: (+-random minutes) dayOffset (+-randomDays)");
                    List<int> offset = Console.ReadLine().Split(' ').ToList().Select(x => Int32.Parse(x)).ToList();
                    List<int> defaults = new List<int> { 0, 60, 0, 0 };
                    //Adds default values to not entered values
                    offset.AddRange(defaults.Skip(offset.Count));



                    Func<Coordinates> coordinatesRandom;
                    Console.WriteLine("Location query String");
                    var city = Console.ReadLine();
                    if (city.Length != 0)
                    {
                        Console.WriteLine("Location distance offset:");
                        var distanceOffset = Console.ReadLine();

                        Random rnd = new Random();
                        coordinatesRandom = CreateCoordinatesRandomFunc(city, Int32.Parse(distanceOffset));
                    }
                    else
                    {

                        coordinatesRandom = () => null;
                    }


                    Func<DateTime> dateTimeRandom = CreateDateTimeRandomFunc(offset[0], offset[1], offset[2], offset[3]);


                    var clicks = new FillDatabase(new MobilniPortalNovicContext12()).SimulateClicks(userId,
                        category,
                        count,
                        dateTimeRandom,
                        coordinatesRandom
                    );

                    Console.WriteLine("{0} clicks added.", clicks.Count());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bad input");
            }
        }

        public static Func<Coordinates> CreateCoordinatesRandomFunc(String query, int KilometerRandomOffset)
        {
            return () =>
            {
                Random rnd = new Random();
                var coordinates = GeoCode(query);
                var dy = rnd.Next(0, 100);
                var dx = 100 - dy;

                //111km == 1 deegre
                float km = ((float)KilometerRandomOffset) / 111f;

                coordinates.Latitude = coordinates.Latitude + (km * dy) / 100;
                coordinates.Longitude = coordinates.Longitude + (km * dx) / 100;
                return coordinates;
            };
        }

        public static Func<DateTime> CreateDateTimeRandomFunc(int HourOffset, int randomMinutesOffset, int daysOffset, int randomDaysOffset)
        {
            return () =>
            {
                Random rnd = new Random();
                var time = DateTime.Now.AddHours(HourOffset).AddMinutes(rnd.Next(-randomMinutesOffset - 1, randomMinutesOffset));
                time = time.AddDays(daysOffset).AddDays(rnd.Next(-randomDaysOffset - 1, randomDaysOffset));
                return time;
            };
        }

        public static Coordinates GeoCode(string location)
        {
            string url = "http://maps.google.com/maps/geo?output=xml&key=AIzaSyDcfCqe30hcD88r8dpRphaAc1GuWpsjjsA&q=" + System.Web.HttpUtility.UrlEncode(location);
            WebRequest req = HttpWebRequest.Create(url);
            WebResponse res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            try
            {
                Match coord = Regex.Match(sr.ReadToEnd(), "<coordinates>.*</coordinates>");
                if (!coord.Success) return null;
                var v = coord.Value.Substring(13, coord.Length - 27).Split(',');
                return new Coordinates { Latitude = float.Parse(v[0].Replace(".", ",")), Longitude = float.Parse(v[1].Replace('.', ',')) };
            }
            finally
            {
                sr.Close();
            }
        }


    }
}
