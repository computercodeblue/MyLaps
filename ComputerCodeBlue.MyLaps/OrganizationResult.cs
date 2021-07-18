using System.Collections.Generic;

namespace ComputerCodeBlue.MyLaps
{
    public class OrganizationResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<EventListItem> Events { get; set; }
    }
}
