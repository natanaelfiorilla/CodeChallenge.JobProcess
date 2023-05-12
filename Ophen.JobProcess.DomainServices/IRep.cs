using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ophen.JobProcess.DomainServices
{
    public interface IRep<T> where T : class
    {
        Task<T> GetById(int id);

        IEnumerable<T> GetAll();

        bool Exists(int id);

        int Count();

        void Add(T newObject);

        void Update(T newObject);

        public int SaveChanges();

        public Task<int> SaveChangesAsync();
    }
}
