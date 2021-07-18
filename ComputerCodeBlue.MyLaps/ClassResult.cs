using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerCodeBlue.MyLaps
{
    public class ClassResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Name { get; set; }
        public List<DriverResult> Results { get; set; }
        public int RunId { get; set; }
        public RunResult Run { get; set; }
    }
}
