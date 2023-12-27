daySolution = {}

package.path = package.path .. ";../Utilities/?.lua"
local solutionCreatorModule = require "solutionCreator";

function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)

    end

    solution.Part2 = function(data)

    end

    return solution;
end

return daySolution;
