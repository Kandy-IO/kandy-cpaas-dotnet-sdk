using dotenv.net;
using Nancy.Hosting.Self;
using System;
using System.IO;

namespace fa {
  class Program {
    static void Main(string[] args) {
      var PORT = 8000;
      var uri = $"http://localhost:{PORT}";

      using (var host = new NancyHost(new Uri(uri))){
        host.Start();
        string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"../../"));
        DotEnv.Config(true, $"{path}/.env");
        Console.WriteLine("NancyFX Stand alone test application.");
        Console.WriteLine($"Listening on {uri}");
        Console.WriteLine("Press enter to exit the application");
        Console.ReadLine();
      }
    }
  }
}