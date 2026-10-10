# Task Tracker CLI

A simple CLI application to track tasks, built with C# .NET. This is my solution to the https://roadmap.sh/projects/task-tracker

## Features
- Data is saved in a local `tasks.json` file.
- Add, update, and delete tasks.
- Mark tasks as `in-progress` or `done`.
- List all tasks or filter them by their status.

## Usage

You can run the application using the `.NET CLI`. Here are the available commands:

**Add a new task:**
```
dotnet run add "Buy groceries"
```
**Update an existing task:**
```
dotnet run update 1 "Cook dinner"
```
**Delete a task:**
```
dotnet run delete 1
```
