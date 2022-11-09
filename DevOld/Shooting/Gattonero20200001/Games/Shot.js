/*
	自弾
*/

/@(ASTR)

/// Shot_t
{
	<double> X // X-位置
	<double> Y // Y-位置
	<boolean> Crashed // 敵と衝突したか
	<double> 当たり判定_R
	<generatorForTask> Each
}

@(ASTR)/

/*
	自弾リスト
*/
var<Shot_t[]> Shots = [];

/*
	自弾生成
*/
function <Shot_t> CreateShot(<double> x, <double> y)
{
	var ret =
	{
		// 位置
		X: x,
		Y: y,

		// 敵と衝突したか
		Crashed: false,

		当たり判定_R: 20.0,
	};

	ret.Each = @@_Each(ret);

	return ret;
}

/*
	ret: ? 生存
*/
function <boolean> Shot_Each(<Shot_t> shot)
{
	return shot.Each.next().value;
}

function* <generatorForTask> @@_Each(<Shot_t> shot)
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
//		PrintRect_XYWH(GetField_L() + shot.X, GetField_T() + shot.Y, 10, 10);

		yield 1;
	}
}
