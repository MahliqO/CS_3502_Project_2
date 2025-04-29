using System;
using System.Collections.Generic;
using System.Linq;

namespace CPUSchedulingSimulator
{
    // Main CPU Scheduling Simulator class
    public class Scheduler
    {
        private List<Process> _processes;

        public Scheduler()
        {
            _processes = new List<Process>();
        }

        // Generate random processes
        public void GenerateRandomProcesses(int count, int maxArrivalTime = 10, int maxBurstTime = 10, int maxPriority = 10)
        {
            _processes.Clear();
            Random random = new Random();

            for (int i = 1; i <= count; i++)
            {
                int arrivalTime = random.Next(maxArrivalTime);
                int burstTime = random.Next(1, maxBurstTime + 1);
                int priority = random.Next(1, maxPriority + 1);
                _processes.Add(new Process(i, arrivalTime, burstTime, priority));
            }
        }

        // Set processes manually
        public void SetProcesses(List<Process> processes)
        {
            _processes = processes;
        }

        // Calculate performance metrics
        private void CalculateMetrics(SimulationResult result)
        {
            if (result.Processes.Count == 0) return;

            result.AvgWaitingTime = result.Processes.Average(p => p.WaitingTime);
            result.AvgTurnaroundTime = result.Processes.Average(p => p.TurnaroundTime);
            
            var respondedProcesses = result.Processes.Where(p => p.ResponseTime >= 0).ToList();
            result.AvgResponseTime = respondedProcesses.Count > 0 ? respondedProcesses.Average(p => p.ResponseTime) : 0;
            
            int totalExecutionTime = result.Processes.Max(p => p.CompletionTime);
            int totalBurstTime = result.Processes.Sum(p => p.BurstTime);
            
            result.CpuUtilization = (double)totalBurstTime / totalExecutionTime * 100;
            result.Throughput = (double)result.Processes.Count / totalExecutionTime;
        }

        // First-Come, First-Served Algorithm
        public SimulationResult FCFS()
        {
            var result = new SimulationResult("First-Come, First-Served (FCFS)");
            var processes = _processes.Select(p => p.Clone()).OrderBy(p => p.ArrivalTime).ToList();
            result.Processes = processes;

            int currentTime = 0;

            foreach (var process in processes)
            {
                if (currentTime < process.ArrivalTime)
                {
                    // Add idle time to timeline
                    result.Timeline.Add((0, currentTime, process.ArrivalTime));
                    currentTime = process.ArrivalTime;
                }

                // Record response time
                process.ResponseTime = currentTime - process.ArrivalTime;
                process.HasStarted = true;

                // Execute process
                result.Timeline.Add((process.Id, currentTime, currentTime + process.BurstTime));
                currentTime += process.BurstTime;

                // Update process metrics
                process.CompletionTime = currentTime;
                process.TurnaroundTime = process.CompletionTime - process.ArrivalTime;
                process.WaitingTime = process.TurnaroundTime - process.BurstTime;
            }

            CalculateMetrics(result);
            return result;
        }

        // Shortest Job First Algorithm
        public SimulationResult SJF()
        {
            var result = new SimulationResult("Shortest Job First (SJF)");
            var processes = _processes.Select(p => p.Clone()).ToList();
            result.Processes = processes;

            int currentTime = 0;
            int completedProcesses = 0;
            var readyQueue = new List<Process>();

            while (completedProcesses < processes.Count)
            {
                // Add newly arrived processes to ready queue
                foreach (var process in processes.Where(p => p.ArrivalTime <= currentTime && !p.HasStarted))
                {
                    readyQueue.Add(process);
                    process.HasStarted = true;
                }

                if (readyQueue.Count == 0)
                {
                    // Find next arrival
                    var nextProcess = processes.Where(p => !p.HasStarted)
                        .OrderBy(p => p.ArrivalTime).FirstOrDefault();
                    
                    if (nextProcess != null)
                    {
                        result.Timeline.Add((0, currentTime, nextProcess.ArrivalTime));
                        currentTime = nextProcess.ArrivalTime;
                        continue;
                    }
                }

                // Select process with shortest burst time
                var selectedProcess = readyQueue.OrderBy(p => p.BurstTime).First();
                readyQueue.Remove(selectedProcess);

                // Record response time if first time running
                if (selectedProcess.ResponseTime == -1)
                {
                    selectedProcess.ResponseTime = currentTime - selectedProcess.ArrivalTime;
                }

                // Execute process
                result.Timeline.Add((selectedProcess.Id, currentTime, currentTime + selectedProcess.BurstTime));
                currentTime += selectedProcess.BurstTime;

                // Update process metrics
                selectedProcess.CompletionTime = currentTime;
                selectedProcess.TurnaroundTime = selectedProcess.CompletionTime - selectedProcess.ArrivalTime;
                selectedProcess.WaitingTime = selectedProcess.TurnaroundTime - selectedProcess.BurstTime;
                selectedProcess.RemainingTime = 0;
                completedProcesses++;
            }

            CalculateMetrics(result);
            return result;
        }

        // Round Robin Algorithm
        public SimulationResult RoundRobin(int timeQuantum = 2)
        {
            var result = new SimulationResult($"Round Robin (RR) - Quantum: {timeQuantum}");
            var processes = _processes.Select(p => p.Clone()).ToList();
            result.Processes = processes;

            int currentTime = 0;
            int completedProcesses = 0;
            var readyQueue = new Queue<Process>();

            while (completedProcesses < processes.Count)
            {
                // Add newly arrived processes to ready queue
                foreach (var process in processes.Where(p => p.ArrivalTime <= currentTime && 
                                                          p.RemainingTime > 0 && 
                                                          !readyQueue.Contains(p) && 
                                                          !p.HasStarted))
                {
                    readyQueue.Enqueue(process);
                    process.HasStarted = true;
                }

                if (readyQueue.Count == 0)
                {
                    // Find next arrival
                    var nextProcess = processes.Where(p => p.RemainingTime > 0 && p.ArrivalTime > currentTime)
                        .OrderBy(p => p.ArrivalTime).FirstOrDefault();
                    
                    if (nextProcess != null)
                    {
                        result.Timeline.Add((0, currentTime, nextProcess.ArrivalTime));
                        currentTime = nextProcess.ArrivalTime;
                        continue;
                    }
                    else
                    {
                        // Check for incomplete processes
                        var incompleteProcesses = processes.Where(p => p.RemainingTime > 0).ToList();
                        if (incompleteProcesses.Count > 0)
                        {
                            foreach (var process in incompleteProcesses)
                            {
                                if (!readyQueue.Contains(process))
                                {
                                    readyQueue.Enqueue(process);
                                }
                            }
                        }
                        else
                        {
                            break; // All processes completed
                        }
                    }
                }

                // Get the next process
                Process currentProcess = readyQueue.Dequeue();

                // Record response time if first time running
                if (currentProcess.ResponseTime == -1)
                {
                    currentProcess.ResponseTime = currentTime - currentProcess.ArrivalTime;
                }

                // Determine execution time
                int executeTime = Math.Min(timeQuantum, currentProcess.RemainingTime);

                // Execute the process
                result.Timeline.Add((currentProcess.Id, currentTime, currentTime + executeTime));
                currentTime += executeTime;
                currentProcess.RemainingTime -= executeTime;

                // Check for new arrivals during execution
                foreach (var process in processes.Where(p => p.ArrivalTime > currentTime - executeTime && 
                                                          p.ArrivalTime <= currentTime && 
                                                          !readyQueue.Contains(p) && 
                                                          p != currentProcess))
                {
                    readyQueue.Enqueue(process);
                    process.HasStarted = true;
                }

                // Check if process is completed
                if (currentProcess.RemainingTime <= 0)
                {
                    currentProcess.CompletionTime = currentTime;
                    currentProcess.TurnaroundTime = currentProcess.CompletionTime - currentProcess.ArrivalTime;
                    currentProcess.WaitingTime = currentProcess.TurnaroundTime - currentProcess.BurstTime;
                    completedProcesses++;
                }
                else
                {
                    readyQueue.Enqueue(currentProcess);
                }
            }

            CalculateMetrics(result);
            return result;
        }

        // Shortest Remaining Time First Algorithm (SRTF) - Preemptive version of SJF
        public SimulationResult SRTF()
        {
            var result = new SimulationResult("Shortest Remaining Time First (SRTF)");
            var processes = _processes.Select(p => p.Clone()).ToList();
            result.Processes = processes;

            int currentTime = 0;
            int completedProcesses = 0;
            Process currentProcess = null;
            int lastExecutionStartTime = 0;
            var readyQueue = new List<Process>();

            while (completedProcesses < processes.Count)
            {
                // Add newly arrived processes to ready queue
                foreach (var process in processes.Where(p => p.ArrivalTime <= currentTime && 
                                                          p.RemainingTime > 0 && 
                                                          !readyQueue.Contains(p) && 
                                                          p != currentProcess))
                {
                    readyQueue.Add(process);
                }

                // Check if preemption is needed
                bool shouldPreempt = false;
                if (currentProcess != null && currentProcess.RemainingTime > 0)
                {
                    var shorterProcess = readyQueue.OrderBy(p => p.RemainingTime).FirstOrDefault();
                    if (shorterProcess != null && shorterProcess.RemainingTime < currentProcess.RemainingTime)
                    {
                        shouldPreempt = true;
                        
                        // Update current process remaining time
                        int executedTime = currentTime - lastExecutionStartTime;
                        currentProcess.RemainingTime -= executedTime;
                        
                        // Add to timeline
                        result.Timeline.Add((currentProcess.Id, lastExecutionStartTime, currentTime));
                        
                        // Put back in ready queue
                        readyQueue.Add(currentProcess);
                    }
                }

                // If no current process or preemption occurred, select a new process
                if (currentProcess == null || currentProcess.RemainingTime <= 0 || shouldPreempt)
                {
                    if (readyQueue.Count == 0)
                    {
                        // Find next arrival
                        var nextProcess = processes.Where(p => p.RemainingTime > 0 && p.ArrivalTime > currentTime)
                            .OrderBy(p => p.ArrivalTime).FirstOrDefault();
                        
                        if (nextProcess != null)
                        {
                            result.Timeline.Add((0, currentTime, nextProcess.ArrivalTime));
                            currentTime = nextProcess.ArrivalTime;
                            continue;
                        }
                        else
                        {
                            break; // All processes completed
                        }
                    }

                    // Select process with shortest remaining time
                    currentProcess = readyQueue.OrderBy(p => p.RemainingTime).First();
                    readyQueue.Remove(currentProcess);
                    lastExecutionStartTime = currentTime;

                    // Record response time if first time running
                    if (currentProcess.ResponseTime == -1)
                    {
                        currentProcess.ResponseTime = currentTime - currentProcess.ArrivalTime;
                    }
                }

                // Determine execution time (until next arrival or completion)
                int executeTime;
                var nextArrival = processes.Where(p => p.ArrivalTime > currentTime)
                    .OrderBy(p => p.ArrivalTime).FirstOrDefault();

                if (nextArrival != null)
                {
                    executeTime = Math.Min(currentProcess.RemainingTime, nextArrival.ArrivalTime - currentTime);
                }
                else
                {
                    executeTime = currentProcess.RemainingTime;
                }

                // Execute the process
                result.Timeline.Add((currentProcess.Id, currentTime, currentTime + executeTime));
                currentTime += executeTime;
                currentProcess.RemainingTime -= executeTime;

                // Check if process is completed
                if (currentProcess.RemainingTime <= 0)
                {
                    currentProcess.CompletionTime = currentTime;
                    currentProcess.TurnaroundTime = currentProcess.CompletionTime - currentProcess.ArrivalTime;
                    currentProcess.WaitingTime = currentProcess.TurnaroundTime - currentProcess.BurstTime;
                    completedProcesses++;
                    currentProcess = null;
                }
            }

            CalculateMetrics(result);
            return result;
        }

        // Multi-Level Feedback Queue Algorithm
        public SimulationResult MLFQ(int numQueues = 3, int baseQuantum = 2)
        {
            var result = new SimulationResult($"Multi-Level Feedback Queue (MLFQ) - Queues: {numQueues}, Base Quantum: {baseQuantum}");
            var processes = _processes.Select(p => p.Clone()).ToList();
            result.Processes = processes;

            int currentTime = 0;
            int completedProcesses = 0;
            
            // Create queues for each priority level
            List<Queue<Process>> readyQueues = new List<Queue<Process>>();
            for (int i = 0; i < numQueues; i++)
            {
                readyQueues.Add(new Queue<Process>());
            }
            
            // Track priority level for each process
            Dictionary<int, int> processPriorityLevels = new Dictionary<int, int>();
            foreach (var process in processes)
            {
                processPriorityLevels[process.Id] = 0; // Start at highest priority
            }

            while (completedProcesses < processes.Count)
            {
                // Add newly arrived processes to highest priority queue
                foreach (var process in processes.Where(p => p.ArrivalTime <= currentTime && 
                                                          !p.HasStarted && 
                                                          p.RemainingTime > 0))
                {
                    readyQueues[0].Enqueue(process);
                    process.HasStarted = true;
                }

                // Find highest non-empty queue
                int currentLevel = -1;
                for (int i = 0; i < numQueues; i++)
                {
                    if (readyQueues[i].Count > 0)
                    {
                        currentLevel = i;
                        break;
                    }
                }

                if (currentLevel == -1)
                {
                    // Find next arrival
                    var nextProcess = processes.Where(p => p.RemainingTime > 0 && p.ArrivalTime > currentTime)
                        .OrderBy(p => p.ArrivalTime).FirstOrDefault();
                    
                    if (nextProcess != null)
                    {
                        result.Timeline.Add((0, currentTime, nextProcess.ArrivalTime));
                        currentTime = nextProcess.ArrivalTime;
                        continue;
                    }
                    else
                    {
                        break; // All processes completed
                    }
                }

                // Get process from current queue
                Process currentProcess = readyQueues[currentLevel].Dequeue();
                
                // Calculate time quantum for this level
                int timeQuantum = baseQuantum * (1 << currentLevel); // Double quantum for each lower level
                
                // Record response time if first time running
                if (currentProcess.ResponseTime == -1)
                {
                    currentProcess.ResponseTime = currentTime - currentProcess.ArrivalTime;
                }
                
                // Determine execution time
                int executeTime = Math.Min(timeQuantum, currentProcess.RemainingTime);
                
                // Execute the process
                result.Timeline.Add((currentProcess.Id, currentTime, currentTime + executeTime));
                currentTime += executeTime;
                currentProcess.RemainingTime -= executeTime;
                
                // Add new arrivals during execution
                foreach (var process in processes.Where(p => p.ArrivalTime > currentTime - executeTime && 
                                                          p.ArrivalTime <= currentTime && 
                                                          !p.HasStarted))
                {
                    readyQueues[0].Enqueue(process);
                    process.HasStarted = true;
                }
                
                // Check if process is completed
                if (currentProcess.RemainingTime <= 0)
                {
                    currentProcess.CompletionTime = currentTime;
                    currentProcess.TurnaroundTime = currentProcess.CompletionTime - currentProcess.ArrivalTime;
                    currentProcess.WaitingTime = currentProcess.TurnaroundTime - currentProcess.BurstTime;
                    completedProcesses++;
                }
                else
                {
                    // Move to lower priority queue
                    int nextLevel = Math.Min(currentLevel + 1, numQueues - 1);
                    readyQueues[nextLevel].Enqueue(currentProcess);
                    processPriorityLevels[currentProcess.Id] = nextLevel;
                }
            }

            CalculateMetrics(result);
            return result;
        }

        // Compare all algorithms and display results
        public void CompareAlgorithms()
        {
            if (_processes.Count == 0)
            {
                Console.WriteLine("No processes to compare. Please generate or set processes first.");
                return;
            }

            Console.WriteLine("\nComparing Scheduling Algorithms...");
            
            // Run all algorithms
            var results = new List<SimulationResult>
            {
                FCFS(),
                SJF(),
                RoundRobin(),
                SRTF(),
                MLFQ()
            };

            // Display comparison table
            Console.WriteLine("\nComparative Results:");
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
        }

        // Display the processes
        public void DisplayProcesses()
        {
            Console.WriteLine("\nProcess List:");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("| ID | Arrival Time | Burst Time | Priority |");
            Console.WriteLine("--------------------------------------------------");
            
            foreach (var process in _processes)
            {
                Console.WriteLine($"| {process.Id,2} | {process.ArrivalTime,12} | {process.BurstTime,10} | {process.Priority,8} |");
            }
            
            Console.WriteLine("--------------------------------------------------");
        }
    }
}