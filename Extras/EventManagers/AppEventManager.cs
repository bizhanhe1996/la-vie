using LaVie.Extras.EventArgs;
using LaVie.Models;

namespace LaVie.Extras.EventManagers;

public class AppEventManager
{
    public event EventHandler<UserRegisteredEventArgs>? UserRegistered;

    public void RaiseUserRegistered(User user)
    {
        UserRegistered?.Invoke(this, new UserRegisteredEventArgs(user));
    }
}
