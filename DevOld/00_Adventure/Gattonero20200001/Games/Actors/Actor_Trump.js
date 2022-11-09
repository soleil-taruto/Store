/*
	アクター - トランプのジョーカー
*/

var<int> ActorKind_Trump = @(AUTO);

function <Actor_t> CreateActor_Trump(<double> x, <double> y, <boolean> reversed)
{
	var ret =
	{
		Kind: ActorKind_Trump,
		X: x,
		Y: y,
		Crash: null,
		Killed: false,

		// ここから固有

		<double> Dest_X: x,
		<double> Dest_Y: y,

		<boolean> Reversed: reversed,
		<generatorForTask> SpecialDraw: ToGenerator([]),
	};

	ret.Draw = @@_Draw(ret);

	return ret;
}

function* <generatorForTask> @@_Draw(<Actor_t> actor)
{
	for (; ; )
	{
		actor.X = Approach(actor.X, actor.Dest_X, 0.97);
		actor.Y = Approach(actor.Y, actor.Dest_Y, 0.97);

		if (!NextVal(actor.SpecialDraw))
		{
			var<Picture_t> picture_01 = P_TrumpFrame;
			var<Picture_t> picture_02;

			if (!actor.Reversed)
			{
				picture_02 = P_TrumpJoker;
			}
			else
			{
				picture_02 = P_TrumpBack;
			}

			Draw(picture_01, actor.X, actor.Y, 1.0, 0.0, 1.0);
			Draw(picture_02, actor.X, actor.Y, 1.0, 0.0, 1.0);
		}

		yield 1;
	}
}

function <void> SetTrumpDest(<Actor_t> actor, <double> x, <double> y)
{
	actor.Dest_X = x;
	actor.Dest_Y = y;
}

function <boolean> IsTrumpReversed(<Actor_t> actor)
{
	return actor.Reversed;
}

function <void> SetTrumpReversed(<Actor_t> actor, <boolean> reversed)
{
	if (actor.Reversed ? !reversed : reversed)
	{
		actor.SpecialDraw = @@_Turn(actor, reversed);
		actor.Reversed = reversed;
	}
}

function* <generatorForTask> @@_Turn(<Actor_t> actor, <boolean> reversed)
{
	for (var<Scene_t> scene of CreateScene(60))
	{
		var<double> wRate = Math.cos(scene.Rate * Math.PI);
		var<boolean> b = reversed;

		if (wRate < 0.0)
		{
			wRate *= -1.0;
			b = !b;
		}

		if (MICRO < wRate)
		{
			var<Picture_t> picture_01 = P_TrumpFrame;
			var<Picture_t> picture_02;

			if (b)
			{
				picture_02 = P_TrumpJoker;
			}
			else
			{
				picture_02 = P_TrumpBack;
			}

			Draw2(picture_01, actor.X, actor.Y, 1.0, 0.0, wRate, 1.0);
			Draw2(picture_02, actor.X, actor.Y, 1.0, 0.0, wRate, 1.0);
		}

		yield 1;
	}
}
