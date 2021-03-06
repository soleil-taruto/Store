/*
	敵
*/

function CreateEnemy(x, y)
{
	var ret =
	{
		// 位置
		X: x,
		Y: y,

		// 体力
		HP: 10,

		// 自弾と衝突したか
		Crashed: false,
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? 生存
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

		// 画面外に出たので退場
		if (enemy.X < -100.0)
		{
			break;
		}

		if (enemy.Crashed)
		{
			enemy.HP--;

			// 敵・死亡
			if (enemy.HP <= 0)
			{
				Score += 100;
				SE(S_Explode);
				AddCommonEffect(Effect_Explode(GetField_L() + enemy.X, GetField_T() + enemy.Y));
				break;
			}
		}

		// 弾を撃つ
		if (GetRand(100) == 0)
		{
			// TODO 画面外・自機に近い場合は撃たない。

			Tamas.push(CreateTama(enemy.X, enemy.Y));
		}

		// 描画
		Draw(P_Enemy_0001, GetField_L() + enemy.X, GetField_T() + enemy.Y, 1.0, 0.0, 1.0);

		// 描画 test
//		SetColor("#00ff00");
//		PrintRectCenter(GetField_L() + enemy.X, GetField_T() + enemy.Y, 20, 20);

		yield 1;
	}
}

/*
	敵リスト
*/
var Enemies = [];
