using HI.SharedKernel.Models;
using Microsoft.EntityFrameworkCore;

namespace HI.Api.Services
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<HistoryData> Histories { get; set; }
    }
}