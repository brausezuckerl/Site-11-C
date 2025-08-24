using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using Exiled.Events.Handlers;
using MEC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerRoles;
using CustomPlayerEffects;
using Site11Cursed.Configs;
using Site11Cursed;

namespace Site11Cursed.CustomItems;

[CustomItem(ItemType.GunCOM18)]

public class Taser : CustomWeapon
{

    public override ItemType Type { get; set; } = ItemType.GunCOM18;

    public override uint Id { get; set; } = 600u;

    public override string Name { get; set; } = "Taser";

    public override string Description { get; set; } = "";

    public override float Weight { get; set; } = 1f;

    public override float Damage { get; set; } = 0f;

    public override byte ClipSize { get; set; } = 1;

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
        Exiled.Events.Handlers.Player.Shot += OnShot;
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.Shot -= OnShot;
        base.UnsubscribeEvents();
    }

    public Taser()
    {
        Description = Plugin.Instance.Config.TaserDescription;
    }

    protected override void OnShot(ShotEventArgs ev)
    {
        Log.Debug("Shot Event Triggered.");
        if (!Check(ev.Player.CurrentItem))
            return;

        Log.Debug("Taser was shot event.");

        if (ev.Player == null || ev.Target == null)
            return;
        Log.Debug("Player and Target are not null.");
        ev.CanHurt = false;

        if (ev.Target.RoleManager.CurrentRole.Team == Team.SCPs)
            return;
        Log.Debug("Target is not SCP.");

        Armor currentArmor = ev.Target.CurrentArmor;
        if (currentArmor == null || currentArmor.Type != ItemType.ArmorHeavy || currentArmor.Type != ItemType.ArmorCombat)
        {
            Log.Debug("Target does not have heavy or combat armor.");

            ev.Target.EnableEffect<Slowness>(85, 4f);
            ev.Target.EnableEffect<Deafened>(6f);
            ev.Target.EnableEffect<Blurred>(6f);
            ev.Target.EnableEffect<Concussed>(6f);
            ev.Target.DropHeldItem();
            Timing.RunCoroutine(Slowness(ev.Target));
            Log.Debug($"Taser effects applied to {ev.Target}.");


        }
    }


    private IEnumerator<float> Slowness(Exiled.API.Features.Player target)
    {
        yield return Timing.WaitForSeconds(4f);
        byte[] slowLevels = new byte[7] { 70, 60, 50, 40, 30, 20, 10 };
        byte[] array = slowLevels;
        foreach (byte slow in array)
        {
            target.EnableEffect<Slowness>(slow, 1f);
            yield return Timing.WaitForSeconds(1f);
        }
    }
}


