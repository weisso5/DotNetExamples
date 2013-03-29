//
//  OperationStatus.cs
//
//
//  Created by Michael Weissman on 3/27/13.
//  Copyright (c) 2013 Michael Weissman. All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


// The idea behind operation status is to provide a meaningful and standardized way of returning from Methods
// In essence forcing the caller to think about the results.
// In large projects, especially, this helps out debugging and as well as headaches.  Returning Null, -1, etc... doesn't really provide a way for
// the developer to figure out what is going on other than the method is not working as expected.

using System;
using System.Diagnostics;

namespace DotNetExamples.Model
{

    /// <summary>
    /// Standardized return value for use with Repository Methods
    /// </summary>
    [DebuggerDisplay("Status : {Status}", Name = "{OperationID}")]
    public sealed class OperationStatus
    {
        //prevent automatic generation of constructor, use static members only
        private OperationStatus(){}

        /// <summary>
        /// GOOD or BAD, check this first
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Metadata for related methods
        /// </summary>
        public int RecordsAffected { get; set; }

        /// <summary>
        /// Message associated with Status
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Unique ID for Operation
        /// </summary>
        public Object OperationID { get; set; }

        /// <summary>
        /// System.Exception.Message
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// System.Exception.StackTrace
        /// </summary>
        public string ExceptionStackTrace { get; set; }

        /// <summary>
        /// System.Exception.InnerException.Message, can be NULL
        /// </summary>
        public string ExceptionInnerMessage { get; set; }

        /// <summary>
        /// System.Exception.InnerException.StackTrace, can be NULL
        /// </summary>
        public string ExceptionInnerStackTrace { get; set; }


        /// <summary>
        /// Helper method for generating a Bad Status with Exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static OperationStatus CreateFromException(string message, Exception ex)
        {
            Guid g = Guid.NewGuid();

            OperationStatus opStatus = new OperationStatus
            {
                Status = false,
                Message = message,
                OperationID = g
            };

            if (ex != null)
            {
                opStatus.ExceptionMessage = ex.Message;
                opStatus.ExceptionStackTrace = ex.StackTrace;
                opStatus.ExceptionInnerMessage = (ex.InnerException == null) ? null : ex.InnerException.Message;
                opStatus.ExceptionInnerStackTrace = (ex.InnerException == null) ? null : ex.InnerException.StackTrace;
            }
            return opStatus;
        }

        /// <summary>
        /// Helper method for generating a Bad Status with message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static OperationStatus CreateFromFailure(string message)
        {
            Guid g = Guid.NewGuid();

            OperationStatus opStatus = new OperationStatus
            {
                Status = false,
                Message = message,
                OperationID = g
            };
            return opStatus;
        }

        /// <summary>
        /// Helper Method for Success Status
        /// </summary>
        /// <param name="message">string</param>
        /// <returns>OperationStatus</returns>
        public static OperationStatus CreateFromSuccess(string message)
        {
            Guid g = Guid.NewGuid();

            OperationStatus opStatus = new OperationStatus
            {
                Status = true,
                Message = message,
                OperationID = g
            };
            return opStatus;
        }

        /// <summary>
        /// Helper Method for Success Status with ID
        /// </summary>
        /// <param name="ID">OperationID</param>
        /// <param name="message">string</param>
        /// <returns>OperationStatus</returns>
        public static OperationStatus CreateFromSuccess(object ID, string message)
        {

            OperationStatus opStatus = new OperationStatus
            {
                Status = true,
                Message = message,
                OperationID = ID
            };

            return opStatus;
        }

        /// <summary>
        /// Helper Method for Success Status with ID, Message, and records affected
        /// </summary>
        /// <param name="ID">OperationID</param>
        /// <param name="message">string</param>
        /// <param name="recordsaffected">int</param>
        /// <returns>OperationStatus</returns>
        public static OperationStatus CreateFromSuccess(object ID, string message, int recordsaffected)
        {
            OperationStatus opStatus = new OperationStatus
            {
                Status = true,
                Message = message,
                RecordsAffected = recordsaffected,
                OperationID = ID
            };
            return opStatus;
        }
    }
}
