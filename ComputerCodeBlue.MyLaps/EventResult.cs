using System;
using System.Collections.Generic;

namespace ComputerCodeBlue.MyLaps
{
    public class EventResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<GroupResult> Groups { get; set; }
    }
}
