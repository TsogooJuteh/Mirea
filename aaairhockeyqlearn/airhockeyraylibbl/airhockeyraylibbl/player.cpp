#include"Player.h"


Player::Player(Rectangle player, Rectangle aicpu) : background(screenWidth, screenHeight, desire_width, desire_height) {
	Texture2D b = LoadTexture("Media/Background.png");
	findtruesize1 = screenHeight / (float)b.height;

	newHeight1 = (float)b.height * findtruesize1;
	newWidth1 = (float)b.width * findtruesize1;


	playerspeed = 5;
	aispeed = 5;
	this->player = player/*{ x / 2 - 75, y / 5 * 4, 150, 20 }*/;
	this->aicpu = aicpu/*{ x / 2 - 75, y / 5 - 20, 150, 20 }*/;

	movingLeft = true;
	leftBoundary = 40; 
	rightBoundary = newWidth1 - player.width - 40;
}

Player::~Player() {

}

void Player::Update() {
	HandleInput();
}

void Player::aimoveLeft()
{
	aicpu.x -= aispeed;
	if (aicpu.x <= 40) aicpu.x += aispeed;
}

void Player::aimoveRight()
{
	aicpu.x += aispeed;
	if (aicpu.x >= newWidth1 - aicpu.width - 35) aicpu.x -= aispeed;
}


void Player::draw() {
	DrawRectangleRoundedLines(player, 0.1, 0, 1.5, DARKBLUE);
	DrawRectangleRounded(player, 0, 0, BLUE);
	DrawRectangleRoundedLines(aicpu, 0.1, 0, 1.5, DARKBLUE);
	DrawRectangleRounded(aicpu, 0, 0, BLUE);
}

void Player::moveLeft() {
	player.x -= playerspeed;
	if (player.x <= 40) {
		player.x += playerspeed;
	}
}

void Player::moveRight() {
	player.x += playerspeed;
	if (player.x >= newWidth1 - player.width - 35) {
		player.x -= playerspeed;
	}
}

void Player::resetPosition()
{
	player.x = newWidth1 / 2 - player.width / 2;
	player.y = newHeight1 / 5 * 4;
	aicpu.x = newWidth1 / 2 - aicpu.width / 2;
	aicpu.y = newHeight1 / 5 - aicpu.height;
}

void Player::HandleInput() {
	if (IsKeyDown(KEY_LEFT)) {
		moveLeft();
	}
	if (IsKeyDown(KEY_RIGHT)) {
		moveRight();
	}
}

void Player::autoMove() {
	if (movingLeft) {
		player.x -= playerspeed;
		if (player.x <= leftBoundary) {
			movingLeft = false;
		}
	}
	else {
		player.x += playerspeed;
		if (player.x >= rightBoundary) {
			movingLeft = true;
		}
	}
}