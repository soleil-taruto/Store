/*
	共通エフェクト
*/

var @@_Effects = [];

/*
	エフェクトを追加する。

	effect: ジェネレータであること。
		ジェネレータが偽を返すと終了と見なす。
*/
function AddCommonEffect(effect)
{
	@@_Effects.push(effect);
}

function @(UNQN)_EACH()
{
	@@_Effects = @@_Effects.filter(function(effect)
	{
		return effect.next().value;
	});
}
