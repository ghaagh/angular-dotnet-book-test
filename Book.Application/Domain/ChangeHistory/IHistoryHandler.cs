using Book.Application.Infrastructure;

namespace Book.Application.Domain.ChangeHistory;

public interface IHistoryHandler
{

    public void OnChangeExtracted(object sender, OnChangedEventArgument e);

}

