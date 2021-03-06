/*
	自弾
*/

function CreateShot(x, y)
{
	var ret =
	{
		// 位置
		X: x,
		Y: y,

		// 敵と衝突したか
		Crashed: false,
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? 生存
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

		// 画面外に出たので退場
		if (GetField_W() < shot.X)
		{
			break;
		}

		// 衝突により消滅
		if (shot.Crashed)
		{
			break;
		}

		// 描画
		Draw(P_Shot_0001, GetField_L() + shot.X, GetField_T() + shot.Y, 1.0, 0.0, 1.0);

		// 描画 test
//		SetColor("#ffffff");
//		PrintRectCenter(GetField_L() + shot.X, GetField_T() + shot.Y, 10, 10);

		yield 1;
	}
}

/*
	自弾リスト
*/
var Shots = [];
