--[[
    Module for measuring method performance
]]
--
methodProfiler = {}

package.path = package.path .. ';lua_modules/share/lua/5.4/?.lua';
package.cpath = package.cpath .. ';lua_modules/lib/lua/5.4/?.dll';

local socket = require "socket";

--- func desc
---@param func method that is being called inside profiler
---@return execution time in ms
function methodProfiler.Mesaure(func)
    local startTime = socket.gettime() * 1000;
    print("Result:" .. func());
    return (socket.gettime() * 1000) - startTime;
end

return methodProfiler;
