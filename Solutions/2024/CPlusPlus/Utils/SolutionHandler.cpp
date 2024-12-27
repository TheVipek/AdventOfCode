#include "SolutionHandler.h"

//#include <chrono>
#include <chrono>
#include <format>
#include <fstream>

#include "Day2.h"
#include "Day1.h"

SolutionHandler::SolutionHandler(
    std::string& solutionsFolderAbsPath, std::string& dayNamePrefix, std::string& sampleName, std::string& inputName
    )
    : solutionsFolderAbsPath(solutionsFolderAbsPath), dayNamePrefix(dayNamePrefix), sampleName(sampleName), inputName(inputName)
    {

    solutions[1] = new Day1();
    solutions[2] = new Day2();
}
std::string SolutionHandler::getSample(const int& day) {
    std::string outputPath = solutionsFolderAbsPath + "\\" + dayNamePrefix + std::to_string(day) + "\\" + sampleName;
    std::ifstream file(outputPath);
    std::string text((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
    return text;
}
std::string SolutionHandler::runSample(const int& day) {
    try {
        std::string sampleData = getSample(day);
        SolutionBase* slt = solutions.at(day);

        std::string part1Result = slt->SolvePart1(sampleData);
        std::string part2Result = slt->SolvePart2(sampleData);

        return std::format("Part1: {0} \n Part2: {1} ", part1Result, part2Result);
    } catch (const std::exception& e) {
        return std::format("ERROR: {0}", e.what());
    }
}

std::string SolutionHandler::getFullData(const int& day){
    std::string outputPath = solutionsFolderAbsPath + "\\" + dayNamePrefix + std::to_string(day) + "\\" + inputName;
    std::ifstream file(outputPath);
    std::string text((std::istreambuf_iterator<char>(file)), std::istreambuf_iterator<char>());
    return text;
}

std::string SolutionHandler::runFullData(const int& day) {
    try {
        std::string fullData = getFullData(day);
        SolutionBase* slt = solutions.at(day);

        auto startPart1 = std::chrono::high_resolution_clock::now();
        std::string part1Result = slt->SolvePart1(fullData);
        auto endPart1 = std::chrono::high_resolution_clock::now();
        auto durationPart1 = std::chrono::duration_cast<std::chrono::microseconds >(endPart1 - startPart1).count();

        auto startPart2 = std::chrono::high_resolution_clock::now();
        std::string part2Result = slt->SolvePart2(fullData);
        auto endPart2 = std::chrono::high_resolution_clock::now();
        auto durationPart2 = std::chrono::duration_cast<std::chrono::microseconds >(endPart2 - startPart2).count();


        return std::format("Part1: {} ({} us) \n Part2: {} ({} us) ", part1Result, durationPart1, part2Result, durationPart2);
    } catch (const std::exception& e) {
        return std::format("ERROR: {0}", e.what());
    }
}