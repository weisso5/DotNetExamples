//
//  IBaseRepository.cs
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
using System.Linq;
using System.Linq.Expressions;

namespace DotNetExamples.Repository
{
    public interface IBaseRepository<T> : IDisposable
    {
        /// <summary>
        /// Add instance of T to DbContext
        /// </summary>
        /// <param name="entity"></param>
        void Insert(T entity);

        /// <summary>
        /// Remove instance of T from DbContext
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);

        /// <summary>
        /// Generic Edit T, Modify State
        /// </summary>
        /// <param name="entity"></param>
        void Edit(T entity);

        /// <summary>
        /// Generic LINQ style search method, e.g (obj => obj.property == instancevar)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Generic LINQ style search method, e.g (obj => obj.property == instancevar) with Includes for Loading
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate, params string[] includes);

        /// <summary>
        /// Basically a Select * from T, use with caution
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Get T by Primary Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(int id);

        /// <summary>
        /// Get T by Primary Key, with includes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        T GetById(int id, params string[] includes);

        /// <summary>
        /// Proxy to underlying <see cref="DbContext.SaveChanges()"/>
        /// </summary>
        void SaveChanges();
    }
}
