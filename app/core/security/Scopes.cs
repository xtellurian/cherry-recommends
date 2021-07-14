namespace SignalBox.Core.Security
{
    public static class Scopes
    {
        private static string TryAddScope(this string currentScope, string newScope)
        {
            if (!currentScope.Contains(newScope))
            {
                return $"{currentScope} {newScope}";
            }
            else
            {
                return currentScope;
            }
        }

        public static string AllScopes(string scope)
        {
            return scope.TryAddScope(Features.Read).TryAddScope(Features.Write);
        }
        public static ReadWriteScope Features => new ReadWriteScope("features");
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