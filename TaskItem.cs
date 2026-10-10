using System;

namespace TaskTracker
{
    public class TaskItem
    {
        public int Id {get; set;}
        public string Description {get; set;} = "";
        public string Status {get; set;} = "";
        public DateTime Created {get; set;}
        public DateTime LastUpdated {get; set;}

        public override string ToString()
        {
            return $"ID: {this.Id} Desc: {this.Description} Status: {this.Status} is Created: {this.Created}";
        }
    }
}