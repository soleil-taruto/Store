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
	{ printf("ERROR %d 0x%x\n", __LINE__, GetLastError()); ShowErrorDialog(); exit(1); }

#define errorCase(status) \
	{ if ((status)) error(); }

#define TEMP_FILE "RunSub_{639ec312-0240-4736-8f30-3745d1ed29fc}.tmp"
#define LINE_BUFF_SIZE 1024
#define TARGET_FILE_NAME_BUFF_SIZE 1024

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

static int ExistFile(char *file)
{
	FILE *fp = fopen(file, "rb");

	if (fp)
	{
		errorCase(fclose(fp));
		return 1;
	}
	return 0;
}

static char *TargetNode;

static void TryRun(char *extension, char *currDir4Prn)
{
	static char line[TARGET_FILE_NAME_BUFF_SIZE];

	errorCase(TARGET_FILE_NAME_BUFF_SIZE < strlen(TargetNode) + strlen(extension) + 1);

	strcpy(line, TargetNode);
	strcat(line, extension);

	if (ExistFile(line))
	{
		printf("Run: %s -> %s\n", currDir4Prn, line);
		system(line);
		printf("done\n");
	}
}

static void RunSubFromFile(char *file)
{
	FILE *fp = fopen(file, "rt");

	errorCase(!fp);

	for (; ; )
	{
		char *line = ReadLine(fp);

		if (!line)
			break;

		errorCase(strchr(line, '?')); // ? ユニコードが含まれている。

		// ローカルディスク上のルートディレクトリではない絶対パスであるはず。
		{
			errorCase(!line[0]);
			errorCase(line[1] != ':');
			errorCase(line[2] != '\\');
			errorCase(!line[3]);
		}

		// 実行したバッチやプログラムによって検出したディレクトリが削除されている場合も考えられる。
		// その場合はスキップする。
		//
		if (_access(line, 0))
			continue;

		errorCase(_chdir(line));

		TryRun(".bat", line);
		TryRun(".exe", line);
	}
	errorCase(fclose(fp));
}

main()
{
	char *targetDir;
	char *returnDir;

	errorCase(__argc != 3);

	targetDir = __argv[1];

	errorCase(!targetDir);
	errorCase(!*targetDir);
	errorCase(_access(targetDir, 0));

	targetDir = _fullpath(NULL, targetDir, 0); // RunSubFromFile() 内であちこち移動するので、フルパス化が必要。

	errorCase(!targetDir);
	errorCase(!*targetDir);
	errorCase(_access(targetDir, 0));

	TargetNode = __argv[2];

	errorCase(!TargetNode);
	errorCase(!*TargetNode);

	returnDir = _getcwd(NULL, 0);

	errorCase(!returnDir);
	errorCase(!*returnDir);
	errorCase(_access(returnDir, 0));

	errorCase(_chdir(targetDir));

	system("DIR /AD /B /ON /S > " TEMP_FILE " 2> NUL");
	RunSubFromFile(TEMP_FILE);
	errorCase(_chdir(targetDir)); // RunSubFromFile() 内であちこち移動するので、ここで帰ってくるようにする。
	errorCase(remove(TEMP_FILE));

	errorCase(_chdir(returnDir));

	free(targetDir);
	free(returnDir);
}
