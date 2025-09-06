using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using Exiled.Events.Handlers;
using MEC;
using PlayerRoles;
using Site11Cursed;
using Site11Cursed.Configs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Scp244 = Exiled.API.Features.Items.Scp244;

namespace Site11Cursed.CustomItems;

[CustomItem(ItemType.GunCOM18)]

public class SmokeGrenade : CustomGrenade
{

    public override ItemType Type { get; set; } = ItemType.GrenadeFlash;

    public override uint Id { get; set; } = 2u;

    public override string Name { get; set; } = "Smoke Grenade";

    public override string Description { get; set; } = "";

    public override float Weight { get; set; } = 1f;

    public override float FuseTime { get; set; } = 4f;

    public override bool ExplodeOnCollision { get; set; } = false;

    public override SpawnProperties SpawnProperties { get; set; } = new SpawnProperties
    {

        DynamicSpawnPoints = new List<DynamicSpawnPoint>
        {
            new DynamicSpawnPoint
            {
                Location = SpawnLocationType.InsideHczArmory,
                Chance = 0f
            }
        }
    };

    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        Exiled.Events.Handlers.Map.ExplodingGrenade += OnGrenadeExplosion;
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Map.ExplodingGrenade -= OnGrenadeExplosion;
        base.UnsubscribeEvents();
    }

    public SmokeGrenade()
    {
        Description = Plugin.Instance.Config.SmokeGrenadeDescription;
    }

    protected void OnGrenadeExplosion(ExplodingGrenadeEventArgs ev)
    {
        ev.IsAllowed = false;

        Vector3 GrenadePosition = ev.Position;
        Scp244 scp244 = (Scp244)Exiled.API.Features.Items.Item.Create(ItemType.SCP244a);
        Pickup pickup = null;
        scp244.Scale = new Vector3(0.01f, 0.01f, 0.01f);
        scp244.Primed = true;
        scp244.MaxDiameter = 0f;
        pickup = scp244.CreatePickup(GrenadePosition);
        pickup.Rigidbody.useGravity = false;
        var SmokeDuration = Plugin.Instance.Config.SmokeGrenadeDuration;
        Timing.CallDelayed(SmokeDuration, delegate
        {
            pickup.Position += Vector3.down * 10f;

            //this is so there is like a fade effect so it just looks better than it just being pew gone, cuz when you destroy the pickup the smoke insta dissapears when moving the pickup it does not
            Timing.CallDelayed(20f, delegate
            {
                pickup.Destroy();
            });
        });
    }
}


