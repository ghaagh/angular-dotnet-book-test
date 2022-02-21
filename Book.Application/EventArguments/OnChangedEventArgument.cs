namespace Book.Application.Domain
{
    public class OnChangedEventArgument : EventArgs
    {
        public List<Change> Changes { get; set; } = new List<Change>();
    }
    public class Change
    {
        public string Field { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
        public int Id { get; set; }
    }
}

