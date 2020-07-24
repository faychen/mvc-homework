using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace Wikipedia
{
    class Program
    {
        static void Main(string[] args)
        {
            var oriTitle = "Taiwan";
           
            //links.Dump();
            /*
            level title
                level title next li

                level 0 taiwan
                level 1 -x1
                        -x2
                    level 2 -x1 -detail 1
            */

            //object1: title with links mapping
            //object2: level with title list

            
            //level 0
            var levelLinkList = new Dictionary<Int32, object>();
            levelLinkList.Add(0, oriTitle);

            var levelTitleList = new Dictionary<int, List<string>>();
            levelTitleList.Add(0, new List<string>() { oriTitle });

            //level 1
            var specificLinkList = new Dictionary<string, JArray>();
            var links = GetArticalLinks(oriTitle);
            specificLinkList.Add(oriTitle, links);
            
            levelLinkList.Add(1, specificLinkList);

            var linkList = links.Select(x => (x.First.Next).First.ToString()).ToList();
            levelTitleList.Add(1, linkList);


            //next level//old
            for (int i = 2; i <= RunLever(); i++)
            {
                var lastLevelLinkList = levelLinkList[i - 1];
                var savedLevelLinkList = new List<string>();

                foreach (var item in (Dictionary<string, JArray>)lastLevelLinkList)
                {
                    foreach (var v in item.Value)
                    {
                        specificLinkList = new Dictionary<string, JArray>();
                        var title = (((JObject)v).First.Next).First.ToString();

                        
                        links = GetArticalLinks(title);
                        specificLinkList.Add(title, links);
                        savedLevelLinkList.AddRange(links.Select(x => (x.First.Next).First.ToString()).ToList());

                        
                    }
                    levelLinkList.Add(i, savedLevelLinkList);

                    levelTitleList.Add(i, savedLevelLinkList);
                }

            }

            //for (int i = 0; i < levelTitleList.Count; i++)
            //{

            //    foreach (var item in levelTitleList[i])
            //    {
            //        Console.WriteLine(item);
            //        var linkList = specificLinkList[item];
            //        Console.WriteLine(((JArray)linkList).Select(x => x["title"]));
            //    }
            //}

            //for (int i = 0; i < levelLinkList.Count; i++)
            //{
            //    Console.WriteLine("level " + i.ToString());
            //    var lastLevelLinkList = levelLinkList[i];
            //    if (lastLevelLinkList is string)
            //    {
            //        Console.WriteLine(lastLevelLinkList);
            //        continue;
            //    }

            //    if (lastLevelLinkList is Dictionary<string, JArray>)
            //    {
            //        foreach (var item in (Dictionary<string, JArray>)lastLevelLinkList)
            //        {
            //            Console.WriteLine(item.Value.First.First.Next.First.ToString());
            //        }
            //        continue;
            //    }
            //    if (lastLevelLinkList is List<object>)
            //    {
            //        var o = ((List<object>)lastLevelLinkList).ToArray();
            //        foreach (var item in ((List<object>)lastLevelLinkList))
            //        {
            //            foreach (var nextLevel in (Dictionary<string, JArray>)item)
            //            {
            //                foreach (var next in nextLevel.Value)
            //                {
            //                    Console.WriteLine((((JObject)next).First.Next).First.ToString());
            //                }
                            
            //            }
            //        }
            //    }

            //    Console.WriteLine(lastLevelLinkList);
            //}

            Console.ReadKey();
        }

        public static JArray GetArticalLinks(string title)
        {
            WebClient client = new WebClient();
            var url = $"https://en.wikipedia.org/w/api.php?action=query&titles={title}&prop=extracts|links&format=json";
            dynamic result = JObject.Parse(client.DownloadString(url));
            dynamic articalObj = ((JObject)result.query.pages).First.First;
            return articalObj["links"];

        }

        static int RunLever()
        {
            return 2;
        }
    }
}
