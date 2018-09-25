using Newtonsoft.Json.Linq;
using System.IO;

namespace GGO.Common
{
    public class Names
    {
        /// <summary>
        /// The Raw name list.
        /// </summary>
        private JObject NameList;

        public Names(string Location)
        {
            string Content = File.ReadAllText(Location);
            NameList = JObject.Parse(Content);
        }

        /// <summary>
        /// Gets the name of a ped from GGO.Names.json
        /// </summary>
        /// <param name="Hash">The Ped hash.</param>
        /// <returns>The name of the ped, or "Unknown" if is not defined.</returns>
        public string GetName(int Hash)
        {
            if (!IsNameDefined(Hash))
            {
                return "Unknown";
            }
            else
            {
                return (string)NameList[Hash.ToString()];
            }
        }

        /// <summary>
        /// Checks if a ped hash exists on the name list.
        /// </summary>
        /// <param name="Hash">The Ped hash.</param>
        /// <returns>True if the ped has a name defined, false otherwise.</returns>
        public bool IsNameDefined(int Hash)
        {
            return NameList.ContainsKey(Hash.ToString());
        }
    }
}
