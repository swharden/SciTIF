// This ImageJ script displays the value of a pixel for every slice in a stack

x = 13;
y = 17;
makeRectangle(x, y, 1, 1);

print("\\Clear");
getDimensions(width, height, channels, slices, frames);
values = newArray(slices);
for (i=0; i<slices; i++) {
	setSlice(i+1);
	values[i] = getValue(x, y);
}

Array.print(values);
