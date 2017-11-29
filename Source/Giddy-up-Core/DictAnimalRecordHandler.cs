using GiddyUpCore.Utilities;
using HugsLib.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiddyUpCore
{
    public class DictAnimalRecordHandler : SettingHandleConvertible
    {
        public Dictionary<String, AnimalRecord> inner = new Dictionary<String, AnimalRecord>();
        public Dictionary<String, AnimalRecord> InnerList { get { return inner; } set { inner = value; } }

        public override void FromString(string settingValue)
        {
            inner = new Dictionary<String, AnimalRecord>();
            if (!settingValue.Equals(string.Empty))
            {
                foreach (string str in settingValue.Split('|'))
                {
                    inner.Add(str.Split(',')[0], new AnimalRecord(Convert.ToBoolean(str.Split(',')[1]), Convert.ToBoolean(str.Split(',')[2])));
                }
            }
        }

        public override string ToString()
        {
            List<String> strings = new List<string>();
            foreach (KeyValuePair<string, AnimalRecord> item in inner)
            {
                strings.Add(item.Key +","+item.Value.ToString());
            }

            return inner != null ? String.Join("|", strings.ToArray()) : "";
        }
    }

}
