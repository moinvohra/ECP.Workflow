namespace ECP.Workflow.Common
{
    using ECP.Workflow.Model.Utility;
    using ECP.Workflow.Service.Model.Utility;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net;


    [Serializable]
    public class BaseResponse<TEntity> : BaseResponse
    {
        new public TEntity ResponseData { get; set; }
    }

    [Serializable]
    public class BaseResponse
    {
        public BaseResponse()
        {
            Error = new List<Error>();
            ResponseCode = (int)HttpStatusCode.OK;
            ResponseData =null;
            ResponseMessage = General.Success;
        }


        /// <summary>
        /// Error code of result
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// List of error messages associated with the request.
        /// </summary>


        [JsonProperty("errors")]
        public List<Error> Error { get; set; }

        /// <summary>
        /// Response code of result
        /// </summary>
        public int ResponseCode { get; set; }

        /// <summary>
        /// Response message of result
        /// </summary>
        public string ResponseMessage { get; set; }

        /// <summary>
        /// List of response data
        /// </summary>
        public dynamic ResponseData { get; set; }

        public long TotalRecords { get; set; } = 1;
    }


}
