#include "Day4.h"

#include <format>
#include <sstream>
#include <unordered_map>
#include <vector>



// class TrieNode {
// public:
//     std::vector<std::pair<char, TrieNode*>> Children;
//     bool IsEndOfWord = false;
//
// };
//
// class TrieCreator {
// public:
//     TrieCreator(std::string input) : data(input) {
//         translateData();
//     }
//     TrieNode* ConstructTrie() {
//         TrieNode* root = new TrieNode();
//
//         for (int i =0; i < translatedData.size(); i++) {
//             for (int j=0; j < translatedData[i].size(); j++) {
//                 if (translatedData[i][j] == 'X') {
//                     createChild(i, j, translatedData[i][j], root);
//                 }
//             }
//         }
//
//         return root;
//     }
// private:
//     const std::string data;
//     std::vector<std::pair<int , int>> directions = {{-1, 0}, {1, 0}, {0, -1}, {0, 1},
//                                                    {-1, -1}, {-1, 1}, {1, -1}, {1, 1}};
//     std::vector<std::vector<char>> translatedData;
//
//     void createChild(const int& x, const int& y, const char ch, TrieNode* parent) {
//         TrieNode* node = new TrieNode();
//
//
//
//     }
//     void translateData() {
//         translatedData.clear();
//
//         char ch;
//         std::istringstream stream(data);
//
//         int currentLine = 0;
//         while (stream.get(ch)) {
//             if (!std::isspace(ch)) {
//                 if (translatedData.size() < currentLine + 1) {
//                     translatedData.emplace_back();
//                 }
//                 translatedData[currentLine].push_back(ch);
//             }
//             else if (ch == '\n') {
//                 currentLine++;
//             }
//         }
//
//
//     }
// };
std::string Day4::SolvePart1(std::string input) {
    std::vector<std::vector<char>> translatedData;
    char ch;
    std::istringstream stream(input);

    int currentLine = 0;
    while (stream.get(ch)) {
        if (!std::isspace(ch)) {
            if (translatedData.size() < currentLine + 1) {
                translatedData.emplace_back();
            }
            translatedData[currentLine].push_back(ch);
        }
        else if (ch == '\n') {
            currentLine++;
        }
    }

    int correctXMas = 0;
    for (int i = 0; i < translatedData.size(); i++) {
        for (int j = 0; j < translatedData[i].size(); j++) {
            char character = translatedData[i][j];
            if (character != 'X') {
                continue;
            }
            correctXMas += correctXMASFromPosition(translatedData,i, j, charSequence);
        }
    }

    return std::format("CorrectXMAS:{}", correctXMas);
}

int Day4::correctXMASFromPosition(const std::vector<std::vector<char>>& translatedData,int& x, int& y, const std::vector<char>& charSequence) {
    int correctXMAS = 0;

    for (auto dir : directions) {
        bool correct = true;

        for (int k = 0 ; k < charSequence.size(); k++) {

            int targetX = x + k * dir.first;
            int targetY = y + k * dir.second;
            if (targetX < 0 || targetY < 0 || targetX >= translatedData.size() || targetY >= translatedData[targetY].size()) {
                correct = false;
                break;
            }
            if (translatedData[targetX][targetY] != charSequence[k]) {
                correct = false;
                break;
            }
        }
        if (correct)
            correctXMAS++;
    }


    return correctXMAS;
}


std::string Day4::SolvePart2(std::string input) {
    std::vector<std::vector<char>> translatedData;
    char ch;
    std::istringstream stream(input);

    int currentLine = 0;
    while (stream.get(ch)) {
        if (!std::isspace(ch)) {
            if (translatedData.size() < currentLine + 1) {
                translatedData.emplace_back();
            }
            translatedData[currentLine].push_back(ch);
        }
        else if (ch == '\n') {
            currentLine++;
        }
    }

    auto isValidXMas = [](char ch, char ch1, char ch2, char ch3, char ch4) -> bool {
        return (ch == 'A') && (
            (ch1 == 'M' && ch2 == 'S' && ch3 == 'M' && ch4 == 'S') ||
            (ch1 == 'M' && ch2 == 'S' && ch3 == 'S' && ch4 == 'M') ||
            (ch1 == 'S' && ch2 == 'M' && ch3 == 'M' && ch4 == 'S') ||
            (ch1 == 'S' && ch2 == 'M' && ch3 == 'S' && ch4 == 'M')
        );
    };

    int correctXMas = 0;
    for (int i = 1; i < translatedData.size() - 1; i++) {
        for (int j = 1; j < translatedData[i].size() - 1; j++) {
            char character = translatedData[i][j];

            if (isValidXMas(character
                , translatedData[i + 1][j + 1]
                , translatedData[i - 1][j - 1]
                , translatedData[i - 1][j + 1]
                , translatedData[i + 1][j - 1])) {
                correctXMas++;
            }
        }
    }

    return std::format("CorrectXMAS:{}", correctXMas);
}


