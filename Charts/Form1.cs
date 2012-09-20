using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MobilniPortalNovicLib.Models;

namespace Charts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MobilniPortalNovicLib.Models.MobilniPortalNovicContext12 context = new MobilniPortalNovicLib.Models.MobilniPortalNovicContext12();
            var counts1 = context.NewsFiles.GroupBy(x => x.Category.CategoryId).Select(x=> new { ID = x.Key, Count= x.Sum(p=>1)}).ToList();
            var parents = context.Categories.Where(x => x.ParentCategory == null).Select(x => new {ID = x.CategoryId, Name = x.Name}).ToList();
            var dict = new Dictionary<String, int>();
            foreach (var i in parents)
            {
                dict[i.Name] = context.NewsFiles.Where(x => x.CategoryId == i.ID || x.Category.ParentCategoryId == i.ID).Sum(x => 1);
            }


            var s = new Dictionary<String,int>();

            chart1.Series.Clear();
            foreach (var i in counts1.Where(x=>x.Count>50).OrderBy(x=>x.Count))
            {
                var cat = context.Categories.Find(i.ID);
                var name = cat.Name;
                if (cat.ParentCategory != null)
                {
                    name = cat.ParentCategory.Name + " -> " + name;
                }
                s[name] = i.Count;
                chart1.Series.Add(name);
                chart1.Series[name].Points.AddY(i.Count);
            }

             File.WriteAllLines(@"C:/stats.csv", s.Select(x => x.Key + ";" + x.Value + ";"));

            chart2.Series.Clear();
            foreach (var i in dict.OrderBy(x=>x.Value))
            {
                chart2.Series.Add(i.Key);
                chart2.Series[i.Key].Points.AddY(i.Value);
            }

            File.WriteAllLines(@"C:/rootCategorije.csv", dict.Select(x => x.Key + ";" + x.Value + ";"));


            var buckets = new List<int>();
            var k = 20;
            var size = counts1.Max(x=>x.Count);
            size = (size + (k - size % k))/k;

            for (int i = 0; i < k; i++)
            {
                buckets.Add(counts1.Where(x => x.Count < (i + 1) * size && x.Count > i * size).Sum(x => 1));
            }
            chart3.Series.Clear();
            int z = 0;
            foreach (var i in buckets)
            {
                var name = (z*size).ToString()+" - "+((z+1)*(size)).ToString();
                chart3.Series.Add(name);
                chart3.Series[name].Points.AddY(i);
                z++;
            }

            
        }


        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }
    }
}
