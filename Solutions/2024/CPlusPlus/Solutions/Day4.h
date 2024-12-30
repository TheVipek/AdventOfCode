#ifndef DAY4_H
#define DAY4_H
#include <vector>

#include "SolutionBase.h"

class Day4 : public SolutionBase {
public:
    std::string SolvePart1(std::string input) override;
    std::string SolvePart2(std::string input) override;
private:
    std::vector<std::pair<int , int>> directions = {{-1, 0}, {1, 0}, {0, -1}, {0, 1},
                                                       {-1, -1}, {-1, 1}, {1, -1}, {1, 1}};
    std::vector<char> charSequence = { 'X', 'M', 'A', 'S'};
    int correctXMASFromPosition(const std::vector<std::vector<char>>& translatedData,int& x, int& y, const std::vector<char>& charSequence);
};
#endif //DAY4_H
