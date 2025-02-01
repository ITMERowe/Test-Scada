using Microsoft.AspNetCore.Mvc.RazorPages;
using InfluxDBWebApp.Services; // Add this namespace
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using NodaTime; // Ensure this is included

namespace InfluxDBWebApp.Pages
{
    public class CpuUsage
    {
        public DateTime Time { get; set; }
        public string Host { get; set; }
        public double UsagePercent { get; set; }
        public double Temperature { get; set; }
        public double CoresActive { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<CpuUsage> CpuUsageData { get; set; } = new();

        private const string Token = "wLxNMU_7tmp82gRtqKRh8IUa5PbcYOF4PQtWioLy7wKMoBayQjzLxSEa3RqU-EeIG1wABKpJ2qkdhXtxhDFnWg==";
        private const string Org = "Dev";
        private const string Bucket = "Test";
        private const string InfluxUrl = "https://us-east-1-1.aws.cloud2.influxdata.com";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("Executing OnGetAsync...");
            
            var query = @"
                from(bucket: ""Test"")
                |> range(start: -1h)
                |> filter(fn: (r) => r._measurement == ""cpu_usage"")
                |> filter(fn: (r) => r._field == ""usage_percent"" or r._field == ""temperature"" or r._field == ""cores_active"")
            ";

            // var query = "from(bucket: \"Test\") |> range(start: -1h)";
            using var influxDBClient = new InfluxDBClient(InfluxUrl, Token);
            // CpuUsageData = await FetchDataFromInfluxDBAsync(influxDBClient, query);

            // using var influxDBClient = new InfluxDBClient(InfluxUrl, Token);
            CpuUsageData = await FetchDataFromInfluxDBAsync(influxDBClient, query);

            // Log the number of data points fetched
            _logger.LogInformation($"Fetched {CpuUsageData.Count} data points from InfluxDB.");
        }

        private async Task<List<CpuUsage>> FetchDataFromInfluxDBAsync(InfluxDBClient client, string query)
        {
            var tables = await client.GetQueryApi().QueryAsync(query, Org);
            var cpuData = new List<CpuUsage>();

            foreach (var table in tables)
            {
                foreach (var record in table.Records)
                {
                    // Extract time and field-specific values
                    var time = record.GetTime().HasValue ? record.GetTime().Value.ToDateTimeUtc() : DateTime.MinValue;
                    var field = record.GetValueByKey("_field")?.ToString();
                    var value = Convert.ToDouble(record.GetValueByKey("_value") ?? 0);

                    // Check if there's already an entry for the given time
                    var existing = cpuData.Find(x => x.Time == time);
                    if (existing == null)
                    {
                        existing = new CpuUsage { Time = time };
                        cpuData.Add(existing);
                    }

                    // Assign the value to the correct field
                    switch (field)
                    {
                        case "usage_percent":
                            existing.UsagePercent = value;
                            break;
                        case "temperature":
                            existing.Temperature = value;
                            break;
                        case "cores_active":
                            existing.CoresActive = value;
                            break;
                    }
                }
            }
            _logger.LogInformation($"Fetched {cpuData.Count} data points:");
            foreach (var item in cpuData)
            {
                _logger.LogInformation($"Time: {item.Time}, Host: {item.Host}, Usage: {item.UsagePercent}, Temp: {item.Temperature}, Cores: {item.CoresActive}");
            }
            return cpuData.OrderBy(x => x.Time).ToList(); // Ensure data is sorted by time
        }
    }
}
