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
            local shouldSwap = false;

            if hands[i + 1].handKind > hands[i].handKind then
                shouldSwap = true;
            elseif hands[i + 1].handKind == hands[i].handKind then
                for k = 1, HAND_SIZE do
                    local currElementStrength = CardStrength[string.sub(hands[i].handCards, k, k)];
                    local nextElementStrength = CardStrength[string.sub(hands[i + 1].handCards, k, k)];
                    if currElementStrength > nextElementStrength then
                        break;
                    elseif currElementStrength < nextElementStrength then
                        shouldSwap = true;
                        break;
                    end
                end
            end
            if shouldSwap then
                hands[i], hands[i + 1] = hands[i + 1], hands[i];
                swapped = true;
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
    if low < high then
        local pivotIdx = Partition(arr, low, high);
        SortByHandStrength_QuickSort(arr, low, pivotIdx - 1);
        SortByHandStrength_QuickSort(arr, pivotIdx + 1, high);
    end
end


local function merge(arr, left, mid, right) end
local function SortByHandStrength_MergeSort(arr, left, right)
    if left < right then
        local mid = math.floor((left + right) / 2);
        SortByHandStrength_MergeSort(arr, left, mid);
        SortByHandStrength_MergeSort(arr, mid + 1, right);
        merge(arr, left, mid, right);
    end
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
        SortByHandStrength_QuickSort(hands, 1, #hands);
        local sum = 0;
        for i = 1, #hands, 1 do
            -- sum = sum + hands[i].bidAmount * ((#hands - i) + 1);
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
