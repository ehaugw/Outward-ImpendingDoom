namespace ImpendingDoom
{
    using SideLoader;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;


    class ImpendingDoomDamage : PunctualDamage
    {
        protected override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            StatusEffect impendingDoom = this.SourceSynchronizer as StatusEffect;
            DamageList damage = ImpendingDoom.RemainingDamageList(_affectedCharacter, impendingDoom);
            Damages = damage.List.ToArray();
            DamageAmplifiedByOwner = false;

            //Debug.Log("Affected: " + _affectedCharacter);
            //Debug.Log("Source: " + SourceCharacter);
            base.ActivateLocally(_affectedCharacter, _infos);
            //}
        }
    }
}
