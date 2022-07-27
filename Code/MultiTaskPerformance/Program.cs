using static System.Console;
using DacaNetDev.MonitoringApp;
using System.Net;
using System.Xml.Linq;


var doc = XDocument.Load("NET.opml");

if (doc == null) return;

var urls = from outline in doc?.Element("opml")?
		   .Element("body")?
		   .Element("outline")?.Elements()
		   select outline?.Attribute("htmlUrl")?.Value;



