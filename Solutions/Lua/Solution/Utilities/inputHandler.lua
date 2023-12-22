--[[
    Module for reading data from file existing in current folder
]] --
inputHandler = {}

--- func desc
---@param fileName string
function inputHandler.GetDataFromPath(pathToFileFromCurrentPath)
    local filePath;

    local p = io.popen("cd")
    filePath = p:read("*a"):gsub("\n$", "") .. pathToFileFromCurrentPath;
    p:close()

    local file, data = io.open(filePath, "r");
    if file then
        data = file:read("*all");
        file:close();
    end
    return data;
end

return inputHandler;
