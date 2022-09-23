/*
	Attack - BDummy ★サンプル
*/

function* <generatorForTask> CreateAttack_BDummy()
{
	for (; ; )
	{
		if (
			GetInput_A() == 1 ||
			GetInput_B() == 1
			)
		{
			break;
		}

		AttackProcPlayer_Move();
		AttackProcPlayer_Fall();

		AttackProcPlayer_Side();
		AttackProcPlayer_Ceiling();
		AttackProcPlayer_Ground();

		AttackProcPlayer_Status();

		var<double> plA = 1.0;

		if (1 <= PlayerInvincibleFrame) // ? 無敵時間中
		{
			plA = 0.5;
		}
		else
		{
			AttackProcPlayer_Atari();
		}

		AddTask(PlayerDrawTasks, function* <generatorForTask> ()
		{
			SetPrint(PlayerX - Camera.X - 90, PlayerY - Camera.Y - 40, 0);
			SetColor("#ffffff");
			SetFSize(16);
			PrintLine("Attack - BDummy テスト");

			Draw(P_PlayerWait, PlayerX - Camera.X, PlayerY - Camera.Y, plA, 0.0, 1.0);
		}());

		yield 1;
	}
}
