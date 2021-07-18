using System.Collections.Generic;

namespace ComputerCodeBlue.MyLaps.Example.Models
{
    public class MyLapsClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<MyLapsDriver> Results { get; set; }
        public int RunId { get; set; }
    }
}
