using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace ConsoleApp2
{
    public class Book
    {
        public int Num { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Book> books = new List<Book>()
            {
                new Book(){Name = "name1",Type = "type1",Num = 1},
                new Book(){Name = "name2",Type = "type2",Num = 2},
                new Book(){Name = "name3",Type = "type2",Num = 2},
                new Book(){Name = "name4",Type = "type1",Num = 1},
                new Book(){Name = "name5",Type = "type1",Num = 1}
            };

            Console.WriteLine(JsonConvert.SerializeObject(books));

            var group = books.GroupBy(b => new { b.Type, b.Num });

            Console.WriteLine(JsonConvert.SerializeObject(group));

            var group1 = group.Select(g => new
            {
                key = g.Key,
                value = g
            });

            Console.WriteLine(JsonConvert.SerializeObject(group1));

            Console.ReadLine();
        }
    }
}