#include "Day1.h"

#include <algorithm>
#include <format>
#include <iostream>
#include <map>
#include <sstream>
#include <vector>


std::string Day1::SolvePart1(std::string input) {
    std::istringstream stream(input);
    std::string line;

    std::pair<std::vector<int>, std::vector<int>> sides = std::make_pair(std::vector<int>(), std::vector<int>());
    while (std::getline(stream, line)) {

        std::istringstream lineStream(line);
        std::string token;

        int idx = 0;
        while (lineStream >> token) {

            int v = std::stoi(token);
            if (idx == 0) {
                sides.first.push_back(v);
            }
            else {
                sides.second.push_back(v);
            }

            idx++;
        }
    }

    if (sides.first.size() != sides.second.size()) {
        return "Sides are not equal.";
    }

    std::ranges::sort(sides.first);
    std::ranges::sort(sides.second);


    int totalDistance = 0;

    for (auto i = 0; i < sides.first.size(); i++) {
        totalDistance += std::abs(sides.first[i] - sides.second[i]);
    }

    return std::format("Distance: {0}", totalDistance);
}

std::string Day1::SolvePart2(std::string input) {
    std::istringstream stream(input);
    std::string line;

    std::pair<std::vector<int>, std::map<int, int>> sides = std::make_pair(std::vector<int>(), std::map<int, int>());
    while (std::getline(stream, line)) {

        std::istringstream lineStream(line);
        std::string token;

        int idx = 0;
        while (lineStream >> token) {

            int v = std::stoi(token);
            if (idx == 0) {
                sides.first.push_back(v);
            }
            else {
                sides.second[v]++;
            }

            idx++;
        }
    }

    int totalDistance = 0;

    for (auto i = 0; i < sides.first.size(); i++) {
        if (sides.second.contains(sides.first[i])) {
            int fV = sides.first[i];
            totalDistance += fV * sides.second[fV];;
        }
    }

    return std::format("Distance: {0}", totalDistance);
}