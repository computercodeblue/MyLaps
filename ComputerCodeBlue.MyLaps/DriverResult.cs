using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComputerCodeBlue.MyLaps
{
    public class DriverResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public string Position { get; set; }
        public string RaceNumber { get; set; }
        public string Competitor { get; set; }
        public string TotalTime { get; set; }
        public string Diff { get; set; }
        public string Laps { get; set; }
        public string BestTime { get; set; }
        public string BestLap { get; set; }
        public string TopSpeed { get; set; }
        public int ClassId { get; set; }
        public ClassResult Class { get; set; }
    }
}
