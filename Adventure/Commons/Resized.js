/*
	画面リサイズ・イベント
*/

// 画面リサイズ・イベントのリスト
var @@_Reactions = [];

// 画面リサイズ・イベントの追加
function AddResized(reaction)
{
	@@_Reactions.push(reaction);
}

window.onresize = function()
{
	@@_Resized();
};

var @@_TimerId;

function @@_Resized()
{
	clearTimeout(@@_TimerId);
	@@_TimerId = setTimeout(@@_Resized_Main, 100);
}

function @@_Resized_Main()
{
	for (var event of @@_Reactions)
	{
		event();
	}
}
