using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GiddyUpCore
{
    public class AnimalRecord
    {
        public bool isSelected = false;
        public bool isException = false;
        public String label = "";
        public AnimalRecord(bool isSelected, bool isException, String label)
        {
            this.isException = isException;
            this.isSelected = isSelected;
            this.label = label;
        }
        public override string ToString()
        {
            return this.isSelected + "," + this.isException + "," + this.label;
        }
    }

}
