/*
	画面リサイズ・イベント
*/

// 画面リサイズ・イベントのリスト
var<Action[]> @@_Reactions = [];

// 画面リサイズ・イベントの追加
function <void> AddResized(<Action> reaction)
{
	@@_Reactions.push(reaction);
}

window.onresize = function()
{
	@@_Resized();
};

var<int> @@_TimerId;

function <void> @@_Resized()
{
	clearTimeout(@@_TimerId);
	@@_TimerId = setTimeout(@@_Resized2, 100);
}

function <void> @@_Resized2()
{
	for (var<Action> event of @@_Reactions)
	{
		event();
	}
}
