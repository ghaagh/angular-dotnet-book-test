namespace Book.Application.Domain
{
    public class History : IAggregateRoot
    {
        public History(HistoryType type, string table, string field, string description = "")
        {
            LogDate = DateTime.Now;
            Type = type;
            Table = table;
            Field = field;
            if (string.IsNullOrEmpty(description))
                Description = $"Field: {field} of the Table: {table} has been {type}";
            else
                Description = description;
        }
        private History() { }
        public int Id { get; set; }
        public DateTime LogDate { get; private set; }
        public HistoryType Type { get; private set; }
        public string Table { get; private set; }
        public string Field { get; private set; }
        public string Description { get; set; }

    }
    public enum HistoryType
    {
        Unspecified = 0,
        Added,
        Changed,
        Removed
    }
}
