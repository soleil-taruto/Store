/*
	Œø‰Ê‰¹Ä¶
*/

var<Sound_t[]> @@_Handles = [];

function <void> SE(<SE_t> se)
{
	var handle = se.Handles[se.Index];

	@@_Handles.push(handle);

	se.Index++;
	se.Index %= 3;
}

function <void> @(UNQN)_EACH()
{
	if (ProcFrame % 2 == 0 && 1 <= @@_Handles.length)
	{
		var handle = @@_Handles.shift();

		handle.Handle.play();
	}
}
