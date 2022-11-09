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
	for (var<int> index = 0; index < @@_Effects.length; index++)
	{
		var<generatorForTask> effect = @@_Effects[index];

		if (effect.next().value) // ? true -> エフェクト継続
		{
			// noop
		}
		else // ? false -> エフェクト終了
		{
			@@_Effects[index] = null;
		}
	}
	RemoveFalse(@@_Effects);
}

// 未使用
/*
function <generatorForTask[]> GetEffects()
{
	return @@_Effects;
}
*/

function <void> ClearAllEffect()
{
	@@_Effects = [];
}

function <generatorForTask[]> EjectEffects()
{
	var ret = @@_Effects;
	@@_Effects = [];
	return ret;
}

function <void> SetEffects(<generatorForTask[]> effects)
{
	@@_Effects = effects;
}
