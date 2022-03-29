/*
	“G’e
*/

function CreateTama(x, y)
{
	var ret =
	{
		X: x,
		Y: y,
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? ¶‘¶
*/
function Tama_Each(tama)
{
	return tama.Each.next().value;
}

function* @@_Each(tama)
{
	var speed = MakeXYSpeed(tama.X, tama.Y, Player_X, Player_Y, 3.0);
	var speedX = speed.X;
	var speedY = speed.Y;

	for (; ; )
	{
		tama.X += speedX;
		tama.Y += speedY;

		var MARGIN = 30.0;

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
//		PrintRectCenter(GetField_L() + tama.X, GetField_T() + tama.Y, 20, 20);

		yield 1;
	}
}

/*
	“G’eƒŠƒXƒg
*/
var Tamas = [];
