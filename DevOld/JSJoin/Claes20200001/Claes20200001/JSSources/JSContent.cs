using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Charlotte.JSSources
{
	public class JSContent
	{
		public JSSourceFile SourceFile;
		public int LineNumb;
		public string[] Lines;

		// <---- prm

		public enum Kind_e
		{
			TEMPLATE = 1,
			FUNCTION,
			VARIABLE,
		}

		public Kind_e Kind;

		public void LoadKind()
		{
			this.Kind = this.GetKind();
		}

		private Kind_e GetKind()
		{
			string line = this.Lines[0];
			JSToken[] tokens = JSTokenTools.I.Tokenize(line).ToArray();

			if (
				2 <= tokens.Length &&
				tokens[0].Kind == JSToken.Kind_e.WORD &&
				tokens[1].Kind == JSToken.Kind_e.WORD &&
				tokens[1].Text == "class"
				)
				return Kind_e.TEMPLATE;

			if (
				4 <= tokens.Length &&
				tokens[0].Kind == JSToken.Kind_e.WORD &&
				tokens[1].Kind == JSToken.Kind_e.WORD &&
				tokens[2].Kind == JSToken.Kind_e.WORD &&
				tokens[3].Kind == JSToken.Kind_e.SYMBOL &&
				tokens[3].Text == "("
				)
				return Kind_e.FUNCTION;

			if (
				4 <= tokens.Length &&
				tokens[0].Kind == JSToken.Kind_e.WORD &&
				tokens[1].Kind == JSToken.Kind_e.WORD &&
				tokens[2].Kind == JSToken.Kind_e.WORD &&
				tokens[3].Kind == JSToken.Kind_e.SYMBOL &&
				(
					tokens[3].Text == "=" ||
					tokens[3].Text == ";"
				))
				return Kind_e.VARIABLE;

			throw new Exception("不明なコンテンツを検出しました。" + this.SourceFile.OriginalFilePath + " (" + this.LineNumb + ")");
		}

		public enum Scope_e
		{
			PUBLIC = 1,
			PRIVATE,
		}

		private Scope_e Scope;

		public void LoadScope()
		{
			this.Scope = this.GetScope();
		}

		private Scope_e GetScope()
		{
			string line = this.Lines[0];
			JSToken[] tokens = JSTokenTools.I.Tokenize(line).ToArray();

			if (
				1 <= tokens.Length &&
				tokens[0].Kind == JSToken.Kind_e.WORD &&
				tokens[0].Text == "public"
				)
				return Scope_e.PUBLIC;

			if (
				1 <= tokens.Length &&
				tokens[0].Kind == JSToken.Kind_e.WORD &&
				tokens[0].Text == "private"
				)
				return Scope_e.PRIVATE;

			throw new Exception("不明なスコープを検出しました。" + this.SourceFile.OriginalFilePath + " (" + this.LineNumb + ")");
		}
	}
}
