using System;

namespace CPUSchedulingSimulator
{
    // Process class to represent a process in the system
    public class Process
    {
        public int Id { get; set; }
        public int ArrivalTime { get; set; }
        public int BurstTime { get; set; }
        public int RemainingTime { get; set; }
        public int Priority { get; set; }
        public int CompletionTime { get; set; }
        public int WaitingTime { get; set; }
        public int TurnaroundTime { get; set; }
        public int ResponseTime { get; set; }
        public bool HasStarted { get; set; }

        public Process(int id, int arrivalTime, int burstTime, int priority = 0)
        {
            Id = id;
            ArrivalTime = arrivalTime;
            BurstTime = burstTime;
            RemainingTime = burstTime;
            Priority = priority;
            CompletionTime = 0;
            WaitingTime = 0;
            TurnaroundTime = 0;
            ResponseTime = -1;
            HasStarted = false;
        }

        // Create a clone of the process
        public Process Clone()
        {
            return new Process(Id, ArrivalTime, BurstTime, Priority)
            {
                RemainingTime = RemainingTime,
                CompletionTime = CompletionTime,
                WaitingTime = WaitingTime,
                TurnaroundTime = TurnaroundTime,
                ResponseTime = ResponseTime,
                HasStarted = HasStarted
            };
        }
    }
}