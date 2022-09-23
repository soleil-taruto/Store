/*
	敵 - アイテム
*/

var<int> EnemyKind_Item = @(AUTO);

/// EnemyItemType_e
//
var<int> EnemyItemType_e_POWER_UP = @(AUTO);
var<int> EnemyItemType_e_ZANKI_UP = @(AUTO);

function <Enemy_t> CreateEnemy_Item(<double> x, <double> y, <EnemyItemType_e> itemType)
{
	var ret =
	{
		Kind: EnemyKind_Item,
		X: x,
		Y: y,
		HP: 0,
		Crash: null,

		// ここから固有

		<EnemyItemType_e> ItemType: itemType,
		<double> YAdd: -3.0,
	};

	ret.Draw = @@_Draw(ret);
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	var<double> Y_ADD_ADD = 0.1;

	for (; ; )
	{
		enemy.YAdd += Y_ADD_ADD;
		enemy.Y += enemy.YAdd;

		if (GetDistanceLessThan(enemy.X - PlayerX, enemy.Y - PlayerY, 100.0)) // アイテム取得
		{
			switch (enemy.ItemType)
			{
			case EnemyItemType_e_POWER_UP:
				PlayerAttackLv = Math.min(PlayerAttackLv + 1, PLAYER_ATTACK_LV_MAX);
				break;

			case EnemyItemType_e_ZANKI_UP:
				PlayerZankiNum++;
				break;

			default:
				error();
			}

			SE(S_PowerUp);
			break;
		}

		if (Screen_H < enemy.Y)
		{
			break;
		}

		enemy.Crash = null; // アイテムにつき当たり判定無し。

		{
			var<Picture_t> picture;

			switch (enemy.ItemType)
			{
			case EnemyItemType_e_POWER_UP: picture = P_PowerUpItem; break;
			case EnemyItemType_e_ZANKI_UP: picture = P_ZankiUpItem; break;

			default:
				error();
			}

			Draw(picture, enemy.X, enemy.Y, 1.0, 0.0, 1.0);
		}

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	// noop
}

function <void> @@_Dead(<Enemy_t> enemy)
{
	// noop
}

function <EnemyItemType_e> GetEnemyItemType(<Enemy_t> enemy)
{
	return enemy.ItemType;
}
