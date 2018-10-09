saveFolder="D:/demoData/tifs/simple/corners/";
for (width=11; width<25; width++){
	height = 11;
	newImage("Untitled", "8-bit black", width, height, 1);
	makeRectangle(0, 0, 2, 2); setForegroundColor(255, 255, 255); run("Fill", "slice");
	makeRectangle(width-1, 0, 1, 1); setForegroundColor(201, 201, 201); run("Fill", "slice");
	makeRectangle(0, height-1, 1, 1); setForegroundColor(91, 91, 91); run("Fill", "slice");
	makeRectangle(width-1, height-1, 1, 1); setForegroundColor(36, 36, 36); run("Fill", "slice");
	filename = "corners_"+width+"x"+height+".tif";
	filePath = saveFolder+filename;
	print(filePath);
	saveAs("Tiff", filePath);
	close();
}