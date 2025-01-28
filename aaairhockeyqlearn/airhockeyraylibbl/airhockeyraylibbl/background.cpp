#include"Background.h"

Background::Background(int screenWidth, int screenHeight, int desire_width, int desire_height) {
	this->screenWidth = screenWidth;
	this->screenHeight = screenHeight;
	this->desire_width = desire_width;
	this->desire_height = desire_height;
	background = LoadTexture("Media/Background.png");
	aspectRatio = (float)background.width / (float)background.height;

	findtruesize = screenHeight / (float)background.height;

	newHeight = (float)(background.height * findtruesize);
	newWidth = (float)(background.width * findtruesize);
}

Background::~Background() {
	UnloadTexture(background);
}

void Background::Update() {

}

void Background::Draw() {

	DrawTextureEx(background,
		{ 0,0 },
		0.0f,
		findtruesize,
		RAYWHITE
	);

}
