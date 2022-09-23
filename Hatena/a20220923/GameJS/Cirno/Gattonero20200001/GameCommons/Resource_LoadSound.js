/*
	âπäyì«Ç›çûÇ›
*/

/@(ASTR)

/// Sound_t
{
	<Audio> Handle
}

@(ASTR)/

function <Sound_t> LoadSound(<string> url)
{
	LOGPOS();
	Loading++;

	var<map> m = {};

	m.URL = url;
	m.Sound = {};
	m.TryLoadCount = 0;

	if (DEBUG)
	{
		LOGPOS();
		m.Sound.Handle = new Audio(m.URL);
		m.Sound.Handle.load();
		Loading--;
	}
	else
	{
		@@_Standby(m, 100);
	}
	return m.Sound;
}

function <void> @@_Standby(<map> m, <int> millis)
{
	setTimeout(
		function()
		{
			@@_TryLoad(m);
		},
		millis
		);
}

var<boolean> @@_Loading = false;

function <void> @@_TryLoad(<map> m)
{
	if (@@_Loading)
	{
		@@_Standby(m, 100);
		return;
	}
	@@_Loading = true;

	m.Loaded = function()
	{
		LOGPOS();
		m.Sound.Handle.removeEventListener("canplaythrough", m.Loaded);
		m.Sound.Handle.removeEventListener("error", m.Errored);

		m.Loaded = null;
		m.Errored = null;

		Loading--;
		@@_Loading = false;
	};

	m.Errored = function()
	{
		LOGPOS();
		m.Sound.Handle.removeEventListener("canplaythrough", m.Loaded);
		m.Sound.Handle.removeEventListener("error", m.Errored);
		m.Sound.Handle = null;

		m.Loaded = null;
		m.Errored = null;

		if (m.TryLoadCount < 10) // rough limit
		{
			@@_Standby(m, 2000);
			@@_Loading = false;
		}
		else
		{
			error();
		}
	};

	LOGPOS();
	m.Sound.Handle = new Audio(m.URL);
	m.Sound.Handle.addEventListener("canplaythrough", m.Loaded);
	m.Sound.Handle.addEventListener("error", m.Errored);
	m.Sound.Handle.load();
	m.TryLoadCount++;
}
