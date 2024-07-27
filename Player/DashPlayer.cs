using System.Linq;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using System;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using DashHotkeyOverride.Decompiled;
namespace DashHotkeyOverride.Player;

public class DashPlayer : ModPlayer
{
  // SolarDashStart, DoCommonDashHandle, GetProjectileSource_OnHit and DashMovement come from decompiled code and were only modified by me
    public bool IsEnabled = Config.Config.DashHotkeyEnabled;
    public Keys dash = Config.Config.DashHotkey;
    
    public override void PreUpdateMovement()
    {
      IsEnabled = Config.Config.DashHotkeyEnabled;
      dash = Config.Config.DashHotkey;
      if(!IsEnabled) return;
          DashMovement();
    }
    private void SolarDashStart(int dashDirection)
    {
      Player.solarDashing = true;
      Player.solarDashConsumedFlare = false;
    }
    public delegate void DashStartAction(int dashDirection);
    private void DoCommonDashHandle(out int dir, out bool dashing, DashStartAction dashStartAction = null)
    {
      dir = 0;
      dashing = false;
      if (Player.dashTime > 0)
      {
        Player.dashTime--;
      }
      if (Player.dashTime < 0)
      {
        Player.dashTime++;
      }
      if (Player.controlRight && Main.keyState.IsKeyDown(dash))
      {
        if (Player.dashTime <= 0)
        {
          Player.dashTime = 15;
          return;
        }
        dir = 1;
        dashing = true;
        Player.dashTime = 0;
        Player.timeSinceLastDashStarted = 0;
        dashStartAction?.Invoke(dir);
      }
      else
      {
        if (Player.controlLeft && Main.keyState.IsKeyDown(dash))
        {
          if (Player.dashTime < 0)
          {
            dir = -1;
            dashing = true;
            Player.dashTime = 0;
            Player.timeSinceLastDashStarted = 0;
            dashStartAction?.Invoke(dir);
          }
          else
          {
            Player.dashTime = -15;
          }
        }
      }
    }
    internal IEntitySource GetProjectileSource_OnHit(Entity victim, int projectileSourceId)
    {
      return Player.GetSource_OnHit(victim, ProjectileSourceID.ToContextString(projectileSourceId));
    }
    public void DashMovement()
{
  if (Player.dashDelay == 0)
    Player.dash = Player.dashType;
  if (Player.dash == 0)
  {
    Player.dashTime = 0;
    Player.dashDelay = 0;
  }
  StatModifier statModifier;
  if (Player.dash == 2 && Player.eocDash > 0)
  {
    if (Player.eocHit < 0)
    {
      Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) ((double) Player.position.X + (double) Player.velocity.X * 0.5 - 4.0), (int) ((double) Player.position.Y + (double) Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
      for (int index = 0; index < 200; ++index)
      {
        NPC npc = Main.npc[index];
        if (npc.active && !npc.dontTakeDamage && !npc.friendly && (npc.aiStyle != 112 || (double) npc.ai[2] <= 1.0) && Player.CanNPCBeHitByPlayerOrPlayerProjectile(npc))
        {
          Microsoft.Xna.Framework.Rectangle rect = npc.getRect();
          if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit((Entity) npc)))
          {
            statModifier = Player.GetTotalDamage(DamageClass.Melee);
            float damage = statModifier.ApplyTo(30f);
            statModifier = Player.GetTotalKnockback(DamageClass.Melee);
            float knockback = statModifier.ApplyTo(9f);
            bool crit = false;
            if ((double) Main.rand.Next(100) < (double) Player.GetTotalCritChance(DamageClass.Melee))
              crit = true;
            int direction = Player.direction;
            if ((double) Player.velocity.X < 0.0)
              direction = -1;
            if ((double) Player.velocity.X > 0.0)
              direction = 1;
            if (Player.whoAmI == Main.myPlayer)
              Player.ApplyDamageToNPC(npc, (int) damage, knockback, direction, crit, DamageClass.Melee);
            Player.eocDash = 10;
            Player.dashDelay = 30;
            Player.velocity.X = (float) (-direction * 9);
            Player.velocity.Y = -4f;
            Player.GiveImmuneTimeForCollisionAttack(4);
            Player.eocHit = index;
          }
        }
      }
    }
    else if ((!Player.controlLeft || (double) Player.velocity.X >= 0.0) && (!Player.controlRight || (double) Player.velocity.X <= 0.0))
      Player.velocity.X *= 0.95f;
  }
  if (Player.dash == 3 && Player.dashDelay < 0 && Player.whoAmI == Main.myPlayer)
  {
    Microsoft.Xna.Framework.Rectangle rectangle = new Microsoft.Xna.Framework.Rectangle((int) ((double) Player.position.X + (double) Player.velocity.X * 0.5 - 4.0), (int) ((double) Player.position.Y + (double) Player.velocity.Y * 0.5 - 4.0), Player.width + 8, Player.height + 8);
    for (int index1 = 0; index1 < 200; ++index1)
    {
      NPC npc = Main.npc[index1];
      if (npc.active && !npc.dontTakeDamage && !npc.friendly && npc.immune[Player.whoAmI] <= 0 && (npc.aiStyle != 112 || (double) npc.ai[2] <= 1.0) && Player.CanNPCBeHitByPlayerOrPlayerProjectile(npc))
      {
        Microsoft.Xna.Framework.Rectangle rect = npc.getRect();
        if (rectangle.Intersects(rect) && (npc.noTileCollide || Player.CanHit((Entity) npc)))
        {
          if (!Player.solarDashConsumedFlare)
          {
            Player.solarDashConsumedFlare = true;
            Player.ConsumeSolarFlare();
          }
          statModifier = Player.GetTotalDamage(DamageClass.Melee);
          float num = statModifier.ApplyTo(150f);
          statModifier = Player.GetTotalKnockback(DamageClass.Melee);
          float knockback = statModifier.ApplyTo(9f);
          bool crit = false;
          if ((double) Main.rand.Next(100) < (double) Player.GetTotalCritChance(DamageClass.Melee))
            crit = true;
          int direction = Player.direction;
          if ((double) Player.velocity.X < 0.0)
            direction = -1;
          if ((double) Player.velocity.X > 0.0)
            direction = 1;
          if (Player.whoAmI == Main.myPlayer)
          {
            Player.ApplyDamageToNPC(npc, (int) num, knockback, direction, crit, DamageClass.Melee);
            int index2 = Projectile.NewProjectile(GetProjectileSource_OnHit((Entity) npc, 2), Player.Center.X, Player.Center.Y, 0.0f, 0.0f, 608, (int) num, 15f, Main.myPlayer);
            Main.projectile[index2].Kill();
          }
          npc.immune[Player.whoAmI] = 6;
          Player.GiveImmuneTimeForCollisionAttack(4);
        }
      }
    }
  }
  if (Player.dashDelay > 0)
  {
    if (Player.eocDash > 0)
      --Player.eocDash;
    if (Player.eocDash == 0)
      Player.eocHit = -1;
    --Player.dashDelay;
  }
  else if (Player.dashDelay < 0)
  {
    Player.StopVanityActions();
    float num1 = 12f;
    float num2 = 0.992f;
    float num3 = Math.Max(Player.accRunSpeed, Player.maxRunSpeed);
    float num4 = 0.96f;
    int num5 = 20;
    if (Player.dash == 1)
    {
      for (int index3 = 0; index3 < 2; ++index3)
      {
        int index4 = (double) Player.velocity.Y != 0.0 ? Dust.NewDust(new Vector2(Player.position.X, (float) ((double) Player.position.Y + (double) (Player.height / 2) - 8.0)), Player.width, 16, 31, Alpha: 100, Scale: 1.4f) : Dust.NewDust(new Vector2(Player.position.X, (float) ((double) Player.position.Y + (double) Player.height - 4.0)), Player.width, 8, 31, Alpha: 100, Scale: 1.4f);
        Main.dust[index4].velocity *= 0.1f;
        Main.dust[index4].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
      }
    }
    else if (Player.dash == 2)
    {
      for (int index5 = 0; index5 < 0; ++index5)
      {
        int index6 = (double) Player.velocity.Y != 0.0 ? Dust.NewDust(new Vector2(Player.position.X, (float) ((double) Player.position.Y + (double) (Player.height / 2) - 8.0)), Player.width, 16, 31, Alpha: 100, Scale: 1.4f) : Dust.NewDust(new Vector2(Player.position.X, (float) ((double) Player.position.Y + (double) Player.height - 4.0)), Player.width, 8, 31, Alpha: 100, Scale: 1.4f);
        Main.dust[index6].velocity *= 0.1f;
        Main.dust[index6].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
      }
      num2 = 0.985f;
      num4 = 0.94f;
      num5 = 30;
    }
    else if (Player.dash == 3)
    {
      for (int index7 = 0; index7 < 4; ++index7)
      {
        int index8 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 6, Alpha: 100, Scale: 1.7f);
        Main.dust[index8].velocity *= 0.1f;
        Main.dust[index8].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
        Main.dust[index8].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
        Main.dust[index8].noGravity = true;
        if (Main.rand.Next(2) == 0)
          Main.dust[index8].fadeIn = 0.5f;
      }
      num1 = 14f;
      num2 = 0.985f;
      num4 = 0.94f;
      num5 = 20;
    }
    else if (Player.dash == 4)
    {
      for (int index9 = 0; index9 < 2; ++index9)
      {
        int index10 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y + 4f), Player.width, Player.height - 8, 229, Alpha: 100, Scale: 1.2f);
        Main.dust[index10].velocity *= 0.1f;
        Main.dust[index10].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
        Main.dust[index10].noGravity = true;
        if (Main.rand.Next(2) == 0)
          Main.dust[index10].fadeIn = 0.3f;
      }
      num2 = 0.985f;
      num4 = 0.94f;
      num5 = 20;
    }
    if (Player.dash == 5)
    {
      for (int index11 = 0; index11 < 2; ++index11)
      {
        int Type = (int) Main.rand.NextFromList<short>((short) 68, (short) 69, (short) 70);
        int index12 = (double) Player.velocity.Y != 0.0 ? Dust.NewDust(new Vector2(Player.position.X, (float) ((double) Player.position.Y + (double) (Player.height / 2) - 8.0)), Player.width, 16, Type, Alpha: 100) : Dust.NewDust(new Vector2(Player.position.X, (float) ((double) Player.position.Y + (double) Player.height - 4.0)), Player.width, 8, Type, Alpha: 100);
        Main.dust[index12].velocity *= 0.2f;
        Main.dust[index12].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
        Main.dust[index12].fadeIn = (float) (0.5 + (double) Main.rand.Next(20) * 0.009999999776482582);
        Main.dust[index12].noGravity = true;
        Main.dust[index12].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
      }
    }
    if (Player.dash <= 0)
      return;
    Player.doorHelper.AllowOpeningDoorsByVelocityAloneForATime(num5 * 3);
    Player.vortexStealthActive = false;
    if ((double) Player.velocity.X > (double) num1 || (double) Player.velocity.X < 0.0 - (double) num1)
      Player.velocity.X *= num2;
    else if ((double) Player.velocity.X > (double) num3 || (double) Player.velocity.X < 0.0 - (double) num3)
    {
      Player.velocity.X *= num4;
    }
    else
    {
      Player.dashDelay = num5;
      if ((double) Player.velocity.X < 0.0)
      {
        Player.velocity.X = 0.0f - num3;
      }
      else
      {
        if ((double) Player.velocity.X <= 0.0)
          return;
        Player.velocity.X = num3;
      }
    }
  }
  else
  {
    if (Player.dash <= 0 || Player.mount.Active)
      return;
    if (Player.dash == 1)
    {
      int dir;
      bool dashing;
      DoCommonDashHandle(out dir, out dashing);
      if (dashing)
      {
        Player.velocity.X = 16.9f * (float) dir;
        Point tileCoordinates1 = (Player.Center + new Vector2((float) (dir * Player.width / 2 + 2), (float) ((double) Player.gravDir * (double) -Player.height / 2.0 + (double) Player.gravDir * 2.0))).ToTileCoordinates();
        Point tileCoordinates2 = (Player.Center + new Vector2((float) (dir * Player.width / 2 + 2), 0.0f)).ToTileCoordinates();
        if (WorldGen.SolidOrSlopedTile(tileCoordinates1.X, tileCoordinates1.Y) || WorldGen.SolidOrSlopedTile(tileCoordinates2.X, tileCoordinates2.Y))
          Player.velocity.X /= 2f;
        Player.dashDelay = -1;
        for (int index13 = 0; index13 < 20; ++index13)
        {
          int index14 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, Alpha: 100, Scale: 2f);
          Main.dust[index14].position.X += (float) Main.rand.Next(-5, 6);
          Main.dust[index14].position.Y += (float) Main.rand.Next(-5, 6);
          Main.dust[index14].velocity *= 0.2f;
          Main.dust[index14].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
        }
        int index15 = Gore.NewGore(Player.GetSource_FromThis(), new Vector2((float) ((double) Player.position.X + (double) (Player.width / 2) - 24.0), (float) ((double) Player.position.Y + (double) (Player.height / 2) - 34.0)), new Vector2(), Main.rand.Next(61, 64));
        Main.gore[index15].velocity.X = (float) Main.rand.Next(-50, 51) * 0.01f;
        Main.gore[index15].velocity.Y = (float) Main.rand.Next(-50, 51) * 0.01f;
        Main.gore[index15].velocity *= 0.4f;
        int index16 = Gore.NewGore(Player.GetSource_FromThis(),new Vector2((float) ((double) Player.position.X + (double) (Player.width / 2) - 24.0), (float) ((double) Player.position.Y + (double) (Player.height / 2) - 14.0)), new Vector2(), Main.rand.Next(61, 64));
        Main.gore[index16].velocity.X = (float) Main.rand.Next(-50, 51) * 0.01f;
        Main.gore[index16].velocity.Y = (float) Main.rand.Next(-50, 51) * 0.01f;
        Main.gore[index16].velocity *= 0.4f;
      }
    }
    else if (Player.dash == 2)
    {
      int dir;
      bool dashing;
      DoCommonDashHandle(out dir, out dashing);
      if (dashing)
      {
        Player.velocity.X = 14.5f * (float) dir;
        Point tileCoordinates3 = (Player.Center + new Vector2((float) (dir * Player.width / 2 + 2), (float) ((double) Player.gravDir * (double) -Player.height / 2.0 + (double) Player.gravDir * 2.0))).ToTileCoordinates();
        Point tileCoordinates4 = (Player.Center + new Vector2((float) (dir * Player.width / 2 + 2), 0.0f)).ToTileCoordinates();
        if (WorldGen.SolidOrSlopedTile(tileCoordinates3.X, tileCoordinates3.Y) || WorldGen.SolidOrSlopedTile(tileCoordinates4.X, tileCoordinates4.Y))
          Player.velocity.X /= 2f;
        Player.dashDelay = -1;
        Player.eocDash = 15;
        for (int index17 = 0; index17 < 0; ++index17)
        {
          int index18 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 31, Alpha: 100, Scale: 2f);
          Main.dust[index18].position.X += (float) Main.rand.Next(-5, 6);
          Main.dust[index18].position.Y += (float) Main.rand.Next(-5, 6);
          Main.dust[index18].velocity *= 0.2f;
          Main.dust[index18].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
        }
      }
    }
    else if (Player.dash == 3)
    {
      int dir;
      bool dashing;
      DoCommonDashHandle(out dir, out dashing, new DashStartAction(SolarDashStart));
      if (dashing)
      {
        Player.velocity.X = 21.9f * (float) dir;
        Point tileCoordinates5 = (Player.Center + new Vector2((float) (dir * Player.width / 2 + 2), (float) ((double) Player.gravDir * (double) -Player.height / 2.0 + (double) Player.gravDir * 2.0))).ToTileCoordinates();
        Point tileCoordinates6 = (Player.Center + new Vector2((float) (dir * Player.width / 2 + 2), 0.0f)).ToTileCoordinates();
        if (WorldGen.SolidOrSlopedTile(tileCoordinates5.X, tileCoordinates5.Y) || WorldGen.SolidOrSlopedTile(tileCoordinates6.X, tileCoordinates6.Y))
          Player.velocity.X /= 2f;
        Player.dashDelay = -1;
        for (int index19 = 0; index19 < 20; ++index19)
        {
          int index20 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, 6, Alpha: 100, Scale: 2f);
          Main.dust[index20].position.X += (float) Main.rand.Next(-5, 6);
          Main.dust[index20].position.Y += (float) Main.rand.Next(-5, 6);
          Main.dust[index20].velocity *= 0.2f;
          Main.dust[index20].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
          Main.dust[index20].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
          Main.dust[index20].noGravity = true;
          Main.dust[index20].fadeIn = 0.5f;
        }
      }
    }
    if (Player.dash != 5)
      return;
    int dir1;
    bool dashing1;
    DoCommonDashHandle(out dir1, out dashing1);
    if (!dashing1)
      return;
    Player.velocity.X = 16.9f * (float) dir1;
    Point tileCoordinates7 = (Player.Center + new Vector2((float) (dir1 * Player.width / 2 + 2), (float) ((double) Player.gravDir * (double) -Player.height / 2.0 + (double) Player.gravDir * 2.0))).ToTileCoordinates();
    Point tileCoordinates8 = (Player.Center + new Vector2((float) (dir1 * Player.width / 2 + 2), 0.0f)).ToTileCoordinates();
    if (WorldGen.SolidOrSlopedTile(tileCoordinates7.X, tileCoordinates7.Y) || WorldGen.SolidOrSlopedTile(tileCoordinates8.X, tileCoordinates8.Y))
      Player.velocity.X /= 2f;
    Player.dashDelay = -1;
    for (int index21 = 0; index21 < 20; ++index21)
    {
      int index22 = Dust.NewDust(new Vector2(Player.position.X, Player.position.Y), Player.width, Player.height, (int) Main.rand.NextFromList<short>((short) 68, (short) 69, (short) 70), Alpha: 100, Scale: 1.5f);
      Main.dust[index22].position.X += (float) Main.rand.Next(-5, 6);
      Main.dust[index22].position.Y += (float) Main.rand.Next(-5, 6);
      Main.dust[index22].velocity = Player.DirectionTo(Main.dust[index22].position) * 2f;
      Main.dust[index22].scale *= (float) (1.0 + (double) Main.rand.Next(20) * 0.009999999776482582);
      Main.dust[index22].fadeIn = (float) (0.5 + (double) Main.rand.Next(20) * 0.009999999776482582);
      Main.dust[index22].noGravity = true;
      Main.dust[index22].shader = GameShaders.Armor.GetSecondaryShader(Player.ArmorSetDye(), Player);
    }
  }
}
}
