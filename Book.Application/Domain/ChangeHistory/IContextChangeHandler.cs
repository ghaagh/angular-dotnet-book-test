using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Book.Application.Domain.ChangeHistory;
public interface IContextChangeHandler
{
    event EventHandler<OnChangedEventArgument> HistoryChanged;
    void OnSaveFinished(object sender, EntityStateChangedEventArgs ev);
    void OnSaving(object sender, SavingChangesEventArgs ev);
}

