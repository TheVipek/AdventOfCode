package = "Solution"
version = "dev-1"
source = {
   url = "git+https://github.com/TheVipek/AdventOfCode.git"
}
description = {
   homepage = "https://github.com/TheVipek/AdventOfCode",
   license = "*** please specify a license ***"
}
dependencies = {
   "lua ~> 5.4"
}
build = {
   type = "builtin",
   modules = {
      ["Day1.Day1"] = "Day1/Day1.lua",
      ["Day2.Day2"] = "Day2/Day2.lua",
      ["Utilities.inputHandler"] = "Utilities/inputHandler.lua",
      ["Utilities.methodProfiler"] = "Utilities/methodProfiler.lua",
      ["Utilities.solutionCreator"] = "Utilities/solutionCreator.lua",
      ["Utilities.switch"] = "Utilities/switch.lua",
      main = "main.lua"
   }
}
