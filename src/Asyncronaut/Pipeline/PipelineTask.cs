using System;
using System.Threading.Tasks;

namespace Asyncronaut.Pipeline
{
    public class PipelineTask
    {
        protected readonly Func<Task> _task;

        public PipelineTask(Func<Task> task)
        {
            _task = task;
        }

        public async Task RunAsync()
        {
            await _task();
        }
    }
}
