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
        --ROW
        table[lineIdx] = {};
        for charIdx = 1, #line do
            --COLUMN IN ROW
            table[lineIdx][charIdx] = line:sub(charIdx, charIdx);
        end
        lineIdx = lineIdx + 1;
    end
    return table;
end

local function DisplayAllData(grid)
    print("COLUMN", "ROW");
    for column, row in ipairs(grid) do
        local rowStr = "";
        for i = 1, #row do
            rowStr = rowStr .. row[i];
        end
        print(column, rowStr);
    end
end

local function isSymbol(char)
    return char ~= "." and not string.match(char, "[%w]");
end

local function GetDigitsWithTheirPosition(row)
    local digits = {};
    for idx, val in ipairs(row) do
        if (val:match("%d")) then
            digits[#digits + 1] = { index = idx, digit = val };
        end
    end
    return digits;
end

local function IsAdjacentToSymbol(grid, column, row)
    local targetRow, targetColumn;
    for _, offset in ipairs(SYMBOL_OFFSETS) do
        targetColumn = column + offset[1];
        targetRow = row + offset[2];
        if (grid[targetColumn] and grid[targetColumn][targetRow] and isSymbol(grid[targetColumn][targetRow])) then
            return true;
        end
    end
    return false;
end

local function GetAllValidNumbers(digitsInRow, column, grid)
    local validNumbers = {};
    local numberParts = {};

    local adjacented = false;
    local digitPos, digit;

    for idx, value in ipairs(digitsInRow) do
        digitPos = value.index;
        digit = value.digit;

        table.insert(numberParts, digit);
        if (IsAdjacentToSymbol(grid, column, digitPos)) then
            adjacented = true;
        end

        if digitsInRow[idx + 1] == nil or digitsInRow[idx + 1].index - digitPos > 1 then
            if adjacented then
                validNumbers[#validNumbers + 1] = tonumber(table.concat(numberParts));
                adjacented = false;
            end
            numberParts = {};
        end
    end
    return validNumbers;
end



function daySolution.GetSolution()
    local sol = solutionCreatorModule.CreateSolution();

    sol.Part1 = function(data)
        local sum = 0;
        local grid = CreateGrid(data);
        for column, row in ipairs(grid) do
            local digitsWithPos = GetDigitsWithTheirPosition(row);
            local validNumbers = GetAllValidNumbers(digitsWithPos, column, grid);
            for i = 1, #validNumbers do
                sum = sum + validNumbers[i];
            end
        end
        return sum;
    end

    sol.Part2 = function(data)

    end

    return sol;
end

return daySolution;
