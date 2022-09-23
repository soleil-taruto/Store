/*
	部屋(ステージ)選択メニュー
*/

function* <generatorForTask> GameProgressMaster()
{
	do
	{
		yield* GameMain();
	}
	while (GameEndReason == GameEndReason_e_RESTART_GAME);
}
