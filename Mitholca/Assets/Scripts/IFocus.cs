using UnityEngine;

namespace Mitholca
{
    public interface IFocus 
    {
        bool HasFocus();
        void Request(bool hasFocus);
    }
}