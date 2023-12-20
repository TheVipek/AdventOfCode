switch = {}

function switch.switch(cases)
    return function(case)
        if cases[case] then
            return cases[case]();
        else
            return nil;
        end
    end
end

return switch;
