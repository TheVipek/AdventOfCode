daySolution = {}


package.path = package.path .. ';lua_modules/share/lua/5.4/?.lua';
package.cpath = package.cpath .. ';lua_modules/lib/lua/5.4/?.dll';
package.path = package.path .. ";../Utilities/?.lua"


local solutionCreatorModule = require "solutionCreator";
local lanes = require "lanes".configure();

local function MergeMapRanges(map)
    table.sort(map, function(a, b) return a.SourceRangeStart < b.SourceRangeStart end);

    local mergedMap = {};
    local current = map[1]

    for i = 2, #map do
        local nextRange = map[i];

        if current.SourceRangeStart + current.RangeLength >= nextRange.SourceRangeStart + nextRange.RangeLength then
            local newRangeLength = math.max(current.SourceRangeStart + current.RangeLength,
                nextRange.SourceRangeStart + nextRange.RangeLength) - current.SourceRangeStart;

            current = {
                DestinationRangeStart = current.DestinationRangeStart,
                SourceRangeStart = current.SourceRangeStart,
                RangeLength = newRangeLength,
            };
        else
            table.insert(mergedMap, current);
            current = nextRange;
        end
    end

    table.insert(mergedMap, current);

    return mergedMap;
end

local function FindLocation(maps, seedNumber)
    local correspondingNumber = seedNumber;
    local currMap,
    dstRangeStart,
    srcRangeStart,
    rangeLen,
    diff;
    for i = 1, #maps do
        currMap = maps[i];
        for j = 1, #currMap do
            dstRangeStart = currMap[j].DestinationRangeStart;
            srcRangeStart = currMap[j].SourceRangeStart;
            rangeLen = currMap[j].RangeLength;
            if (srcRangeStart <= correspondingNumber and srcRangeStart + (rangeLen - 1) >= correspondingNumber) then
                diff = correspondingNumber - srcRangeStart;
                correspondingNumber = dstRangeStart + diff;
                break;
            end
        end
    end
    return correspondingNumber;
end

local function InitializeSeeds(seedsTable, seedsValues)
    for i = 1, #seedsValues do
        seedsTable[i] = { SeedValue = tonumber(seedsValues[i]) };
    end
end

local function CreateMapElement(numbersTable)
    if #numbersTable == 3 then
        return
        {
            DestinationRangeStart = tonumber(numbersTable[1]),
            SourceRangeStart = tonumber(numbersTable[2]),
            RangeLength = tonumber(numbersTable[3])
        };
    end
    return {};
end

local function GetLowestLocationFromSeed(maps, startV, endV)
    print("START");
    local lowestLocation = nil;
    for j = startV, endV do
        local currLoc = FindLocation(maps, j);
        if (lowestLocation == nil or currLoc < lowestLocation) then
            lowestLocation = currLoc;
        end
    end
    print("END");
    return lowestLocation;
end


function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)
        local seeds = {};
        local maps = {};
        local currentMap = nil;
        for line in string.gmatch(data, "[^\r\n]+") do
            if (string.match(line, "(map)")) then
                if currentMap then
                    currentMap = MergeMapRanges(currentMap);
                    table.insert(maps, currentMap);
                end
                currentMap = {};
            else
                local numbers = {};
                for number in string.gmatch(line, "%d+") do
                    table.insert(numbers, number);
                end

                if currentMap then
                    table.insert(currentMap, CreateMapElement(numbers));
                else
                    InitializeSeeds(seeds, numbers);
                end
            end
        end

        -- preprocess last map
        if currentMap then
            currentMap = MergeMapRanges(currentMap);
            table.insert(maps, currentMap);
        end

        local lowestLocation = nil;
        for i = 1, #seeds do
            local currLocation = FindLocation(maps, seeds[i].SeedValue);
            if (lowestLocation == nil or currLocation < lowestLocation) then
                lowestLocation = currLocation;
            end
        end
        return lowestLocation;
    end
    solution.Part2 = function(data)
        local seeds = {};
        local maps = {};
        local currentMap = nil;
        for line in string.gmatch(data, "[^\r\n]+") do
            if (string.match(line, "(map)")) then
                if currentMap then
                    currentMap = MergeMapRanges(currentMap);
                    table.insert(maps, currentMap);
                end
                currentMap = {};
            else
                local numbers = {};
                for number in string.gmatch(line, "%d+") do
                    table.insert(numbers, number);
                end

                if currentMap then
                    table.insert(currentMap, CreateMapElement(numbers));
                else
                    InitializeSeeds(seeds, numbers);
                end
            end
        end

        -- preprocess last map
        if currentMap then
            currentMap = MergeMapRanges(currentMap);
            table.insert(maps, currentMap);
        end


        local laneThreads = {};
        local processRangeLane = lanes.gen("*", GetLowestLocationFromSeed);
        for i = 1, #seeds, 2 do
            local startV = seeds[i].SeedValue;
            local endV = startV + seeds[i + 1].SeedValue - 1;
            laneThreads[#laneThreads + 1] = processRangeLane(maps, startV, endV);
        end

        local overallLowestLocation = nil
        for _, thread in ipairs(laneThreads) do
            local locationValue = thread:join()
            if locationValue then
                if overallLowestLocation == nil or overallLowestLocation > locationValue then
                    overallLowestLocation = locationValue
                end
            end
        end


        return overallLowestLocation;
    end

    return solution;
end

return daySolution;
