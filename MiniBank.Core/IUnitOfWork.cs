using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBank.Core
{
        public interface IUnitOfWork
        {
            int SaveChanges();
            Task<int> SaveChangesAsync(); 
        }
}
