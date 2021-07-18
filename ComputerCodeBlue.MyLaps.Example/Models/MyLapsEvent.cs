using System;
using System.Collections.Generic;

namespace ComputerCodeBlue.MyLaps.Example.Models
{
    public class MyLapsEvent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<MyLapsGroup> Groups { get; set; }
    }
}
