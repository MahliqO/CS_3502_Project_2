using System;
using System.Collections.Generic;
using System.Linq;

namespace CPUSchedulingSimulator
{
    // Class to hold simulation results
    public class SimulationResult
    {
        public string AlgorithmName { get; set; }
        public double AvgWaitingTime { get; set; }
        public double AvgTurnaroundTime { get; set; }
        public double AvgResponseTime { get; set; }
        public double CpuUtilization { get; set; }
        public double Throughput { get; set; }
        public List<Process> Processes { get; set; }
        public List<(int ProcessId, int StartTime, int EndTime)> Timeline { get; set; }

        public SimulationResult(string name)
        {
            AlgorithmName = name;
            Processes = new List<Process>();
            Timeline = new List<(int, int, int)>();
        }

        // Display the results
        public void Display()
        {
            Console.WriteLine($"\nResults for {AlgorithmName}:");
            Console.WriteLine($"Average Waiting Time: {AvgWaitingTime:F2}");
            Console.WriteLine($"Average Turnaround Time: {AvgTurnaroundTime:F2}");
            Console.WriteLine($"Average Response Time: {AvgResponseTime:F2}");
            Console.WriteLine($"CPU Utilization: {CpuUtilization:F2}%");
            Console.WriteLine($"Throughput: {Throughput:F4} processes/time unit");
        }

        // Display Gantt chart
        public void DisplayGantt()
        {
            Console.WriteLine($"\nGantt Chart for {AlgorithmName}:");
            Console.WriteLine("----------------------------------");
            foreach (var (processId, start, end) in Timeline)
            {
                string processName = processId == 0 ? "Idle" : $"P{processId}";
                Console.WriteLine($"{processName}: {start} -> {end}");
            }
        }
    }
}