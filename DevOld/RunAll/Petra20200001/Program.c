#pragma comment(lib, "user32.lib") // for MessageBox

#include <stdio.h>
#include <conio.h>
#include <io.h>
#include <direct.h>
#include <windows.h>
#include <winuser.h> // for MessageBox

static void ShowErrorDialog(void)
{
	MessageBox(NULL, "ERROR !", __argv[0], MB_OK | MB_ICONSTOP | MB_TOPMOST);
}

#define error() \
	{ printf("ERROR %d 0x%x\n", __LINE__, GetLastError()); ShowErrorDialog(); exit(2); }

#define errorCase(status) \
	{ if ((status)) error(); }

#define TEMP_FILE "RunAll.tmp"
#define LINE_BUFF_SIZE 1024

static char *ReadLine(FILE *fp)
{
	static char line[LINE_BUFF_SIZE];
	int index;

	for (index = 0; ; index++)
	{
		int chr = fgetc(fp);

		if (chr == EOF)
		{
			if (!index)
				return NULL;

			break;
		}
		if (chr == '\r')
			continue;

		if (chr == '\n')
			break;

		errorCase(LINE_BUFF_SIZE - 1 <= index);

		line[index] = chr;
	}
	line[index] = '\0';
	return line;
}

static void RunAllFromFile(char *file)
{
	FILE *fp = fopen(file, "rt");

	errorCase(!fp);

	for (; ; )
	{
		char *line = ReadLine(fp);

		if (!line)
			break;

		printf("Run: %s\n", line);
		system(line);
		printf("done\n");
	}
	errorCase(fclose(fp));
}

main()
{
	char *targetDir;
	char *returnDir;

	errorCase(__argc != 2);

	targetDir = __argv[1];

	errorCase(!targetDir);
	errorCase(!*targetDir);
	errorCase(_access(targetDir, 0));

	returnDir = _getcwd(NULL, 0);

	errorCase(!returnDir);
	errorCase(!*returnDir);
	errorCase(_access(returnDir, 0));

	errorCase(_chdir(targetDir));

	system("DIR *.bat /B /ON > " TEMP_FILE " 2> NUL");
	RunAllFromFile(TEMP_FILE);
	errorCase(remove(TEMP_FILE));

	system("DIR *.exe /B /ON > " TEMP_FILE " 2> NUL");
	RunAllFromFile(TEMP_FILE);
	errorCase(remove(TEMP_FILE));

	errorCase(_chdir(returnDir));

	free(returnDir);
}
