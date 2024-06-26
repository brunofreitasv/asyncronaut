# Asyncronaut - Simplify Asynchronous Task Management in .NET

This library simplifies the execution and management of asynchronous tasks in a structured and efficient way;,. It provides a clear approach for defining stages with sequential or parallel execution modes, capturing results from previous tasks.

## Pipelines Module

[View the src folder](https://github.com/brunofreitasv/asyncronaut/tree/master/src/Asyncronaut/Pipeline)

Pipelines module is designed to facilitate the construction and execution of complex asynchronous workflows. It offers a structured and intuitive approach to managing a series of interdependent tasks, making it easier to handle concurrency, error management, and result propagation in your applications.

### Key Features

- **Intuitive Pipeline Construction**: Easily define stages with clear names and execution modes (sequential or parallel).
- **Flexible Task Management**: Add tasks with or without input/output parameters using lambdas or functions.
- **Error Handling and Rollback**: Automatic error handling within stages and the entire pipeline with exception messages.
- **Improved Code Readability**: Organize complex asynchronous workflows with a concise and easy-to-follow syntax.

### Core Concepts

- **Pipeline:** Represents the overall workflow consisting of one or more stages.
- **PipelineStage:** Encapsulates a group of tasks that are executed sequentially or in parallel based on the defined mode.
- **PipelineTask:** Represents a single asynchronous task within a stage.
- **ExecutionMode:** Defines how tasks within a stage are executed (sequential or parallel).

### Getting Started

Here's a basic example demonstrating how to create a pipeline with two stages:

```csharp
using System;
using System.Threading.Tasks;
using Asyncronaut.Pipeline;

public class ExampleUsage
{
    public static async Task RunPipeline()
    {
        var pipeline = new Pipeline("My Pipeline");

        pipeline.AddStage("Stage 1 - Sequential Tasks", stage =>
        {
            stage.SetExecutionMode(ExecutionMode.Sequential)
                .AddTask(WriteToConsoleAndWait, ("Sequential Task 1.1", TimeSpan.FromSeconds(3)))
                .AddTask(WriteToConsoleAndWait, ("Sequential Task 1.2", TimeSpan.FromSeconds(4)));
        });

        pipeline.AddStage("Stage 2 - Parallel Tasks", stage =>
        {
            stage.SetExecutionMode(ExecutionMode.Parallel)
                .AddTask(WriteToConsoleAndWait, ("Parallel Task 2.1", TimeSpan.Zero))
                .AddTask(WriteToConsoleAndWait, ("Parallel Task 2.2", TimeSpan.Zero));
        });

        await pipeline.RunAsync();
    }

    public static async Task WriteToConsoleAndWait((string message, TimeSpan delay) input)
    {
        Console.WriteLine(input.message);
        await Task.Delay(input.delay);
    }

    public static void Main(string[] args)
    {
        RunPipeline().Wait();
    }
}
```

### Explanation

- We create a new `Pipeline` instance with a descriptive name.
- We define two stages using the `AddStage` method, each with a descriptive stage name.
- Within each stage, we use the `PipelineStageBuilder` to configure tasks and their execution mode.
- Tasks are added using various `AddTask` methods depending on whether they require input, capture output, or simply execute an asynchronous operation.
- Finally, we call the `RunAsync` method on the pipeline to initiate the asynchronous workflow.

### Advanced Usage

- **Capturing Results:** Use `AddTask` with output parameters (`Task<TOutput>`) to capture results from previous tasks and pass them to subsequent tasks within the same pipeline.
- **Error Handling:** Exceptions within tasks are automatically caught by the pipeline. The `HasError` property on `PipelineStage` and `ErrorMessage` provide details about encountered errors.