using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace FishingAdjustMod
{
    public class Main : Mod
    {
        private Config Config { get; set; }

        public override void Entry(IModHelper helper)
        {
            base.Entry(helper);

            helper.ConsoleCommands.Add("dump_fishingData", "Dump loaded fishing data. You can specify item numbers to dump specified fishes.", (_, args) => DumpFishingData(args));
            Config = helper.ReadConfig<Config>();

            ControlEvents.KeyPressed += this.ControlEvents_KeyPressed;
            ContentEvents.AfterLocaleChanged += this.ContentEvents_AfterLocaleChanged;
            SaveEvents.AfterLoad += this.SaveEvents_AfterLoad;
        }

        private void ControlEvents_KeyPressed(object sender, EventArgsKeyPressed e)
        {
            if (e.KeyPressed == Microsoft.Xna.Framework.Input.Keys.P)
            {
                new StardewValley.Menus.BobberBar(163, 150, false, 695);
            }
        }

        private void DumpFishingData(string[] args)
        {
            try
            {
                var fishingData = Game1.content.Load<Dictionary<int, string>>(@"Data\Fish");
                if (args.Length == 0)
                {
                    foreach (var p in fishingData)
                    {
                        Monitor.Log($"{p.Key} = {p.Value}");
                    }
                }
                else
                {
                    var keys = new HashSet<int>(args.Select(int.Parse));
                    foreach (var p in fishingData)
                    {
                        if (keys.Contains(p.Key))
                        {
                            Monitor.Log($"{p.Key} = {p.Value}");
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Monitor.Log(e.ToString(), LogLevel.Error);
            }
        }

        private void SaveEvents_AfterLoad(object sender, EventArgs e)
        {
            Monitor.Log("SaveEvents_AfterLoad");

            InjectFishingDifficultyAdjustance();
        }

        private void ContentEvents_AfterLocaleChanged(object sender, EventArgsValueChanged<string> e)
        {
            Monitor.Log("ContentEvents_AfterLocaleChanged");

            InjectFishingDifficultyAdjustance();
        }

        private void InjectFishingDifficultyAdjustance()
        {
            var fishingData = Game1.content.Load<Dictionary<int, string>>(@"Data\Fish");

            if (fishingData[163].Split('/')[1] != "110")
            {
                this.Monitor.Log("Fishing difficulty already adjusted!", LogLevel.Error);
                return;
            }

            foreach (var k in fishingData.Keys.ToArray())
            {
                var dataGroup = fishingData[k].Split('/');
                if (int.TryParse(dataGroup[1], out int level))
                {
                    dataGroup[1] = ((int)(level * Config.AdjustRatio)).ToString();
                    fishingData[k] = string.Join("/", dataGroup);
                }
            }
            Monitor.Log("Fishing difficulty adjusted.", LogLevel.Info);
        }
    }
}
