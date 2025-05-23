// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Runtime.CompilerServices;
using Dungeon;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.CallerInfo;
using Serilog.Enrichers.WithCaller;
using Serilog.Events;

Console.WriteLine("Hello, World!");

var assembly = Assembly.GetExecutingAssembly().FullName;

Log.Logger = new LoggerConfiguration()
    // .Enrich.WithCallerInfo(includeFileInfo: true, allowedAssemblies: [
    //     "Dungeon",
    //     "Dungeon.Game",
    //     "Dungeon.",
    // ])
    .Enrich.WithCaller()
    .WriteTo.Console()
    .WriteTo.Sink(new MySink())
    .CreateLogger();

// Log.Information("Hello world!");

var l = Log.ForContext<MySink>();
l.Information("Test");


Log.CloseAndFlush();

namespace Dungeon
{
    public class MySink : ILogEventSink
    {
        public void Emit(LogEvent logEvent)
        {
        
        }
    }

    public class CallerInfoEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var callerInfo = GetCallerInfo();
            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("CallerInfo", callerInfo));
        }
    
        private string GetCallerInfo([CallerMemberName] string memberName = "", [CallerFilePath] string filePath = "")
        {
            return $"{memberName} in {filePath}";
        }
    }
}