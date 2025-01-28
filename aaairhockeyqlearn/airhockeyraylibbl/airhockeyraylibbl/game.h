#pragma once

#ifndef _GAME_
#define _GAME_

#include<iostream>
#include<math.h>
#include<ctime>
#include<cstdio>

#include<raylib.h>
#include"Puck.h"
#include"background.h"
#include"Player.h"
#include"QLearn.h"

class Game {
private:
	Player player;
	Background background;
	Puck puck;
	QLearn qlearn;

	Rectangle leftwall;
	Rectangle rightwall;
	Rectangle topwall1;
	Rectangle bottomwall1;
	Rectangle topwall2;
	Rectangle bottomwall2;

	float aspectRatio1;
	float findtruesize1;
	float newHeight1;
	float newWidth1;

	int playerScore;
	int aiScore;
	int i;
	bool gameStart;

	Font font;

	int frameCount;
	int N;

	Vector2 lastPuckPosition;
	Vector2 lastPuckSpeed;

public:
	Game();
	~Game();
	void Draw();
	void Update();

	void checkPuckCollision();

	void bounceonPaddle(Rectangle playerRect);

	bool checkCollisionWithWalls(Rectangle wall) const;

	bool puckScoredGoal();
	bool puckConcededGoal();
	void resetGameAfterGoal();

	void endofGame();

	void drawScore();
	int determineState();

	bool successfulHit();
	bool missedHit();
};


#endif