using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Attributes;
using Exiled.API.Features.Items;
using Exiled.API.Features.Spawn;
using Exiled.CustomItems.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using Exiled.Events.Handlers;
using InventorySystem;
using InventorySystem.Disarming;
using LabApi.Features.Wrappers;
using MEC;
using PlayerRoles;
using Site11Cursed;
using Site11Cursed.Configs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils.Networking;
using Player = Exiled.API.Features.Player;

namespace Site11Cursed.CustomItems;

[CustomItem(ItemType.SCP1344)]

public class HandCuffs : CustomItem
{

    public override ItemType Type { get; set; } = ItemType.SCP1344;

    public override uint Id { get; set; } = 602u;

    public override string Name { get; set; } = "Handcuffs";

    public override string Description { get; set; } = "";

    public override float Weight { get; set; } = 1f;

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
        Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
    }

    protected override void UnsubscribeEvents()
    {
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
        base.UnsubscribeEvents();
    }

    public HandCuffs()
    {
        Description = Plugin.Instance.Config.HandCuffsDescription;
    }

    public void OnUsingItem(UsingItemEventArgs ev)
    {
        if (!Check(ev.Player.CurrentItem))
            return;

        ev.IsAllowed = false;


        Vector3 origin = ev.Player.CameraTransform.position;
        Vector3 direction = ev.Player.CameraTransform.forward;

        int layerMask = ~0;
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 5f, layerMask))
        {
            Log.Debug("Raycast hit something: " + hit.collider.name);

            Player target = Player.Get(hit.collider.gameObject);

            if (target != null && target != ev.Player && target.Role.Team != Team.SCPs && !target.IsCuffed)
            {

                // can't use the ev.Player as cuffer because that would have it so you can't cuff anyone in your team
                target.ReferenceHub.inventory.SetDisarmedStatus(null);
                DisarmedPlayers.Entries.Add(new DisarmedPlayers.DisarmedEntry(target.ReferenceHub.networkIdentity.netId, 0U));
                new DisarmedPlayersListMessage(DisarmedPlayers.Entries).SendToAuthenticated();

                if (Plugin.Instance.Config.HandCuffsOneUse == true)
                {
                    ev.Player.RemoveHeldItem();
                }
            }
        }
        else
        {
            return;
        }

    }


}