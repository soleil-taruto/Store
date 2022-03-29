/*
	Œø‰Ê‰¹Ä¶
*/

var @@_Handles = [];

function SE(se)
{
	var handle = se.Handles[se.Index];

	@@_Handles.push(handle);

	se.Index++;
	se.Index %= 3;
}

function @(UNQN)_EACH()
{
	if (ProcFrame % 2 == 0 && 1 <= @@_Handles.length)
	{
		var handle = @@_Handles.shift();

		handle.play();
	}
}
