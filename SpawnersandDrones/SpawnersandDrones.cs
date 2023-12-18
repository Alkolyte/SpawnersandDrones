using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using UnityEngine;
using RimWorld;

using HarmonyLib;

namespace SpawnersandDrones
{
    public class SpawnersandDronesSettings : ModSettings
    {
        /// <summary>
        /// The settings our mod has.
        /// </summary>
        ///public bool isBuildable;
        public float maxSpawnerPoints = 500f;
        public bool tenTimes;
        public bool fiveTimes;
        public bool threeTimes;
        public bool twoTimes;
        public bool halfTimes;
        public bool noInterval;

        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            ///Scribe_Values.Look(ref isBuildable, "isBuildable");
            Scribe_Values.Look(ref maxSpawnerPoints, "maxSpawnerPoints");
            Scribe_Values.Look(ref tenTimes, "tenTimes");
            Scribe_Values.Look(ref fiveTimes, "fiveTimes");
            Scribe_Values.Look(ref threeTimes, "threeTimes");
            Scribe_Values.Look(ref twoTimes, "twoTimes");
            Scribe_Values.Look(ref halfTimes, "halfTime");
            Scribe_Values.Look(ref noInterval, "noInterval");
            base.ExposeData();
        }
    }
    [StaticConstructorOnStartup]
    public class SpawnersandDrones : Mod
    {
        /// <summary>
        /// A reference to our settings.
        /// </summary>
        SpawnersandDronesSettings settings;

        /// <summary>
        /// A mandatory constructor which resolves the reference to our settings.
        /// </summary>
        /// <param name="content"></param>
        public SpawnersandDrones(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<SpawnersandDronesSettings>();
            var harmony = new Harmony("Alkolyte.DeathOnDowned");
            harmony.PatchAll();
        }

        [StaticConstructorOnStartup] // this makes the static constructor get called AFTER defs are loaded
        public static class OnDefsLoaded
        { 
            static OnDefsLoaded()
            {
                // apply settings to defs now that defs are loaded:
                ApplySettingsToDefs();
            }
            public static void ApplySettingsToDefs()
            {  // a public static method that can be called from anywhere
                String[] spawnerNames = new String[13] { "ScrappieAssembler", "PepperAssembler", "LoggerAssembler", "TokamacAssembler", "HomerAssembler", "CinderAssembler", "CutramAssembler", "EScoutAssembler", "ESpecialistAssembler", "CentipedeAssembler", "ScytherAssembler", "LancerAssembler", "PikemanAssembler" };
                
                float maxSpawnerPoints = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().maxSpawnerPoints;
                for (int i = 0; i < spawnerNames.Length; i++)
                {
                    DefDatabase<ThingDef>.GetNamed(spawnerNames[i]).GetCompProperties<CompProperties_SpawnerPawn>().maxSpawnedPawnsPoints = (int)maxSpawnerPoints;
                }
                bool tenTimes = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().tenTimes;
                bool fiveTimes = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().fiveTimes;
                bool threeTimes = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().threeTimes;
                bool twoTimes = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().twoTimes;
                bool halfTimes = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().halfTimes;
                bool noInterval = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().noInterval;

                float mini = 0.85f;
                float maxi = 1.15f;
                if (tenTimes || fiveTimes || threeTimes || twoTimes || halfTimes || noInterval)
                {
                    if (tenTimes)
                    {
                        mini = mini * 10f;
                        maxi = maxi * 10f;
                    }
                    else if (fiveTimes)
                    {
                        mini = mini * 5f;
                        maxi = maxi * 5f;
                    }
                    else if (threeTimes)
                    {
                        mini = mini * 3f;
                        maxi = maxi * 3f;
                    }
                    else if (twoTimes)
                    {
                        mini = mini * 2f;
                        maxi = maxi * 2f;
                    }
                    else if (halfTimes)
                    {
                        mini = mini * 0.5f;
                        maxi = maxi * 0.5f;
                    }
                    else if (noInterval)
                    {
                        mini = mini * 0.001f;
                        maxi = maxi * 0.001f;
                    }
                    for (int i = 0; i < spawnerNames.Length; i++)
                    {
                        DefDatabase<ThingDef>.GetNamed(spawnerNames[i]).GetCompProperties<CompProperties_SpawnerPawn>().pawnSpawnIntervalDays = new FloatRange(mini, maxi);
                    }
                }
            

                ///bool isBuildable = LoadedModManager.GetMod<SpawnersandDrones>().GetSettings<SpawnersandDronesSettings>().isBuildable;
                ///if (isBuildable)
                ///{
                /// attempt to toggle buildability 1
                ///BuildableDef buildableDef = DefDatabase<BuildableDef>.GetNamed("HomerAssembler");
                ///DefDatabase<BuildableDef>.GetNamed("HomerAssembler").designationCategory = DefDatabase<DesignationCategoryDef>.GetNamed("Security");

                // attempt 2
                //(Unknown object goes her).GameRules.SetAllowBuilding(DefDatabase<BuildableDef>.GetNamed("BaseSpawner"), isBuildable);
                ///}
            }
        }
        /// <summary>
        /// The (optional) GUI part to set your settings.
        /// </summary>
        /// <param //name="inRect">A Unity Rect with the size of the settings window.</param>

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            listingStandard.Label("Max points a spawner has to grow population, default 500. Centipede has 400 points, pikeman are 110, lancer, scyther and specialists are 150, scouts are 100, homers are 75, elites have 50 more than base versions. Warning: spawn intervals are dynamic can get very long");
            settings.maxSpawnerPoints = listingStandard.Slider(settings.maxSpawnerPoints, 500, 2500);
            listingStandard.Label("|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|");
            listingStandard.Label("500                                                                                                                                                                                                         2500");
            ///listingStandard.CheckboxLabeled("Are spawners buildable by pawns?", ref settings.isBuildable, "Probally requires restart");
            listingStandard.Label("Spawner spawn interval selection. Default spawner intervals are about 5 - 1 days. Choosing one multiplies all intervals by that factor, choosing multiple has no increasing effect, the topmost will be the only active selection");
            listingStandard.CheckboxLabeled("Multiply the default interval by 10", ref settings.tenTimes, "you did not get this mod to fight modded factions");
            listingStandard.CheckboxLabeled("Multiply the default interval by 5", ref settings.fiveTimes, "its not Rimworld without loosing my pawns");
            listingStandard.CheckboxLabeled("Multiply the default interval by 3", ref settings.threeTimes, "my turrets need some love too");
            listingStandard.CheckboxLabeled("Multiply the default interval by 2", ref settings.twoTimes, "choose this for balance reasons");
            listingStandard.CheckboxLabeled("Half the default interval", ref settings.halfTimes, "makes game rather easy");
            listingStandard.CheckboxLabeled("No spawn interval", ref settings.noInterval, "for when you want to afk a raid, over powered");
            listingStandard.End();
            OnDefsLoaded.ApplySettingsToDefs();
            base.DoSettingsWindowContents(inRect);
        }

        /// <summary>
        /// Override SettingsCategory to show up in the list of settings.
        /// Using .Translate() is optional, but does allow for localisation.
        /// </summary>
        /// <returns>The (translated) mod name.</returns>
        public override string SettingsCategory()
        {
            return "Spawners and Drones";
        }

    }
    public class DeathOnDownedChance : ThingComp
    {
        public CompProperties_DeathOnDownedChance Props
        {
            get
            {
                return (CompProperties_DeathOnDownedChance)this.props;
            }
        }
    }

    public class CompProperties_DeathOnDownedChance : CompProperties
    {
        public float DeathChance;
        // (<DeathChance>###</DeathChance>)

        public CompProperties_DeathOnDownedChance()
        {
            // (redundancy) - this.compClass = typeof(DeathOnDownedChance);
            compClass = typeof(DeathOnDownedChance);
        }
    }

    [HarmonyPatch(typeof(Pawn_HealthTracker), "MakeDowned")] class DownedPatch 
    {
        public static bool Prefix(Pawn_HealthTracker __instance, DamageInfo? dinfo, Hediff hediff, Pawn ___pawn)
        {
            DeathOnDownedChance comp = ___pawn.GetComp<DeathOnDownedChance>();
            if (comp != null & !___pawn.Dead)
            {
                ___pawn.Kill(dinfo, null);
                return false;
                //if (___pawn.Downed) // (Rand.Chance(comp.Props.DeathChance ) && ___pawn.Downed)
                //{
                //    ___pawn.Kill(dinfo, null);
                //    Log.Warning("hello3");
                //    return false;
                //}
            }
            return true;
        }
    }

    // For is you want the relations object to be created on spawn <laggy probably>
    [HarmonyPatch(typeof(PawnComponentsUtility), "CreateInitialComponents")] class AddFields
    {
        public static bool Prefix(Pawn pawn)
        {
            DeathOnDownedChance comp = pawn.GetComp<DeathOnDownedChance>();
            if (comp != null)
            {
    //            if (pawn.relations == null)
    //            {
    //                pawn.relations = new Pawn_RelationsTracker(pawn);
    //            }
                if (pawn.drafter == null)
                {
                    pawn.drafter = new Pawn_DraftController(pawn);
                }
            }
            return true;
        }
    }

    // if you just want to suppress the error, instead of doing this hacky way of adding it in the prefix, 
    // you can use a finalizer patch and it will wrap the original method in a try catch, and you can handle the exception yourself
    // The current fix causes melee pawns to freeze (maybe set up relations on spawn)
    [HarmonyPatch(typeof(Pawn_HealthTracker), "NotifyPlayerOfKilled")] class DeathLetterPatch
    {
        public static bool Prefix(Pawn ___pawn)
        {
            DeathOnDownedChance comp = ___pawn.GetComp<DeathOnDownedChance>();
            if (comp != null)
            {
                if (___pawn.relations == null)
                {
                    ___pawn.relations = new Pawn_RelationsTracker(___pawn);
               }
                ___pawn.SetFaction(null, null);
            }
            return true;
        }
    }

    //[HarmonyPatch(typeof(RimWorld.Planet.CaravanUIUtility), "AddPawnsSections")] internal class CaravanUIUtility_AddPawnsSections
    //{
    //    private static void Postfix(ref TransferableOneWayWidget widget, List<TransferableOneWay> transferables)
    //    {
    //        widget.AddSection("Mechanoids".Translate(), from x in transferables where x.ThingDef.category == ThingCategory.Pawn && ((Pawn)x.AnyThing).RaceProps.IsMechanoid select x);
    //        foreach (var transferable in transferables)
    //        {
    //            Log.Message($"Label: {transferable.Label}, Category: {transferable.ThingDef.category.ToString()}, IsMechanoid - {(transferable.AnyThing as Pawn)?.RaceProps?.IsMechanoid}");
    //        }
    //    }
    //}
}