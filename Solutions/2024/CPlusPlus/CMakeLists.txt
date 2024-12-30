cmake_minimum_required(VERSION 3.30)
project(CPlusPlus)

set(CMAKE_VERBOSE_MAKEFILE ON)
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
include_directories(Utils)
include_directories(Solutions)

file(GLOB MY_FILES "Inputs/*")
file(COPY ${MY_FILES} DESTINATION ${CMAKE_CURRENT_BINARY_DIR}/Inputs)


add_executable(CPlusPlus
        Utils/SolutionBase.h
        Utils/SolutionHandler.h
        Utils/SolutionHandler.cpp
        main.cpp
        Solutions/Day1.h
        Solutions/Day2.h
        Solutions/Day1.cpp
        Solutions/Day2.cpp
        Solutions/Day3.h
        Solutions/Day3.cpp
        Solutions/Day4.cpp
        Solutions/Day4.h

)
