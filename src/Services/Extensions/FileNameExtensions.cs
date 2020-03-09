using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Extensions
{
    public static class FileNameExtensions
    {
        public static string RemoveSpaces(this string stringToRemoveSpacesFrom)
        {
            return stringToRemoveSpacesFrom.Replace(" ", string.Empty);
        }
    }
}