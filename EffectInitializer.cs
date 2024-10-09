using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyHelper;
using UnityEngine;

namespace ImpendingDoom
{
    using EffectSourceConditions;
    using SideLoader;
    using System.IO;

    public class EffectInitializer
    {
        public static StatusEffect MakeImpendingDoomPrefab()
        {
            var statusEffect = TinyEffectManager.MakeStatusEffectPrefab(
                effectName: ModTheme.ImpendingDoomEffectIdentifierName,
                displayName: ModTheme.ImpendingDoomEffectName,
                familyName: ModTheme.ImpendingDoomEffectIdentifierName,
                description: "Take 1 " + HolyDamageManager.HolyDamageManager.GetDamageType().ToString() + " over time and be smitten by thunder if you build up too much " + ModTheme.ImpendingDoomEffectName + ".",
                lifespan: ImpendingDoom.LIFE_SPAN,
                refreshRate: ImpendingDoom.REFRESH_RATE,
                stackBehavior: StatusEffectFamily.StackBehaviors.Override,
                targetStatusName: "HolyBlaze",
                isMalusEffect: true,
                modGUID: ImpendingDoomMod.GUID,
                iconFileName: @"\SideLoader\Texture2D\impendingDoomIcon.png",
                rootPath: Directory.GetParent(typeof(ImpendingDoomMod).Assembly.Location).ToString()
            );

            var effectSignature = statusEffect.StatusEffectSignature;
            Transform effectsContainer;

            //Impending Doom
            effectsContainer = TinyGameObjectManager.MakeFreshObject(EffectSourceConditions.EFFECTS_CONTAINER, true, true, effectSignature.transform).transform;
            var impendingDoom = effectsContainer.gameObject.AddComponent<ImpendingDoom>();
            impendingDoom.UseOnce = false;

            //Nova
            effectsContainer = TinyGameObjectManager.MakeFreshObject(EffectSourceConditions.EFFECTS_CONTAINER, true, true, effectSignature.transform).transform;
            WillDieFromImpendingDoom condition = effectsContainer.gameObject.AddComponent<WillDieFromImpendingDoom>();

            var shootBlast = effectsContainer.gameObject.AddComponent<TinyHelper.Effects.ShootBlastFromEffect>();

            shootBlast.UseOnce = true;
            shootBlast.enabled = true;
            shootBlast.transform.parent = effectsContainer;
            shootBlast.BaseBlast = SL_ShootBlast.GetBlastPrefab(SL_ShootBlast.BlastPrefabs.ForceRaiseLightning).GetComponent<Blast>();
            shootBlast.InstanstiatedAmount = 5;
            shootBlast.CastPosition = Shooter.CastPositionType.Local;
            shootBlast.TargetType = Shooter.TargetTypes.Enemies;
            shootBlast.TransformName = "ShooterTransform";

            shootBlast.UseTargetCharacterPositionType = false;

            shootBlast.SyncType = Effect.SyncTypes.OwnerSync;
            shootBlast.OverrideEffectCategory = EffectSynchronizer.EffectCategories.None;
            shootBlast.BasePotencyValue = 1f;
            shootBlast.Delay = 0.25f;
            shootBlast.LocalCastPositionAdd = new Vector3(0f, -1.0f, 0);
            shootBlast.BaseBlast.Radius = ImpendingDoom.RANGE;

            effectSignature.Effects = new List<Effect>() { impendingDoom, shootBlast };

            //thunderGameObject.GetComponentInChildren<PunctualDamage>().enabled = true;

            var blastEffects = shootBlast.BaseBlast.transform.Find("Effects");
            var damage = blastEffects.GetComponent<PunctualDamage>();
            GameObject.Destroy(damage);
            damage = blastEffects.gameObject.AddComponent<ImpendingDoomDamage>();
            damage.Delay = 0.25f;
            damage.Knockback = 40;

            return statusEffect;
        }
    }
}
