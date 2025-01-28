#include"game.h"

int main() {
	InitWindow(screenWidth, screenHeight, "Air Hockey w/ RL");

	Game game;


	SetTargetFPS(144);

	while (!WindowShouldClose()) {
		BeginDrawing();
		ClearBackground(GRAY);
		game.Update();
		game.Draw();
		EndDrawing();
	}

	CloseWindow();

	return 0;
}