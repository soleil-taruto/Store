/*
	敵共通
*/

function <void> EnemyCommon_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	SE(S_EnemyDamaged);
}

function <void> EnemyCommon_Dead(<Enemy_t> enemy, <boolean> destroyed) // destroyed: プレイヤー等(の攻撃行動)によって撃破されたか
{
	if (IsEnemyTama(enemy)) // ? 敵弾
	{
		AddEffect_TamaExplode(enemy.X, enemy.Y);
	}
	else
	{
		AddEffect_Explode(enemy.X, enemy.Y);
		SE(S_EnemyDead);
	}
}

function <void> EnemyCommon_Draw(<Enemy_t> enemy)
{
	var<Picture_t> picture;

	switch (enemy.Kind)
	{
	case EnemyKind_E0001: picture = P_Enemy0001; break;
	case EnemyKind_E0002: picture = P_Enemy0002; break;
	case EnemyKind_E0003: picture = P_Enemy0003; break;
	case EnemyKind_E0004: picture = P_Enemy0004; break;
	case EnemyKind_E0005: picture = P_Enemy0005; break;
	case EnemyKind_E0006: picture = P_Enemy0006; break;
	case EnemyKind_E0007: picture = P_Enemy0007; break;
	case EnemyKind_E0008: picture = P_Enemy0008; break;

	default:
		error();
	}

	enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, 50.0);

	Draw(picture, enemy.X, enemy.Y, 1.0, 0.0, 1.0);

	// HP 表示
	{
		var<string> str = "" + enemy.HP;

		SetPrint(ToInt(enemy.X - str.length * 5), ToInt(enemy.Y - 30), 0);
		SetFSize(16);
		SetColor(enemy.Kind == EnemyKind_E0006 ? "#000000" : "#ffffff");
		PrintLine(str);
	}
}

function <void> EnemyCommon_AddScore(<int> scoreAdd)
{
	Score += scoreAdd;
}

/*
	指定された敵は「アイテム」か判定する。
*/
function <boolean> IsEnemyItem(<Enemy_t> enemy)
{
	var ret =
		enemy.Kind == EnemyKind_Item;
//		enemy.Kind == EnemyKind_Item_0001 ||
//		enemy.Kind == EnemyKind_Item_0002 ||
//		enemy.Kind == EnemyKind_Item_0003;

	return ret;
}

/*
	指定された敵は「敵弾」か判定する。
*/
function <boolean> IsEnemyTama(<Enemy_t> enemy)
{
	var ret =
		enemy.Kind == EnemyKind_Tama;
//		enemy.Kind == EnemyKind_Tama_0001 ||
//		enemy.Kind == EnemyKind_Tama_0002 ||
//		enemy.Kind == EnemyKind_Tama_0003;

	return ret;
}

/*
	指定された敵は「ボス」か判定する。
*/
function <boolean> IsEnemyBoss(<Enemy_t> enemy)
{
	var ret =
		enemy.Kind == EnemyKind_Boss01 ||
		enemy.Kind == EnemyKind_Boss02 ||
		enemy.Kind == EnemyKind_Boss03;

	return ret;
}
