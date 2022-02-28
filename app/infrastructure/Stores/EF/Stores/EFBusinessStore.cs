using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalBox.Core;

namespace SignalBox.Infrastructure.EntityFramework
{
    public class EFBusinessStore : EFCommonEntityStoreBase<Business>, IBusinessStore
    {
        protected override bool IsEnvironmentScoped => true;
        
        public EFBusinessStore(IDbContextProvider<SignalBoxDbContext> contextProvider, IEnvironmentProvider environmentProvider)
        : base(contextProvider, environmentProvider, (c) => c.Businesses)
        { }

    }
}