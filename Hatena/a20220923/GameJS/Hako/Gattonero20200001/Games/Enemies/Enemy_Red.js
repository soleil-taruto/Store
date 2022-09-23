/*
	赤敵
*/

var<int> EnemyKind_Red = @(AUTO);

function <Enemy_t> CreateEnemy_Red(<double> x, <double> y, <int> initDirect)
{
	var<double> SPEED = 4.0;

	var ret =
	{
		Kind: EnemyKind_Red,
		X: x,
		Y: y,
		HP: 1,
		Crash: null,

		// ここから固有

		<double> Hiest: y + 7.0,

		<double> XSpeed: SPEED * initDirect,
		<double> YSpeed: 0.0,
	};

	ret.Draw = @@_Draw(ret);
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	var<double> GRAVITY = 0.5;
	var<double> FALL_SPEED_MAX = 19.0;

	for (; ; )
	{
		enemy.YSpeed += GRAVITY;

		// 側面と地面の跳ね返り
		{
			var<boolean> lsw = @@_IsWall(GetMapCell(ToTablePoint_XY(enemy.X - TILE_W / 2.0, enemy.Y                ))); // 左側面
			var<boolean> rsw = @@_IsWall(GetMapCell(ToTablePoint_XY(enemy.X + TILE_W / 2.0, enemy.Y                ))); // 右側面
			var<boolean> lbw = @@_IsWall(GetMapCell(ToTablePoint_XY(enemy.X - TILE_W / 2.0, enemy.Y + TILE_H / 2.0 ))); // 左下
			var<boolean> rbw = @@_IsWall(GetMapCell(ToTablePoint_XY(enemy.X + TILE_W / 2.0, enemy.Y + TILE_H / 2.0 ))); // 右下

			if (lsw)
			{
				enemy.XSpeed = Math.abs(enemy.XSpeed);
			}
			if (rsw)
			{
				enemy.XSpeed = Math.abs(enemy.XSpeed) * -1.0;
			}
			if ((!(lsw || rsw) && (lbw || rbw)) || (lbw && rbw))
			{
				enemy.YSpeed = Math.abs(enemy.YSpeed) * -1.0;
			}
		}

		if (enemy.Y < enemy.Hiest && enemy.YSpeed < 0.0)
		{
			enemy.YSpeed /= 2.0;
		}

		enemy.YSpeed = Math.min(enemy.YSpeed, FALL_SPEED_MAX);

		enemy.X += enemy.XSpeed;
		enemy.Y += enemy.YSpeed;

		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(enemy.X, enemy.Y, TILE_W, TILE_H));

		Draw(P_Enemy_R, enemy.X, enemy.Y, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <boolean> @@_IsWall(<MapCell_t> cell)
{
	return cell.WallFlag || cell.NarrowFlag;
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy)
{
	EnemyCommon_Dead(enemy);
}
