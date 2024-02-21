// See https://aka.ms/new-console-template for more information
using practicode_2;
using System.Text.RegularExpressions;
Serializer url = new Serializer();
var result = await url.LoadAndParse("https://learn.malkabruk.co.il/practicode/projects/pract-2/");
var dom = url.BuildHtmlTree(result);
var r = dom.FindElements(Selector.FromQueryString("div div div div"));
r.ToList().ForEach(e => Console.WriteLine(e.ToString()));
r.ToList().ForEach(e => Console.WriteLine(e.ToString2()));

Console.ReadLine();
