using System;
using System.Linq;

namespace famiLYNX3.Infrastructure {
    public interface IRepository : IDisposable{
        void Add<T>(T entityToCreate) where T : class;
        void Delete<T>(params object[] keyValues) where T : class;
        T Find<T>(params object[] keyValues) where T : class;
        IQueryable<T> Query<T>() where T : class;
        void SaveChanges();
    }
}