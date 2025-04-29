using System;

namespace CPUSchedulingSimulator
{
    /// <summary>
    /// Test class for the CPU Scheduling Simulator without a Main method
    /// </summary>
    public class ProgramTest
    {
        /// <summary>
        /// Runs the testing interface for the CPU Scheduling Simulator
        /// </summary>
        public static void RunTests()
        {
            Console.WriteLine("CPU Scheduling Simulator - Testing Suite");
            Console.WriteLine("========================================");
            
            // Create a tester instance
            SchedulerTester tester = new SchedulerTester();
            
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nTesting Menu:");
                Console.WriteLine("1. Run all tests");
                Console.WriteLine("2. Run basic test case");
                Console.WriteLine("3. Run edge case - mixed burst times");
                Console.WriteLine("4. Run random process test (10 processes)");
                Console.WriteLine("5. Run random process test (20 processes)");
                Console.WriteLine("6. Validate algorithm correctness");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice: ");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        tester.RunAllTests();
                        break;
                        
                    case "2":
                        var basicTest = new System.Collections.Generic.List<Process>
                        {
                            new Process(1, 0, 6, 3),
                            new Process(2, 2, 4, 1),
                            new Process(3, 4, 2, 2)
                        };
                        tester.RunTestCase(basicTest, "Basic Test Case");
                        break;
                        
                    case "3":
                        var edgeCase = new System.Collections.Generic.List<Process>
                        {
                            new Process(1, 0, 1, 5),
                            new Process(2, 1, 20, 3),
                            new Process(3, 2, 2, 2),
                            new Process(4, 3, 15, 4),
                            new Process(5, 4, 3, 1)
                        };
                        tester.RunTestCase(edgeCase, "Edge Case - Mixed Burst Times");
                        break;
                        
                    case "4":
                        var random10 = new System.Collections.Generic.List<Process>();
                        Random rand = new Random();
                        for (int i = 1; i <= 10; i++)
                        {
                            int arrivalTime = rand.Next(0, 20);
                            int burstTime = rand.Next(1, 15);
                            int priority = rand.Next(1, 10);
                            random10.Add(new Process(i, arrivalTime, burstTime, priority));
                        }
                        tester.RunTestCase(random10, "Random 10 Processes");
                        break;
                        
                    case "5":
                        var random20 = new System.Collections.Generic.List<Process>();
                        Random rand2 = new Random();
                        for (int i = 1; i <= 20; i++)
                        {
                            int arrivalTime = rand2.Next(0, 20);
                            int burstTime = rand2.Next(1, 15);
                            int priority = rand2.Next(1, 10);
                            random20.Add(new Process(i, arrivalTime, burstTime, priority));
                        }
                        tester.RunTestCase(random20, "Random 20 Processes");
                        break;
                        
                    case "6":
                        tester.ValidateBasicTestCase();
                        break;
                        
                    case "0":
                        exit = true;
                        break;
                        
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            
            Console.WriteLine("\nTesting completed. Thank you for using the CPU Scheduling Simulator!");
        }
    }
}