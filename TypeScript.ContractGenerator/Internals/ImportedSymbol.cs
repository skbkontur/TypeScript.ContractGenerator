namespace SkbKontur.TypeScript.ContractGenerator.Internals
{
    internal class ImportedSymbol
    {
        public ImportedSymbol(string sourceName, string localName, string path)
        {
            SourceName = sourceName;
            LocalName = localName;
            Path = path;
        }

        public string SourceName { get; }
        public string LocalName { get; }
        public string Path { get; }

        private bool Equals(ImportedSymbol other)
        {
            return string.Equals(SourceName, other.SourceName) && string.Equals(LocalName, other.LocalName) && string.Equals(Path, other.Path);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ImportedSymbol)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = SourceName != null ? SourceName.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (LocalName != null ? LocalName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Path != null ? Path.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}