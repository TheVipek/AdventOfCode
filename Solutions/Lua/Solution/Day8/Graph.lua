Graph = {}

function Graph:New(nodes, instructions)
    local graphObj = {
        graph = {},
        instructions = {}
    };

    graphObj.graph = nodes;
    graphObj.instructions = instructions;
    local function MoveAccordingToInstructions(node, targetNode)
        local steps = 0;
        local index = 1;
        while node ~= targetNode do
            steps = steps + 1;
            node = graphObj.graph[node].edges[graphObj.instructions[index]];
            index = (index % #graphObj.instructions) + 1;
        end

        return steps;
    end
    graphObj.MoveAccordingToInstructions = MoveAccordingToInstructions;

    return graphObj;
end

return Graph;
