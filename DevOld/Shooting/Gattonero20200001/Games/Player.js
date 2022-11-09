/*
	ゲーム -- プレイヤー情報・制御
*/

var<double> Player_当たり判定_R = 30.0;

var<double> Player_X = ToInt(GetField_W() / 2);
var<double> Player_Y = ToInt(GetField_H() / 2);

function <void> DrawPlayer()
{
	Draw(P_Player, GetField_L() + Player_X, GetField_T() + Player_Y, 1.0, 0.0, 1.0);

	// test
//	SetColor("#ffa000");
//	PrintRect_XYWH(GetField_L() + Player_X, GetField_T() + Player_Y, 20, 20);
}
