using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SignalBox.Core;

namespace SignalBox.Infrastructure
{
    public class InMemoryExperimentStore : InMemoryStore<Experiment>, IExperimentStore
    {
        // private Dictionary<string, IExperiment> store = new Dictionary<string, IExperiment>();
    }
}