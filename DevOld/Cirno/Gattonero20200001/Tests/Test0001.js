/*
	テスト-0001
*/

function* <generatorForTask> Test01()
{
	yield* GameMain(0); // テスト用
//	yield* GameMain(1); // Stage-01
//	yield* GameMain(2); // Stage-02
//	yield* GameMain(3); // Stage-03
}

function* <generatorForTask> Test02()
{
	yield* Ending();
}
