#pragma once

#include<iostream>
#include<chrono>
#include<vector>
#include"background.h"

class Player {
public:
	Rectangle aicpu;
	Background background;
	float playerspeed;
	float aispeed;
	float findtruesize1;
	float newHeight1;
	float newWidth1;

	bool movingLeft;
	float leftBoundary;
	float rightBoundary;

public:
	Rectangle player;

	Player() = default;
	Player(Rectangle player, Rectangle aicpu);
	~Player();

	void resetPosition();

	void moveRight();
	void moveLeft();

	void aimoveRight();
	void aimoveLeft();

	void Update();
	void HandleInput();
	void draw();

	void autoMove();
};