/*
	エフェクト
*/

var <generatorForTask[]> @@_Effects = [];

/*
	エフェクトを追加する。

	effect: ジェネレータであること。
		ジェネレータが偽を返すと終了と見なす。
*/
function <void> AddEffect(<generatorForTask> effect)
{
	@@_Effects.push(effect);
}

function <void> @(UNQN)_EACH()
{
	@@_Effects = @@_Effects.filter(<boolean> function(<generatorForTask> effect)
	{
		return effect.next().value;
	});
}
