using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComputerCodeBlue.MyLaps
{
    public class RunResult
    {
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ClassResult> Classes { get; set; }
        public int GroupId { get; set; }
        public GroupWithRunsResult Group { get; set; }
    }

}
