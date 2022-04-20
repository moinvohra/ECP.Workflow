using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Service.Validation
{

    public partial class WorkflowStep
    {
        [JsonProperty("StepCode")]
        public string StepCode { get; set; }

        [JsonProperty("StepType")]
        public string StepType { get; set; }

        [JsonProperty("StepLabel")]
        public string StepLabel { get; set; }

        [JsonProperty("StepIdentity")]
        public long StepIdentity { get; set; }

        [JsonProperty("StepTransitions")]
        public StepTransitions StepTransitions { get; set; }

        [JsonProperty("actionParameters")]
        public object ActionParameters { get; set; }
    }

    public partial class StepTransitions
    {
        [JsonProperty("WhenValid")]
        public long? WhenValid { get; set; }

        [JsonProperty("WhenInvalid")]
        public long? WhenInvalid { get; set; }
    }
}
