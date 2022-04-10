using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniBank.Core;

namespace MiniBank.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MiniBankContext _context;

        public UnitOfWork(MiniBankContext context)
        {
            _context = context;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
