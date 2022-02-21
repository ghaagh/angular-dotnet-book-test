namespace Book.Application.Controllers.Dto
{
    public class HistoryResponse
    {
        public int Id { get; set; }
        public DateTime LogDate { get; private set; }
        public string? OldValue { get; protected set; }
        public string CurrentValue { get; protected set; }
        public string Field { get; private set; }
        public string Description { get; set; }
        public int BookId { get; set; }
    }
}
