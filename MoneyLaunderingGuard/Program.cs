// See https://aka.ms/new-console-template for more information
using MoneyLaunderingGuard;
using ServiceLibrary.Services;

Console.WriteLine("Hello, World!");

var moneyLaunderingService = new MoneyLaunderingService();

var app = new App(moneyLaunderingService);

app.Run();