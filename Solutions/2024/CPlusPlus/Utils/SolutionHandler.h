#ifndef SOLUTIONHANDLER_H
#define SOLUTIONHANDLER_H

#include <map>
#include <string>

#include "SolutionBase.h"

class SolutionHandler
{
public:
    SolutionHandler(
        std::string& solutionsFolderAbsPath, std::string& dayNamePrefix, std::string& sampleName, std::string& inputName
        );
    std::string getSample(const int& day);
    std::string runSample(const int& day);
    std::string getFullData(const int& day);
    std::string runFullData(const int& day);
private:
    std::string solutionsFolderAbsPath;
    std::string dayNamePrefix;
    std::string sampleName;
    std::string inputName;

    std::pmr::map<int, SolutionBase*> solutions;
};

#endif
