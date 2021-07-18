using System.Collections.Generic;

namespace ComputerCodeBlue.MyLaps.Example.Models
{
    public class MyLapsRun
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<MyLapsClass> Classes { get; set; }
        public int GroupId { get; set; }
    }
}
