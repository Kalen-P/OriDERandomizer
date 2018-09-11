using System;
using System.Collections.Generic;
using Game;

// Token: 0x02000A0F RID: 2575
public static class RandomizerBonusSkill
{
	// Token: 0x060037F6 RID: 14326 RVA: 0x000E579C File Offset: 0x000E399C
	public static void SwitchBonusSkill()
	{
		if (RandomizerBonusSkill.UnlockedBonusSkills.Count < 1)
		{
			Randomizer.MessageQueue.Enqueue("No bonus skills unlocked!");
			return;
		}
		RandomizerBonusSkill.ActiveBonus = (RandomizerBonusSkill.ActiveBonus + 1) % RandomizerBonusSkill.UnlockedBonusSkills.Count;
		Randomizer.MessageQueue.Enqueue(RandomizerBonusSkill.CurrentBonusName());
	}

	// Token: 0x060037F7 RID: 14327 RVA: 0x000E57EC File Offset: 0x000E39EC
	public static void ActivateBonusSkill()
	{
		if (RandomizerBonusSkill.UnlockedBonusSkills.Count == 0)
		{
			return;
		}
		int item = RandomizerBonusSkill.CurrentBonus();
		switch (item)
		{
		case 101:
			if (Characters.Sein.Energy.Current > 0f)
			{
				float amount = Characters.Sein.Energy.Current * 4f;
				Characters.Sein.Energy.SetCurrent(Characters.Sein.Mortality.Health.Amount / 4f);
				Characters.Sein.Mortality.Health.SetAmount(amount);
				return;
			}
			break;
		case 102:
			if (RandomizerBonusSkill.ActiveDrainSkills.Contains(item))
			{
				RandomizerBonusSkill.ActiveDrainSkills.Remove(item);
				Randomizer.MessageQueue.Enqueue("Gravity Shift off");
				Characters.Sein.PlatformBehaviour.Gravity.BaseSettings.GravityAngle = 0f;
				RandomizerBonusSkill.EnergyDrainRate -= 0.001f;
				return;
			}
			RandomizerBonusSkill.ActiveDrainSkills.Add(item);
			Randomizer.MessageQueue.Enqueue("Gravity Shift on");
			Characters.Sein.PlatformBehaviour.Gravity.BaseSettings.GravityAngle = 180f;
			RandomizerBonusSkill.EnergyDrainRate += 0.001f;
			return;
		case 103:
			if (RandomizerBonusSkill.ActiveDrainSkills.Contains(item))
			{
				RandomizerBonusSkill.ActiveDrainSkills.Remove(item);
				Randomizer.MessageQueue.Enqueue("ExtremeSpeed off");
				Characters.Sein.PlatformBehaviour.LeftRightMovement.Settings.Ground.MaxSpeed = 11.6666f;
				Characters.Sein.PlatformBehaviour.LeftRightMovement.Settings.Air.MaxSpeed = 11.6666f;
				RandomizerBonusSkill.EnergyDrainRate -= 0.001f;
				return;
			}
			RandomizerBonusSkill.ActiveDrainSkills.Add(item);
			Randomizer.MessageQueue.Enqueue("ExtremeSpeed on");
			Characters.Sein.PlatformBehaviour.LeftRightMovement.Settings.Ground.MaxSpeed = 40f;
			Characters.Sein.PlatformBehaviour.LeftRightMovement.Settings.Air.MaxSpeed = 40f;
			RandomizerBonusSkill.EnergyDrainRate += 0.001f;
			return;
		case 104:
			if (Characters.Sein.Energy.Current >= 0.25f)
			{
				Characters.Sein.Energy.SetCurrent(Characters.Sein.Energy.Current - 0.25f);
				Characters.Sein.PlatformBehaviour.PlatformMovement.LocalSpeedY = 8f;
				return;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060037F8 RID: 14328 RVA: 0x000E5A7C File Offset: 0x000E3C7C
	static RandomizerBonusSkill()
	{
		RandomizerBonusSkill.UnlockedBonusSkills = new List<int>();
		RandomizerBonusSkill.BonusSkillsLastSave = new List<int>();
		RandomizerBonusSkill.Reset();
	}

	// Token: 0x060037F9 RID: 14329 RVA: 0x000E5AE8 File Offset: 0x000E3CE8
	public static void Update()
	{
		if (RandomizerBonusSkill.EnergyDrainRate > 0f)
		{
			if (RandomizerBonusSkill.EnergyDrainRate > Characters.Sein.Energy.Current)
			{
				RandomizerBonusSkill.EnergyDrainRate = 0f;
				RandomizerBonusSkill.DisableAllPersistant();
				return;
			}
			Characters.Sein.Energy.SetCurrent(Characters.Sein.Energy.Current - RandomizerBonusSkill.EnergyDrainRate);
		}
	}

	// Token: 0x060037FA RID: 14330 RVA: 0x000E5B4C File Offset: 0x000E3D4C
	public static void DisableAllPersistant()
	{
		RandomizerBonusSkill.ActiveDrainSkills.Clear();
		RandomizerBonusSkill.EnergyDrainRate = 0f;
		Characters.Sein.PlatformBehaviour.LeftRightMovement.Settings.Ground.MaxSpeed = 11.6666f;
		Characters.Sein.PlatformBehaviour.LeftRightMovement.Settings.Air.MaxSpeed = 11.6666f;
		Characters.Sein.PlatformBehaviour.Gravity.BaseSettings.GravityAngle = 0f;
	}

	// Token: 0x060037FB RID: 14331 RVA: 0x0002BF6B File Offset: 0x0002A16B
	public static void OnSave()
	{
		RandomizerBonusSkill.BonusSkillsLastSave = new List<int>(RandomizerBonusSkill.UnlockedBonusSkills);
	}

	// Token: 0x060037FC RID: 14332 RVA: 0x0002BF7C File Offset: 0x0002A17C
	public static void OnDeath()
	{
		RandomizerBonusSkill.DisableAllPersistant();
		RandomizerBonusSkill.UnlockedBonusSkills = new List<int>(RandomizerBonusSkill.BonusSkillsLastSave);
	}

	// Token: 0x060037FD RID: 14333 RVA: 0x0002BF92 File Offset: 0x0002A192
	public static int CurrentBonus()
	{
		if (RandomizerBonusSkill.UnlockedBonusSkills.Count > RandomizerBonusSkill.ActiveBonus)
		{
			return RandomizerBonusSkill.UnlockedBonusSkills[RandomizerBonusSkill.ActiveBonus];
		}
		return 0;
	}

	// Token: 0x060037FE RID: 14334 RVA: 0x0002BFB6 File Offset: 0x0002A1B6
	public static string CurrentBonusName()
	{
		if (RandomizerBonusSkill.UnlockedBonusSkills.Count > RandomizerBonusSkill.ActiveBonus)
		{
			return RandomizerBonusSkill.BonusSkillNames[RandomizerBonusSkill.CurrentBonus()];
		}
		return "None";
	}

	// Token: 0x060037FF RID: 14335 RVA: 0x000E5BD4 File Offset: 0x000E3DD4
	public static void FoundBonusSkill(int ID)
	{
		Randomizer.showHint(RandomizerBonusSkill.BonusSkillNames[ID]);
		Characters.Sein.Inventory.IncRandomizerItem(ID, 1);
		if (!RandomizerBonusSkill.UnlockedBonusSkills.Contains(ID))
		{
			RandomizerBonusSkill.UnlockedBonusSkills.Add(ID);
			RandomizerBonusSkill.ActiveBonus = RandomizerBonusSkill.UnlockedBonusSkills.Count - 1;
		}
	}

	// Token: 0x06003800 RID: 14336 RVA: 0x000E5C2C File Offset: 0x000E3E2C
	public static void Reset()
	{
		RandomizerBonusSkill.UnlockedBonusSkills = new List<int>();
		RandomizerBonusSkill.BonusSkillsLastSave = new List<int>();
		RandomizerBonusSkill.ActiveDrainSkills = new HashSet<int>();
		RandomizerBonusSkill.EnergyDrainRate = 0f;
		foreach (int num in RandomizerBonusSkill.BonusSkillNames.Keys)
		{
			if (Characters.Sein && num >= 100 && Characters.Sein.Inventory.GetRandomizerItem(num) > 0)
			{
				RandomizerBonusSkill.UnlockedBonusSkills.Add(num);
			}
		}
		RandomizerBonusSkill.ActiveBonus = 0;
	}

	// Token: 0x040032BC RID: 12988
	public static int ActiveBonus = 0;

	// Token: 0x040032BD RID: 12989
	public static List<int> UnlockedBonusSkills;

	// Token: 0x040032BE RID: 12990
	public static float EnergyDrainRate;

	// Token: 0x040032BF RID: 12991
	public static HashSet<int> ActiveDrainSkills;

	// Token: 0x040032C0 RID: 12992
	public static List<int> BonusSkillsLastSave;

	// Token: 0x040032C1 RID: 12993
	public static Dictionary<int, string> BonusSkillNames = new Dictionary<int, string>
	{
		{
			101,
			"Polarity Shift"
		},
		{
			102,
			"Gravity Swap"
		},
		{
			103,
			"ExtremeSpeed"
		},
		{
			104,
			"Energy Jump"
		}
	};
}
