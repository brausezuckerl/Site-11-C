using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Core.UserSettings;
using Exiled.API.Features.Doors;
using Exiled.CustomItems.API.Features;
using Exiled.CustomRoles;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Site11Cursed.Commands;
using Site11Cursed.Configs;
using Site11Cursed.EventHandler;
using System;
using System.Collections.Generic;
using UnityEngine;
using UserSettings.ServerSpecific;
using Config = Site11Cursed.Configs.Config;

namespace Site11Cursed;

public class Plugin : Plugin<Config>
{

    public override string Name => "Site-11C";

    public override string Author => "Cursed";

    public static Plugin Instance;

    private DisableShiv DisableShivEvent;

    private PlayerEventHandler playereventhandler;

    public override void OnEnabled()
    {
        Instance = this;

        HeaderSetting header = new HeaderSetting(5555, "Site-11C");
        IEnumerable<SettingBase> settingBases = new SettingBase[]
        { header, new KeybindSetting(5500, "Shiv", KeyCode.RightAlt, hintDescription: "Take a shiv"), };
        SettingBase.Register(settingBases);
        SettingBase.SendToAll();

        DisableShivEvent = new DisableShiv();
        playereventhandler= new PlayerEventHandler();

        ServerSpecificSettingsSync.ServerOnSettingValueReceived += playereventhandler.OnSettingValueReceived;

        Exiled.CustomItems.API.Features.CustomItem.RegisterItems();
        Exiled.CustomRoles.API.Features.CustomRole.RegisterRoles(false, null);
        Exiled.CustomRoles.API.Features.CustomAbility.RegisterAbilities();

        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        Instance = null;

        ServerSpecificSettingsSync.ServerOnSettingValueReceived -= playereventhandler.OnSettingValueReceived;

        Exiled.CustomItems.API.Features.CustomItem.UnregisterItems();
        Exiled.CustomRoles.API.Features.CustomRole.UnregisterRoles();

        base.OnDisabled();
    }
}