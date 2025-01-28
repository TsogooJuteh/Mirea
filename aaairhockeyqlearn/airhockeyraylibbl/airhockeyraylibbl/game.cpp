#include"game.h"

Game::Game() : background(screenWidth, screenHeight, desire_width, desire_height), aiScore(0), playerScore(0), qlearn(0.5, 0.8, 1, 2, 4), frameCount(0) {
	Texture2D back = LoadTexture("Media/Background.png");
	aspectRatio1 = (float)back.width / (float)back.height;

	findtruesize1 = screenHeight / (float)back.height;

	newHeight1 = (float)back.height * findtruesize1;
	newWidth1 = (float)back.width * findtruesize1;

	player = Player({ newWidth1 / 2 - 75, newHeight1 / 5 * 4, 150, 20 }, { newWidth1 / 2 - 75, newHeight1 / 5 - 20, 150, 20 });
	puck = Puck({ newWidth1 / 2, newHeight1 / 2 }, { -10 ,-10 }, 10);

	leftwall = { 0,0, 20, newHeight1 };
	rightwall = { newWidth1 - 20, 0, 20, newHeight1 };
	topwall1 = { 0, 20, newWidth1 / 3 - 10, 0 };
	topwall2 = { newWidth1 - newWidth1 / 3 + 10, 20, newWidth1 / 3, 0 };
	bottomwall1 = { 0, newHeight1 - 20, newWidth1 / 3 - 10 , 0 };
	bottomwall2 = { newWidth1 - newWidth1 / 3 + 10, newHeight1 - 20, newWidth1 / 3 , 0 };

	i = 0;
	gameStart = true;

	font = LoadFontEx("Media/monogram.ttf", 64, 0, 0);

	N = 300;

	lastPuckSpeed = { 0, 0 };
	lastPuckPosition = puck.position;
}

Game::~Game() {

}

void Game::Draw() {
	background.Draw();
	player.draw();
	puck.draw();
	drawScore();
}
void Game::Update() {
	frameCount++;
	player.Update();
	player.autoMove();
	int state = puck.position.x < player.aicpu.x ? 0 : 1;
	int action = qlearn.chooseAction(state);


	if (action == 0) {
		player.aimoveLeft();
	}
	else {
		player.aimoveRight();
	}

	puck.update();

	Vector2 newPos = { puck.position.x + puck.speed.x, puck.position.y + puck.speed.y };
	checkPuckCollision();
	if (CheckCollisionCircleRec(newPos, puck.radius, player.player)) {
		bounceonPaddle(player.player);
	}
	if (CheckCollisionCircleRec(newPos, puck.radius, player.aicpu)) {
		bounceonPaddle(player.aicpu);
	}
	

	int reward = 0;

	if (successfulHit()) {
		reward += 2;
	}
	if (missedHit()) {
		reward -= 2;
	}

	if (puckScoredGoal()) {
		reward += 10;
		endofGame();
	}
	if (puckConcededGoal()) {
		reward -= 10;
		endofGame();
	}

	int newState = determineState();
	qlearn.update(state, action, reward, newState);

	lastPuckSpeed = puck.speed;
	lastPuckPosition = puck.position;

	resetGameAfterGoal();

}

void Game::checkPuckCollision() {
	if (CheckCollisionCircleRec({ puck.position.x + puck.speed.x, puck.position.y }, puck.radius, leftwall) ||
		CheckCollisionCircleRec({ puck.position.x + puck.speed.x, puck.position.y }, puck.radius, rightwall)) {
		puck.speed.x = -puck.speed.x;
	}

	if (CheckCollisionCircleRec({ puck.position.x, puck.position.y + puck.speed.y }, puck.radius, topwall1) ||
		CheckCollisionCircleRec({ puck.position.x, puck.position.y + puck.speed.y }, puck.radius, topwall2) ||
		CheckCollisionCircleRec({ puck.position.x, puck.position.y + puck.speed.y }, puck.radius, bottomwall1) ||
		CheckCollisionCircleRec({ puck.position.x, puck.position.y + puck.speed.y }, puck.radius, bottomwall2)) {
		puck.speed.y = -puck.speed.y;
	}
}

void Game::bounceonPaddle(Rectangle playerRect) {

	Vector2 rs = { puck.position.x + puck.radius, puck.position.y + puck.radius };
	Vector2 ls = { puck.position.x - puck.radius, puck.position.y + puck.radius };
	Vector2 rn = { puck.position.x + puck.radius, puck.position.y - puck.radius };
	Vector2 ln = { puck.position.x - puck.radius, puck.position.y - puck.radius };

	if ((rs.x + puck.speed.x <= playerRect.x - player.playerspeed && rs.y + puck.speed.y <= playerRect.y)) {
		puck.speed.y = -puck.speed.y;
	}
	else if (ls.x + puck.speed.x >= playerRect.x + playerRect.width + player.playerspeed && ls.y + puck.speed.y <= playerRect.y) {
		puck.speed.y = -puck.speed.y;
	}
	else if (rn.x + puck.speed.x <= playerRect.x  - player.playerspeed && rn.y + puck.speed.y >= playerRect.y + playerRect.height) {
		puck.speed.y = -puck.speed.y;
	}
	else if (ln.x + puck.speed.x >= playerRect.x + playerRect.width + player.playerspeed && ln.y + puck.speed.y >= playerRect.y + playerRect.height) {
		puck.speed.y = -puck.speed.y;
	}

	if ((puck.position.x >= playerRect.x && puck.position.x <= playerRect.x + playerRect.width) && puck.position.y <= playerRect.y) {
		puck.speed.y = -puck.speed.y;
	}
	else if ((puck.position.x >= playerRect.x && puck.position.x <= playerRect.x + playerRect.width) && puck.position.y >= playerRect.y + playerRect.height) {
		puck.speed.y = -puck.speed.y;
	}
	else if ((puck.position.y - 2 * puck.speed.y >= playerRect.y && puck.position.y + 2 * puck.speed.y <= playerRect.y + playerRect.height) && puck.position.x <= playerRect.x) {
		puck.speed.x = -puck.speed.x;
	}
	else if ((puck.position.y >= playerRect.y && puck.position.y <= playerRect.y + playerRect.height) && puck.position.x >= playerRect.x + playerRect.width) {
		puck.speed.x = -puck.speed.x;
	}
}

bool Game::checkCollisionWithWalls(Rectangle wall) const {
	Vector2 newPos = { puck.position.x + puck.speed.x, puck.position.y + puck.speed.y };
	return CheckCollisionCircleRec(newPos, puck.radius, wall);
}

void Game::endofGame()
{
	qlearn.decreaseEpsilon(0.95); 
	qlearn.epsilon = std::max(qlearn.epsilon, 0.01);
}

bool Game::puckScoredGoal() {
	if (puck.position.y > newHeight1) {
		return true;
	}
	return false;
}
bool Game::puckConcededGoal() {
	if (puck.position.y + puck.radius < 20) {
		return true;
	}
	return false;
}

void Game::resetGameAfterGoal()
{
	if (puckConcededGoal()) {
		puck.position.x = newWidth1 / 2;
		puck.position.y = newHeight1 / 2;
		player.resetPosition();
		playerScore++; 
		puck.speed.x = GetRandomValue(-10, 10);
		puck.speed.y = GetRandomValue(-10, 10);
		if (puck.speed.x >= 0 && puck.speed.x < 5) {
			puck.speed.x += 5;
		}
		else if (puck.speed.x < 0 && puck.speed.x > -5) {
			puck.speed.x -= 5;
		}
		if (puck.speed.y >= 0 && puck.speed.y < 5) {
			puck.speed.y += 5;
		}
		else if (puck.speed.y < 0 && puck.speed.y > -5) {
			puck.speed.y -= 5;
		}
	}
	else if (puckScoredGoal()) {
		puck.position.x = newWidth1 / 2;
		puck.position.y = newHeight1 / 2;
		player.resetPosition();
		aiScore++;
		puck.speed.x = GetRandomValue(-10, 10);
		puck.speed.y = GetRandomValue(-10, 10);
		if (puck.speed.x >= 0 && puck.speed.x < 5) {
			puck.speed.x += 5;
		}
		else if (puck.speed.x < 0 && puck.speed.x > -5) {
			puck.speed.x -= 5;
		}
		if (puck.speed.y >= 0 && puck.speed.y < 5) {
			puck.speed.y += 5;
		}
		else if (puck.speed.y < 0 && puck.speed.y > -5) {
			puck.speed.y -= 5;
		}
	}
}

void Game::drawScore()
{
	DrawTextEx(font, "AI", { screenWidth / 3 * 2, screenHeight / 6 }, 40, 0, BLACK);
	DrawTextEx(font, "Player", { screenWidth / 3 * 2, screenHeight / 5 * 4 - 10 }, 40, 0, BLACK);
	DrawRectangle(screenWidth / 6 * 5, screenHeight / 7 + 20, 100, 50, WHITE);
	DrawRectangle(screenWidth / 6 * 5, screenHeight / 4 * 3 + 30, 100, 50, WHITE);

	char scoreText[10];
	sprintf_s(scoreText, "%d", playerScore);
	DrawTextEx(font, scoreText, { screenWidth / 6 * 5 + 40 , screenHeight / 4 * 3 + 30 }, 40, 0, BLACK);

	char aiscoreText[10];
	sprintf_s(aiscoreText, "%d", aiScore);
	DrawTextEx(font, aiscoreText, { screenWidth / 6 * 5 + 40 , screenHeight / 6 + 5 }, 40, 0, BLACK);
}

int Game::determineState()
{
	return puck.position.x < player.aicpu.x ? 0 : 1;
}

bool Game::successfulHit() {
	if (CheckCollisionCircleRec(puck.position, puck.radius, player.aicpu)) {
		if (puck.speed.y > 0 && lastPuckSpeed.y <= 0) {
			return true;
		}
	}
	return false;
}

bool Game::missedHit() {
	if (puck.position.y > player.aicpu.y + player.aicpu.height && lastPuckPosition.y <= player.aicpu.y + player.aicpu.height) {
		if (puck.position.x > player.aicpu.x && puck.position.x < player.aicpu.x + player.aicpu.width) {
			return true;
		}
	}
	return false;
}