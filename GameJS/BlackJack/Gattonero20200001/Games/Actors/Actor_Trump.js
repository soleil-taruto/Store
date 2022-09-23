/*
	アクター - Trump (トランプのカード)
*/

var<int> ActorKind_Trump = @(AUTO);

/*
	(x, y): 初期位置
	suit: 絵柄のスート, 1 ～ 4 (並びはスハダクラブ)
	number: 絵柄の数字, 1 ～ 13
	reversed: 裏返っているか
*/
function <Actor_t> CreateActor_Trump(<double> x, <double> y, <int> suit, <int> number, <boolean> reversed)
{
	var ret =
	{
		Kind: ActorKind_Trump,
		X: x,
		Y: y,

		// ここから固有

		<double> Dest_X: x,
		<double> Dest_Y: y,

		<int> Suit: suit,     // 1 ～ 4  == 絵柄のスート
		<int> Number: number, // 1 ～ 13 == 絵柄の数字
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
			Draw(P_TrumpFrame, actor.X, actor.Y, 1.0, 0.0, 1.0);

			if (actor.Reversed)
			{
				Draw(P_Trump[actor.Suit][actor.Number], actor.X, actor.Y, 1.0, 0.0, 1.0);
			}
			else
			{
				Draw(P_TrumpBack, actor.X, actor.Y, 1.0, 0.0, 1.0);
			}
		}

		yield 1;
	}
}

function <void> SetTrumpDest(<Actor_t> actor, <double> x, <double> y)
{
	actor.Dest_X = x;
	actor.Dest_Y = y;
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
		var<boolean> b = !reversed;

		if (wRate < 0.0)
		{
			wRate *= -1.0;
			b = !b;
		}

		if (MICRO < wRate)
		{
			Draw2(P_TrumpFrame, actor.X, actor.Y, 1.0, 0.0, wRate, 1.0);

			if (b)
			{
				Draw2(P_Trump[actor.Suit][actor.Number], actor.X, actor.Y, 1.0, 0.0, wRate, 1.0);
			}
			else
			{
				Draw2(P_TrumpBack, actor.X, actor.Y, 1.0, 0.0, wRate, 1.0);
			}
		}

		yield 1;
	}
}
