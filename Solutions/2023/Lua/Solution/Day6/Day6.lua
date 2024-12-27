daySolution = {}


package.path = package.path .. ";../Utilities/?.lua"


local solutionCreatorModule = require "solutionCreator";

local function calculateDistance(holdTime, raceTime)
    return holdTime * (raceTime - holdTime);
end

local function GetNumberOfWaysTogether(winPossibilies)
    local result = 1;
    for i = 1, #winPossibilies do
        result = result * winPossibilies[i];
    end
    return result
end

local function GetWinPossibilies(races)
    local winPossibilies = {};
    for i = 1, #races / 2 do
        local raceTime = races[i];
        local raceDistance = races[(#races / 2) + i];
        local wins = 0;
        for hold = 1, raceTime do
            if (calculateDistance(hold, raceTime) > raceDistance) then
                wins = wins + 1;
            end
        end
        table.insert(winPossibilies, wins);
    end
    return winPossibilies;
end

-- local function GetHoldTimes(time, distance)
--     local vertex = time / 2;
--     local maxDistance = vertex * (time - vertex);

--     if maxDistance <= distance then
--         return 0;
--     end
--     local discriminant = math.sqrt(time ^ 2 - (4 * distance))
--     local minH = (time - discriminant) / 2;
--     local maxH = (time - discriminant) / 2;

--     return math.floor(maxH) - math.ceil(minH) + 1;
-- end
-- local function GetWinPossibiliesV2(races)
--     local winPossibilies = {};
--     for i = 1, #races / 2 do
--         local raceTime = races[i];
--         local raceDistance = races[(#races / 2) + i];
--         table.insert(winPossibilies, GetHoldTimes(raceTime, raceDistance));
--     end
--     return winPossibilies;
-- end

function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)
        local races = {};

        for line in string.gmatch(data, "[^\r\n]+") do
            for number in string.gmatch(line, "%d+") do
                table.insert(races, tonumber(number));
            end
        end

        local winPossibilies = GetWinPossibilies(races);

        return GetNumberOfWaysTogether(winPossibilies);
    end
    solution.Part2 = function(data)
        local races = {};

        for line in string.gmatch(data, "[^\r\n]+") do
            local numbers = {};
            for number in string.gmatch(line, "%d+") do
                table.insert(numbers, number);
            end
            table.insert(races, tonumber(table.concat(numbers)));
        end

        local winPossibilies = GetWinPossibilies(races);

        return GetNumberOfWaysTogether(winPossibilies);
    end

    return solution;
end

return daySolution;
