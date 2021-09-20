using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFEnvironmentStore : EFEntityStoreBase<Core.Environment>, IEnvironmentStore
    {
        public EFEnvironmentStore(SignalBoxDbContext context) : base(context, c => c.Environments)
        {
        }


    }
}