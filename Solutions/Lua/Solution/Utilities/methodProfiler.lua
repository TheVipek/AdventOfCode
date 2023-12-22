--[[
    Module for measuring method performance
]] --
methodProfiler = {}

--- func desc
---@param func method that is being called inside profiler
---@return execution time in ms 
function methodProfiler.Mesaure(func)

    -- do it suing LuaSOCKET later
    local startTime = os.clock();
    func();
    local elapsedTime
    return (os.clock() - startTime) * 1000;
end

return methodProfiler;
