using System.Threading.Tasks;

namespace Ophen.JobProcess.Infraestructure.Repositories
{
    public abstract class BaseRep
    {
        protected readonly JobProcessContext _context;
        public BaseRep(JobProcessContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
