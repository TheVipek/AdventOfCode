#include "Day2.h"

#include <format>
#include <sstream>
#include <string>
#include <vector>
#include <algorithm>

std::string Day2::SolvePart1(std::string input) {
    std::istringstream stream(input);
    std::string line;

    std::vector<std::vector<int>> reports = std::vector<std::vector<int>>();

    std::vector<int> report = std::vector<int>();

    while (std::getline(stream, line)) {

        std::istringstream lineStream(line);
        std::string token;

        while (lineStream >> token) {

            int v = std::stoi(token);
            report.push_back(v);
        }
        reports.push_back(report);

        report.clear();
    }


    int validReports = 0;

    for (auto i = 0; i < reports.size(); i++) {
        bool correctReport = true;
        bool shouldInc = false;

        if (reports[i].size() >= 2) {
            if (reports[i][0] < reports[i][1]) {
                shouldInc = true;
            }
            else if (reports[i][0] > reports[i][1]){
                shouldInc = false;
            }
        }

        for (auto j = 0; j < reports[i].size() - 1; j++) {

            int reportDiff = reports[i][j] - reports[i][j + 1];
            if (reportDiff == 0 || std::abs(reportDiff) > 3) {
                correctReport = false;
                break;
            }
            else if (reportDiff < 0 && !shouldInc) {
                correctReport = false;
                break;
            }
            else if (reportDiff > 0 && shouldInc) {
                correctReport = false;
                break;
            }
        }

        if (correctReport)
            validReports++;

    }

    return std::format("Valid Reports: {}", validReports);
}

std::string Day2::SolvePart2(std::string input) {
    std::istringstream stream(input);
    std::string line;

    std::vector<std::vector<int>> reports = std::vector<std::vector<int>>();

    std::vector<int> report = std::vector<int>();

    while (std::getline(stream, line)) {

        std::istringstream lineStream(line);
        std::string token;

        while (lineStream >> token) {

            int v = std::stoi(token);
            report.push_back(v);
        }
        reports.push_back(report);

        report.clear();
    }


    int validReports = 0;
    int invalidReports = 0;

    int singleErrorReports = 0;
    for (auto i = 0; i < reports.size(); i++) {

        std::vector<int> _report = reports[i];
        bool correctReport = true;
        bool shouldInc = false;
        int errorCount = 0;

        if (_report.size() >= 2) {
            if (_report[0] < _report[1]) {
                shouldInc = true;
            }
            else if (_report[0] > _report[1]){
                shouldInc = false;
            }
        }


        for (auto j = 0; j < _report.size() - 1; j++) {

            int reportDiff = _report[j] - _report[j + 1];
            if (reportDiff == 0 || std::abs(reportDiff) > 3) {
                errorCount++;

                if (errorCount > 1) {
                    correctReport = false;
                    break;
                }
            } else if (reportDiff < 0 && !shouldInc) {
                errorCount++;

                if (errorCount > 1) {
                    correctReport = false;
                    break;
                }
            }
            else if (reportDiff > 0 && shouldInc) {
                errorCount++;

                if (errorCount > 1) {
                    correctReport = false;
                    break;
                }
            }
        }

        if (errorCount == 1) {
            singleErrorReports++;
        }
        else if (correctReport)
            validReports++;
        else
            invalidReports++;

    }

    const int& autoCorrectedSingleErrors = std::ranges::clamp(singleErrorReports, 0, invalidReports);

    validReports += autoCorrectedSingleErrors;

    return std::format("Valid Reports: {}(invalid {})", validReports, invalidReports);
}