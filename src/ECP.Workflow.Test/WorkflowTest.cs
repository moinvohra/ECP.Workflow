using ECP.Workflow.Api.Controllers;
using ECP.Workflow.Common;
using ECP.Workflow.Model;
using ECP.Workflow.Repository.WorkflowRepository;
using ECP.Workflow.Service;
using ECP.Workflow.Test.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Net;
using Xunit;
using static ECP.Workflow.Model.Utility.General;

namespace ECP.Workflow.Test
{
    public class WorkflowTest
    {
        //private readonly Mock<IWorkflowRepository> _service;
        //public WorkflowTest()
        //{
        //    _service = new Mock<IWorkflowRepository>();
        //}
        [Fact]
        public async void Add_New_Workflow()
        {
            var workflow = WorkflowData.NewWorkflowRequest();

            //arrange
            var workflowService = new Mock<IWorkflowService>();

            var sut = new WorkflowsController(workflowService.Object,null);
            
            //act
            var result = await sut.Post(workflow,"","").ConfigureAwait(true);


            //Assert
            workflowService.Verify(_ => _.Create(workflow, "tenantId", "applicationId"));
            Assert.IsType<BaseResponse<WorkflowDetail>>(result);
        }


        [Fact]
        public async void Edit_A_Workflow()
        {
            var workflow = WorkflowData.EditWorkflowRequest();

            //arrange
            var workflowService = new Mock<IWorkflowService>();

            var sut = new WorkflowsController(workflowService.Object, null);

            //act
            var result = await sut.Put(workflow, "", "").ConfigureAwait(true);


            //Assert
            workflowService.Verify(_ => _.Update(workflow, "tenantId", "applicationId"));
            Assert.IsType<BaseResponse<WorkflowDetail>>(result);
        }


        [Fact]
        public async void Delete_Workflow()
        {
            var workflow = WorkflowData.NewWorkflowRequest();

            //arrange
            var workflowService = new Mock<IWorkflowService>();

            var sut = new WorkflowsController(workflowService.Object, null);

            //act
            var result = await sut.Delete("", "", 4).ConfigureAwait(true);

            //Assert
            workflowService.Verify(_ => _.Delete("tenantId", "applicationId",4));
            Assert.IsType<NoContentResult>(result);
        }
    }
}
