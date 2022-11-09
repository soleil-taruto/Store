/*
	アクター - Trump (トランプのカード)
*/

var<int> ActorKind_Trump = @(AUTO);

/*
	(x, y): 初期位置
	suit: 絵柄のスート
	number: 絵柄の数字 (1～13)
	reversed: 裏返っているか
*/
function <Actor_t> CreateActor_Trump(<double> x, <double> y, <Suit_e> suit, <int> number, <boolean> reversed)
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

		<Suit_e> Suit: suit,  // 絵柄のスート
		<int> Number: number, // 絵柄の数字 (1～13)
		<boolean> Reversed: reversed,
		<generatorForTask> SpecialDraw: ToGenerator([]),

		<double> Rot: 0.0,
	};

	ret.Draw = @@_Draw(ret);

	return ret;
}

var<double> @@_PICTURE_Z = 1.0;

function* <generatorForTask> @@_Draw(<Actor_t> actor)
{
	for (; ; )
	{
		actor.X = Approach(actor.X, actor.Dest_X, 0.93);
		actor.Y = Approach(actor.Y, actor.Dest_Y, 0.93);

		actor.Rot = Approach(actor.Rot, 0.0, 0.9);

		if (!NextVal(actor.SpecialDraw))
		{
			Draw(P_TrumpFrame, actor.X, actor.Y, 1.0, actor.Rot, @@_PICTURE_Z);

			if (!actor.Reversed)
			{
				Draw(P_Trump[actor.Suit][actor.Number], actor.X, actor.Y, 1.0, actor.Rot, @@_PICTURE_Z);
			}
			else
			{
				Draw(P_TrumpBack, actor.X, actor.Y, 1.0, actor.Rot, @@_PICTURE_Z);
			}
		}

		yield 1;
	}
}

function <void> SetTrumpPos(<Actor_t> actor, <double> x, <double> y)
{
	actor.X = x;
	actor.Y = y;
	actor.Dest_X = x;
	actor.Dest_Y = y;
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
	for (var<Scene_t> scene of CreateScene(30))
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
			Draw2(P_TrumpFrame, actor.X, actor.Y, 1.0, actor.Rot, wRate, @@_PICTURE_Z);

			if (b)
			{
				Draw2(P_Trump[actor.Suit][actor.Number], actor.X, actor.Y, 1.0, actor.Rot, wRate, @@_PICTURE_Z);
			}
			else
			{
				Draw2(P_TrumpBack, actor.X, actor.Y, 1.0, actor.Rot, wRate, @@_PICTURE_Z);
			}
		}

		yield 1;
	}
}

function <void> SetTrumpAutoStRot(<Actor_t> actor) // 回転開始をセットする。
{
	SetTrumpStRot(actor, GetRand2() * 200.0);
}

function <void> SetTrumpStRot(<Actor_t> actor, <double> rot)
{
	actor.Rot = rot;
}
