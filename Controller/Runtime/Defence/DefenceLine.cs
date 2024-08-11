using System.Collections.Generic;
using Alchemy.Inspector;
using Pancake;

namespace _Root.Scripts.Controller.Runtime.Defence
{
    public class DefenceLine : GameComponent
    {
        public GuardCharacter guardCharacterPrefab;
        public List<DefenceLineSegment> segments = new();

        [Button]
        public void RequestGuard(int lineIndex)
        {
            guardCharacterPrefab.Request(lineIndex, segments[lineIndex], this);
        }
        
        public DefenceLineSegment GetLine(int index)
        {
            return segments[index];
        }
    }
}