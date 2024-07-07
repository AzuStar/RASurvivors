using System;
using System.Collections.Generic;
using Godot;

namespace RA2Survivors
{
    public static class ResourceProvider
    {
        public const string RESOURCE_MASTER_PATH = "res://Prefabs/";

        public static T CreateResource<T>(string name)
            where T : Node
        {
            return ResourceLoader.Load<PackedScene>(RESOURCE_MASTER_PATH + name).Instantiate<T>();
        }

        #region Entities
        private static readonly Dictionary<string, string> ENTITY_PATHS = new Dictionary<
            string,
            string
        >()
        {
            ["Conscript"] = RESOURCE_MASTER_PATH + "Entities/Conscript.tscn",
            ["GI"] = RESOURCE_MASTER_PATH + "Entities/GI.tscn"
        };
        private static readonly Dictionary<EEntityType, string> ENTITY_NAMES = new Dictionary<
            EEntityType,
            string
        >()
        {
            [EEntityType.Conscript] = "Conscript",
            [EEntityType.GI] = "GI"
        };

        public static T CreateEntity<T>(string entityName)
            where T : RigidBody3D
        {
            return ResourceLoader.Load<PackedScene>(ENTITY_PATHS[entityName]).Instantiate<T>();
        }

        public static T LoadEntity<T>(EEntityType entityType)
            where T : RigidBody3D
        {
            return CreateEntity<T>(ENTITY_NAMES[entityType]);
        }

        #endregion

        #region FloatingText
        public static string FloatingTextPath = RESOURCE_MASTER_PATH + "/FloatingText.tscn";

        public static FloatingText CreateFloatingText()
        {
            FloatingText floatingText = ResourceLoader
                .Load<PackedScene>(FloatingTextPath)
                .Instantiate<FloatingText>();
            return floatingText;
        }
        #endregion

        public static EEntityType SelectedCharacter = EEntityType.Conscript;

        #region Exp Orbs
        private static ExpOrbConfig[] expOrbConfigs = new ExpOrbConfig[]
        {
            new ExpOrbConfig
            {
                expThreshold = 50,
                scenePath = RESOURCE_MASTER_PATH + "/ExpOrbBig.tscn"
            },
            new ExpOrbConfig
            {
                expThreshold = 10,
                scenePath = RESOURCE_MASTER_PATH + "/ExpOrbMedium.tscn"
            },
            new ExpOrbConfig
            {
                expThreshold = 0,
                scenePath = RESOURCE_MASTER_PATH + "/ExpOrbSmall.tscn"
            },
        };

        public static ExpOrb CreateExpOrb(double expAmount)
        {
            ExpOrbConfig config = null;
            foreach (ExpOrbConfig orbConfig in expOrbConfigs)
            {
                if (expAmount >= orbConfig.expThreshold)
                {
                    config = orbConfig;
                    break;
                }
            }
            if (config == null)
            {
                config = expOrbConfigs[expOrbConfigs.Length - 1];
            }
            ExpOrb expOrb = ResourceLoader
                .Load<PackedScene>(config.scenePath)
                .Instantiate<ExpOrb>();
            expOrb.expAmount = expAmount;
            return expOrb;
        }

        private class ExpOrbConfig
        {
            public double expThreshold;
            public string scenePath;
        }

        #endregion
    }
}