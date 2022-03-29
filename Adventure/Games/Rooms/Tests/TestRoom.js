/*
	テスト・ルーム (★サンプルとしてキープ)
*/

function* TestRoom()
{
	EnterRoom();

	for (; ; )
	{
		// 部屋固有の処理ここから

		if (GetMouseDown() == -1)
		{
			NextRoom = TestRoom2(); // 次の部屋のジェネレータをセットする。
			break;
		}

		// 部屋固有の処理ここまで

		StartDrawRoom();

		// 部屋固有の描画ここから

		Draw(P_背景_校門[0], Screen_W / 2, Screen_H / 2, 1, 0, 1);

		SetColor("#ffffff");
		SetPrint(50, 60, 50);
		SetSize(16);
		PrintLine("TestRoom -- Click -> TestRoom2");

		// 部屋固有の描画ここまで

		EndDrawRoom();

		yield 1;
	}
	LeaveRoom();
}

function* TestRoom2()
{
	EnterRoom();

	for (; ; )
	{
		// 部屋固有の処理ここから

		if (GetMouseDown() == -1)
		{
			if (GetMouseX() < Screen_W / 2)
			{
				NextRoom = TestRoom(); // 次の部屋のジェネレータをセットする。
				break;
			}
			else
			{
				// 再帰的_部屋移動
				{
					LeaveRoom();
					yield* TestRoom3();
					EnterRoom();
				}
			}
		}

		// 部屋固有の処理ここまで

		StartDrawRoom();

		// 部屋固有の描画ここから

		Draw(P_背景_学校廊下[0], Screen_W / 2, Screen_H / 2, 1, 0, 1);

		SetColor("#ffffff");
		SetPrint(50, 60, 50);
		SetSize(16);
		PrintLine("TestRoom2 -- Click Left -> TestRoom, Right -> TestRoom3");

		// 部屋固有の描画ここまで

		EndDrawRoom();

		yield 1;
	}
	LeaveRoom();
}

/*
	再帰的に入ってくる部屋
*/
function* TestRoom3()
{
	EnterRoom();

	for (; ; )
	{
		// 部屋固有の処理ここから

		if (GetMouseDown() == -1)
		{
			// 再帰的に入ってくる部屋なので NextRoom のセットは不要
			break;
		}

		// 部屋固有の処理ここまで

		StartDrawRoom();

		// 部屋固有の描画ここから

		Draw(P_背景_更衣室A[0], Screen_W / 2, Screen_H / 2, 1, 0, 1);

		SetColor("#ffffff");
		SetPrint(50, 60, 50);
		SetSize(16);
		PrintLine("TestRoom3 -- Click -> TestRoom2");

		// 部屋固有の描画ここまで

		EndDrawRoom();

		yield 1;
	}
	LeaveRoom();
}
