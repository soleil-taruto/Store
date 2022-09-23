/*
	“G - “G’e
*/

var<int> EnemyKind_Tama = @(AUTO);

function <Enemy_t> CreateEnemy_Tama(<double> x, <double> y, <double> xAdd, <double> yAdd)
{
	var ret =
	{
		Kind: EnemyKind_Tama,
		X: x,
		Y: y,
		HP: 0,
		Crash: null,

		// ‚±‚±‚©‚çŒÅ—L

		<double> XAdd: xAdd,
		<double> YAdd: yAdd,
	};

	ret.Draw = @@_Draw(ret);
	ret.Damaged = @@_Damaged;
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Enemy_t> enemy)
{
	for (var<int> frame = 0; ; frame++)
	{
		enemy.X += enemy.XAdd;
		enemy.Y += enemy.YAdd;

		if (IsOutOfScreen(CreateD2Point(enemy.X, enemy.Y), 50.0))
		{
			break;
		}

		enemy.Crash = CreateCrash_Circle(enemy.X, enemy.Y, 16.0);

		Draw(P_Tama0001, enemy.X, enemy.Y, 1.0, frame / 13.0, 1.0);

		yield 1;
	}
}

function <void> @@_Damaged(<Enemy_t> enemy, <int> damagePoint)
{
	// noop
}

function <void> @@_Dead(<Enemy_t> enemy, <boolean> destroyed)
{
	EnemyCommon_Dead(enemy, destroyed);
}
