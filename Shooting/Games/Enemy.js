/*
	“G
*/

function CreateEnemy(x, y)
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
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? ¶‘¶
*/
function Enemy_Each(enemy)
{
	return enemy.Each.next().value;
}

function* @@_Each(enemy)
{
	var speedX = Math.random() * -2.0 - 1.0;
	var speedY = (Math.random() - 0.5) * 6.0;

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
				AddCommonEffect(Effect_Explode(GetField_L() + enemy.X, GetField_T() + enemy.Y));
				break;
			}
		}

		// ’e‚ğŒ‚‚Â
		if (GetRand(100) == 0)
		{
			// TODO ‰æ–ÊŠOE©‹@‚É‹ß‚¢ê‡‚ÍŒ‚‚½‚È‚¢B

			Tamas.push(CreateTama(enemy.X, enemy.Y));
		}

		// •`‰æ
		Draw(P_Enemy_0001, GetField_L() + enemy.X, GetField_T() + enemy.Y, 1.0, 0.0, 1.0);

		// •`‰æ test
//		SetColor("#00ff00");
//		PrintRectCenter(GetField_L() + enemy.X, GetField_T() + enemy.Y, 20, 20);

		yield 1;
	}
}

/*
	“GƒŠƒXƒg
*/
var Enemies = [];
