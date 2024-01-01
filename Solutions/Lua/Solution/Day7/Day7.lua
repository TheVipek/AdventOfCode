daySolution = {}
package.path = package.path .. ";../Utilities/?.lua"

local solutionCreatorModule = require "solutionCreator";

-- Combinations are amount of the same card needed in hand
local HandKindsCombinations <const> = {
    FiveOfAKind = { order = 6, combination = { 5 } },
    FourOfAKind = { order = 5, combination = { 4, 1 } },
    FullHouse = { order = 4, combination = { 3, 2 } },
    ThreeOfAKind = { order = 3, combination = { 3, 1, 1 } },
    TwoPair = { order = 2, combination = { 2, 2, 1 } },
    OnePair = { order = 1, combination = { 2, 1, 1, 1 } },
    HighCard = { order = 0, combination = { 1, 1, 1, 1, 1 } }
}
local CardStrength <const> =
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
};
local HAND_SIZE <const> = 5;

Hand = {}

function Hand:New()
    local o = {
        bidAmount = 0,
        handKind = -1,
        handCards = "",
    };

    local function CalculateHandKind()
        local dict = {};
        for i = 1, #o.handCards do
            local c = o.handCards:sub(i, i);
            dict[c] = (dict[c] or 0) + 1;
        end


        local sortedArr = {};
        for k, v in pairs(dict) do
            sortedArr[#sortedArr + 1] = { key = k, count = v };
        end
        table.sort(sortedArr, function(a, b) return a.count > b.count end);

        for _, comb in pairs(HandKindsCombinations) do
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
                    o.handKind = comb.order;
                    return;
                end
            end
        end
    end

    o.DetermineHandKind = CalculateHandKind;

    setmetatable(o, { __index = Hand });

    return o;
end

function Hand:InitializeHand(handCards)
    self.handCards = handCards;
    self.DetermineHandKind();
end

function Hand:InitializeBid(bid)
    self.bidAmount = bid;
end

---@param hands table
local function SortHandByStrength_BubbleSort(hands)
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
                    local leftElementStrength = CardStrength[string.sub(leftElement.handCards, k, k)];
                    local rightElementStrength = CardStrength[string.sub(rightElement.handCards, k, k)];
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
local function Partition(arr, low, high)
    local pivot = arr[high];
    local i = low - 1;
    local shouldSwap;
    for j = low, high - 1 do
        shouldSwap = false;
        if arr[j].handKind < pivot.handKind then
            shouldSwap = true;
        elseif arr[j].handKind == pivot.handKind then
            for k = 1, HAND_SIZE do
                local pivotStrength = CardStrength[string.sub(pivot.handCards, k, k)];
                local elementStrength = CardStrength[string.sub(arr[j].handCards, k, k)];
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
local function SortByHandStrength_QuickSort(arr, low, high)
    if high <= low then return end;

    local pivotIdx = Partition(arr, low, high);
    SortByHandStrength_QuickSort(arr, low, pivotIdx - 1);
    SortByHandStrength_QuickSort(arr, pivotIdx + 1, high);
end

local function Merge(leftArr, rightArr, arr)
    local leftIndex, rightIndex, arrIndex = 1, 1, 1;
    local leftSize = math.floor(#arr / 2);
    local rightSize = #arr - leftSize;
    while leftIndex <= leftSize and rightIndex <= rightSize do
        local leftElement = leftArr[leftIndex];
        local rightElement = rightArr[rightIndex];

        if (leftElement.handKind < rightElement.handKind) then
            arr[arrIndex] = leftArr[leftIndex];
            leftIndex = leftIndex + 1;
            arrIndex = arrIndex + 1;
        elseif (leftElement.handKind == rightElement.handKind) then
            for k = 1, HAND_SIZE do
                local leftStrength = CardStrength[string.sub(leftElement.handCards, k, k)];
                local rightStrength = CardStrength[string.sub(rightElement.handCards, k, k)];
                if leftStrength < rightStrength then
                    arr[arrIndex] = leftArr[leftIndex];
                    leftIndex = leftIndex + 1;
                    arrIndex = arrIndex + 1;
                    break;
                elseif leftStrength >= rightStrength then
                    arr[arrIndex] = rightArr[rightIndex];
                    rightIndex = rightIndex + 1;
                    arrIndex = arrIndex + 1;
                    break;
                end
            end
        else
            arr[arrIndex] = rightArr[rightIndex];
            rightIndex = rightIndex + 1;
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

local function SortByHandStrength_MergeSort(arr)
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
    SortByHandStrength_MergeSort(lArr);
    SortByHandStrength_MergeSort(rArr);

    Merge(lArr, rArr, arr);
end



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

            local hand = Hand:New();
            hand:InitializeHand(parts[1]);
            hand:InitializeBid(parts[2]);
            hands[#hands + 1] = hand;
        end

        --SortHandByStrength_BubbleSort(hands);
        --SortByHandStrength_QuickSort(hands, 1, #hands);
        SortByHandStrength_MergeSort(hands);
        local sum = 0;
        for i = 1, #hands, 1 do
            --sum = sum + hands[i].bidAmount * ((#hands - i) + 1);
            sum = sum + hands[i].bidAmount * i;
        end

        return sum;
    end
    solution.Part2 = function(data)
        return 0;
    end

    return solution;
end

return daySolution;
