/*
	é©íe - Normal(í èÌíe)
*/

var<int> ShotKind_Normal = @(AUTO);

function <Shot_t> CreateShot_Normal(<doule> x, <double> y, <boolean> facingLeft, <boolean> uwamuki, <boolean> shitamuki)
{
	var<int> xDir;
	var<int> yDir;

	if (facingLeft)
	{
		xDir = -1;
		yDir = 0;
	}
	else
	{
		xDir = 1;
		yDir = 0;
	}
	if (uwamuki)
	{
		xDir = 0;
		yDir = -1;
	}
	if (shitamuki)
	{
		xDir = 0;
		yDir = 1;
	}

	var<double> SPEED = 10.0;

	var<double> xAdd = SPEED * xDir;
	var<double> yAdd = SPEED * yDir;

	var ret =
	{
		Kind: ShotKind_Normal,
		X: x,
		Y: y,
		AttackPoint: 1,
		Crash: null,

		// Ç±Ç±Ç©ÇÁå≈óL

		<double> XAdd: xAdd,
		<double> YAdd: yAdd,
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

		if (IsPtWall_XY(shot.X, shot.Y))
		{
			KillShot(shot);
			break;
		}

		shot.Crash = CreateCrash_Circle(shot.X, shot.Y, SHOT_R);

		SetColor(I3ColorToString(CreateI3Color(0, 255, 128)));
		PrintCircle(shot.X - Camera.X, shot.Y - Camera.Y, SHOT_R);

		yield 1;
	}
}

function <void> @@_Dead(<Shot_t> shot)
{
	ShotCommon_Dead(shot);
}
