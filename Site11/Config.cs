using Exiled.API.Enums;
using Exiled.API.Interfaces;
using Site11Cursed;
using Site11Cursed.CustomItems;
using System.ComponentModel;

namespace Site11Cursed.Configs
{
    public class Config : IConfig
    {

        [Description("Whether the plugin is enabled")]
        public bool IsEnabled { get; set; } = true;

        [Description("Whether Debug Mode is enabled")]
        public bool Debug { get; set; } = false;


        [Description("Sets the cooldown for the shiv ability")]
        public float ShivCooldown { get; set; } = 300f;

        [Description("The description of the custom item Taser")]
        public string TaserDescription { get; set; } = "";

        [Description("The description of the custom item Smoke Grenade")]
        public string SmokeGrenadeDescription { get; set; } = "";

        [Description("The duration of the smoke")]
        public float SmokeGrenadeDuration { get; set; } = 15f;
    }
}