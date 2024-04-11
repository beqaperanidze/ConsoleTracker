using ConsoleTracker.dir;
using Spectre.Console;


const string connectionString = "Data Source=Sessions.sqlite;Version=3;";
DatabaseManager.RunCodingSessions(connectionString);