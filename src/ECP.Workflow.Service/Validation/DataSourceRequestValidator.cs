using ECP.KendoGridFilter;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECP.Workflow.Service.Validation
{
    public class DataSourceRequestValidator : AbstractValidator<DataSourceRequest>
    {
        public DataSourceRequestValidator()
        {
            RuleFor(x => x.Take).LessThanOrEqualTo(50);
        }
    }
}
