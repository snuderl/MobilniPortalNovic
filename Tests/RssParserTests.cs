using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using HtmlAgilityPack;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Worker.Parsers;

namespace Tests
{
    [TestClass]
    internal class RssParserTests
    {
        [DeploymentItem("NewsFile.xml")]
        public void GetBody()
        {
            GenericNewsParser g = new GenericNewsParser("article");
            var html = new HtmlDocument();
            html.LoadHtml(new StreamReader("NewsFile.xml").ReadToEnd());

            var content = g.GetBody(html);

            string expected = @"To je prvič pred letošnjimi volitvami, da je Mitt Romney ali kakšen drug republikanski kandidat, zbral več denarja kot Barack Obama.

Romney je maja zase in za tako imenovani Romneyjev sklad za zmago, ki zbira denar tako zanj kot za nacionalni odbor Republikanske stranke, zbral 76,8 milijona dolarjev. Obama je zase in za nacionalni odbor Demokratske stranke zbral 60 milijonov dolarjev. Romney in nacionalni odbor stranke sta imela tako maja v žepu 107 milijonov dolarjev, Obamova predvolilna kampanja pa skupnega zneska še ni objavila.
Premožni volivci v Romneyju vidijo zagotovilo za nizke davke

Romney je do sredine aprila zbiral denar le za svojo kampanjo brez sodelovanja z nacionalno stranko. Združitev operacij zbiranja denarja kandidatu omogoča, da lahko od posameznika dobi kar 75.000 dolarjev. Romneyjeva kampanja je od ljudi, ki so dali največ do 250 dolarjev, zbrala le 12 milijonov dolarjev. Kar pomeni, da večina denarja prihaja od premožnih volivcev, ki v Romneyju vidijo zagotovilo za nizke davke oziroma dobro naložbo za prihodnost.
Romney je samo v nekaj dnevih v Teksasu zbral 15 milijonov dolarjev

Tiskovni predstavnik Obamove predvolilne kampanje Ben LaBolt je dejal, da so Romneyjevo premoč v denarju pričakovali in mora služiti kot budnica demokratom, da razširijo svojo bazo donatorjev. Obama in Romney bosta zbrala veliko denarja tudi junija. Romney je samo v nekaj dnevih v Teksasu zbral 15 milijonov dolarjev, Obama pa je v New Yorku v ponedeljek in v Kaliforniji v sredo zbral skupno devet 9 milijonov dolarjev.

Vse tri velike države nimajo veliko besede pri izbiri strankarskih predsedniških kandidatov, ker so za volitve na vrsti ponavadi, ko je tekma že odločena. Vendar pa so pomembne za zbiranje denarja in za končno zmago, saj imajo največ delegatskih glasov. Teksas bo tudi letos volil za Romneyja, Kalifornija in New York pa za Obamo.Vse tri velike države nimajo veliko besede pri izbiri strankarskih predsedniških kandidatov, ker so za volitve na vrsti ponavadi, ko je tekma že odločena. Vendar pa so pomembne za zbiranje denarja in za končno zmago, saj imajo največ delegatskih glasov. Teksas bo tudi letos volil za Romneyja, Kalifornija in New York pa za Obamo.
";

            expected = Regex.Replace(expected, @"\s", "");
            content = Regex.Replace(content, @"\s", "");

            Assert.IsTrue(String.Equals(content, expected, StringComparison.OrdinalIgnoreCase));
        }

        [TestMethod, DeploymentItem("rss.xml")]
        public void RssParserTest()
        {
            var doc = XDocument.Parse(new StreamReader("rss.xml").ReadToEnd());
            RssFeedParser rss = new RssFeedParser();
            var list = rss.parseRssDocument(doc, 1);

            Assert.AreEqual(2, list.Count());
            Assert.AreEqual(list.Where(x => x.Title == "Romney maja prvič doslej premagal Obamo v zbiranju denarja za kampanjo").Count() > 0, true);
        }

        [TestMethod, DeploymentItem("TextToExtract.html")]
        public void ExtractTextTest()
        {
            var doc = new StreamReader("TextToExtract.html").ReadToEnd();
            var content = ParsingHelpers.ExtractText(doc);

            String expected = "Introduction " + " BoofCV " + "  is a new real time computer vision library written in Java. Written from scratch for ease of use and high performance, it provides a range of functionality from low level image processing, wavelet denoising, to higher level 3D geometric vision. Released under an Apache license for both academic and commercial use. BoofCV's speed has been demonstrated in a couple of comparative studies against other popular computer vision libraries ( " + " link )." +
                " To demonstrate BoofCV's API, an example of how to associate point features between two images is shown below. Image association is a vital component in creating image mosaics, image stabilization, visual odometry, 3D structure estimation, and many other applications. " +
  " BoofCV's website contains numerious examples and several tutorials. You can run a multitude of Java Applets in your webbrowser to see its features before installing. BoofCV also has a youtube channel explaining different concepts and examples. ";

            expected = Regex.Replace(expected, @"\s", "");
            content = Regex.Replace(content, @"\s", "");

            Assert.AreEqual(content, expected);
        }
    }
}