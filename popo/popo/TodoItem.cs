using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Timey
{
    public class TodoItem
    {
        public string TodoText { get; set; }
        public bool Complete { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public int ID;
        /*public TodoItem(string TodoText, bool Complete, DateTime Date, TimeSpan Time)

        {
            this.TodoText = TodoText;
            this.Complete = Complete;
            this.Date = Date;
            this.Time = Time;
        }*/

    }
}
