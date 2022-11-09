/*
	“G’e
*/

/@(ASTR)

/// Tama_t
{
	<double> X
	<double> Y
	<double> “–‚½‚è”»’è_R
	<generatorForTask> Each
}

@(ASTR)/

/*
	“G’eƒŠƒXƒg
*/
var<Tama_t[]> Tamas = [];

/*
	“G’e¶¬
*/
function <Tama_t> CreateTama(<double> x, <double> y)
{
	var ret =
	{
		X: x,
		Y: y,

		“–‚½‚è”»’è_R: 15.0,
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? ¶‘¶
*/
function <boolean> Tama_Each(<Tama_t> tama)
{
	return tama.Each.next().value;
}

function* <generatorForTask> @@_Each(<Tama_t> tama)
{
	var speed = MakeXYSpeed(tama.X, tama.Y, Player_X, Player_Y, 3.0);
	var speedX = speed.X;
	var speedY = speed.Y;

	for (; ; )
	{
		tama.X += speedX;
		tama.Y += speedY;

		var<double> MARGIN = 30.0;

		// ‰æ–ÊŠO‚Éo‚½‚Ì‚Å‘Şê
		if (
			tama.X < -MARGIN || GetField_W() + MARGIN < tama.X ||
			tama.Y < -MARGIN || GetField_H() + MARGIN < tama.Y
			)
		{
			break;
		}

		// •`‰æ
		Draw(P_Tama_0001, GetField_L() + tama.X, GetField_T() + tama.Y, 1.0, 0.0, 1.0);

		// •`‰æ test
//		SetColor("#00ffff");
//		PrintRect_XYWH(GetField_L() + tama.X, GetField_T() + tama.Y, 20, 20);

		yield 1;
	}
}
