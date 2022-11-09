using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Commons;

namespace Charlotte.JSConfusers
{
	public class JSResource
	{
		#region 予約語リスト

		public static string RES_予約語リスト = @"

; https://www.javadrive.jp/javascript/ini/index5.html
; 予約語の一覧 (ECMAScript 2020)

await
break
case
catch
class
const
continue
debugger
default
delete
do
else
enum
export
extends
false
finally
for
function
if
import
in
instanceof
new
null
return
super
switch
this
throw
true
try
typeof
var
void
while
with
yield

; 他に strict mode で予約語として登録されているものがあります。

let
static
implements
interface
package
private
protected
public

; また将来の予約語として登録されているものは次の通りです。

enum

; ----

; 予約語

of

; よく使うオブジェクト？

console
document
window

; クラス

Date
Math
Image
Audio
Number

; 関数

clearTimeout
setTimeout
requestAnimationFrame

";

		public static string[] 予約語リスト = SCommon.TextToLines(RES_予約語リスト)
			.Select(v => v.Trim())
			.Where(v => v != "" && v[0] != ';') // 空行とコメント行を除去
			.Distinct()
			.ToArray();

		#endregion
	}
}
