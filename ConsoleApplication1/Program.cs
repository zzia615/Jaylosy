using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jaylosy.Kernel.Database;
using Jaylosy.Kernel.Attributes;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Test> testList = new List<Test>();
            for (int i = 0; i < 10 ; i++)
            {
                testList.Add(new Test
                {
                    name = "哈哈哈12443"
                });
            }
            OracleDbHelper db = new OracleDbHelper("defaultDb");
            db.BatchUpdate(testList, null);

        }
    }
    [Table(Name="test20181105",PrimaryKey="id")]
    class Test
    {

        public string name { get; set; }

        public int id { get; set; }
    }
}
