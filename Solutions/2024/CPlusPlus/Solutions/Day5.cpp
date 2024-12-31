#include "Day5.h"

#include <cmath>
#include <format>
#include <map>
#include <sstream>
#include <vector>
#include <bits/stdc++.h>

std::string Day5::SolvePart1(std::string input) {
    std::istringstream stream(input);
    std::string line;

    std::pmr::map<int, std::unordered_set<int>> pageOrderingRules = std::pmr::map<int, std::unordered_set<int>>();
    std::pmr::map<int, std::vector<int>> updates = std::pmr::map<int, std::vector<int>>();
    int updateSequence = 0;
    bool firstSection = true;
    while (std::getline(stream, line)) {
        if (line.empty()) {
            firstSection = false;
            continue;
        }
        std::istringstream lineStream(line);
        std::string token;

        if (firstSection) {
            int firstNumber, secondNumber;
            char delimiter;
            lineStream >> firstNumber >> delimiter >> secondNumber;

            pageOrderingRules[firstNumber].emplace(secondNumber);
        }
        else {
            ++updateSequence;
            while (std::getline(lineStream, token, ',')) {
                updates[updateSequence].push_back(std::stoi(token));
            }
        }
    }
    int correctPageRulesSum = 0;
    for (const auto& page : updates) {
        bool afterValid = false;
        bool beforeValid = false;
        for (int i = 0; i < page.second.size() - 1; ++i) {

            int currentValue = page.second[i];

            afterValid = std::all_of(page.second.begin() + i + 1, page.second.end(), [&](int num) {
                return pageOrderingRules[currentValue].contains(num);
            });
            beforeValid = std::all_of(page.second.begin(), page.second.begin() + i , [&](int num) {
               return pageOrderingRules[num].contains(currentValue);
           });

            if (!afterValid || !beforeValid)
                break;
        }

        if (afterValid && beforeValid) {
            int targetPage = page.second[std::ceil(page.second.size() / 2)];
            correctPageRulesSum += targetPage;
        }
    }



    return std::format("Correct Page Ordering {}", correctPageRulesSum);
};
std::string Day5::SolvePart2(std::string input) {
    std::istringstream stream(input);
    std::string line;
    std::pmr::map<int, std::unordered_set<int>> pageOrderingRules = std::pmr::map<int, std::unordered_set<int>>();
    std::pmr::map<int, std::vector<int>> updates = std::pmr::map<int, std::vector<int>>();
    int updateSequence = 0;
    bool firstSection = true;
    while (std::getline(stream, line)) {
        if (line.empty()) {
            firstSection = false;
            continue;
        }
        std::istringstream lineStream(line);
        std::string token;

        if (firstSection) {
            int firstNumber, secondNumber;
            char delimiter;
            lineStream >> firstNumber >> delimiter >> secondNumber;

            pageOrderingRules[firstNumber].emplace(secondNumber);
        }
        else {
            ++updateSequence;
            while (std::getline(lineStream, token, ',')) {
                updates[updateSequence].push_back(std::stoi(token));
            }
        }
    }

    int incorrectPageRulesSum = 0;
    for (auto& page : updates) {
        bool afterValid = false;
        bool beforeValid = false;
        for (int i = 0; i < page.second.size() - 1; ++i) {

            int currentValue = page.second[i];

            afterValid = std::all_of(page.second.begin() + i + 1, page.second.end(), [&](int num) {
                return pageOrderingRules[currentValue].contains(num);
            });
            beforeValid = std::all_of(page.second.begin(), page.second.begin() + i , [&](int num) {
               return pageOrderingRules[num].contains(currentValue);
           });

            if (!afterValid || !beforeValid)
                break;
        }

        if (!afterValid || !beforeValid) {
            std::ranges::sort(page.second, [&pageOrderingRules](int a, int b) {
                return pageOrderingRules[a].contains(b);
            });


            int targetPage = page.second[std::ceil(page.second.size() / 2)];
            incorrectPageRulesSum += targetPage;
        }
    }

    return std::format("Incorrect Page Ordering {}", incorrectPageRulesSum);
};