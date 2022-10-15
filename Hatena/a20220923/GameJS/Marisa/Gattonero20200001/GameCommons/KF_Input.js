/*
	入力

	ファイル名 Input.js -> KF_Input.js の理由：
	本ソースの *_EACH() を Gamepad.js, Keyboard.js の *_EACH より後に実行する必要がある。
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
var<int> PadInputIndex_Pause = 9;

/*
	各ボタンの押下状態カウンタ
*/
var<int> @@_Count_2 = 0;
var<int> @@_Count_4 = 0;
var<int> @@_Count_6 = 0;
var<int> @@_Count_8 = 0;
var<int> @@_Count_A = 0;
var<int> @@_Count_B = 0;
var<int> @@_Count_Pause = 0;

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
	yield @@_Count_Pause;
}

function <void> @(UNQN)_EACH()
{
	@@_Count_2     = @@_Check(@@_Count_2,     false,           PadInputIndex_2,     [ 40,  98, 83 ]); // カーソル下 , テンキー2 , S
	@@_Count_4     = @@_Check(@@_Count_4,     false,           PadInputIndex_4,     [ 37, 100, 65 ]); // カーソル左 , テンキー4 , A
	@@_Count_6     = @@_Check(@@_Count_6,     1 <= @@_Count_4, PadInputIndex_6,     [ 39, 102, 68 ]); // カーソル右 , テンキー6 , D
	@@_Count_8     = @@_Check(@@_Count_8,     1 <= @@_Count_2, PadInputIndex_8,     [ 38, 104, 87 ]); // カーソル上 , テンキー8 , W
	@@_Count_A     = @@_Check(@@_Count_A,     false,           PadInputIndex_A,     [ 90 ]); // Z
	@@_Count_B     = @@_Check(@@_Count_B,     false,           PadInputIndex_B,     [ 88 ]); // X
	@@_Count_Pause = @@_Check(@@_Count_Pause, false,           PadInputIndex_Pause, [ 32 ]); // スペース
}

function <int> @@_Check(<int> counter, <boolean> oppositeInput, <int> padInputIndex, <int[]> keyCodes)
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
	var<boolean> status = !oppositeInput && (statusPad || statusKey);

	if (status) // ? 押している。
	{
		// 前回 ⇒ 今回
		// -2   ⇒ -2
		// -1   ⇒  1
		//  0   ⇒  1
		//  1～ ⇒  2～

		if (counter != -2)
		{
			counter = Math.max(counter + 1, 1);
		}
	}
	else // ? 押していない。
	{
		// 前回 ⇒ 今回
		// -2   ⇒  0
		// -1   ⇒  0
		//  0   ⇒  0
		//  1～ ⇒ -1

		counter = Math.max(Math.max(counter, 0) * -1, -1);
	}
	return counter;
}

function <int> @@_GetInput(<int> counter)
{
	return 1 <= FreezeInputFrame || counter == -2 ? 0 : counter;
}

// ★★★ ボタン・キー押下は 1 マウス押下は -1 で判定する。

// ----
// GetInput_X ここから
// ----

function <int> GetInput_2()
{
	return @@_GetInput(@@_Count_2, true);
}

function <int> GetInput_4()
{
	return @@_GetInput(@@_Count_4, true);
}

function <int> GetInput_6()
{
	return @@_GetInput(@@_Count_6, true);
}

function <int> GetInput_8()
{
	return @@_GetInput(@@_Count_8, true);
}

/*
	Z
	決定
	ジャンプ
	etc.
*/
function <int> GetInput_A()
{
	return @@_GetInput(@@_Count_A, false);
}

/*
	X
	キャンセル
	攻撃
	etc.
*/
function <int> GetInput_B()
{
	return @@_GetInput(@@_Count_B, false);
}

/*
	スペース
	ポーズ
	etc.
*/
function <int> GetInput_Pause()
{
	return @@_GetInput(@@_Count_Pause, false);
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
	var<int> COUNT_FREEZE_INPUT = -2;

//	@@_Count_2     = COUNT_FREEZE_INPUT;
//	@@_Count_4     = COUNT_FREEZE_INPUT;
//	@@_Count_6     = COUNT_FREEZE_INPUT;
//	@@_Count_8     = COUNT_FREEZE_INPUT;
	@@_Count_A     = COUNT_FREEZE_INPUT;
	@@_Count_B     = COUNT_FREEZE_INPUT;
	@@_Count_Pause = COUNT_FREEZE_INPUT;
}

function* <generatorForTask> WaitToReleaseButton()
{
	while (ToArray(@@_Counts()).some(counter => counter != 0))
	{
		yield 1;
	}
}
