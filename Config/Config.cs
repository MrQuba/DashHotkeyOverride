using System.IO;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.IO;

namespace DashHotkeyOverride.Config
{
	// based on: https://forums.terraria.org/index.php?threads/modders-guide-to-config-files-and-optional-features.48581/
	public static class Config
	{
		public static Keys DashHotkey = Keys.LeftShift;
		public static bool DashHotkeyEnabled = true;
		static string ConfigPath = Path.Combine(Main.SavePath, "ModConfigs", "DashHotkeyOverride.json");
		static Preferences Configuration = new Preferences(ConfigPath);
		public static void Load()
		{
			bool success = ReadConfig();

			if (!success)
			{
				CreateConfig();
			}
		}

		//Returns "true" if the config file was found and successfully loaded.
		static bool ReadConfig()
		{
			if (Configuration.Load())
			{
				// is enabled
				Configuration.Get("Dash Hotkey Enabled", ref DashHotkeyEnabled);
				// key
				Configuration.Get("DashHotkey", ref DashHotkey);
				return true;
			}
			return false;
		}

		//Creates a config file. This will only be called if the config file doesn't exist yet or it's invalid. 
		static void CreateConfig()
		{
			Configuration.Clear();
			// is enabled
			Configuration.Put("Dash Hotkey Enabled", DashHotkeyEnabled);
			// key
			Configuration.Put("DashHotkey", DashHotkey);
			Configuration.Save();
		}
	}
}