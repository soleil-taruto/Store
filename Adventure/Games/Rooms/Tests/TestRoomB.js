/*
	テスト・ルーム (★サンプルとしてキープ)
*/

var @@_背景Table =
[
	P_背景_自宅,
	P_背景_自室,
	P_背景_自宅トイレ,
	P_背景_学校廊下,
	P_背景_更衣室A,
	P_背景_更衣室B,
	P_背景_校門,
	P_背景_体育館,
	P_背景_公園,
];

var @@_背景List = P_背景_自宅;

var @@_時間帯 = 0; // 0〜3: { 日中, 夕方, 夜(点灯), 夜(消灯) }

function* TestRoomB()
{
	EnterRoom();

	for (; ; )
	{
		// 部屋固有の処理ここから

		if (GetMouseDown() == -1)
		{
			if (GetMouseX() < Screen_W / 2)
			{
				var i = @@_背景Table.indexOf(@@_背景List);
				i = (i + 1) % @@_背景Table.length;
				@@_背景List = @@_背景Table[i];
			}
			else
			{
				@@_時間帯 = (@@_時間帯 + 1) % 4;
			}
		}

		// 部屋固有の処理ここまで

		StartDrawRoom();

		// 部屋固有の描画ここから

		Draw(@@_背景List[@@_時間帯], Screen_W / 2, Screen_H / 2, 1, 0, 1);

		SetColor("#ffffff");
		SetPrint(50, 60, 50);
		SetSize(16);
		PrintLine("TestRoom2 -- Click Left -> Move, Right -> Time");

		// 部屋固有の描画ここまで

		EndDrawRoom();

		yield 1;
	}
	LeaveRoom();
}
