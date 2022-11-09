/*
	テスト-0001
*/

function* <generatorForTask> Test01()
{
	yield* GameMain(0); // テスト用ステージ
//	yield* GameMain(1);
//	yield* GameMain(2);
//	yield* GameMain(3);
}

function* <generatorForTask> Test02()
{
	yield* Ending();
}
