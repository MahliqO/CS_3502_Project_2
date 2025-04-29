using System;
using System.Collections.Generic;

namespace CPUSchedulingSimulator
{
    /// <summary>
    /// Main entry point class for the CPU Scheduling Simulator
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the application
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("CPU Scheduling Simulator");
            Console.WriteLine("=======================");
            
            Console.WriteLine("\nSelect mode:");
            Console.WriteLine("1. Run normal program");
            Console.WriteLine("2. Run testing suite");
            Console.Write("Enter your choice (1 or 2): ");
            
            string modeChoice = Console.ReadLine();
            
            if (modeChoice == "2")
            {
                // Run the testing suite
                ProgramTest.RunTests();
                return;
            }
            
            // Run the normal program
            var scheduler = new Scheduler();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Generate random processes");
                Console.WriteLine("2. Enter processes manually");
                Console.WriteLine("3. Display processes");
                Console.WriteLine("4. Run FCFS algorithm");
                Console.WriteLine("5. Run SJF algorithm");
                Console.WriteLine("6. Run Round Robin algorithm");
                Console.WriteLine("7. Run SRTF algorithm (new)");
                Console.WriteLine("8. Run MLFQ algorithm (new)");
                Console.WriteLine("9. Compare all algorithms");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        Console.Write("Number of processes: ");
                        int count = int.Parse(Console.ReadLine());
                        scheduler.GenerateRandomProcesses(count);
                        Console.WriteLine($"{count} processes generated.");
                        break;

                    case "2":
                        EnterProcessesManually(scheduler);
                        break;

                    case "3":
                        scheduler.DisplayProcesses();
                        break;

                    case "4":
                        var fcfsResult = scheduler.FCFS();
                        fcfsResult.Display();
                        fcfsResult.DisplayGantt();
                        break;

                    case "5":
                        var sjfResult = scheduler.SJF();
                        sjfResult.Display();
                        sjfResult.DisplayGantt();
                        break;

                    case "6":
                        Console.Write("Time quantum: ");
                        int quantum = int.Parse(Console.ReadLine());
                        var rrResult = scheduler.RoundRobin(quantum);
                        rrResult.Display();
                        rrResult.DisplayGantt();
                        break;

                    case "7":
                        var srtfResult = scheduler.SRTF();
                        srtfResult.Display();
                        srtfResult.DisplayGantt();
                        break;

                    case "8":
                        var mlfqResult = scheduler.MLFQ();
                        mlfqResult.Display();
                        mlfqResult.DisplayGantt();
                        break;

                    case "9":
                        scheduler.CompareAlgorithms();
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            
            Console.WriteLine("\nThank you for using the CPU Scheduling Simulator!");
        }

        /// <summary>
        /// Allows the user to manually enter process information
        /// </summary>
        static void EnterProcessesManually(Scheduler scheduler)
        {
            Console.Write("Number of processes: ");
            int count = int.Parse(Console.ReadLine());
            
            List<Process> processes = new List<Process>();
            
            for (int i = 1; i <= count; i++)
            {
                Console.WriteLine($"\nProcess {i}:");
                
                Console.Write("Arrival time: ");
                int arrivalTime = int.Parse(Console.ReadLine());
                
                Console.Write("Burst time: ");
                int burstTime = int.Parse(Console.ReadLine());
                
                Console.Write("Priority: ");
                int priority = int.Parse(Console.ReadLine());
                
                processes.Add(new Process(i, arrivalTime, burstTime, priority));
            }
            
            scheduler.SetProcesses(processes);
            Console.WriteLine($"{count} processes entered successfully.");
        }
    }
}