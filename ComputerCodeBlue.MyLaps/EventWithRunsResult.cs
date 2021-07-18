using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComputerCodeBlue.MyLaps
{
    public class EventWithRunsResult
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public List<GroupWithRunsResult> Groups { get; set; }
    }
}
