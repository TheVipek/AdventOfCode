package.path = package.path .. ";Utilities/?.lua";
local inputHandlerModule = require "inputHandler";
local methodProfiler = require "methodProfiler";
print("Enter day of AoC2023");
local day = io.read();

package.path = package.path .. ";Day" .. day .. "/?.lua";
local daySolution = require("Day" .. day);

local data = inputHandlerModule.GetDataFromPath("/Day" .. day .. "/data.txt");
local sol = daySolution.GetSolution();
print("Part1:" .. sol.Part1(data));
--print("Part2:" .. sol.Part2(data));

print("Part1 execution time:" .. methodProfiler.Mesaure(function()
    return sol.Part1(data);
end));

print("Part2 execution time:" .. methodProfiler.Mesaure(function()
    return sol.Part2(data);
end));
