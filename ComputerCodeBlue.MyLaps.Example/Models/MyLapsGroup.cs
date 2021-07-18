using System.Collections.Generic;

namespace ComputerCodeBlue.MyLaps.Example.Models
{
    public class MyLapsGroup
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public List<MyLapsRun> Runs { get; set; }
    }
}
