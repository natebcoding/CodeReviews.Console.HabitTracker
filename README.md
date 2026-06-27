# Habit Tracker
This is a Habit Tracker application created by me for the C# Academy .NET Roadmap. 

## Usage
Users will be able to track certain habits such as exercising, drink water, and any other routines. 

## CRUD 
This project has basic CRUD functionality. User data is stored in a SQLite Database, where data can be created, read, update, or deleted

## Project Requirements
* Console Application for logging habits
* Habits are tracked by quantity
* Full CRUD functionality
* Data should persist to SQLite database using ADO.NET
* App should not crash if user enters invalid input
* Should use parameterized queries to prevent SQL injection
* Custom Habit units (glasses of water, reps, miles, km, etc)
* Data should be seeded on first run

## How To Run
* Must have .NET 10 SDK Installed
* Clone the repo
* Navigate to the project folder - cd HabitTracker/HabitTracker
* dotnet run
* Once ran, the database will be created

## My Approach
* Coming from Java, I found this project very interesting in learning how CRUD could be approached using C#
* I came in with the goal of really practicing the DRY approach and making sure logic was consistent throughout the codebase (validation, seperation of concerns)
* One thing I'd change is merging some of the helpers to have shared logic in one method instead of multiple
