using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSpec;


namespace KaleidoscopeGenerator.Data.Test
{
    class kaleidoscope_spec : nspec
    {
        void before_each() { name = "NSpec"; }

        void it_works()
        {
            name.should_be("NSpec");
        }

        void describe_nesting()
        {
            before = () => name += " BDD";

            it["works here"] = () => name.should_be("NSpec BDD");

            it["and here"] = () => name.should_be("NSpec BDD");
        }
        string name;
    }
}
