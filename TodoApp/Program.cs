using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Text.Json;
//hi?
class TaskItem
{
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
}
class TaskManager
{
    const int maxToShow=6;
    private const string FilePath = "tasks.json";
    private List<TaskItem> tasks;

    public TaskManager()
    {
        tasks = LoadTasks();
        
    }

    private List<TaskItem> LoadTasks()
    {
        if (File.Exists(FilePath))
        {
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<TaskItem>>(json) ?? new List<TaskItem>();
        }
        return new List<TaskItem>();
    }

    private void SaveTasks()
    {
        string json = JsonSerializer.Serialize(tasks, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public void AddTask()
    {
        Console.Write("task name: ");
        string description = Console.ReadLine();
        tasks.Add(new TaskItem {  Description = description, IsCompleted = false });
        SaveTasks();
    }

    public void InteractiveViewTasks()
    {
        int selectedIndex = 0;
        bool finish = false;
        while (!finish)
        {
            Console.Clear();
            Console.WriteLine("TODO LIST (Use ↑/↓ to navigate)\n A add : C check/uncheck : D delete : Q exit");
            
               if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks available.");
               
            }
            // for (int i = 0; i < tasks.Count; i++)
            // {
            //     if (i == selectedIndex)
            //         Console.BackgroundColor = ConsoleColor.Gray;

            //     Console.ForegroundColor = tasks[i].IsCompleted ? ConsoleColor.Green : ConsoleColor.White;
            //     Console.WriteLine($"[{(tasks[i].IsCompleted ? "✔" : "✗")}] . {tasks[i].Description}");

            //     Console.ResetColor();
            // }
            for (int i =0;i<maxToShow ;i++)
            {
                if(i+(selectedIndex/maxToShow)*maxToShow  == tasks.Count) break;
                
                if (i == (selectedIndex%maxToShow))
                    Console.BackgroundColor = ConsoleColor.Gray;
                    
                Console.ForegroundColor = tasks[i+(selectedIndex/maxToShow)*maxToShow].IsCompleted ? ConsoleColor.Green : ConsoleColor.White;
                Console.WriteLine($"[{(tasks[i+(selectedIndex/maxToShow)*maxToShow].IsCompleted ? "✔" : "✗")}] . {tasks[i+(selectedIndex/maxToShow)*maxToShow].Description}");
                Console.ResetColor();
                
            }

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (selectedIndex > 0) selectedIndex--;
                    break;
                case ConsoleKey.DownArrow:
                    if (selectedIndex < tasks.Count - 1) selectedIndex++;
                    break;
                case ConsoleKey.A: // adding
                    AddTask();
                    break;
                case ConsoleKey.C: //completing task
                    CompleteTask(selectedIndex);
                    break;
                case ConsoleKey.D: //deleting
                    DeleteTask(selectedIndex);
                    if (selectedIndex > 0) selectedIndex--;
                    break;
                case ConsoleKey.U: //shifiting up
                    if (selectedIndex > 0)
                    {
                        var temp = tasks[selectedIndex];
                        tasks[selectedIndex] = tasks[selectedIndex-1];
                        tasks[selectedIndex-1]  =temp;
                        selectedIndex--;
                    }
                    break;
                case ConsoleKey.I: //shifiting down
                    if (selectedIndex < tasks.Count-1)
                    {
                        var temp = tasks[selectedIndex];
                        tasks[selectedIndex] = tasks[selectedIndex+1];
                        tasks[selectedIndex+1]  =temp;
                        selectedIndex++;
                    }
                    break;
                case ConsoleKey.Q:
                    finish = true;
                    return;
            }
        }
        SaveTasks();
    }

    public void CompleteTask(int id)
    {
        tasks[id].IsCompleted = !tasks[id].IsCompleted;
        
    }

    public void DeleteTask(int id)
    {
        if(tasks.Count==0) return;
        tasks.RemoveAt(id);
        SaveTasks();
    }
}
class Program
{
    static void Main()
    {
        TaskManager taskManager = new TaskManager();
        
            taskManager.InteractiveViewTasks();
        Console.Clear();
    }
}