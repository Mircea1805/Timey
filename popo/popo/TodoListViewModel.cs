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
            TodoItems.DefaultIfEmpty(new TodoItem
            {
                TodoText = string.Empty,
                Complete = false,
                Date = DateTime.Now,
                Time = TimeSpan.Zero,
                ID = 0
            });
        }
        public ICommand AddTodoCommand => new Command(AddTodoItem);
        public string NewTodoInputValue { get; set; }

        
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    void AddTodoItem()
        {
            TodoItem newtodo = new TodoItem
            {
                TodoText = NewTodoInputValue,
                Complete = false,
                Date = Date,
                Time = Time,
                ID = TodoItems.LastOrDefault().ID + 1
            };
            TodoItems.Add(newtodo);
            TodoItemDatabase database = new TodoItemDatabase();
            database.SaveItemAsync(newtodo);
        }
        
        public ICommand RemoveTodoCommand => new Command(RemoveTodoItem);
        
        void RemoveTodoItem(object o)
        {
            TodoItem todoItemBeingRemoved = o as TodoItem;
            TodoItems.Remove(todoItemBeingRemoved);
        }

    }
}
