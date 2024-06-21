using System;
using System.Threading.Tasks;

namespace Asyncronaut.Pipeline
{
    public class PipelineStageBuilder
    {
        private readonly PipelineStage _stage;

        public PipelineStageBuilder(PipelineStage stage)
        {
            _stage = stage;
        }

        public PipelineStageBuilder SetExecutionMode(ExecutionMode mode)
        {
            _stage.SetExecutionMode(mode);
            return this;
        }

        public PipelineStageBuilder AddTask(Func<Task> task)
        {
            _stage.AddTask(task);
            return this;
        }

        public PipelineStageBuilder AddTask<TInput>(Func<TInput, Task> task, TInput input)
        {
            _stage.AddTask(task, input);
            return this;
        }

        public PipelineStageBuilder AddTask<TInput>(Func<Task<TInput>, Task> task, Task<TInput> input)
        {
            _stage.AddTask(task, input);
            return this;
        }

        public PipelineStageBuilder AddTask<TOutput>(Func<Task<TOutput>> task, out Task<TOutput> output)
        {
            _stage.AddTask(task, out output);
            return this;
        }

        public PipelineStageBuilder AddTask<TInput, TOutput>(Func<TInput, Task<TOutput>> task, TInput input, out Task<TOutput> output)
        {
            _stage.AddTask(task, input, out output);
            return this;
        }

        public PipelineStageBuilder AddTask<TInput, TOutput>(Func<Task<TInput>, Task<TOutput>> task, Task<TInput> input, out Task<TOutput> output)
        {
            _stage.AddTask(task, input, out output);
            return this;
        }
    }
}
