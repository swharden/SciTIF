function printPixel(x, y) {
	print("pixel ["+x+", "+y+"] = " + getPixel(x, y));
	print("value ["+x+", "+y+"] = " + getValue(x, y));
}

function printDimensions() {
	getDimensions(width, height, channels, slices, frames);
	print("width = " + width);
	print("height = " + height);
	print("channels = " + channels);
	print("slices = " + slices);
	print("frames = " + frames);
}

print("\\Clear");
titleList = getList("image.titles");
for (i=0; i<titleList.length; i++) {
	selectWindow(titleList[i]);
	print("");
	print("title = " + getTitle());
	printDimensions();
	print("depth = " + bitDepth());
	print("grayscale = " + is("grayscale"));
	printPixel(0, 0);
	printPixel(7, 13);
	printPixel(13, 17);
	printPixel(37, 42);
}
