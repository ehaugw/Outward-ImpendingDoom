using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImpendingDoom
{
    public class ModTheme
    {
        public enum Theme
        {
            Elatt,
            DawnWeaver,
            Atheist,
            Crusader
        }

        public static Theme? modTheme = null;

        public static Theme GetTheme
        {
            get
            {
                return modTheme ?? Theme.Crusader;
            }
        }
        public const string ImpendingDoomEffectIdentifierName = "ImpendingDoom";

        public static string ImpendingDoomEffectName
        {
            get
            {
                return "Impending Doom";
            }
        }
        public static string Themifization() { return null; }
    }
}
