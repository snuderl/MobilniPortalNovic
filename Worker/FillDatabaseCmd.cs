using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    List<int> offset = Console.ReadLine().Split(' ').ToList().Select(x=>Int32.Parse(x)).ToList();
                    List<int> defaults = new List<int> { 0, 60, 0, 0 };
                    //Adds default values to not entered values
                    offset.AddRange(defaults.Skip(offset.Count));
                    Random rnd = new Random();
                    var clicks = new FillDatabase(new MobilniPortalNovicContext12()).SimulateClicks(userId, category, count, () =>
                    {
                        int HourOffset = offset[0];
                        int randomMinutesOffset = offset[1];
                        int daysOffset = offset[2];
                        int randomDaysOffset = offset[3];
                        var time = DateTime.Now.AddHours(HourOffset).AddMinutes(rnd.Next(-randomMinutesOffset-1, randomMinutesOffset));
                        time = time.AddDays(daysOffset).AddDays(rnd.Next(-randomDaysOffset-1, randomDaysOffset));
                        return time;
                    }
                    );

                    Console.WriteLine("{0} clicks added.", clicks.Count());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Bad input");
            }
        }

        
    }
}
