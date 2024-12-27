#include "Day3.h"

#include <format>
#include <iostream>
#include <regex>
std::string Day3::SolvePart1(std::string input) {

    std::regex pattern(R"(mul\((\d+),(\d+)\))");

    int result = 0;
    for (std::sregex_iterator it(input.begin(), input.end(), pattern), end; it != end; ++it) {
        std::smatch match = *it;
        int firstNumber = std::stoi(match[1]);
        int secondNumber = std::stoi(match[2]);

        result += firstNumber * secondNumber;
    }

    return std::format("Result {}", result);
}
std::string Day3::SolvePart2(std::string input) {
    std::regex pattern(R"(don't\(\)|do\(\)|mul\((\d+),(\d+)\))");

    int result = 0;
    bool includeNext = true;
    for (std::sregex_iterator it(input.begin(), input.end(), pattern), end; it != end; ++it) {
        std::smatch match = *it;

        if (match.str() == "don't()") {
            includeNext = false;
            continue;
        }
        else if (match.str() == "do()") {
            includeNext = true;
            continue;
        }

        if (includeNext) {
            int firstNumber = std::stoi(match[1]);
            int secondNumber = std::stoi(match[2]);

            result += firstNumber * secondNumber;
        }
    }

    return std::format("Result {}", result);
}