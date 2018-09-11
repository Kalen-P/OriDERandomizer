using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core;
using Game;
using Sein.World;
using UnityEngine;

// Token: 0x020009F5 RID: 2549
public static class Randomizer
{
	// Token: 0x0600374E RID: 14158 RVA: 0x000E1128 File Offset: 0x000DF328
	public static void initialize()
	{
		Randomizer.OHKO = false;
		Randomizer.ZeroXP = false;
		Randomizer.BonusActive = true;
		Randomizer.GiveAbility = false;
		Randomizer.Chaos = false;
		Randomizer.ChaosVerbose = false;
		Randomizer.Returning = false;
		Randomizer.Sync = false;
		Randomizer.ForceMaps = false;
		Randomizer.ForceRandomEscape = false;
		Randomizer.WhichEscape = -1;
		Randomizer.SyncMode = 1;
		Randomizer.StringKeyPickupTypes = new List<string>
		{
			"TP",
			"SH",
			"NO"
		};
		Randomizer.ShareParams = "";
		RandomizerChaosManager.initialize();
		Randomizer.DamageModifier = 1f;
		Randomizer.Table = new Hashtable();
		Randomizer.GridFactor = 4.0;
		Randomizer.Message = "Good luck on your rando!";
		Randomizer.MessageProvider = (RandomizerMessageProvider)ScriptableObject.CreateInstance(typeof(RandomizerMessageProvider));
		Randomizer.ProgressiveMapStones = true;
		Randomizer.ForceTrees = false;
		Randomizer.CluesMode = false;
		Randomizer.SeedMeta = "";
		Randomizer.MistySim = new WorldEvents();
		Randomizer.MistySim.MoonGuid = new MoonGuid(1061758509, 1206015992, 824243626, -2026069462);
		Randomizer.TeleportTable = new Hashtable();
		Randomizer.TeleportTable["Forlorn"] = "forlorn";
		Randomizer.TeleportTable["Grotto"] = "moonGrotto";
		Randomizer.TeleportTable["Sorrow"] = "valleyOfTheWind";
		Randomizer.TeleportTable["Grove"] = "spiritTree";
		Randomizer.TeleportTable["Swamp"] = "swamp";
		Randomizer.TeleportTable["Valley"] = "sorrowPass";
		Randomizer.TeleportTable["Ginso"] = "ginsoTree";
		Randomizer.TeleportTable["Horu"] = "mountHoru";
		Randomizer.Entrance = false;
		Randomizer.DoorTable = new Hashtable();
		Randomizer.ColorShift = false;
		Randomizer.MessageQueue = new Queue();
		Randomizer.MessageQueueTime = 0;
		Randomizer.QueueBash = false;
		Randomizer.BashWasQueued = false;
		Randomizer.BashTap = false;
		Randomizer.fragDungeon = false;
		Randomizer.fragsEnabled = false;
		Randomizer.OpenMode = false;
		RandomizerDataMaps.LoadGinsoData();
		RandomizerDataMaps.LoadForlornData();
		RandomizerDataMaps.LoadHoruData();
		RandomizerRebinding.ParseRebinding();
		RandomizerSettings.ParseSettings();
		if (File.Exists("randomizer.dat"))
		{
			string[] array = File.ReadAllLines("randomizer.dat");
			string[] array2 = array[0].Split(new char[]
			{
				'|'
			});
			string s = array2[1];
			string[] array3 = array2[0].Split(new char[]
			{
				','
			});
			Randomizer.SeedMeta = array[0];
			foreach (string text in array3)
			{
				if (text.ToLower() == "ohko")
				{
					Randomizer.OHKO = true;
				}
				if (text.ToLower().StartsWith("sync"))
				{
					Randomizer.Sync = true;
					Randomizer.SyncId = text.Substring(4);
					RandomizerSyncManager.Initialize();
				}
				if (text.ToLower() == "frags")
				{
					Randomizer.fragDungeon = true;
				}
				if (text.ToLower().StartsWith("frags/"))
				{
					Randomizer.fragsEnabled = true;
					int num = Randomizer.ordHash(s) % 6;
					string[] array5 = text.Split(new char[]
					{
						'/'
					});
					Randomizer.maxFrags = int.Parse(array5[1]);
					Randomizer.fragKey1 = int.Parse(array5[2]);
					Randomizer.fragKey2 = int.Parse(array5[3]);
					Randomizer.fragKey3 = int.Parse(array5[4]);
					Randomizer.fragKeyFinish = int.Parse(array5[5]);
					switch (num)
					{
					case 0:
						Randomizer.fragDungeonOrder = new List<int>
						{
							0,
							1,
							2
						};
						break;
					case 1:
						Randomizer.fragDungeonOrder = new List<int>
						{
							0,
							2,
							1
						};
						break;
					case 2:
						Randomizer.fragDungeonOrder = new List<int>
						{
							1,
							0,
							2
						};
						break;
					case 3:
						Randomizer.fragDungeonOrder = new List<int>
						{
							1,
							2,
							0
						};
						break;
					case 4:
						Randomizer.fragDungeonOrder = new List<int>
						{
							2,
							0,
							1
						};
						break;
					case 5:
						Randomizer.fragDungeonOrder = new List<int>
						{
							2,
							1,
							0
						};
						break;
					}
				}
				if (text.ToLower().StartsWith("mode="))
				{
					string text2 = text.Substring(5).ToLower();
					int syncMode;
					if (text2 == "shared")
					{
						syncMode = 1;
					}
					else if (text2 == "swap")
					{
						syncMode = 2;
					}
					else if (text2 == "split")
					{
						syncMode = 3;
					}
					else if (text2 == "none")
					{
						syncMode = 4;
					}
					else
					{
						syncMode = int.Parse(text2);
					}
					Randomizer.SyncMode = syncMode;
				}
				if (text.ToLower().StartsWith("shared="))
				{
					Randomizer.ShareParams = text.Substring(7);
				}
				if (text.ToLower() == "0xp")
				{
					Randomizer.ZeroXP = true;
				}
				if (text.ToLower() == "nobonus")
				{
					Randomizer.BonusActive = false;
				}
				if (text.ToLower() == "nonprogressivemapstones")
				{
					Randomizer.ProgressiveMapStones = false;
				}
				if (text.ToLower() == "forcetrees")
				{
					Randomizer.ForceTrees = true;
				}
				if (text.ToLower() == "forcemaps")
				{
					Randomizer.ForceMaps = true;
				}
				if (text.ToLower() == "forcerandomescape")
				{
					Randomizer.ForceRandomEscape = true;
				}
				if (text.ToLower() == "clues")
				{
					Randomizer.CluesMode = true;
					RandomizerClues.initialize();
				}
				if (text.ToLower() == "entrance")
				{
					Randomizer.Entrance = true;
				}
				if (text.ToLower() == "open")
				{
					Randomizer.OpenMode = true;
				}
			}
			for (int j = 1; j < array.Length; j++)
			{
				string[] array6 = array[j].Split(new char[]
				{
					'|'
				});
				int num2;
				int.TryParse(array6[0], out num2);
				if (Randomizer.StringKeyPickupTypes.Contains(array6[1]))
				{
					Randomizer.Table[num2] = new RandomizerAction(array6[1], array6[2]);
				}
				else
				{
					int num3;
					int.TryParse(array6[2], out num3);
					if (array6[1] == "EN")
					{
						int num4;
						int.TryParse(array6[3], out num4);
						Randomizer.DoorTable[num2] = new Vector3((float)num3, (float)num4);
					}
					else
					{
						Randomizer.Table[num2] = new RandomizerAction(array6[1], num3);
						if (array6[1] == "EX" && Randomizer.ForceRandomEscape && Randomizer.WhichEscape < 0)
						{
							Randomizer.WhichEscape = num3 % 3;
						}
						if (Randomizer.CluesMode && array6[1] == "EV" && num3 % 2 == 0)
						{
							RandomizerClues.AddClue(array6[3], num3 / 2);
						}
					}
				}
			}
			if (Randomizer.CluesMode)
			{
				RandomizerClues.FinishClues();
			}
		}
		RandomizerBonusSkill.Reset();
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x0002B8AB File Offset: 0x00029AAB
	public static void getPickup()
	{
		Randomizer.getPickup(Characters.Sein.Position);
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x000E183C File Offset: 0x000DFA3C
	public static void returnToStart()
	{
		if (Characters.Sein.Abilities.Carry.IsCarrying || !Characters.Sein.Controller.CanMove || !Characters.Sein.Active)
		{
			return;
		}
		if (Items.NightBerry != null)
		{
			Items.NightBerry.transform.position = new Vector3(-755f, -400f);
		}
		Randomizer.Returning = true;
		Characters.Sein.Position = new Vector3(189f, -215f);
		Characters.Ori.Position = new Vector3(190f, -210f);
		int value = World.Events.Find(Randomizer.MistySim).Value;
		if (value != 1 && value != 8)
		{
			World.Events.Find(Randomizer.MistySim).Value = 10;
		}
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x0002B8BC File Offset: 0x00029ABC
	public static void getEvent(int ID)
	{
		RandomizerBonus.CollectPickup();
		if (Randomizer.ColorShift)
		{
			Randomizer.changeColor();
		}
		RandomizerSwitch.GivePickup((RandomizerAction)Randomizer.Table[ID * 4], ID * 4, true);
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x0002B8EF File Offset: 0x00029AEF
	public static void showHint(string message)
	{
		Randomizer.Message = message;
		Randomizer.MessageQueue.Enqueue(message);
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x0002B902 File Offset: 0x00029B02
	public static void playLastMessage()
	{
		Randomizer.MessageQueue.Enqueue(Randomizer.Message);
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x0002B913 File Offset: 0x00029B13
	public static void log(string message)
	{
		StreamWriter streamWriter = File.AppendText("randomizer.log");
		streamWriter.WriteLine(message);
		streamWriter.Flush();
		streamWriter.Dispose();
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x000E190C File Offset: 0x000DFB0C
	public static bool WindRestored()
	{
		return Sein.World.Events.WindRestored && Scenes.Manager.CurrentScene != null && Scenes.Manager.CurrentScene.Scene != "forlornRuinsResurrection" && Scenes.Manager.CurrentScene.Scene != "forlornRuinsRotatingLaserFlipped";
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x0002B931 File Offset: 0x00029B31
	public static void getSkill()
	{
		Characters.Sein.Inventory.IncRandomizerItem(27, 1);
		Randomizer.getPickup();
		Randomizer.showProgress();
	}

	// Token: 0x06003757 RID: 14167 RVA: 0x000E1964 File Offset: 0x000DFB64
	public static void hintAndLog(float x, float y)
	{
		string message = ((int)x).ToString() + " " + ((int)y).ToString();
		Randomizer.showHint(message);
		Randomizer.log(message);
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x000E199C File Offset: 0x000DFB9C
	public static void getPickup(Vector3 position)
	{
		RandomizerBonus.CollectPickup();
		if (Randomizer.ColorShift)
		{
			Randomizer.changeColor();
		}
		int num = (int)(Math.Floor((double)((int)position.x) / Randomizer.GridFactor) * Randomizer.GridFactor) * 10000 + (int)(Math.Floor((double)((int)position.y) / Randomizer.GridFactor) * Randomizer.GridFactor);
		if (Randomizer.Table.ContainsKey(num))
		{
			RandomizerSwitch.GivePickup((RandomizerAction)Randomizer.Table[num], num, true);
			return;
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (Randomizer.Table.ContainsKey(num + (int)Randomizer.GridFactor * (10000 * i + j)))
				{
					RandomizerSwitch.GivePickup((RandomizerAction)Randomizer.Table[num + (int)Randomizer.GridFactor * (10000 * i + j)], num + (int)Randomizer.GridFactor * (10000 * i + j), true);
					return;
				}
			}
		}
		for (int k = -2; k <= 2; k += 4)
		{
			for (int l = -1; l <= 1; l++)
			{
				if (Randomizer.Table.ContainsKey(num + (int)Randomizer.GridFactor * (10000 * k + l)))
				{
					RandomizerSwitch.GivePickup((RandomizerAction)Randomizer.Table[num + (int)Randomizer.GridFactor * (10000 * k + l)], num + (int)Randomizer.GridFactor * (10000 * k + l), true);
					return;
				}
			}
		}
		Randomizer.showHint("Error finding pickup at " + ((int)position.x).ToString() + ", " + ((int)position.y).ToString());
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x000E1B58 File Offset: 0x000DFD58
	public static void Update()
	{
		Randomizer.UpdateMessages();
		if (Characters.Sein && Randomizer.ForceRandomEscape && Scenes.Manager.CurrentScene != null)
		{
			if (!RandomizerBonus.GinsoEscapeDone() && Scenes.Manager.CurrentScene.Scene == "kuroMomentTreeDuplicate")
			{
				Characters.Sein.Inventory.SetRandomizerItem(300, 1);
				Randomizer.MessageQueue.Enqueue("*Ginso Escape Cleared*");
			}
			if (!RandomizerBonus.ForlornEscapeDone() && Scenes.Manager.CurrentScene.Scene == "forlornRuinsNestC")
			{
				Characters.Sein.Inventory.SetRandomizerItem(301, 1);
				Randomizer.MessageQueue.Enqueue("#Forlorn Escape Cleared#");
			}
		}
		if (Characters.Sein && !Characters.Sein.IsSuspended)
		{
			RandomizerBonus.Update();
			if (Scenes.Manager.CurrentScene != null && Scenes.Manager.CurrentScene.Scene == "catAndMouseResurrectionRoom" && !Randomizer.canFinalEscape())
			{
				if (Randomizer.Entrance)
				{
					Randomizer.EnterDoor(new Vector3(-242f, 489f));
				}
				else
				{
					Characters.Sein.Position = new Vector3(20f, 105f);
				}
			}
			if (Randomizer.Chaos)
			{
				RandomizerChaosManager.Update();
			}
			if (Randomizer.Sync)
			{
				RandomizerSyncManager.Update();
			}
			if (Randomizer.Returning)
			{
				Characters.Sein.Position = new Vector3(189f, -215f);
				if (Scenes.Manager.CurrentScene.Scene == "sunkenGladesRunaway")
				{
					Randomizer.Returning = false;
				}
			}
		}
		if (RandomizerRebinding.BonusSwitch.IsPressed() && Characters.Sein)
		{
			RandomizerBonusSkill.SwitchBonusSkill();
			return;
		}
		if (RandomizerRebinding.BonusToggle.IsPressed() && Characters.Sein)
		{
			RandomizerBonusSkill.ActivateBonusSkill();
			return;
		}
		if (RandomizerRebinding.ReplayMessage.IsPressed())
		{
			Randomizer.playLastMessage();
			return;
		}
		if (RandomizerRebinding.ReturnToStart.IsPressed() && Characters.Sein)
		{
			Randomizer.returnToStart();
			return;
		}
		if (RandomizerRebinding.ReloadSeed.IsPressed())
		{
			Randomizer.initialize();
			Randomizer.showSeedInfo();
			return;
		}
		if (RandomizerRebinding.ShowProgress.IsPressed() && Characters.Sein)
		{
			Randomizer.showProgress();
			return;
		}
		if (RandomizerRebinding.ColorShift.IsPressed())
		{
			string obj = "Color shift enabled";
			if (Randomizer.ColorShift)
			{
				obj = "Color shift disabled";
			}
			Randomizer.ColorShift = !Randomizer.ColorShift;
			Randomizer.MessageQueue.Enqueue(obj);
		}
		if (RandomizerRebinding.ToggleChaos.IsPressed() && Characters.Sein)
		{
			if (Randomizer.Chaos)
			{
				Randomizer.showChaosMessage("Chaos deactivated");
				Randomizer.Chaos = false;
				RandomizerChaosManager.ClearEffects();
				return;
			}
			Randomizer.showChaosMessage("Chaos activated");
			Randomizer.Chaos = true;
			return;
		}
		else if (RandomizerRebinding.ChaosVerbosity.IsPressed() && Randomizer.Chaos)
		{
			Randomizer.ChaosVerbose = !Randomizer.ChaosVerbose;
			if (Randomizer.ChaosVerbose)
			{
				Randomizer.showChaosMessage("Chaos messages enabled");
				return;
			}
			Randomizer.showChaosMessage("Chaos messages disabled");
			return;
		}
		else
		{
			if (RandomizerRebinding.ForceChaosEffect.IsPressed() && Randomizer.Chaos && Characters.Sein)
			{
				RandomizerChaosManager.SpawnEffect();
				return;
			}
			return;
		}
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x0002B94F File Offset: 0x00029B4F
	public static void showChaosEffect(string message)
	{
		if (Randomizer.ChaosVerbose)
		{
			Randomizer.MessageQueue.Enqueue(message);
		}
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x0002B963 File Offset: 0x00029B63
	public static void showChaosMessage(string message)
	{
		Randomizer.MessageQueue.Enqueue(message);
	}

	// Token: 0x0600375C RID: 14172 RVA: 0x000E1E80 File Offset: 0x000E0080
	public static void getMapStone()
	{
		if (!Randomizer.ProgressiveMapStones)
		{
			Randomizer.getPickup();
			return;
		}
		RandomizerBonus.CollectMapstone();
		if (Randomizer.ColorShift)
		{
			Randomizer.changeColor();
		}
		RandomizerSwitch.GivePickup((RandomizerAction)Randomizer.Table[20 + RandomizerBonus.MapStoneProgression() * 4], 20 + RandomizerBonus.MapStoneProgression() * 4, true);
	}

	// Token: 0x0600375D RID: 14173 RVA: 0x000E1EDC File Offset: 0x000E00DC
	public static void showProgress()
	{
		string text = "";
		if (RandomizerBonus.SkillTreeProgression() == 10 && Randomizer.ForceTrees)
		{
			text += "$Trees (10/10)$  ";
		}
		else
		{
			text = text + "Trees (" + RandomizerBonus.SkillTreeProgression().ToString() + "/10)  ";
		}
		if (RandomizerBonus.MapStoneProgression() == 9 && Randomizer.ForceMaps)
		{
			text += "$Maps (9/9)$  ";
		}
		else
		{
			text = text + "Maps (" + RandomizerBonus.MapStoneProgression().ToString() + "/9)  ";
		}
		if (Randomizer.ForceRandomEscape)
		{
			if (Randomizer.WhichEscape == 0)
			{
				if (RandomizerBonus.GinsoEscapeDone())
				{
					text += "$Escape: Ginso$ ";
				}
				else
				{
					text += "Escape: Ginso ";
				}
			}
			else if (Randomizer.WhichEscape == 1)
			{
				if (RandomizerBonus.ForlornEscapeDone())
				{
					text += "$Escape: Forlorn$ ";
				}
				else
				{
					text += "Escape: Forlorn ";
				}
			}
			else
			{
				text += "Escape: Horu ";
			}
		}
		text = text + "Total (" + RandomizerBonus.GetPickupCount().ToString() + "/248)\n";
		if (Randomizer.CluesMode)
		{
			text += RandomizerClues.GetClues();
		}
		else
		{
			if (Keys.GinsoTree)
			{
				text += "*WV (3/3)*  ";
			}
			else
			{
				text = text + " *WV* (" + RandomizerBonus.WaterVeinShards().ToString() + "/3)  ";
			}
			if (Keys.ForlornRuins)
			{
				text += "#GS (3/3)#  ";
			}
			else
			{
				text = text + "#GS# (" + RandomizerBonus.GumonSealShards().ToString() + "/3)  ";
			}
			if (Keys.MountHoru)
			{
				text += "@SS (3/3)@";
			}
			else
			{
				text = text + " @SS@ (" + RandomizerBonus.SunstoneShards().ToString() + "/3)";
			}
			if (Randomizer.fragsEnabled)
			{
				text = string.Concat(new string[]
				{
					text,
					" Frags: (",
					RandomizerBonus.WarmthFrags().ToString(),
					"/",
					Randomizer.fragKeyFinish.ToString(),
					")"
				});
			}
		}
		Randomizer.MessageQueue.Enqueue(text);
	}

	// Token: 0x0600375E RID: 14174 RVA: 0x000E20F8 File Offset: 0x000E02F8
	public static void showSeedInfo()
	{
		string obj = "v3.0b - seed loaded: " + Randomizer.SeedMeta;
		Randomizer.MessageQueue.Enqueue(obj);
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x000E2120 File Offset: 0x000E0320
	public static void changeColor()
	{
		if (Characters.Sein)
		{
			Characters.Sein.PlatformBehaviour.Visuals.SpriteRenderer.material.color = new Color(FixedRandom.Values[0], FixedRandom.Values[1], FixedRandom.Values[2], 0.5f);
		}
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x000E2178 File Offset: 0x000E0378
	public static void UpdateMessages()
	{
		if (Randomizer.MessageQueueTime == 0)
		{
			if (Randomizer.MessageQueue.Count == 0)
			{
				return;
			}
			Randomizer.MessageProvider.SetMessage((string)Randomizer.MessageQueue.Dequeue());
			UI.Hints.Show(Randomizer.MessageProvider, HintLayer.GameSaved, 3f);
			Randomizer.MessageQueueTime = 60;
		}
		Randomizer.MessageQueueTime--;
	}

	// Token: 0x06003761 RID: 14177 RVA: 0x0002B970 File Offset: 0x00029B70
	public static void OnDeath()
	{
		if (Randomizer.Sync)
		{
			RandomizerSyncManager.onDeath();
		}
		Characters.Sein.Inventory.OnDeath();
		RandomizerBonusSkill.OnDeath();
	}

	// Token: 0x06003762 RID: 14178 RVA: 0x0002B992 File Offset: 0x00029B92
	public static void OnSave()
	{
		if (Randomizer.Sync)
		{
			RandomizerSyncManager.onSave();
		}
		Characters.Sein.Inventory.OnSave();
		RandomizerBonusSkill.OnSave();
	}

	// Token: 0x06003763 RID: 14179 RVA: 0x000E21D8 File Offset: 0x000E03D8
	public static bool canFinalEscape()
	{
		if (Randomizer.fragsEnabled && RandomizerBonus.WarmthFrags() < Randomizer.fragKeyFinish)
		{
			Randomizer.MessageQueue.Enqueue(string.Concat(new string[]
			{
				"Frags: (",
				RandomizerBonus.WarmthFrags().ToString(),
				"/",
				Randomizer.fragKeyFinish.ToString(),
				")"
			}));
			return false;
		}
		if (Randomizer.ForceTrees && RandomizerBonus.SkillTreeProgression() < 10)
		{
			Randomizer.MessageQueue.Enqueue("Trees (" + RandomizerBonus.SkillTreeProgression().ToString() + "/10)");
			return false;
		}
		if (Randomizer.ForceMaps && RandomizerBonus.MapStoneProgression() < 9)
		{
			Randomizer.MessageQueue.Enqueue("Maps (" + RandomizerBonus.MapStoneProgression().ToString() + "/9)");
			return false;
		}
		if (Randomizer.ForceRandomEscape)
		{
			if (Randomizer.WhichEscape == 0 && !RandomizerBonus.GinsoEscapeDone())
			{
				Randomizer.MessageQueue.Enqueue("*Do Ginso Escape*");
				return false;
			}
			if (Randomizer.WhichEscape == 1 && !RandomizerBonus.ForlornEscapeDone())
			{
				Randomizer.MessageQueue.Enqueue("#Do Forlorn Escape#");
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003764 RID: 14180 RVA: 0x000E22FC File Offset: 0x000E04FC
	public static void EnterDoor(Vector3 position)
	{
		if (!Randomizer.Entrance)
		{
			return;
		}
		int num = (int)(Math.Floor((double)((int)position.x) / Randomizer.GridFactor) * Randomizer.GridFactor) * 10000 + (int)(Math.Floor((double)((int)position.y) / Randomizer.GridFactor) * Randomizer.GridFactor);
		if (Randomizer.DoorTable.ContainsKey(num))
		{
			Characters.Sein.Position = (Vector3)Randomizer.DoorTable[num];
			return;
		}
		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				if (Randomizer.DoorTable.ContainsKey(num + (int)Randomizer.GridFactor * (10000 * i + j)))
				{
					Characters.Sein.Position = (Vector3)Randomizer.DoorTable[num + (int)Randomizer.GridFactor * (10000 * i + j)];
					return;
				}
			}
		}
		for (int k = -2; k <= 2; k += 4)
		{
			for (int l = -1; l <= 1; l++)
			{
				if (Randomizer.DoorTable.ContainsKey(num + (int)Randomizer.GridFactor * (10000 * k + l)))
				{
					Characters.Sein.Position = (Vector3)Randomizer.DoorTable[num + (int)Randomizer.GridFactor * (10000 * k + l)];
					return;
				}
			}
		}
		Randomizer.showHint("Error using door at " + ((int)position.x).ToString() + ", " + ((int)position.y).ToString());
	}

	// Token: 0x06003765 RID: 14181 RVA: 0x000E2494 File Offset: 0x000E0694
	public static void setTree(int tree)
	{
		int num = tree + 6;
		if ((Characters.Sein.Inventory.SkillPointsCollected >> num) % 2 == 0)
		{
			int skillPointsCollected = Characters.Sein.Inventory.SkillPointsCollected + (1 << num);
			while ((Characters.Sein.Inventory.SkillPointsCollected >> num) % 2 == 0)
			{
				Characters.Sein.Inventory.SkillPointsCollected = skillPointsCollected;
			}
		}
	}

	// Token: 0x06003766 RID: 14182 RVA: 0x0002B9B4 File Offset: 0x00029BB4
	public static void getSkill(int tree)
	{
		Randomizer.setTree(tree);
		Randomizer.getSkill();
	}

	// Token: 0x06003767 RID: 14183 RVA: 0x000E2500 File Offset: 0x000E0700
	public static int ordHash(string s)
	{
		int num = 0;
		foreach (char c in s)
		{
			num += (int)c;
		}
		return num;
	}

	// Token: 0x0400322E RID: 12846
	public static Hashtable Table;

	// Token: 0x0400322F RID: 12847
	public static bool GiveAbility;

	// Token: 0x04003230 RID: 12848
	public static double GridFactor;

	// Token: 0x04003231 RID: 12849
	public static RandomizerMessageProvider MessageProvider;

	// Token: 0x04003232 RID: 12850
	public static bool OHKO;

	// Token: 0x04003233 RID: 12851
	public static bool ZeroXP;

	// Token: 0x04003234 RID: 12852
	public static bool BonusActive;

	// Token: 0x04003235 RID: 12853
	public static string Message;

	// Token: 0x04003236 RID: 12854
	public static bool Chaos;

	// Token: 0x04003237 RID: 12855
	public static bool ChaosVerbose;

	// Token: 0x04003238 RID: 12856
	public static float DamageModifier;

	// Token: 0x04003239 RID: 12857
	public static bool ProgressiveMapStones;

	// Token: 0x0400323A RID: 12858
	public static bool ForceTrees;

	// Token: 0x0400323B RID: 12859
	public static string SeedMeta;

	// Token: 0x0400323C RID: 12860
	public static Hashtable TeleportTable;

	// Token: 0x0400323D RID: 12861
	public static WorldEvents MistySim;

	// Token: 0x0400323E RID: 12862
	public static bool Returning;

	// Token: 0x0400323F RID: 12863
	public static bool CluesMode;

	// Token: 0x04003240 RID: 12864
	public static bool ColorShift;

	// Token: 0x04003241 RID: 12865
	public static Queue MessageQueue;

	// Token: 0x04003242 RID: 12866
	public static int MessageQueueTime;

	// Token: 0x04003243 RID: 12867
	public static bool Sync;

	// Token: 0x04003244 RID: 12868
	public static string SyncId;

	// Token: 0x04003245 RID: 12869
	public static int SyncMode;

	// Token: 0x04003246 RID: 12870
	public static string ShareParams;

	// Token: 0x04003247 RID: 12871
	public static List<string> StringKeyPickupTypes;

	// Token: 0x04003248 RID: 12872
	public static bool ForceMaps;

	// Token: 0x04003249 RID: 12873
	public static bool ForceRandomEscape;

	// Token: 0x0400324A RID: 12874
	public static int WhichEscape;

	// Token: 0x0400324B RID: 12875
	public static bool Entrance;

	// Token: 0x0400324C RID: 12876
	public static Hashtable DoorTable;

	// Token: 0x0400324D RID: 12877
	public static bool QueueBash;

	// Token: 0x0400324E RID: 12878
	public static bool BashWasQueued;

	// Token: 0x0400324F RID: 12879
	public static bool BashTap;

	// Token: 0x04003250 RID: 12880
	public static bool fragDungeon;

	// Token: 0x04003251 RID: 12881
	public static bool fragsEnabled;

	// Token: 0x04003252 RID: 12882
	public static int maxFrags;

	// Token: 0x04003253 RID: 12883
	public static int fragKey1;

	// Token: 0x04003254 RID: 12884
	public static int fragKey2;

	// Token: 0x04003255 RID: 12885
	public static int fragKey3;

	// Token: 0x04003256 RID: 12886
	public static List<int> fragDungeonOrder;

	// Token: 0x04003257 RID: 12887
	public static int fragKeyFinish;

	// Token: 0x04003258 RID: 12888
	public static ArrayList GinsoData;

	// Token: 0x04003259 RID: 12889
	public static ArrayList ForlornData;

	// Token: 0x0400325A RID: 12890
	public static ArrayList HoruData;

	// Token: 0x0400325B RID: 12891
	public static bool OpenMode;
}
