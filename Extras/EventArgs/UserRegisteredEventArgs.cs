using LaVie.Models;

namespace LaVie.Extras.EventArgs;

public class UserRegisteredEventArgs : System.EventArgs
{
    public UserRegisteredEventArgs(User user)
    {
        User = user;
    }

    public User User { get; }
}
