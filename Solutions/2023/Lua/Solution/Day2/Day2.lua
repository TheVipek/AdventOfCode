daySolution = {}

package.path = package.path .. ";../Utilities/?.lua"
local solutionCreatorModule = require "solutionCreator";

local function IsOverLimit(gmatchIterator, limit)
    for number in gmatchIterator do
        if (tonumber(number) > limit) then
            return false;
        end
    end
    return true;
end
local function GetMaximumPlayed(gmatchIterator)
    local maxNumber = 0;
    for number in gmatchIterator do
        if (tonumber(number) > maxNumber) then
            maxNumber = tonumber(number);
        end
    end
    return maxNumber;
end

function daySolution.GetSolution()
    local sol = solutionCreatorModule.CreateSolution();

    sol.Part1 = function(data)

        local gameIDSum = 0;
        for line in string.gmatch(data, "[^\r\n]+") do

            if (IsOverLimit(line:gmatch("(%d+)%s*(red)"), 12) and IsOverLimit(line:gmatch("(%d+)%s*(green)"), 13) and
                IsOverLimit(line:gmatch("(%d+)%s*(blue)"), 14)) then
                gameIDSum = gameIDSum + tonumber(line:match("Game (%d+)")) or 0;
            end
        end
        return gameIDSum;
    end

    sol.Part2 = function(data)
        local powerOfAllSets = 0;
        for line in string.gmatch(data, "[^\r\n]+") do
            local greenMax = GetMaximumPlayed(line:gmatch("(%d+)%s*(green)"));
            local blueMax = GetMaximumPlayed(line:gmatch("(%d+)%s*(blue)"));
            local redMax = GetMaximumPlayed(line:gmatch("(%d+)%s*(red)"));

            powerOfAllSets = powerOfAllSets + (greenMax * blueMax * redMax);
        end

        return powerOfAllSets;
    end

    return sol;
end

return daySolution;
