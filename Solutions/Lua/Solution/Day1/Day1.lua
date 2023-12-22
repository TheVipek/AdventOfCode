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
        local len, mid;
        local leftChar, rightChar;
        for line in string.gmatch(data, "[^\r\n]+") do
            firstNumber, lastNumber = "", "";
            local len = #line

            for i = 1, len, 1 do

                if tonumber(firstNumber) and tonumber(lastNumber) then
                    break
                end

                if tonumber(firstNumber) == nil then
                    leftChar = string.sub(line, i, i);
                    if tonumber(leftChar) then
                        firstNumber = leftChar;
                    else
                        firstNumber = LettersIntoNumbers(firstNumber .. leftChar, letterNumbers);
                    end
                end

                if tonumber(lastNumber) == nil then
                    rightChar = string.sub(line, len - i + 1, len - i + 1);
                    if tonumber(rightChar) then
                        lastNumber = rightChar;
                    else
                        lastNumber = LettersIntoNumbers(rightChar .. lastNumber, letterNumbers);
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
