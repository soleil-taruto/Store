/*
	背景
*/

function* <generatorForTask> BackgroundTask()
{
	// ★サンプル -- 要削除
	{
		for (; ; )
		{
			yield* Repeat(1, 60); // 60フレーム待つ。-- ウェイトはこの様に記述する。
		}
	}
}
