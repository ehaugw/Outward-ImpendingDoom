using EffectSourceConditions;
using UnityEngine;

namespace ImpendingDoom
{
    public class WillDieFromImpendingDoom : EffectCondition
    {
        protected override bool CheckIsValid(Character _affectedCharacter)
        {
            if (_affectedCharacter.StatusEffectMngr.GetStatusEffectOfName(ImpendingDoomMod.Instance.impendingDoomInstance.IdentifierName) is StatusEffect impendingDoom)
            {
                float remaining = ImpendingDoom.RemainingDamage(_affectedCharacter, impendingDoom);
                if (remaining >= _affectedCharacter.Health && remaining >= ImpendingDoom.BOOM_THRESHOLD)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
