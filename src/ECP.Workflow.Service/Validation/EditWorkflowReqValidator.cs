namespace ECP.Workflow.Service.Validation
{
    using ECP.Workflow.Model;
    using FluentValidation;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    public class EditWorkflowReqValidator : AbstractValidator<EditWorkflowReq>
    {
        public EditWorkflowReqValidator()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Name).MinimumLength(3);
            RuleFor(x => x.Name).MaximumLength(50);
            RuleFor(x => x.Code).NotNull();
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Code).MinimumLength(3);
            RuleFor(x => x.Code).MaximumLength(50);
            RuleFor(x => x.Code).Matches("^[a-zA-Z0-9_.]*$");
            RuleFor(x => x.DefinitionJson).Must(WorkflowDefinitionParser.IsValidFieldType).WithMessage("Please enter definition");
        }
    }
}
