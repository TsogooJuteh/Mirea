#include<iostream>
#include <random>
#include <vector>
#include <set>
#include <chrono>
#include <GLFW/glfw3.h>
#include <cstdlib>
#include <ctime>
#include <cmath>
#include <chrono>

struct point {
	float x, y;
    bool operator<(const point& other) const {
        return (y < other.y || x < other.x);
    }
    bool operator==(const point& other) const {
        return (y == other.y && x == other.x);
    }
};

std::vector<point> points;
std::vector<point> hullGraham;
std::vector<point> hullJarvis;
std::set<point> recursive_hull;

int orientation(point p, point q, point r) {
	float val = (q.y - p.y) * (r.x - p.x) - (r.y - p.y) * (q.x - p.x);
	if (val == 0) return 0;
	return (val > 0) ? 1 : 2;
}

int findside(point p, point q, point r) {
    float val = -((q.y - p.y) * (r.x - p.x) - (r.y - p.y) * (q.x - p.x));
    if (val == 0) return 0;
    return (val > 0) ? 1 : -1;
}

float dist(point p, point q, point r) {
    return abs((q.y - p.y) * (r.x - p.x) - (r.y - p.y) * (q.x - p.x));
}



std::vector<point> GrahamAlgo(std::vector<point> points) {
	size_t n = points.size();

    if (n < 3) return points;

    // Найти точку с минимальной y-координатой (и минимальной x при равенстве)
	size_t ymin = 0;
	for (size_t i = 1; i < n; i++) {
		if ((points[i].y < points[ymin].y) || (points[i].y == points[ymin].y && points[i].x < points[ymin].x)) {
			ymin = i;
		}
	}
	
    // Переместить точку P0 в начало массива
	std::swap(points[0], points[ymin]);

	point p0 = points[0];

    // Сортировка точек по полярному углу относительно P0
	std::sort(points.begin() + 1, points.end(), [p0](point a, point b) {
		int o = orientation(p0, a, b);
		if (o == 0) return (std::hypot(p0.x - a.x, p0.y - a.y) < std::hypot(p0.x - b.x, p0.y - b.y));
		return (o == 2);
		});

    // Удаление коллинеарных точек, оставляя самую дальнюю
    size_t m = 1; // Initialize size of modified array
                  // Размер модифицированного массива
    for (size_t i = 1; i < n; i++) {
        while (i < n - 1 && orientation(p0, points[i], points[i + 1]) == 0)
            i++;
        points[m] = points[i];
        m++; 
    }

    if (m < 3) return std::vector<point>(points.begin(), points.begin() + m);

    // Инициализация стека выпуклой оболочки первыми тремя точками
	std::vector<point> hull;
	hull.push_back(points[0]);
	hull.push_back(points[1]);
	hull.push_back(points[2]);

    // Обход оставшихся точек и построение выпуклой оболочки
	for (size_t i = 3; i < n; i++) {
		while (hull.size() > 1 && orientation(*(hull.rbegin() + 1), hull.back(), points[i]) != 2) hull.pop_back();
		hull.push_back(points[i]);
	}
	return hull;
}

//algorithm Jarvis
std::vector<point> jarvisMarch(std::vector<point> points) {
    size_t n = points.size();
    if (n < 3) return points;
    std::vector<point> hull;

    
    size_t l = 0; // Find the leftmost point
    for (size_t i = 1; i < n; i++)
        if (points[i].x < points[l].x)
            l = i;

    size_t p = l, q;
    do {
        hull.push_back(points[p]);
        q = (p + 1) % n;
        for (size_t i = 0; i < n; i++)
            if (orientation(points[p], points[i], points[q]) == 2)
                q = i;
        p = q;
    } while (p != l);
    return hull;
}

// Recursive Convex Hull (QuickHull)
void quickHullUtil(std::vector<point>& points, point A, point B, int side, std::vector<point>& hull) {
    int idx = -1;
    float max_dist = 0;

    for (size_t i = 0; i < points.size(); i++) {
        float temp = dist(A, B, points[i]);
        if (findside(A, B, points[i]) == side && temp > max_dist) {
            idx = i;
            max_dist = temp;
        }
    }

    if (idx == -1) {
        hull.push_back(A);
        hull.push_back(B);
        return;
    }

    quickHullUtil(points, points[idx], A, -findside(points[idx], A, B), hull);
    quickHullUtil(points, points[idx], B, -findside(points[idx], B, A), hull);
}

std::vector<point> quickHull(std::vector<point> points) {
    std::vector<point> hull;
    if (points.size() < 3) return points;

    // Find the min and max x-coordinate points
    size_t min_x = 0, max_x = 0;
    for (size_t i = 1; i < points.size(); i++) {
        if (points[i].x < points[min_x].x)
            min_x = i;
        if (points[i].x > points[max_x].x)
            max_x = i;
    }

    quickHullUtil(points, points[min_x], points[max_x], 1, hull);
    quickHullUtil(points, points[min_x], points[max_x], -1, hull);

    // Remove duplicate points
    std::sort(hull.begin(), hull.end(), [](point a, point b) {
        return (a.x < b.x) || (a.x == b.x && a.y < b.y);
        });
    hull.erase(std::unique(hull.begin(), hull.end(), [](point a, point b) {
        return a.x == b.x && a.y == b.y;
        }), hull.end());

    return hull;
}

// Function to draw points and hull
void drawpointsAndHull(const std::vector<point>& points, const std::vector<point>& hull) {
    if (!glfwInit()) return;
    GLFWwindow* window = glfwCreateWindow(800, 800, "Convex Hull", NULL, NULL);
    if (!window) {
        glfwTerminate();
        return;
    }
    glfwMakeContextCurrent(window);

    // Main loop
    while (!glfwWindowShouldClose(window)) {
        glClear(GL_COLOR_BUFFER_BIT);

        glPointSize(5.0f);
        glBegin(GL_POINTS);
        for (const auto& p : points) {
            glVertex2d(p.x / 100.0, p.y / 100.0);
        }
        glEnd();

        glBegin(GL_LINE_LOOP);
        for (const auto& p : hull) {
            glVertex2d(p.x / 100.0, p.y / 100.0);
        }
        glEnd();

        glfwSwapBuffers(window);
        glfwPollEvents();
    }
    glfwTerminate();
}

int main() {
    std::vector<point> test_points = {
        {1, 1}, {2, 5}, {1, 9}, {4, 3},
        {6, 4}, {5, 7}, {8, 2}, {8, 9}
    };

    std::vector<point> hull = GrahamAlgo(test_points);

    drawpointsAndHull(test_points, hull);

    // Generate 1000 random points
    std::srand(std::time(0));
    std::vector<point> random_points(1000);
    for (auto& p : random_points) {
        p.x = -100 + std::rand() % 201;
        p.y = -100 + std::rand() % 201;
    }

    // Time Graham's Scan
    auto start = std::chrono::high_resolution_clock::now();
    std::vector<point> graham_hull = GrahamAlgo(random_points);
    auto end = std::chrono::high_resolution_clock::now();
    auto graham_time = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();

    // Time Jarvis's March
    start = std::chrono::high_resolution_clock::now();
    std::vector<point> jarvis_hull = jarvisMarch(random_points);
    end = std::chrono::high_resolution_clock::now();
    auto jarvis_time = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();

    // Time Recursive Convex Hull
    start = std::chrono::high_resolution_clock::now();
    std::vector<point> quick_hull = quickHull(random_points);
    end = std::chrono::high_resolution_clock::now();
    auto quick_time = std::chrono::duration_cast<std::chrono::microseconds>(end - start).count();

    std::cout << "Graham's Scan time: " << graham_time << " microseconds\n";
    std::cout << "Jarvis's March time: " << jarvis_time << " microseconds\n";
    std::cout << "QuickHull time: " << quick_time << " microseconds\n";

    return 0;
}