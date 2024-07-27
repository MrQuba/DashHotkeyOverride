using Terraria.ModLoader;

namespace DashHotkeyOverride
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	public class DashHotkeyOverride : Mod
	{
		public override void Load()
		{
			Config.Config.Load();
		}
	}
}
