/*
	•`‰æ
*/

// ƒXƒNƒŠ[ƒ“‚ÌƒNƒŠƒA
function ClearScreen()
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
function Draw(image, x, y, a, r, z)
{
	var w = image.naturalWidth;
	var h = image.naturalHeight;

	w *= z;
	h *= z;

	var l = x - w / 2;
	var t = y - h / 2;

	Context.translate(x, y);
	Context.rotate(r);
	Context.translate(-x, -y);
	Context.globalAlpha = a;

	Context.drawImage(image, l, t, w, h);

	// restore
	Context.translate(x, y);
	Context.rotate(-r);
	Context.translate(-x, -y);
	Context.globalAlpha = 1.0;
}
