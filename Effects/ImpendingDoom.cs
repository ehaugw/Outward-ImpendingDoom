using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImpendingDoom
{
    using HarmonyLib;
    using SideLoader;
    using UnityEngine;
    using EffectSourceConditions;

    public class ImpendingDoom : Effect
    {
        public const float RANGE = 4f;
        public const float LIFE_SPAN = 15;
        public const float DAMAGE = 15f;
        public const float REFRESH_RATE = 1f;
        public const float BOOM_THRESHOLD = 40f;

        protected override void ActivateLocally(Character character, object[] _infos)
        {
            if (At.GetField<Effect>(this, "m_parentStatusEffect") is StatusEffect parent && parent.SourceCharacter != null)
            {
                
                if (!character.Invincible)
                {
                    float dmg = HolyDamageManager.HolyDamageManager.BuffHolyDamageOrHealing(parent.SourceCharacter, DAMAGE * REFRESH_RATE / LIFE_SPAN);
                    var damageList = new DamageList(HolyDamageManager.HolyDamageManager.GetDamageType(), dmg);
                    character.Stats.GetMitigatedDamage(null, ref damageList, true);
                    character.Stats.ReceiveDamage(damageList.TotalDamage);
                }
            }
           
        }

        public static DamageList RemainingDamageList(Character character, StatusEffect parentStatusEffect, bool mitigated=true)
        {
            var remainingDamage = HolyDamageManager.HolyDamageManager.BuffHolyDamageOrHealing(character, DAMAGE * parentStatusEffect.RemainingLifespan / LIFE_SPAN);
            var remainingDamageList = new DamageList(HolyDamageManager.HolyDamageManager.GetDamageType(), remainingDamage);
            if (mitigated)
            {
                character.Stats.GetMitigatedDamage(null, ref remainingDamageList, true);
            }
            return remainingDamageList;
        }

        public static float RemainingDamage(Character character, StatusEffect parentStatusEffect, bool mitigated = true)
        {
            return RemainingDamageList(character, parentStatusEffect, mitigated).TotalDamage;
        }
    }

    [HarmonyPatch(typeof(StatusEffectManager), "OverrideStatusEffect", new Type[] { typeof(StatusEffect), typeof(StatusEffect) })]
    public class StatusEffectManager_OverrideStatusEffect_ImpendingDoom
    {
        [HarmonyPrefix]
        public static void Prefix(StatusEffectManager __instance, StatusEffect _existingEffect, StatusEffect _newEffect, out StatusData __state)
        {
            __state = null;
            
            if (_newEffect.IdentifierName == _existingEffect.IdentifierName && _newEffect.IdentifierName == ImpendingDoomMod.Instance.impendingDoomInstance.IdentifierName)
            {
                __state = _newEffect.StatusData;
                _newEffect.StatusData = new StatusData(__state);
                _newEffect.StatusData.LifeSpan = (_newEffect.RemainingLifespan != 0 ? _newEffect.RemainingLifespan : _newEffect.StartLifespan) + _existingEffect.RemainingLifespan;
            }
        }

        [HarmonyPostfix]
        public static void Postfix(StatusEffectManager __instance, StatusEffect _existingEffect, StatusEffect _newEffect, StatusData __state)
        {
            if (__state != null)
            {
                _newEffect.StatusData = __state;
            }
        }
    }
}
