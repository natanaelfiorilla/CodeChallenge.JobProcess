using Microsoft.EntityFrameworkCore;
using Ophen.JobProcess.Domain.Entities;

namespace Ophen.JobProcess.Infraestructure
{
    public class JobProcessContext : DbContext
    {
        public JobProcessContext(DbContextOptions<JobProcessContext> options)
           : base(options)
        { }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobItem> JobItems { get; set; }
    }
}
