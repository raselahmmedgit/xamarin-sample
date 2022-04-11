using System.ComponentModel;

namespace Covi.Features.Chat.Components
{
    public enum MessageUserEnum
    {
        [Description("UserHealthBuddy")]
        UserHealthBuddy = 1,
        [Description("UserLogin")]
        UserLogin = 2,
        [Description("UserAnonymous")]
        UserAnonymous = 3
    }
}
