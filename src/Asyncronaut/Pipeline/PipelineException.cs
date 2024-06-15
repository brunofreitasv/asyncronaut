using System;

namespace Asyncronaut.Pipeline
{
    [Serializable]
    internal class PipelineException : Exception
    {
        public PipelineException(string pipelineName, string errorMessage)
            : base($"{pipelineName} - {errorMessage}")
        {
        }
    }
}
