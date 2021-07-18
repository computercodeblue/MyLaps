using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerCodeBlue.MyLaps
{
    public class GroupWithRunsResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public int EventId { get; set; }
        public string Name { get; set; }
        public List<RunResult> Runs { get; set; }
        public EventWithRunsResult Event { get; set; }
    }
}
