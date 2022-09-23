/*
	—Î“G
*/

var<int> EnemyKind_Green = @(AUTO);

function <Enemy_t> CreateEnemy_Green(<double> x, <double> y, <int> rotDirect, <double> initAngle)
{
	var<double> ROT_SPEED = Math.PI / 60.0;

	var ret =
	{
		Kind: EnemyKind_Green,
		X: x,
		Y: y,
		HP: 1,
		Crash: null,

		// ‚±‚±‚©‚çŒÅ—L

		<double> RotSpeed: ROT_SPEED * rotDirect,
		<double> Angle: initAngle,
	};

	ret.Draw = @@_Draw(ret);
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	var<double> RADIUS = 100.0;

	for (; ; )
	{
		enemy.Angle += enemy.RotSpeed;
		enemy.Angle = ToInRangeAngle(enemy.Angle);

		var<D2Point_t> relPos = AngleToPoint(enemy.Angle, RADIUS);

		var<double> dx = enemy.X + relPos.X;
		var<double> dy = enemy.Y + relPos.Y;

		enemy.Crash = CreateCrash_Rect(CreateD4Rect_XYWH(dx, dy, TILE_W, TILE_H));

		Draw(P_Enemy_G, dx, dy, 1.0, 0.0, 1.0);

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	EnemyCommon_Damaged(enemy, damagePoint);
}

function <void> @@_Dead(<Enemy_t> enemy)
{
	EnemyCommon_Dead(enemy);
}
