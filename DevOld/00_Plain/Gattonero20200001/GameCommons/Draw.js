/*
	•`‰æ
*/

// ‰æ‘œ‚Ì•‚ğæ“¾‚·‚éB
function <int> GetPicture_W(<Picture_t> picture)
{
	return picture.Handle.naturalWidth;
}

// ‰æ‘œ‚Ì‚‚³‚ğæ“¾‚·‚éB
function <int> GetPicture_H(<Picture_t> picture)
{
	return picture.Handle.naturalHeight;
}

// ƒXƒNƒŠ[ƒ“‚ÌƒNƒŠƒA
function <void> ClearScreen()
{
	Context.clearRect(0, 0, Screen_W, Screen_H);
}

/*
	•`‰æ

	x: ‰æ‘œ‚Ì’†S‚Æ‚·‚é X-À•W
	y: ‰æ‘œ‚Ì’†S‚Æ‚·‚é Y-À•W
	a: •s“§–¾“x (0.0 “§–¾ ` 1.0 •s“§–¾)
	r: ‰ñ“]
		0.0 == ‰ñ“]–³‚µ
		2*PI == Œv‰ñ‚è‚É1‰ñ“]
		-2*PI == ”½Œv‰ñ‚è‚É1‰ñ“]
	z: Šg‘å—¦
		1.0 == “™”{
		2.0 == 2”{
		0.5 == 0.5”{
*/
function <void> Draw(<Picture_t> picture, <double> x, <double> y, <double> a, <double> r, <double> z)
{
	Draw2(picture, x, y, a, r, z, z);
}

function <void> Draw2(<Picture_t> picture, <double> x, <double> y, <double> a, <double> r, <double> zw, <double> zh)
{
	var<int> w = GetPicture_W(picture);
	var<int> h = GetPicture_H(picture);

	w *= zw;
	h *= zh;

	var<double> l = x - w / 2;
	var<double> t = y - h / 2;

	Context.translate(x, y);
	Context.rotate(r);
	Context.translate(-x, -y);
	Context.globalAlpha = a;

	Context.drawImage(picture.Handle, l, t, w, h);

	// restore
	Context.translate(x, y);
	Context.rotate(-r);
	Context.translate(-x, -y);
	Context.globalAlpha = 1.0;

	@@_CheckHover(picture, CreateD4Rect(l, t, w, h));
}

var<Picture_t> HoveredPicture = null;
var<Picture_t> @@_CH_CurrHoveredPicture = null;
var<int> @@_CH_LastProcFrame = 0;

function <void> @@_CheckHover(<Picture_t> picture, <D4Rect_t> pictureRect)
{
	if (@@_CH_LastProcFrame != ProcFrame)
	{
		HoveredPicture = @@_CH_CurrHoveredPicture;
		@@_CH_CurrHoveredPicture = null;
		@@_CH_LastProcFrame = ProcFrame;
	}

	var<D2Point_t> mousePt = CreateD2Point(GetMouseX(), GetMouseY());

	if (!IsOut(mousePt, pictureRect, 0.0))
	{
		@@_CH_CurrHoveredPicture = picture;
	}
}
