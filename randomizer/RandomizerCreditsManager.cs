using System.Collections.Generic;
using Core;
using Game;
using Sein.World;
using UnityEngine;

public static class RandomizerCreditsManager {
	public static void Initialize() 
	{
		CreditsDone = false;
		Credits = new List<KeyValuePair<string, int>>();
		Credits.Add(new KeyValuePair<string, int>(
@"ANCHORTOPPARAMS_20_7.5_2_Ori DE Randomizer (3.0)

Developed by:
Sigmasin
*Eiko*  #Meldon#  @Vulajin@", 9));
		Credits.Add(new KeyValuePair<string, int>(
@"ANCHORTOPPARAMS_20_7.5_2_Major contributions by:
DevilSquirrel

Community Contributions by:
GreeZ  Hydra  Jitaenow  LusTher
Phant  Skulblaka  Terra  xaviershay", 11));
		Credits.Add(new KeyValuePair<string, int>(
@"ANCHORTOPPARAMS_20_10_2_Additional community contributions by:
Athos213    AvengedRuler    Cereberon    Cleanfel
CovertMuffin    Grimelios    iRobin    JHobz
Jitaenow    Kirefel    madinsane    Mattermonkey
RainbowPoogle    Roryrai    UncleRonny   Wingheart
SeaAverage   DireKrow   Eph   shedd", 14));

		Credits.Add(new KeyValuePair<string, int>(
@"ANCHORTOPPARAMS_20_10_2_Ori DE Randomizer inspired by:
Chicken_Supreme's Ori 'remix'
Link to the Past Randomizer", 7));


		Credits.Add(new KeyValuePair<string, int>(
@"ALIGNLEFTANCHORTOPPARAMS_20_10_2_        Ori Randomizer Tournament Champions

		2017
Singles:	Covert_Muffin
Doubles:	That Is Faster (Sigmasin and IMRaziel)

		2018
Singles:	Sigmasin
Doubles:	That Is Still Faster (Sigmasin and IMRaziel)
", 14));


		// Credits.Add(new KeyValuePair<string, int>("In memory of Grandma Irine", 5));
		Credits.Add(new KeyValuePair<string, int>(RandomizerStatsManager.GetStatsPage(0), 50));
		Credits.Add(new KeyValuePair<string, int>(RandomizerStatsManager.GetStatsPage(1), 50));
		Credits.Add(new KeyValuePair<string, int>(RandomizerStatsManager.GetStatsPage(2), 50));
		Credits.Add(new KeyValuePair<string, int>(
@"ANCHORTOPPARAMS_20_12_2_Thanks for playing!
Website: orirando.com
Join the Ori community: orirando.com/discord", 30));

		NextCreditCountdown = 0;
	}

	public static void Tick() {
		NextCreditCountdown--;
		if(Scenes.Manager.CurrentScene.Scene != "creditsScreen") {
			End();
			return;
		}
		if(NextCreditCountdown <= 0) {
		if(Credits.Count == 0) {
			End();
			return;
		}
			KeyValuePair<string, int> nextCredits = Credits[0];
			Credits.RemoveAt(0);
			NextCreditCountdown = nextCredits.Value;
			Randomizer.showCredits(nextCredits.Key, nextCredits.Value);
		}
	}

	public static void End() {
		if(!CreditsDone)
		{
			CreditsDone = true;
			Credits.Clear();
			Randomizer.CreditsActive = false;
		}
	}

	public static int NextCreditCountdown;
	public static bool CreditsDone;
	public static List<KeyValuePair<string, int>> Credits;
}