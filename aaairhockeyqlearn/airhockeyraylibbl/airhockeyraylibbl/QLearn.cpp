#include "QLearn.h"
#include <math.h>
#include <stdlib.h>
#include <algorithm>

QLearn::QLearn(double alpha, double gamma, double epsilon, int numStates, int numActions)
    : alpha(alpha), gamma(gamma), epsilon(epsilon), totalPlays(0) {
    qTable.resize(numStates, std::vector<double>(numActions, 0.0));
    playCounts.resize(numActions, 0);
}

QLearn::~QLearn() {}

int QLearn::chooseAction(int state) {
    totalPlays++;

    if ((double)rand() / RAND_MAX < epsilon) {
        return rand() % qTable[state].size();
    }
    else {
        double bestValue = -INFINITY;
        int bestAction = 0;
        for (int a = 0; a < qTable[state].size(); a++) {
            double ucbValue = getUCBValue(state, a);
            if (ucbValue > bestValue) {
                bestValue = ucbValue;
                bestAction = a;
            }
        }
        return bestAction;
    }
}

void QLearn::update(int state, int action, int reward, int newState) {
    double bestFutureQ = *std::max_element(qTable[newState].begin(), qTable[newState].end());
    qTable[state][action] += alpha * (reward + gamma * bestFutureQ - qTable[state][action]);
    playCounts[action]++;
}

void QLearn::decreaseEpsilon(double decayRate) {
    epsilon *= decayRate;
}

void QLearn::reset() {
    std::fill(playCounts.begin(), playCounts.end(), 0);
    totalPlays = 0;
}

double QLearn::getUCBValue(int state, int action) {
    if (playCounts[action] == 0) return INFINITY;
    double exploitation = qTable[state][action];
    double exploration = sqrt(2 * log(totalPlays) / playCounts[action]);
    return exploitation + exploration;
}
