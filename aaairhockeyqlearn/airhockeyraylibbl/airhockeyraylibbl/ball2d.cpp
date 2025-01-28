#include"ball2d.h"

Ball2d::Ball2d(Vector2 position, Vector2 speed, float radius) {
	this->position = position;
	this->speed = speed;
	this->radius = radius;
	color = BLACK;

	Texture2D back = LoadTexture("Media/Background.png");

	float aspectRatio = (float)back.width / (float)back.height;

	float findtruesize = screenHeight / (float)back.height;

	float newHeight = desire_height;
	float newWidth = (float)(desire_height * aspectRatio);

	leftwall = { 0,0, 20, newHeight };
	rightwall = { newWidth - 20, 0, 20, newHeight };
	topwall1 = { 0, -5, newWidth / 3, 20 };
	topwall2 = { newWidth - newWidth / 3, -5, newWidth / 3, 20 };
	bottomwall1 = { 0, newHeight - 20,newWidth / 3 , 30 };
	bottomwall2 = { newWidth - newWidth / 3, newHeight - 20, newWidth / 3 , 30 };
}

void Ball2d::Draw() {
	DrawCircleV(position, radius, color);

	DrawRectangle(topwall1.x, topwall1.y, topwall1.width, topwall2.height, WHITE);
}

void Ball2d::Update() {

}