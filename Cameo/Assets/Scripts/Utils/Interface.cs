using Cameo.Mono;
using System;

namespace Cameo.Utils
{
    public interface I_Power
    {
        bool hasPower();
        void usePower(Player player);
    }

    public interface I_CustomAction
    {
        void ExecuteAction();
    }

}
