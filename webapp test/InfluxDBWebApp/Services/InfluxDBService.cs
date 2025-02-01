using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client; // Ensure this namespace matches your library
using InfluxDB.Client.Api.Domain;

namespace InfluxDBWebApp.Services
{
    public class InfluxDBService
    {
        private readonly InfluxDBClient _client;

        public InfluxDBService(string url, string token)
        {
            _client = new InfluxDBClient(url, token);
        }
        
        public async Task<List<dynamic>> GetCpuUsageAsync()
        {
            var query = "from(bucket: \"Test\") |> range(start: -1h)";

            try
            {
                var queryApi = _client.GetQueryApi();
                var tables = await queryApi.QueryAsync(query, "Dev");

                var results = new List<dynamic>();

                foreach (var table in tables)
                {
                    foreach (var record in table.Records)
                    {
                        // Log all keys and values to understand the data structure
                        foreach (var key in record.Values.Keys)
                        {
                            Console.WriteLine($"Key: {key}, Value: {record.Values[key]}");
                        }

                        // Add data to the results only if the expected keys exist
                        if (record.Values.ContainsKey("host") && record.Values.ContainsKey("usage_percentage"))
                        {
                            results.Add(new
                            {
                                Time = record.GetTime()?.ToDateTimeUtc(),
                                Host = record.Values["host"],
                                UsagePercentage = record.Values["usage_percentage"]
                            });
                        }
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<dynamic>();
            }
        }
    }
}


// namespace Examples
// {
//     public class WriteQueryExample
//     {
//         static async Task Main(string[] args)
//         {
//             var hostUrl = "https://us-east-1-1.aws.cloud2.influxdata.com";
//             var authToken = "wLxNMU_7tmp82gRtqKRh8IUa5PbcYOF4PQtWioLy7wKMoBayQjzLxSEa3RqU-EeIG1wABKpJ2qkdhXtxhDFnWg=="; // Replace with your auth token
            
//             // Connect to InfluxDB
//             using var client = new InfluxDBClient(hostUrl, token: authToken);

//             const string database = "Test"; // Ensure this database exists

//             // Simulate CPU data
//             var random = new Random();
//             for (int i = 0; i < 10; i++) // Write 10 data points
//             {
//                 var cpuUsage = random.NextDouble() * 100; // Simulate CPU usage (0-100%)
//                 var coresActive = random.Next(1, 9);      // Simulate active cores (1-8)
//                 var temperature = random.Next(30, 80);   // Simulate temperature (30-80°C)

//                 var point = PointData.Measurement("cpu_usage")
//                     .SetTag("host", "server-1")
//                     .SetField("usage_percent", cpuUsage)
//                     .SetField("cores_active", coresActive)
//                     .SetField("temperature", temperature);

//                 await client.WritePointAsync(point: point, database: database);
//                 Console.WriteLine($"Written data: CPU Usage = {cpuUsage:F2}%, Cores = {coresActive}, Temp = {temperature}°C");

//                 Thread.Sleep(1000); // Separate points by 1 second
//             }

//             Console.WriteLine("Data population complete. Return to the InfluxDB UI to view.");

//             // Query data
//             const string sql = @"SELECT *
//             FROM 'cpu_usage'
//             WHERE time >= now() - interval '24 hours'
//             ORDER BY time ASC";
                    
//             Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", "Usage (%)", "Cores", "Temp (°C)", "Time");
//             await foreach (var row in client.Query(query: sql, database: database))
//             {
//                 Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", row[0], row[1], row[2], row[3]);
//             }
//         }
//     }
// }
