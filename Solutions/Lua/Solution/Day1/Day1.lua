package.path = package.path .. ";../?.lua"
local inputHandlerModule = require "InputHandler";
local solutionCreatorModule = require "solutionCreator";
local switchModule = require "switch";

local data = inputHandlerModule.GetInputFromCurrentFolder("data.txt");
local solution = solutionCreatorModule.CreateSolution();

solution.Part1 = function()
    local sum = 0;
    for line in string.gmatch(data, "[^\r\n]+") do -- returns iterator function and we iterate over all matches (sequence of characters that are not newlines)
        sum = sum + tonumber(string.match(line, "(%d)") .. string.match(line, ".*(%d)")); -- first occured number + last occured number
    end
    return sum;
end

solution.Part2 = function()
    local switch = switchModule.switch({
        one = function()
            return 1
        end,
        two = function()
            return 2
        end,
        three = function()
            return 3
        end,
        four = function()
            return 4
        end,
        five = function()
            return 5
        end,
        six = function()
            return 6
        end,
        seven = function()
            return 7
        end,
        eight = function()
            return 8
        end,
        nine = function()
            return 9
        end
    });

    local sum = 0;
    for line in string.gmatch(data, "[^\r\n]+") do -- returns iterator function and we iterate over all matches (sequence of characters that are not newlines)
        local lineValues = {}

        for digit in string.gmatch(line, "%d") do
            table.insert(lineValues, tonumber(digit))
        end

        for nonDigit in string.gmatch(line, "%D+") do
            table.insert(lineValues, nonDigit)
        end

        local firstNumber = nil;
        local lastNumber = nil;

        for i, value in ipairs(lineValues) do
            if (type(value) == "number") then
                if firstNumber == nil then
                    print("set first numb" .. value);
                    firstNumber = value;
                end
                lastNumber = value;
            else
                local switchResult = switch(value);
                if switchResult ~= nil then
                    print("asd");
                    if firstNumber == nil then
                        print("set first numb" .. value);
                        firstNumber = switchResult;
                    end
                    lastNumber = switchResult;
                end
            end
        end
        print(firstNumber .. lastNumber);
    end
end

solution.Part2();
