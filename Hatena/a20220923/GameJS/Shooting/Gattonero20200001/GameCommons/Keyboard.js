/*
	キーボード入力
*/

// memo:
// キーを押し続けていると keydown が何度も呼び出される。
// keypress はカーソルキーなどの押下を拾えない。

function <void> @(UNQN)_INIT()
{
	document.addEventListener("keydown", @@_A_KeyDown, false);
	document.addEventListener("keyup",   @@_A_KeyUp,   false);
}

var @@_Status = {};

function <void> @@_A_KeyDown(e)
{
	@@_Status["" + e.keyCode] = true;

	if ((37 <= e.keyCode && e.keyCode <= 40) || e.keyCode == 32) // ? カーソルキー || スペースキー -> 画面スクロール阻止のため動作キャンセルする。
	{
		e.preventDefault();
	}
}

function <void> @@_A_KeyUp(e)
{
	@@_Status["" + e.keyCode] = false;
}

var @@_Counters = {};

function <void> @(UNQN)_EACH()
{
	for (var key in @@_Counters)
	{
		if (@@_Counters[key] == -1)
		{
			delete @@_Counters[key];
		}
		else
		{
			@@_Counters[key]++;
		}
	}

	for (var key in @@_Status)
	{
		if (@@_Status[key])
		{
			if (!(key in @@_Counters))
			{
				@@_Counters[key] = 1;
			}
		}
		else
		{
			if (key in @@_Counters)
			{
				@@_Counters[key] = -1;
			}
		}
	}

	@@_Status = {};
}

// ★★★ ボタン・キー押下は 1 マウス押下は -1 で判定する。

/*
	特定のキーの押下状態を得る。

	key: キーコード

	戻り値：
		-1  == キーを離した。
		0   == キーを押していない。
		1   == キーを押した。
		2～ == キーを押し続けている。
*/
function <int> GetKeyInput(<int> keyCode)
{
	var ret;
	var key = "" + keyCode;

	if (key in @@_Counters)
	{
		ret = @@_Counters[key];
	}
	else
	{
		ret = 0;
	}
	return ret;
}

/*
	全てのキーの押下状態を得る。

	戻り値：連想配列
		キー：キーコードの文字列表現(10進数)
		値：
			-1  == キーを離した。
			1   == キーを押した。
			2～ == キーを押し続けている。

			★押していないキーは連想配列に含まないので値が 0 をとることはない。
*/
function <map> GetAllKeyInput()
{
	return @@_Counters;
}
