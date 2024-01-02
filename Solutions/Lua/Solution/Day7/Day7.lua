daySolution = {}
package.path = package.path .. ";../Utilities/?.lua"

local solutionCreatorModule = require "solutionCreator";

-- Combinations are amount of the same card needed in hand
local HandKindsCombinations <const> = {
    { name = "FiveOfAKind",  order = 6, combination = { 5 } },
    { name = "FourOfAKind",  order = 5, combination = { 4, 1 } },
    { name = "FullHouse",    order = 4, combination = { 3, 2 } },
    { name = "ThreeOfAKind", order = 3, combination = { 3, 1, 1 } },
    { name = "TwoPair",      order = 2, combination = { 2, 2, 1 } },
    { name = "OnePair",      order = 1, combination = { 2, 1, 1, 1 } },
    { name = "HighCard",     order = 0, combination = { 1, 1, 1, 1, 1 } }
}

local HAND_SIZE <const> = 5;

Hand = {}

---@param includeJokers boolean
function Hand:New(includeJokers)
    local handObj = {
        bidAmount = 0,
        handKind = -1,
        handCards = "",
        includeJokers = includeJokers,
    };

    local function EvaluateHandWithoutJokers(sortedArr)
        for _, comb in ipairs(HandKindsCombinations) do
            local correctCombination = false;
            if (#comb.combination == #sortedArr) then
                for idx, combinationVal in ipairs(comb.combination) do
                    if combinationVal == sortedArr[idx].count then
                        correctCombination = true;
                    else
                        correctCombination = false;
                        break;
                    end
                end
                if correctCombination then
                    handObj.handKind = comb.order;
                    return;
                end
            end
        end
    end

    local function EvaluateHandWithJokers(arrWithoutJokers, jokersCount)
        local jokersCount = jokersCount;
        local arrWithoutJokers = arrWithoutJokers;

        if jokersCount == HAND_SIZE then
            handObj.handKind = HandKindsCombinations[1].order;
            return;
        end

        for _, comb in ipairs(HandKindsCombinations) do
            local correctCombination = false;

            if #arrWithoutJokers == #comb.combination then
                for idx, combinationVal in ipairs(comb.combination) do
                    if combinationVal == (arrWithoutJokers[idx].count + jokersCount) then
                        correctCombination = true;
                    elseif combinationVal == arrWithoutJokers[idx].count then
                        correctCombination = true;
                    else
                        correctCombination = false;
                        break;
                    end
                end

                if correctCombination then
                    handObj.handKind = comb.order;
                    return;
                end
            end
        end
    end

    local function CalculateHandKind()
        local dict = {};

        for i = 1, #handObj.handCards do
            local c = handObj.handCards:sub(i, i);
            dict[c] = (dict[c] or 0) + 1;
        end
        local sortedArr = {};
        for k, v in pairs(dict) do
            sortedArr[#sortedArr + 1] = { key = k, count = v };
        end

        table.sort(sortedArr,
            function(a, b)
                return a.count > b.count
            end);

        if includeJokers then
            local jokersCount = 0;
            local arrWithoutJokers = {};
            for i = 1, #sortedArr do
                if sortedArr[i].key == "J" then
                    jokersCount = sortedArr[i].count;
                else
                    arrWithoutJokers[#arrWithoutJokers + 1] = { key = sortedArr[i].key, count = sortedArr[i].count };
                end
            end

            EvaluateHandWithJokers(arrWithoutJokers, jokersCount);
        else
            EvaluateHandWithoutJokers(sortedArr);
        end
    end

    handObj.CalculateHandKind = CalculateHandKind;

    setmetatable(handObj, { __index = Hand });

    return handObj;
end

function Hand:InitializeHand(handCards)
    self.handCards = handCards;
    self.CalculateHandKind();
end

function Hand:InitializeBid(bid)
    self.bidAmount = bid;
end

-- region Sorting Algorithms

---@param hands table
---@param cardsStrength table key-value
local function SortHandByStrength_BubbleSort(hands, cardsStrength)
    local n = #hands;
    local swapped;

    repeat
        swapped = false;
        for i = 1, n - 1 do
            local leftElement = hands[i];
            local rightElement = hands[i + 1];

            if leftElement.handKind > rightElement.handKind then
                hands[i], hands[i + 1] = hands[i + 1], hands[i];
                swapped = true;
            elseif leftElement.handKind == rightElement.handKind then
                for k = 1, HAND_SIZE do
                    local leftElementStrength = cardsStrength[string.sub(leftElement.handCards, k, k)];
                    local rightElementStrength = cardsStrength[string.sub(rightElement.handCards, k, k)];
                    if leftElementStrength > rightElementStrength then
                        hands[i], hands[i + 1] = hands[i + 1], hands[i];
                        swapped = true;
                        break;
                    elseif leftElementStrength < rightElementStrength then
                        break;
                    end
                end
            end
        end

        n = n - 1;
    until not swapped
end

---@param cardsStrength table key-value
local function Partition(arr, low, high, cardsStrength)
    local pivot = arr[high];
    local i = low - 1;
    local shouldSwap;
    for j = low, high - 1 do
        shouldSwap = false;
        if arr[j].handKind < pivot.handKind then
            shouldSwap = true;
        elseif arr[j].handKind == pivot.handKind then
            for k = 1, HAND_SIZE do
                local pivotStrength = cardsStrength[string.sub(pivot.handCards, k, k)];
                local elementStrength = cardsStrength[string.sub(arr[j].handCards, k, k)];
                if pivotStrength < elementStrength then
                    break;
                elseif pivotStrength > elementStrength then
                    shouldSwap = true;
                    break;
                end
            end
        end
        if shouldSwap then
            i = i + 1;
            arr[i], arr[j] = arr[j], arr[i];
        end
    end
    arr[i + 1], arr[high] = arr[high], arr[i + 1];
    return i + 1;
end
---@param cardsStrength table key-value
local function SortByHandStrength_QuickSort(arr, low, high, cardsStrength)
    if high <= low then return end;

    local pivotIdx = Partition(arr, low, high, cardsStrength);
    SortByHandStrength_QuickSort(arr, low, pivotIdx - 1, cardsStrength);
    SortByHandStrength_QuickSort(arr, pivotIdx + 1, high, cardsStrength);
end

---@param cardsStrength table key-value
local function Merge(leftArr, rightArr, arr, cardsStrength)
    local leftIndex, rightIndex, arrIndex = 1, 1, 1;
    local leftSize = math.floor(#arr / 2);
    local rightSize = #arr - leftSize;
    while leftIndex <= leftSize and rightIndex <= rightSize do
        local leftElement = leftArr[leftIndex];
        local rightElement = rightArr[rightIndex];

        if (leftElement.handKind > rightElement.handKind) then
            arr[arrIndex] = rightArr[rightIndex];
            rightIndex = rightIndex + 1;
            arrIndex = arrIndex + 1;
        elseif (leftElement.handKind == rightElement.handKind) then
            for k = 1, HAND_SIZE do
                local leftStrength = cardsStrength[string.sub(leftElement.handCards, k, k)];
                local rightStrength = cardsStrength[string.sub(rightElement.handCards, k, k)];
                if leftStrength < rightStrength then
                    arr[arrIndex] = leftArr[leftIndex];
                    leftIndex = leftIndex + 1;
                    arrIndex = arrIndex + 1;
                    break;
                elseif leftStrength > rightStrength then
                    arr[arrIndex] = rightArr[rightIndex];
                    rightIndex = rightIndex + 1;
                    arrIndex = arrIndex + 1;
                    break;
                end
            end
        else
            arr[arrIndex] = leftArr[leftIndex];
            leftIndex = leftIndex + 1;
            arrIndex = arrIndex + 1;
        end
    end

    while leftIndex <= #leftArr do
        arr[arrIndex] = leftArr[leftIndex];

        leftIndex = leftIndex + 1;
        arrIndex = arrIndex + 1;
    end

    while rightIndex <= #rightArr do
        arr[arrIndex] = rightArr[rightIndex];

        rightIndex = rightIndex + 1;
        arrIndex = arrIndex + 1;
    end
end

---@param cardsStrength table key-value
local function SortByHandStrength_MergeSort(arr, cardsStrength)
    local len = #arr;
    if len <= 1 then
        return;
    end

    local lArr = {};
    local rArr = {};
    local mid = math.floor(len / 2);
    for i = 1, len do
        if i <= mid then
            lArr[#lArr + 1] = arr[i];
        else
            rArr[#rArr + 1] = arr[i];
        end
    end
    SortByHandStrength_MergeSort(lArr, cardsStrength);
    SortByHandStrength_MergeSort(rArr, cardsStrength);

    Merge(lArr, rArr, arr, cardsStrength);
end

-- endregion

function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)
        local hands = {};

        for line in string.gmatch(data, "[^\r\n]+") do
            local spaceIdx = string.find(line, "%s");
            local parts = {
                string.sub(line, 1, spaceIdx - 1),
                tonumber(string.sub(line, spaceIdx + 1, #line)),
            }

            local hand = Hand:New(false);
            hand:InitializeHand(parts[1]);
            hand:InitializeBid(parts[2]);
            hands[#hands + 1] = hand;
        end

        --SortHandByStrength_BubbleSort(hands, CardsStrengthPT1);
        --SortByHandStrength_QuickSort(hands, 1, #hands, CardsStrengthPT1);
        SortByHandStrength_MergeSort(hands,
            {
                ['A'] = 13,
                ['K'] = 12,
                ['Q'] = 11,
                ['J'] = 10,
                ['T'] = 9,
                ['9'] = 8,
                ['8'] = 7,
                ['7'] = 6,
                ['6'] = 5,
                ['5'] = 4,
                ['4'] = 3,
                ['3'] = 2,
                ['2'] = 1
            });

        local sum = 0;
        for i = 1, #hands, 1 do
            sum = sum + hands[i].bidAmount * i;
        end

        return sum;
    end
    solution.Part2 = function(data)
        local hands = {};

        for line in string.gmatch(data, "[^\r\n]+") do
            local spaceIdx = string.find(line, "%s");
            local parts = {
                string.sub(line, 1, spaceIdx - 1),
                tonumber(string.sub(line, spaceIdx + 1, #line)),
            }

            local hand = Hand:New(true);
            hand:InitializeHand(parts[1]);
            hand:InitializeBid(parts[2]);
            hands[#hands + 1] = hand;
        end

        --SortHandByStrength_BubbleSort(hands, CardsStrengthPT2);
        --SortByHandStrength_QuickSort(hands, 1, #hands, CardsStrengthPT2);
        SortByHandStrength_MergeSort(hands,
            {
                ['A'] = 13,
                ['K'] = 12,
                ['Q'] = 11,
                ['T'] = 10,
                ['9'] = 9,
                ['8'] = 8,
                ['7'] = 7,
                ['6'] = 6,
                ['5'] = 5,
                ['4'] = 4,
                ['3'] = 3,
                ['2'] = 2,
                ['J'] = 1
            });

        local sum = 0;
        for i = 1, #hands, 1 do
            sum = sum + hands[i].bidAmount * i;
        end

        return sum;
    end

    return solution;
end

return daySolution;
