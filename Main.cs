using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MelonLoader;
using UnityEngine;
using Microsoft.Win32;
using System.Linq;

namespace PWPlus
{
    public class PWPlus : MelonMod
    {
        public static MelonLogger.Instance Logger;
        public static Tab tab = Tab.None;
        public static World world;
        public static Player player;
        public static PlayerData playerData;
        private static int totalgems;
        private static string worldToWarp = "";
        private static bool showMenu = true;
        private static Rect windowSize = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 250, 250);

        // on startup
        public override void OnApplicationStart()
        {
            LoggerInstance.Msg(ConsoleColor.Cyan, "Starting PW Plus...");
            Logger = LoggerInstance;
        }
        
        // on quit
        public override void OnApplicationQuit()
        {
            LoggerInstance.Msg(ConsoleColor.Cyan, "Quitting PW Plus...");
        }
        
        // tab enum
        public enum Tab
        {
            None,
            GemCalculator,
            WorldWarper,
            GemSeller
        }


        private void MyWindow(int id)
        {
            // buttons to switch tabs
            if (GUI.Button(new Rect(0f, 16.5f, 83.333f, 33f), "Gem Calc"))
            {
                tab = Tab.GemCalculator;
            }
            if (GUI.Button(new Rect(83.333f, 16.5f, 83.333f, 33f), "World Warp"))
            {
                tab = Tab.WorldWarper;
            }
            if (GUI.Button(new Rect(166.600f, 16.5f, 83.333f, 33f), "Gem Seller"))
            {
                tab = Tab.GemSeller;
            }

            // check value of tab to see what tab to render
            switch (tab)
            {
                case Tab.None:
                    // No tab open

                    break;

                case Tab.GemCalculator:
                    // Gem Calculator Tab
                    GUI.Label(new Rect(5f, 50f, 100f, 20f), "Gem Calculator");

                    if (GUI.Button(new Rect(5f, 75f, 115f, 33f), "Calculate Gems"))
                    {
                        if (world != null)
                        {
                            totalgems = 0;

                            foreach (CollectableData collectable in world.collectables)
                            {
                                totalgems += (Tools.GemCalculator.GetGemAmount(collectable.blockType) * collectable.amount); 
                            }
                        }
                        else
                        {
                            totalgems = 0;
                        }
                    }
                    GUI.Label(new Rect(5f, 108f, 100f, 20f), $"Gems: {totalgems}");

                    break;

                case Tab.WorldWarper:
                    // Warp World Tab
                    GUI.Label(new Rect(5f, 50f, 100f, 20f), "Warp To World");
                    
                    worldToWarp = GUI.TextField(new Rect(125f, 75f, 85f, 33f), worldToWarp);
                    
                    if (GUI.Button(new Rect(5f, 75f, 115f, 33f), "Warp To World"))
                    {
                        SceneLoader.CheckIfWeCanGoFromWorldToWorld(worldToWarp, "", null, false, null);
                    }

                    break;

                case Tab.GemSeller:
                    // Gem Seller Tab
                    GUI.Label(new Rect(5f, 50f, 100f, 20f), "Gem Seller");
                    
                    if (GUI.Button(new Rect(5, 75f, 115f, 33f), "Sell Gems"))
                    {
                        int gems = 0;

                        var inventory = playerData.GetInventoryAsOrderedByInventoryItemType();

                        for (int i = 0; i < inventory.Count(); i++)
                        {
                            short count = playerData.GetCount(inventory[i]);
                            World.BlockType blockType = inventory[i].blockType;

                            // mgems
                            if (ConfigData.IsBlockMiningGemstone(blockType))
                            {
                                OutgoingMessages.RecycleMiningGemstone(inventory[i], count);
                                gems += Tools.GemCalculator.GetGemAmount(blockType) * count;
                                playerData.RemoveItemsFromInventory(inventory[i], count);
                            }

                            // fgems
                            if (ConfigData.IsFish(blockType))
                            {
                                OutgoingMessages.RecycleFish(inventory[i], count);
                                gems += Tools.GemCalculator.GetGemAmount(blockType) * count;
                                playerData.RemoveItemsFromInventory(inventory[i], count);
                            }
                        }

                        Logger.Msg(ConsoleColor.Green, $"Recycled: {gems} gems.");
                        playerData.AddGems(gems);
                    }

                    break;

                default:
                    // If it is none of these
                    break;
            }

            GUI.DragWindow();
        }

        public override void OnGUI()
        {
            // Watermark rendering
            GUIStyle style = new GUIStyle();
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 60;
            style.normal.textColor = Color.white;
            GUI.Label(new Rect(Screen.width - 280, 40, 480, 180), "PW Plus", style);


            // Render the menu if showMenu is true
            if (showMenu)
            {
                windowSize = GUI.Window(0, windowSize, (GUI.WindowFunction)MyWindow, "PW Plus Window");
            }
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Insert))
            {
                showMenu = !showMenu;
            }
        }

    }
}
