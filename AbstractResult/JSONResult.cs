//
//  JSONResult.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.Runtime.Serialization;


namespace DotNetExamples
{

    /// <summary>
    /// Simple Type to return a JSON Wrapped Response
    /// </summary>
    [DataContract]
    public class JSONResult : MyResult
    {
        private WebOperationContext _context;

        public JSONResult(WebOperationContext context)
        {
            _context = context;
        }

        public override void SetHeaders()
        {
            _context.OutgoingResponse.ContentType = "application/json";
            if (ResultStatus == GlobalConstants.StatusCode.ERROR)
            {
                _context.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            else
            {
                _context.OutgoingResponse.StatusCode = System.Net.HttpStatusCode.OK;
            }
        }
    }
}