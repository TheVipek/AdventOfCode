#ifndef SOLUTIONBASE_H
#define SOLUTIONBASE_H

#include <string>

class SolutionBase{
  public:
    virtual std::string SolvePart1(std::string input) = 0;
    virtual std::string SolvePart2(std::string input) = 0;
};

#endif //SOLUTIONBASE_H
