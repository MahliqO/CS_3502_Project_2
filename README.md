# CS_3502_Project_2# CPU Scheduling Simulator

A comprehensive simulator for evaluating and comparing CPU scheduling algorithms, built with C#.

## Overview

This project implements a CPU scheduling simulator that allows users to analyze and compare the performance of various scheduling algorithms under different workloads. The simulator measures key performance metrics such as average waiting time, average turnaround time, CPU utilization, throughput, and response time.

## Features

- **Multiple Scheduling Algorithms**:
  - First-Come, First-Served (FCFS)
  - Shortest Job First (SJF)
  - Round Robin (RR) with configurable time quantum
  - Shortest Remaining Time First (SRTF)
  - Multi-Level Feedback Queue (MLFQ)

- **Comprehensive Metrics**:
  - Average Waiting Time (AWT)
  - Average Turnaround Time (ATT)
  - Average Response Time (ART)
  - CPU Utilization (%)
  - Throughput (processes per time unit)

- **Flexible Process Management**:
  - Generate random process sets
  - Manually input process details
  - Visualize execution with Gantt charts

- **Testing Framework**:
  - Predefined test cases
  - Edge case scenarios
  - Random workload generation
  - Automatic validation against expected results

## Project Structure

- **Process.cs** - Core data structure representing a process
- **SimulationResult.cs** - Collects and calculates performance metrics
- **Scheduler.cs** - Implements all scheduling algorithms
- **Program.cs** - Main application entry point
- **ProgramTest.cs** - Comprehensive testing framework
- **SchedulerTester.cs** - Testing utilities and validation

## Getting Started

### Prerequisites

- .NET SDK 6.0 or later
- Visual Studio 2019/2022 or Visual Studio Code

### Installation

1. Clone the repository:
```
git clone https://github.com/yourusername/CPUSchedulingSimulator.git
```

2. Navigate to the project directory:
```
cd CPUSchedulingSimulator
```

3. Build the project:
```
dotnet build
```

4. Run the application:
```
dotnet run
```

## Usage

When you run the application, you'll have two options:

### 1. Normal Mode

The standard interface allows you to:
- Generate random processes
- Enter processes manually
- Run individual scheduling algorithms
- Compare all algorithms
- View performance metrics and Gantt charts

### 2. Testing Mode

The testing interface provides:
- Basic test cases with known outcomes
- Edge case scenarios to stress algorithms
- Random process generation for large-scale testing
- Validation of algorithm correctness

## Key Algorithms Explained

### First-Come, First-Served (FCFS)
Non-preemptive algorithm that executes processes in the order they arrive in the ready queue.

### Shortest Job First (SJF)
Non-preemptive algorithm that selects the process with the shortest burst time from the ready queue.

### Round Robin (RR)
Preemptive algorithm that allocates each process a fixed time quantum in a circular manner.

### Shortest Remaining Time First (SRTF)
Preemptive version of SJF that selects the process with the shortest remaining time.

### Multi-Level Feedback Queue (MLFQ)
Uses multiple queues with different priority levels and dynamically moves processes between queues based on their behavior.

## Project Screenshots

[Include screenshots of the application here]

## Performance Analysis

The simulator reveals different performance characteristics for each algorithm:

- **FCFS**: Simple but may suffer from the convoy effect
- **SJF**: Optimal average waiting time but potential starvation
- **RR**: Good response time with increased context switching overhead
- **SRTF**: Best average waiting time with increased preemption overhead
- **MLFQ**: Balanced approach for mixed workloads

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

Potential areas for improvement:
- Adding more scheduling algorithms
- Implementing a graphical user interface
- Supporting I/O operations and multi-CPU scheduling
- Enhancing visualization capabilities

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

- This project was developed as part of an Operating Systems course assignment
- Inspired by theoretical concepts from Silberschatz, Galvin & Gagne's "Operating System Concepts"
