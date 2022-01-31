namespace SignalBox.Core.Security
{
    public static class Scopes
    {
        private static string TryAddScope(this string currentScope, string newScope)
        {
            if (newScope == null)
            {
                return currentScope;
            }
            if (!currentScope.Contains(newScope))
            {
                return $"{currentScope} {newScope}";
            }
            else
            {
                return currentScope;
            }
        }

        public static string AllScopes(string scope, Tenant tenant)
        {
            return scope.TryAddScope(Metrics.Read).TryAddScope(Metrics.Write).TryAddScope(tenant?.AccessScope());
        }
        // warning - DO NOT CHANGE THESE.
        // see azure/Auth0.cs
        public static ReadWriteScope Metrics => new ReadWriteScope("metrics"); 
    }

    public class ReadWriteScope
    {
        public ReadWriteScope(string name)
        {
            this.Name = name;
        }
        private string Name { get; }
        public string Read => $"read:{Name}";
        public string Write => $"write:{Name}";
    }
}