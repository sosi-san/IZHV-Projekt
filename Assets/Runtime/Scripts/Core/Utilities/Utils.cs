using Random = UnityEngine.Random;

namespace Woska.Core
{
    public static class Utils
    {
        const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";
    
        public static string RandomString(int stringLength)
        {
            string roomName = "";
            for(int i=0; i<stringLength; i++)
            {
                roomName += glyphs[Random.Range(0, glyphs.Length)];
            }

            return roomName;
        }
    }
}