#pragma once

#include"constants.h"

#ifndef _BACKGROUND_
#define _BACKGROUND_

class Background {
public:
	int screenHeight;
	int screenWidth;
	int desire_width;
	int desire_height;
	Texture2D background;


	float aspectRatio;

	float findtruesize;

	float newHeight;
	float newWidth;


public:
	Background() = default;
	Background(int screenWidth, int screenHeight, int desire_width, int desire_height);
	~Background();
	void Update();
	void Draw();
};

#endif // !BACKGROUND
