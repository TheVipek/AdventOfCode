daySolution = {}

package.path = package.path .. ";../Utilities/?.lua"
local solutionCreatorModule = require "solutionCreator";
local switchModule = require "switch";

local letterNumbers = {
    one = "1",
    two = "2",
    three = "3",
    four = "4",
    five = "5",
    six = "6",
    seven = "7",
    eight = "8",
    nine = "9"
};

local function LettersIntoNumbers(str, letterNumbers)
    for k, v in pairs(letterNumbers) do
        if str:find(k) then
            return v;
        end
    end

    return str;
end

function daySolution.GetSolution()
    local sol = solutionCreatorModule.CreateSolution();

    sol.Part1 = function(data)
        local sum = 0;
        local firstDigit, lastDigit;
        for line in string.gmatch(data, "[^\r\n]+") do
            firstDigit = string.match(line, "(%d)");
            if firstDigit ~= nil then
                lastDigit = string.match(line, ".*(%d)");
                sum = sum + tonumber(firstDigit .. lastDigit);
            end
        end
        return sum;
    end

    sol.Part2 = function(data)
        local sum = 0;

        local firstNumber, lastNumber;
        local leftChar, rightChar;
        local foundFirst, foundLast;
        local len;
        for line in string.gmatch(data, "[^\r\n]+") do
            firstNumber, lastNumber = "", "";
            foundFirst, foundLast = false, false;
            len = #line

            for i = 1, len do
                if (foundFirst and foundLast) then
                    break
                end

                if not foundFirst then
                    leftChar = line:sub(i, i);
                    if tonumber(leftChar) then
                        firstNumber = leftChar;
                        foundFirst = true;
                    else
                        firstNumber = LettersIntoNumbers(firstNumber .. leftChar, letterNumbers);
                        foundFirst = tonumber(firstNumber) ~= nil;
                    end
                end

                if not foundLast then
                    rightChar = line:sub(len - i + 1, len - i + 1);
                    if tonumber(rightChar) then
                        lastNumber = rightChar;
                        foundLast = true;
                    else
                        lastNumber = LettersIntoNumbers(rightChar .. lastNumber, letterNumbers);
                        foundLast = tonumber(lastNumber) ~= nil;
                    end
                end
            end
            sum = sum + tonumber(firstNumber .. lastNumber);
        end

        return sum;
    end

    return sol;
end

return daySolution;
