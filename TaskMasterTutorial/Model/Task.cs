using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMasterTutorial.Model
{
    internal class Task
    {
        public int Id { get; set; }

        public DateTime? DueDate { get; set; }

        public string Name { get; set; }

        //the foregin key to the Status table
        public int StatusId { get; set; }

        //the navigation property to the Status table
        public Status Status { get; set; }
    }
}
