using ECP.Workflow.Model.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ECP.Workflow.Service.Validation
{
    public static class WorkflowDefinitionParser
    {
        public const string FINISH = "finish";
        public const string ACTION = "action";
        public const string CONDITION = "condition";
        public const int END_STEPIDENTITY = 0; 
        public static List<WorkflowStep> Parse(string jsonData)
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<List<WorkflowStep>>(new JsonTextReader(new StringReader(jsonData)));
        }

        public static bool IsValidFieldType(dynamic FieldType)
        {
            List<WorkflowStep> workflowDefinition = Parse(JsonConvert.SerializeObject(FieldType));

            if (workflowDefinition.Count >= 1)
                return true;
            return false;
        }

        public static IEnumerable<Error> Validate(dynamic jsonData)
        {
           List<WorkflowStep> workflowStep = Parse(JsonConvert.SerializeObject(jsonData));

            List<Error> errors = new List<Error>();

            int arrayIndex = 1;

            //Prepare unique step labels (Reason: step label cannot  duplicate, will be used in preparing error message)
            var visitedStepLabels = new List<string>();

            //Prepare unique step Identity  (Reason: step Identity cannot duplicate, will be used in preparing error message)
            var visitedStepIdentity = new List<long>();
            var allStepIdentity = new List<long>();
            allStepIdentity.AddRange(workflowStep.Select(s => s.StepIdentity).Distinct());

            // Check for Action Step
            if (!workflowStep.Any(x => x.StepType.Equals(ACTION)))
            {
                errors.Add(new Error() { ErrorCode = "ACTION_MISSING", ErrorMessage = $"Minimum 1 Action step is required", RecordIndex = arrayIndex });
                return errors;
            }

            // Check for end step maping
            if (!(workflowStep.Any(x => x.StepTransitions.WhenValid == END_STEPIDENTITY) ||
                workflowStep.Any(x => x.StepTransitions.WhenInvalid == END_STEPIDENTITY)))
            {
                errors.Add(new Error() { ErrorCode = "MISSING_END", ErrorMessage = $"'End' step is not mapped with any valid/invalid transition", RecordIndex = arrayIndex });
                return errors;
            }

            foreach (var item in workflowStep)
            {
                
                if (!item.StepType.Equals(FINISH, StringComparison.OrdinalIgnoreCase))
                {
                    //Check Step Identity valid transition refers to same step
                    if (item.StepIdentity == item.StepTransitions.WhenValid)
                    {
                        errors.Add(new Error() { ErrorCode = "LOOP_DETECTED", ErrorMessage = $"'{item.StepLabel}' Valid transition refers to same step", RecordIndex = arrayIndex });
                    }

                    //Step label must be unique:
                    if (visitedStepLabels.Any(label => label.Equals(item.StepLabel, StringComparison.OrdinalIgnoreCase)))
                    {
                        errors.Add(new Error() { ErrorCode = "DUPLICATE_STEP_LABEL", ErrorMessage = $"Step Label '{item.StepLabel}' is duplicate", RecordIndex = arrayIndex });
                    }                  

                    if (item.StepType.Equals(CONDITION, StringComparison.OrdinalIgnoreCase))
                    {
                        //Check Step Identity Invalid transition refers to same step
                        if (item.StepIdentity == item.StepTransitions.WhenInvalid)
                        {
                            errors.Add(new Error() { ErrorCode = "LOOP_DETECTED", ErrorMessage = $"'{item.StepLabel}' Invalid transition refers to same step", RecordIndex = arrayIndex });
                        }

                        //Check if Invalid transition has corresponding reference present in step identity:
                        if (!allStepIdentity.Any(stepId => stepId.Equals(item.StepTransitions.WhenInvalid)))
                        {
                            errors.Add(new Error() { ErrorCode = "STEPIDENTITY_REFERENCE_DOES_NOT_EXIST", ErrorMessage = $"Step label '{item.StepLabel}' does not have invalid transition", RecordIndex = arrayIndex });
                        }

                        ////Check if Valid transition has corresponding reference present in step identity:
                        if (!allStepIdentity.Any(stepId => stepId.Equals(item.StepTransitions.WhenValid)))
                        {
                            errors.Add(new Error() { ErrorCode = "STEPIDENTITY_REFERENCE_DOES_NOT_EXIST", ErrorMessage = $"Step label '{item.StepLabel}' does not have valid transition", RecordIndex = arrayIndex });
                        }
                    }
                }

                visitedStepLabels.Add(item.StepLabel);
                visitedStepIdentity.Add(item.StepIdentity);

                arrayIndex++;
            }

            return errors;
        }
    }
}