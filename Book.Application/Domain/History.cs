namespace Book.Application.Domain
{
    public abstract class History : IAggregateRoot
    {
        public History(string field, object oldValue, object currentValue)
        {
            LogDate = DateTime.Now;
            OldValue = oldValue?.ToString();
            CurrentValue = currentValue?.ToString();
            Field = field;
        }
        protected History() { }
        public int Id { get; set; }
        public DateTime LogDate { get; private set; }
        public string? OldValue { get; protected set; }
        public string CurrentValue { get; protected set; }
        public string Field { get; private set; }
        public string Description { get; set; }

    }
    public enum HistoryType
    {
        Field,
        Table
    }
}
