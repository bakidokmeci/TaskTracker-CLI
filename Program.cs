using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace TaskTracker
{
    class Program
    {
        static readonly string filePath = "tasks.json";
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("Enter a command");
                return;
            }

            string command = args[0].ToLower();
            switch(command)
            {
                case "add":
                    if(args.Length < 2)
                    {
                        Console.WriteLine("You must enter a description.");
                        break;
                    }
                    AddTask(args[1]);
                    break;
                
                case "list":
                    string statusFilter = null;
                    if(args.Length > 1)
                    {
                        statusFilter = args[1].ToLower();
                    }
                    ListTasks(statusFilter);
                    break;
                
                case "update":
                    if(args.Length < 3)
                    {
                        Console.WriteLine("No Id and Description program exits...");
                        break;
                    }

                    if(!int.TryParse(args[1], out int updateId))
                    {
                        Console.WriteLine("You must give Id as int for second argument, program exits");
                        break;
                    }

                    UpdateTask(updateId, args[2]);
                    break;
                
                case "delete":
                    if(args.Length < 2)
                    {
                        Console.WriteLine("Enter a Id for delete operation.");
                        break;
                    }

                    if(!int.TryParse(args[1], out int deleteId))
                    {
                        Console.WriteLine("You must give Id as int for second argument, program exits.");
                        break;
                    }

                    DeleteTask(deleteId);
                    break;
                
                case "mark-in-progress":
                case "mark-done":
                    if(args.Length < 2)
                    {
                        Console.WriteLine("You must enter an id.");
                        break;
                    }

                    if(!int.TryParse(args[1], out int markId))
                    {
                        Console.WriteLine("You must give id as an int value.");
                        break;
                    }

                    string newStatus = command == "mark-in-progress" ? "in-progress" : "done";
                    MarkTask(markId, newStatus);
                    break;
                default:
                    break;
            }
        }
        static void AddTask(string description)
        {
            List<TaskItem> tasks = new List<TaskItem>();

            if(File.Exists(filePath))
            {
                string existingJson = File.ReadAllText(filePath);
                var parsedList = JsonSerializer.Deserialize<List<TaskItem>>(existingJson);
                if(parsedList != null)
                {
                    tasks = parsedList;
                }
            }

            int newId = 1;
            if(tasks.Count > 0)
            {
                newId = tasks[tasks.Count - 1].Id + 1;
            }

            TaskItem newTask = new TaskItem
            {
                Id = newId,
                Description = description,
                Status = "todo",
                Created = DateTime.Now,
                LastUpdated = DateTime.Now
            };

            tasks.Add(newTask);
            string jsonOutput = JsonSerializer.Serialize(tasks, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText(filePath, jsonOutput);
            Console.WriteLine($"Task added Successfully, ID {newId}");
        }

        static void ListTasks(string statusFilter)
        {
            List<TaskItem> tasks = new List<TaskItem>();
            if(!File.Exists(filePath))
            {
                Console.WriteLine("No file in the system.");
                return;
            }

            string existingJson = File.ReadAllText(filePath);
            tasks = JsonSerializer.Deserialize<List<TaskItem>>(existingJson);
            if(tasks == null || tasks.Count == 0)
            {
                Console.WriteLine("No task in the File, program exits...");
                return;
            }

            bool foundAny = false;
            foreach(var task in tasks)
            {
                if(statusFilter == null || task.Status == statusFilter)
                {
                    Console.WriteLine(task.ToString());
                    foundAny = true;
                }
            }
            if(!foundAny)
            {
                Console.WriteLine("No task found for this type.");
            }
        }

        static void UpdateTask(int id, string newDescription)
        {
            if(!File.Exists(filePath))
            {
                Console.WriteLine("No such file in the system, program exits.");
                return;
            }

            string existingJson = File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(existingJson) ?? new List<TaskItem>();
            var taskToUpdate = tasks.FirstOrDefault(t => t.Id == id);

            if(taskToUpdate == null)
            {
                Console.WriteLine($"No record of any task with Id: {id}");
                return;
            }

            taskToUpdate.Description = newDescription;
            taskToUpdate.LastUpdated = DateTime.Now;

            string jsonOutput = JsonSerializer.Serialize(tasks, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(filePath, jsonOutput);
            Console.WriteLine("Operation was Successfull.");
        }
    
        static void DeleteTask(int id)
        {
            if(!File.Exists(filePath))
            {
                Console.WriteLine("No such file in the system.");
                return;
            }

            string existingJson = File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(existingJson) ?? new List<TaskItem>();

            var taskToDelete = tasks.FirstOrDefault(t => t.Id == id);
            if(taskToDelete == null)
            {
                Console.WriteLine($"Cannot find the task with id {id}.");
                return;
            }

            tasks.Remove(taskToDelete);

            string jsonOutput = JsonSerializer.Serialize(tasks, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(filePath, jsonOutput);
            Console.WriteLine("Deleting operation was Successfull.");
        }
    
        static void MarkTask(int id, string newStatus)
        {
            if(!File.Exists(filePath))
            {
                Console.WriteLine("No such file gg :(");
                return;
            }

            string existingJson = File.ReadAllText(filePath);
            var tasks = JsonSerializer.Deserialize<List<TaskItem>>(existingJson) ?? new List<TaskItem>();

            var taskToMark = tasks.FirstOrDefault(t => t.Id == id);
            if(taskToMark == null)
            {
                Console.WriteLine("There is not any task with id you're given.");
                return;
            }

            taskToMark.Status = newStatus;
            taskToMark.LastUpdated = DateTime.Now;

            string jsonOutput = JsonSerializer.Serialize(tasks, new JsonSerializerOptions {WriteIndented = true});
            File.WriteAllText(filePath, jsonOutput);

            Console.WriteLine("Operation was succesfull.");
        }
    }
}