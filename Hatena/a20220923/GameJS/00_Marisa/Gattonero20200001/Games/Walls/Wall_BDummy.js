/*
	•Ç† - BDummy šƒTƒ“ƒvƒ‹
*/

var<int> WallKind_BDummy = @(AUTO);

function <Wall_t> CreateWall_BDummy()
{
	var ret =
	{
		// ‚±‚±‚©‚çŒÅ—L

		<double> Dummy_01: 1.0,
		<double> Dummy_02: 2.0,
		<double> Dummy_03: 3.0,
	};

	ret.Draw = @@_Draw(ret);

	return ret;
}

function* <generatorForTask> @@_Draw(<Wall_t> wall)
{
	for (; ; )
	{
		SetColor("#000040");
		PrintRect(0.0, 0.0, Screen_W, Screen_H);

		yield 1;
	}
}
