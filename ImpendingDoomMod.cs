namespace ImpendingDoom
{
    using System.Collections.Generic;
    using UnityEngine;
    using BepInEx;
    using HarmonyLib;
    using System;
    using SideLoader;

    [BepInPlugin(GUID, NAME, VERSION)]
    public class ImpendingDoomMod : BaseUnityPlugin
    {
        public const string GUID = "com.ehaugw.impendingdoom";
        public const string VERSION = "1.0.0";
        public const string NAME = "Impending Doom";

        public static ImpendingDoomMod Instance;
        public StatusEffect impendingDoomInstance;

        internal void Awake()
        {
            Instance = this;
            var harmony = new Harmony(GUID);
            harmony.PatchAll();

            SL.OnPacksLoaded += OnPackLoaded;
        }
        private void OnPackLoaded()
        {
            impendingDoomInstance = EffectInitializer.MakeImpendingDoomPrefab();
        }
    }
}