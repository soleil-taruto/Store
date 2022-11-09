/*
	アクター - BDummy ★サンプル
*/

var<int> ActorKind_BDummy = @(AUTO);

function <Actor_t> CreateActor_BDummy(<double> x, <double> y)
{
	var ret =
	{
		Kind: ActorKind_BDummy,
		X: x,
		Y: y,
		Crash: null,
		Killed: false,

		// ここから固有

		<double> Dummy_01: 1.0,
		<double> Dummy_02: 2.0,
		<double> Dummy_03: 3.0,
	};

	ret.Draw = @@_Draw(ret);

	return ret;
}

function* <generatorForTask> @@_Draw(<Actor_t> actor)
{
	for (; ; )
	{
		actor.Y += 2.0;

		if (Map.H * TILE_H < actor.Y)
		{
			break;
		}

		Draw(P_Dummy, actor.X - Camera.X, actor.Y - Camera.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}
