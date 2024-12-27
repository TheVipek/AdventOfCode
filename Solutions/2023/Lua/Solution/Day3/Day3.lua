daySolution = {}

package.path = package.path .. ";../Utilities/?.lua"
local solutionCreatorModule = require "solutionCreator";

-- Create 2D grid from the input data
local function CreateGrid(gridData)
    local table = {};
    local lineIdx = 1;

    for line in string.gmatch(gridData, "[^\r\n]+") do
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

-- Display rows and columns
local function DisplayGridData(grid)
    print("COLUMN", "ROW");
    for column, row in ipairs(grid) do
        local concenatedRow = {};
        for i = 1, #row do
            table.insert(concenatedRow, row[i]);
        end
        print(column, table.concat(concenatedRow));
    end
end

-- PART 1 RELATED

local function IsSymbol(character)
    return not string.match(character, "[%w.]");
end

-- Get all digits from depending on given pattern
local function GetValuesWithTheirPositions(row, digitPattern)
    local digits = {};
    for i, v in ipairs(row) do
        if (v:match(digitPattern)) then
            digits[#digits + 1] = { idx = i, val = v };
        end
    end
    return digits;
end


local adjacentSymbolCache = {};

-- Chechk whether character on grid[x][y] has any symbol near it
local function HasAdjacentSymbol(grid, x, y)
    local cacheKey = tostring(x) .. ":" .. tostring(y);

    if adjacentSymbolCache[cacheKey] ~= nil then
        return adjacentSymbolCache[cacheKey];
    end

    local offsets = {
        { 0,  1 },
        { 0,  -1 },
        { 1,  0 },
        { -1, 0 },
        { 1,  1 },
        { 1,  -1 },
        { -1, 1 },
        { -1, -1 }
    };
    for _, offset in ipairs(offsets) do
        local targetX, targetY = x + offset[1], y + offset[2];
        if (grid[targetX] and grid[targetX][targetY] and IsSymbol(grid[targetX][targetY])) then
            adjacentSymbolCache[cacheKey] = true;
            return true;
        end
    end
    adjacentSymbolCache[cacheKey] = false;
    return false;
end

-- Get all digits that have adjacent symbol near it
local function GetValidAdjacentNumbers(IdxDigitTable, y, grid)
    local validNumbers = {};
    local currentNumberParts = {};

    local isAdjacent = false;
    local currentDigitPosition, currentDigit;

    for idx, value in ipairs(IdxDigitTable) do
        currentDigitPosition = value.idx;
        currentDigit = value.val;

        if not isAdjacent then
            isAdjacent = HasAdjacentSymbol(grid, y, currentDigitPosition);
        end

        table.insert(currentNumberParts, currentDigit);

        if IdxDigitTable[idx + 1] == nil or IdxDigitTable[idx + 1].idx - currentDigitPosition > 1 then
            if isAdjacent then
                validNumbers[#validNumbers + 1] = tonumber(table.concat(currentNumberParts));
                isAdjacent = false;
            end
            currentNumberParts = {};
        end
    end
    return validNumbers;
end


-- PART 2 RELATED

local function IsGear(character)
    return character == "*";
end

local function reverseTable(table)
    for i = 1, math.floor(#table / 2) do
        table[i], table[#table - i + 1] = table[#table - i + 1], table[i]
    end
end

-- Get all numbers that are going from grid[x][y] in direction
local function GetNumber(grid, x, y, direction)
    local currentRow = y;
    local number = {};
    while true do
        if (grid[x][currentRow] and string.match(grid[x][currentRow], "%d")) then
            table.insert(number, grid[x][currentRow]);
        else
            break;
        end
        currentRow = currentRow + direction;
    end
    return number;
end

-- Similar to GetNumber, although it handle sitaution where between number can be break
local function GetHorizontalNumbers(grid, x, y)
    local numbers = {};
    local numberOnLeft = GetNumber(grid, x, y - 1, -1);
    reverseTable(numberOnLeft);
    local numberOnRight = GetNumber(grid, x, y + 1, 1);

    if string.match(grid[x][y], "%d") then
        -- If the current cell contains a number, concatenate left and right numbers with the current cell's number.
        table.insert(numbers, tonumber(table.concat(numberOnLeft) .. grid[x][y] .. table.concat(numberOnRight)));
    else
        -- If the current cell does not contain a number, add left and right numbers separately if they exist.
        if #numberOnLeft > 0 then
            table.insert(numbers, tonumber(table.concat(numberOnLeft)));
        end
        if #numberOnRight > 0 then
            table.insert(numbers, tonumber(table.concat(numberOnRight)));
        end
    end
    return numbers;
end

-- Get all numbers around specified position
local function GetAllNumbersAround(grid, x, y)
    local allNumbers = {};
    for _, offset in ipairs({ { 0, 1 }, { 0, -1 }, { 1, 0 }, { -1, 0 } }) do
        local adjacentRow, adjacentColumn = x + offset[2], y + offset[1];

        if (grid[adjacentRow] and grid[adjacentRow][adjacentColumn]) then
            local numbers;
            if offset[1] ~= 0 then
                -- Vertical search
                numbers = table.concat(GetNumber(grid, adjacentRow, adjacentColumn, offset[1]));
                if #numbers > 0 then
                    local num = tonumber(offset[1] == -1 and string.reverse(numbers) or numbers);
                    if num then table.insert(allNumbers, num) end
                end
            else
                --Horizontal search

                numbers = GetHorizontalNumbers(grid, adjacentRow, adjacentColumn)
                for _, num in ipairs(numbers) do
                    if num then table.insert(allNumbers, num) end
                end
            end
        end
    end

    return allNumbers;
end


function daySolution.GetSolution()
    local sol = solutionCreatorModule.CreateSolution();

    sol.Part1 = function(data)
        local sum = 0;
        local gridData = CreateGrid(data);
        for rowIndex, row in ipairs(gridData) do
            local valuesWithPos = GetValuesWithTheirPositions(row, "%d");
            local validNumbers = GetValidAdjacentNumbers(valuesWithPos, rowIndex, gridData);
            for i = 1, #validNumbers do
                sum = sum + validNumbers[i];
            end
        end
        return sum;
    end

    sol.Part2 = function(data)
        local sum = 0;
        local gridData = CreateGrid(data);
        for rowIndex, row in ipairs(gridData) do
            for charIndex, character in ipairs(row) do
                if IsGear(character) then
                    local gearNumbers = GetAllNumbersAround(gridData, rowIndex, charIndex)
                    if (#gearNumbers == 2) then
                        sum = sum + (gearNumbers[1] * gearNumbers[2]);
                    end
                end
            end
        end
        return sum;
    end

    return sol;
end

return daySolution;
