#pragma once

#include"background.h"

class Puck {
public:
	Vector2 position;
	Vector2 speed;
	float radius;
	Vector2 newPos;

	Color color;

	float findtruesize1;
	float newHeight1;
	float newWidth1;

public:
	Puck() = default;
	Puck(Vector2 position, Vector2 speed, float radius);

	void draw();
	void update();

};