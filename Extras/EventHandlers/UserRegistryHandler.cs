using LaVie.Extras.EventArgs;

namespace LaVie.Extras.EventHandlers;

public class UserRegistryHandler
{
    public void OnUserRegistered(object? sender, UserRegisteredEventArgs args)
    {
        Console.WriteLine(
            $"[UserRegistryListener] Welcome {args.User.UserName}! Your email is {args.User.Email}"
        );
    }
}
