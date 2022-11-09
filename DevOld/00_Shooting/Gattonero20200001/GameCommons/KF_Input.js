/*
	入力

	ファイル名 Input.js -> KF_Input.js の理由：
	本ソースの @@_EACH() を Gamepad.js, Keyboard.js の *_EACH より後に実行する必要がある。
*/

/*
	ゲームパッドのボタン・インデックス
	変更可能
*/
var<int> PadInputIndex_2 = 102;
var<int> PadInputIndex_4 = 104;
var<int> PadInputIndex_6 = 106;
var<int> PadInputIndex_8 = 108;
var<int> PadInputIndex_A = 0;
var<int> PadInputIndex_B = 3;

/*
	各ボタンの押下状態カウンタ
*/
var<int> @@_Count_2 = 0;
var<int> @@_Count_4 = 0;
var<int> @@_Count_6 = 0;
var<int> @@_Count_8 = 0;
var<int> @@_Count_A = 0;
var<int> @@_Count_B = 0;

/*
	各ボタンの押下状態カウンタの列挙
*/
function* <int[]> @@_Counts()
{
	yield @@_Count_2;
	yield @@_Count_4;
	yield @@_Count_6;
	yield @@_Count_8;
	yield @@_Count_A;
	yield @@_Count_B;
}

function <void> @@_EACH()
{
	@@_Count_2 = @@_Check(@@_Count_2, PadInputIndex_2, [ 40, 74,  98 ]); // カーソル下 , J , テンキー2
	@@_Count_4 = @@_Check(@@_Count_4, PadInputIndex_4, [ 37, 72, 100 ]); // カーソル左 , H , テンキー4
	@@_Count_6 = @@_Check(@@_Count_6, PadInputIndex_6, [ 39, 76, 102 ]); // カーソル右 , L , テンキー6
	@@_Count_8 = @@_Check(@@_Count_8, PadInputIndex_8, [ 38, 75, 104 ]); // カーソル上 , K , テンキー8
	@@_Count_A = @@_Check(@@_Count_A, PadInputIndex_A, [ 90 ]); // Z
	@@_Count_B = @@_Check(@@_Count_B, PadInputIndex_B, [ 88 ]); // X
}

function <int> @@_Check(<int> counter, <int> padInputIndex, <int[]> keyCodes)
{
	var<boolean> statusPad = GetPadInput(padInputIndex);
	var<boolean> statusKey = false;

	for (var<int> keyCode of keyCodes)
	{
		if (1 <= GetKeyInput(keyCode))
		{
			statusKey = true;
		}
	}
	var<boolean> status = statusPad || statusKey;

	if (status) // ? 押している。
	{
		// 前回 ⇒ 今回
		// -1   ⇒  1
		//  0   ⇒  1
		//  1～ ⇒  2～

		counter = Math.max(counter + 1, 1);
	}
	else // ? 押していない。
	{
		// 前回 ⇒ 今回
		// -1   ⇒  0
		//  0   ⇒  0
		//  1～ ⇒ -1

		counter = Math.max(Math.max(counter, 0) * -1, -1);
	}
	return counter;
}

var @@_FreezeInputUntilReleaseFlag = false;

function <int> @@_GetInput(<int> counter)
{
	if (@@_FreezeInputUntilReleaseFlag)
	{
		if (ToArray(@@_Counts()).some(counter => counter != 0))
		{
			return 0;
		}

		@@_FreezeInputUntilReleaseFlag = false;
	}

	return 1 <= FreezeInputFrame ? 0 : counter;
}

// ----
// GetInput_X ここから
// ----

function <int> GetInput_2()
{
	return @@_GetInput(@@_Count_2);
}

function <int> GetInput_4()
{
	return @@_GetInput(@@_Count_4);
}

function <int> GetInput_6()
{
	return @@_GetInput(@@_Count_6);
}

function <int> GetInput_8()
{
	return @@_GetInput(@@_Count_8);
}

/*
	決定
	ジャンプ
*/
function <int> GetInput_A()
{
	return @@_GetInput(@@_Count_A);
}

/*
	キャンセル
	攻撃
*/
function <int> GetInput_B()
{
	return @@_GetInput(@@_Count_B);
}

// ----
// GetInput_X ここまで
// ----

/*
	キーやボタンの押しっぱなしを連打として検出する。

	使用例：
		if (IsPound(GetInput_A()))
		{
			// ...
		}
*/
function <boolean> IsPound(<int> counter)
{
	var<int> POUND_FIRST_DELAY = 17;
	var<int> POUND_DELAY = 4;

	return counter == 1 || POUND_FIRST_DELAY < counter && (counter - POUND_FIRST_DELAY) % POUND_DELAY == 1;
}

// 入力抑止フレーム数
var<int> FreezeInputFrame = 0;

function @(UNQN)_EACH()
{
	FreezeInputFrame = CountDown(FreezeInputFrame);
}

function <void> FreezeInput_Frame(<int> frame) // frame: 1 == このフレームのみ, 2 == このフレームと次のフレーム ...
{
	ClearMouseDown();
	FreezeInputFrame = Math.max(FreezeInputFrame, frame); // frame より長いフレーム数が既に設定されていたら、そちらを優先する。
}

function <void> FreezeInput()
{
	FreezeInput_Frame(1);
}

function <void> FreezeInputUntilRelease()
{
	@@_FreezeInputUntilReleaseFlag = true;
}
