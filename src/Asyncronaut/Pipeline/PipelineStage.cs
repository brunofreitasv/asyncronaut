using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asyncronaut.Pipeline
{
    public class PipelineStage
    {
        private ExecutionMode _executionMode = ExecutionMode.Parallel;
        private readonly string _stageName;
        private readonly List<PipelineTask> _tasks;
        private bool _hasError;
        private string _errorMessage;

        public bool HasError => _hasError;
        public string ErrorMessage => _errorMessage;

        public PipelineStage(string stageName)
        {
            _stageName = stageName;
            _tasks = new List<PipelineTask>();
        }

        public void SetExecutionMode(ExecutionMode mode)
        {
            _executionMode = mode;
        }

        public void AddTask(Func<Task> task)
        {
            _tasks.Add(new PipelineTask(task));
        }

        public void AddTask<TInput>(Func<TInput, Task> task, TInput input)
        {
            _tasks.Add(new PipelineTask(async () =>
            {
                await task.Invoke(input);
            }));
        }

        public void AddTask<TOutput>(Func<Task<TOutput>> task, out Task<TOutput> output)
        {
            var captureOutput = new TaskCompletionSource<TOutput>();
            _tasks.Add(new PipelineTask(async () =>
            {
                var result = await task.Invoke();
                captureOutput.SetResult(result);
            }));
            output = captureOutput.Task;
        }

        public void AddTask<TInput, TOutput>(Func<TInput, Task<TOutput>> task, TInput input, out Task<TOutput> output)
        {
            var captureOutput = new TaskCompletionSource<TOutput>();
            _tasks.Add(new PipelineTask(async () =>
            {
                var result = await task.Invoke(input);
                captureOutput.SetResult(result);
            }));
            output = captureOutput.Task;
        }

        public async Task RunAsync()
        {
            if (_hasError)
                return;

            if (_executionMode == ExecutionMode.Sequential)
            {
                foreach (var task in _tasks)
                    await RunTask(task);
            }
            else
            {
                await Task.WhenAll(_tasks.Select(RunTask));
            }
        }

        private async Task RunTask(PipelineTask task)
        {
            try
            {
                await task.RunAsync();
            }
            catch (Exception ex)
            {
                _hasError = true;
                _errorMessage = $"Error in stage '{_stageName}' -> {ex.Message}.";
            }
        }
    }
}
