using famiLYNX3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace famiLYNX3.Infrastructure {
    public class Repository : IRepository {

        private ApplicationDbContext _db;

        public Repository(ApplicationDbContext db) {
            _db = db;
        }

        public IQueryable<T> Query<T>() where T : class {
            return _db.Set<T>().AsQueryable();
        }

        public T Find<T>(params object[] keyValues) where T : class {
            return _db.Set<T>().Find(keyValues);
        }

        public void Add<T>(T entityToCreate) where T : class {
            _db.Set<T>().Add(entityToCreate);
        }

        public void Delete<T>(params object[] keyValues) where T : class {
            _db.Set<T>().Remove(Find<T>(keyValues));
        }

        public void SaveChanges() {
            try {
                _db.SaveChanges();
            }
            catch (DbEntityValidationException error) {
                var firstError = error.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
                throw new ValidationException(firstError);
            }
        }

        public void Dispose() {
            _db.Dispose();
        }

    }

    public static class GenericRepositoryExtensions {
        public static IQueryable<T> Include<T, TProperty>(this IQueryable<T> queryable, Expression<Func<T, TProperty>> relatedEntity) where T : class {
            return System.Data.Entity.QueryableExtensions.Include<T, TProperty>(queryable, relatedEntity);
        }
    }
}