 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CPUSchedulingSimulator
{
    /// <summary>
    /// Class for testing CPU scheduling algorithms
    /// </summary>
    public class SchedulerTester
    {
        private Scheduler _scheduler;
        
        public SchedulerTester()
        {
            _scheduler = new Scheduler();
        }
        
        /// <summary>
        /// Runs a test case with the specified processes and name
        /// </summary>
        public void RunTestCase(List<Process> processes, string testName)
        {
            Console.WriteLine($"\n=== Running Test: {testName} ===");
            Console.WriteLine($"Number of processes: {processes.Count}");
            
            // Display test process details
            Console.WriteLine("\nProcess Details:");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("| ID | Arrival Time | Burst Time | Priority |");
            Console.WriteLine("--------------------------------------------------");
            
            foreach (var process in processes)
            {
                Console.WriteLine($"| {process.Id,2} | {process.ArrivalTime,12} | {process.BurstTime,10} | {process.Priority,8} |");
            }
            
            Console.WriteLine("--------------------------------------------------");
            
            // Set the processes for the scheduler
            _scheduler.SetProcesses(processes);
            
            // Run all algorithms
            var fcfsResult = _scheduler.FCFS();
            var sjfResult = _scheduler.SJF();
            var rrResult = _scheduler.RoundRobin(2); // Using quantum = 2
            var srtfResult = _scheduler.SRTF();
            var mlfqResult = _scheduler.MLFQ();
            
            // Collect results
            var results = new List<SimulationResult>
            {
                fcfsResult,
                sjfResult,
                rrResult,
                srtfResult,
                mlfqResult
            };
            
            // Display comparison table
            Console.WriteLine("\nResults Summary:");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("| Algorithm                      | Avg Wait Time | Avg Turnaround | Avg Response | CPU Util (%) | Throughput     |");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            
            foreach (var result in results)
            {
                Console.WriteLine($"| {result.AlgorithmName.PadRight(30)} | " +
                                 $"{result.AvgWaitingTime,13:F2} | " +
                                 $"{result.AvgTurnaroundTime,14:F2} | " +
                                 $"{result.AvgResponseTime,12:F2} | " +
                                 $"{result.CpuUtilization,12:F2} | " +
                                 $"{result.Throughput,14:F4} |");
            }
            
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------");
            
            // Save results to CSV file
            SaveResultsToCsv(results, $"{testName.Replace(" ", "_")}_results.csv");
        }
        
        /// <summary>
        /// Runs a comprehensive test suite with various test cases
        /// </summary>
        public void RunAllTests()
        {
            Console.WriteLine("Starting CPU Scheduler Testing Suite");
            Console.WriteLine("====================================");
            
            // Test Case 1: Basic test with varied arrival times
            var testCase1 = new List<Process>
            {
                new Process(1, 0, 6, 3),  // Process 1: Arrival=0, Burst=6, Priority=3
                new Process(2, 2, 4, 1),  // Process 2: Arrival=2, Burst=4, Priority=1
                new Process(3, 4, 2, 2)   // Process 3: Arrival=4, Burst=2, Priority=2
            };
            RunTestCase(testCase1, "Basic Test - Varied Arrival Times");
            
            // Test Case 2: All processes arrive at same time
            var testCase2 = new List<Process>
            {
                new Process(1, 0, 8, 3),  // Process 1: Arrival=0, Burst=8, Priority=3
                new Process(2, 0, 4, 1),  // Process 2: Arrival=0, Burst=4, Priority=1
                new Process(3, 0, 2, 2)   // Process 3: Arrival=0, Burst=2, Priority=2
            };
            RunTestCase(testCase2, "All Processes Same Arrival Time");
            
            // Test Case 3: Processes with sequential arrivals
            var testCase3 = new List<Process>
            {
                new Process(1, 0, 3, 2),  // Process 1: Arrival=0, Burst=3, Priority=2
                new Process(2, 3, 3, 3),  // Process 2: Arrival=3, Burst=3, Priority=3
                new Process(3, 6, 3, 1)   // Process 3: Arrival=6, Burst=3, Priority=1
            };
            RunTestCase(testCase3, "Sequential Arrivals");
            
            // Edge Case 1: Mix of very short and very long burst times
            var edgeCase1 = new List<Process>
            {
                new Process(1, 0, 1, 5),    // Very short
                new Process(2, 1, 20, 3),   // Very long
                new Process(3, 2, 2, 2),    // Very short
                new Process(4, 3, 15, 4),   // Very long
                new Process(5, 4, 3, 1)     // Very short
            };
            RunTestCase(edgeCase1, "Edge Case - Mixed Burst Times");
            
            // Edge Case 2: Extreme priority differences
            var edgeCase2 = new List<Process>
            {
                new Process(1, 0, 10, 1),   // Highest priority
                new Process(2, 0, 8, 10),   // Lowest priority
                new Process(3, 0, 6, 2),    // High priority
                new Process(4, 0, 4, 9),    // Low priority
                new Process(5, 0, 2, 5)     // Medium priority
            };
            RunTestCase(edgeCase2, "Edge Case - Priority Differences");
            
            // Larger scale test - 10 processes with random parameters
            RunTestCase(GenerateRandomProcessSet(10), "Random 10 Processes");
            
            // Larger scale test - 20 processes with random parameters
            RunTestCase(GenerateRandomProcessSet(20), "Random 20 Processes");
            
            Console.WriteLine("\nAll tests completed successfully.");
        }
        
        /// <summary>
        /// Generates a set of random processes for testing
        /// </summary>
        private List<Process> GenerateRandomProcessSet(int count)
        {
            Random random = new Random();
            List<Process> processes = new List<Process>();
            
            for (int i = 1; i <= count; i++)
            {
                int arrivalTime = random.Next(0, 20);
                int burstTime = random.Next(1, 15);
                int priority = random.Next(1, 10);
                processes.Add(new Process(i, arrivalTime, burstTime, priority));
            }
            
            return processes;
        }
        
        /// <summary>
        /// Saves test results to a CSV file
        /// </summary>
        private void SaveResultsToCsv(List<SimulationResult> results, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    // Write header
                    writer.WriteLine("Algorithm,Avg Waiting Time,Avg Turnaround Time,Avg Response Time,CPU Utilization,Throughput");
                    
                    // Write data rows
                    foreach (var result in results)
                    {
                        writer.WriteLine($"{result.AlgorithmName},{result.AvgWaitingTime:F2},{result.AvgTurnaroundTime:F2}," +
                                        $"{result.AvgResponseTime:F2},{result.CpuUtilization:F2},{result.Throughput:F4}");
                    }
                }
                
                Console.WriteLine($"Results saved to {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving results to CSV: {ex.Message}");
            }
        }
        
        /// <summary>
        /// Validates algorithm results against manually calculated expected values
        /// </summary>
        public void ValidateBasicTestCase()
        {
            Console.WriteLine("\n=== Validating Basic Test Case ===");
            
            // Create a simple test case with known expected results
            var processes = new List<Process>
            {
                new Process(1, 0, 6, 3),
                new Process(2, 2, 4, 1),
                new Process(3, 4, 2, 2)
            };
            
            _scheduler.SetProcesses(processes);
            
            // Run FCFS and validate results
            var fcfsResult = _scheduler.FCFS();
            Console.WriteLine("\nValidating FCFS Results:");
            
            // Expected values for FCFS (manually calculated)
            double expectedFcfsAvgWaitingTime = 2.67; // (0 + 4 + 4) / 3
            double expectedFcfsAvgTurnaroundTime = 7.67; // (6 + 8 + 6) / 3
            
            // Check if results match expected values
            bool fcfsValid = Math.Abs(fcfsResult.AvgWaitingTime - expectedFcfsAvgWaitingTime) < 0.1 &&
                            Math.Abs(fcfsResult.AvgTurnaroundTime - expectedFcfsAvgTurnaroundTime) < 0.1;
            
            Console.WriteLine($"FCFS Waiting Time: Expected={expectedFcfsAvgWaitingTime:F2}, Actual={fcfsResult.AvgWaitingTime:F2}");
            Console.WriteLine($"FCFS Turnaround Time: Expected={expectedFcfsAvgTurnaroundTime:F2}, Actual={fcfsResult.AvgTurnaroundTime:F2}");
            Console.WriteLine($"FCFS Validation: {(fcfsValid ? "PASSED" : "FAILED")}");
            
            // Run SJF and validate results
            var sjfResult = _scheduler.SJF();
            Console.WriteLine("\nValidating SJF Results:");
            
            // Expected values for SJF (manually calculated)
            double expectedSjfAvgWaitingTime = 2.33; // (0 + 0 + 7) / 3
            double expectedSjfAvgTurnaroundTime = 7.33; // (6 + 4 + 12) / 3
            
            // Check if results match expected values
            bool sjfValid = Math.Abs(sjfResult.AvgWaitingTime - expectedSjfAvgWaitingTime) < 0.1 &&
                            Math.Abs(sjfResult.AvgTurnaroundTime - expectedSjfAvgTurnaroundTime) < 0.1;
            
            Console.WriteLine($"SJF Waiting Time: Expected={expectedSjfAvgWaitingTime:F2}, Actual={sjfResult.AvgWaitingTime:F2}");
            Console.WriteLine($"SJF Turnaround Time: Expected={expectedSjfAvgTurnaroundTime:F2}, Actual={sjfResult.AvgTurnaroundTime:F2}");
            Console.WriteLine($"SJF Validation: {(sjfValid ? "PASSED" : "FAILED")}");
            
            // Additional validations for other algorithms can be added here
        }
    }
}