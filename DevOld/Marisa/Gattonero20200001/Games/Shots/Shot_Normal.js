/*
	é©íe - Normal(í èÌíe)
*/

var<int> ShotKind_Normal = @(AUTO);

function <Shot_t> CreateShot_Normal(<doule> x, <double> y, <int> direction)
{
	var<D2Point_t> speed = GetXYSpeed(direction, 10.0);

	var ret =
	{
		Kind: ShotKind_Normal,
		X: x,
		Y: y,
		AttackPoint: 1,
		Crash: null,

		// Ç±Ç±Ç©ÇÁå≈óL

		<double> XAdd: speed.X,
		<double> YAdd: speed.Y,
	};

	ret.Draw = @@_Draw(ret);
	ret.Dead = @@_Dead;

	return ret;
}

function* <generatorForTask> @@_Draw(<Shot_t> shot)
{
	var<double> SHOT_R = 5.0;

	for (; ; )
	{
		shot.X += shot.XAdd;
		shot.Y += shot.YAdd;

		if (IsOutOfCamera(CreateD2Point(shot.X, shot.Y), 0.0))
		{
			break;
		}

		if (IsPtWallForAir_XY(shot.X, shot.Y))
		{
			KillShot(shot);
			break;
		}

		shot.Crash = CreateCrash_Circle(shot.X, shot.Y, SHOT_R);

		SetColor(I3ColorToString(CreateI3Color(128, 64, 255)));
		PrintCircle(shot.X - Camera.X, shot.Y - Camera.Y, SHOT_R);

		yield 1;
	}
}

function <void> @@_Dead(<Shot_t> shot)
{
	ShotCommon_Dead(shot);
}
