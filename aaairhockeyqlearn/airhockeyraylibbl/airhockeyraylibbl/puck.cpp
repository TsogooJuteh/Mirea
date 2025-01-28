#include"puck.h"

Puck::Puck(Vector2 position, Vector2 speed, float radius) {
	this->position = position;
	this->speed = speed;
	this->radius = radius;
	color = BLACK;

	Texture2D b = LoadTexture("Media/Background.png");
	findtruesize1 = screenHeight / (float)b.height;

	newHeight1 = (float)b.height * findtruesize1;
	newWidth1 = (float)b.width * findtruesize1;
	newPos = { position.x + speed.x, position.y + speed.y };
}

void Puck::draw() {
	DrawCircleV(position, radius, color);

	DrawCircleLines(position.x, position.y, radius, GREEN);
}

void Puck::update() {
	position.x += speed.x;
	position.y += speed.y;
}