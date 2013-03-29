//
//  MyResult.cs
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



// Abstract class that can be used with WCF to keep a standardized result for all Methods


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel.Web;
using System.Reflection;
using System.Runtime.Serialization;

namespace DotNetExamples
{
    /// <summary>
    /// Encapsulated Base type for all Web Service Responses
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
    [KnownType("GetKnownTypes")]
    public abstract class MyResult
    {

        public MyResult()
        {

        }

        private static bool IsSubClassOfMyParameter(Type TypeToCheck)
        {
            while (TypeToCheck != typeof(object))
            {
                if (TypeToCheck.BaseType == null)
                    break;
                if (TypeToCheck.BaseType == typeof(MyResult))
                {
                    return true;
                }
                TypeToCheck = TypeToCheck.BaseType;
            }
            return false;
        }

        public static Type[] GetKnownTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(IsSubClassOfMyParameter).ToArray();
        }

        private string _version;

        /// <summary>
        /// Allows for extra headers to be set
        /// </summary>
        public abstract void SetHeaders();

        /// <summary>
        /// API version
        /// </summary>
        [DataMember]
        public string Version {
            get {
                return "1.0";
            }
            private set {_version = value;}
        }

        /// <summary>
        /// Status "Code"
        /// </summary>
        [DataMember(Name = "Status")]
        public GlobalConstants.StatusCode ResultStatus { get; set; }

        /// <summary>
        ///  Status Message, e.g. Error Message, Row Count, Warnings, etc.
        /// </summary>
        [DataMember(Name="Message")]
        public virtual string ResultMessage { get; set; }

        /// <summary>
        /// Block of Result Data of type T
        /// </summary>
        [DataMember(Name="Data")]
        public virtual object ResultData { get; set; }
    }
}