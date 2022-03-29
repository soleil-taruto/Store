/*
	ゲーム・メイン
*/

function* GameMain(room)
{
	// 開始ルームをセットする。
	//
//	NextRoom = StartRoom(); // 本番用
//	NextRoom = TestRoom(); // テスト・ルーム
//	NextRoom = TestRoomB(); // テスト・ルームB

	switch (room)
	{
	case 1:
		NextRoom = TestRoom();
		break;

	case 2:
		NextRoom = TestRoomB();
		break;

	default:
		error;
	}

	for (; ; )
	{
		yield* NextRoom;
	}
}

// 部屋を出る前に次の部屋のジェネレータをセットすること。
var NextRoom = null;

function EnterRoom()
{
	ClearMouseDown();
	NextRoom = null; // reset
}

function LeaveRoom()
{
	ClearMouseDown();
}

function StartDrawRoom()
{
	// none
}

function EndDrawRoom()
{
	// none
}
