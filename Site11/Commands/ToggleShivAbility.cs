using CommandSystem;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using System;

namespace Site11Cursed.Commands
{

    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class DisableShiv : ICommand
    {
        public string Command => "ShivAbility";
        public string[] Aliases => new string[] { "sva" };
        public string Description => "Disables or Enables the shiv ability for every player.";

        public static class PublicStatic
        {
            public static bool shivdisabled = false;
        }

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count < 1)
            {
                response = "Usage: ShivAbility [enable / disable]";
                return false;
            }

            string action = arguments.At(0).ToLower();

            switch (action)
            {
                case "disable":
                    PublicStatic.shivdisabled = true;
                    
                    response = "Shiv ability disabled for all players.";
                    Log.Debug("Shiv ability disabled via command.");
                    return true;

                case "enable":
                    PublicStatic.shivdisabled = false;
                    Log.Debug("Shiv ability enabled via command.");

                    response = "Shiv ability enabled for all players.";
                    return true;

                default:
                    response = "Invalid argument use: ShivAbility [Enable / Disable].";
                    return false;
            }
        }
    }
}
