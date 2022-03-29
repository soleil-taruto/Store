/*
	é©íe
*/

function CreateShot(x, y)
{
	var ret =
	{
		// à íu
		X: x,
		Y: y,

		// ìGÇ∆è’ìÀÇµÇΩÇ©
		Crashed: false,
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? ê∂ë∂
*/
function Shot_Each(shot)
{
	return shot.Each.next().value;
}

function* @@_Each(shot)
{
	for (; ; )
	{
		shot.X += 10;

		// âÊñ äOÇ…èoÇΩÇÃÇ≈ëﬁèÍ
		if (GetField_W() < shot.X)
		{
			break;
		}

		// è’ìÀÇ…ÇÊÇËè¡ñ≈
		if (shot.Crashed)
		{
			break;
		}

		// ï`âÊ
		Draw(P_Shot_0001, GetField_L() + shot.X, GetField_T() + shot.Y, 1.0, 0.0, 1.0);

		// ï`âÊ test
//		SetColor("#ffffff");
//		PrintRectCenter(GetField_L() + shot.X, GetField_T() + shot.Y, 10, 10);

		yield 1;
	}
}

/*
	é©íeÉäÉXÉg
*/
var Shots = [];
