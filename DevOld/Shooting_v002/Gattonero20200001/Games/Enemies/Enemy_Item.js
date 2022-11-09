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

		if (FIELD_B < enemy.Y)
		{
			break;
		}

		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(enemy.X, enemy.Y, 200.0, 140.0));

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
	EnemyCommon_AddScore(50);
//	EnemyCommon_Dead(enemy); // 敵ではないので、通常の敵_死亡イベントは適用しない。

//	AddEffect_PlayerPowerUp(PlayerX, PlayerY); // TODO
	SE(S_PowerUp);
}

function <EnemyItemType_e> GetEnemyItemType(<Enemy_t> enemy)
{
	return enemy.ItemType;
}
