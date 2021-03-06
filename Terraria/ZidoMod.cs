﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Terraria
{
	class ZidoMod
	{
        public static bool fullbright = false; //Doneski
        public static float fullbrightLevel = 0.8f; //Doneski
        public static bool noClip = false; //Doneski
        public static bool accuratePlayers = false; //Doneski
        public static bool freeCam = false; //Doneski
        public static bool godMode = false; //Doneski
        public static bool undead = false; //Doneski
        public static bool infiniteMana = false; //Doneski
        public static int tileRange = 4; //Doneski
        public static bool tracking = false; //Doneski
        public static bool infiniteRockets = false; //Doneski
        public static bool slowFall = false; //Doneski
        public static bool waterWalk = false; //Doneski
        public static bool infiniteBreath = false; //Doneski
        public static bool thorns = false; //Doneski
        public static bool gravityControl = false; //Doneski
        public static bool noKnockback = false; //Doneski
        public static float speedHack = 1f; //Doneski
        public static bool autoReuse = false; //Doneski
        public static bool infiniteStack = false; //Doneski
        public static bool infiniteJump = false; //Doneski
        public static int fastUse = 1; //Doneski
        public static bool noAnimateSend = false; //Doneski
        public static bool noProjectileSend = false; //Doneski
        public static bool noMovementSend = false;
        public static bool capNetStats = false; //Doneski
        public static bool forceMaxStack = false; //Doneski
        public static bool GPSDisplay = true; //Doneski
        public static bool flashlight = false; //Doneski
        public static bool showAllRecipes = false; //Doneski
        public static bool freeCrafting = false; //Doneski
        public static bool disableDebuffs = false; //Doneski
        public static bool allowRemoveDebuffs = true; //Doneski
        public static int pickupRange = 38; //Doneski
        public static bool instantRespawn = false; //Doneski
        public static bool maxRespawn = false; //Doneski
        public static bool invisible = false; //Doneski
        public static bool showUI = true; //Doneski
        public static bool showRadar = true; //Doneski
        public static bool superJump = false; //Doneski
        public static bool uberDefense = false; //Doneski
        public static bool bypassNetMode = false; //Doneski
        public static bool useAlternativeSendData = false; //Doneski
        public static bool noFallDmg = false; //Doneski
        public static bool showInvis = false; //Doneski

        public static int mouseMode = 0;
        public static bool mouseReleaseNeeded = true;
        public static int brushType = 0;
        public static int brushSize = 0;
        public static int brushExtra = 0;

        public static int followMode = 0;
        public static int followTarget = 0;
        public static Item[] invisArmor = new Item[11];
        public static Item[] backupInventory = new Item[49];
        public static Item[] backupArmor = new Item[11];
        public static Item[] recoveryInventory = new Item[49];
        public static Item[] recoveryArmor = new Item[11];
        public static Vector2 homeLoc = new Vector2(0, 0);
        public static string lastCommand;

        public static Color GetStatusColor(bool test)
        {
            return new Color(Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor, Main.mouseTextColor);
        }

        public static int GetPlayer(string player)
        {
            player = player.ToLower().Trim();
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].name.ToLower() == player || player == "-" && string.IsNullOrWhiteSpace(Main.player[i].name))
                {
                    return i;
                }
            }
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].name.ToLower().Contains(player))
                {
                    return i;
                }
            }
            return -1;
        }

        public static void OnLeftClick(bool release)
        {
            int tileTargetX = (int)(((float)Main.mouseX + Main.screenPosition.X) / 16f);
            int tileTargetY = (int)(((float)Main.mouseY + Main.screenPosition.Y) / 16f);
            int TargetX = (int)((float)Main.mouseX + Main.screenPosition.X);
            int TargetY = (int)((float)Main.mouseY + Main.screenPosition.Y);

            if (mouseReleaseNeeded && !release)
                return;

            if (mouseMode == 1)
            {
                int index = Projectile.NewProjectile(TargetX, TargetY, -brushSize, 0, brushType, 999999, 0.0f, 0xff);
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(0x1b, -1, -1, "", index, 0f, 0f, 0f, 0);
                }
            }
            else if (mouseMode == 2)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.PlaceTile(tx, ty, brushType, true, true);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 1, tx, ty, brushType, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 3)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.PlaceWall(tx, ty, brushType, true);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 3, tx, ty, brushType, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 4)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        Main.tile[tx, ty].liquid = (byte)brushExtra;
                        Main.tile[tx, ty].lava = (brushType == 1 ? true : false);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.sendWater(tx, ty);
                            else
                            {
                                bypassNetMode = true;
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                                bypassNetMode = false;
                            }
                        }
                    }
                }
            }
            else if (mouseMode == 5)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.KillTile(tx, ty, false, false, true);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 4, tx, ty, 0, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 6)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.KillWall(tx, ty);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 2, tx, ty, 0, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 7)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        Main.tile[tx, ty].liquid = 0;
                        Main.tile[tx, ty].lava = false;
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.sendWater(tx, ty);
                            else
                            {
                                bypassNetMode = true;
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                                bypassNetMode = false;
                            }
                        }
                    }
                }
            }
            else if (mouseMode == 8)
            {
                int index5 = Item.NewItem(TargetX, TargetY, Main.player[Main.myPlayer].width, Main.player[Main.myPlayer].height, brushType, brushSize, false, 0);
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(21, -1, -1, "", index5, 0f, 0f, 0f, 0);
                }
            }
            else if (mouseMode == 9)
            {
                Main.player[Main.myPlayer].position = new Vector2(TargetX, TargetY);
            }
            return;
        }

        public static void OnRightClick(bool release)
        {
            int tileTargetX = (int)(((float)Main.mouseX + Main.screenPosition.X) / 16f);
            int tileTargetY = (int)(((float)Main.mouseY + Main.screenPosition.Y) / 16f);
            int TargetX = (int)((float)Main.mouseX + Main.screenPosition.X);
            int TargetY = (int)((float)Main.mouseY + Main.screenPosition.Y);

            if (mouseReleaseNeeded && !release)
                return;

            if (mouseMode == 1)
            {
                int index = Projectile.NewProjectile(TargetX, TargetY, brushSize, 0, brushType, 999999, 0.0f, 0xff);
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(0x1b, -1, -1, "", index, 0f, 0f, 0f, 0);
                }
            }
            else if (mouseMode == 2)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.PlaceTile(tx, ty, brushType, true, true);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 1, tx, ty, brushType, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 3)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.PlaceWall(tx, ty, brushType, true);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 3, tx, ty, brushType, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 4)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        Main.tile[tx, ty].liquid = (byte)brushExtra;
                        Main.tile[tx, ty].lava = (brushType == 1 ? true : false);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.sendWater(tx, ty);
                            else
                            {
                                bypassNetMode = true;
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                                bypassNetMode = false;
                            }
                        }
                    }
                }
            }
            else if (mouseMode == 5)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.KillTile(tx, ty, false, false, true);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 4, tx, ty, 0, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 6)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        WorldGen.KillWall(tx, ty);
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.SendData(0x11, -1, -1, "", 2, tx, ty, 0, 0);
                            else
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                        }
                    }
                }
            }
            else if (mouseMode == 7)
            {
                for (int x = -brushSize; x <= brushSize; x++)
                {
                    for (int y = -brushSize; y <= brushSize; y++)
                    {
                        int tx = tileTargetX + x;
                        int ty = tileTargetY + y;
                        Main.tile[tx, ty].liquid = 0;
                        Main.tile[tx, ty].lava = false;
                        if (Main.netMode == 1)
                        {
                            if (useAlternativeSendData)
                                NetMessage.sendWater(tx, ty);
                            else
                            {
                                bypassNetMode = true;
                                NetMessage.SendTileSquare(-1, tx, ty, 1);
                                bypassNetMode = false;
                            }
                        }
                    }
                }
            }
            else if (mouseMode == 8)
            {
                int index5 = Item.NewItem(TargetX, TargetY, Main.player[Main.myPlayer].width, Main.player[Main.myPlayer].height, brushType, brushSize, false, 0);
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(21, -1, -1, "", index5, 0f, 0f, 0f, 0);
                }
            }
            else if (mouseMode == 9)
            {
                Main.player[Main.myPlayer].position = new Vector2(TargetX, TargetY);
            }
            return;
        }

        public static bool OnCommand(string cmd, string[] args, int length, string full)
        {
            try
            {
                switch (cmd)
                {

                    case "tshock":
                    case "usealt":
                        useAlternativeSendData = !useAlternativeSendData;
                        return true;

                    case "ui":
                    case "gui":
                        showUI = !showUI;
                        return true;

                    case "fullbright":
                    case "brightness":
                    case "fb":
                        if (length == 2)
                            fullbrightLevel = Convert.ToSingle(args[1]);
                        else
                            fullbright = !fullbright;
                        return true;

                    case "noclip":
                    case "nc":
                        noClip = !noClip;
                        if (noClip)
                            freeCam = false;
                        return true;

                    case "accurate":
                    case "accurateplayers":
                        accuratePlayers = !accuratePlayers;
                        return true;

                    case "freecam":
                    case "outofbody":
                        freeCam = !freeCam;
                        if (freeCam)
                            noClip = false;
                        return true;

                    case "god":
                    case "g":
                        godMode = !godMode;
                        return true;

                    case "undead":
                    case "nodie":
                    case "nodeath":
                        undead = !undead;
                        return true;

                    case "infmana":
                    case "infinitemana":
                        infiniteMana = !infiniteMana;
                        return true;

                    case "range":
                    case "itemrange":
                    case "hitrange":
                    case "tilerange":
                        if (length == 2)
                            tileRange = Convert.ToInt16(args[1]);
                        else if (tileRange > 4)
                            tileRange = 4;
                        else
                            tileRange = 9999;
                        return true;

                    case "pickup":
                    case "pickuprange":
                        if (length == 2)
                            pickupRange = Convert.ToInt16(args[1]);
                        else if (pickupRange > 38)
                            pickupRange = 38;
                        else
                            pickupRange = 9999;
                        return true;

                    case "track":
                    case "tracking":
                        tracking = !tracking;
                        return true;

                    case "radar":
                    case "showradar":
                        showRadar = !showRadar;
                        return true;

                    case "showinvis":
                    case "noinvis":
                        showInvis = !showInvis;
                        return true;

                    case "infrockets":
                    case "infboots":
                    case "infwings":
                        infiniteRockets = !infiniteRockets;
                        return true;

                    case "slowfall":
                        slowFall = !slowFall;
                        return true;

                    case "waterwalk":
                    case "lavawalk":
                        waterWalk = !waterWalk;
                        return true;

                    case "infbreath":
                    case "breath":
                        infiniteBreath = !infiniteBreath;
                        return true;

                    case "thorns":
                        thorns = !thorns;
                        return true;

                    case "gravctrl":
                    case "grav":
                    case "gravity":
                        if (noClip) return false;
                        gravityControl = !gravityControl;
                        return true;

                    case "knockback":
                    case "noknockback":
                        noKnockback = !noKnockback;
                        return true;

                    case "speed":
                    case "speedhack":
                    case "sh":
                        if (length == 2)
                            speedHack = Convert.ToSingle(args[1]);
                        else if (speedHack > 1)
                            speedHack = 1.0f;
                        else
                            speedHack = 3.0f;
                        return true;

                    case "reuse":
                    case "autoreuse":
                        autoReuse = !autoReuse;
                        return true;

                    case "infammo":
                        infiniteStack = !infiniteStack;
                        return true;

                    case "infjump":
                        infiniteJump = !infiniteJump;
                        return true;

                    case "fastuse":
                    case "usespeed":
                    case "rapidfire":
                        if (length == 2)
                            fastUse = Convert.ToInt16(args[1]);
                        else if (fastUse > 1)
                            fastUse = 1;
                        else
                            fastUse = 30;
                        return true;

                    case "noanimate":
                        noAnimateSend = !noAnimateSend;
                        return true;

                    case "noprojectile":
                        noProjectileSend = !noProjectileSend;
                        return true;

                    case "capstats":
                    case "fakehealth":
                        capNetStats = !capNetStats;
                        return true;

                    case "maxstack":
                    case "infstack":
                        forceMaxStack = !forceMaxStack;
                        return true;

                    case "gps":
                    case "pos":
                        GPSDisplay = !GPSDisplay;
                        return true;

                    case "light":
                    case "flashlight":
                        flashlight = !flashlight;
                        return true;

                    case "nodebuff":
                    case "disabledebuff":
                        disableDebuffs = !disableDebuffs;
                        return true;

                    case "allowdelbuff":
                        allowRemoveDebuffs = !allowRemoveDebuffs;
                        return true;

                    case "maxrespawn":
                    case "fullrespawn":
                        maxRespawn = !maxRespawn;
                        return true;

                    case "instantspawn":
                    case "instantrespawn":
                        instantRespawn = !instantRespawn;
                        return true;

                    case "nofall":
                    case "nofalldmg":
                        noFallDmg = !noFallDmg;
                        return true;

                    case "showrecipes":
                        showAllRecipes = !showAllRecipes;
                        return true;

                    case "uberdef":
                    case "uberdefense":
                        uberDefense = !uberDefense;
                        return true;

                    case "superjump":
                    case "sjump":
                    case "jump":
                        superJump = !superJump;
                        return true;

                    case "fastmouse":
                    case "mouserelease":
                        mouseReleaseNeeded = !mouseReleaseNeeded;
                        return true;

                    case "freecrafting":
                        freeCrafting = !freeCrafting;
                        Recipe.numRecipes = 0;
                        for (int i = 0; i < Recipe.maxRecipes; i++)
                        {
                            Main.recipe[i] = new Recipe();
                            Main.availableRecipeY[i] = 0x41 * i;
                        }
                        Recipe.SetupRecipes();
                        return true;

                    case "invis":
                    case "invisible":
                        invisible = !invisible;
                        return true;

                    case "shoot":
                        int shoot;
                        if (int.TryParse(args[1], out shoot))
                        {
                            Main.NewText("Shoot set: " + Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].shoot, 255, 240, 20);
                            return true;
                        }
                        else
                            return false;

                    case "ammo":
                        int ammo;
                        if (int.TryParse(args[1], out ammo))
                        {
                            Main.NewText("Ammo set: " + Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].ammo, 255, 240, 20);
                            return true;
                        }
                        else
                            return false;

                    case "shootSpeed":
                        int shootSpeed;
                        if (int.TryParse(args[1], out shootSpeed))
                        {
                            Main.NewText("ShootSpeed set: " + Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].shootSpeed, 255, 240, 20);
                            return true;
                        }
                        else
                            return false;

                    case "damage":
                        int damage;
                        if (int.TryParse(args[1], out damage))
                        {
                            Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].damage = damage;
                            Main.NewText("Damage set: " + Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].damage, 255, 240, 20);
                            return true;
                        }
                        else
                            return false;

                    case "camfollow":
                    case "watch":
                        if (length <= 1)
                        {
                            freeCam = false;
                            followMode = 0;
                            return false;
                        }
                        int cftarget = GetPlayer(full.Substring(full.IndexOf(' ')));
                        if (cftarget < 0)
                            return false;
                        freeCam = true;
                        followMode = 1;
                        followTarget = cftarget;
                        Main.NewText("Watching " + Main.player[cftarget].name, 255, 240, 20);
                        return true;

                    case "stalk":
                    case "follow":
                        if (length <= 1)
                        {
                            noClip = false;
                            followMode = 0;
                            return false;
                        }
                        int ftarget = GetPlayer(full.Substring(full.IndexOf(' ')));
                        if (ftarget < 0)
                            return false;
                        noClip = true;
                        followMode = 2;
                        followTarget = ftarget;
                        Main.NewText("Stalking " + Main.player[ftarget].name, 255, 240, 20);
                        return true;

                    case "camto":
                        if (length <= 1)
                            return false;
                        int camtarget = GetPlayer(full.Substring(full.IndexOf(' ')));
                        if (camtarget < 0)
                            return false;
                        Main.player[Main.myPlayer].position = Main.player[camtarget].position;
                        Main.NewText("Freecammed to " + Main.player[camtarget].name, 255, 240, 20);
                        return true;

                    case "tile":
                    case "placetile":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int tileType;
                        int tileSize = 1;
                        if (int.TryParse(args[1], out tileType))
                            brushType = tileType;
                        else
                            return false;
                        if (length >= 3 && int.TryParse(args[2], out tileSize))
                            brushSize = tileSize;
                        else
                            brushSize = 1;
                        mouseMode = 2;
                        Main.NewText("Tile brush enabled: " + tileType.ToString() + " (" + tileSize.ToString() + ")", 255, 240, 20);
                        return true;

                    case "wall":
                    case "placewall":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int wallType;
                        int wallSize = 1;
                        if (int.TryParse(args[1], out wallType))
                            brushType = wallType;
                        else
                            return false;
                        if (length >= 3 && int.TryParse(args[2], out wallSize))
                            brushSize = wallSize;
                        else
                            brushSize = 1;
                        mouseMode = 3;
                        Main.NewText("Wall brush enabled: " + wallType.ToString() + " (" + wallSize.ToString() + ")", 255, 240, 20);
                        return true;

                    case "liquid":
                    case "placeliquid":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int liquidType;
                        int liquidSize = 1;
                        byte liquidAmount = 255;
                        if (int.TryParse(args[1], out liquidType))
                            brushType = liquidType;
                        else
                            return false;
                        if (length >= 3 && int.TryParse(args[2], out liquidSize))
                            brushSize = liquidSize;
                        else
                            brushSize = 1;
                        if (length >= 4 && byte.TryParse(args[3], out liquidAmount))
                            brushExtra = liquidAmount;
                        else
                            liquidAmount = 255;
                        mouseMode = 4;
                        Main.NewText("Liquid brush enabled: " + liquidType.ToString() + " (" + liquidSize.ToString() + ") (" + liquidAmount.ToString() + ")", 255, 240, 20);
                        return true;

                    case "projectile":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int projectile;
                        int count = 1;
                        if (int.TryParse(args[1], out projectile))
                            brushType = projectile;
                        else
                            return false;
                        if (length >= 3 && int.TryParse(args[2], out count))
                            brushSize = count;
                        else
                            brushSize = 1;
                        mouseMode = 1;
                        Main.NewText("Projectile brush enabled: " + projectile.ToString() + " (" + count.ToString() + ")", 255, 240, 20);
                        return true;

                    case "drop":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int itemType;
                        int itemStack = 1;
                        if (int.TryParse(args[1], out itemType))
                            brushType = itemType;
                        else
                            return false;
                        if (length >= 3 && int.TryParse(args[2], out itemStack))
                            brushSize = itemStack;
                        else
                            brushSize = 1;
                        mouseMode = 8;
                        Main.NewText("Item drop brush enabled: " + itemType.ToString() + " (" + itemStack.ToString() + ")", 255, 240, 20);
                        return true;

                    case "tpmouse":
                    case "mousetp":
                        if (mouseMode != 9)
                            mouseMode = 9;
                        else
                            mouseMode = 0;
                        return true;

                    case "resetmouse":
                    case "mousereset":
                        mouseMode = 0;
                        return true;

                    case "removetile":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int tsize = 1;
                        if (int.TryParse(args[1], out tsize))
                            brushSize = tsize;
                        else
                            brushSize = 1;
                        mouseMode = 5;
                        Main.NewText("Remove tile brush enabled: " + tsize.ToString(), 255, 240, 20);
                        return true;

                    case "removewall":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int wsize =1;
                        if (int.TryParse(args[1], out wsize))
                            brushSize = wsize;
                        else
                            brushSize = 1;
                        mouseMode = 6;
                        Main.NewText("Remove wall brush enabled: " + wsize.ToString(), 255, 240, 20);
                        return true;

                    case "removeliquid":
                        if (length <= 1)
                        {
                            mouseMode = 0;
                            brushType = 0;
                            return true;
                        }
                        int lsize = 1;
                        if (int.TryParse(args[1], out lsize))
                            brushSize = lsize;
                        else
                            brushSize = 1;
                        mouseMode = 7;
                        Main.NewText("Remove liquid brush enabled: " + lsize.ToString(), 255, 240, 20);
                        return true;

                    case "spawn":
                    case "respawn":
                        Main.player[Main.myPlayer].Spawn();
                        Main.NewText("Respawned", 255, 240, 20);
                        return true;

                    case "tp":
                    case "teleport":
                        if (length <= 1)
                            return false;
                        int target = GetPlayer(full.Substring(full.IndexOf(' ')));
                        if (target < 0)
                            return false;
                        Main.player[Main.myPlayer].position = Main.player[target].position;
                        Main.NewText("Teleported to " + Main.player[target].name, 255, 240, 20);
                        return true;

                    case "clear":
                        recoveryInventory = Main.player[Main.myPlayer].inventory;
                        recoveryArmor = Main.player[Main.myPlayer].armor;
                        for (int i = 0; i < Main.player[Main.myPlayer].inventory.Length; i++)
			            {
                            Main.player[Main.myPlayer].inventory[i].SetDefaults("");
                            Main.player[Main.myPlayer].inventory[i].active = false;
                            Main.player[Main.myPlayer].inventory[i].name = "";
                            Main.player[Main.myPlayer].inventory[i].type = 0;
                            Main.player[Main.myPlayer].inventory[i].stack = 0;
                        }
                        Main.NewText("Cleared inventory", 255, 240, 20);
                        return true;

                    case "recover":
                        Main.player[Main.myPlayer].inventory = recoveryInventory;
                        Main.player[Main.myPlayer].armor = recoveryArmor;
                        Main.NewText("Recovering inventory", 255, 240, 20);
                        return true;

                    case "killme":
                        if (length > 1)
                        {
                            Main.player[Main.myPlayer].KillMe(999999, 0, false, full.Substring(full.IndexOf(' ')));
                        }
                        else
                            Main.player[Main.myPlayer].KillMe(999999, 0);
                        Main.NewText("Killed yourself", 255, 240, 20);
                        return true;

                    case "kill":
                        if (length <= 1)
                            return false;
                        int killtarget = GetPlayer(full.Substring(full.IndexOf(' ')));
                        if (killtarget < 0)
                            return false;
                        int index = Projectile.NewProjectile(Main.player[killtarget].position.X, Main.player[killtarget].position.Y, 0, 0, 0x63, 999999, 0.0f, 0xff);
                        if (Main.netMode == 1)
                        {
                            NetMessage.SendData(0x1b, -1, -1, "", index, 0f, 0f, 0f, 0);
                        }
                        Main.NewText("Killed " + Main.player[killtarget].name, 255, 240, 20);
                        return true;

                    case "killall":
                    case "killplrs":
                    case "killplayers":
                        int killplayers = 0;
                        for (int i = 0; i < Main.player.Length; i++)
                        {
                            if (Main.player[i].active && i != Main.myPlayer)
                            {
                                int index2 = Projectile.NewProjectile(Main.player[i].position.X, Main.player[i].position.Y, 0, 0, 0x63, 999999, 0.0f, 0xff);
                                if (Main.netMode == 1)
                                {
                                    NetMessage.SendData(0x1b, -1, -1, "", index2, 0f, 0f, 0f, 0);
                                }
                                killplayers++;
                            }
                        }
                        Main.NewText("Killed " + killplayers + " players", 255, 240, 20);
                        return true;

                    case "killmobs":
                        int killmobs = 0;
                        for (int i = 0; i < Main.npc.Length; i++)
                        {
                            if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].townNPC)
                            {
                                Main.npc[i].StrikeNPC(999999, 90f, 1);
                                if (Main.netMode == 1)
                                {
                                    NetMessage.SendData(0x1c, -1, -1, "", i, 999999, 90f, 1);
                                }
                                killmobs++;
                            }
                        }
                        Main.NewText("Killed " + killmobs + " mobs", 255, 240, 20);
                        return true;

                    case "killnpcs":
                        int killnpc = 0;
                        for (int i = 0; i < Main.npc.Length; i++)
                        {
                            if (Main.npc[i].active)
                            {
                                Main.npc[i].StrikeNPC(999999, 90f, 1);
                                if (Main.netMode == 1)
                                {
                                    NetMessage.SendData(0x1c, -1, -1, "", i, 999999, 90f, 1);
                                }
                                killnpc++;
                            }
                        }
                        Main.NewText("Killed " + killnpc + " npcs", 255, 240, 20);
                        return true;

                    case "backup":
                        backupInventory = Main.player[Main.myPlayer].inventory;
                        backupArmor = Main.player[Main.myPlayer].armor;
                        Main.NewText("Backing up inventory", 255, 240, 20);
                        return true;

                    case "restore":
                        Main.player[Main.myPlayer].inventory = backupInventory;
                        Main.player[Main.myPlayer].armor = backupArmor;
                        Main.NewText("Restoring inventory backup", 255, 240, 20);
                        return true;

                    case "fullstack":
                        for (int k = 0; k < 0x31; k++)
                        {
                            Main.player[Main.myPlayer].inventory[k].stack = Main.player[Main.myPlayer].inventory[k].maxStack;
                        }
                        Main.NewText("Set all stacks to max", 255, 240, 20);
                        return true;

                    case "itemprefix":
                        byte prefix;
                        if (byte.TryParse(args[1], out prefix))
                        {
                            if (prefix > 83)
                                return false;
                            string prefixitem = full.Substring(full.IndexOf(' ', full.IndexOf(' ') + 1)).Trim().ToProper();
                            Main.player[Main.myPlayer].inventory[9].SetDefaults(prefixitem);
                            Main.player[Main.myPlayer].inventory[9].Prefix(prefix);
                            Main.player[Main.myPlayer].inventory[9].stack = Main.player[Main.myPlayer].inventory[9].maxStack;
                            Main.NewText("Created " + Main.player[Main.myPlayer].inventory[9].name, 255, 240, 20);
                            return true;
                        }
                        else
                            return false;

                    case "item":
                        if (length <= 1)
                            return false;
                        int itemId;
                        if (length == 2 && int.TryParse(args[1], out itemId))
                        {
                            Main.player[Main.myPlayer].inventory[9].netDefaults(itemId);
                            Main.player[Main.myPlayer].inventory[9].stack = Main.player[Main.myPlayer].inventory[9].maxStack;
                            Main.NewText("Created " + Main.player[Main.myPlayer].inventory[9].name, 255, 240, 20);
                        }
                        else
                        {
                            string item = full.Substring(full.IndexOf(' ')).Trim().ToProper();
                            Main.player[Main.myPlayer].inventory[9].SetDefaults(item);
                            Main.player[Main.myPlayer].inventory[9].stack = Main.player[Main.myPlayer].inventory[9].maxStack;
                            Main.NewText("Created " + Main.player[Main.myPlayer].inventory[9].name, 255, 240, 20);
                        }
                        return true;

                    case "chest":
                        int chest;
                        if (int.TryParse(args[1], out chest))
                        {
                            Main.player[Main.myPlayer].chest = chest;
                            Main.NewText("Open chest changed", 255, 240, 20);
                            return true;
                        }
                        else
                            return false;

                    case "home":
                        if(homeLoc.X != 0 && homeLoc.Y != 0)
                        {
                            Main.player[Main.myPlayer].position = homeLoc;
                            Main.NewText("Going home", 255, 240, 20);
                            return true;
                        }
                        else 
                            return false;

                    case "sethome":
                        homeLoc = Main.player[Main.myPlayer].position;
                        Main.NewText("Set home location", 255, 240, 20);
                        return true;

                    case "steal":
                        if (length <= 1)
                            return false;
                        recoveryInventory = Main.player[Main.myPlayer].inventory;
                        recoveryArmor = Main.player[Main.myPlayer].armor;
                        int clonetarget = GetPlayer(full.Substring(full.IndexOf(' ')));
                        if (clonetarget < 0)
                            return false;
                        Main.player[Main.myPlayer].armor = Main.player[clonetarget].armor;
                        Main.player[Main.myPlayer].inventory = Main.player[clonetarget].inventory;
                        Main.NewText("Copied " + Main.player[clonetarget].name + " inventory", 255, 240, 20);
                        return true;

                    case "healplrs":
                    case "healplayers":
                    case "healall":
                        int healplayers = 0;
                        for (int i = 0; i < Main.player.Length; i++)
                        {
                            if (Main.player[i].active && i != Main.myPlayer)
                            {
                                for (int j = 0; j < 20; j++)
                                {
                                    int index3 = Item.NewItem((int)Main.player[i].position.X, (int)Main.player[i].position.Y, Main.player[i].width, Main.player[i].height, 0x3a, 1, false, 0);
                                    if (Main.netMode == 1)
                                    {
                                        NetMessage.SendData(21, -1, -1, "", index3, 0f, 0f, 0f, 0);
                                    }
                                }
                                healplayers++;
                            }
                        }
                        Main.NewText("Healed " + healplayers + " players", 255, 240, 20);
                        return true;

                    case "manaplrs":
                    case "manaplayers":
                    case "manaall":
                        int manaplayers = 0;
                        for (int i = 0; i < Main.player.Length; i++)
                        {
                            if (Main.player[i].active && i != Main.myPlayer)
                            {
                                for (int j = 0; j < 10; j++)
                                {
                                    int index4 = Item.NewItem((int)Main.player[i].position.X, (int)Main.player[i].position.Y, Main.player[i].width, Main.player[i].height, 0xb8, 1, false, 0);
                                    if (Main.netMode == 1)
                                    {
                                        NetMessage.SendData(21, -1, -1, "", index4, 0f, 0f, 0f, 0);
                                    }
                                }
                                manaplayers++;
                            }
                        }
                        Main.NewText("Manaed " + manaplayers + " players", 255, 240, 20);
                        return true;

                    case "heal":
                        if (length <= 1)
                        {
                            Main.player[Main.myPlayer].statLife = Main.player[Main.myPlayer].statLifeMax;
                            Main.player[Main.myPlayer].statMana = Main.player[Main.myPlayer].statManaMax;
                            Main.NewText("Healed yourself", 255, 240, 20);
                            return true;
                        }
                        int healtarget = GetPlayer(full.Substring(full.IndexOf(' ')));
                        if (healtarget < 0)
                            return false;
                        for (int i = 0; i < 10; i++)
                        {
                            int index3 = Item.NewItem((int)Main.player[healtarget].position.X, (int)Main.player[healtarget].position.Y, Main.player[healtarget].width, Main.player[healtarget].height, 0x3a, 1, false, 0);
                            int index4 = Item.NewItem((int)Main.player[healtarget].position.X, (int)Main.player[healtarget].position.Y, Main.player[healtarget].width, Main.player[healtarget].height, 0xb8, 1, false, 0);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendData(21, -1, -1, "", index3, 0f, 0f, 0f, 0);
                                NetMessage.SendData(21, -1, -1, "", index4, 0f, 0f, 0f, 0);
                            }
                        }
                        Main.NewText("Healed " + Main.player[healtarget].name, 255, 240, 20);
                        return true;

                    case "give":
                        if (length <= 1)  
                            return false;
                        int gift;
                        if (int.TryParse(args[1], out gift))
                        {
                            int givetarget = GetPlayer(full.Substring(full.IndexOf(' ', full.IndexOf(' ') + 1)));
                            if (givetarget < 0)
                                return false;
                            int index5 = Item.NewItem((int)Main.player[givetarget].position.X, (int)Main.player[givetarget].position.Y, Main.player[givetarget].width, Main.player[givetarget].height, 0x3a, 1, false, 0);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendData(21, -1, -1, "", index5, 0f, 0f, 0f, 0);
                            }
                            Main.NewText("Gave " + Main.player[givetarget].name + " a gift of " + gift.ToString(), 255, 240, 20);
                            return true;
                        }
                        else
                            return false;

                    case "setstats":
                        if (length <= 1)
                        {
                            Main.player[Main.myPlayer].statLifeMax = 400;
                            Main.player[Main.myPlayer].statManaMax = 260;
                            Main.player[Main.myPlayer].statLife = Main.player[Main.myPlayer].statLifeMax;
                            Main.player[Main.myPlayer].statMana = Main.player[Main.myPlayer].statManaMax;
                            Main.NewText("Reset to default max", 255, 240, 20);
                            return true;
                        }
                        else if (length == 2)
                        {
                            int health;
                            if (int.TryParse(args[1], out health))
                            {
                                Main.player[Main.myPlayer].statLifeMax = health;
                                Main.NewText("Health max set to " + health.ToString(), 255, 240, 20);
                                return true;
                            }
                            else
                                return false;
                        }
                        else if (length == 3)
                        {
                            int health;
                            int mana;
                            if (int.TryParse(args[1], out health))
                                return false;
                            if (!int.TryParse(args[2], out mana))
                                return false;
                            Main.player[Main.myPlayer].statLifeMax = health;
                            Main.player[Main.myPlayer].statManaMax = mana;
                            Main.NewText("Health/mana max set to " + health.ToString() + "/" + mana.ToString(), 255, 240, 20);
                            return true;
                        }
                        else 
                            return false;

                    case "repeat":
                        if (cmd == "repeat") return false;
                        if (lastCommand == null) return false;
                        string full2 = lastCommand.Substring(1);
                        string[] args2 = full2.Split(' ');
                        if (!ZidoMod.OnCommand(args2[0].ToLower(), args2, args2.Length, full2))
                        {
                            Main.NewText("Command Failed", 255, 240, 20);
                        }
                        return true;

                    default:
                        return false;
                }
            }
            catch (Exception exception)
            {
                Main.NewText("Command Error: " + exception, 255, 240, 20);
                return false;
            }
        }

        /*
         * Commands:
         * 
         * RainProjectile
         * RainLiquid
         * RainTile
         * RainItems
         * 
         * QuickBars
         * 
         * SpoofInventory
         * SpawnMob (Statues)
         * 
         * CrashServer
         * CrashClients
         * 
         */
    }
}
