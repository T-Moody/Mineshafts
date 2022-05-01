using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mineshafts.Configuration
{
    public class LocalizationConfig
    {
        public string entrance_name { get; set; } = "Mineshaft entrance";

        public void InsertLocalization()
        {
            if (Main.localizationInstance == null) return;

            Main.localizationInstance.m_translations.Remove("MS_entrance");
            Main.localizationInstance.m_translations.Add("MS_entrance", entrance_name);
        }
    }
}
