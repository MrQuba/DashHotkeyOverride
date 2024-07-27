namespace DashHotkeyOverride.Decompiled;
// this class is from decompiled code and was not made by me
internal static class ProjectileSourceID
{
  public const int None = 0;
  public const int SetBonus_SolarExplosion_WhenTakingDamage = 1;
  public const int SetBonus_SolarExplosion_WhenDashing = 2;
  public const int SetBonus_ForbiddenStorm = 3;
  public const int SetBonus_Titanium = 4;
  public const int SetBonus_Orichalcum = 5;
  public const int SetBonus_Chlorophyte = 6;
  public const int SetBonus_Stardust = 7;
  public const int WeaponEnchantment_Confetti = 8;
  public const int PlayerDeath_TombStone = 9;
  public const int TorchGod = 10;
  public const int FallingStar = 11;
  public const int PlayerHurt_DropFootball = 12;
  public const int StormTigerTierSwap = 13;
  public const int AbigailTierSwap = 14;
  public const int SetBonus_GhostHeal = 15;
  public const int SetBonus_GhostHurt = 16;
  public const int VampireKnives = 18;
  public static readonly int Count = 19;

  public static string? ToContextString(int itemSourceId)
  {
    string contextString;
    switch (itemSourceId)
    {
      case 1:
        contextString = "SetBonus_SolarExplosion_WhenTakingDamage";
        break;
      case 2:
        contextString = "SetBonus_SolarExplosion_WhenDashing";
        break;
      case 3:
        contextString = "SetBonus_ForbiddenStorm";
        break;
      case 4:
        contextString = "SetBonus_Titanium";
        break;
      case 5:
        contextString = "SetBonus_Orichalcum";
        break;
      case 6:
        contextString = "SetBonus_Chlorophyte";
        break;
      case 7:
        contextString = "SetBonus_Stardust";
        break;
      case 8:
        contextString = "WeaponEnchantment_Confetti";
        break;
      case 9:
        contextString = "PlayerDeath_TombStone";
        break;
      case 10:
        contextString = "TorchGod";
        break;
      case 11:
        contextString = "FallingStar";
        break;
      case 12:
        contextString = "PlayerHurt_DropFootball";
        break;
      case 13:
        contextString = "StormTigerTierSwap";
        break;
      case 14:
        contextString = "AbigailTierSwap";
        break;
      case 15:
        contextString = "SetBonus_GhostHeal";
        break;
      case 16:
        contextString = "SetBonus_GhostHurt";
        break;
      case 18:
        contextString = "VampireKnives";
        break;
      default:
        contextString = (string) null;
        break;
    }
    return contextString;
  }
}