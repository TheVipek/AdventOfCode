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
        winningNumbers = {}
    };
    setmetatable(o, { __index = CardContainer });
    return o;
end

function CardContainer:InitializeWinningNumber(number)
    self.winningNumbers[number] = 0;
end

function CardContainer:AddWinningNumber(number)
    self.winningNumbers[number] = self.winningNumbers[number] + 1
end

function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)
        local allCards = {};
        local allCardInfo, specifiedCardInfo, winningNumbersCard;
        for line in string.gmatch(data, "[^\r\n]+") do
            allCardInfo = GetScratchCardLine(line);

            local card = CardContainer:New();

            specifiedCardInfo = allCardInfo[1];
            for i = 1, #specifiedCardInfo do
                card:InitializeWinningNumber(specifiedCardInfo[i]);
            end

            winningNumbersCard = allCardInfo[2];
            for i = 1, #winningNumbersCard do
                if (card.winningNumbers[winningNumbersCard[i]]) then
                    card:AddWinningNumber(winningNumbersCard[i]);
                end
            end
            allCards[#allCards + 1] = card;
        end

        local sum = 0;
        local winningNumbersCount;
        local winningNumbers;
        for i = 1, #allCards do
            winningNumbersCount = 0;
            winningNumbers = allCards[i].winningNumbers;

            for _, value in pairs(winningNumbers) do
                winningNumbersCount = winningNumbersCount + value;
            end

            if (winningNumbersCount > 0) then
                sum = sum + (2 ^ (winningNumbersCount - 1));
            end
        end

        return sum;
    end

    solution.Part2 = function(data)

    end

    return solution;
end

return daySolution;
