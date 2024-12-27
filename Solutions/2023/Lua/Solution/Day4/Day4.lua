daySolution = {}

package.path = package.path .. ";../Utilities/?.lua"
local solutionCreatorModule = require "solutionCreator";

local function GetScratchCardLine(inputStr)
    -- Remove the initial text up to and including ": "
    local numbersOnly = inputStr:match(": (.+)$");

    local cardAndWinningNumbersSides = {};

    for str in string.gmatch(numbersOnly, "([^" .. "|" .. "]+)") do
        local numbers = {};
        for number in string.gmatch(str, "%S+") do
            table.insert(numbers, number);
        end
        cardAndWinningNumbersSides[#cardAndWinningNumbersSides + 1] = numbers;
    end
    return cardAndWinningNumbersSides;
end
local function ShowCardsData(allCards)
    for _, card in ipairs(allCards) do
        print("CARD: " .. _)
        for key, value in pairs(card.winningNumbers) do
            print(key, value);
        end
    end
end

-- Card Class Related
CardContainer = {}

function CardContainer:New()
    local o = {
        winningNumbers = {},
        winningNumbersCount = 0
    };
    setmetatable(o, { __index = CardContainer });
    return o;
end

function CardContainer:InitializeWinningNumber(number)
    self.winningNumbers[number] = 0;
end

function CardContainer:AddWinningNumber(number)
    self.winningNumbers[number] = self.winningNumbers[number] + 1
    self.winningNumbersCount = self.winningNumbersCount + 1;
end

local function SetupSpecifiedCardInfo(cardInstance, dataForCard)
    for i = 1, #dataForCard do
        cardInstance:InitializeWinningNumber(dataForCard[i]);
    end
end
local function AddWiningNumbersToCardInstance(cardInstance, winningNumbersData)
    for i = 1, #winningNumbersData do
        if (cardInstance.winningNumbers[winningNumbersData[i]]) then
            cardInstance:AddWinningNumber(winningNumbersData[i]);
        end
    end
end
local function SumCardWinningNumbers(allCards)
    local sum = 0;
    local winningNumbersCount;
    for i = 1, #allCards do
        winningNumbersCount = allCards[i].winningNumbersCount;

        if (winningNumbersCount > 0) then
            sum = sum + (2 ^ (winningNumbersCount - 1));
        end
    end
    return sum;
end
local function GetTotalCardsInstances(allCards)
    local sum = 0;
    local totalCardInstances = {};
    for cardIdx = 1, #allCards do
        local winningNumbers = allCards[cardIdx].winningNumbers;

        totalCardInstances[cardIdx] = (totalCardInstances[cardIdx] or 0) + 1;
        local i = 1;
        for _, value in pairs(winningNumbers) do
            if (value > 0) then
                repeat
                    local targetIdx = cardIdx + i;
                    totalCardInstances[targetIdx] = (totalCardInstances[targetIdx] or 0) +
                        (1 * totalCardInstances[cardIdx]);
                    i = i + 1;
                until i >= value
            end
        end
        sum = sum + totalCardInstances[cardIdx];
    end
    return sum;
end

function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)
        local allCards = {};
        local cardInfo;
        for line in string.gmatch(data, "[^\r\n]+") do
            cardInfo = GetScratchCardLine(line);

            local card = CardContainer:New();

            SetupSpecifiedCardInfo(card, cardInfo[1]);

            AddWiningNumbersToCardInstance(card, cardInfo[2]);

            allCards[#allCards + 1] = card;
        end

        return SumCardWinningNumbers(allCards);
    end

    solution.Part2 = function(data)
        local allCards = {};
        local cardInfo;
        for line in string.gmatch(data, "[^\r\n]+") do
            cardInfo = GetScratchCardLine(line);

            local card = CardContainer:New();

            SetupSpecifiedCardInfo(card, cardInfo[1]);

            AddWiningNumbersToCardInstance(card, cardInfo[2]);

            allCards[#allCards + 1] = card;
        end

        return GetTotalCardsInstances(allCards);
    end

    return solution;
end

return daySolution;
