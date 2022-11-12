using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace Timey
{
    public class TodoItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string TodoText { get; set; }
        public bool Complete { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }
}
