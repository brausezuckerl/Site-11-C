using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.CustomRoles;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using PlayerRoles.Subroutines;
using Site11Cursed.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UserSettings.ServerSpecific;
using static Site11Cursed.Commands.DisableShiv;

namespace Site11Cursed.EventHandler
{
    internal class PlayerEventHandler
    {
        private readonly Dictionary<string, float> cooldowns = new();

        public float ShivAbilityCooldown { get; set; } = global::Site11Cursed.Plugin.Instance.Config.ShivCooldown;


        public void OnSettingValueReceived(ReferenceHub hub, ServerSpecificSettingBase settingBase)
        {
            if (DisableShiv.PublicStatic.shivdisabled)
                return;

            if (settingBase is not SSKeybindSetting keybindSetting)
                return;

            if (keybindSetting.SettingId != 5500 || !keybindSetting.SyncIsPressed)
                return;

            var player = Player.Get(hub);

            if (cooldowns.TryGetValue(player.UserId, out float nextAvailable) && Time.realtimeSinceStartup < nextAvailable)
            {
                float remaining = Mathf.Ceil(nextAvailable - Time.realtimeSinceStartup);
                player.ShowHint($"<color=red>Shiv is on cooldown for {remaining} seconds!</color>", 3);
                Log.Debug($"[PlayerEventHandler] {player.Nickname} used shiv on cooldown {remaining} seconds remaining.");
                return;
            }
            player.PlaceTantrum(false);
            Log.Debug($"[PlayerEventHandler] Shiv used from {player.Nickname}");

            cooldowns[player.UserId] = Time.realtimeSinceStartup + ShivAbilityCooldown;
        }
    }
}
