daySolution = {}

package.path = package.path .. ";../Utilities/?.lua"
local solutionCreatorModule = require "solutionCreator";

-- Card Class Related
Seed = {}

function Seed:New(seedValue)
    local o = {
        Properties = {
            seedValue, -- Seed
            0,         -- Soil
            0,         -- Fertilizer
            0,         -- Water
            0,         -- Light
            0,         -- Temperature
            0,         -- Humidity
            0          -- Location
        }
    };
    setmetatable(o, {
        __index = Seed
    });
    return o;
end

function Seed:DisplayData()
    print("Seed:" ..
        self.Properties[1] ..
        " Soil:" ..
        self.Properties[2] ..
        " Fertilizer:" ..
        self.Properties[3] ..
        " Water:" ..
        self.Properties[4] ..
        " Light:" ..
        self.Properties[5] ..
        " Temperature:" ..
        self.Properties[6] ..
        " Humidity:" ..
        self.Properties[7] ..
        " Location:" ..
        self.Properties[8]);
end

function Seed:FillAllProperties(maps)
    if #maps ~= 7 then
        print("There need to be 7 maps (Soil,Fertilizer,Water,Light,Temperature,Humidity,Location)");
        return;
    end

    local correspondingNumber;
    for i = 1, #maps do
        local currMap = maps[i];
        correspondingNumber = self.Properties[i];
        for j = 1, #currMap do
            local dstRangeStart = currMap[j].DestinationRangeStart;
            local srcRangeStart = currMap[j].SourceRangeStart;
            local rangeLen = currMap[j].RangeLength;

            local srcMax = srcRangeStart + (rangeLen - 1);
            local dstMax = dstRangeStart + (rangeLen - 1)
            if (srcRangeStart <= correspondingNumber and srcMax >= correspondingNumber) then
                local diff = srcMax - correspondingNumber;
                correspondingNumber = dstMax - diff;
            end
        end
        self.Properties[i + 1] = correspondingNumber;
    end
end

local function InitializeSeeds(seedsTable, seedsValues)
    for i = 1, #seedsValues do
        seedsTable[i] = Seed:New(tonumber(seedsValues[i]));
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


local function DisplaySeedsAndMaps(seeds, maps)
    print("\nDISPLAYING\n");
    print("\nSEEDS\n");
    for i = 1, #seeds do
        print(seeds[i]);
    end
    print("\nMaps\n");

    for i = 1, #maps do
        print("Map " .. i);
        local map = maps[i];
        for j = 1, #map do
            print(map[j].DestinationRangeStart .. " - " .. map[j].SourceRangeStart .. " - " .. map[j].RangeLength);
        end
    end
    print("\nEND\n\n");
end


function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)
        local seeds = {};
        local maps = {};
        for line in string.gmatch(data, "[^\r\n]+") do
            if (string.match(line, "(map)")) then
                table.insert(maps, {});
            else
                local numbers = {};
                for number in string.gmatch(line, "%d+") do
                    table.insert(numbers, number);
                end

                if #maps > 0 then
                    table.insert(maps[#maps], CreateMapElement(numbers));
                else
                    InitializeSeeds(seeds, numbers);
                end
            end
        end

        for i = 1, #seeds do
            seeds[i]:FillAllProperties(maps);
        end
        for i = 1, #seeds do
            seeds[i]:DisplayData();
        end

        local lowestLocation = nil;
        for i = 1, #seeds do
            if lowestLocation == nil or seeds[i].Properties[8] < lowestLocation then
                lowestLocation = seeds[i].Properties[8];
            end
        end
        return lowestLocation;
    end
    solution.Part2 = function(data)
        return 0;
    end

    return solution;
end

return daySolution;
