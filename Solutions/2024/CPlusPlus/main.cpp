
#include <filesystem>
#include <iostream>
#include "Utils/SolutionHandler.h"
#include <stdexcept>
#include <format>
using namespace std;


int main(int argc, char* argv[])
{
    try {
        if (argc != 2) {
            throw std::invalid_argument("Invalid number of arguments. Expected no arguments.");
        }
        int day = strtol(argv[1], NULL, 0);

        string currentPath = filesystem::absolute(std::filesystem::current_path()).string();
        std::string dayPrefix = "Day";
        std::string solutionsPath = currentPath + "\\Inputs";
        std::string sampleName = "sample.txt";
        std::string inputName = "input.txt";

        auto handler = new SolutionHandler(
            solutionsPath, dayPrefix, sampleName, inputName
            );

        std::cout << std::format("[SAMPLE] Result day {0}: \n {1} \n", day, handler->runSample(day));
        std::cout << std::format("[INPUT] Result day {0}: \n {1} \n", day, handler->runFullData(day));
    } catch (const std::exception& e) {
        std::cerr << "Error: " << e.what() << std::endl;
        return 1; // Non-zero return code indicates an error
    }


}
