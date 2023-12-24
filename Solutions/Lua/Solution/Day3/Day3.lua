daySolution = {}

package.path = package.path .. ";../Utilities/?.lua"
local solutionCreatorModule = require "solutionCreator";

--[[
    [1] == Column,
    [2] == Row
]]
--
local SYMBOL_OFFSETS = {
    { 0,  1 },
    { 0,  -1 },
    { 1,  0 },
    { -1, 0 },
    { 1,  1 },
    { 1,  -1 },
    { -1, 1 },
    { -1, -1 }
};

local function CreateGrid(data)
    local table = {};
    local lineIdx = 1;

    for line in string.gmatch(data, "[^\r\n]+") do
        table[lineIdx] = {};
        for charIdx = 1, #line do
            table[lineIdx][charIdx] = line:sub(charIdx, charIdx);
        end
        lineIdx = lineIdx + 1;
    end
    return table;
end

local function isSymbol(char)
    return ~(char == ".") and ~(string.match(char, "[%w]"));
end


local function GetDigitsWithTheirPosition(row)
    local digits = {};
    for idx, val in ipairs(row) do
        if (string.match(val, "(%d)")) then
            digits[idx] = val;
            --table.insert(digits, { index = idx, digit = val });
        end
    end
    return digits;
end

local function IsAdjacentToSymbol(grid, column, row)
    local targetRow, targetColumn;
    for idx, offset in ipairs(SYMBOL_OFFSETS) do
        targetColumn = grid[column + offset[1]];
        if (targetColumn) then
            targetRow = targetColumn[row + offset[2]];
            if (targetRow) then
                if (isSymbol(targetRow)) then
                    return true;
                end
            end
        end
    end
    return false;
end

local function GetAllValidNumbers(digitsInRow, column, grid)
    local validNumbers = {};

    local lastKey;
    local number = "";
    local adjacented = false;
    for k, v in pairs(digitsInRow) do
        number = number .. v;
        if (IsAdjacentToSymbol(grid, column, k)) then
            adjacented = true;
        end

        -- Handle situation where there's break between two digits (in grid it would be symbol)
        if (digitsInRow[k + 1] == nil) then
            if (adjacented) then
                table.insert(validNumbers, number);
                adjacented = false;
            end
            number = "";
        end
        lastKey = k;
    end

    -- There were digits to end of dictionary keys
    if (adjacented) and digitsInRow[lastKey] ~= nil then
        table.insert(validNumbers, number);
    end

    return validNumbers;
end


function daySolution.GetSolution()
    local sol = solutionCreatorModule.CreateSolution();

    sol.Part1 = function(data)
        local sum = 0;
        local grid = CreateGrid(data);
        for column, row in ipairs(grid) do
            local validNumbers = GetAllValidNumbers(GetDigitsWithTheirPosition(row), column, grid);
            print("VALID NUMBERS:");
            for i = 1, #validNumbers do
                print(validNumbers[i]);
            end
        end
    end

    sol.Part2 = function(data)

    end

    return sol;
end

return daySolution;
