using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Asyncronaut.Pipeline
{
    public class Pipeline
    {
        private readonly string _pipelineName;
        private readonly List<PipelineStage> _stages;

        public Pipeline(string pipelineName)
        {
            _pipelineName = pipelineName;
            _stages = new List<PipelineStage>();
        }

        public void AddStage(string stageName, Action<PipelineStageBuilder> stageBuilder)
        {
            var stage = new PipelineStage(stageName);
            stageBuilder(new PipelineStageBuilder(stage));
            _stages.Add(stage);
        }

        public async Task RunAsync()
        {
            foreach(var stage in _stages)
            {
                await stage.RunAsync();

                if(stage.HasError)
                {
                    throw new PipelineException(_pipelineName, stage.ErrorMessage);
                }
            }
        }
    }
}
