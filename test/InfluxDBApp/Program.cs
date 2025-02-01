using System;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB3.Client;
using InfluxDB3.Client.Write;

namespace Examples
{
    public class WriteQueryExample
    {
        static async Task Main(string[] args)
        {
            var hostUrl = "https://us-east-1-1.aws.cloud2.influxdata.com";
            var authToken = "wLxNMU_7tmp82gRtqKRh8IUa5PbcYOF4PQtWioLy7wKMoBayQjzLxSEa3RqU-EeIG1wABKpJ2qkdhXtxhDFnWg=="; // Replace with your auth token
            
            // Connect to InfluxDB
            using var client = new InfluxDBClient(hostUrl, token: authToken);

            const string database = "Test"; // Ensure this database exists

            // Simulate CPU data
            var random = new Random();
            for (int i = 0; i < 24; i++) // Write 24 data points for each hour of the day
            {
                // Simulate trends based on the time of day (i.e., i)
                double cpuUsage;
                int coresActive;
                int temperature;

                if (i >= 9 && i <= 17) // Peak hours (9 AM to 5 PM)
                {
                    cpuUsage = random.NextDouble() * 50 + 50; // CPU usage is higher during peak hours (50-100%)
                    coresActive = random.Next(6, 13);         // More cores active during peak hours (6-12)
                    temperature = random.Next(50, 85);        // Higher temperatures during peak (50-85°C)
                }
                else // Off-peak hours
                {
                    cpuUsage = random.NextDouble() * 50; // CPU usage is lower during off-peak hours (0-50%)
                    coresActive = random.Next(1, 6);     // Fewer cores active during off-peak hours (1-5)
                    temperature = random.Next(30, 55);    // Lower temperatures during off-peak (30-55°C)
                }

                var point = PointData.Measurement("cpu_usage")
                    .SetTag("host", "server-1")
                    .SetField("usage_percent", cpuUsage)
                    .SetField("cores_active", coresActive)
                    .SetField("temperature", temperature);

                await client.WritePointAsync(point: point, database: database);
                Console.WriteLine($"Written data: CPU Usage = {cpuUsage:F2}%, Cores = {coresActive}, Temp = {temperature}°C");

                Thread.Sleep(10); // Separate points by 1 second
            }

            Console.WriteLine("Data population complete. Return to the InfluxDB UI to view.");

            // Query data
            const string sql = @"SELECT *
            FROM 'cpu_usage'
            WHERE time >= now() - interval '1 hours'
            ORDER BY time ASC";
                            
            Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-20}", "Usage (%)", "Cores", "Temp (°C)", "Time");

            await foreach (var row in client.Query(query: sql, database: database))
            {
                Console.WriteLine("{0,-15}{1,-15}{2,-15}{3,-15}", row[0], row[1], row[2], row[3]);
            }
        }
    }
}
