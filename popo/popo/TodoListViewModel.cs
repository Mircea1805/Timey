using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Data.SqlTypes;
using System.Linq;

namespace Timey
{

    public class TodoListViewModel
    {
        public ObservableCollection<TodoItem> TodoItems { get; set; }

        public TodoListViewModel ()
        {
            TodoItems = new ObservableCollection<TodoItem>();
        }
        
        public string NewTodoInputValue { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public ICommand AddTodoCommand => new Command(AddTodoItem);

        void AddTodoItem()
        {
            TodoItem newtodo;
            newtodo = new TodoItem
            {
                TodoText = NewTodoInputValue,
                Complete = false,
                Date = Date,
                Time = Time,
            };
            App.database.SaveItemAsync(newtodo);
            TodoItems.Add(newtodo);
        }
        
        public ICommand RemoveTodoCommand => new Command(RemoveTodoItem);
        
        void RemoveTodoItem(object o)
        {
            TodoItem todoItemBeingRemoved = o as TodoItem;
            App.database.DeleteItemAsync(todoItemBeingRemoved);
            TodoItems.Remove(todoItemBeingRemoved);
        }
    }
}
