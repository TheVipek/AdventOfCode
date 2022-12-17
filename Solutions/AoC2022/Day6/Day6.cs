using System;
using System.Collections.Generic;
using System.Linq;
namespace AoC2022.Day6
{
	public class Day : BaseDay
	{
        public override string FirstPart()
        {
            string _inputContent = string.Join("", inputContent);
            int distinctCharacters = 4;
            for(int i =0;i<_inputContent.Length;i+=1)
            {
                IEnumerable<char> letters = _inputContent.Skip(i).Take(distinctCharacters).Distinct();
                if(letters.Count() == 4)
                {
                    int afterCharacter = i + distinctCharacters;
                    return $"After {afterCharacter} element";
                }
            }
            return string.Empty;

        }
        public override string SecondPart()
        {
            string _inputContent = string.Join("", inputContent);
            int distinctCharacters = 14;

            for (int i = 0; i < _inputContent.Length; i += 1)
            {
                IEnumerable<char> letters = _inputContent.Skip(i).Take(distinctCharacters).Distinct();
                if (letters.Count() == 14) 
                {
                    int afterCharacter = i + distinctCharacters;
                    return $"After {afterCharacter} element";
                
                } 
            }
            return string.Empty;
        }
    }
}
