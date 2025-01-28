#pragma once

#include"background.h"

class Ball2d {
public:
	Vector2 position;
	Vector2 speed;
	float radius;
	Color color;
	Rectangle leftwall;
	Rectangle rightwall;
	Rectangle topwall1;
	Rectangle bottomwall1;
	Rectangle topwall2;
	Rectangle bottomwall2;
	Background background;

public:
	Ball2d() = default;
	Ball2d(Vector2 position, Vector2 speed, float radius);

	void Update();
	void Draw();
};