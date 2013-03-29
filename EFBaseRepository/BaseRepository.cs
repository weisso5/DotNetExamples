//
//  BaseRepository.cs
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
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace DotNetExamples.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class, IEntityKey
    {
        private DbSet<T> _dbset;
        private DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = dbContext.Set<T>();
        }

        #region IBaseRepository<T> Members

        /// <summary>
        /// Add instance of T to DbContext
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(T entity)
        {
            _dbset.Add(entity);
        }

        /// <summary>
        /// Remove instance of T from DbContext
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        /// <summary>
        /// Generic Edit T, Modify State
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Edit(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Generic LINQ style search method, e.g (obj => obj.property == instancevar)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public virtual IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate);
        }

        /// <summary>
        /// Generic LINQ style search method, e.g (obj => obj.property == instancevar) with Includes for Loading
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate, params string[] includes)
        {
            IQueryable<T> query = this._dbset;
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }
            return query.Where(predicate);
        }

        /// <summary>
        /// Basically a Select * from T, use with caution
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return _dbset;
        }

        /// <summary>
        /// Get T by Primary Key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(int id)
        {
            return _dbset.Find(id);
        }

        /// <summary>
        /// Get T by Primary Key, with includes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public virtual T GetById(int id, params string[] includes)
        {
            IQueryable<T> query = this._dbset;
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }

            return query.SingleOrDefault(k => k.PrimaryKey == id );
        }

        /// <summary>
        /// Proxy to underlying <see cref="DbContext.SaveChanges()"/>
        /// </summary>
        public virtual void SaveChanges()
        {
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var error = e.EntityValidationErrors.First();
                if (error != null)
                {
                    throw new Exception("Failed to Save => " + error.ValidationErrors.First().ErrorMessage);
                }
            }
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
            return;
        }

        protected virtual void Dispose(bool full)
        {
            _dbset = null;
            _dbContext.Dispose();
        }
        #endregion
    }
}
