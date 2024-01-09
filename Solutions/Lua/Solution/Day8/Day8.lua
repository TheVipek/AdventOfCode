daySolution = {}

local graphModule = require "Graph";

package.path = package.path .. ";../Utilities/?.lua"

local solutionCreatorModule = require "solutionCreator";

function daySolution.GetSolution()
    local solution = solutionCreatorModule.CreateSolution();

    solution.Part1 = function(data)
        local afterInstructions = false;
        local instructions = {};
        local nodes = {};
        for line in string.gmatch(data, "([^\r\n]*)") do
            if line == "" then
                afterInstructions = true;
                goto continue
            end

            if afterInstructions then
                local nodeAndEdges = {};
                for s in string.gmatch(line, "%a+") do
                    table.insert(nodeAndEdges, s);
                end

                nodes[nodeAndEdges[1]] = { source = nodeAndEdges[1], edges = { nodeAndEdges[2], nodeAndEdges[3] } };
            else
                for s in string.gmatch(line, "%a") do
                    if s == "L" then
                        table.insert(instructions, 1);
                    else
                        table.insert(instructions, 2);
                    end
                end
            end
            ::continue::
        end

        local graph = Graph:New(nodes, instructions);
        return graph.MoveAccordingToInstructions("AAA", "ZZZ");
    end
    solution.Part2 = function(data)
    end
    return solution;
end

return daySolution;
