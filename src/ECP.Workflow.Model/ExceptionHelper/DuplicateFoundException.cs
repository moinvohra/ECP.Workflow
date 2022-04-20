﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ECP.Workflow.Model
{
    /// <summary>
    /// DuplicateFoundException, ex:-
    /// Workflow duplicates
    /// Status Code-409 (Conflict)
    /// </summary>
    public class DuplicateFoundException : Exception
    {
        /// <summary>
        /// fieldName
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// statusCode
        /// </summary>
        public string FieldValue { get; set; }

        /// <summary>
        /// fieldName
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// StatusCode
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// DuplicateFoundException
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public DuplicateFoundException(string fieldName, string fieldValue, string errorCode, string errorMessage)
            : base($"{fieldValue} {fieldName} already exists")
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// DuplicateFoundException
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        public DuplicateFoundException(string fieldName, string fieldValue, string errorCode, HttpStatusCode statusCode)
            : base($"{fieldValue} {fieldName} already exists")
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.ErrorCode = errorCode;
            this.StatusCode = statusCode;
        }
    }
}
