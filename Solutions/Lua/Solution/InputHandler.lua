inputHandler = {}

--- func desc
---@param fileName string
function inputHandler.GetInputFromCurrentFolder(fileName)
    local filePath;

    local p = io.popen("cd")
    filePath = p:read("*a"):gsub("\n$", "") .. "\\" .. fileName;
    p:close()

    local file, data = io.open(filePath, "r");
    if file then
        data = file:read("*all");
        file:close();
    end
    return data;
end

return inputHandler;
