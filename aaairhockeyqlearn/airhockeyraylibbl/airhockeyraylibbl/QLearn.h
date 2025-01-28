#pragma once
#include <vector>

class QLearn {
public:
    double alpha; // learning rate
    double gamma; // discount factor
    double epsilon; // exploration rate
    std::vector<std::vector<double>> qTable; // q-table
    double totalPlays; // total plays UCB
    std::vector<double> playCounts; // count of plays

public:
    QLearn(double alpha, double gamma, double epsilon, int numStates, int numActions);
    ~QLearn();

    int chooseAction(int state);
    void update(int state, int action, int reward, int newState);
    void decreaseEpsilon(double decayRate);
    void reset();

    double getUCBValue(int state, int action);
};
