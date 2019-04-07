using System;

// Token: 0x02000092 RID: 146
public class ReturnToTitleScreenAction : ActionMethod
{
	// Token: 0x060003A5 RID: 933
	public override void Perform(IContext context)
	{
		Randomizer.Returning = false;
		Randomizer.Warping = 0;
		RandomizerStatsManager.OnReturnToMenu();
		GameController.Instance.RestartGame();
	}
}
