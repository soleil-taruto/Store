/*
	“G
*/

/@(ASTR)

/// Enemy_t
{
	<double> X // X-ˆÊ’u
	<double> Y // Y-ˆÊ’u
	<int> HP // ‘Ì—Í
	<boolean> Crashed // ©’e‚ÆÕ“Ë‚µ‚½‚©
	<double> “–‚½‚è”»’è_R
	<generatorForTask> Each
}

@(ASTR)/

/*
	“GƒŠƒXƒg
*/
var<Enemy_t> Enemies = [];

/*
	“G¶¬
*/
function <Enemy_t> CreateEnemy(<double> x, <double> y)
{
	var ret =
	{
		// ˆÊ’u
		X: x,
		Y: y,

		// ‘Ì—Í
		HP: 10,

		// ©’e‚ÆÕ“Ë‚µ‚½‚©
		Crashed: false,

		“–‚½‚è”»’è_R: 45.0,
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? ¶‘¶
*/
function <boolean> Enemy_Each(<Enemy_t> enemy)
{
	return enemy.Each.next().value;
}

function* <generatorForTask> @@_Each(<Enemy_t> enemy)
{
	var<double> speedX = GetRand1() * 2.0 - 3.0;
	var<double> speedY = GetRand2() * 3.0;

	for (; ; )
	{
		enemy.X += speedX;
		enemy.Y += speedY;

		if (enemy.Y < 0 && speedY < 0)
		{
			speedY = Math.abs(speedY);
		}
		else if (GetField_H() < enemy.Y && 0 < speedY)
		{
			speedY = -Math.abs(speedY);
		}

		// ‰æ–ÊŠO‚Éo‚½‚Ì‚Å‘Şê
		if (enemy.X < -100.0)
		{
			break;
		}

		if (enemy.Crashed)
		{
			enemy.HP--;

			// “GE€–S
			if (enemy.HP <= 0)
			{
				Score += 100;
				SE(S_Explode);
				AddEffect(Effect_Explode(GetField_L() + enemy.X, GetField_T() + enemy.Y));
				break;
			}
		}

		// ’e‚ğŒ‚‚Â
		if (GetRand(100) == 0)
		{
			// HACK: ‰æ–ÊŠOE©‹@‚É‹ß‚¢ê‡‚ÍŒ‚‚½‚È‚¢‚æ‚¤‚É‚·‚é‚×‚«‚©B

			Tamas.push(CreateTama(enemy.X, enemy.Y));
		}

		// •`‰æ
		Draw(P_Enemy_0001, GetField_L() + enemy.X, GetField_T() + enemy.Y, 1.0, 0.0, 1.0);

		// •`‰æ test
//		SetColor("#00ff00");
//		PrintRect_XYWH(GetField_L() + enemy.X, GetField_T() + enemy.Y, 20, 20);

		yield 1;
	}
}
